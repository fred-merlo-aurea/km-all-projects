using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ecn.common.classes;

namespace ecn.wizard.webservice.Objects  {	
	public enum CustomerTypeEnum { New, Existing, All}
	
	public class Customer: CustomerBase
    {
		public Customer() : this(IdNone, IdNone) {
		}		
		public Customer(int id, int baseChannelID): base(id, baseChannelID) {}		

		public Customer(int id) : this(id, IdNone) {}

		#region General Properites
		
		// TODO : Need to find a neat way to lazy load bool value. (It doesn't have a null value like -1, null
		private bool _isActive;
		public bool IsActive {
			get {				
				return (this._isActive);
			}
			set {
				this._isActive = value;
			}
		}

		private DateTime _createDate = DateTime.MinValue;
		public DateTime CreateDate {
			get {
				if (!IsNew && _createDate == DateTime.MinValue) {
					_createDate = LoadedCustomer.CreateDate;
				}
				return (this._createDate);
			}
			set {
				this._createDate = value;
			}
		}	

		#endregion

		#region Products Info
		private int _communicatorChannelID = -1;
		public int CommunicatorChannelID {
			get {
				if (!IsNew && _communicatorChannelID == -1) {
					_communicatorChannelID = LoadedCustomer.CommunicatorChannelID;
				}
				return (this._communicatorChannelID);
			}
			set {
				this._communicatorChannelID = value;
			}
		}

		private int _collectorChannelID = -1;
		public int CollectorChannelID {
			get {
				if (!IsNew && _collectorChannelID == -1) {
					_collectorChannelID = LoadedCustomer.CollectorChannelID;
				}
				return (this._collectorChannelID);
			}
			set {
				this._collectorChannelID = value;
			}
		}

		private int _creatorChannelID = -1;
		public int CreatorChannelID {
			get {
				if (!IsNew && _creatorChannelID == -1) {
					_creatorChannelID = LoadedCustomer.CreatorChannelID;
				}
				return (this._creatorChannelID);
			}
			set {
				this._creatorChannelID = value;
			}
		}	

		private int _communicatorLevel = -1;
		public int CommunicatorLevel {
			get {
				if (!IsNew && _communicatorLevel == -1) {
					_communicatorLevel = LoadedCustomer.CommunicatorLevel;
				}
				return (this._communicatorLevel);
			}
			set {
				this._communicatorLevel = value;
			}
		}

		#endregion

		#region Public Methods		

		/// <summary>
		/// create 2 Folders under the ecn.accounts directory
		/// dataFolder = /ecn.accounts/assets/<-channelID_<channelID>->/customers/<-customerID->/data 
		/// imageFolder = /ecn.accounts/assets/<-channelID_<channelID>->/customers/<-customerID->/images
		/// </summary>
		/// <param name="server"></param>
		public void CreateAssertPaths(HttpServerUtility server) {
			AssertCustomerIsSaved("Create Assert Paths");

			ChannelCheck cc = new ChannelCheck();
			string assetsPath = cc.getAssetsPath("accounts");		

			string customerHomeDir = assetsPath +"/"+"channelID_"+BaseChannelID.ToString()+"/customers/" + ID.ToString();  
			DirectoryInfo customerHomeDirInfo = new DirectoryInfo(server.MapPath(customerHomeDir));
			
			if (customerHomeDirInfo.Exists) {
				return;
			}
				
			string dataPath	= assetsPath + "/"+"channelID_"+ BaseChannelID.ToString() +"/customers/" + ID.ToString() + "/data";  
			DirectoryInfo dataDirInfo	= new DirectoryInfo(server.MapPath(dataPath));
			dataDirInfo.Create();

			string imagePath = assetsPath + "/"+"channelID_"+ BaseChannelID.ToString() +"/customers/" + ID.ToString() + "/images";  
			DirectoryInfo imageDirInfo	= new DirectoryInfo(server.MapPath(imagePath));
			imageDirInfo.Create();
		}

