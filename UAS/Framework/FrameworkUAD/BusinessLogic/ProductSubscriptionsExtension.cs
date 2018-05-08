using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class ProductSubscriptionsExtension
    {
        public bool Save(List<FrameworkUAD.Object.PubSubscriptionAdHoc> x, int pubSubscriptionID, int pubID, KMPlatform.Object.ClientConnections client)
        {
            bool save = true;
            using (TransactionScope scope = new TransactionScope())
            {
                save = DataAccess.PubSubscriptionsExtension.Save(client, pubSubscriptionID, pubID, x);
                scope.Complete();
            }

            return save;
        }
    }
}
