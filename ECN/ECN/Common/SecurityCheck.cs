//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Web;
//using System.Configuration;
//using System.Web.Security;

//namespace ecn.common.classes {
	
//    public class SecurityCheck {

//        char[] secOptionsArray;
//        char[] customerLevelsArray;
//        public static string accountsdb=ConfigurationManager.AppSettings["accountsdb"];

//        public SecurityCheck(){
//        }

//        //get UserID
//        public string UserID()
//        {
//            try
//            {
//                return getCookie().Name;
//            }
//            catch
//            {
//                return string.Empty;
//            }
//        }

//        //get CustomerID
//        public string CustomerID()
//        {
//            try
//            {
//                string[] myIDs = getCookie().UserData.Split(',');
//                return myIDs[0];
//            }
//            catch
//            {
//                return string.Empty;
//            }
//        }
		
//        //get the BaseChannelID  as ChannelID
//        public string ChannelID()
//        {
//            try
//            {
//                string[] myIDs = getCookie().UserData.Split(',');
//                return myIDs[1];
//            }
//            catch
//            {
//                return string.Empty;
//            }
//        }
		
//        //get CommunicatorChannelID  for the Customer
//        public string CommunicatorChannelID() {
//            string commChannelID="";
//            char[] channelArray;
//            try {
//                string[] myIDs = getCookie().UserData.Split(',');
//                commChannelID=myIDs[2];
//                channelArray = commChannelID.ToCharArray();
//                commChannelID = channelArray[0].ToString();
//            }
//            catch  {
				
//                commChannelID="0";
//            }
//            return commChannelID;
//        }

//        //get CollectorChannelID  for the Customer
//        public string CollectorChannelID() {
//            string collChannelID="";
//            char[] channelArray;
//            try {
//                string[] myIDs = getCookie().UserData.Split(',');
//                collChannelID=myIDs[2];
//                channelArray = collChannelID.ToCharArray();
//                collChannelID = channelArray[1].ToString();
//            }
//            catch  {
				
//                collChannelID="0";
//            }
//            return collChannelID;
//        }

//        //get CollectorChannelID  for the Customer
//        public string CreatorChannelID() {
//            string crtrChannelID="";
//            char[] channelArray;
//            try {
//                string[] myIDs = getCookie().UserData.Split(',');
//                crtrChannelID=myIDs[2];
//                channelArray = crtrChannelID.ToCharArray();
//                crtrChannelID = channelArray[2].ToString();
//            }
//            catch  {
				
//                crtrChannelID="0";
//            }
//            return crtrChannelID;
//        }

//        //get Sec Options
//        public string SecurityOptions() {
//            try {
//                string[] myIDs = getCookie().UserData.Split(',');
//                return myIDs[3];
//            }
//            catch  {
//                return string.Empty;
//            }
//        }

//        // Product Feature check
//        //public bool hasProductFeature(string product_name,string feature_name) 
//        //{
//        //    try {
//        //        string customer_id = CustomerID();
//        //        /*string product_id = DataFunctions.ExecuteScalar("select ProductID from " + accountsdb + ".dbo.Product where ProductName = '" + product_name + "'").ToString();
//        //        string product_detail_id = DataFunctions.ExecuteScalar("select ProductDetailID from " + accountsdb + ".dbo.ProductDetail where ProductID = " + product_id + " and ProductDetailName='"+ feature_name + "'").ToString();  
//        //        string sql_stmt = "select Active from " + accountsdb + ".dbo.CustomerProduct where CustomerID = " + customer_id + " and ProductDetailID = " + product_detail_id;
//        //        */
//        //        //Combined all the 3 sqls in to one - ashok [10/04/06]
//        //        string hasFeatureSQL = "SELECT cp.Active "+
//        //            " FROM "+accountsdb+".dbo.CustomerProduct cp "+
//        //            " JOIN "+accountsdb+".dbo.ProductDetail pd ON cp.ProductDetailID = pd.ProductDetailID AND ProductDetailName='"+feature_name+"' "+
//        //            " JOIN "+accountsdb+".dbo.Product p ON pd.ProductID = p.ProductID AND p.ProductName = '"+product_name+"' "+
//        //            " WHERE CustomerID = "+customer_id;
//        //        string my_answer = DataFunctions.ExecuteScalar(hasFeatureSQL).ToString();
//        //        if(my_answer.Equals("y")) {
//        //            return true;
//        //        }
//        //        return false;
//        //    } 
//        //    catch  {
//        //        return false;
//        //    }
//        //}