		public void CreateDefaultFeatures() {
			AssertCustomerIsSaved("Create default features");

            DataTable product_details = DataFunctions.GetDataTable("select ProductDetailID from ProductDetail");
			foreach(DataRow dr in product_details.Rows) {
				if(DataFunctions.ExecuteScalar("select ProductDetailID from CustomerProduct where CustomerID = "+ID.ToString()+" and ProductDetailID = "+dr["ProductDetailID"].ToString()) == null){
                    DataFunctions.Execute("insert into CustomerProduct (CustomerID, ProductDetailID, ModifyDate) values (" + ID.ToString() + "," + dr["ProductDetailId"].ToString() + ",'" + DateTime.Now.ToString() + "')");
				}
			}			
		}

		public void CreateDefaulRole() {
			string role_id = DataFunctions.ExecuteScalar("insert into Roles (CustomerID, RoleName) values (" + ID.ToString() + ",'Everything');SELECT @@IDENTITY").ToString();

			DataTable action_ids = DataFunctions.GetDataTable("select * from Action");
			foreach(DataRow aid in action_ids.Rows) {
				DataFunctions.Execute("insert into RoleActions (RoleID , ActionID, Active) values (" + role_id + "," + aid["ActionID"].ToString() + ",'Y')");
			}
		}

		#endregion

		#region Datadase Methods
		
		private Customer _loadedCustomer = null;
		private Customer LoadedCustomer {
			get {				
				if (_loadedCustomer == null) {
					_loadedCustomer = Customer.GetCustomerByID(ID);
				}			
				_isActive = _loadedCustomer.IsActive;
				return _loadedCustomer;
			}
		}

        protected override CustomerBase LoadedCustomerBase => LoadedCustomer;

        public void Save() {
			SqlConnection conn = new SqlConnection(DataFunctions.GetConnectionString());
			try {
				conn.Open();

				if (IsNew) {
				    ID = New(conn);
					SaveBillingContact(ID, BillingContact, conn);
				} else {
					Update(conn);
					SaveBillingContact(ID, BillingContact, conn);
				}
			} finally {
				conn.Close();
				conn.Dispose();
			}			
		}

		private void Update(SqlConnection conn) {	
			string sqlquery= @" UPDATE Customer SET  BaseChannelID = @baseChannelID,
				 CustomerName= @customerName,
				 ActiveFlag= @activeFlag,
				 Address= @address,
				 City= @city,
				 State= @state,
				 Country = @country,
				 Zip= @zip,				 
				 Salutation = @salutation,
				 ContactName= @contactName,
				 FirstName = @firstName,
				 LastName = @lastName,
				 ContactTitle= @contactTitle,
				 Email= @email,
				 Phone= @phone,
				 Fax = @fax,
				 WebAddress= @webAddress,
				 TechContact = @techContact,
				 TechEmail = @techEmail,
				 TechPhone = @techPhone,
				 SubscriptionsEmail = @subscriptionsEmail,
				 AccountsLevel = @accountsLevel,				 
				 CommunicatorLevel = @communicatorLevel,
				 CommunicatorChannelID = @communicatorChannelID,
				 CollectorLevel = @collectorLevel,
				 CollectorChannelID = @collectorChannelID,
				 CreatorLevel = @creatorLevel,
				 CreatorChannelID = @creatorChannelID
				 WHERE CustomerID= @customerID";
			SqlCommand update = new SqlCommand(sqlquery, conn);

		    MapCustomerGeneralContact(update);
		    MapCustomerLevelInformation(update);
            update.Parameters.Add("@communicatorChannelID", SqlDbType.Int, 4).Value = CommunicatorChannelID;
			update.Parameters.Add("@collectorChannelID", SqlDbType.Int, 4).Value = CollectorChannelID;
			update.Parameters.Add("@creatorChannelID", SqlDbType.Int, 4).Value = CreatorChannelID;
			update.Parameters.Add("@accountsLevel", SqlDbType.VarChar, 50).Value = AccountLevel;
			update.Parameters.Add("@communicatorLevel", SqlDbType.VarChar, 50).Value = CommunicatorLevel;
			update.Parameters.Add("@collectorLevel", SqlDbType.VarChar, 50).Value = CollectorLevel;
			update.Parameters.Add("@creatorLevel", SqlDbType.VarChar, 50).Value = CreatorLevel;
			update.Parameters.Add("@customerID", SqlDbType.Int, 4).Value = ID;
			try {
				update.Prepare();
				update.ExecuteNonQuery();
			} finally {
				update.Dispose();
			}
		}

