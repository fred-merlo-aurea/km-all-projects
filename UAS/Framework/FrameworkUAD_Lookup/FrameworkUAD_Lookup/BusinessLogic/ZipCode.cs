using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class ZipCode
    {
        public List<Entity.ZipCode> Select()
        {
            List<Entity.ZipCode> retList = null;
            retList = DataAccess.ZipCode.Select();
            return retList;
        }
    }
}
