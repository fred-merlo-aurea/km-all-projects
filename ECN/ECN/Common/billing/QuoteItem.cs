using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using ecn.common.classes.license;

namespace ecn.common.classes.billing
{
	public class QuoteItem : QuoteItemBase {

		public QuoteItem(FrequencyEnum frequency, string code, string name, string description, long quantity, double rate, LicenseTypeEnum licenseType, PriceTypeEnum priceType):
			this(frequency, code, name, description, quantity, rate, licenseType, priceType, false) {			
		}

		public QuoteItem(FrequencyEnum frequency, string code, string name, string description, long quantity, double rate, LicenseTypeEnum licenseType, PriceTypeEnum priceType, bool isCustomerCredit) : 
			base(code, name, description, quantity, rate, licenseType, priceType){			
			Frequency = frequency;
			IsCustomerCredit = isCustomerCredit;
		}


		#region Properties
		private Quote _parent;
		public Quote Parent {
			get {
				return (this._parent);
			}
			set {
				this._parent = value;
			}
		}


		public string QuoteCode {
			get { return Parent.QuoteCode;}
		}

		private FrequencyEnum _frequency;
		public FrequencyEnum Frequency {
			get {
				return (this._frequency);
			}
			set {				
				this._frequency = value;
			}
		}

		private double _discountRate;
		public double DiscountRate {
			get {
				return (this._discountRate);
			}
			set {
				this._discountRate = value;
			}
		}


		private bool _isActive = true;
		public bool IsActive {
			get {
				return (this._isActive);
			}			
			set {
				_isActive = value;
			}
		}

		private string _recurringProfileID = "";
		public string RecurringProfileID {
			get {
				return (this._recurringProfileID);
			}
			set {
				this._recurringProfileID = value;
			}
		}

		public double ActualItemPrice {
			get {
				return this.ItemPrice*(1-DiscountRate);
			}
		}

		public double SavedTotal {
			get {
				return ItemPrice-ActualItemPrice;
			}
		}

		#endregion
		
		public void AddProductFromOption(QuoteOption quoteOption) {
			foreach(Product p in quoteOption.Products) {
				this.AddProduct(p);
			}
		}	

		public void AddProductFeatureFromOption(QuoteOption quoteOption) {
			foreach(ProductFeature pf in quoteOption.ProductFeatures) {
				this.AddProductFeature(pf);
			}
		}

		public void AddServiceFromOption(QuoteOption quoteOption) {
			foreach(Service service in quoteOption.Services) {
				this.AddService(service);
			}
		}

		public BillItem CreateBillItem(BillItem historyBillItem, DateTime billCreatedDate) {			
			if (historyBillItem != null && this.PriceType == PriceTypeEnum.OneTime) {
				return null;
			}

			if (historyBillItem != null && historyBillItem.StartDate > billCreatedDate) {
				return null;
			}
			BillItem item = new BillItem(this);	
			item.StartDate = item.CalculateStartDate(historyBillItem, billCreatedDate);
			item.EndDate = item.CalculateEndDate();		
			return item;
		}

		#region Database Methods
		
		/// This function always need to use together with Save() in Quote class
		/// Connection need to be openned before passed in.
		
