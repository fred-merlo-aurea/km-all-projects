using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class Frequency
    {
        public List<Entity.Frequency> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Frequency> retList = null;
            retList = DataAccess.Frequency.Select(client);
            return retList;
        }
    }
}
