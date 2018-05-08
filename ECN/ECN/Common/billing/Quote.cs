using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.WebControls;
using ecn.common.classes;
using ECN_Framework_Common.Objects;

namespace ecn.common.classes.billing {
	public enum QuoteStatusEnum { Pending, Approved, Denied}

	public class Quote {
		public Quote(int qid) : this(qid, null) {}
		public Quote(Customer customer): this(-1, customer) {}

		public Quote(int qid, Customer customer) {
			_quoteID = qid;
			Customer = customer;	
			_items = new QuoteItemCollection();
		}

		#region Properites
		private int _quoteID;
		public int QuoteID {
			get {
				return (this._quoteID);
			}
			set {
				this._quoteID = value;
			}
		}		

		private int _channelID = 0;
		public int ChannelID {
			get {
				if (!Customer.IsNew) {
					_channelID = Customer.BaseChannelID;
					return _channelID;
				}

				if (!IsNew &&  _channelID ==0) {
					_channelID = LoadedQuote.ChannelID;
				}
				return (this._channelID);
			}
			set {
				this._channelID = value;
			}
		}


		private Customer _customer;
		public Customer Customer {
			get {
				if (!IsNew && _customer == null) {
					_customer = LoadedQuote.Customer;
				}
				return (this._customer);
			}
			set {
				this._customer = value;
			}
		}

		private QuoteItemCollection _items;
		public QuoteItemCollection Items {
			get {
				return this._items;
			}			
		}

        private DateTime _createdDate;
        public DateTime CreatedDate
        {
			get {
                if (!IsNew && _createdDate == DateTime.MinValue)
                {
                    _createdDate = LoadedQuote.CreatedDate;
				}
                return (this._createdDate);
			}
			set {
                this._createdDate = value;
                UpdatedDate = _createdDate;
			}
		}

        private DateTime _updatedDate;
        public DateTime UpdatedDate
        {
            get
            {
                if (!IsNew && _updatedDate == DateTime.MinValue)
                {
                    _updatedDate = LoadedQuote.UpdatedDate;
                }
                return (this._updatedDate);
            }
            set
            {
                this._updatedDate = value;
            }
        }

		private DateTime _approveDate = DateTimeInterpreter.MaxValue;
		public DateTime ApproveDate {
			get {				
				return (this._approveDate);
			}
			set {
				this._approveDate = value;
			}
		}


		private DateTime _startDate;
		public DateTime StartDate {
			get {
				if (!IsNew && _startDate == DateTime.MinValue) {
					_startDate = LoadedQuote.StartDate;
				}
				return (this._startDate);
			}
			set {
				this._startDate = value;
			}
		}
	
		private string _salutation;
		public string Salutation {
			get {
				if (!IsNew && _salutation == null) {
					_salutation = LoadedQuote.Salutation;
				}
				return (this._salutation);
			}
			set {
				this._salutation = value;
			}
		}

		private string _firstName;
		public string FirstName {
			get {
				if (!IsNew && _firstName == null) {
					_firstName = LoadedQuote.FirstName;
				}
				return (this._firstName);
			}
			set {
				this._firstName = value;
			}
		}

		private string _lastName;
		public string LastName {
			get {
				if (!IsNew && _lastName == null) {
					_lastName = LoadedQuote.LastName;
				}
				return (this._lastName);
			}
			set {
				this._lastName = value;
			}
		}

		private string _email;
		public string Email {
			get {
				if (!IsNew && _email == null) {
					_email = LoadedQuote.Email;
				}
				return (this._email);
			}
			set {
				this._email = value;
			}
		}

		private string _phone;
		public string Phone {
			get {
				if (!IsNew && _phone == null) {
					_phone = LoadedQuote.Phone;
				}
				return (this._phone);
			}
			set {
				this._phone = value;
			}
		}

		private string _fax;
		public string Fax {
			get {
				if (!IsNew && _fax == null) {
					_fax = LoadedQuote.Fax;
				}
				return (this._fax);
			}
			set {
				this._fax = value;
			}
		}

