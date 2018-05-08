//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Web;
//using System.Configuration;
//using System.Web.Security;

//namespace ecn.common.classes 
//{
//    public class SecurityAccess
//    {

//        static bool taint = false;
//        public SecurityAccess()
//        {

//        }
//        public static void enableTaint()
//        {
//            taint = true;
//        }
//        public static void canI(string type, string row_id)
//        {
//            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

//            string cust_id = sc.CustomerID();
//            string base_channel_id = sc.ChannelID();
//            string userID = sc.UserID();

//            if (type.ToLower() == "survey" && Convert.ToInt32(base_channel_id) == 25)
//            {
//                if (!sc.CheckSysAdmin() && !sc.CheckChannelAdmin())
//                    if (!Convert.ToBoolean(DataFunctions.ExecuteScalar("misc", "exec sp_nebook_checkRole " + row_id + ',' + userID)))
//                        throw new SecurityException("SECURITY VIOLATION!");
//            }
//            else
//            {
//                if (!hasAccess(type, row_id) && !taint)
//                    throw new SecurityException("SECURITY VIOLATION!");
//            }
//        }

//        public static bool hasAccess(string type, string row_id, string cust_id, string base_channel_id)
//        {
//            try
//            {
//                switch (type)
//                {
//                    case "Emails":
//                        return canAccessEmailID(row_id, cust_id);
//                    case "EmailDataValues":
//                        return canAccessEmailDataValuesID(row_id, cust_id);
//                    case "Groups":
//                        return canAccessGroupID(row_id, cust_id);
//                    case "Content":
//                        return canAccessContentID(row_id, cust_id);
//                    case "Blasts":
//                        return canAccessBlastID(row_id, cust_id);
//                    case "FiltersDetails":
//                        return canAccessFiltersDetailsID(row_id, cust_id);
//                    case "Filters":
//                        return canAccessFilterID(row_id, cust_id);
//                    case "ContentFiltersDetails":
//                        return canAccessContentFiltersDetailsID(row_id, cust_id);
//                    case "ContentFilters":
//                        return canAccessContentFilterID(row_id, cust_id);
//                    case "Layouts":
//                        return canAccessLayoutID(row_id, cust_id);
//                    case "Folders":
//                        return canAccessFolderID(row_id, cust_id);
//                    case "Survey":
//                        return canAccessSurveyID(row_id, cust_id);
//                    case "Events":
//                        return canAccessEventID(row_id, cust_id);
//                    case "Menus":
//                        return canAccessMenuID(row_id, cust_id);
//                    case "Pages":
//                        return canAccessPageID(row_id, cust_id);
//                    case "Templates":
//                        return canAccessTemplateID(row_id, cust_id);
//                    case "HeaderFooters":
//                        return canAccessHeaderFooterID(row_id, cust_id);
//                    case "colHeaderFooters":
//                        return canAccesscolHeaderFooterID(row_id, cust_id);
//                    case "Users":
//                        return canAccessUserID(row_id, cust_id);
//                    case "Customers":
//                        return canAccessCustomerID(row_id, base_channel_id);
//                    case "CustomerTemplates":
//                        return canAccessCustomerTemplateID(row_id, base_channel_id);
//                    case "CustomerLicenses":
//                        return canAccessCustomerLicenseID(row_id, base_channel_id);
//                    case "BaseChannel":
//                        return row_id.Equals(base_channel_id);
//                    case "Channels":
//                        return canAccessChannelID(row_id, base_channel_id);
//                    case "Publications":
//                        return canAccessPublicationID(row_id, cust_id);
//                    case "Editions":
//                        return canAccessEditionID(row_id, cust_id);
//                    case "ChannelPartnerTemplates":
//                        return canAccessChannelPartnerTemplates(row_id, base_channel_id);
//                }
//            }
//            catch (Exception)
//            {
//                // throw e;
//                throw new SecurityException("ID Does Not Exist!");
//            }
//            return false;
//        }

