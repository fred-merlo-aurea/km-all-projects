using System;
using System.Data;
using System.Data.SqlClient;
using ecn.common.classes;
using ecn.wizard.webservice.Objects;

namespace ecn.wizard.webservice.Objects {
	/// <summary>
	/// Emails provides ways of adding and quering the emails table. It works with Groups to allow for attachment and deletion of
	/// email addresses.
	/// </summary>
	public class Emails : DatabaseAccessor {
		int _customer_id;
		string _email_address;
        
		/// <summary>
		/// Nullary constructor
		/// </summary>
		public Emails():base() { }

		/// <summary>
		/// ID constructor
		/// </summary>
		/// <param name="input_id">EmailID</param>
		public Emails(int input_id):base(input_id) {
        
		}
		/// <summary>
		/// String Constructor
		/// </summary>
		/// <param name="input_id">EmailID</param>
		public Emails(string input_id):base(input_id) {
		}


		#region Properties -- We may have two fields duplicated with Jt's code. No time to refactor it right now.
		private string _email;
		public string Email {
			get {
				return (this._email);
			}
			set {
				this._email = value;
			}
		}
		
		public int CustID {
			get {
				return (this._customer_id);
			}
			set {
				this._customer_id = value;
			}
		}

		private string _title;
		public string Title {
			get {
				return (this._title);
			}
			set {
				this._title = value;
			}
		}

		private string _firstName;
		public string FirstName {
			get {
				return (this._firstName);
			}
			set {
				this._firstName = value;
			}
		}

		private string _lastName;
		public string LastName {
			get {
				return (this._lastName);
			}
			set {
				this._lastName = value;
			}
		}

		private string _fullName;
		public string FullName {
			get {
				return (this._fullName);
			}
			set {
				this._fullName = value;
			}
		}

		private string _company;
		public string Company {
			get {
				return (this._company);
			}
			set {
				this._company = value;
			}
		}

		private string _occupation;
		public string Occupation {
			get {
				return (this._occupation);
			}
			set {
				this._occupation = value;
			}
		}

		private string _address;
		public string Address {
			get {
				return (this._address);
			}
			set {
				this._address = value;
			}
		}

		private string _address2;
		public string Address2 {
			get {
				return (this._address2);
			}
			set {
				this._address2 = value;
			}
		}

		private string _city;
		public string City {
			get {
				return (this._city);
			}
			set {
				this._city = value;
			}
		}

		private string _state;
		public string State {
			get {
				return (this._state);
			}
			set {
				this._state = value;
			}
		}

		private string _zip;
		public string Zip {
			get {
				return (this._zip);
			}
			set {
				this._zip = value;
			}
		}

		private string _country;
		public string Country {
			get {
				return (this._country);
			}
			set {
				this._country = value;
			}
		}

		private string _voice;
		public string Voice {
			get {
				return (this._voice);
			}
			set {
				this._voice = value;
			}
		}

		private string _mobile;
		public string Mobile {
			get {
				return (this._mobile);
			}
			set {
				this._mobile = value;
			}
		}

		private string _fax;
		public string Fax {
			get {
				return (this._fax);
			}
			set {
				this._fax = value;
			}
		}

		private string _website;
		public string Website {
			get {
				return (this._website);
			}
			set {
				this._website = value;
			}
		}

		private string _age;
		public string Age {
			get {
				return (this._age);
			}
			set {
				this._age = value;
			}
		}

		private string _income;
		public string Income {
			get {
				return (this._income);
			}
			set {
				this._income = value;
			}
		}

		private string _gender;
		public string Gender {
			get {
				return (this._gender);
			}
			set {
				this._gender = value;
			}
		}

		private string _user1;
		public string User1 {
			get {
				return (this._user1);
			}
			set {
				this._user1 = value;
			}
		}
		private string _user2;
		public string User2 {
			get {
				return (this._user2);
			}
			set {
				this._user2 = value;
			}
		}

		private string _user3;
		public string User3 {
			get {
				return (this._user3);
			}
			set {
				this._user3 = value;
			}
		}
		private string _user4;
		public string User4 {
			get {
				return (this._user4);
			}
			set {
				this._user4 = value;
			}
		}
		private string _user5;
		public string User5 {
			get {
				return (this._user5);
			}
			set {
				this._user5 = value;
			}
		}
		private string _user6;
		public string User6 {
			get {
				return (this._user6);
			}
			set {
				this._user6 = value;
			}
		}

