using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.DomainTracker
{
    public class DomainTrackerValue
    {
        public static int Save(ECN_Framework_Entities.DomainTracker.DomainTrackerValue domainTrackerValue, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                domainTrackerValue.DomainTrackerValueID = ECN_Framework_DataLayer.DomainTracker.DomainTrackerValue.Save(domainTrackerValue, user.UserID);
                scope.Complete();
            }
            return domainTrackerValue.DomainTrackerValueID;
        }

        public static DataTable GetByProfileID(int profileID, int domainTrackerID, KMPlatform.Entity.User user)
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.DomainTracker.DomainTrackerValue.GetByProfileID(profileID, domainTrackerID);
                scope.Complete();
            }
            return dt;
        }
    }
}
