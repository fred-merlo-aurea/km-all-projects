using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class LandingPageAssignContent
    {
        public static List<ECN_Framework_Entities.Accounts.LandingPageAssignContent> GetByLPAID(int LPAID)
        {
            List<ECN_Framework_Entities.Accounts.LandingPageAssignContent> itemList = new List<ECN_Framework_Entities.Accounts.LandingPageAssignContent>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Accounts.LandingPageAssignContent.GetByLPAID(LPAID);
                scope.Complete();
            }

            return itemList;
        }

        public static ECN_Framework_Entities.Accounts.LandingPageAssignContent GetByLPACID(int LPACID)
        {
            ECN_Framework_Entities.Accounts.LandingPageAssignContent item = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Accounts.LandingPageAssignContent.GetByLPACID(LPACID);
                scope.Complete();
            }

            return item;
        }

        public static List<ECN_Framework_Entities.Accounts.LandingPageAssignContent> GetByLPOID(int LPOID, int LPAID)
        {
            List<ECN_Framework_Entities.Accounts.LandingPageAssignContent> retList = new List<ECN_Framework_Entities.Accounts.LandingPageAssignContent>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Accounts.LandingPageAssignContent.GetByLPOID(LPOID, LPAID);
                scope.Complete();
            }
            return retList;
        }

        public static void Delete(int LPAID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Accounts.LandingPageAssignContent.Delete(LPAID, user.UserID);
                scope.Complete();
            }
        }

        public static void Save(ECN_Framework_Entities.Accounts.LandingPageAssignContent lpac, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                lpac.LPACID = ECN_Framework_DataLayer.Accounts.LandingPageAssignContent.Save(lpac, user.UserID);
                scope.Complete();
            }
        }

        public static DataTable GetReasons(int CustomerID, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = null;
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Accounts.LandingPageAssignContent.GetReasons(CustomerID, fromDate, toDate);
                scope.Complete();
            }
            return dt;
        }
    }
}
