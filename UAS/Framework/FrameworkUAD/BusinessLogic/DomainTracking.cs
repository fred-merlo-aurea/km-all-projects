using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.BusinessLogic
{
    public class DomainTracking
    {
        public List<Entity.DomainTracking> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.DomainTracking> x = null;
            x = DataAccess.DomainTracking.Select(client).ToList();
            return x;
        }
    }
}
