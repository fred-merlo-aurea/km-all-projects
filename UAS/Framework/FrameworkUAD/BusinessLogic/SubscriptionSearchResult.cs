using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class SubscriptionSearchResult
    {
        public List<Object.SubscriptionSearchResult> Select(int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            List<Object.SubscriptionSearchResult> x = null;
            x = DataAccess.SubscriptionSearchResult.Select(subscriptionID, client);
            return x;
        }

        public List<Object.SubscriptionSearchResult> SelectMultiple(List<int> subscriberIds, KMPlatform.Object.ClientConnections client)
        {
            int BatchSize = 500;// Probably wont ever go above 500
            int total = subscriberIds.Count;
            int counter = 0;
            int processedCount = 0;

            List<int> bulkList = new List<int>();
            List<Object.SubscriptionSearchResult> list = new List<Object.SubscriptionSearchResult>();
            List<Object.SubscriptionSearchResult> ret = new List<Object.SubscriptionSearchResult>();
            foreach (int x in subscriberIds)
            {
                counter++;
                processedCount++;
                bulkList.Add(x);
                if (processedCount == total || counter == BatchSize)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            ret = DataAccess.SubscriptionSearchResult.SelectMultiple(bulkList, client);

                            scope.Complete();
                        }
                        catch (Exception)
                        {
                            scope.Dispose();
                        }
                    }
                    counter = 0;
                    bulkList = new List<int>();
                    foreach (var r in ret)
                    {
                        list.Add(r);
                    }
                }
            }

            return list;
        }
        
    }
}
