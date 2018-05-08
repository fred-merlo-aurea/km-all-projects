using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ecn.common.classes
{
	
	/// TODO: User is only used for create new user for quote prove process.
	/// It's a starting place to put more user-related logics.
	
	public class User
	{
        public User()
        {
        }

		public User(string userName, string password, Customer customer)
		{
			_userName = userName;
			_password = password;
			_customer = customer;
			_createdDate = DateTime.Now;
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

		private DateTime _createdDate;
		public DateTime CreatedDate {
			get {
				return (this._createdDate);
			}
			set {
				this._createdDate = value;
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
				" SELECT top 1 RoleID FROM Role WHERE CustomerID=" + this.Customer.ID.ToString();
            return Convert.ToInt32(DataFunctions.ExecuteScalar("accounts", sqlQuery));
		}

        public int GetEverythingRoleID()
        {
            string sqlQuery =
                " SELECT top 1 RoleID FROM Role WHERE CustomerID=" + this.Customer.ID.ToString() + " AND RoleName = 'Everything'";
            return Convert.ToInt32(DataFunctions.ExecuteScalar("accounts", sqlQuery));
        }

        // removed condition for checking the useraccount with the customer.(should be global) - 01/30/2008
		public bool UserNameExists() {
            int count = Convert.ToInt32(DataFunctions.ExecuteScalar("accounts", " SELECT count(*) FROM Users WHERE UserName = '"+ this.UserName.ToString() +"'"));
			return count>0;
		}

        // removed condition for checking the useraccount with the customer.(should be global) - 01/30/2008
		#region Database methods
		public bool Exists {
			get {
				string sql = string.Format(@"if exists(select * from Users where userName = '{0}')
select 1
else
select 0", this.UserName);
                return Convert.ToBoolean(DataFunctions.ExecuteScalar("accounts", sql));
			}
		}

		public void Save() {
			if (Exists) {
				return;
			}
			if (IsNew) {
				SqlConnection conn = new SqlConnection(DataFunctions.GetConnectionString("accounts"));
				SqlCommand insert = new SqlCommand(@"INSERT INTO Users (CustomerID, UserName, Password, CommunicatorOptions, CollectorOptions, CreatorOptions, AccountsOptions, ActiveFlag, CreatedDate, RoleID) VALUES 
(@customerID, @userName, @password, @communicatorOptions, @collectorOptions, @creatorOptions, @AccountsOptions, @activeFlag, @createdDate, @roleID);SELECT @@IDENTITY",conn);
				
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
					insert.Parameters.Add("@createdDate", SqlDbType.DateTime, 8).Value = CreatedDate;
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

		private void EnableAllActions() 
        {
            DataFunctions.Execute("accounts", string.Format("INSERT into UserActions (UserID,ActionID,Active) select {0}, ActionID, Active from RoleAction where RoleID = {1};", this.ID, RoleID));
		}

		#endregion
	}
}
