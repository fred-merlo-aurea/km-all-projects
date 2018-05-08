using System;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class PaidOrderDetail
    {
        public List<Entity.PaidOrderDetail> SelectPaidOrder(int paidOrderId, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.PaidOrderDetail> retItem = null;
            retItem = DataAccess.PaidOrderDetail.SelectPaidOrder(paidOrderId, client);
            return retItem;
        }
        public List<Entity.PaidOrderDetail> SelectProductSubscription(int productSubscriptionId, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.PaidOrderDetail> item = null;
            item = DataAccess.PaidOrderDetail.SelectProductSubscription(productSubscriptionId, client);
            return item;
        }
        public List<Entity.PaidOrderDetail> SelectSubGenBundle(int subGenBundleId, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.PaidOrderDetail> item = null;
            item = DataAccess.PaidOrderDetail.SelectSubGenBundle(subGenBundleId, client);
            return item;
        }
        public int Save(Entity.PaidOrderDetail x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.PaidOrderId = DataAccess.PaidOrderDetail.Save(x, client);
                scope.Complete();
            }

            return x.PaidOrderId;
        }
        public bool SaveBulkInsert(List<Entity.PaidOrderDetail> x, KMPlatform.Object.ClientConnections client)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.PaidOrderDetail.SaveBulkInsert(x, client);
                scope.Complete();
                done = true;
            }

            return done;
        }
    }
}
