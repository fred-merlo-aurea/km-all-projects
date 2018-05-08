using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace KMPlatform.BusinessLogic
{
    public class ClientServiceFeatureMap
    {
        public int Save(Entity.ClientServiceFeatureMap x)
        {
            return DataAccess.ClientServiceFeatureMap.Save(x);
        }
    }
}
