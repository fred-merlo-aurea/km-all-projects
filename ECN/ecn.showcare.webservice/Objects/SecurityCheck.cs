using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;
using System.Web.Security;

namespace ecn.showcare.webservice.Objects {
	/// <summary>
	/// Summary description for SecurityCheck.
	/// </summary>
	public class SecurityCheck {

		char[] secOptionsArray;
		char[] customerLevelsArray;
		public static string accountsdb=ConfigurationManager.AppSettings["accountsdb"];

		public SecurityCheck(){
			//Empty Constructor
		}

		//get UserID
		public string UserID() {
			string theUserID="";
			try {
				theUserID=getCookie().Name;
			}
			catch (Exception e) {
				string devnull=e.ToString();
			}
			return theUserID;
		}

		//get CustomerID
		public string CustomerID () {
			string theCustomerID="";
			try {
				string[] myIDs = getCookie().UserData.Split(',');
				theCustomerID=myIDs[0];
			}
			catch (Exception e) {
				string devnull=e.ToString();
				theCustomerID="";
			}
			return theCustomerID;
		}
		
		//get the BaseChannelID  as ChannelID
		public string ChannelID() {
			string theChannelID="";
			try {
				string[] myIDs = getCookie().UserData.Split(',');
				theChannelID=myIDs[1];
			}
			catch (Exception e) {
				string devnull=e.ToString();
				theChannelID="";
			}
			return theChannelID;
		}
		
		//get CommunicatorChannelID  for the Customer
		public string CommunicatorChannelID() {
			string commChannelID="";
			char[] channelArray;
			try {
				string[] myIDs = getCookie().UserData.Split(',');
				commChannelID=myIDs[2];
				channelArray = commChannelID.ToCharArray();
				commChannelID = channelArray[0].ToString();
			}
			catch (Exception e) {
				string devnull=e.ToString();
				commChannelID="0";
			}
			return commChannelID;
		}

		//get CollectorChannelID  for the Customer
		public string CollectorChannelID() {
			string collChannelID="";
			char[] channelArray;
			try {
				string[] myIDs = getCookie().UserData.Split(',');
				collChannelID=myIDs[2];
				channelArray = collChannelID.ToCharArray();
				collChannelID = channelArray[1].ToString();
			}
			catch (Exception e) {
				string devnull=e.ToString();
				collChannelID="0";
			}
			return collChannelID;
		}

		//get CollectorChannelID  for the Customer
		public string CreatorChannelID() {
			string crtrChannelID="";
			char[] channelArray;
			try {
				string[] myIDs = getCookie().UserData.Split(',');
				crtrChannelID=myIDs[2];
				channelArray = crtrChannelID.ToCharArray();
				crtrChannelID = channelArray[2].ToString();
			}
			catch (Exception e) {
				string devnull=e.ToString();
				crtrChannelID="0";
			}
			return crtrChannelID;
		}

		//get Sec Options
		public string SecurityOptions() {
			string theSecurityOptions="";
			try {
				string[] myIDs = getCookie().UserData.Split(',');
				theSecurityOptions=myIDs[3];
			}
			catch (Exception e) {
				string devnull=e.ToString();
				theSecurityOptions="";
			}
			return theSecurityOptions;
		}

		// Product Feature check
		public bool hasProductFeature(string product_name,string feature_name) {
			try {
				string customer_id = CustomerID();
				string product_id = DataFunctions.ExecuteScalar("select ProductID from " + accountsdb + ".dbo.Product where ProductName = '" + product_name + "'").ToString();
                string product_detail_id = DataFunctions.ExecuteScalar("select ProductDetailID from " + accountsdb + ".dbo.ProductDetail where ProductID = " + product_id + " and ProductDetailName='" + feature_name + "'").ToString();  
				string sql_stmt = "select Active from " + accountsdb + ".dbo.CustomerProduct where CustomerID = " + customer_id + " and ProductDetailID = " + product_detail_id;
				string my_answer = DataFunctions.ExecuteScalar(sql_stmt).ToString();
				if(my_answer.Equals("y")) {
					return true;
				}
				return false;
			} 
			catch (Exception ) {
				return false;
			}
		}