		/// <param name="connection"></param>
		public void Save(SqlConnection connection, int userID) {
			// Insert
			if (ID == -1) {
				string insertSql = @"INSERT INTO [QuoteItem] (QuoteID, CreatedUserID, CreatedDate, IsDeleted, Code, Name, Description, Quantity, Rate, DiscountRate, LicenseType, PriceType, FrequencyType, IsCustomerCredit, IsActive, ProductIDs, ProductFeatureIDs, Services, RecurringProfileID)
				Values (@qouteID, @createduserid, GETDATE(), 0, @code, @name, @description, @quantity, @rate, @discountRate, @licenseType, @priceType, @frequencyType, @isCustomerCredit, @isActive, @productIDs, @productFeatureIDs, @services, @recurringProfileID);select @@identity;";
				SqlCommand insertCommand = new SqlCommand(insertSql, connection);
				try {
					insertCommand.Parameters.Add("@qouteID", SqlDbType.Int, 4).Value = Parent.QuoteID;
                    insertCommand.Parameters.Add("@createduserid", SqlDbType.Int, 4).Value = userID;
                    insertCommand.Parameters.Add("@code", SqlDbType.VarChar, 50).Value = this.Code;
					insertCommand.Parameters.Add("@name", SqlDbType.VarChar, 100).Value = this.Name;
					insertCommand.Parameters.Add("@description", SqlDbType.Text, 500).Value = this.Description;
					insertCommand.Parameters.Add("@quantity", SqlDbType.Int, 4).Value = this.Quantity;
					insertCommand.Parameters.Add("@rate", SqlDbType.Float, 8).Value = this.Rate;
					insertCommand.Parameters.Add("@discountRate", SqlDbType.Float, 8).Value = this.DiscountRate;
					insertCommand.Parameters.Add("@licenseType", SqlDbType.SmallInt, 2).Value = this.LicenseType;
					insertCommand.Parameters.Add("@priceType", SqlDbType.SmallInt, 2).Value = this.PriceType;
					insertCommand.Parameters.Add("@frequencyType", SqlDbType.SmallInt, 2).Value = this.Frequency;
					insertCommand.Parameters.Add("@isCustomerCredit", SqlDbType.Bit, 2).Value = this.IsCustomerCredit;
					insertCommand.Parameters.Add("@isActive", SqlDbType.Bit, 2).Value = this.IsActive;
					insertCommand.Parameters.Add("@productIDs", SqlDbType.VarChar, 47).Value = this.ProductIDs;
					insertCommand.Parameters.Add("@productFeatureIDs", SqlDbType.VarChar, 399).Value = this.ProductFeatureIDs;
					insertCommand.Parameters.Add("@services", SqlDbType.VarChar, 1000).Value = Service.SerializeServices(Services);
					insertCommand.Parameters.Add("@recurringProfileID", SqlDbType.VarChar, 100).Value = RecurringProfileID;
					insertCommand.Prepare();

					ID = Convert.ToInt32(insertCommand.ExecuteScalar());
				} finally {
					insertCommand.Dispose();
				}				
				return;
			}

			// Update 
			// QuoteID = @quoteID,
			
			string updateSQL = string.Format(@"UPDATE [QuoteItem] SET Code = @code, UpdatedUserID=@updateduserid, UpdatedDate=GetDate(), Name = @name, Description = @description, 
								Quantity = @quantity, Rate = @rate, DiscountRate = @discountRate, 
								LicenseType = @licenseType, PriceType = @priceType, FrequencyType = @frequencyType,
								IsCustomerCredit = @isCustomerCredit, IsActive = @isActive, Services = @services, RecurringProfileID = @recurringProfileID
								{0}
								{1}
								 where QuoteItemID = @quoteItemID;", 
				needToSaveProductIDs?",ProductIDs = @productIDs":"", 
				needToSaveProductFeatureIDs?",ProductFeatureIDs = @productFeatureIDs":"");
			SqlCommand updateCommand = new SqlCommand(updateSQL, connection);

			try {
				//updateCommand.Parameters.Add("@quoteID", SqlDbType.Int, 4).Value = quoteID;
				updateCommand.Parameters.Add("@quoteItemID", SqlDbType.Int, 4).Value = this.ID;
                updateCommand.Parameters.Add("@updateduserid", SqlDbType.Int, 4).Value = userID;
				updateCommand.Parameters.Add("@code", SqlDbType.VarChar, 50).Value = this.Code;
				updateCommand.Parameters.Add("@name", SqlDbType.VarChar, 100).Value = this.Name;
				updateCommand.Parameters.Add("@description", SqlDbType.Text, 500).Value = this.Description;
				updateCommand.Parameters.Add("@quantity", SqlDbType.Int, 4).Value = this.Quantity;
				updateCommand.Parameters.Add("@rate", SqlDbType.Float, 8).Value = this.Rate;
				updateCommand.Parameters.Add("@discountRate", SqlDbType.Float, 8).Value = this.DiscountRate;
				updateCommand.Parameters.Add("@licenseType", SqlDbType.SmallInt, 2).Value = this.LicenseType;
				updateCommand.Parameters.Add("@priceType", SqlDbType.SmallInt, 2).Value = this.PriceType;
				updateCommand.Parameters.Add("@frequencyType", SqlDbType.SmallInt, 2).Value = this.Frequency;
				updateCommand.Parameters.Add("@isCustomerCredit", SqlDbType.Bit, 2).Value = this.IsCustomerCredit;
				updateCommand.Parameters.Add("@isActive", SqlDbType.Bit, 2).Value = this.IsActive;
				updateCommand.Parameters.Add("@Services", SqlDbType.VarChar, 1000).Value = Service.SerializeServices(Services);
				updateCommand.Parameters.Add("@recurringProfileID", SqlDbType.VarChar, 100).Value = RecurringProfileID;
				if (needToSaveProductIDs) {
					updateCommand.Parameters.Add("@productIDs", SqlDbType.VarChar, 47).Value = this.ProductIDs;
				}
				if (needToSaveProductFeatureIDs) {
					updateCommand.Parameters.Add("@productFeatureIDs", SqlDbType.VarChar, 399).Value = this.ProductFeatureIDs;
				}
				updateCommand.Prepare();
				updateCommand.ExecuteNonQuery();
			} finally {
				updateCommand.Dispose();
			}
		}


