using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class ClientGroupServiceFeatureMap
    {
        public List<Entity.ClientGroupServiceFeatureMap> SelectForClientGroup(int clientGroupID)
        {
            List<Entity.ClientGroupServiceFeatureMap> x = null;
            x = DataAccess.ClientGroupServiceFeatureMap.SelectForClientGroup(clientGroupID);

            return x;
        }
        public List<Entity.ClientGroupServiceFeatureMap> SelectForServiceFeature(int clientGroupID, int serviceFeatureID)
        {
            List<Entity.ClientGroupServiceFeatureMap> x = null;
            x = DataAccess.ClientGroupServiceFeatureMap.SelectForServiceFeature(clientGroupID, serviceFeatureID);

            return x;
        }
        public List<Entity.ClientGroupServiceFeatureMap> SelectForService(int clientGroupID, int serviceID)
        {
            List<Entity.ClientGroupServiceFeatureMap> x = null;
            x = DataAccess.ClientGroupServiceFeatureMap.SelectForService(clientGroupID, serviceID);

            return x;
        }
       
        public int Save(Entity.ClientGroupServiceFeatureMap x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.ClientGroupServiceFeatureMapID = DataAccess.ClientGroupServiceFeatureMap.Save(x);
                scope.Complete();
            }

            return x.ClientGroupServiceFeatureMapID;
        }
    }
}
