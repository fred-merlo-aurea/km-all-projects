using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ecn.common.classes;
using ecn.common.classes.billing;

namespace ecn.common.classes
{
	public enum ChannelPartnerTypeEnum {NotInitialized = 0,  Silver = 1, Gold = 2, Platinum =3, NotApplicable = 4}
	public class BaseChannel {
		public BaseChannel(int id, string name) : this(name) {	
			_id = id;		
		}

		public BaseChannel(string name) {
			_name = name;			
		}

		private int _id;
		public int ID {
			get {
				return (this._id);
			}
			set {
				this._id = value;
			}
		}

		public bool IsNew {
			get { return _id <=0; }
		}

		private string _name;
		public string Name {
			get {
				return (this._name);
			}
			set {
				this._name = value;
			}
		}	


		private string _channelURL;
		public string ChannelURL {
			get {
				if (!IsNew && _channelURL == null) {
					_channelURL = LoadedBaseChannel.ChannelURL;
				}
				return (this._channelURL);
			}
			set {
				this._channelURL = value;
			}
		}

		private string _bounceDomain = "bounce2.com";
		public string BounceDomain {
			get {				
				return (this._bounceDomain);
			}
			set {
				this._bounceDomain = value;
			}
		}

		private string _webAddress;
		public string WebAddress {
			get {
				if (!IsNew && _webAddress == null) {
					_webAddress = LoadedBaseChannel.WebAddress;
				}
				return (this._webAddress);
			}
			set {
				this._webAddress = value;
			}
		}

		private bool _hasAccessToCreator = true;
		public bool HasAccessToCreator {
			get {
				return (this._hasAccessToCreator);
			}
			set {
				this._hasAccessToCreator = value;
			}
		}

		private bool _hasAccessToCollector = true;
		public bool HasAccessToCollector {
			get {
				return (this._hasAccessToCollector);
			}
			set {
				this._hasAccessToCollector = value;
			}
		}

		private bool _hasAccessToCommunicator = true;
		public bool HasAccessToCommunicator {
			get {
				return (this._hasAccessToCommunicator);
			}
			set {
				this._hasAccessToCommunicator = value;
			}
		}

		private bool _hasAccessToCharity = true;
		public bool HasAccessToCharity {
			get {
				return (this._hasAccessToCharity);
			}
			set {
				this._hasAccessToCharity = value;
			}
		}

		private bool _hasAccessToPublisher = true;
		public bool HasAccessToPublisher {
			get {
				return (this._hasAccessToPublisher);
			}
			set {
				this._hasAccessToPublisher = value;
			}
		}

		private RatesKeeper _rates;
		public RatesKeeper Rates {
			get {
				if (IsNew && _rates == null) {
					_rates = new RatesKeeper();
					return _rates;
				}

				if (!IsNew && _rates == null) {
					_rates = new RatesKeeper();
					//_rates = LoadedBaseChannel.Rates;
				}
				return (this._rates);
			}			
			set {
				_rates = value;
			}
		}

		private Contact _contact = null;
		public Contact Contact {
			get {
				if (IsNew && _contact == null) {
					_contact = new Contact();
					return (this._contact);
				}

				if (!IsNew && _contact == null) {
					_contact = new Contact();
					//_contact = LoadedBaseChannel.Contact;
				}
				return (this._contact);
			}
			set {
				this._contact = value;
			}
		}

		private ChannelPartnerTypeEnum _channelPartnerType = ChannelPartnerTypeEnum.NotInitialized;
		public ChannelPartnerTypeEnum ChannelPartnerType {
			get {
				if (!IsNew && _channelPartnerType == ChannelPartnerTypeEnum.NotInitialized) {
					_channelPartnerType = LoadedBaseChannel.ChannelPartnerType;
				}
				return (this._channelPartnerType);
			}
			set {
				this._channelPartnerType = value;
			}
		}

		private string _channelType;
		public string ChannelType 
		{
			get 
			{
				return (this._channelType);
			}
			set 
			{
				this._channelType = value;
			}
		}


		#region These properties need to be moved out to Channel Licenses
//		private DateTime _purchaseDate = DateTime.MinValue;
//		public DateTime PurchaseDate {
//			get {
//				if (!IsNew && _purchaseDate == DateTime.MinValue) {
//					_purchaseDate = LoadedBaseChannel.PurchaseDate;
//				}
//				return (this._purchaseDate);
//			}
//			set {
//				this._purchaseDate = value;
//			}
//		}
//
//		private DateTime _expirationDate = DateTime.MinValue;
//		public DateTime ExpirationDate {
//			get {
//				if (!IsNew && _expirationDate == DateTime.MinValue) {
//					_expirationDate = LoadedBaseChannel.ExpirationDate;
//				}
//				return (this._expirationDate);
//			}
//			set {
//				this._expirationDate = value;
//			}
//		}
//
//		private int _channelUsage = -1;
//		public int ChannelUsage {
//			get {
//				if (!IsNew && _channelUsage == -1) {
//					_channelUsage = LoadedBaseChannel.ChannelUsage;
//				}
//				return (this._channelUsage);
//			}
//			set {
//				this._channelUsage = value;
//			}
//		}
//
//		private int _used = -1;
//		public int Used {
//			get {
//				if (!IsNew && _used == -1) {
//					_used = LoadedBaseChannel.Used;
//				}
//				return (this._used);
//			}
//			set {
//				this._used = value;
//			}
//		}
		#endregion