		private DateTime _birthDate = DateTime.MinValue;
		public DateTime BirthDate {
			get {
				return (this._birthDate);
			}
			set {
				this._birthDate = value;
			}
		}

		private DateTime _userEvent1Date = DateTime.MinValue;
		public DateTime UserEvent1Date {
			get {
				return (this._userEvent1Date);
			}
			set {
				this._userEvent1Date = value;
			}
		}
		private string _userEvent1;
		public string UserEvent1 {
			get {
				return (this._userEvent1);
			}
			set {
				this._userEvent1 = value;
			}
		}
		private DateTime _userEvent2Date = DateTime.MinValue;
		public DateTime UserEvent2Date {
			get {
				return (this._userEvent2Date);
			}
			set {
				this._userEvent2Date = value;
			}
		}
		private string _userEvent2;
		public string UserEvent2 {
			get {
				return (this._userEvent2);
			}
			set {
				this._userEvent2 = value;
			}
		}

		private string _notes;
		public string Notes {
			get {
				return (this._notes);
			}
			set {
				this._notes = value;
			}
		}
		#endregion

		#region Static Database Method
		public static Emails GetEmailByID(int emailID) {
			if (emailID <=0) {
				return null;
			}

			DataTable dt = DataFunctions.GetDataTable("SELECT * from Emails where EmailID = " + emailID,DatabaseAccessor.con_communicator);
			if (dt.Rows.Count == 0) {
				return null;
			}	
			Emails email = new Emails(emailID);
			DataRow row = dt.Rows[0];
			email.Email = Convert.ToString(row["EmailAddress"]);
			email.CustID = Convert.ToInt32(row["CustomerID"]);
			email.Title = Convert.ToString(row["Title"]);
			email.FirstName = Convert.ToString(row["FirstName"]);
			email.LastName = Convert.ToString(row["LastName"]);
			email.FullName = Convert.ToString(row["FullName"]);
			email.Company = Convert.ToString(row["Company"]);
			email.Occupation = Convert.ToString(row["Occupation"]);
			email.Address = Convert.ToString(row["Address"]);
			email.Address2 = Convert.ToString(row["Address2"]);
			email.City = Convert.ToString(row["City"]);
			email.State = Convert.ToString(row["State"]);
			email.Zip = Convert.ToString(row["Zip"]);
			email.Country = Convert.ToString(row["Country"]);
			email.Voice = Convert.ToString(row["Voice"]);
			email.Mobile = Convert.ToString(row["Mobile"]);
			email.Fax = Convert.ToString(row["Fax"]);
			email.Website = Convert.ToString(row["Website"]);
			email.Age = Convert.ToString(row["Age"]);
			email.Income = Convert.ToString(row["Income"]);
			email.Gender = Convert.ToString(row["Gender"]);

			email.User1 = Convert.ToString(row["User1"]);
			email.User2 = Convert.ToString(row["User2"]);
			email.User3 = Convert.ToString(row["User3"]);
			email.User4 = Convert.ToString(row["User4"]);
			email.User5 = Convert.ToString(row["User5"]);
			email.User6 = Convert.ToString(row["User6"]);

			email.BirthDate = row["BirthDate"]== DBNull.Value? DateTime.MinValue:Convert.ToDateTime(row["birthDate"]);

			email.UserEvent1Date = row["userEvent1Date"]== DBNull.Value? DateTime.MinValue:Convert.ToDateTime(row["userEvent1Date"]);
			email.UserEvent1 = Convert.ToString(row["UserEvent1"]);
			email.UserEvent2Date = row["userEvent2Date"]== DBNull.Value? DateTime.MinValue:Convert.ToDateTime(row["userEvent2Date"]);
			email.UserEvent2 = Convert.ToString(row["UserEvent2"]);

			email.Notes = Convert.ToString(row["Notes"]);		

			return email;
		}
		
		#endregion

