using System;
using System.Diagnostics;
using System.Text;
using ECN_Framework.Common.Helpers;
using ECN_Framework.Common.Interfaces;
using ECN_Framework_Common.Objects;

namespace ECN_Framework.Common
{
    public class SecurityAccess
    {
        private const string CommunicatorConnectionString = "communicator";
        private const string PublisherConnectionString = "publisher";
        private const string CollectorConnectionString = "collector";
        private const string CreatorConnectionString = "creator";
        private const string AccountsConnectionString = "accounts";

        private static IDataFunctions _dataFunctions;
        static bool taint = false;

        static SecurityAccess()
        {
            _dataFunctions = new DataFunctionsAdapter();
        }

        public static void Initialize(IDataFunctions dataFunctions)
        {
            _dataFunctions = dataFunctions;
        }

        public static void enableTaint()
        {
            taint = true;
        }

        public static void canI(string type, string row_id)
        {
            SecurityCheck sc = new SecurityCheck();

            int cust_id = sc.CustomerID();
            int base_channel_id = sc.BasechannelID();
            string userID = sc.UserID();

            if (type.ToLower() == "survey" && Convert.ToInt32(base_channel_id) == 25)
            {
                if (!sc.CheckSysAdmin() && !sc.CheckChannelAdmin())
                    if (!Convert.ToBoolean(ECN_Framework_DataLayer.DataFunctions.ExecuteScalar("misc", "exec sp_nebook_checkRole " + row_id + ',' + userID)))
                        throw new ECN_Framework_Common.Objects.SecurityException("SECURITY VIOLATION!");
            }
            else
            {
                if (!hasAccess(type, row_id) && !taint)
                    throw new ECN_Framework_Common.Objects.SecurityException("SECURITY VIOLATION!");
            }
        }

        public static bool hasAccess(string type, string rowId, string customerId, string baseChannelId)
        {
            try
            {
                switch (type)
                {
                    case "Emails":
                        return canAccessEmailID(rowId, customerId);
                    case "EmailDataValues":
                        return canAccessEmailDataValuesID(rowId, customerId);
                    case "Groups":
                        return canAccessGroupID(rowId, customerId);
                    case "Content":
                        return canAccessContentID(rowId, customerId);
                    case "Blasts":
                        return canAccessBlastID(rowId, customerId);
                    case "FiltersDetails":
                        return canAccessFiltersDetailsID(rowId, customerId);
                    case "Filters":
                        return canAccessFilterID(rowId, customerId);
                    case "ContentFiltersDetails":
                        return canAccessContentFiltersDetailsID(rowId, customerId);
                    case "ContentFilters":
                        return canAccessContentFilterID(rowId, customerId);
                    case "Layouts":
                        return canAccessLayoutID(rowId, customerId);
                    case "Folders":
                        return canAccessFolderID(rowId, customerId);
                    case "Survey":
                        return canAccessSurveyID(rowId, customerId);
                    case "Events":
                        return canAccessEventID(rowId, customerId);
                    case "Menus":
                        return canAccessMenuID(rowId, customerId);
                    case "Pages":
                        return canAccessPageID(rowId, customerId);
                    case "Templates":
                        return canAccessTemplateID(rowId, customerId);
                    case "HeaderFooters":
                        return canAccessHeaderFooterID(rowId, customerId);
                    case "colHeaderFooters":
                        return canAccesscolHeaderFooterID(rowId, customerId);
                    case "Users":
                        return canAccessUserID(rowId, customerId);
                    case "Customers":
                        return canAccessCustomerID(rowId, baseChannelId);
                    case "CustomerTemplates":
                        return canAccessCustomerTemplateID(rowId, baseChannelId);
                    case "CustomerLicenses":
                        return canAccessCustomerLicenseID(rowId, baseChannelId);
                    case "BaseChannel":
                        return string.CompareOrdinal(rowId, baseChannelId) == 0;
                    case "Channels":
                        return canAccessChannelID(rowId, baseChannelId);
                    case "Publications":
                        return canAccessPublicationID(rowId, customerId);
                    case "Editions":
                        return canAccessEditionID(rowId, customerId);
                    case "ChannelPartnerTemplates":
                        return canAccessChannelPartnerTemplates(rowId, baseChannelId);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw new SecurityException("ID Does Not Exist!");
            }
            return false;
        }

        public static bool hasAccess(string type, string rowId)
        {
            var securityCheck = new SecurityCheck();
            var customerId = securityCheck.CustomerID().ToString();
            var baseChannelId = securityCheck.BasechannelID().ToString();

            return hasAccess(type, rowId, customerId, baseChannelId);
        }

        private static bool canAccessPublicationID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from Publications where PublicationID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, PublisherConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }

        private static bool canAccessEditionID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from Editions e join Publications m on e.PublicationID = m.PublicationID where e.EditionID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, PublisherConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }

