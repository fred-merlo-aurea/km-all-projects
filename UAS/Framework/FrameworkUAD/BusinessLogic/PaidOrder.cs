using System;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class PaidOrder
    {
        public List<Entity.PaidOrder> SelectSubscription(int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.PaidOrder> retItem = null;
            retItem = DataAccess.PaidOrder.SelectSubscription(subscriptionID, client);
            return retItem;
        }
        public List<Entity.PaidOrder> SelectSubGenSubscriber(int subGenSubscriberId, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.PaidOrder> item = null;
            item = DataAccess.PaidOrder.SelectSubGenSubscriber(subGenSubscriberId, client);
            return item;
        }

        public int Save(Entity.PaidOrder x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.PaidOrderId = DataAccess.PaidOrder.Save(x, client);
                scope.Complete();
            }

            return x.PaidOrderId;
        }
    }
}