		private string _company;
		public string Company {
			get {
				if (!IsNew && _company == null) {
					_company = LoadedQuote.Company;
				}
				return (this._company);
			}
			set {
				this._company = value;
			}
		}


		private string _billType;
		public string BillType {
			get {
				if (!IsNew && _billType == null) {
					_billType = LoadedQuote.BillType;
				}
				return (this._billType);
			}
			set {
				this._billType = value;
			}
		}

		private string _notes;
		public string Notes {
			get {
				if (!IsNew && _notes == null) {
					_notes = LoadedQuote.Notes;
				}
				return (this._notes);
			}
			set {
				this._notes = value;
			}
		}

		private string _internalNotes;
		public string InternalNotes {
			get {
				if (!IsNew && _internalNotes == null) {
					_internalNotes = LoadedQuote.InternalNotes;
				}
				return (this._internalNotes);
			}
			set {
				this._internalNotes = value;
			}
		}

        private int _CreatedUserID = -1;
        public int CreatedUserID
        {
            get
            {
                if (!IsNew && _CreatedUserID == -1)
                {
                    _CreatedUserID = LoadedQuote.CreatedUserID;
                }
                return (this._CreatedUserID);
            }
            set
            {
                this._CreatedUserID = value;
            }
        }

		private int _AccountManagerID = -1;
		public int AccountManagerID 
		{
			get 
			{
				if (!IsNew && _AccountManagerID == -1) 
				{
					_AccountManagerID = LoadedQuote.AccountManagerID;
				}
				return (this._AccountManagerID);
			}
			set 
			{
				this._AccountManagerID = value;
			}
		}

		private QuoteStatusEnum _status = QuoteStatusEnum.Pending;
		public QuoteStatusEnum Status {
			get {				
				return (this._status);
			}
			set {
				if (_status != value) {
                    UpdatedDate = DateTime.Now;

					if (value == QuoteStatusEnum.Approved) {
						ApproveDate = DateTime.Now;
					}
				}	
				this._status = value;
			}
		}

		public bool HasTestAccount {
			get { return TestUserName != string.Empty;}
		}

		private string _testUserName=string.Empty;
		public string TestUserName {
			get {
				return (this._testUserName);
			}
			set {
				this._testUserName = value;
			}
		}

		private string _testPassword=string.Empty;
		public string TestPassword {
			get {
				return (this._testPassword);
			}
			set {
				this._testPassword = value;
			}
		}

		#endregion

		#region Derived Propetries	
		public string QuoteCode {
			get {
				return string.Format("Q{0}_{1}_{2}{3}{4}_{5}",Customer.BaseChannelID.ToString("0000"), Customer.ID.ToString("000000"),CreatedDate.Month.ToString("00"), CreatedDate.Day.ToString("00"), CreatedDate.Year, QuoteID==-1?"New":QuoteID.ToString()) ;
			}		
		}

		public string CustomerName {
			get {
				if (Customer.ID == -1) {
					return "New Customer";
				}

				return Customer.Name;
			}
		}
		public bool IsNew {
			get { return _quoteID <=0 ;}
		}

		public bool HasAnnualTechItem {
			get {
				foreach(QuoteItem item in Items) {
					if (item.LicenseType == LicenseTypeEnum.AnnualTechAccess) {
						return true;
					}
				}
				return false;
			}
		}

		
		public double AnnualTotal {
			get {
				return GetTotalByFrequency(FrequencyEnum.Annual);
			}		
		}

		
		public double QuarterTotal {
			get {
				return GetTotalByFrequency(FrequencyEnum.Quarterly);
			}			
		}
		
		public double MonthTotal {
			get {
				return GetTotalByFrequency(FrequencyEnum.Monthly);
			}			
		}

		
		public double OneTimeTotal {
			get {
				return GetTotalByFrequency(FrequencyEnum.OneTime);
			}			
		}
		
		public double AnnualTotalSaving {
			get {
				return GetTotalSavingByFrequency(FrequencyEnum.Annual);
			}		
		}

		
		public double QuarterTotalSaving {
			get {
				return GetTotalSavingByFrequency(FrequencyEnum.Quarterly);
			}			
		}
		
