using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class RegionGroup
    {
        public List<Entity.RegionGroup> Select()
        {
            List<Entity.RegionGroup> retList = null;
            retList = DataAccess.RegionGroup.Select();

            return retList;
        }
    }
}
