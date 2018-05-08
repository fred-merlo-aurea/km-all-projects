using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class HistoryResponseMap
    {
        public List<Entity.HistoryResponseMap> Select(int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.HistoryResponseMap> retList = null;
            retList = DataAccess.HistoryResponseMap.Select(subscriptionID, client);
            return retList;
        }

        public int Save(Entity.HistoryResponseMap x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.HistoryResponseMapID = DataAccess.HistoryResponseMap.Save(x,client);
                scope.Complete();
            }

            return x.HistoryResponseMapID;
        }

        public List<Entity.HistoryResponseMap> SaveBulkUpdate(List<Entity.HistoryResponseMap> list, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.HistoryResponseMap> x = null;

            using (TransactionScope scope = new TransactionScope())
            {
                x = DataAccess.HistoryResponseMap.SaveBulkUpdate(list,client);
                scope.Complete();
            }

            return x;
        }
    }
}