		public double MonthTotalSaving {
			get {
				return GetTotalSavingByFrequency(FrequencyEnum.Monthly);
			}			
		}

		
		public double OneTimeTotalSaving {
			get {
				return GetTotalSavingByFrequency(FrequencyEnum.OneTime);
			}			
		}

		public double Total {
			get { return OneTimeTotal + AnnualTotal + MonthTotal + QuarterTotal;}
		}

		private double GetTotalByFrequency(FrequencyEnum frequency) {
			double total = 0;
			foreach(QuoteItem item in Items) {
				if (item.Frequency == frequency && !item.IsCustomerCredit) {
					total += item.ActualItemPrice;
				}
			}
			return total;
		}

		private double GetTotalSavingByFrequency(FrequencyEnum frequency) {
			double total = 0;
			foreach(QuoteItem item in Items) {
				if (item.Frequency == frequency && !item.IsCustomerCredit) {
					total += item.SavedTotal;
				}
			}
			return total;
		}
		#endregion

		public void AddItem(QuoteItem quoteItem) {
			if (quoteItem == null) {
				return;
			}

			if (quoteItem.LicenseType != LicenseTypeEnum.AnnualTechAccess) {
				quoteItem.Parent = this;
				_items.Add(quoteItem);
				return;
			}
			
			if (Customer.HasAnnualTechItem || HasAnnualTechItem) {
				throw new InvalidOperationException(string.Format("Only one annual tech quote item is allowed."));
                //throwInvalidOperationException("Only one annual tech quote item is allowed.");
			}		

			quoteItem.Parent = this;
			_items.Add(quoteItem);			
		}



		public void AddItems(QuoteItemCollection items) {
		    if (items != null)
		    {
                foreach (QuoteItem item in items)
                {
                    AddItem(item);
                }
		    }
		}

		public QuoteItemCollection GetQuoteItemsByType(LicenseTypeEnum licenseType) {
			QuoteItemCollection items = new QuoteItemCollection();
			foreach(QuoteItem item in this.Items) {
				if (item.LicenseType == licenseType) {
					items.Add(item);
				}
			}
			return items;
		}


        public Bill CreateBill(IBillingHistoryManager billingHistoryManager, DateTime billCreatedDate)
        {
			if (this.Status == QuoteStatusEnum.Denied || this.Status == QuoteStatusEnum.Pending) {
				return null;
			}
			Bill bill = new Bill(this.Customer);
            bill.CreatedDate = billCreatedDate;
			bill.Quote = this;
			foreach(QuoteItem item in this.Items) {
				if (item.IsActive) {
                    bill.AddItem(item.CreateBillItem(billingHistoryManager.GetLatestBillItem(item), billCreatedDate));
				}
			}
			return bill;
		}
		
		#region Database Methods

