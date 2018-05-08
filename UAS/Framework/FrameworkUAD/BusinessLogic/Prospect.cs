using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class Prospect
    {
        public List<Entity.Prospect> Select(int subscriberID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Prospect> x = null;
            x = DataAccess.Prospect.Select(subscriberID, client).ToList();
            return x;
        }

        public bool Save(Entity.Prospect x, KMPlatform.Object.ClientConnections client)
        {
            bool saveDone = false;
            using (TransactionScope scope = new TransactionScope())
            {
                saveDone = DataAccess.Prospect.Save(x, client);
                scope.Complete();
            }

            return saveDone;
        }
    }
}
