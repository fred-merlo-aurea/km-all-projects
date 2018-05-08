using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;

namespace ecn.common.classes {
	public class AccountData {
		/*
		 * int GetCustomerID(int userID)
		 * get the customer id for a user
		 * args: int userID 
		 * returns: this user's customerID
		 */
		public static int GetCustomerID(int userID) {
			string sqlQuery = "SELECT CustomerID FROM ecn_accounts.dbo.Users WHERE UserID = "+userID;
			return Convert.ToInt32(DataFunctions.ExecuteScalar(sqlQuery));
		}
		/*
		 * int GetCustomerName(int customerID)
		 * get the name of a customer
		 * args: int customerID 
		 * returns: this customer's name
		 */
		public static string GetCustomerName(int customerID) {
			string sqlQuery = "SELECT CustomerName FROM ecn_accounts.dbo.Customer WHERE CustomerID = "+customerID;
			return DataFunctions.ExecuteScalar(sqlQuery).ToString();
		}
	}
}
