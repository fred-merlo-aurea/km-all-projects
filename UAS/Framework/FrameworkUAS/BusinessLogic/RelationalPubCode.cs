using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAS.BusinessLogic
{
    public class RelationalPubCode
    {
        public List<Entity.RelationalPubCode> Select(int clientID)
        {
            List<Entity.RelationalPubCode> x = null;
            x = DataAccess.RelationalPubCode.Select(clientID);

            return x;
        }
        public List<Entity.RelationalPubCode> Select(int clientID, string specialFileName)
        {
            List<Entity.RelationalPubCode> x = null;
            x = DataAccess.RelationalPubCode.Select(clientID, specialFileName);

            return x;
        }
   
    }
}
