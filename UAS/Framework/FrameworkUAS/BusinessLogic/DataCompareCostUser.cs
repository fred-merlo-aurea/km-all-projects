using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAS.BusinessLogic
{
    public class DataCompareCostUser
    {
        public List<Entity.DataCompareCostUser> Select(int userId)
        {
            List<Entity.DataCompareCostUser> x = null;
            x = DataAccess.DataCompareCostUser.Select(userId).ToList();

            return x;
        }
    }
}
