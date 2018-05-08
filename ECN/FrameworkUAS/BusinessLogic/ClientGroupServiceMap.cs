using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class ClientGroupServiceMap
    {
        public List<Entity.ClientGroupServiceMap> Select()
        {
            List<Entity.ClientGroupServiceMap> x = null;
            x = DataAccess.ClientGroupServiceMap.Select();

            foreach (Entity.ClientGroupServiceMap cs in x)
            {
                ClientGroupServiceFeatureMap csfData = new ClientGroupServiceFeatureMap();
                cs.Features = csfData.SelectForService(cs.ClientGroupID, cs.ServiceID).ToList();
            }
            return x;
        }
        public List<Entity.ClientGroupServiceMap> Select(int clientGroupID)
        {
            List<Entity.ClientGroupServiceMap> x = null;
            x = DataAccess.ClientGroupServiceMap.Select(clientGroupID);

            foreach (Entity.ClientGroupServiceMap cs in x)
            {
                ClientGroupServiceFeatureMap csfData = new ClientGroupServiceFeatureMap();
                cs.Features = csfData.SelectForService(cs.ClientGroupID, cs.ServiceID).ToList();
            }
            return x;
        }
       
        public int Save(Entity.ClientGroupServiceMap x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.ClientGroupServiceMapID = DataAccess.ClientGroupServiceMap.Save(x);
                scope.Complete();
            }

            return x.ClientGroupServiceMapID;
        }
    }
}
