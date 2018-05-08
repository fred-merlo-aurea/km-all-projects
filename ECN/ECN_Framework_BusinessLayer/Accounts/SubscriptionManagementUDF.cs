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
    [Serializable]
    public class SubscriptionManagementUDF
    {
        public static List<ECN_Framework_Entities.Accounts.SubsriptionManagementUDF> GetBySMGID(int SMGID)
        {
            List<ECN_Framework_Entities.Accounts.SubsriptionManagementUDF> retList = new List<ECN_Framework_Entities.Accounts.SubsriptionManagementUDF>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Accounts.SubscriptionManagementUDF.GetBySMGID(SMGID);
                scope.Complete();
            }
            return retList;
        }

        public static bool Exists(int SMGID, int SMID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Accounts.SubscriptionManagementUDF.Exists(SMID, SMGID);
                scope.Complete();
            }

            return exists;
        }

        public static void Delete(int SMGID, KMPlatform.Entity.User user, int? SMGUDFID = null)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (SMGUDFID == null)
                {
                    ECN_Framework_DataLayer.Accounts.SubscriptionManagementUDF.Delete(SMGID, user.UserID);
                }
                else
                {
                    ECN_Framework_DataLayer.Accounts.SubscriptionManagementUDF.Delete(SMGID, user.UserID, SMGUDFID.Value);
                }
                scope.Complete();
            }
        }

        public static void Save(ECN_Framework_Entities.Accounts.SubsriptionManagementUDF smgudf)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Accounts.SubscriptionManagementUDF.Save(smgudf);
                scope.Complete();
            }
        }

    }
}
