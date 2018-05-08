using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using ecn.common.classes.billing;


namespace ecn.common.classes
{
    public enum CustomerTypeEnum { New, Existing, All }
    public class Customer : CustomerBase
    {
        private const int NewIdValue = 0;
        private const string YesValue = "Y";

        public Customer(): this(IdNone, IdNone) {}

        public Customer(int id, int baseChannelID): base(id, baseChannelID)
        {
            _quotes = new ArrayList();
            _bills = new BillCollection();
        }

        public Customer(int id) : this(id, IdNone) { }

        #region General Properites

        // TODO : Need to find a neat way to lazy load bool value. (It doesn't have a null value like -1, null
        //Changed from bool to string -ashok 03/30/07
        private string _isActive;
        public string IsActive
        {
            get
            {
                if (!IsNew && _isActive == null)
                {
                    _isActive = LoadedCustomer.IsActive;
                }
                return (this._isActive);
            }
            set
            {
                this._isActive = value;
            }
        }
        private string _isDemo;
        public string IsDemo
        {
            get
            {
                if (!IsNew && _isDemo == null)
                {
                    _isDemo = LoadedCustomer.IsDemo;
                }

                if (_isDemo == null)
                    _isDemo = "N";

                return (this._isDemo);
            }
            set
            {
                this._isDemo = value;
            }
        }

        private DateTime _createdDate = DateTime.MinValue;
        public DateTime CreatedDate
        {
            get
            {
                if (!IsNew && _createdDate == DateTime.MinValue)
                {
                    _createdDate = LoadedCustomer.CreatedDate;
                }
                return (this._createdDate);
            }
            set
            {
                this._createdDate = value;
            }
        }

        private string _customerType;
        public string CustomerType
        {
            get
            {
                if (!IsNew && _customerType == null)
                {
                    _customerType = LoadedCustomer.CustomerType;
                }
                //Added 'cos if its a new customer, it was error'ing out 'cos of the customerType is null. - ashok 11/08/06
                if (_customerType == null)
                {
                    _customerType = "Other";
                }
                return (this._customerType);
            }
            set
            {
                this._customerType = value;
            }
        }
        //sunil - added 8/20/2007
        private int _AccountExecutiveID = -1;
        public int AccountExecutiveID
        {
            get
            {
                if (!IsNew && _AccountExecutiveID == -1)
                {
                    _AccountExecutiveID = LoadedCustomer.AccountExecutiveID;
                }
                return (this._AccountExecutiveID);
            }
            set
            {
                this._AccountExecutiveID = value;
            }
        }

        private int _AccountManagerID = -1;
        public int AccountManagerID
        {
            get
            {
                if (!IsNew && _AccountManagerID == -1)
                {
                    _AccountManagerID = LoadedCustomer.AccountManagerID;
                }
                return (this._AccountManagerID);
            }
            set
            {
                this._AccountManagerID = value;
            }
        }

        private string _IsStrategic;
        public string IsStrategic
        {
            get
            {
                if (!IsNew && _IsStrategic == null)
                {
                    _IsStrategic = LoadedCustomer._IsStrategic;
                }
                return (this._IsStrategic);
            }
            set
            {
                this._IsStrategic = value;
            }
        }

        private string _customer_udf1;
        public string customer_udf1
        {
            get
            {
                if (!IsNew && _customer_udf1 == null)
                {
                    _customer_udf1 = LoadedCustomer._customer_udf1;
                }
                return (this._customer_udf1);
            }
            set
            {
                this._customer_udf1 = value;
            }
        }

        private string _customer_udf2;
        public string customer_udf2
        {
            get
            {
                if (!IsNew && _customer_udf2 == null)
                {
                    _customer_udf2 = LoadedCustomer._customer_udf2;
                }
                return (this._customer_udf2);
            }
            set
            {
                this._customer_udf2 = value;
            }
        }
        private string _customer_udf3;
        public string customer_udf3
        {
            get
            {
                if (!IsNew && _customer_udf3 == null)
                {
                    _customer_udf3 = LoadedCustomer._customer_udf3;
                }
                return (this._customer_udf3);
            }
            set
            {
                this._customer_udf3 = value;
            }
        }
        private string _customer_udf4;
        public string customer_udf4
        {
            get
            {
                if (!IsNew && _customer_udf4 == null)
                {
                    _customer_udf4 = LoadedCustomer._customer_udf4;
                }
                return (this._customer_udf4);
            }
            set
            {
                this._customer_udf4 = value;
            }
        }