		/// <summary>
		/// Ensures that a user has the proper permissions to use a particular feature
		/// If something goes wrong, we allow by default
		/// </summary>
		/// <param name="product_name"> The ECN product line</param>
		/// <param name="action_code"> The action they want to perform</param>
		/// <returns></returns>
		public bool CanDoAction(string product_name,string action_code) {
			try {  
				string product_id = DataFunctions.ExecuteScalar("select ProductID from " + accountsdb + ".dbo.Products where ProductName = '" + product_name + "'").ToString();
				string action_id =  DataFunctions.ExecuteScalar("select ActionID from " + accountsdb + ".dbo.Action where ProductID = " + product_id + " AND ActionCode = '" + action_code + "'").ToString();
				object ret_value = DataFunctions.ExecuteScalar("select Active from " + accountsdb + ".dbo.UserActions where UserID=" + UserID() + " AND ActionID=" + action_id );
				if(null == ret_value) return true;
				string active = ret_value.ToString();
				if(active == "Y") return true;

				return false;
			} catch (Exception ) {
				return true;
			}
		}

		// each Character of  "theSecurityOptions" represents the security options as follows:
		//
		// Accounts Options				Communicator Options		Collector Options			CreatorOptions
		//-----------------				---------------------		----------------			---------------
		// char 0 - Sys Admin			char 6 - Group Prev			char 9 - Prev 1			char 12 - Prev 1
		// char 1 - Channel Admin		char 7 - Content Prev		char 10 - Prev 2			char 13 - Prev 2
		// char 2 - Admin					char 8 - Blast Prev			char 11 - Prev 3			char 14 - Prev 3
		// char 3 - User
		// char 4 - Customer
		// char 5 - Channel


		//Sys Admin 
		public bool CheckSysAdmin(){
			secOptionsArray = SecurityOptions().ToCharArray();
			bool check = false;
			if(secOptionsArray[0] == '1'){
				check = true;
			}else {
				check = false;
			}
			return check;
		}
		//Channel Admin
		public bool CheckChannelAdmin(){
			secOptionsArray = SecurityOptions().ToCharArray();
			bool check = false;
			if(secOptionsArray[1] == '1'){
				check = true;
			}else {
				check = false;
			}		
			return check;
		}
		//Admin
		public bool CheckAdmin(){
			secOptionsArray = SecurityOptions().ToCharArray();
			bool check = false;
			if(secOptionsArray[2] == '1'){
				check = true;
			}else {
				check = false;
			}		
			return check;
		}
		//Group
		public bool CheckGroupPrevilage(){
			return CanDoAction("ecn.communicator","grouppriv");
			//			secOptionsArray = SecurityOptions().ToCharArray();
			//			bool check = false;
			//			if(secOptionsArray[6] == '1'){
			//				check = true;
			//			}else {
			//				check = false;
			//			}		
			//			return check;
		}
		//Content
		public bool CheckContentPrevilage(){
			return CanDoAction("ecn.communicator","contentpriv");
			//			secOptionsArray = SecurityOptions().ToCharArray();
			//			bool check = false;
			//			if(secOptionsArray[7] == '1'){
			//				check = true;
			//			}else {
			//				check = false;
			//			}		
			//			return check;
		}
		//Blast
		public bool CheckBlastPrevilage(){
			return CanDoAction("ecn.communicator","blastpriv");
			//			secOptionsArray = SecurityOptions().ToCharArray();
			//			bool check = false;
			//			if(secOptionsArray[8] == '1'){
			//				check = true;
			//			}else {
			//				check = false;
			//			}		
			//			return check;
		}


		//get the Customer Levels
		public string CustomerLevel() {
			string theCustomerLevel="";
			try {
				string[] myIDs = getCookie().UserData.Split(',');
				theCustomerLevel=myIDs[4];
			}
			catch (Exception e) {
				string devnull=e.ToString();
				theCustomerLevel="";
			}
			return theCustomerLevel;
		}

		// each Character of  "theCustomerLevel" represents the customerLevels as follows:
		//
		// Communicator Level			Collector Level					Creator Level				Account Level
		//-------------------			-------------------			--------------				---------------
		// char 0 -  1 - silver /			char 1 - Collector Prev		char 2 - Prev 1			char 3 - Prev 1
		//				2 - gold /
		//				3 - platinum
		// Example Check for Product levels: 
		// if getXXXLevel() > 0{
		//		<!-- customer is authorized -->
		// }else { <!-- not authorized -->}
		//Communicator Level
		public string getCommunicatorLevel(){
			customerLevelsArray = CustomerLevel().ToCharArray();
			return customerLevelsArray[0].ToString();
		}

