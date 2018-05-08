using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class FilterScheduleLog
    {
        public List<Entity.FilterScheduleLog> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.FilterScheduleLog> retList = null;
            retList = DataAccess.FilterScheduleLog.Select(client);
            return retList;
        }

        public int Save(KMPlatform.Object.ClientConnections client, Entity.FilterScheduleLog x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.FilterScheduleLogID = DataAccess.FilterScheduleLog.Save(client, x);
                scope.Complete();
            }

            return x.FilterScheduleLogID;
        }
    }
}