        private string _customer_udf5;
        public string customer_udf5
        {
            get
            {
                if (!IsNew && _customer_udf5 == null)
                {
                    _customer_udf5 = LoadedCustomer._customer_udf5;
                }
                return (this._customer_udf5);
            }
            set
            {
                this._customer_udf5 = value;
            }
        }


        #endregion

        #region Quote Properties
        private ArrayList _quotes = null;
        public ArrayList Quotes
        {
            get { return ArrayList.ReadOnly(_quotes); }
        }

        public int QuoteCount
        {
            get { return _quotes.Count; }
        }

        public bool HasAnnualTechItem
        {
            get
            {
                foreach (Quote q in _quotes)
                {
                    if (q.HasAnnualTechItem)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public QuoteItemCollection QuoteItems
        {
            get
            {
                QuoteItemCollection items = new QuoteItemCollection();
                foreach (Quote q in Quotes)
                {
                    items.AddRange(q.Items);
                }
                return items;
            }
        }

        public QuoteItemCollection ActiveQuoteItems
        {
            get
            {
                QuoteItemCollection items = new QuoteItemCollection();
                foreach (QuoteItem item in QuoteItems)
                {
                    if (item.Parent.Status == QuoteStatusEnum.Approved && item.IsActive)
                    {
                        items.Add(item);
                    }
                }
                return items;
            }
        }



        /// This function tests if there is an annual tech item exists in other quote.

        /// <param name="quoteID"></param>
        /// <returns></returns>
        public bool HasOtherAnnualTechItem(int quoteID)
        {
            if (ID == -1)
            {
                return false;
            }
            string sql = string.Format("select count(*) from [QuoteItem] where QuoteId in (select quoteID from [Quote] where  CustomerID = {0} and IsDeleted = 0) and QuoteID <> {1} and LicenseType = 0 and IsDeleted = 0", ID, quoteID);
            bool ret = Convert.ToInt32(DataFunctions.ExecuteScalar(sql)) > 0;
            return ret;
        }
        #endregion

        #region Bills
        // Bills are not loaded from the database since we don't always need to access bills.
        private BillCollection _bills;
        public BillCollection Bills
        {
            get
            {
                return (this._bills);
            }
            set
            {
                this._bills = value;
            }
        }
        #endregion

        #region Products Info
        private int _communicatorChannelID = -1;
        public int CommunicatorChannelID
        {
            get
            {
                if (!IsNew && _communicatorChannelID == -1)
                {
                    _communicatorChannelID = LoadedCustomer.CommunicatorChannelID;
                }
                return (this._communicatorChannelID);
            }
            set
            {
                this._communicatorChannelID = value;
            }
        }

        private int _collectorChannelID = -1;
        public int CollectorChannelID
        {
            get
            {
                if (!IsNew && _collectorChannelID == -1)
                {
                    _collectorChannelID = LoadedCustomer.CollectorChannelID;
                }
                return (this._collectorChannelID);
            }
            set
            {
                this._collectorChannelID = value;
            }
        }

        private int _creatorChannelID = -1;
        public int CreatorChannelID
        {
            get
            {
                if (!IsNew && _creatorChannelID == -1)
                {
                    _creatorChannelID = LoadedCustomer.CreatorChannelID;
                }
                return (this._creatorChannelID);
            }
            set
            {
                this._creatorChannelID = value;
            }
        }

        private int _publisherChannelID = -1;
        public int PublisherChannelID
        {
            get
            {
                if (!IsNew && _publisherChannelID == -1)
                {
                    _publisherChannelID = LoadedCustomer.PublisherChannelID;
                }
                return (this._publisherChannelID);
            }
            set
            {
                this._publisherChannelID = value;
            }
        }

        private int _charityChannelID = -1;
        public int CharityChannelID
        {
            get
            {
                if (!IsNew && _publisherChannelID == -1)
                {
                    _charityChannelID = LoadedCustomer.CharityChannelID;
                }
                return (this._charityChannelID);
            }
            set
            {
                this._charityChannelID = value;
            }
        }

        private int _communicatorLevel = -1;
        public int CommunicatorLevel
        {
            get
            {
                if (!IsNew && _communicatorLevel == -1)
                {
                    _communicatorLevel = LoadedCustomer.CommunicatorLevel;
                }
                return (this._communicatorLevel);
            }
            set
            {
                this._communicatorLevel = value;
            }
        }

        private int _publisherLevel = -1;
        public int PublisherLevel
        {
            get
            {
                if (!IsNew && _publisherLevel == -1)
                {
                    _publisherLevel = LoadedCustomer.PublisherLevel;
                }
                return (this._publisherLevel);
            }
            set
            {
                this._publisherLevel = value;
            }
        }

        private int _charityLevel = -1;
        public int CharityLevel
        {
            get
            {
                if (!IsNew && _charityLevel == -1)
                {
                    _charityLevel = LoadedCustomer.CharityLevel;
                }
                return (this._charityLevel);
            }
            set
            {
                this._charityLevel = value;
            }
        }
        #endregion

        #region Public Methods

        public void AddQuote(Quote q)
        {
            q.Customer = this;
            _quotes.Add(q);
        }

        public void AddQuotes(ArrayList quotes)
        {
            foreach (Quote q in quotes)
            {
                AddQuote(q);
            }
        }

        public void RemoveQuoteAt(int index)
        {
            _quotes.RemoveAt(index);
        }

        /// create 2 Folders under the ecn.accounts directory
        /// dataFolder = /ecn.accounts/assets/<-channelID_<channelID>->/customers/<-customerID->/data 
        /// imageFolder = /ecn.accounts/assets/<-channelID_<channelID>->/customers/<-customerID->/images

        /// <param name="server"></param>
        public void CreateAssertPaths(HttpServerUtility server)
        {
            AssertCustomerIsSaved("Create Assert Paths");

            string customerHomeDir = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + ID.ToString(); ;

            if (!Directory.Exists(server.MapPath(customerHomeDir)))
            {
                Directory.CreateDirectory(server.MapPath(customerHomeDir));
            }

            string dataPath = customerHomeDir + "/data";
            if (!Directory.Exists(server.MapPath(dataPath)))
                Directory.CreateDirectory(server.MapPath(dataPath));

            string imagePath = customerHomeDir + "/images";
            if (!Directory.Exists(server.MapPath(imagePath)))
                Directory.CreateDirectory(server.MapPath(imagePath));
        }

        public void CreateDefaultFeatures(int userID)
        {
            AssertCustomerIsSaved("Create default features");

            DataTable product_details = DataFunctions.GetDataTable("select ProductDetailID from [ProductDetail]", ConfigurationManager.AppSettings["act"].ToString());
            foreach (DataRow dr in product_details.Rows)
            {
                if (DataFunctions.ExecuteScalar("accounts", "select ProductDetailID from [CustomerProduct] where IsDeleted = 0 and CustomerID = " + ID.ToString() + " and ProductDetailID = " + dr["ProductDetailID"].ToString()) == null)
                {
                    DataFunctions.Execute("accounts", "insert into [CustomerProduct] (CustomerID, ProductDetailID, CreatedDate, CreatedUserID, IsDeleted) values (" + ID.ToString() + "," + dr["ProductDetailId"].ToString() + ",'" + DateTime.Now.ToString() + "', " + userID + ", 0)");
                }
            }
        }

        public void CreateDefaulRole(int userID)
        {
            string role_id = DataFunctions.ExecuteScalar("accounts", "insert into [Role] (CustomerID, RoleName, CreatedUserID, CreatedDate, IsDeleted) values (" + ID.ToString() + ",'Everything', " + userID + ", GetDate(), 0);SELECT @@IDENTITY").ToString();

            DataTable action_ids = DataFunctions.GetDataTable("select * from [Action] where IsDeleted = 0", ConfigurationManager.AppSettings["act"].ToString());
            foreach (DataRow aid in action_ids.Rows)
            {
                // added the following condition 'cos makes the "Create Approval Messages" check active by default & people forget to UNCheck it. Ref. Iris mail via Darin on 10/23/07
                //- ashok 10/23/07


                if (aid["ActionCode"].ToString().Trim().ToLower().Equals("approvalblast") || aid["ActionCode"].ToString().Trim().ToLower().Equals("ecnwiz") || aid["ActionCode"].ToString().Trim().ToLower().Equals("msgwiz") || aid["ActionCode"].ToString().Trim().ToLower().Equals("maf") || aid["ActionCode"].ToString().Trim().ToLower().Equals("wqt"))
                {
                    DataFunctions.Execute("accounts", "insert into [RoleAction] (RoleID , ActionID, Active, CreatedUserID, CreatedDate, IsDeleted) values (" + role_id + "," + aid["ActionID"].ToString() + ",'N', " + userID + ", GetDate(), 0)");
                }
                else
                {
                    DataFunctions.Execute("accounts", "insert into [RoleAction] (RoleID , ActionID, Active, CreatedUserID, CreatedDate, IsDeleted) values (" + role_id + "," + aid["ActionID"].ToString() + ",'Y', " + userID + ", GetDate(), 0)");
                }
            }
        }

        

        #endregion

        #region Datadase Methods

        private Customer _loadedCustomer = null;
        private Customer LoadedCustomer
        {
            get
            {
                if (_loadedCustomer == null)
                {
                    _loadedCustomer = Customer.GetCustomerByID(ID);
                }
                //_isActive	= _loadedCustomer.IsActive;
                //_isDemo	= _loadedCustomer.IsDemo;

                return _loadedCustomer;
            }
        }

        protected override CustomerBase LoadedCustomerBase => LoadedCustomer;

        public void Save(int userID)
        {
            SqlConnection conn = new SqlConnection(DataFunctions.GetConnectionString("accounts"));
            try
            {
                conn.Open();

                if (IsNew)
                {
                    ID = New(conn, userID);
                    SaveBillingContact(ID, BillingContact, conn, userID);
                }
                else
                {
                    if (this.IsActive.Equals("N"))
                    {
                        DataFunctions.Execute("update users set ActiveFlag='N' where customerID = " + ID.ToString());
                    }
                    Update(conn, userID);
                    SaveBillingContact(ID, BillingContact, conn, userID);
                }
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void Update(SqlConnection conn, int userID)
        {
            string sqlquery = @" UPDATE [Customer] SET  BaseChannelID = @baseChannelID,
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
				 CreatorChannelID = @creatorChannelID,
				 PublisherLevel = @publisherLevel,
				 PublisherChannelID = @publisherChannelID,
				 CharityLevel = @charityLevel,
				 CharityChannelID = @charityChannelID,
				 CustomerType = @CustomerType,
				 DemoFlag = @demoFlag,
				 IsStrategic = @IsStrategic,
				 AccountExecutiveID = @AccountExecutiveID,
				 AccountManagerID = @AccountManagerID,
                 customer_udf1 = @customer_udf1,   
                 customer_udf2 = @customer_udf2,   
                 customer_udf3 = @customer_udf3,   
                 customer_udf4 = @customer_udf4,   
                 customer_udf5 = @customer_udf5,
                 UpdatedUserID = @UserID,
                 UpdatedDate = GetDate(),   
				 WHERE CustomerID= @customerID";

            SqlCommand update = new SqlCommand(sqlquery, conn);
            MapCustomerParameters(userID, update);
            try
            {
                update.Prepare();
                update.ExecuteNonQuery();
            }
            finally
            {
                update.Dispose();
            }
        }

        public SqlCommand MapCustomerParameters(int userId, SqlCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            MapCustomerGeneralContact(command);
            MapCustomerLevelInformation(command);
            MapCustomerAccountsInformation(userId, command);

            return command;
        }

        public override void MapCustomerGeneralContact(SqlCommand command)
        {
            base.MapCustomerGeneralContact(command);
            command.Parameters.Add("@activeFlag", SqlDbType.VarChar, 1).Value = IsActive;
        }

        public override void MapCustomerLevelInformation(SqlCommand command)
        {
            base.MapCustomerLevelInformation(command);
            command.Parameters.Add("@communicatorChannelID", SqlDbType.Int, 4).Value = GetValueOrDefault(CommunicatorChannelID);
            command.Parameters.Add("@collectorChannelID", SqlDbType.Int, 4).Value = GetValueOrDefault(CollectorChannelID);
            command.Parameters.Add("@creatorChannelID", SqlDbType.Int, 4).Value = GetValueOrDefault(CreatorChannelID);
            command.Parameters.Add("@publisherChannelID", SqlDbType.Int, 4).Value = GetValueOrDefault(PublisherChannelID);
            command.Parameters.Add("@charityChannelID", SqlDbType.Int, 4).Value = GetValueOrDefault(CharityChannelID);
            command.Parameters.Add("@accountsLevel", SqlDbType.VarChar, 50).Value = GetValueOrDefault(AccountLevel);
            command.Parameters.Add("@communicatorLevel", SqlDbType.VarChar, 50).Value = GetValueOrDefault(CommunicatorLevel);
            command.Parameters.Add("@collectorLevel", SqlDbType.VarChar, 50).Value = GetValueOrDefault(CollectorLevel);
            command.Parameters.Add("@creatorLevel", SqlDbType.VarChar, 50).Value = GetValueOrDefault(CreatorLevel);
            command.Parameters.Add("@publisherLevel", SqlDbType.Int, 4).Value = GetValueOrDefault(PublisherLevel);
            command.Parameters.Add("@charityLevel", SqlDbType.Int, 4).Value = GetValueOrDefault(CharityLevel);
            command.Parameters.Add("@CustomerType", SqlDbType.VarChar, 50).Value = CustomerType;
            command.Parameters.Add("@demoFlag", SqlDbType.VarChar, 50).Value = IsDemo;
        }

        public void MapCustomerAccountsInformation(int userId, SqlCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.Parameters.Add("@customerID", SqlDbType.Int, 4).Value = ID;
            command.Parameters.Add("@IsStrategic", SqlDbType.Bit).Value = IsStrategic.ToUpper() == YesValue ? 1 : 0;
            command.Parameters.Add("@AccountExecutiveID", SqlDbType.Int, 4).Value = GetValueOrDefault(AccountExecutiveID);
            command.Parameters.Add("@AccountManagerID", SqlDbType.Int, 4).Value = GetValueOrDefault(AccountManagerID);
            command.Parameters.Add("@customer_udf1", SqlDbType.VarChar, 255).Value = customer_udf1 ?? string.Empty;
            command.Parameters.Add("@customer_udf2", SqlDbType.VarChar, 255).Value = customer_udf2 ?? string.Empty;
            command.Parameters.Add("@customer_udf3", SqlDbType.VarChar, 255).Value = customer_udf3 ?? string.Empty;
            command.Parameters.Add("@customer_udf4", SqlDbType.VarChar, 255).Value = customer_udf4 ?? string.Empty;
            command.Parameters.Add("@customer_udf5", SqlDbType.VarChar, 255).Value = customer_udf5 ?? string.Empty;
            command.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = userId;
        }

        public int GetValueOrDefault(int value)
        {
            return value == IdNone ? NewIdValue : value;
        }

        private int New(SqlConnection conn, int userID)
        {
            string sqlquery = @" INSERT INTO [Customer] (BaseChannelID, CreatedDate ,CustomerName, ActiveFlag, Address,City,State,Country,Zip,Salutation,ContactName,FirstName,LastName,ContactTitle,Email,Phone,Fax,WebAddress, TechContact, TechEmail, TechPhone, SubscriptionsEmail, AccountsLevel ,  CommunicatorLevel ,CommunicatorChannelID ,CollectorLevel , CollectorChannelID ,CreatorLevel ,CreatorChannelID, PublisherLevel ,PublisherChannelID,CharityLevel ,CharityChannelID,CustomerType, DemoFlag,IsStrategic,AccountExecutiveID,AccountManagerID, customer_udf1,customer_udf2,customer_udf3,customer_udf4,customer_udf5, CreatedUserID,  IsDeleted) VALUES
(@baseChannelID, getdate(), @customerName,@activeFlag,@address,@city,@state, @country, @zip, @salutation,@contactName,@firstName,@lastName,@contactTitle,@email,@phone,@fax,@webAddress, @techContact, @techEmail, @techPhone, @subscriptionsEmail, @accountsLevel, @communicatorLevel,@communicatorChannelID, @collectorLevel, @collectorChannelID, @creatorLevel, @creatorChannelID, @publisherLevel, @publisherChannelID,@charityLevel, @charityChannelID,@CustomerType, @demoFlag,@IsStrategic,@AccountExecutiveID,@AccountManagerID, @customer_udf1,@customer_udf2,@customer_udf3,@customer_udf4,@customer_udf5, @UserID, 0);SELECT @@IDENTITY;";

            SqlCommand insert = new SqlCommand(sqlquery, conn);
            MapCustomerParameters(userID, insert);
            try
            {
                return Convert.ToInt32(insert.ExecuteScalar());
            }
            finally
            {
                insert.Dispose();
            }
        }

        private void SaveBillingContact(int customerID, Contact billingContact, SqlConnection conn, int userID)
        {
            if (DataFunctions.ExecuteScalar("accounts", "SELECT BillingContactID from [BillingContact] where CustomerID = " + customerID.ToString() + " and IsDeleted = 0") == null)
            {
                NewBillingContact(BillingContact, conn, userID);
                return;
            }

            UpdateBillingContact(BillingContact, conn, userID);
        }

        private void UpdateBillingContact(Contact billingContact, SqlConnection conn, int userID)
        {
            SqlCommand update = new SqlCommand(@"UPDATE [BillingContact]  SET CustomerID = @customerID, Salutation = @salutation, ContactName = @contactName,FirstName=@firstName, LastName=@lastName, ContactTitle = @contactTitle, Phone = @phone, Fax=@fax, Email=@email, StreetAddress=@streetAddress, City=@city, State = @state, Country = @country, Zip = @zip, UpdatedDate=GetDate(), UpdatedUserID=@UserID where CustomerID = " + ID.ToString(), conn);
            try
            {
                MapBillingContactParameters(userID, update);
                update.Prepare();
                update.ExecuteNonQuery();
            }
            finally
            {
                update.Dispose();
            }
        }

        private void NewBillingContact(Contact billingContact, SqlConnection conn, int userId)
        {
            var insert = new SqlCommand(
                "INSERT INTO [BillingContact] (" +
                "CustomerID, Salutation, ContactName, FirstName, LastName, ContactTitle, Phone, Fax, Email, " +
                "StreetAddress, City, State, Country, Zip, CreatedUserID, CreatedDate, IsDeleted) " +
                "VALUES (@customerID, @salutation, @contactName, @firstName, @lastName, @contactTitle, @phone, " +
                "@fax, @email, @streetAddress, @city, @state, @country, @zip, @UserID, GetDate(), 0);", 
                conn);
            try
            {
                MapBillingContactParameters(userId, insert);
                insert.Prepare();
                insert.ExecuteNonQuery();
            }
            finally
            {
                insert.Dispose();
            }
        }

        public void MapBillingContactParameters(int userId, SqlCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            MapBillingContactParametersBase(command);
            command.Parameters.Add("@UserID", SqlDbType.Int, 4).Value = userId;
        }

        #endregion

        #region Static Database Methods
        public static Customer GetCustomerByID(int customerID)
        {
            ArrayList customers = new ArrayList();
            DataTable dt = DataFunctions.GetDataTable(string.Format("SELECT * from [Customer] where CustomerID = {0} and IsDeleted = 0", customerID));
            if (dt.Rows.Count == 0)
            {
                return null;
            }

            return CreateCustomer(dt.Rows[0]);
        }

        public static ArrayList GetAllCustomersByChannelID(int baseChannelID)
        {
            ArrayList customers = new ArrayList();
            DataTable dt = DataFunctions.GetDataTable(string.Format("SELECT CustomerID,BaseChannelID,CustomerName,ActiveFlag, DemoFlag from [Customer] where BaseChannelID = {0} and ActiveFlag ='Y' and IsDeleted = 0 ORDER BY CustomerName", baseChannelID));
            foreach (DataRow c in dt.Rows)
            {
                customers.Add(CreateCustomerWithFewProperites(c));
            }
            return customers;
        }

        public static ArrayList GetAllCustomers()
        {
            ArrayList customers = new ArrayList();
            DataTable dt = DataFunctions.GetDataTable("SELECT * from [Customer] where ActiveFlag ='Y' and IsDeleted = 0 ORDER BY CustomerID");
            foreach (DataRow c in dt.Rows)
            {
                customers.Add(CreateCustomer(c));
            }
            return customers;
        }

        private static Customer CreateCustomer(DataRow row)
        {
            Customer customer = new Customer(Convert.ToInt32(row["CustomerID"]), Convert.ToInt32(row["BaseChannelID"]));
            customer.Name = Convert.ToString(row["CustomerName"]);
            customer.IsActive = Convert.ToString(row["ActiveFlag"]);
            customer.WebAddress = Convert.ToString(row["WebAddress"]);
            customer.TechContact = Convert.ToString(row["TechContact"]);
            customer.TechEmail = Convert.ToString(row["TechEmail"]);
            customer.TechPhone = Convert.ToString(row["TechPhone"]);
            customer.SubscriptionsEmail = Convert.ToString(row["SubscriptionsEmail"]);
            customer.CustomerType = Convert.ToString(row["CustomerType"]);
            customer.IsDemo = Convert.ToString(row["DemoFlag"]);
            customer.AccountLevel = row["AccountsLevel"] is DBNull ? 0 : Convert.ToInt32(row["AccountsLevel"]);
            customer.GeneralContact = new Contact(row["Salutation"].ToString(), row["ContactName"].ToString(), row["ContactTitle"].ToString(), row["Phone"].ToString(),
                row["Fax"].ToString(), row["Email"].ToString(), row["Address"].ToString(), row["City"].ToString(), row["State"].ToString(),
                row["Country"].ToString(), row["Zip"].ToString());

            customer.AccountExecutiveID = row.IsNull("AccountExecutiveID") ? 0 : Convert.ToInt32(row["AccountExecutiveID"]);
            customer.AccountManagerID = row.IsNull("AccountManagerID") ? 0 : Convert.ToInt32(row["AccountManagerID"]);
            customer.IsStrategic = row.IsNull("IsStrategic") ? "N" : (Convert.ToBoolean(row["IsStrategic"]) ? "Y" : "N");

            customer.customer_udf1 = Convert.ToString(row["customer_udf1"]);
            customer.customer_udf2 = Convert.ToString(row["customer_udf2"]);
            customer.customer_udf3 = Convert.ToString(row["customer_udf3"]);
            customer.customer_udf4 = Convert.ToString(row["customer_udf4"]);
            customer.customer_udf5 = Convert.ToString(row["customer_udf5"]);

            return customer;
        }

        private static Customer CreateCustomerWithFewProperites(DataRow row)
        {
            Customer customer = new Customer(Convert.ToInt32(row["CustomerID"]), Convert.ToInt32(row["BaseChannelID"]));
            customer.Name = Convert.ToString(row["CustomerName"]);
            customer.IsActive = Convert.ToString(row["ActiveFlag"]);
            customer.IsDemo = Convert.ToString(row["DemoFlag"]);
            return customer;
        }

        protected override Contact GetBillingContact(int customerId)
        {
            DataTable dt = DataFunctions.GetDataTable("SELECT * from [BillingContact] where CustomerID = " + customerId.ToString() + " and IsDeleted = 0");
            if (dt.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dt.Rows[0];
            return new Contact(row["Salutation"].ToString(), row["ContactName"].ToString(), row["ContactTitle"].ToString(), row["Phone"].ToString(),
                row["Fax"].ToString(), row["Email"].ToString(), row["StreetAddress"].ToString(), row["City"].ToString(), row["State"].ToString(),
                row["Country"].ToString(), row["Zip"].ToString());
        }

        public static void DeleteByCustomerID(int customerID, int userID)
        {
            //DataFunctions.Execute("UPDATE [Customer] set IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = {1} WHERE CustomerID = {0};
            StringBuilder sql = new StringBuilder();
            sql.Append(string.Format(@" UPDATE [Customer] set IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = {1} WHERE CustomerID = {0};
                                        UPDATE [BillingContact] set IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = {1} WHERE CustomerID = {0};
                                        UPDATE [Role] set IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = {1} WHERE CustomerID = {0};
                                        UPDATE [CustomerProduct] set IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = {1} WHERE CustomerID = {0};
                                        DELETE FROM [UserActions] where UserID in (select UserID from Users where CustomerID = {0});
                                        UPDATE [Users] set IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = {1} WHERE CustomerID = {0};", customerID, userID));
            DataFunctions.Execute(sql.ToString());

            // TODO: Need to delete other customer related data, quote, quote item, bill, bill item, License
        }
        #endregion
    }
}