//        public static bool hasAccess(string type, string row_id)
//        {
//            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
//            string cust_id = sc.CustomerID();
//            string base_channel_id = sc.ChannelID();
//            try
//            {
//                switch (type)
//                {
//                    case "Emails":
//                        return canAccessEmailID(row_id, cust_id);
//                    case "EmailDataValues":
//                        return canAccessEmailDataValuesID(row_id, cust_id);
//                    case "Groups":
//                        return canAccessGroupID(row_id, cust_id);
//                    case "Content":
//                        return canAccessContentID(row_id, cust_id);
//                    case "Blasts":
//                        return canAccessBlastID(row_id, cust_id);
//                    case "FiltersDetails":
//                        return canAccessFiltersDetailsID(row_id, cust_id);
//                    case "Filters":
//                        return canAccessFilterID(row_id, cust_id);
//                    case "ContentFiltersDetails":
//                        return canAccessContentFiltersDetailsID(row_id, cust_id);
//                    case "ContentFilters":
//                        return canAccessContentFilterID(row_id, cust_id);
//                    case "Layouts":
//                        return canAccessLayoutID(row_id, cust_id);
//                    case "Folders":
//                        return canAccessFolderID(row_id, cust_id);
//                    case "Survey":
//                        return canAccessSurveyID(row_id, cust_id);
//                    case "Events":
//                        return canAccessEventID(row_id, cust_id);
//                    case "Menus":
//                        return canAccessMenuID(row_id, cust_id);
//                    case "Pages":
//                        return canAccessPageID(row_id, cust_id);
//                    case "Templates":
//                        return canAccessTemplateID(row_id, cust_id);
//                    case "HeaderFooters":
//                        return canAccessHeaderFooterID(row_id, cust_id);
//                    case "colHeaderFooters":
//                        return canAccesscolHeaderFooterID(row_id, cust_id);
//                    case "Users":
//                        return canAccessUserID(row_id, cust_id);
//                    case "Customers":
//                        return canAccessCustomerID(row_id, base_channel_id);
//                    case "CustomerTemplates":
//                        return canAccessCustomerTemplateID(row_id, base_channel_id);
//                    case "CustomerLicenses":
//                        return canAccessCustomerLicenseID(row_id, base_channel_id);
//                    case "BaseChannel":
//                        return row_id.Equals(base_channel_id);
//                    case "Channels":
//                        return canAccessChannelID(row_id, base_channel_id);
//                    case "Publications":
//                        return canAccessPublicationID(row_id, cust_id);
//                    case "Editions":
//                        return canAccessEditionID(row_id, cust_id);
//                    case "ChannelPartnerTemplates":
//                        return canAccessChannelPartnerTemplates(row_id, base_channel_id);
//                }
//            }
//            catch (Exception)
//            {
//                // throw e;
//                throw new SecurityException("ID Does Not Exist!");
//            }
//            return false;
//        }

//        private static bool canAccessPublicationID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("publisher", "SELECT CustomerID from Publication where PublicationID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }

//        private static bool canAccessEditionID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("publisher", "SELECT CustomerID from Edition e join Publication m on e.PublicationID = m.PublicationID where e.EditionID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }

//        private static bool canAccessEmailID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("communicator", "SELECT CustomerID from Emails where EmailID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }

//        private static bool canAccessEmailDataValuesID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("communicator", "SELECT e.CustomerID from Emails e, EmailDataValues ed where e.EmailID = ed.EmailID AND ed.EmailDataValuesID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }

//        private static bool canAccessGroupID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("communicator", "SELECT CustomerID from Groups where GroupID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }

//        private static bool canAccessContentID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("communicator", "SELECT CustomerID from Content where ContentID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }

