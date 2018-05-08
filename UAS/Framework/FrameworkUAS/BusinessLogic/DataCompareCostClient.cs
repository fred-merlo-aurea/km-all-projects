using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAS.BusinessLogic
{
    public class DataCompareCostClient
    {
        public List<Entity.DataCompareCostClient> Select(int clientId)
        {
            List<Entity.DataCompareCostClient> x = null;
            x = DataAccess.DataCompareCostClient.Select(clientId).ToList();

            return x;
        }
    }
}
