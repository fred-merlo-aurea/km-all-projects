using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class EmailStatus
    {
        public List<Entity.EmailStatus> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.EmailStatus> x = null;
            x = DataAccess.EmailStatus.Select(client).ToList();

            return x;
        }
        public Entity.EmailStatus Select(Enums.EmailStatus status, KMPlatform.Object.ClientConnections client)
        {
            return Select(client).Single(x => x.Status.Equals(status.ToString()));
        }
    }
}
