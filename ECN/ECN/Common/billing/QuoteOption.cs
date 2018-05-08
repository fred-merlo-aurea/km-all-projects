using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ecn.common.classes.license;

namespace ecn.common.classes.billing
{	
	/// For all the Enum Type, its int value is tied to database table.
	/// PLEASE DO NOT CHANGE THEIR ORDER OR VALUE. You can only APPEND to it. 
	public enum LicenseTypeEnum { AnnualTechAccess, EmailBlock,Option}
	
	[Flags]
	public enum FrequencyEnum { 
		Annual = 1, 
		Quarterly = 2,
		Monthly = 4,
		OneTime = 8,
		BiWeekly = 16,
		Weekly = 32
	}
	
	public enum PriceTypeEnum { Recurring =1, Usage = 2, OneTime =3}

	public class QuoteOption : QuoteItemBase {
		public QuoteOption(string code, string name, string description, LicenseTypeEnum licenseType, int quantity, double rate, PriceTypeEnum priceType, FrequencyEnum allowedFrequency) :
			this(code, name, description, licenseType, quantity, rate, priceType, allowedFrequency, false) {
		}
		public QuoteOption(string code, string name, string description, LicenseTypeEnum licenseType, int quantity, double rate, PriceTypeEnum priceType, FrequencyEnum allowedFrequency, bool isCustomerCredit) :
			base(code, name, description, quantity, rate, licenseType, priceType) {		
			AllowedFrequency = allowedFrequency;
			IsCustomerCredit = isCustomerCredit;
		}	
		
		#region Properties
		private int _baseChannelID;
		public int BaseChannelID {
			get {
				return (this._baseChannelID);
			}
			set {
				this._baseChannelID = value;
			}
		}

		private FrequencyEnum _allowedFrequency;
		public FrequencyEnum AllowedFrequency {
			get {
				return (this._allowedFrequency);
			}
			set {
				this._allowedFrequency = value;
			}
		}		
	
		#endregion

		public override bool Equals(object obj) {
			QuoteOption item = obj as QuoteOption;
			if (item == null) {
				return base.Equals (obj);
			}

			return item.Code == this.Code &&
				item.Name == this.Name &&
				item.Description == this.Description &&
				item.LicenseType == this.LicenseType &&
				item.Quantity == this.Quantity &&
				item.ItemPrice == this.ItemPrice &&
				item.PriceType == this.PriceType &&
				item.AllowedFrequency == this.AllowedFrequency;					
		}

		public override int GetHashCode() {
			return this.Name.GetHashCode() ^ this.Code.GetHashCode();
		}

		public string AllowedFrequencyNames {
			get {
				StringBuilder names = new StringBuilder();
				AddFrequencyName(names, FrequencyEnum.Annual, "Annual");
				AddFrequencyName(names, FrequencyEnum.Quarterly, "Quarterly");
				AddFrequencyName(names, FrequencyEnum.Monthly, "Monthly");
				AddFrequencyName(names, FrequencyEnum.OneTime, "OneTime");
				AddFrequencyName(names, FrequencyEnum.BiWeekly, "Every Two Week");
				return names.ToString();
			}
		}

		public bool IsAllowed(FrequencyEnum frequency) {
			return (frequency & AllowedFrequency) != 0;
		}

		private void AddFrequencyName(StringBuilder names, FrequencyEnum frequency, string name) {
			if (IsAllowed(frequency)) {
				if (names.Length > 0) {
					names.Append("/");
				}
				names.Append(name);
			}
		}

