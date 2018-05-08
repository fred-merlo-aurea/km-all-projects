using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class PubCode
    {
        public List<Object.PubCode> Select(string dbName)
        {
            List<Object.PubCode> x = null;
            x = DataAccess.PubCode.Select(dbName).ToList();
            return x;
        }

        public List<Object.PubCode> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Object.PubCode> x = null;
            x = DataAccess.PubCode.Select(client).ToList();
            return x;
        }
    }
}