		/// <summary>
		/// Gets a prepared cursor to find emailIDs for customers (used by the import process)
		/// </summary>
		/// <returns>Prepared cursor for gettng emailid</returns>
		private SqlCommand SqlEmailAddressCustomerID() {
			SqlCommand get_cmd;
			get_cmd = new SqlCommand(null,GetDbConnection());
			get_cmd.CommandText = "Select EmailID from Emails where EmailAddress=@email_address and CustomerID=@customer_id";
            
			get_cmd.Parameters.Add ("@email_address",SqlDbType.VarChar,255,"EmailAddress").Value = DBNull.Value;
			get_cmd.Parameters.Add ("@customer_id", SqlDbType.Int,4,"CustomerID").Value = DBNull.Value;
			get_cmd.Prepare();

			return get_cmd;
		}
		/// <summary>
		/// Sets customer ID
		/// </summary>
		/// <param name="new_id">CustomerID</param>
		/// <returns>CustomerID</returns>
		public int CustomerID(int new_id) {
			_customer_id = new_id;
			return _customer_id;
		}
		/// <summary>
		/// Sets EmailAddresss
		/// </summary>
		/// <param name="eaddr">EmailAddresss</param>
		/// <returns>EmailAddresss</returns>
		public string EmailAddress(string eaddr) {
			_email_address = eaddr;
			return _email_address;
		}

		/// <summary>
		/// Gets the Email address from the Database
		/// </summary>
		/// <returns>EmailAddress</returns>
		public string EmailAddress() {
			// We should keep a prepaired handler around to get at this data fast I'm going with a simpler model for
			// rad development
			if(ID() != 0) {
				try {
					_email_address = DataFunctions.ExecuteScalar("Select EmailAddress from Emails where EmailID=" + ID()).ToString();
				} catch {

				}
			}
			return _email_address;
		}

		/// <summary>
		/// Returns the CustomerID from the Database
		/// </summary>
		/// <returns>CustomerID</returns>
		public int CustomerID() {
			// We should keep a prepaired handler around to get at this data fast I'm going with a simpler model for
			// rad development
			if(ID() != 0) {
				try {
					_customer_id = Convert.ToInt32(DataFunctions.ExecuteScalar("Select CustomerID from Emails where EmailID=" + ID()).ToString());
				} catch {
				}
			}
			return _customer_id;
		}

		/// <summary>
		/// Creats a new email from the forward to a friend object
		/// </summary>
		/// <param name="emailaddress"> Email to add</param>
		/// <param name="fullname">Fullname Entered</param>
		/// <param name="note">Our Note</param>
		/// <param name="CustomerID"> CustomerID to insert into</param>
		public void InsertEmailFromForward(string emailaddress, string fullname, string note, int CustomerID) {
			// Do a select to ensure they are not in here before.
			Emails my_email = new Emails();
			my_email.GetEmail(emailaddress,CustomerID);
			if(my_email.ID() != 0) {
				ID(my_email.ID());
				return;
			}
			string emailsQuery=
				" INSERT INTO Emails ( "+
				" EmailAddress, FullName, "+
				" CustomerID, Notes "+
				" ) VALUES ( "+
				" '"+emailaddress+"', '"+fullname+"', "+
				CustomerID + ", '"+ note + "'" +
				" ); SELECT @@IDENTITY; ";
			int theEmailID=Convert.ToInt32(DataFunctions.ExecuteScalar(emailsQuery));
			ID(theEmailID);
		}

		/// <summary>
		/// Inserts an DemoEmail from the demo object... These functions need a real constructor ala the LayoutPlans Create and Update
		/// for DB access
		/// </summary>
		/// <param name="email_address"></param>
		/// <param name="first_name"></param>
		/// <param name="last_name"></param>
		/// <param name="company"></param>
		/// <param name="phone_number"></param>
		/// <param name="live_demo"></param>
		/// <param name="test_acct"></param>
		/// <param name="ip_addr"></param>
		public void InsertDemoEmail (string email_address, string first_name, string last_name, string company, string phone_number, string live_demo, string test_acct, string ip_addr) {
			Emails my_email = new Emails();
			my_email.GetEmail(email_address,1);

			if(my_email.ID() != 0) {
				ID(my_email.ID());
				return;
			}
           
			SqlCommand insert_cmd = new SqlCommand(null,GetDbConnection());
			insert_cmd.CommandText = "INSERT into Emails (EmailAddress,CustomerIDFirstName,LastName,Company,Voice,User1,User2,User3) VALUES " +
				"(@email_address,1,@first_name,@last_name,@company,@voice,@user1,@user2,@user3);Select @@IDENTITY";
            
			insert_cmd.Parameters.Add ("@email_address",SqlDbType.VarChar,255,"EmailAddress").Value = email_address;
			insert_cmd.Parameters.Add ("@first_name",SqlDbType.VarChar,50,"FirstName").Value = first_name;
			insert_cmd.Parameters.Add ("@last_name",SqlDbType.VarChar,50,"LastName").Value = last_name;
			insert_cmd.Parameters.Add ("@company",SqlDbType.VarChar,50,"Company").Value = company;
			insert_cmd.Parameters.Add ("@voice",SqlDbType.VarChar,50,"Voice").Value = phone_number;
			insert_cmd.Parameters.Add ("@user1",SqlDbType.VarChar,50,"User1").Value = live_demo;
			insert_cmd.Parameters.Add ("@user2",SqlDbType.VarChar,50,"User2").Value = test_acct;
			insert_cmd.Parameters.Add ("@user3",SqlDbType.VarChar,50,"User3").Value = ip_addr;
			insert_cmd.Prepare();
			insert_cmd.ExecuteScalar().ToString();
			ID(Convert.ToInt32(insert_cmd.ExecuteScalar().ToString()));
            
		}

