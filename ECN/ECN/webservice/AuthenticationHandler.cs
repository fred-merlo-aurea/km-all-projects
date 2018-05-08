using System;
using System.Configuration;
using System.Data;
using ecn.common.classes;
using ecn.accounts.classes;

namespace ecn.webservice.classes
{
	/// <summary>
	/// Summary description for LoginHandler.
	/// </summary>
	public class AuthenticationHandler {
        #region Getters & Setters
        public string _ecnAccessKey;
        public string ecnAccessKey { get { return _ecnAccessKey; } set { _ecnAccessKey = value; } }

        public int _userID;
        public int userID { get { return _userID; } set { _userID = value; } }

        public int _customerID;
        public int customerID { get { return _customerID; } set { _customerID = value; } }

        public int _baseChannelID;
        public int baseChannelID { get { return _baseChannelID; } set { _baseChannelID = value; } }

        public Customer Customer;


        private string accountsOptions;

        #endregion

        public AuthenticationHandler(string accessKey) {
            ecnAccessKey = accessKey;
        }

		public bool authenticateUser(){
            try {
                string sql = " SELECT c.BaseChannelID, u.CustomerID, c.CustomerName, u.UserID, u.AccountsOptions,c.CommunicatorChannelID,c.CollectorChannelID,c.CreatorChannelID,c.PublisherChannelID,c.CharityChannelID,c.CommunicatorLevel,c.CollectorLevel,c.CreatorLevel,c.PublisherLevel,c.CharityLevel,c.AccountsLevel " +
                    " FROM Users u JOIN Customer c ON u.CustomerID = c.CustomerID " +
                    " WHERE u.AccessKey = '" + ecnAccessKey+"'";
                try {
                    DataTable dt = DataFunctions.GetDataTable(sql, ConfigurationManager.AppSettings["act"].ToString());
                    baseChannelID = Convert.ToInt32(dt.Rows[0]["BaseChannelID"].ToString());
                    customerID = Convert.ToInt32(dt.Rows[0]["CustomerID"].ToString());
                    userID = Convert.ToInt32(dt.Rows[0]["UserID"].ToString());
                    accountsOptions = dt.Rows[0]["AccountsOptions"].ToString();

                    Customer = Customer.GetCustomerByID(customerID);

                    return true;
                } catch (Exception){
                    return false;
                }
            } catch {
                return false;
            }
		}

        public bool isSysAdmin()
        {
            char[] chars = accountsOptions.ToCharArray();
            if (chars[0].ToString().Equals("1"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isChannelAdmin()
        {
            char[] chars = accountsOptions.ToCharArray();
            if (chars[1].ToString().Equals("1"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isAdmin()
        {
            char[] chars = accountsOptions.ToCharArray();
            if (chars[2].ToString().Equals("1"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
	}
}
