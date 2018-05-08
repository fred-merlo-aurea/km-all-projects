using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;

namespace ECN_Framework_BusinessLayer.DomainTracker
{
    [Serializable]
    public class DomainTracker
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.DomainTracker;

        public static int Save(ECN_Framework_Entities.DomainTracker.DomainTracker item, KMPlatform.Entity.User user)
        {
            Validate(item, user);
            using (TransactionScope scope = new TransactionScope())
            {
                item.DomainTrackerID = ECN_Framework_DataLayer.DomainTracker.DomainTracker.Save(item);
                scope.Complete();
            }
            return item.DomainTrackerID;
        }

        public static void Validate(ECN_Framework_Entities.DomainTracker.DomainTracker item, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();


            Regex regex = new Regex(@"^(([a-zA-Z0-9]+\.)?([a-zA-Z0-9\-]+\.).{2,3})$");
            Match match = regex.Match(item.Domain);
            if (!match.Success)
            {
                errorList.Add(new ECNError(Entity, Method, "Invalid Domain"));
            }

            if (item.BaseChannelID == -1)
            {
                errorList.Add(new ECNError(Entity, Method, "BaseChannelID is invalid"));
            }
            else
            {
                if (Exists(item.Domain, item.BaseChannelID))
                {
                    errorList.Add(new ECNError(Entity, Method, "This domain is already being tracked. Please select a different domain."));
                }
                if (item.Domain.Equals(string.Empty))
                {
                    errorList.Add(new ECNError(Entity, Method, "The domain name cannot be empty."));
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static bool Exists(string domainName, int baseChannelID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.DomainTracker.DomainTracker.Exists(domainName, baseChannelID);
                scope.Complete();
            }
            return exists;
        }

        public static void Delete(int domainTrackerID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_BusinessLayer.DomainTracker.DomainTrackerFields.Delete(domainTrackerID, user);
                ECN_Framework_DataLayer.DomainTracker.DomainTracker.Delete(domainTrackerID, user.UserID);
                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.DomainTracker.DomainTracker> GetByBaseChannelID(int baseChannelID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.DomainTracker.DomainTracker> DomainTrackerList = new List<ECN_Framework_Entities.DomainTracker.DomainTracker>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                DomainTrackerList = ECN_Framework_DataLayer.DomainTracker.DomainTracker.GetByBaseChannelID(baseChannelID);
                scope.Complete();
            }
            return DomainTrackerList;
        }

        public static ECN_Framework_Entities.DomainTracker.DomainTracker GetByDomainTrackerID(int domainTrackerID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.DomainTracker.DomainTracker domainTracker = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                domainTracker = ECN_Framework_DataLayer.DomainTracker.DomainTracker.GetByDomainTrackerID(domainTrackerID);
                scope.Complete();
            }
            return domainTracker;
        }

        public static ECN_Framework_Entities.DomainTracker.DomainTracker GetByTrackerKey(string trackerKey)
        {
            ECN_Framework_Entities.DomainTracker.DomainTracker domainTracker = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                domainTracker = ECN_Framework_DataLayer.DomainTracker.DomainTracker.GetByTrackerKey(trackerKey);
                scope.Complete();
            }
            return domainTracker;
        }

        public static DataTable GetUserExport(int domainTrackerID, string StartDate, string EndDate, string email = null, string TypeFilter = "known")
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTracker.DomainTrackerFields_User_Export(domainTrackerID, StartDate, EndDate, email, TypeFilter);
                scope.Complete();
            }
            return dt;
        }

        public static DataTable GetDataTable(int CustomerID, DateTime StartDate, DateTime EndDate, string GroupIDs)
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.Report.DataDumpReport.GetDataTable(CustomerID, StartDate, EndDate, GroupIDs);
                scope.Complete();
            }
            return dt;

        }

    }
}
