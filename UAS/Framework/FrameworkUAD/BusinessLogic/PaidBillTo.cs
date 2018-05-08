using System;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class PaidBillTo
    {
        public Entity.PaidBillTo SelectSubscription(int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            Entity.PaidBillTo retItem = null;
            retItem = DataAccess.PaidBillTo.SelectSubscription(subscriptionID, client);
            return retItem;
        }
        public Entity.PaidBillTo Select(int subscriptionPaidID, KMPlatform.Object.ClientConnections client)
        {
            Entity.PaidBillTo item = null;
            item = DataAccess.PaidBillTo.Select(subscriptionPaidID, client);
            return item;
        }

        public int Save(Entity.PaidBillTo x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.PaidBillToID = DataAccess.PaidBillTo.Save(x, client);
                scope.Complete();
            }

            return x.PaidBillToID;
        }
    }
}