        private static bool canAccessEmailID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from Email where EmailID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CommunicatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }

        private static bool canAccessEmailDataValuesID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT e.CustomerID from Email e, EmailDataValues ed where e.EmailID = ed.EmailID AND ed.EmailDataValuesID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CommunicatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }

        private static bool canAccessGroupID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from Groups where GroupID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CommunicatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }

        private static bool canAccessContentID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from Content where ContentID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CommunicatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }

        private static bool canAccessBlastID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from Blasts where BlastID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CommunicatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }
        private static bool canAccessFilterID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from Filters where FilterID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CommunicatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }
        private static bool canAccessFiltersDetailsID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT f.CustomerID from Filters f, FiltersDetails fd where f.FilterID = fd.FilterID AND fd.FDID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CommunicatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }
        private static bool canAccessContentFilterID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT l.CustomerID from ContentFilter f, Layout l where l.LayoutID = f.LayoutID AND f.FilterID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CommunicatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }
        private static bool canAccessContentFiltersDetailsID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT l.CustomerID from ContentFilter f, ContentFilterDetail fd, Layout l where f.FilterID = fd.FilterID AND f.layoutID = l.layoutID AND fd.FDID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CommunicatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }
        private static bool canAccessLayoutID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from Layout where LayoutID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CommunicatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }
        private static bool canAccessFolderID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from Folder where FolderID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CommunicatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }
        private static bool canAccessSurveyID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from survey where surveyid = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CollectorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }
        private static bool canAccessEventID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from Events where EventID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CreatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }
        private static bool canAccessMenuID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from Menus where MenuID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CreatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }
        private static bool canAccessPageID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from Page where PageID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CreatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }
        private static bool canAccessHeaderFooterID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from HeaderFooters where HeaderFooterID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CreatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }
        private static bool canAccesscolHeaderFooterID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from HeaderFooters where HeaderFooterID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CollectorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }
        private static bool canAccessTemplateID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from Templates where TemplateID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, CreatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }
        private static bool canAccessUserID(string rowId, string customerId)
        {
            var query = GetQuery("SELECT CustomerID from Users where UserID = ", rowId);
            var goodCustomerId = _dataFunctions.ExecuteScalar(query, AccountsConnectionString)?.ToString();
            return string.CompareOrdinal(goodCustomerId, customerId) == 0;
        }
        private static bool canAccessCustomerID(string rowId, string baseChannelId)
        {
            var query = GetQuery("SELECT BaseChannelID from Customer where CustomerID = ", rowId);
            var goodBaseChannelId = _dataFunctions.ExecuteScalar(query, AccountsConnectionString)?.ToString();
            return string.CompareOrdinal(goodBaseChannelId, baseChannelId) == 0;
        }
        private static bool canAccessCustomerTemplateID(string rowId, string baseChannelId)
        {
            var query = GetQuery("SELECT c.BaseChannelID from Customer c, CustomerTemplate ct where c.CustomerID = ct.CustomerID AND ct.CTID=", rowId);
            var goodBaseChannelId = _dataFunctions.ExecuteScalar(query, AccountsConnectionString)?.ToString();
            return string.CompareOrdinal(goodBaseChannelId, baseChannelId) == 0;
        }
        private static bool canAccessCustomerLicenseID(string rowId, string baseChannelId)
        {
            var query = GetQuery("SELECT c.BaseChannelID from Customer c, CustomerLicense cl where c.CustomerID = cl.CustomerID AND cl.CLID=", rowId);
            var goodBaseChannelId = _dataFunctions.ExecuteScalar(query, AccountsConnectionString)?.ToString();
            return string.CompareOrdinal(goodBaseChannelId, baseChannelId) == 0;
        }
        private static bool canAccessChannelID(string rowId, string baseChannelId)
        {
            var query = GetQuery("SELECT BaseChannelID from Channel where ChannelID=", rowId);
            var goodBaseChannelId = _dataFunctions.ExecuteScalar(query, AccountsConnectionString)?.ToString();
            return string.CompareOrdinal(goodBaseChannelId, baseChannelId) == 0;
        }
        private static bool canAccessChannelPartnerTemplates(string rowId, string baseChannelId)
        {
            var query = GetQuery("SELECT ChannelID FROM Templates WHERE ChannelID = ", rowId, " AND TemplateID = ", baseChannelId);
            var goodBaseChannelId = _dataFunctions.ExecuteScalar(query, CommunicatorConnectionString)?.ToString();
            return string.CompareOrdinal(goodBaseChannelId, baseChannelId) == 0;
        }
        private static string GetQuery(string selectPart, string rowId)
        {
            return new StringBuilder(selectPart)
                        .Append(rowId).ToString();
        }
        private static string GetQuery(string selectPart, string rowId, string andPart, string baseChannelId)
        {
            return new StringBuilder(selectPart)
                        .Append(baseChannelId)
                        .Append(andPart)
                        .Append(rowId).ToString();
        }
    }
}
