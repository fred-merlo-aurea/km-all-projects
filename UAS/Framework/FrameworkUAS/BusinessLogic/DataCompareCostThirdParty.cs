using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAS.BusinessLogic
{
    public class DataCompareCostThirdParty
    {
        public List<Entity.DataCompareCostThirdParty> Select(int clientId)
        {
            List<Entity.DataCompareCostThirdParty> x = null;
            x = DataAccess.DataCompareCostThirdParty.Select(clientId).ToList();

            return x;
        }
    }
}
