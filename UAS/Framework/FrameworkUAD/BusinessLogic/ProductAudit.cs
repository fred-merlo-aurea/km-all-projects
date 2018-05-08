using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class ProductAudit
    {
        public List<Entity.ProductAudit> Select(int productId, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ProductAudit> x = null;
            x = DataAccess.ProductAudit.Select(productId, client).ToList();
            return x;
        }
        public bool Save(Entity.ProductAudit x, KMPlatform.Object.ClientConnections client)
        {
            bool success = false;
            using (TransactionScope scope = new TransactionScope())
            {
                success = DataAccess.ProductAudit.Save(x, client);
                scope.Complete();
            }

            return success;
        }
    }
}
