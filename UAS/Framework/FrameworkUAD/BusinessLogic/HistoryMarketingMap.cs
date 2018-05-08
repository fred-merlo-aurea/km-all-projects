using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class HistoryMarketingMap
    {
        public List<Entity.HistoryMarketingMap> Select(int subscriberID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.HistoryMarketingMap> retList = null;
            retList = DataAccess.HistoryMarketingMap.Select(subscriberID,client);
            return retList;
        }

        public int Save(Entity.HistoryMarketingMap x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.HistoryMarketingMapID = DataAccess.HistoryMarketingMap.Save(x,client);
                scope.Complete();
            }

            return x.HistoryMarketingMapID;
        }
        public List<Entity.HistoryMarketingMap> SaveBulkUpdate(List<Entity.HistoryMarketingMap> list, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.HistoryMarketingMap> x = null;

            using (TransactionScope scope = new TransactionScope())
            {
                x = DataAccess.HistoryMarketingMap.SaveBulkUpdate(list,client);
                scope.Complete();
            }

            return x;
        }
    }
}
