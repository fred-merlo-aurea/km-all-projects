using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class Region
    {
        public  List<Entity.Region> Select()
        {
            List<Entity.Region> retList = null;
            retList = DataAccess.Region.Select();

            return retList;
        }

        public static IEnumerable<SelectListItem> GetStatesforSelectList()
        {
            return DataAccess.Region.Select().OrderBy(x => x.CountryID).ThenBy(t => t.RegionName).Select(i => new SelectListItem()
            {
                Text = i.RegionName,
                Value = i.RegionID.ToString()
            });
        }
    }
}
