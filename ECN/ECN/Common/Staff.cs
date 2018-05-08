using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using ecn.common.classes.billing;

namespace ecn.common.classes
{
	[Flags]
	public enum StaffRoleEnum { 
		CustomerService = 1, 
		AccountExecutive = 2,
		AccountManager = 3,
		DemoManager = 4
		}

	public class Staff
	{
		public Staff(string firstName, string lastName, string email, StaffRoleEnum role)
		{
			_firstName = firstName;
			_lastName = lastName;
			_email = email;
			_role = role;
		}

		public Staff(int id) {
			_id = id;
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
			get { return _id <= 0;}
		}

		public string FullName
		{
			get { return FirstName + " " + LastName;}
		}

		private string _firstName = null;
		public string FirstName {
			get {
				if (!IsNew && _firstName == null) {
					_firstName = LoadedStaff.FirstName;
				}
				return (this._firstName);
			}
			set {
				this._firstName = value;
			}
		}

		private string _lastName = null;
		public string LastName {
			get {
				if (!IsNew && _lastName == null) {
					_lastName = LoadedStaff.LastName;
				}
				return (this._lastName);
			}
			set {
				this._lastName = value;
			}
		}

		private string _email = null;
		public string Email {
			get {
				if (!IsNew && _email == null) {
					_email = LoadedStaff.Email;
				}
				return (this._email);
			}
			set {
				this._email = value;
			}
		}

		private StaffRoleEnum _role;
		public StaffRoleEnum Role {
			get {				
				return (this._role);
			}
			set {
				this._role = value;
			}
		}

		private Staff _loadedStaff = null;
		protected Staff LoadedStaff {
			get {
				if (_loadedStaff == null) {
					_loadedStaff = Staff.GetStaffByID(_id);
					_role = _loadedStaff.Role;
				}
				return (this._loadedStaff);
			}			
		}

		public string FromEmailAddress {
			get {
				return string.Format("{0} {1} <{2}>", this.FirstName, this.LastName, this.Email);
			}
		}

//		public bool IsRole(StaffRoleEnum role) {
//			return (_roles&role) > 0;
//		}

		#region Database Methods
		
		/// At this point, insert only!
		
		public void Save() {
			SqlConnection conn = new SqlConnection(DataFunctions.GetConnectionString());
			SqlCommand insert = new SqlCommand("INSERT INTO Staff (FirstName, LastName, Email, Roles) VALUES (@firstName, @lastName, @email, @roles);", conn);
			insert.Parameters.Add("@firstName", SqlDbType.VarChar, 50).Value = FirstName;
			insert.Parameters.Add("@lastName", SqlDbType.VarChar, 50).Value = LastName;
			insert.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = Email;
			insert.Parameters.Add("@roles", SqlDbType.SmallInt, 2).Value = (Int16) Role;

			try {
				conn.Open();
				insert.Prepare();
				insert.ExecuteScalar();
			} finally {
				conn.Close();
				conn.Dispose();
				insert.Dispose();
			}
		}
		#endregion

		#region Static Database Methods
		public static ArrayList GetStaff() {
			DataTable dt = DataFunctions.GetDataTable("SELECT * from Staff order by FirstName, LastName");
			ArrayList staff = new ArrayList();
			foreach(DataRow row in dt.Rows) {
				staff.Add(GetStaff(row));
			}
			return staff;
		}

		public static ArrayList GetStaffByRole(StaffRoleEnum role) {
			ArrayList allStaff = GetStaff();
			ArrayList staff = new ArrayList();
			foreach(Staff s in allStaff) {
				if (s.Role == role) {
					staff.Add(s);
				}
			}
			return staff;
		}

//		public static ArrayList GetStaffByQuoteID(Quote quote) {
//			ArrayList nbds = new ArrayList();
//			DataTable dt = DataFunctions.GetDataTable(string.Format("SELECT NBDIDs from Quote where QuoteID = {0}",quote.QuoteID));
//			if (dt.Rows.Count == 0) {
//				return null;
//			}
//			string ids = Convert.ToString(dt.Rows[0]["NBDIDs"]);
//
//			DataTable staff = DataFunctions.GetDataTable(string.Format("SELECT * from Staff where StaffID in ({0})", ids));
//			
//			foreach (DataRow row in staff.Rows) {
//				nbds.Add(GetStaff(row));
//			}
//			return nbds;
//		}

		public static Staff GetStaffByID(int staffID) {
			DataTable dt = DataFunctions.GetDataTable("SELECT * from Staff where StaffID = " + staffID.ToString());
				
			if (dt.Rows.Count == 0) {
				return null;
			}
			
			return GetStaff(dt.Rows[0]);
		}

		
		public static Staff CurrentStaff {
			get {
				ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
				return GetStaffByUserID(Convert.ToInt32(sc.UserID()));
			}
		}
		
		/// Users and Staff are different entities. Users refer to ECN users while staff refer to staff for a business firm. One staff is associated with one user.
		
		/// <param name="userID"></param>
		/// <returns></returns>
		public static Staff GetStaffByUserID(int userID) {
			DataTable dt = DataFunctions.GetDataTable("SELECT * from Staff where userID = " + userID.ToString());
				
			if (dt.Rows.Count == 0) {
				return null;
			}			
			
			return GetStaff(dt.Rows[0]);
		}

		private static Staff GetStaff(DataRow row) {
			Staff staff = new Staff(Convert.ToString(row["FirstName"]), Convert.ToString(row["LastName"]),Convert.ToString(row["Email"]), (StaffRoleEnum) Convert.ToInt16(row["Roles"]));
			staff.ID = Convert.ToInt32(row["StaffID"]);
			return staff;
		}

		public bool IsRole(StaffRoleEnum role) 
		{
			return (_role == role);
		}
		#endregion
	}
}
