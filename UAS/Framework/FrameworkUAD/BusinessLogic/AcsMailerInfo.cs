using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class AcsMailerInfo
    {
        public Entity.AcsMailerInfo SelectByID(int acsMailerInfoId, KMPlatform.Object.ClientConnections client)
        {
            Entity.AcsMailerInfo x = DataAccess.AcsMailerInfo.SelectByID(acsMailerInfoId,client);
            return x;
        }

        public List<Entity.AcsMailerInfo> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.AcsMailerInfo> x = DataAccess.AcsMailerInfo.Select(client);
            return x;
        }

        public int Save(Entity.AcsMailerInfo x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.AcsMailerInfoId = DataAccess.AcsMailerInfo.Save(x,client);
                scope.Complete();
            }

            return x.AcsMailerInfoId;
        }
    }
}
