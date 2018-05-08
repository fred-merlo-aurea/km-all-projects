using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class SubscriberAddKill
    {
        public List<Entity.SubscriberAddKill> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriberAddKill> retList = null;
            retList = DataAccess.SubscriberAddKill.Select(client);
            return retList;
        }

        public int UpdateSubscription(int addKillID, int productID, string subscriptionIDs, bool deleteAddRemoveID, KMPlatform.Object.ClientConnections client)
        {
            int result = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                result = DataAccess.SubscriberAddKill.UpdateSubscription(addKillID, productID, subscriptionIDs, deleteAddRemoveID, client);
                scope.Complete();
            }

            return result;
        }

        public int Save(Entity.SubscriberAddKill subAddKill, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                subAddKill.AddKillID = DataAccess.SubscriberAddKill.Save(subAddKill, client);
                scope.Complete();
            }

            return subAddKill.AddKillID;
        }

        public bool BulkInsertDetail(List<FrameworkUAD.Entity.SubscriberAddKillDetail> subs, int addRemoveID, KMPlatform.Object.ClientConnections client)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.SubscriberAddKill.BulkInsertDetail(subs, addRemoveID, client);
                scope.Complete();
            }

            return complete;
        }

        public bool ClearDetails(int productID, KMPlatform.Object.ClientConnections client)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.SubscriberAddKill.ClearDetails(productID, client);
                scope.Complete();
            }

            return complete;
        }
    }
}
