using System;
using System.Collections.Generic;
using System.Linq;
//using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class ClientConfigurationMap
    {
        public List<Entity.ClientConfigurationMap> Select()
        {
            List<Entity.ClientConfigurationMap> x = null;
            x = DataAccess.ClientConfigurationMap.Select().ToList();
            return x;
        }
        public List<Entity.ClientConfigurationMap> Select(int clientID)
        {
            List<Entity.ClientConfigurationMap> x = null;
            x = DataAccess.ClientConfigurationMap.Select(clientID).ToList();
            return x;
        }
    }
}