		//Collector Level
		public string getCollectorLevel(){
			customerLevelsArray = CustomerLevel().ToCharArray();
			return customerLevelsArray[1].ToString();
		}

		//Creator Level
		public string getCreatorLevel(){
			customerLevelsArray = CustomerLevel().ToCharArray();
			return customerLevelsArray[2].ToString();
		}

		//Accounts Level
		public string getAccountsLevel(){
			customerLevelsArray = CustomerLevel().ToCharArray();
			return customerLevelsArray[3].ToString();
		}

		public FormsAuthenticationTicket getCookie() {
			FormsAuthenticationTicket ticket = null;
			if (HttpContext.Current.User != null) {
				if (HttpContext.Current.User.Identity.IsAuthenticated) {
					if (HttpContext.Current.User.Identity is FormsIdentity) {
						System.Web.Security.FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
						ticket = id.Ticket;
						string AuthData = ticket.UserData;
						string AuthName = ticket.Name;
						string AuthTime = ticket.IssueDate.ToString();
						string[] myIDs = AuthData.Split(',');
					}
				}
			}
			return ticket;
		}

		public string GetHashedPass(String aPassword) {
			return FormsAuthentication.HashPasswordForStoringInConfigFile(aPassword,"sha1");
		}

	}


	public class SecurityAccess {

		static bool taint = false;
		public SecurityAccess() {
            
		}
		public static void enableTaint(){
			taint = true;
		}
		public static void canI(string type, string row_id) {

			if( !hasAccess(type,row_id) && !taint)
				throw new SecurityException("SECURITY VIOLATION!");

		} 
      
		public static bool hasAccess( string type, string row_id) {
			SecurityCheck sc = new SecurityCheck();
			string cust_id = sc.CustomerID();
			string base_channel_id = sc.ChannelID();
			try {
				switch(type) {
					case "Emails":
						return canAccessEmailID(row_id,cust_id);
					case "EmailDataValues":
						return canAccessEmailDataValuesID(row_id,cust_id);
					case "Groups":
						return canAccessGroupID(row_id,cust_id);
					case "Content":
						return canAccessContentID(row_id,cust_id);
					case "Blasts":
						return canAccessBlastID(row_id,cust_id);
					case "FiltersDetails":
						return canAccessFiltersDetailsID(row_id,cust_id);
					case "Filters":
						return canAccessFilterID(row_id,cust_id);
					case "ContentFiltersDetails":
						return canAccessContentFiltersDetailsID(row_id,cust_id);
					case "ContentFilters":
						return canAccessContentFilterID(row_id,cust_id);
					case "Layouts":
						return canAccessLayoutID(row_id,cust_id);
					case "Folders":
						return canAccessFolderID(row_id,cust_id);
					case "Survey":
						return canAccessSurveyID(row_id,cust_id);
					case "Events":
						return canAccessEventID(row_id,cust_id);
					case "Menus":
						return canAccessMenuID(row_id,cust_id);
					case "Pages":
						return canAccessPageID(row_id,cust_id);
					case "Templates":
						return canAccessTemplateID(row_id,cust_id);
					case "HeaderFooters":
						return canAccessHeaderFooterID(row_id,cust_id);
					case "Users":
						return canAccessUserID(row_id,cust_id);
					case "Customers":
						return canAccessCustomerID(row_id,base_channel_id);
					case "CustomerTemplates":
						return canAccessCustomerTemplateID(row_id,base_channel_id);
					case "CustomerLicenses":
						return canAccessCustomerLicenseID(row_id,base_channel_id);
					case "BaseChannel":
						return row_id.Equals(base_channel_id);
					case "Channels":
						return canAccessChannelID(row_id,base_channel_id);
				}
			} catch (Exception ){
				// throw e;
				throw new SecurityException("ID Does Not Exist!");                
			}
			return false;
		}
        
