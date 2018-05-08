using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ecn.common.classes;

namespace ecn.wizard.webservice.Objects {
	/// <summary>
	/// TODO: User is only used for create new user for quote prove process.
	/// It's a starting place to put more user-related logics.
	/// </summary>
	public class User {
		public User(){}

		public User(string userName, string password, Customer customer) {
			_userName = userName;
			_password = password;
			_customer = customer;
			_createDate = DateTime.Now;
		}

		private int _id = -1;
		public int ID {
			get {
				return (this._id);
			}
			set {
				this._id = value;
			}
		}

		public bool IsNew {
			get { return _id == -1;}
		}

		private Customer _customer;
		public Customer Customer {
			get {
				return (this._customer);
			}
			set {
				this._customer = value;
			}
		}

		private string _userName;
		public string UserName {
			get {
				return (this._userName);
			}
			set {
				this._userName = value;
			}
		}

		private string _password;
		public string Password {
			get {
				return (this._password);
			}
			set {
				this._password = value;
			}
		}

		public string CommunicatorOptions {
			get { return "000";}
		}

		public string CollectorOptions {
			get { return "000";}
		}

		public string CreatorOptions {
			get { return "000";}
		}

		private string _accountsOptions = "000000";
		public string AccountsOptions {
			get {
				return (this._accountsOptions);
			}
			set {
				this._accountsOptions = value;
			}
		}

		private bool _isActive = true;
		public bool IsActive {
			get {
				return (this._isActive);
			}
			set {
				this._isActive = value;
			}
		}

		private DateTime _createDate;
		public DateTime CreateDate {
			get {
				return (this._createDate);
			}
			set {
				this._createDate = value;
			}
		}

		// Don't think role ID is still in use. Just keep the existing logic.
		private int _roleID = -1;
		public int RoleID {
			get {
				return (this._roleID);
			}
			set {
				this._roleID = value;
			}
		}
		
		public int GetTopOneRoleID() {
			string sqlQuery=
				" SELECT top 1 RoleID FROM Roles WHERE CustomerID=" + this.Customer.ID.ToString();
			return Convert.ToInt32( DataFunctions.ExecuteScalar(sqlQuery));
		}

		public bool UserNameExists() {
			String sqlQuery=
				" SELECT count(*) "+
				" FROM Users "+
				" WHERE UserName = '"+ this.UserName.ToString() +"' and CustomerID = "+ this.Customer.ID.ToString();

			int count = Convert.ToInt32(DataFunctions.ExecuteScalar(sqlQuery));
			return count>0;
		}

		#region Database methods
		public bool Exists {
			get {
				string sql = string.Format(@"if exists(select * from Users join customer on users.customerID = customer.customerID where basechannelID = {0} and userName = '{1}' and customer.CustomerID = {2}) 
select 1
else
select 0", this.Customer.BaseChannelID, this.UserName, this.Customer.ID);// -- 	
			
				return Convert.ToBoolean(DataFunctions.ExecuteScalar(sql));
			}
		}

		public int getUserID() {
			string sql = string.Format(@"SELECT UserID FROM Users WHERE UserName = '{0}'", this.UserName, this.Customer.ID);
			int uID = 0;
			try{
				uID = Convert.ToInt32(DataFunctions.ExecuteScalar(sql).ToString());
			}catch{
			
			}
			return uID;
		}

		public void Save() {
			if (Exists) {
				return;
			}
			if (IsNew) {
				SqlConnection conn = new SqlConnection(DataFunctions.GetConnectionString());
				SqlCommand insert = new SqlCommand(@"INSERT INTO Users (CustomerID, UserName, Password, CommunicatorOptions, CollectorOptions, CreatorOptions, AccountsOptions, ActiveFlag, CreateDate, RoleID) VALUES 
(@customerID, @userName, @password, @communicatorOptions, @collectorOptions, @creatorOptions, @AccountsOptions, @activeFlag, @createDate, @roleID);SELECT @@IDENTITY",conn);
				
				try {
					conn.Open();
					insert.Parameters.Add("@customerID", SqlDbType.Int, 4).Value = this.Customer.ID;
					insert.Parameters.Add("@userName", SqlDbType.VarChar, 100).Value = this.UserName;
					insert.Parameters.Add("@password", SqlDbType.VarChar, 50).Value = this.Password;
					insert.Parameters.Add("@communicatorOptions", SqlDbType.VarChar, 50).Value = this.CommunicatorOptions;
					insert.Parameters.Add("@collectorOptions", SqlDbType.VarChar, 50).Value = this.CollectorOptions;
					insert.Parameters.Add("@creatorOptions", SqlDbType.VarChar, 50).Value = this.CreatorOptions;
					insert.Parameters.Add("@accountsOptions", SqlDbType.VarChar, 50).Value = this.AccountsOptions;
					insert.Parameters.Add("@activeFlag", SqlDbType.VarChar, 1).Value = IsActive?"Y":"N";
					insert.Parameters.Add("@createDate", SqlDbType.DateTime, 8).Value = CreateDate;
					insert.Parameters.Add("@roleID", SqlDbType.Int, 4).Value = RoleID;					
					insert.Prepare();

					ID = Convert.ToInt32(insert.ExecuteScalar());
					EnableAllActions();
				} finally {
					insert.Dispose();
					conn.Close();
					conn.Dispose();
				}
				return;
			}

			throw new ApplicationException("Update user operation is not implemented yet.");
		}

		private void EnableAllActions() {
			DataTable dt = DataFunctions.GetDataTable("select ActionID from Action");
			StringBuilder sql = new StringBuilder();
			foreach (DataRow dr in dt.Rows) {
				sql.Append(string.Format("INSERT into UserActions (UserID,ActionID,Active) values ( {0}, {1}, 'Y');", this.ID, dr["ActionID"]) );
			}
			common.classes.DataFunctions.Execute(sql.ToString());
		}

		#endregion
	}
}
