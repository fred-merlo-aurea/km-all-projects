using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    public class SubscriptionManagementGroup
    {
        public static List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup> GetBySMID(int SMID)
        {
            List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup> retList = new List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Accounts.SubscriptionManagementGroup.GetBySMID(SMID);
                scope.Complete();
            }
            return retList;
        }


        public static bool Exists(int SMID, int SMGID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Accounts.SubscriptionManagementGroup.Exists(SMGID, SMID);
                scope.Complete();
            }
            return exists;
        }

        public static void Delete(int SMID, int SMGID, KMPlatform.Entity.User User)
        {
            if (Exists(SMID, SMGID))
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementUDF.Delete(SMGID, User);

                    ECN_Framework_DataLayer.Accounts.SubscriptionManagementGroup.Delete(SMID, SMGID, User.UserID);

                    scope.Complete();
                }
            }
        }

        public static void Delete(int SMID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup> SMGroups = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementGroup.GetBySMID(SMID);
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (ECN_Framework_Entities.Accounts.SubscriptionManagementGroup smg in SMGroups)
                {
                    ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementUDF.Delete(smg.SubscriptionManagementGroupID, user);
                }
                ECN_Framework_DataLayer.Accounts.SubscriptionManagementGroup.Delete(SMID, user.UserID);

                scope.Complete();
            }
        }

        public static int Save(ECN_Framework_Entities.Accounts.SubscriptionManagementGroup smg)
        {
            int retID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Accounts.SubscriptionManagementGroup.Save(smg);
                scope.Complete();
            }

            return retID;
        }
    }
}
