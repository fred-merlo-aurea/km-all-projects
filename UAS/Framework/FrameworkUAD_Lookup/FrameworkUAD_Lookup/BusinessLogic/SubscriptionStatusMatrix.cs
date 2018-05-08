using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class SubscriptionStatusMatrix
    {
        public List<Entity.SubscriptionStatusMatrix> Select()
        {
            List<Entity.SubscriptionStatusMatrix> retList = null;
            retList = DataAccess.SubscriptionStatusMatrix.Select();
            return retList;
        }
        public Entity.SubscriptionStatusMatrix Select(int subscriptionStatusID, int categoryCodeID, int transactionCodeID)
        {
            Entity.SubscriptionStatusMatrix item = null;
            item = DataAccess.SubscriptionStatusMatrix.Select(subscriptionStatusID, categoryCodeID, transactionCodeID);
            return item;
        }
       
        public int Save(Entity.SubscriptionStatusMatrix x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.StatusMatrixID = DataAccess.SubscriptionStatusMatrix.Save(x);
                scope.Complete();
            }

            return x.StatusMatrixID;
        }
    }
}