		private static bool canAccessEmailID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("communicator","SELECT CustomerID from Emails where EmailID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}

		private static bool canAccessEmailDataValuesID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("communicator","SELECT e.CustomerID from Emails e, EmailDataValues ed where e.EmailID = ed.EmailID AND ed.EmailDataValuesID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}

		private static bool canAccessGroupID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("communicator","SELECT CustomerID from Groups where GroupID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}

		private static bool canAccessContentID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("communicator","SELECT CustomerID from Content where ContentID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}

		private static bool canAccessBlastID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("communicator","SELECT CustomerID from Blasts where BlastID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}
		private static bool canAccessFilterID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("communicator","SELECT CustomerID from Filters where FilterID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}
		private static bool canAccessFiltersDetailsID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("communicator","SELECT f.CustomerID from Filters f, FiltersDetails fd where f.FilterID = fd.FilterID AND fd.FDID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}
		private static bool canAccessContentFilterID(string row_id, string cust_id) {
			//            throw new Exception("cfid=" + row_id);
			string good_cust_id = DataFunctions.ExecuteScalar("communicator","SELECT l.CustomerID from ContentFilters f, Layouts l where l.LayoutID = f.LayoutID AND f.FilterID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}
		private static bool canAccessContentFiltersDetailsID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("communicator","SELECT l.CustomerID from ContentFilters f, ContentFiltersDetails fd, Layouts l where f.FilterID = fd.FilterID AND f.layoutID = l.layoutID AND fd.FDID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}
		private static bool canAccessLayoutID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("communicator","SELECT CustomerID from Layouts where LayoutID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}
		private static bool canAccessFolderID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("communicator","SELECT CustomerID from Folders where FolderID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}
		private static bool canAccessSurveyID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("collector","SELECT customer_id from survey where survey_id = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}
		private static bool canAccessEventID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("creator","SELECT CustomerID from Events where EventID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}
		private static bool canAccessMenuID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("creator","SELECT CustomerID from Menus where MenuID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}
		private static bool canAccessPageID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("creator","SELECT CustomerID from Pages where PageID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}
		private static bool canAccessHeaderFooterID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("creator","SELECT CustomerID from HeaderFooters where HeaderFooterID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}
		private static bool canAccessTemplateID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("creator","SELECT CustomerID from Templates where TemplateID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}
		private static bool canAccessUserID(string row_id, string cust_id) {
			string good_cust_id = DataFunctions.ExecuteScalar("accounts","SELECT CustomerID from Users where UserID = " + row_id).ToString();
			return good_cust_id.Equals(cust_id);
		}
		private static bool canAccessCustomerID(string row_id, string base_channel_id) {
			string good_base_channel_id = DataFunctions.ExecuteScalar("accounts","SELECT BaseChannelID from Customer where CustomerID = " + row_id).ToString();
			return good_base_channel_id.Equals(base_channel_id);
		}
		private static bool canAccessCustomerTemplateID(string row_id, string base_channel_id) {
			string good_base_channel_id = DataFunctions.ExecuteScalar("accounts","SELECT c.BaseChannelID from Customer c, CustomerTemplate ct where c.CustomerID = ct.CustomerID AND ct.CTID=" + row_id).ToString();
			return good_base_channel_id.Equals(base_channel_id);
		}
		private static bool canAccessCustomerLicenseID(string row_id, string base_channel_id) {
			string good_base_channel_id = DataFunctions.ExecuteScalar("accounts","SELECT c.BaseChannelID from Customer c, CustomerLicense cl where c.CustomerID = cl.CustomerID AND cl.CLID=" + row_id).ToString();
			return good_base_channel_id.Equals(base_channel_id);
		}
		private static bool canAccessChannelID(string row_id, string base_channel_id) {
			string good_base_channel_id = DataFunctions.ExecuteScalar("accounts","SELECT BaseChannelID from Channel where ChannelID=" + row_id).ToString();
			return good_base_channel_id.Equals(base_channel_id);
		}
	}

	public class SecurityException : ApplicationException {
		// Default constructor
		public SecurityException () {
		}
		// Constructor accepting a single string message
		public SecurityException (string message) : base(message) {
		}
		// Constructor accepting a string message and an inner exception 
		// that will be wrapped by this custom exception class
		public SecurityException(string message, Exception inner) : base(message, inner) {
		}
	}
}
