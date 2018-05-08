using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class ProductGroup
    {
        public List<Entity.ProductGroup> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ProductGroup> x = null;
            x = DataAccess.ProductGroup.Select(client).ToList();
            return x;
        }

        public int Save(Entity.ProductGroup x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.PubID = DataAccess.ProductGroup.Save(x, client);
                scope.Complete();
            }

            return x.PubID;
        }

        public bool Delete(KMPlatform.Object.ClientConnections client, int PubID)
        {
            bool delete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                delete = DataAccess.ProductGroup.Delete(client, PubID);
                scope.Complete();
            }

            return delete;
        }
    }
}
