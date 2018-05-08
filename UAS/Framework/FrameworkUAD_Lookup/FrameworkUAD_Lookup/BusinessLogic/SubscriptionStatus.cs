using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class SubscriptionStatus
    {
        public List<Entity.SubscriptionStatus> Select()
        {
            List<Entity.SubscriptionStatus> retList = null;
            retList = DataAccess.SubscriptionStatus.Select();
            return retList;
        }
        public Entity.SubscriptionStatus Select(int categoryCodeID,int transactionCodeID)
        {
            Entity.SubscriptionStatus retItem = null;
            retItem = DataAccess.SubscriptionStatus.Select(categoryCodeID, transactionCodeID);
            return retItem;
        }
        public Entity.SubscriptionStatus Select(int subscriptionStatusID)
        {
            Entity.SubscriptionStatus retItem = null;
            retItem = DataAccess.SubscriptionStatus.Select(subscriptionStatusID);
            return retItem;
        }
        
        public int Save(Entity.SubscriptionStatus x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.SubscriptionStatusID = DataAccess.SubscriptionStatus.Save(x);
                scope.Complete();
            }

            return x.SubscriptionStatusID;
        }
    }
}