//        private static bool canAccessBlastID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("communicator", "SELECT CustomerID from Blast where BlastID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }
//        private static bool canAccessFilterID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("communicator", "SELECT CustomerID from Filter where FilterID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }
//        private static bool canAccessFiltersDetailsID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("communicator", "SELECT f.CustomerID from Filter f, FiltersDetails fd where f.FilterID = fd.FilterID AND fd.FDID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }
//        private static bool canAccessContentFilterID(string row_id, string cust_id)
//        {
//            //            throw new Exception("cfid=" + row_id);
//            string good_cust_id = DataFunctions.ExecuteScalar("communicator", "SELECT l.CustomerID from ContentFilter f, Layout l where l.LayoutID = f.LayoutID AND f.FilterID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }
//        private static bool canAccessContentFiltersDetailsID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("communicator", "SELECT l.CustomerID from ContentFilter f, ContentFilterDetail fd, Layout l where f.FilterID = fd.FilterID AND f.layoutID = l.layoutID AND fd.FDID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }
//        private static bool canAccessLayoutID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("communicator", "SELECT CustomerID from Layout where LayoutID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }
//        private static bool canAccessFolderID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("communicator", "SELECT CustomerID from Folder where FolderID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }
//        private static bool canAccessSurveyID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("collector", "SELECT CustomerID from survey where SurveyID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }
//        private static bool canAccessEventID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("creator", "SELECT CustomerID from Events where EventID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }
//        private static bool canAccessMenuID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("creator", "SELECT CustomerID from Menus where MenuID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }
//        private static bool canAccessPageID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("creator", "SELECT CustomerID from Page where PageID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }
//        private static bool canAccessHeaderFooterID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("creator", "SELECT CustomerID from HeaderFooters where HeaderFooterID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }
//        private static bool canAccesscolHeaderFooterID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("collector", "SELECT CustomerID from HeaderFooters where HeaderFooterID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }
//        private static bool canAccessTemplateID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("creator", "SELECT CustomerID from Template where TemplateID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }
//        private static bool canAccessUserID(string row_id, string cust_id)
//        {
//            string good_cust_id = DataFunctions.ExecuteScalar("accounts", "SELECT CustomerID from Users where UserID = " + row_id).ToString();
//            return good_cust_id.Equals(cust_id);
//        }
//        private static bool canAccessCustomerID(string row_id, string base_channel_id)
//        {
//            string good_base_channel_id = DataFunctions.ExecuteScalar("accounts", "SELECT BaseChannelID from Customer where CustomerID = " + row_id).ToString();
//            return good_base_channel_id.Equals(base_channel_id);
//        }
//        private static bool canAccessCustomerTemplateID(string row_id, string base_channel_id)
//        {
//            string good_base_channel_id = DataFunctions.ExecuteScalar("accounts", "SELECT c.BaseChannelID from Customer c, CustomerTemplate ct where c.CustomerID = ct.CustomerID AND ct.CTID=" + row_id).ToString();
//            return good_base_channel_id.Equals(base_channel_id);
//        }
//        private static bool canAccessCustomerLicenseID(string row_id, string base_channel_id)
//        {
//            string good_base_channel_id = DataFunctions.ExecuteScalar("accounts", "SELECT c.BaseChannelID from Customer c, CustomerLicense cl where c.CustomerID = cl.CustomerID AND cl.CLID=" + row_id).ToString();
//            return good_base_channel_id.Equals(base_channel_id);
//        }
//        private static bool canAccessChannelID(string row_id, string base_channel_id)
//        {
//            string good_base_channel_id = DataFunctions.ExecuteScalar("accounts", "SELECT BaseChannelID from Channel where ChannelID=" + row_id).ToString();
//            return good_base_channel_id.Equals(base_channel_id);
//        }
//        private static bool canAccessChannelPartnerTemplates(string row_id, string base_channel_id) {
//            string good_base_channel_id = DataFunctions.ExecuteScalar("communicator", "SELECT ChannelID FROM Template WHERE ChannelID = " + base_channel_id + " AND TemplateID = " + row_id).ToString();
//            return good_base_channel_id.Equals(base_channel_id);
//        }
//    }
//}