		public void Save(int userID) {
			SqlConnection conn = new SqlConnection(DataFunctions.GetConnectionString());
			try {
				conn.Open();
				Save(conn, userID);
			} finally {
				conn.Close();
				conn.Dispose();
			}	
		}

		public void Delete(int userID) {
			if (ID > 0) {
                DataFunctions.Execute("UPDATE [QuoteItem] set IsDeleted = 1, UpdatedDate=GETDATE(), UpdatedUserID=" + userID + " where QuoteID = " + this.ID.ToString());
			}				
		}
		#endregion

		#region Static Database Methods
		public static QuoteItemCollection GetQuoteItemsByQuoteID(int quoteID) {			
			QuoteItemCollection items = new QuoteItemCollection();
            DataTable dt = DataFunctions.GetDataTable(string.Format("SELECT * from [QuoteItem] where QuoteID = {0} AND Code != '{1}'", quoteID.ToString() + " and IsDeleted = 0", "TestingEMB"));
			Quote parent = new Quote(quoteID);
			foreach(DataRow row in dt.Rows) {			
				items.Add(GetQuoteItem(row,parent));
			}
			return items;
		}

		public static QuoteItem GetQuoteItemByID(int quoteItemID) {
            DataTable dt = DataFunctions.GetDataTable("SELECT * from [QuoteItem] where QuoteItemID = " + quoteItemID.ToString() + " and IsDeleted = 0");
			
			if (dt.Rows.Count == 0) {
				return null;
			}

			return GetQuoteItem(dt.Rows[0], Quote.GetQuoteByID(Convert.ToInt32(dt.Rows[0]["QuoteID"])));			
		}

		private static QuoteItem GetQuoteItem(DataRow row, Quote parent) {
			QuoteItem item = new QuoteItem((FrequencyEnum) Convert.ToInt16(row["FrequencyType"]),Convert.ToString(row["Code"]),Convert.ToString(row["Name"]),Convert.ToString(row["Description"]),Convert.ToInt64(row["Quantity"]),
				Convert.ToDouble(row["Rate"]), (LicenseTypeEnum) Convert.ToInt16(row["LicenseType"]), (PriceTypeEnum) Convert.ToInt16(row["PriceType"]));
			item.Parent = parent;
			item.DiscountRate = Convert.ToDouble(row["DiscountRate"]);
			item.ID = Convert.ToInt32(row["QuoteItemID"]);
			item.IsCustomerCredit = Convert.ToBoolean(row["IsCustomerCredit"]);
			item.IsActive = Convert.ToBoolean(row["IsActive"]);	
			item.RecurringProfileID = Convert.ToString(row["RecurringProfileID"]);
			//LoadProducts(item, Convert.ToString(row["ProductIDs"]));
			//LoadProductFeatures(item, Convert.ToString(row["ProductFeatureIDs"]));
			LoadServices(item, Convert.ToString(row["Services"]));
			return item;
		}		
		#endregion
	}

	public class QuoteItemCollection : CollectionBase {
		public void Add(QuoteItem item) {
			base.InnerList.Add(item);
		}

		public void AddRange(QuoteItemCollection items) {
			base.InnerList.AddRange(items);
		}

		public QuoteItem this[int index] {
			get { return (QuoteItem) base.InnerList[index];}
		}		

		public new void RemoveAt(int index) {			
			base.InnerList.RemoveAt(index);
		}
	}
}
