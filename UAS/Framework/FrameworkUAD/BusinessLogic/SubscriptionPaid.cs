using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class SubscriptionPaid
    {
        public List<Entity.SubscriptionPaid> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriptionPaid> x = null;
            x = DataAccess.SubscriptionPaid.Select(client).ToList();
            return x;
        }
        public Entity.SubscriptionPaid Select(int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            Entity.SubscriptionPaid x = null;
            x = DataAccess.SubscriptionPaid.Select(subscriptionID,client);
            return x;
        }

        public int Save(Entity.SubscriptionPaid x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.SubscriptionPaidID = DataAccess.SubscriptionPaid.Save(x,client);
                scope.Complete();
            }

            return x.SubscriptionPaidID;
        }
        public int ImportFromSubGen(string processCode, KMPlatform.Object.ClientConnections client)
        {
            int subscriptionPaidId = 0;

                subscriptionPaidId = DataAccess.SubscriptionPaid.ImportFromSubGen(processCode, client);

            return subscriptionPaidId;
        }
        public bool Save(List<Entity.SubscriptionPaid> spList, KMPlatform.Object.ClientConnections client)
        {
            bool success = false;
            using (TransactionScope scope = new TransactionScope())
            {
                success = DataAccess.SubscriptionPaid.Save(spList, client);
                scope.Complete();
            }
            return success;
        }
    }
}