		/// <summary>
		/// Checks to see if an email belongs to a customer. Updates self if so.
		/// </summary>
		/// <param name="email_address">Email Address</param>
		/// <param name="customer_id">Customer</param>
		public void GetEmail(string email_address,int customer_id) {

			object eid = DataFunctions.ExecuteScalar("communicator",
				"Select EmailID from Emails where EmailAddress = '"+ email_address +"' and CustomerID = "+customer_id);

			int new_id = 0;
			if(null != eid) {
				new_id = Convert.ToInt32(eid);
			}
			ID(new_id);
            
		}

		/// <summary>
		/// Ensures that an email address exists in the database.
		/// </summary>
		/// <returns>number of created emails (1 or 0)</returns>
		public int EnsureEmail() {
			GetEmail(_email_address,_customer_id);
			if(ID() == 0) {
				SqlCommand insert_cmd = new SqlCommand(null,GetDbConnection());
				insert_cmd.CommandText = "Insert INTO Emails (EmailAddress,CustomerID) VALUES (@email_address,@customer_id);SELECT @@IDENTITY";
            
				insert_cmd.Parameters.Add ("@email_address",SqlDbType.VarChar,255,"EmailAddress").Value = EmailAddress();
				insert_cmd.Parameters.Add ("@customer_id", SqlDbType.Int,4,"CustomerID").Value = CustomerID();
				insert_cmd.Prepare();
				int new_id = Convert.ToInt32(insert_cmd.ExecuteScalar());
				ID(new_id);
				return 1;
			}
			return 0;
		}

		// This method DOESN'T save all the columns right now. Please add new columns to both Insert() and Update();
		public void Save() {
			GetEmail(_email, _customer_id);
			if (ID() == 0) {
				Insert();
				return;
			}

			Update();
		}

		public void SetValue(string columnName, SqlDbType dbtype, int length, object val) {
			if (ID() == 0) {
				throw new ApplicationException("Can't set value for email when email ID is 0.");
			}
			string updateSql = string.Format("Update Emails set {0}=@{0} where EmailID = @emailID", columnName);
			SqlConnection conn = GetDbConnection("communicator");
			SqlCommand update_cmd = null;
			try {
				update_cmd = new SqlCommand(updateSql,conn);            			            				
				update_cmd.Parameters.Add("@"+columnName, dbtype, length, "columnName").Value = val;
				update_cmd.Parameters.Add("@emailID", SqlDbType.Int, 4, "EmailID").Value = ID();
				conn.Open();
				update_cmd.Prepare();
				update_cmd.ExecuteNonQuery();
			} finally {
				conn.Close();
				conn.Dispose();
				if (update_cmd != null) {
					update_cmd.Dispose();
				}
			}
		}
		private void Insert() {
			string insertSql = "Insert INTO Emails (EmailAddress,CustomerID, FirstName, LastName,Company, Voice, Notes, User1, User6, UserEvent1Date, UserEvent2Date) VALUES (@email_address,@customer_id, @firstName,@lastName, @company, @voice, @notes, @user1, @user6,@userEvent1Date,@userEvent2Date);SELECT @@IDENTITY";
			SqlConnection conn = GetDbConnection("communicator");
			SqlCommand insert_cmd = null;
			
			try {
				insert_cmd = new SqlCommand(insertSql,conn);            			            
				insert_cmd.Parameters.Add ("@email_address",SqlDbType.VarChar,255,"EmailAddress").Value = Email;
				insert_cmd.Parameters.Add ("@customer_id", SqlDbType.Int,4,"CustomerID").Value = CustID;
				insert_cmd.Parameters.Add("@firstname", SqlDbType.VarChar, 50, "FirstName").Value = FirstName;
				insert_cmd.Parameters.Add("@lastName", SqlDbType.VarChar, 50, "LastName").Value = LastName;
				insert_cmd.Parameters.Add("@company", SqlDbType.VarChar, 100, "Company").Value = Company;
				insert_cmd.Parameters.Add("@voice", SqlDbType.VarChar, 50, "Voice").Value = Voice;
				insert_cmd.Parameters.Add("@notes", SqlDbType.Text, 1000, "Notes").Value = Notes;
				insert_cmd.Parameters.Add("@user1", SqlDbType.VarChar, 255, "User1").Value = User1;
				insert_cmd.Parameters.Add("@user6", SqlDbType.VarChar, 255, "User6").Value = User6;
				if (UserEvent1Date==DateTime.MinValue) {
					insert_cmd.Parameters.Add("@userEvent1Date", SqlDbType.DateTime, 8, "userEvent1Date").Value = DBNull.Value;
				} else {
					insert_cmd.Parameters.Add("@userEvent1Date", SqlDbType.DateTime, 8, "userEvent1Date").Value =UserEvent1Date;
				}
				if (UserEvent2Date==DateTime.MinValue) {
					insert_cmd.Parameters.Add("@userEvent2Date", SqlDbType.DateTime, 8, "userEvent2Date").Value = DBNull.Value;
				} else {
					insert_cmd.Parameters.Add("@userEvent2Date", SqlDbType.DateTime, 8, "userEvent2Date").Value = UserEvent2Date;
				}

				conn.Open();
				insert_cmd.Prepare();
				int new_id = Convert.ToInt32(insert_cmd.ExecuteScalar());
				ID(new_id);
			} finally {
				conn.Close();
				conn.Dispose();
				if (insert_cmd != null) {
					insert_cmd.Dispose();
				}
			}
		}

