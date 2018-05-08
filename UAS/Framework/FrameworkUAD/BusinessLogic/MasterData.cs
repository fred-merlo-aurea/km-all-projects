using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class MasterData
    {
        public List<Object.MasterData> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Object.MasterData> x = null;
            x = DataAccess.MasterData.Select(client).ToList();
            return x;
        }
    }
}
