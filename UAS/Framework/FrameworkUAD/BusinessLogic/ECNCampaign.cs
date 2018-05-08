using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.BusinessLogic
{
    public class ECNCampaign
    {
        public List<Entity.ECNCampaign> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ECNCampaign> x = null;
            x = DataAccess.ECNCampaign.Select(client).ToList();
            return x;
        }
    }
}
