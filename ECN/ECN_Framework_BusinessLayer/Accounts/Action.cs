using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Accounts
{
    [Serializable]
    public class Action
    {
        public static bool Exists(int actionID)
        {
            return ECN_Framework_DataLayer.Accounts.Action.Exists(actionID);
        }

        public static List<ECN_Framework_Entities.Accounts.Action> GetAll()
        {
            if (!ECN_Framework_Common.Functions.CacheHelper.IsCacheEnabled())
            {
                return ECN_Framework_DataLayer.Accounts.Action.GetAll();
            }
            else if (ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache("Action_Cache") == null)
            {
                List<ECN_Framework_Entities.Accounts.Action> lAction = null;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    lAction = ECN_Framework_DataLayer.Accounts.Action.GetAll(); 
                    scope.Complete();
                }

                ECN_Framework_Common.Functions.CacheHelper.AddToCache("Action_Cache", lAction);
                return lAction;
            }
            else
            {
                return (List<ECN_Framework_Entities.Accounts.Action>)ECN_Framework_Common.Functions.CacheHelper.GetCurrentCache("Action_Cache");
            }
        }
    }
}