		#region Database Methods
		public void Save(SqlConnection connection, int userID) {
			if (ID == -1) {
				string insertSql = @"INSERT INTO [QuoteOption] (BaseChannelID, CreatedUserID, CreatedDate, IsDeleted, Code, Name, Description, Quantity, Rate, AllowedFrequency, LicenseType, PriceType, IsCustomerCredit, ProductIDs, ProductFeatureIDs, Services) VALUES
(@baseChannelID, @UserID, GetDate(), 0, @code, @name, @description, @quantity, @rate, @allowedFrequency, @licenseType, @priceType, @isCustomerCredit, @productIDs, @productFeatureIDs, @services);
SELECT @@IDENTITY;";
				SqlCommand insert = new SqlCommand(insertSql, connection);	
				insert.Parameters.Add("@baseChannelID",SqlDbType.Int, 4).Value = BaseChannelID;
                insert.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = userID;
				insert.Parameters.Add("@code", SqlDbType.VarChar,50).Value = Code;
				insert.Parameters.Add("@name", SqlDbType.VarChar,100).Value = Name;
				insert.Parameters.Add("@description", SqlDbType.VarChar,500).Value = Description;
				insert.Parameters.Add("@quantity", SqlDbType.Int,4).Value = Quantity;
				insert.Parameters.Add("@rate", SqlDbType.Float,8).Value = Rate;
				insert.Parameters.Add("@allowedFrequency", SqlDbType.SmallInt,2).Value = AllowedFrequency;
				insert.Parameters.Add("@licenseType", SqlDbType.SmallInt,2).Value = LicenseType;
				insert.Parameters.Add("@priceType", SqlDbType.SmallInt,2).Value = PriceType;
				insert.Parameters.Add("@isCustomerCredit", SqlDbType.Bit, 1).Value = IsCustomerCredit;
				insert.Parameters.Add("@productIDs", SqlDbType.VarChar, 47).Value = ProductIDs;
				insert.Parameters.Add("@productFeatureIDs", SqlDbType.VarChar, 399).Value = ProductFeatureIDs;
				insert.Parameters.Add("@Services", SqlDbType.VarChar, 1000).Value = Service.SerializeServices(Services);
				insert.Prepare();
				
				ID = Convert.ToInt32(insert.ExecuteScalar());
				return;
			}

			string updateSql = string.Format(@"UPDATE [QuoteOption] SET Code = @code, UpdatedUserID=@UserID, UpdatedDate=GetDate(), Name = @name, Description = @description, Quantity = @quantity, Rate = @rate,
					AllowedFrequency = @allowedFrequency, LicenseType = @licenseType, PriceType = @priceType, isCustomerCredit = @isCustomerCredit, Services = @services 
					{1} {2} WHERE QuoteOptionID = {0}", 
				ID, 
				needToSaveProductIDs?",ProductIDs = @productIDs" :"", 
				needToSaveProductFeatureIDs?",ProductFeatureIDs = @productFeatureIDs" : "");
			SqlCommand update = new SqlCommand(updateSql, connection);
			
			update.Parameters.Add("@code", SqlDbType.VarChar,50).Value = Code;
            update.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = userID;
			update.Parameters.Add("@name", SqlDbType.VarChar,100).Value = Name;
			update.Parameters.Add("@description", SqlDbType.VarChar,500).Value = Description;
			update.Parameters.Add("@quantity", SqlDbType.Int,4).Value = Quantity;
			update.Parameters.Add("@rate", SqlDbType.Float,8).Value = Rate;
			update.Parameters.Add("@allowedFrequency", SqlDbType.SmallInt,2).Value = AllowedFrequency;
			update.Parameters.Add("@licenseType", SqlDbType.SmallInt,2).Value = LicenseType;
			update.Parameters.Add("@priceType", SqlDbType.SmallInt,2).Value = PriceType;
			update.Parameters.Add("@isCustomerCredit", SqlDbType.Bit, 1).Value = IsCustomerCredit;
			update.Parameters.Add("@Services", SqlDbType.VarChar, 1000).Value = Service.SerializeServices(Services);
			if (needToSaveProductIDs) {
				update.Parameters.Add("@productIDs", SqlDbType.VarChar, 47).Value = ProductIDs;
			}
			if (needToSaveProductFeatureIDs) {
				update.Parameters.Add("@productFeatureIDs", SqlDbType.VarChar, 399).Value = ProductFeatureIDs;
			}
			
			update.Prepare();
			update.ExecuteNonQuery();
		}

		public void Save(int userID) {
			SqlConnection conn = new SqlConnection(DataFunctions.GetConnectionString());
			conn.Open();
			try {				
				this.Save(conn, userID);				
			} finally {
				conn.Close();
				conn.Dispose();
			}
		}


		public void Delete(int userID) 
        {
			if (ID > 0) 
            {
				//DataFunctions.Execute("DELETE from QuoteOptions where QuoteOptionID = " + this.ID.ToString());
                DataFunctions.Execute("UPDATE [QuoteOption] set IsDeleted = 1, UpdatedDate=GETDATE(), UpdatedUserID=" + userID + " where QuoteOptionID = " + this.ID.ToString());
			}				
		}
		#endregion

		#region Static Database Methods
		public static QuoteOptionCollection GetServiceLevelQuoteOptions(int channelID) {
			return GetQuoteOptionsByLicenseType(channelID, LicenseTypeEnum.AnnualTechAccess);
		}
		
		public static QuoteOptionCollection GetEmailUsageQuoteOptions(int channelID) {			
			return GetQuoteOptionsByLicenseType(channelID, LicenseTypeEnum.EmailBlock);;		
		}
				
		public static QuoteOptionCollection GetOptionQuoteOptions(int channelID) {		
			return GetQuoteOptionsByLicenseType(channelID, LicenseTypeEnum.Option);;		
		}

		private static QuoteOptionCollection GetQuoteOptionsByLicenseType(int channelID, LicenseTypeEnum licenseType) {
            DataTable dt = DataFunctions.GetDataTable(string.Format("SELECT * FROM [QuoteOption] where IsDeleted = 0 and BaseChannelID = {0} and LicenseType = {1}", channelID, (int)licenseType));
			QuoteOptionCollection options = new QuoteOptionCollection();
			foreach(DataRow row in dt.Rows) {
				options.Add(GetQuoteOption(row));
			}
			return options;
		}

		private static QuoteOption GetQuoteOption(DataRow row) {
			QuoteOption option = new QuoteOption(Convert.ToString(row["Code"]), Convert.ToString(row["Name"]),Convert.ToString(row["Description"]), 
				(LicenseTypeEnum) Convert.ToInt16(row["LicenseType"]), Convert.ToInt32(row["Quantity"]), 
				Convert.ToDouble(row["Rate"]), (PriceTypeEnum) Convert.ToInt16(row["PriceType"]), (FrequencyEnum) Convert.ToInt16(row["AllowedFrequency"]),
				Convert.ToBoolean(row["IsCustomerCredit"]));
			option.BaseChannelID = Convert.ToInt32(row["BaseChannelID"]);
			option.ID = Convert.ToInt32(row["QuoteOptionID"]);
            //LoadPlatformServices(option, "");
			//LoadProducts(option, Convert.ToString(row["ProductIDs"]));
			//LoadProductFeatures(option, Convert.ToString(row["ProductFeatureIDs"]));
			LoadServices(option, Convert.ToString(row["Services"]));
			return option;
		}
		#endregion
	}

	public class QuoteOptionCollection : CollectionBase {
		public void Add(QuoteOption option) {
			base.InnerList.Add(option);
		}

		public QuoteOption this[int index] {
			get { return (QuoteOption) base.InnerList[index];}
		}

		public QuoteOption FindByCode(string code) {
			foreach(QuoteOption option in base.InnerList) {
				if (option.Code == code) {
					return option;
				}
			}
			return null;
		}
	}
}
