using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class WaveMailing
    {
        public List<Entity.WaveMailing> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.WaveMailing> x = DataAccess.WaveMailing.Select(client);
            return x;
        }

        public int Save(Entity.WaveMailing x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.WaveMailingID = DataAccess.WaveMailing.Save(x, client);
                scope.Complete();
            }

            return x.WaveMailingID;
        }
    }
}