		private void Update() {
			string updateSql = "Update Emails set EmailAddress=@email_address,CustomerID=@customer_id,FirstName=@firstName, LastName=@lastName,Company=@company, Voice=@voice, Notes=@notes, User1=@user1, User6=@user6, UserEvent1Date=@userEvent1Date,UserEvent2Date=@userEvent2Date where EmailID = @emailID";
			SqlConnection conn = GetDbConnection("communicator");
			SqlCommand update_cmd = null;
			try {
				update_cmd = new SqlCommand(updateSql,conn);            			            
				update_cmd.Parameters.Add ("@email_address",SqlDbType.VarChar,255,"EmailAddress").Value = Email;
				update_cmd.Parameters.Add ("@customer_id", SqlDbType.Int,4,"CustomerID").Value = CustID;
				update_cmd.Parameters.Add("@firstname", SqlDbType.VarChar, 50, "FirstName").Value = FirstName;
				update_cmd.Parameters.Add("@lastName", SqlDbType.VarChar, 50, "LastName").Value = LastName;
				update_cmd.Parameters.Add("@company", SqlDbType.VarChar, 100, "Company").Value = Company;
				update_cmd.Parameters.Add("@voice", SqlDbType.VarChar, 50, "Voice").Value = Voice;
				update_cmd.Parameters.Add("@notes", SqlDbType.Text, 1000, "Notes").Value = Notes;
				update_cmd.Parameters.Add("@user1", SqlDbType.VarChar, 255, "User1").Value = User1;
				update_cmd.Parameters.Add("@user6", SqlDbType.VarChar, 255, "User6").Value = User6;
				if (UserEvent1Date==DateTime.MinValue) {
					update_cmd.Parameters.Add("@userEvent1Date", SqlDbType.DateTime, 8, "userEvent1Date").Value = DBNull.Value;
				} else {
					update_cmd.Parameters.Add("@userEvent1Date", SqlDbType.DateTime, 8, "userEvent1Date").Value =UserEvent1Date;
				}
				if (UserEvent2Date==DateTime.MinValue) {
					update_cmd.Parameters.Add("@userEvent2Date", SqlDbType.DateTime, 8, "userEvent2Date").Value = DBNull.Value;
				} else {
					update_cmd.Parameters.Add("@userEvent2Date", SqlDbType.DateTime, 8, "userEvent2Date").Value = UserEvent2Date;
				}
				update_cmd.Parameters.Add("@emailID", SqlDbType.Int, 4, "EmailID").Value = ID();
				conn.Open();
				update_cmd.Prepare();
				update_cmd.ExecuteNonQuery();
			} finally {
				conn.Close();
				conn.Dispose();
				if (update_cmd != null) {
					update_cmd.Dispose();
				}
			}
		}
	}
}
