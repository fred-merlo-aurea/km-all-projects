using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class Databases
    {
        public List<Entity.Databases> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Databases> x = null;
            x = DataAccess.Databases.Select(client).ToList();
            return x;
        }
      
    }
}
