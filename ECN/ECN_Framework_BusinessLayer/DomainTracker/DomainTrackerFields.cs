using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;


namespace ECN_Framework_BusinessLayer.DomainTracker
{
    [Serializable]
    public class DomainTrackerFields
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.DomainTrackerFields;

        public static DataTable GetByDomainTrackerID_DT(int domainTrackerID, KMPlatform.Entity.User user)
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerFields.GetByDomainTrackerID_DT(domainTrackerID);
                scope.Complete();
            }
            return dt;
        }

        public static List<ECN_Framework_Entities.DomainTracker.DomainTrackerFields> GetByDomainTrackerID(int domainTrackerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.DomainTracker.DomainTrackerFields> dt = new List<ECN_Framework_Entities.DomainTracker.DomainTrackerFields>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerFields.GetByDomainTrackerID(domainTrackerID);
                scope.Complete();
            }
            return dt;
        }

        public static void Delete(int domainTrackerFieldsID, int domainTrackerID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.DomainTracker.DomainTrackerFields.Delete(domainTrackerFieldsID, domainTrackerID, user.UserID);
                scope.Complete();
            }
        }

        public static void Delete(int domainTrackerID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.DomainTracker.DomainTrackerFields.Delete(domainTrackerID, user.UserID);
                scope.Complete();
            }
        }

        public static int Save(ECN_Framework_Entities.DomainTracker.DomainTrackerFields item, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                item.DomainTrackerFieldsID = ECN_Framework_DataLayer.DomainTracker.DomainTrackerFields.Save(item);
                scope.Complete();
            }
            return item.DomainTrackerID;
        }

        public static bool Exists(string FieldName, int DomainTrackerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.DomainTracker.DomainTrackerFields.Exists(FieldName, DomainTrackerID);
                scope.Complete();
            }
            return exists;
        }

    }
}