        public override void MapCustomerGeneralContact(SqlCommand command)
        {
            base.MapCustomerGeneralContact(command);
            command.Parameters.Add("@activeFlag", SqlDbType.VarChar, 1).Value = IsActive ? "Y" : "N";
        }


        private int New(SqlConnection conn) {
			string sqlquery= @" INSERT INTO Customer (BaseChannelID, CreateDate ,CustomerName, ActiveFlag, Address,City,State,Country,Zip,Salutation,ContactName,FirstName,LastName,ContactTitle,Email,Phone,Fax,WebAddress, TechContact, TechEmail, TechPhone, SubscriptionsEmail, AccountsLevel ,  CommunicatorLevel ,CommunicatorChannelID ,CollectorLevel , CollectorChannelID ,CreatorLevel ,CreatorChannelID) VALUES
(@baseChannelID, @createDate, @customerName,@activeFlag,@address,@city,@state, @country, @zip, @salutation,@contactName,@firstName,@lastName,@contactTitle,@email,@phone,@fax,@webAddress, @techContact, @techEmail, @techPhone, @subscriptionsEmail, @accountsLevel, @communicatorLevel,@communicatorChannelID, @collectorLevel, @collectorChannelID, @creatorLevel, @creatorChannelID);SELECT @@IDENTITY;";
		
			SqlCommand insert = new SqlCommand(sqlquery, conn);
			insert.Parameters.Add("@baseChannelID", SqlDbType.Int, 4).Value = this.BaseChannelID;
			insert.Parameters.Add("@createDate", SqlDbType.DateTime, 8).Value = DateTime.Now;
			insert.Parameters.Add("@customerName", SqlDbType.VarChar, 150).Value = this.Name;
			insert.Parameters.Add("@activeFlag", SqlDbType.VarChar, 1).Value = this.IsActive?"Y":"N";
			insert.Parameters.Add("@address", SqlDbType.VarChar, 255).Value = this.GeneralContact.StreetAddress;
			insert.Parameters.Add("@city", SqlDbType.VarChar, 255).Value = this.GeneralContact.City;
			insert.Parameters.Add("@state", SqlDbType.VarChar, 255).Value = this.GeneralContact.State;
			insert.Parameters.Add("@country", SqlDbType.VarChar, 50).Value = this.GeneralContact.Country;
			insert.Parameters.Add("@zip", SqlDbType.VarChar, 50).Value = this.GeneralContact.Zip;
			insert.Parameters.Add("@salutation", SqlDbType.VarChar,10).Value = this.GeneralContact.Salutation;
			insert.Parameters.Add("@contactName", SqlDbType.VarChar, 255).Value = this.GeneralContact.ContactName;
			insert.Parameters.Add("@firstName", SqlDbType.VarChar, 50).Value = this.GeneralContact.FirstName;
			insert.Parameters.Add("@lastName", SqlDbType.VarChar, 50).Value = this.GeneralContact.LastName;
			insert.Parameters.Add("@contactTitle", SqlDbType.VarChar, 255).Value = this.GeneralContact.ContactTitle;
			insert.Parameters.Add("@email", SqlDbType.VarChar, 255).Value = this.GeneralContact.Email;
			insert.Parameters.Add("@phone", SqlDbType.VarChar, 255).Value = this.GeneralContact.Phone;
			insert.Parameters.Add("@fax", SqlDbType.VarChar, 50).Value = this.GeneralContact.Fax;
			insert.Parameters.Add("@webAddress", SqlDbType.VarChar, 255).Value = this.WebAddress;
			insert.Parameters.Add("@techContact", SqlDbType.VarChar, 50).Value = this.TechContact;
			insert.Parameters.Add("@techEmail", SqlDbType.VarChar, 255).Value = this.TechEmail;
			insert.Parameters.Add("@techPhone", SqlDbType.VarChar, 255).Value = this.TechPhone;
			insert.Parameters.Add("@subscriptionsEmail", SqlDbType.VarChar, 255).Value = this.SubscriptionsEmail;
			insert.Parameters.Add("@communicatorChannelID", SqlDbType.Int, 4).Value = this.CommunicatorChannelID;
			insert.Parameters.Add("@collectorChannelID", SqlDbType.Int, 4).Value = this.CollectorChannelID;
			insert.Parameters.Add("@creatorChannelID", SqlDbType.Int, 4).Value = this.CreatorChannelID;
			insert.Parameters.Add("@accountsLevel", SqlDbType.VarChar, 50).Value = this.AccountLevel;
			insert.Parameters.Add("@communicatorLevel", SqlDbType.VarChar, 50).Value = this.CommunicatorLevel;
			insert.Parameters.Add("@collectorLevel", SqlDbType.VarChar, 50).Value = this.CollectorLevel;
			insert.Parameters.Add("@creatorLevel", SqlDbType.VarChar, 50).Value = this.CreatorLevel;
			try {
				return Convert.ToInt32(insert.ExecuteScalar());
			} finally {
				insert.Dispose();
			}
		}

