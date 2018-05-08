using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Accounts
{
    public class SubscriptionManagementReason
    {
        public static int Save(ECN_Framework_Entities.Accounts.SubscriptionManagementReason smr)
        {
            int retID = -1;
            using(TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Accounts.SubscriptionManagementReason.Save(smr);
                scope.Complete();
            }
            return retID;
        }

        public static List<ECN_Framework_Entities.Accounts.SubscriptionManagementReason> GetBySMID(int SMID)
        {
            List<ECN_Framework_Entities.Accounts.SubscriptionManagementReason> retList = new List<ECN_Framework_Entities.Accounts.SubscriptionManagementReason>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Accounts.SubscriptionManagementReason.GetBySMID(SMID);
                scope.Complete();
            }
            return retList;
        }
    }
}
