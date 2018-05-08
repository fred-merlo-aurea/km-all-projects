using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class Market
    {
        public List<Entity.Market> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Market> x = null;
            x = DataAccess.Market.Select(client).ToList();
            return x;
        }
        public List<Entity.Market> SelectByUserID(int UserID,KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Market> x = null;
            x = DataAccess.Market.SelectByUserID(UserID,client).ToList();
            return x;
        }

        public int Save(Entity.Market x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.BrandID = DataAccess.Market.Save(x, client);
                scope.Complete();
            }

            return x.BrandID;
        }
    }
}
