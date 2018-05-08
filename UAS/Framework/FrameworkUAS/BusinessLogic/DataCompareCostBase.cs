using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAS.BusinessLogic
{
    public class DataCompareCostBase
    {
        public List<Entity.DataCompareCostBase> Select()
        {
            List<Entity.DataCompareCostBase> x = null;
            x = DataAccess.DataCompareCostBase.Select().ToList();

            return x;
        }
    }
}