//        public bool hasProductFeature(string product_name,string feature_name, string customerID) {
//            //'cos the blast engine sometimes call this method & there's no way we can get a custoemrID from the cookie.
//            try {
//                string hasFeatureSQL = "SELECT cp.Active "+
//                    " FROM CustomerProduct cp JOIN ProductDetail pd ON cp.ProductDetailID = pd.ProductDetailID AND ProductDetailName='"+feature_name+"' "+
//                    " JOIN Product p ON pd.ProductID = p.ProductID AND p.ProductName = '"+product_name+"' "+
//                    " WHERE CustomerID = "+customerID;
//                string my_answer = DataFunctions.ExecuteScalar("accounts", hasFeatureSQL).ToString();
//                if(my_answer.Equals("y")) {
//                    return true;
//                }
//                return false;
//            } 
//            catch  {
				
//                return false;
//            }
//        }

//        // Use ECNSession object to find out permissions.
//        // Sunil - 10/19/2007
        
//        ///// Ensures that a user has the proper permissions to use a particular feature
//        ///// If something goes wrong, we allow by default
        
//        ///// <param name="product_name"> The ECN product line</param>
//        ///// <param name="action_code"> The action they want to perform</param>
//        ///// <returns></returns>
//        //public bool CanDoAction(string product_name,string action_code) {
//        //    try {
//        //        object ret_value = DataFunctions.ExecuteScalar("select UA.active from " + accountsdb + ".dbo.useractions UA join " + accountsdb + ".dbo.Action a on ua.actionID = a.actionID join " + accountsdb + ".dbo.product p on p.productID = a.productID where p.productname = '" + product_name + "' and a.actioncode='" + action_code + "' and UserID=" + UserID());

//        //        //Commented the above If Condition 'cos it returns true, if that useraction doesn't exist for that User.. 
//        //        //it should be false. -ashok 5/2/06
//        //        //if(null == ret_value) return true;
//        //        if(null == ret_value) return false;

//        //        string active = ret_value.ToString();
//        //        if(active == "Y") return true;

//        //        return false;
//        //    } catch {
//        //        return true;
//        //    }
//        //}

//        // each Character of  "theSecurityOptions" represents the security options as follows:
//        //
//        // Accounts Options				Communicator Options		Collector Options			CreatorOptions
//        //-----------------				---------------------		----------------			---------------
//        // char 0 - Sys Admin			char 6 - Group Prev			char 9 - Prev 1			char 12 - Prev 1
//        // char 1 - Channel Admin		char 7 - Content Prev		char 10 - Prev 2			char 13 - Prev 2
//        // char 2 - Admin					char 8 - Blast Prev			char 11 - Prev 3			char 14 - Prev 3
//        // char 3 - User
//        // char 4 - Customer
//        // char 5 - Channel


//        //Sys Admin 
//        public bool CheckSysAdmin(){
//            secOptionsArray = SecurityOptions().ToCharArray();
//            if (secOptionsArray[0] == '1')
//                return true;
//            else
//                return false;
//        }
//        //Channel Admin
//        public bool CheckChannelAdmin()
//        {
//            secOptionsArray = SecurityOptions().ToCharArray();
//            if (secOptionsArray[1] == '1')
//                return true;
//            else
//                return false;
//        }

//        //Admin
//        public bool CheckAdmin()
//        {
//            secOptionsArray = SecurityOptions().ToCharArray();
//            if (secOptionsArray[2] == '1')
//                return true;
//            else
//                return false;
//        }

//        //Admin
//        public bool CheckUserAccess()
//        {
//            secOptionsArray = SecurityOptions().ToCharArray();
//            if (secOptionsArray[3] == '1')
//                return true;
//            else
//                return false;
//        }

//        //Admin
//        public bool CheckCustomerAccess()
//        {
//            secOptionsArray = SecurityOptions().ToCharArray();
//            if (secOptionsArray[4] == '1')
//                return true;
//            else
//                return false;
//        }

//        //Admin
//        public bool CheckChannelAccess()
//        {
//            secOptionsArray = SecurityOptions().ToCharArray();
//            if (secOptionsArray[5] == '1')
//                return true;
//            else
//                return false;
//        }

//        // Use ECNSession object to find out permissions.
//        // Sunil - 10/19/2007
//        #region  Use ECNSession object to find out permissions.
        
