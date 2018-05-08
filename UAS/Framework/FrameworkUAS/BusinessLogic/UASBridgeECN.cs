using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAS.BusinessLogic
{
    public class UASBridgeECN
    {
        public List<Entity.UASBridgeECN> Select(int userID)
        {
            List<Entity.UASBridgeECN> x = null;
            x = DataAccess.UASBridgeECN.Select(userID);

            return x;
        }
    }
}
