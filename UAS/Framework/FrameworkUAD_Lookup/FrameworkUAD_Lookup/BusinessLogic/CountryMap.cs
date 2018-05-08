using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class CountryMap
    {
        public  List<Entity.CountryMap> Select()
        {
            List<Entity.CountryMap> retList = null;
            retList = DataAccess.CountryMap.Select();

            return retList;
        }
      
    }
}