		#region Methods for Lazy Loading
		private BaseChannel _loadedBaseChannel = null;
		protected BaseChannel LoadedBaseChannel {
			get {
				if (_loadedBaseChannel == null) {
					_loadedBaseChannel					= GetBaseChannelByID(this.ID);
					this.HasAccessToCreator				= _loadedBaseChannel.HasAccessToCreator;
					this.HasAccessToCollector			= _loadedBaseChannel.HasAccessToCollector;
					this.HasAccessToCommunicator	= _loadedBaseChannel.HasAccessToCommunicator;					
					this.HasAccessToCharity				= _loadedBaseChannel.HasAccessToCharity;			
					this.HasAccessToPublisher			= _loadedBaseChannel.HasAccessToPublisher;		
				}
				return (this._loadedBaseChannel);
			}		
		}

		#endregion

		#region Database Methods
		public void Save() {
			if (this.IsNew) {
				Insert();
				return;
			}

			Update();
		}

		private void Insert()
        {
            var builder = new StringBuilder();
            builder.Append("INSERT INTO BaseChannel (BaseChannelName, ChannelPartnerType, RatesXML, Salutation, ContactName,");
            builder.Append("ContactTitle, Phone, Fax, Email, Address, City, State, Country, Zip, BounceDomain, ChannelURL, WebAddress, ");
            builder.Append("AccessCommunicator, AccessCreator, AccessCollector, AccessCharity, AccessPublisher, channelType) VALUES ");
            builder.Append("(@name,@channelPartnerType,@ratesXML, @salutation,@contactName,@contactTitle,@phone,@fax,@email,@streetAddress,");
            builder.Append("@city,@state,@country,@zip, @bounceDomain, @channelURL, @webAddress, @AccessCommunicator, @AccessCreator, ");
            builder.Append("@AccessCollector, @AccessCharity, @AccessPublisher, @channelType); SELECT @@IDENTITY;");

            var connection = new SqlConnection(DataFunctions.GetConnectionString());
            var command = new SqlCommand(builder.ToString(), connection);

            try
            {
                connection.Open();
                command = PrepareCommand(command);
                this.ID = Convert.ToInt32(command.ExecuteScalar());
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
		}

		private void Update()
        {
            var builder = new StringBuilder();
            builder.Append("Update BaseChannel SET BaseChannelName = @name, ChannelPartnerType = @channelPartnerType, RatesXML = @ratesXML, ");
            builder.Append("Salutation = @salutation, ContactName = @contactName, ContactTitle = @contactTitle, Phone = @phone, ");
            builder.Append("Fax = @fax, Email = @email, Address = @streetAddress, City = @city, State = @state, Country = @country, ");
            builder.Append("Zip = @zip, BounceDomain=@bounceDomain, ChannelURL =@channelURL, WebAddress=@webAddress, ");
            builder.Append("AccessCommunicator=@AccessCommunicator, AccessCreator=@AccessCreator, AccessCollector=@AccessCollector, ");
            builder.Append("AccessCharity=@AccessCharity, AccessPublisher=@AccessPublisher, channelType=@channelType ");
            builder.Append("Where BaseChannelID = @baseChannelID;");

            var connection = new SqlConnection(DataFunctions.GetConnectionString());
            var command = new SqlCommand(builder.ToString(), connection);

            try
            {
                connection.Open();
                command = PrepareCommand(command);
				command.ExecuteNonQuery();
			}
            finally
            {
				connection.Close();
				connection.Dispose();
			}
		}

        private SqlCommand PrepareCommand(SqlCommand sqlCommand)
        {
            sqlCommand.Parameters.Add("@baseChannelID", SqlDbType.Int, 8).Value = this.ID;
            sqlCommand.Parameters.Add("@name", SqlDbType.VarChar, 50).Value = this.Name;
            sqlCommand.Parameters.Add("@channelPartnerType", SqlDbType.SmallInt, 2).Value = Convert.ToInt16(this.ChannelPartnerType);
            sqlCommand.Parameters.Add("@ratesXML", SqlDbType.VarChar, 2000).Value = RatesKeeper.Serialize(this.Rates);
            sqlCommand.Parameters.Add("@salutation", SqlDbType.VarChar, 50).Value = this.Contact.Salutation;
            sqlCommand.Parameters.Add("@contactName", SqlDbType.VarChar, 100).Value = this.Contact.ContactName;
            sqlCommand.Parameters.Add("@contactTitle", SqlDbType.VarChar, 50).Value = this.Contact.ContactTitle;
            sqlCommand.Parameters.Add("@phone", SqlDbType.VarChar, 50).Value = this.Contact.Phone;
            sqlCommand.Parameters.Add("@fax", SqlDbType.VarChar, 50).Value = this.Contact.Fax;
            sqlCommand.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = this.Contact.Email;
            sqlCommand.Parameters.Add("@streetAddress", SqlDbType.VarChar, 200).Value = this.Contact.StreetAddress;
            sqlCommand.Parameters.Add("@city", SqlDbType.VarChar, 50).Value = this.Contact.City;
            sqlCommand.Parameters.Add("@state", SqlDbType.VarChar, 50).Value = this.Contact.State;
            sqlCommand.Parameters.Add("@country", SqlDbType.VarChar, 50).Value = this.Contact.Country;
            sqlCommand.Parameters.Add("@zip", SqlDbType.VarChar, 20).Value = this.Contact.Zip;
            sqlCommand.Parameters.Add("@bounceDomain", SqlDbType.VarChar, 100).Value = this.BounceDomain;
            sqlCommand.Parameters.Add("@channelURL", SqlDbType.VarChar, 50).Value = this.ChannelURL;
            sqlCommand.Parameters.Add("@webAddress", SqlDbType.VarChar, 255).Value = this.WebAddress;
            sqlCommand.Parameters.Add("@AccessCommunicator", SqlDbType.Bit, 1).Value = this.HasAccessToCommunicator;
            sqlCommand.Parameters.Add("@AccessCreator", SqlDbType.Bit, 1).Value = this.HasAccessToCreator;
            sqlCommand.Parameters.Add("@AccessCollector", SqlDbType.Bit, 1).Value = this.HasAccessToCollector;
            sqlCommand.Parameters.Add("@AccessCharity", SqlDbType.Bit, 1).Value = this.HasAccessToCharity;
            sqlCommand.Parameters.Add("@AccessPublisher", SqlDbType.Bit, 1).Value = this.HasAccessToPublisher;
            sqlCommand.Parameters.Add("@channelType", SqlDbType.VarChar, 50).Value = this.ChannelType;
            sqlCommand.Prepare();

            return sqlCommand;
        }

        #endregion

        #region Static Database methods
        public static BaseChannel GetBaseChannelByID(int id) {
            DataTable dt = DataFunctions.GetDataTable("SELECT * FROM BaseChannel WHERE IsDeleted = 0 and BaseChannelID = " + id.ToString());
			if (dt.Rows.Count == 0) {
				return null;
			}
			
			return GetBaseChannel(dt.Rows[0]);			
		}

		public static ArrayList GetBaseChannels() {
			ArrayList channels = new ArrayList();
            DataTable dt = DataFunctions.GetDataTable("SELECT BaseChannelID, BaseChannelName FROM BaseChannel WHERE IsDeleted = 0 ORDER BY BaseChannelName");

			foreach(DataRow row in dt.Rows) {
				BaseChannel channel = new BaseChannel(Convert.ToInt32(row["BaseChannelID"]), Convert.ToString(row["BaseChannelName"]));
				channels.Add(channel);
			}
			return channels;
		}

		/// This method doesn't not really belong to here, but we don't have Channle Class right now.
		public static int GetChannelIDFromChannelCode(int basechannelID, string channelTypeCode){
			string sqlquery =	" SELECT ChannelID FROM Channel " +
				" WHERE BaseChannelID = "+basechannelID+
                " AND IsDeleted = 0 AND ChannelTypeCode = '" + channelTypeCode + "'";
			return Convert.ToInt32(DataFunctions.ExecuteScalar(sqlquery));			
		}

		private static BaseChannel GetBaseChannel(DataRow row) {
			BaseChannel channel = new BaseChannel(Convert.ToInt32(row["BaseChannelID"]), Convert.ToString(row["BaseChannelName"]));

			channel.ChannelPartnerType = (ChannelPartnerTypeEnum) Convert.ToInt16(row["ChannelPartnerType"]);
			channel.Rates = RatesKeeper.Deserialize(Convert.ToString(row["RatesXML"]));						
			channel.Contact = new Contact(Convert.ToString(row["Salutation"]),Convert.ToString(row["ContactName"]),Convert.ToString(row["ContactTitle"]),Convert.ToString(row["Phone"]),
				Convert.ToString(row["Fax"]),Convert.ToString(row["Email"]),Convert.ToString(row["Address"]),
				Convert.ToString(row["City"]),Convert.ToString(row["State"]),Convert.ToString(row["Country"]),Convert.ToString(row["Zip"]));		
			
			channel.BounceDomain = Convert.ToString(row["BounceDomain"]);
			channel.WebAddress = Convert.ToString(row["WebAddress"]);
			channel.ChannelURL = Convert.ToString(row["ChannelURL"]);
			channel.ChannelType = Convert.ToString(row["ChannelType"]);
	

			channel.HasAccessToCollector = Convert.ToBoolean(row["AccessCollector"]);
			channel.HasAccessToCreator = Convert.ToBoolean(row["AccessCreator"]);
			channel.HasAccessToCommunicator = Convert.ToBoolean(row["AccessCommunicator"]);
			channel.HasAccessToCharity = Convert.ToBoolean(row["AccessCharity"]);
			channel.HasAccessToPublisher = Convert.ToBoolean(row["AccessPublisher"]);
			return channel;
		}
		#endregion
	}


}