//        //        //Group
////        public bool CheckGroupPrevilage(){
////            return CanDoAction("ecn.communicator","grouppriv");
//////			secOptionsArray = SecurityOptions().ToCharArray();
//////			bool check = false;
//////			if(secOptionsArray[6] == '1'){
//////				check = true;
//////			}else {
//////				check = false;
//////			}		
//////			return check;
////        }
////        //Content
////        public bool CheckContentPrevilage(){
////            return CanDoAction("ecn.communicator","contentpriv");
//////			secOptionsArray = SecurityOptions().ToCharArray();
//////			bool check = false;
//////			if(secOptionsArray[7] == '1'){
//////				check = true;
//////			}else {
//////				check = false;
//////			}		
//////			return check;
////        }
////        //Blast
////        public bool CheckBlastPrevilage(){
////            return CanDoAction("ecn.communicator","blastpriv");
//////			secOptionsArray = SecurityOptions().ToCharArray();
//////			bool check = false;
//////			if(secOptionsArray[8] == '1'){
//////				check = true;
//////			}else {
//////				check = false;
//////			}		
//////			return check;
////        }

////        public bool IsApprovalMessageCreator(){
////            return CanDoAction("ecn.communicator","approvalblast");		
////        }

//        #endregion

//        //get the Customer Levels
//        public string CustomerLevel()
//        {
//            try
//            {
//                string[] myIDs = getCookie().UserData.Split(',');
//                return myIDs[4];
//            }
//            catch
//            {
//                return string.Empty;
//            }
//        }

//        //get the Customer Levels
//        public int MasterUserID()
//        {
//            try
//            {
//                string[] myIDs = getCookie().UserData.Split(',');
//                return Convert.ToInt32(myIDs[5]);
//            }
//            catch
//            {
//                return 0;
//            }
//        }

//        // each Character of  "theCustomerLevel" represents the customerLevels as follows:
//        //
//        // Communicator Level			Collector Level					Creator Level				Account Level
//        //-------------------			-------------------			--------------				---------------
//        // char 0 -  1 - silver /			char 1 - Collector Prev		char 2 - Prev 1			char 3 - Prev 1
//        //				2 - gold /
//        //				3 - platinum
//        // Example Check for Product levels: 
//        // if getXXXLevel() > 0{
//        //		<!-- customer is authorized -->
//        // }else { <!-- not authorized -->}
//        //Communicator Level

//        public string getCommunicatorLevel(){
//            customerLevelsArray = CustomerLevel().ToCharArray();
//            return customerLevelsArray[0].ToString();
//        }

//        //Collector Level
//        public string getCollectorLevel(){
//            customerLevelsArray = CustomerLevel().ToCharArray();
//            return customerLevelsArray[1].ToString();
//        }

//        //Creator Level
//        public string getCreatorLevel(){
//            customerLevelsArray = CustomerLevel().ToCharArray();
//            return customerLevelsArray[2].ToString();
//        }

//        //Accounts Level
//        public string getAccountsLevel(){
//            customerLevelsArray = CustomerLevel().ToCharArray();
//            return customerLevelsArray[3].ToString();
//        }

//        //Publisher Level
//        public string getPublisherLevel()
//        {
//            customerLevelsArray = CustomerLevel().ToCharArray();
//            return customerLevelsArray[4].ToString();
//        }

//        //Charity Level
//        public string getCharityLevel() {
//            customerLevelsArray = CustomerLevel().ToCharArray();
//            return customerLevelsArray[5].ToString();
//        }

//        public FormsAuthenticationTicket getCookie() {
//            FormsAuthenticationTicket ticket = null;
//            if (HttpContext.Current.User != null) {
//                if (HttpContext.Current.User.Identity.IsAuthenticated) {
//                    if (HttpContext.Current.User.Identity is FormsIdentity) {
//                        System.Web.Security.FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
//                        ticket = id.Ticket;
//                        string AuthData = ticket.UserData;
//                        string AuthName = ticket.Name;
//                        string AuthTime = ticket.IssueDate.ToString();
//                        string[] myIDs = AuthData.Split(',');
//                    }
//                }
//            }
//            return ticket;
//        }

//        public string GetHashedPass(String aPassword) {
//            return FormsAuthentication.HashPasswordForStoringInConfigFile(aPassword,"sha1");
//        }

//    }
//}