		private Quote _loadedQuote = null;
		private Quote LoadedQuote {
			get {
				if (_loadedQuote == null) {
					_loadedQuote = Quote.GetQuoteByID(_quoteID);
					_status = _loadedQuote.Status;
					_approveDate = _loadedQuote.ApproveDate;
				}
				return _loadedQuote;
			}
		}
		public void Save() {			
			SqlConnection connection = new SqlConnection(DataFunctions.GetConnectionString());

			if (_quoteID == -1) {
				// Insert;				
				string insertSql = @"INSERT INTO [Quote] (CustomerID, ChannelID, CreatedDate, UpdatedDate, ApproveDate, StartDate, Salutation, FirstName, LastName, Email, Phone, Fax, Company, BillType, CreatedUserID, AccountManagerID, Status, Notes, TestUserName, TestPassword, InternalNotes, IsDeleted) 
					Values (@customerID, @channelID, @createdDate, @UpdatedDate,@approveDate, @startDate, @salutation, @firstName, @lastName, @email, @phone, @fax,@company, @billType, @CreatedUserID, @AccountManagerID, @status, @notes, @testUserName, @testPassword, @internalNotes, 0);select @@identity";
				SqlCommand insertCommand = new SqlCommand(insertSql, connection);
				try 
                {
					connection.Open();
					insertCommand.Parameters.Add("@customerID",SqlDbType.Int, 4).Value = this.Customer.ID;
					insertCommand.Parameters.Add("@channelID",SqlDbType.Int, 4).Value = this.ChannelID;
                    insertCommand.Parameters.Add("@createdDate", SqlDbType.DateTime, 8).Value = this.CreatedDate;
                    insertCommand.Parameters.Add("@UpdatedDate", SqlDbType.DateTime, 8).Value = this.UpdatedDate;
					insertCommand.Parameters.Add("@approveDate", SqlDbType.DateTime,8).Value = this.ApproveDate;
					insertCommand.Parameters.Add("@startDate", SqlDbType.DateTime,8).Value = this.StartDate;
					insertCommand.Parameters.Add("@salutation", SqlDbType.VarChar,10).Value = this.Salutation;
					insertCommand.Parameters.Add("@firstName", SqlDbType.VarChar,50).Value = this.FirstName;
					insertCommand.Parameters.Add("@LastName", SqlDbType.VarChar,50).Value = this.LastName;
					insertCommand.Parameters.Add("@email", SqlDbType.VarChar,100).Value = this.Email;
					insertCommand.Parameters.Add("@phone", SqlDbType.VarChar,50).Value = this.Phone;
					insertCommand.Parameters.Add("@fax", SqlDbType.VarChar,50).Value = this.Fax;
					insertCommand.Parameters.Add("@company", SqlDbType.VarChar,50).Value = this.Company;
					insertCommand.Parameters.Add("@billType", SqlDbType.VarChar, 20).Value = this.BillType;
                    insertCommand.Parameters.Add("@CreatedUserID", SqlDbType.VarChar, 20).Value = this.CreatedUserID;
                    insertCommand.Parameters.Add("@AccountManagerID", SqlDbType.VarChar, 20).Value = (this.AccountManagerID == -1 ? 0 : this.AccountManagerID);
					insertCommand.Parameters.Add("@status", SqlDbType.SmallInt, 2).Value = (Int16) this.Status;
					insertCommand.Parameters.Add("@notes", SqlDbType.Text, 2000).Value = this.Notes;					
					insertCommand.Parameters.Add("@testUserName", SqlDbType.VarChar,100).Value = this.TestUserName;
					insertCommand.Parameters.Add("@testPassword", SqlDbType.VarChar,20).Value = this.TestPassword;
					insertCommand.Parameters.Add("@internalNotes", SqlDbType.Text, 2000).Value = this.InternalNotes;		
					insertCommand.Prepare();				
					
					QuoteID = Convert.ToInt32(insertCommand.ExecuteScalar());
                    SaveQuoteItems(connection, this.CreatedUserID);
				} 
                finally 
                {
					connection.Close();
					insertCommand.Dispose();
				}
				return;
			}

			string updateSql = @"UPDATE [Quote] Set customerID = @CustomerID,ChannelID=@channelID, 
				UpdatedDate = @UpdatedDate, ApproveDate = @approveDate, StartDate = @startDate, 
				Salutation = @salutation, FirstName = @firstName, LastName = @lastName, Email = @email,
				Phone = @phone, Fax = @fax, Company = @company,
				BillType = @billType, CreatedUserID=@CreatedUserID, UpdatedUserID=@CreatedUserID, AccountManagerID=@AccountManagerID, Notes = @notes, Status = @status, InternalNotes = @internalNotes where QuoteID = @QuoteID";

			
			SqlCommand updateCommand = new SqlCommand(updateSql, connection);
			try 
            {
				connection.Open();
				updateCommand.Parameters.Add("@customerID",SqlDbType.Int, 4).Value = this.Customer.ID;			
				updateCommand.Parameters.Add("@channelID",SqlDbType.Int, 4).Value = this.ChannelID;
                updateCommand.Parameters.Add("@UpdatedDate", SqlDbType.DateTime, 8).Value = this.UpdatedDate;
				updateCommand.Parameters.Add("@approveDate", SqlDbType.DateTime,8).Value = this.ApproveDate;
				updateCommand.Parameters.Add("@startDate", SqlDbType.DateTime,8).Value = this.StartDate;
				updateCommand.Parameters.Add("@salutation", SqlDbType.VarChar,10).Value = this.Salutation;
				updateCommand.Parameters.Add("@firstName", SqlDbType.VarChar,50).Value = this.FirstName;
				updateCommand.Parameters.Add("@LastName", SqlDbType.VarChar,50).Value = this.LastName;
				updateCommand.Parameters.Add("@email", SqlDbType.VarChar,100).Value = this.Email;
				updateCommand.Parameters.Add("@phone", SqlDbType.VarChar,50).Value = this.Phone;
				updateCommand.Parameters.Add("@fax", SqlDbType.VarChar,50).Value = this.Fax;
				updateCommand.Parameters.Add("@company", SqlDbType.VarChar,50).Value = this.Company;
				updateCommand.Parameters.Add("@billType", SqlDbType.VarChar, 20).Value = this.BillType;
                updateCommand.Parameters.Add("@CreatedUserID", SqlDbType.VarChar, 20).Value = this.CreatedUserID;
                updateCommand.Parameters.Add("@AccountManagerID", SqlDbType.VarChar, 20).Value = (this.AccountManagerID == -1 ? 0 : this.AccountManagerID);
				updateCommand.Parameters.Add("@notes", SqlDbType.Text, 2000).Value = this.Notes;
				updateCommand.Parameters.Add("@status", SqlDbType.SmallInt, 2).Value = (Int16) this.Status;
				updateCommand.Parameters.Add("@internalNotes", SqlDbType.Text, 2000).Value = this.InternalNotes;
				updateCommand.Parameters.Add("@quoteID", SqlDbType.Int,4).Value = this.QuoteID;
				updateCommand.Prepare();	
				
				updateCommand.ExecuteNonQuery();
                SaveQuoteItems(connection, this.CreatedUserID);
			} 
            finally 
            {
				connection.Close();
				updateCommand.Dispose();
			}			
		}


