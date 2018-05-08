using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class FilterCategory
    {
        public List<Entity.FilterCategory> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.FilterCategory> x = null;
            x = DataAccess.FilterCategory.Select(client).ToList();
            return x;
        }        
    }
}