		private void SaveBillingContact(int customerID, Contact billingContact, SqlConnection conn) {
			if (DataFunctions.ExecuteScalar("SELECT BillingContactID from BillingContact where CustomerID = " + customerID.ToString()) == null) {
				NewBillingContact(BillingContact, conn);
				return;
			}

			UpdateBillingContact(BillingContact, conn);
		}

		private void UpdateBillingContact(Contact billingContact, SqlConnection conn) {			
			SqlCommand update = new SqlCommand(@"UPDATE BillingContact  SET CustomerID = @customerID, Salutation = @salutation, ContactName = @contactName,FirstName=@firstName, LastName=@lastName, ContactTitle = @contactTitle, Phone = @phone, Fax=@fax, Email=@email, StreetAddress=@streetAddress, City=@city, State = @state, Country = @country, Zip = @zip where CustomerID = " + ID.ToString(), conn);
			try {
			    MapBillingContactParametersBase(update);
				update.Prepare();
				update.ExecuteNonQuery();	
			} finally {
				update.Dispose();
			}
		}

		private void NewBillingContact(Contact billingContact, SqlConnection conn)
		{			
			var insert = new SqlCommand(
			    "INSERT INTO BillingContact (" +
			    "CustomerID, Salutation, ContactName, FirstName, LastName, ContactTitle, Phone, Fax, " +
			    "Email, StreetAddress, City, State, Country, Zip) " +
			    "VALUES (@customerID, @salutation, @contactName, @firstName, @lastName, @contactTitle, " +
			    "@phone, @fax, @email, @streetAddress, @city, @state, @country, @zip);", 
			    conn);
			try
			{
				MapBillingContactParametersBase(insert);
			    insert.Prepare();
				insert.ExecuteNonQuery();	
			}
			finally
			{
				insert.Dispose();
			}
		}

        #endregion
		
		#region Static Database Methods
		public static Customer GetCustomerByID(int customerID) {
			ArrayList customers = new ArrayList();
			DataTable dt = DataFunctions.GetDataTable(string.Format("SELECT * from Customer where CustomerID = {0}", customerID));
			if (dt.Rows.Count == 0) {
				return null;
			}

			return CreateCustomer(dt.Rows[0]);
		}

		public static ArrayList GetAllCustomersByChannelID(int baseChannelID) {
			ArrayList customers = new ArrayList();
			DataTable dt = DataFunctions.GetDataTable(string.Format("SELECT CustomerID,BaseChannelID,CustomerName,ActiveFlag from Customer where BaseChannelID = {0} and ActiveFlag ='Y' ORDER BY CustomerName", baseChannelID));
			foreach(DataRow c in dt.Rows) {				
				customers.Add(CreateCustomerWithFewProperites(c));
			}
			return customers;
		}

