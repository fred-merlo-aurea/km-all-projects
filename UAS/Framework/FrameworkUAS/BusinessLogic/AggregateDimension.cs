using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAS.BusinessLogic
{
    public class AggregateDimension
    {
        public List<Entity.AggregateDimension> Select(int clientID)
        {
            List<Entity.AggregateDimension> x = null;
            x = DataAccess.AggregateDimension.Select(clientID).ToList();

            return x;
        }
       
    }
}
