using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class MarketingMap
    {
        public Entity.MarketingMap Select(int marketingID, int subscriberID, int publicationID, KMPlatform.Object.ClientConnections client)
        {
            Entity.MarketingMap retItem = null;
            retItem = DataAccess.MarketingMap.Select(marketingID,subscriberID,publicationID, client);
            return retItem;
        }
        public List<Entity.MarketingMap> SelectPublication(int publicationID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.MarketingMap> retList = null;
            retList = DataAccess.MarketingMap.SelectPublication(publicationID, client);
            return retList;
        }
        public List<Entity.MarketingMap> SelectSubscriber(int PubSubscriptionID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.MarketingMap> retList = null;
            retList = DataAccess.MarketingMap.SelectSubscriber(PubSubscriptionID, client);
            return retList;
        }

        public bool Save(Entity.MarketingMap x, KMPlatform.Object.ClientConnections client)
        {
            bool saveDone = false; 
            using (TransactionScope scope = new TransactionScope())
            {
                saveDone = DataAccess.MarketingMap.Save(x, client);
                scope.Complete();
            }

            return saveDone;
        }

        public bool SaveBulkUpdate(List<Entity.MarketingMap> list, KMPlatform.Object.ClientConnections client)
        {
            bool done = true;
            int BatchSize = 500;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;

            List<Entity.MarketingMap> bulkUpdateList = new List<Entity.MarketingMap>();
            foreach (Entity.MarketingMap x in list)
            {
                counter++;
                processedCount++;
                done = false;
                bulkUpdateList.Add(x);
                if (processedCount == total || counter == BatchSize)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            DataAccess.MarketingMap.SaveBulkUpdate(bulkUpdateList, client);

                            scope.Complete();
                            done = true;
                        }
                        catch (Exception)
                        {
                            scope.Dispose();
                            done = false;

                        }
                    }
                    counter = 0;
                    bulkUpdateList = new List<Entity.MarketingMap>();
                }
            }

            return done;
        }
    }
}