		public void Delete() 
        {
            DataFunctions.Execute(string.Format("UPDATE [QuoteItem] set IsDeleted = 1, UpdatedDate=GETDATE(), UpdatedUserID={1} where QuoteID = {0}; UPDATE [Quote] set IsDeleted = 1, UpdatedDate=GETDATE(), UpdatedUserID={1} where QuoteID = {0}", this.QuoteID, this.CreatedUserID));
		}
		private void SaveQuoteItems(SqlConnection connection, int userID) 
        {
			foreach(QuoteItem item in this.Items) 
            {
				item.Save(connection, userID);
			}
		}

		#endregion

		#region Static Database Methods
		public static Quote GetQuoteByID(int quoteID) {
			DataTable dt = DataFunctions.GetDataTable("SELECT * from [Quote] where QuoteID = " + quoteID.ToString() + " and IsDeleted = 0");
			if (dt.Rows.Count == 0) {
				return null;
			}
		
			int customerID = Convert.ToInt32( dt.Rows[0]["CustomerID"] );
			Customer customer = new Customer(customerID);

			return GetQuote(dt.Rows[0],customer);				
		}		

		public static ArrayList GetQuotesByCustomerID(int customerID) {
			ArrayList quotes = new ArrayList();
			if (customerID == -1) {
				return quotes;
			}
            DataTable dt = DataFunctions.GetDataTable("SELECT * from [Quote] where CustomerID = " + customerID.ToString() + " and IsDeleted = 0");
			Customer customer = new Customer(customerID);
			foreach(DataRow row in dt.Rows) {				
				quotes.Add(GetQuote(row, customer));
			}
			return quotes;
		}

		public static ArrayList GetQuotesByWhereClause(string whereClause) {
			ArrayList quotes = new ArrayList();
            DataTable dt = DataFunctions.GetDataTable("SELECT * from [Quote] where " + whereClause + " and IsDeleted = 0");			
			foreach(DataRow row in dt.Rows) {	
				int quoteID = Convert.ToInt32(row["QuoteID"]);
				quotes.Add(new Quote(quoteID));
			}
			return quotes;
		}

