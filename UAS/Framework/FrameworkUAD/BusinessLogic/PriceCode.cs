using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class PriceCode
    {
        public List<Entity.PriceCode> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.PriceCode> retList = null;
            retList = DataAccess.PriceCode.Select(client);
            return retList;
        }

        public List<Entity.PriceCode> Select(int publicationID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.PriceCode> retList = null;
            retList = DataAccess.PriceCode.Select(publicationID, client);
            return retList;
        }

        public Entity.PriceCode Select(string priceCode, int publicationID, KMPlatform.Object.ClientConnections client)
        {
            Entity.PriceCode retItem = null;
            retItem = DataAccess.PriceCode.Select(priceCode, publicationID, client);
            return retItem;
        }

        public int Save(Entity.PriceCode x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.PriceCodeID = DataAccess.PriceCode.Save(x, client);
                scope.Complete();
            }

            return x.PriceCodeID;
        }
    }
}
