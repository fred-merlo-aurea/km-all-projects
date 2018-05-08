using System;
//using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace KMPlatform.BusinessLogic
{
    public class ClientServiceMap
    {
        public int Save( Entity.ClientServiceMap x)
        {
            return DataAccess.ClientServiceMap.Save(x);
        }
    }
}