		public static ArrayList GetQuotesByBillType(string billType) {
            DataTable dt = DataFunctions.GetDataTable(string.Format("SELECT quoteID from [Quote] where BillType = '{0}' and IsDeleted = 0", billType));
			ArrayList quotes = new ArrayList();
			foreach(DataRow row in dt.Rows) {
				quotes.Add(new Quote(Convert.ToInt32(row["quoteID"])));
			}
			return quotes;
		}

		public static ArrayList GetApprovedQuotes() {
            DataTable dt = DataFunctions.GetDataTable(string.Format("SELECT quoteID from [Quote] where status = {0} and IsDeleted = 0", (int)QuoteStatusEnum.Approved));
			ArrayList quotes = new ArrayList();
			foreach(DataRow row in dt.Rows) {
				quotes.Add(new Quote(Convert.ToInt32(row["quoteID"])));
			}
			return quotes;
		}

//		public static int GetQuoteNumberWith90DaysByStaffID(int staffID) {
//			string sql = string.Format("select count(*) from Quotes where (CreateDate > getdate() - 90) AND (NBDIDs = '{0}' OR NBDIDs like '%,{0}' or NBDIDs like '{0},%')", staffID);
//			return Convert.ToInt32(DataFunctions.ExecuteScalar(sql));
//		}

        public static int GetQuoteNumberByStaffIDAndDateRange(int CreatedUserID, DateTime start, DateTime end)
        {
            string sql = string.Format("select count(*) from [Quote] where (CreatedDate >= '{1}' or UpdatedDate >= '{1}') AND (CreatedDate <='{2}' or UpdatedDate <='{2}') AND (CreatedUserID = {0} or UpdatedUserID = {0}) and IsDeleted = 0", CreatedUserID, start.ToShortDateString(), end.ToShortDateString());
			return Convert.ToInt32(DataFunctions.ExecuteScalar(sql));
		}

        public static ArrayList GetQuotesByStaffID(int CreatedUserID)
        {
            DataTable dt = DataFunctions.GetDataTable(string.Format("select * from [Quote] where (CreatedUserID = {0} or IsNull(UpdatedUserID,0) = {0}) and IsDeleted = 0", CreatedUserID));
			ArrayList quotes = new ArrayList();
			foreach(DataRow row in dt.Rows) {
				quotes.Add(new Quote(Convert.ToInt32(row["quoteID"])));
			}
			return quotes;
		}

		private static Quote GetQuote(DataRow row, Customer parent) {			
			Customer customer = parent;
			Quote quote = new Quote(customer);
			quote.QuoteID = Convert.ToInt32( row["QuoteID"] );
			quote.ChannelID = Convert.ToInt32(row["ChannelID"]);
			quote.Status = (QuoteStatusEnum) Convert.ToInt16(row["Status"]);
			quote.CreatedDate = Convert.ToDateTime( row["CreatedDate"] );
			quote.UpdatedDate = Convert.ToDateTime( row["UpdatedDate"] );
			quote.ApproveDate = Convert.ToDateTime( row["ApproveDate"] );
			quote.StartDate = Convert.ToDateTime( row["StartDate"] );
			quote.Salutation = Convert.ToString( row["Salutation"] );
			quote.FirstName = Convert.ToString( row["FirstName"] );
			quote.LastName = Convert.ToString( row["LastName"] );
			quote.Email = Convert.ToString( row["Email"] );
			quote.Phone = Convert.ToString( row["Phone"] );
			quote.Fax = Convert.ToString( row["Fax"] );
			quote.Company = Convert.ToString( row["Company"] );
			quote.BillType = Convert.ToString(row["BillType"]);
			quote.Notes = Convert.ToString(row["Notes"]);			
			quote.InternalNotes = Convert.ToString(row["InternalNotes"]);		
			quote.TestUserName = Convert.ToString(row["TestUserName"]);
			quote.TestPassword = Convert.ToString(row["TestPassword"]);
            quote.CreatedUserID = row.IsNull("CreatedUserID") ? 0 : Convert.ToInt32(row["CreatedUserID"]);
			quote.AccountManagerID = row.IsNull("AccountManagerID")?0:Convert.ToInt32( row["AccountManagerID"]);

			return quote;
		}

		#endregion
	}	
}

