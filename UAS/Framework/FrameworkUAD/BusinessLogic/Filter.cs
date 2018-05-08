using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class Filter
    {
        public List<Entity.Filter> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Filter> x = null;
            x = DataAccess.Filter.Select(client).ToList();
            return x;
        }

        public bool Delete(int filterID, KMPlatform.Object.ClientConnections client)
        {
            bool x = DataAccess.Filter.Delete(filterID, client);
            return x;
        }

        public int Save(Entity.Filter x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.FilterID = DataAccess.Filter.Save(x, client);
                scope.Complete();
            }

            return x.FilterID;
        }
    }
}
