using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAS.BusinessLogic
{
    public class SecurityGroupServiceMap
    {
        public List<Entity.SecurityGroupServiceMap> Select()
        {
            List<Entity.SecurityGroupServiceMap> x = null;
            x = DataAccess.SecurityGroupServiceMap.Select().ToList();

            return x;
        }
    }
}