		public static ArrayList GetAllCustomers() {
			ArrayList customers = new ArrayList();
			DataTable dt = DataFunctions.GetDataTable("SELECT * from Customer where ActiveFlag ='Y' ORDER BY CustomerID");
			foreach(DataRow c in dt.Rows) {				
				customers.Add(CreateCustomer(c));
			}
			return customers;
		}

		private static Customer CreateCustomer(DataRow row) {
			Customer customer = new Customer(Convert.ToInt32(row["CustomerID"]), Convert.ToInt32(row["BaseChannelID"]));			
			customer.Name = Convert.ToString(row["CustomerName"]);		
			customer.IsActive = Convert.ToString(row["ActiveFlag"]) == "Y"? true:false;
			customer.WebAddress = Convert.ToString(row["WebAddress"]);
			customer.TechContact = Convert.ToString(row["TechContact"]);
			customer.TechEmail = Convert.ToString(row["TechEmail"]);
			customer.TechPhone = Convert.ToString(row["TechPhone"]);
			customer.SubscriptionsEmail = Convert.ToString(row["SubscriptionsEmail"]);

			customer.CommunicatorChannelID = Convert.ToInt32(row["CommunicatorChannelID"]);
			customer.CollectorChannelID = Convert.ToInt32(row["CollectorChannelID"]);
			customer.CreatorChannelID = Convert.ToInt32(row["CreatorChannelID"]);
			customer.AccountLevel = row["AccountsLevel"] is DBNull?0:Convert.ToInt32(row["AccountsLevel"]);
			customer.CommunicatorLevel = Convert.ToInt32(row["CommunicatorLevel"]);
			customer.CollectorLevel = Convert.ToInt32(row["CollectorLevel"]);
			customer.CreatorLevel = Convert.ToInt32(row["CreatorLevel"]);

			customer.GeneralContact = new Contact(row["Salutation"].ToString(),row["ContactName"].ToString(),row["ContactTitle"].ToString(),row["Phone"].ToString(),
				row["Fax"].ToString(), row["Email"].ToString(),row["Address"].ToString(),row["City"].ToString(),row["State"].ToString(),
				row["Country"].ToString(), row["Zip"].ToString());			
			return customer;
		}

		private static Customer CreateCustomerWithFewProperites(DataRow row) {
			Customer customer = new Customer(Convert.ToInt32(row["CustomerID"]), Convert.ToInt32(row["BaseChannelID"]));			
			customer.Name = Convert.ToString(row["CustomerName"]);		
			customer.IsActive = Convert.ToString(row["ActiveFlag"]) == "Y"? true:false;			
			return customer;
		}

		protected override Contact GetBillingContact(int customerID) {
			DataTable dt = DataFunctions.GetDataTable("SELECT * from BillingContact where CustomerID = " + customerID.ToString());
			if (dt.Rows.Count == 0) {
				return null;
			}

			DataRow row = dt.Rows[0];
			return new Contact(row["Salutation"].ToString(),row["ContactName"].ToString(),row["ContactTitle"].ToString(),row["Phone"].ToString(),
				row["Fax"].ToString(), row["Email"].ToString(),row["StreetAddress"].ToString(),row["City"].ToString(),row["State"].ToString(),
				row["Country"].ToString(), row["Zip"].ToString());	
		}

		public static void DeleteByCustomerID(int customerID) {
			StringBuilder sql = new StringBuilder();
			sql.Append(string.Format(@"DELETE FROM Customer WHERE CustomerID = {0};
DELETE FROM BillingContact WHERE CustomerID = {0};
DELETE FROM Roles where CustomerID = {0};
DELETE FROM CustomerProduct where CustomerID = {0};
DELETE FROM UserActions where UserID in (select UserID from Users where CustomerID = {0});
DELETE FROM Users where CustomerID = {0};", customerID));
			DataFunctions.Execute(sql.ToString());
			
			// TODO: Need to delete other customer related data, quote, quote item, bill, bill item, License
		}
		#endregion
	}
}
