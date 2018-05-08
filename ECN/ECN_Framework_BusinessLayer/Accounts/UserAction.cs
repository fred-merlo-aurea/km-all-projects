using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

using ECN_Framework_Common.Functions;

namespace ECN_Framework_BusinessLayer.Accounts
{

    [Serializable]
    public class UserAction
    {

        private static readonly string CacheName = "CACHE_USERACTIONS_";

        public static List<ECN_Framework_Entities.Accounts.UserAction> GetbyUserID(int userID)
        {
            List<ECN_Framework_Entities.Accounts.UserAction> lAction = new List<ECN_Framework_Entities.Accounts.UserAction>();
            if (!ECN_Framework_Common.Functions.CacheHelper.IsCacheEnabled())
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    lAction = ECN_Framework_DataLayer.Accounts.UserAction.GetbyUserID(userID);
                    scope.Complete();
                }
            }
            else if (ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache(CacheName + userID) == null)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    lAction = ECN_Framework_DataLayer.Accounts.UserAction.GetbyUserID(userID);
                    scope.Complete();
                }
                ECN_Framework_Common.Functions.CacheHelper.AddToCache(CacheName + userID, lAction);
            }
            else
            {
                lAction = (List<ECN_Framework_Entities.Accounts.UserAction>)ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache(CacheName + userID);
            }
            return lAction;
        }

        public static void Save(ECN_Framework_Entities.Accounts.UserAction userAction)
        {
            CacheHelper.ClearCache(CacheName + userAction.UserID);
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Accounts.UserAction.Save(userAction);
                scope.Complete();
            }
        }
        
        public static void Delete(int useractionID)
        { 
        }

        public static void DeleteAll(int userID)
        {
        }
    }
}
