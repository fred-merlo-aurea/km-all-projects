using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class Brand
    {
        public List<Entity.Brand> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Brand> x = null;
            x = DataAccess.Brand.Select(client).ToList();
            return x;
        }

        public List<Entity.Brand> SelectByUserID(int UserID,KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Brand> x = null;
            x = DataAccess.Brand.SelectByUserID(UserID, client).ToList();
            return x;
        }
        public List<Entity.Brand> SelectByPubID(int PubID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Brand> x = null;
            x = DataAccess.Brand.SelectByPubID(PubID, client).ToList();
            return x;
        }

        public int Save(Entity.Brand x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.BrandID = DataAccess.Brand.Save(x, client);
                scope.Complete();
            }

            return x.BrandID;
        }
    }
}
