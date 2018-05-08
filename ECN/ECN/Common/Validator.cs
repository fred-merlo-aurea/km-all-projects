using System;

namespace ecn.common.classes {
	
	public class Validator {
		public Validator() {
		}
		public static bool IsValidUser(string username, string password) {
			string userQuery = "SELECT COUNT(*) FROM Users WHERE" +
				" UserName     = '" + username + "'" +
				" AND Password = '" + password + "';";
			bool isValid = Convert.ToInt32(DataFunctions.ExecuteScalar(userQuery)) > 0;
			return isValid;
		}
		/*
		 * int IsAuthorized(int customerID, string product)
		 * see if customer is authorized to access a product
		 * args: int customerID
		 *       string product
		 * returns: true if customer is authorized to use product
		 *          else false
		 */ 
		public static bool IsAuthorized(int customerID, string product) {
			int authLevel       = 0;
			string productField = "";
			string sqlQuery     = "";
			switch(product) {
				case "communicator":
					productField = "CommunicatorLevel";
					break;
				case "collector":
					productField = "CollectorLevel";
					break;
				case "creator":
					productField = "CreatorLevel";
					break;
			}
			sqlQuery = "SELECT " + productField + " FROM ecn_accounts.dbo.Customer WHERE customerID = " + customerID;
			try {
				authLevel = Convert.ToInt32(DataFunctions.ExecuteScalar(sqlQuery));
			}
			catch ( FormatException fe ) {
				string devnull = fe.ToString();
				authLevel = 0;
			}
			catch (Exception ex){
				string devnull = ex.ToString();
				authLevel = 0;			
			}
			return authLevel > 0;
		}
	}
}
