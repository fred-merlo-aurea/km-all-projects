using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class RegionMap
    {
        public  List<Entity.RegionMap> Select()
        {
            List<Entity.RegionMap> retList = null;
            retList = DataAccess.RegionMap.Select();

            return retList;
        }
    
    }
}
