using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class IssueArchiveProductSubscriptionDetail
    {
        public bool SaveBulkSqlInsert(List<Entity.IssueArchiveProductSubscriptionDetail> list, KMPlatform.Object.ClientConnections client)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.IssueArchiveProductSubscriptionDetail.SaveBulkSqlInsert(list, client);
                scope.Complete();
            }

            return complete;
        }

        public bool Save(Entity.IssueArchiveProductSubscriptionDetail x, KMPlatform.Object.ClientConnections client)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.IssueArchiveProductSubscriptionDetail.Save(x, client);
                scope.Complete();
            }

            return done;
        }

        public List<Entity.IssueArchiveProductSubscriptionDetail> SelectForUpdate(int productID, int issueid, List<int> pubsubs, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueArchiveProductSubscriptionDetail> retItem = null;
            string s = "<XML>";
            pubsubs.ForEach(x => s += "<S><ID>" + x.ToString() + "</ID></S>");
            s += "</XML>";
            retItem = DataAccess.IssueArchiveProductSubscriptionDetail.SelectForUpdate(productID, issueid, s, client);

            return retItem;
        }

        public List<FrameworkUAD.Entity.IssueArchiveProductSubscriptionDetail> IssueArchiveProductSubscriptionDetailUpdateBulkSql(KMPlatform.Object.ClientConnections client, List<Entity.IssueArchiveProductSubscriptionDetail> list)
        {
            int BatchSize = 500;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;

            List<Entity.IssueArchiveProductSubscriptionDetail> returnList = new List<Entity.IssueArchiveProductSubscriptionDetail>();
            List<Entity.IssueArchiveProductSubscriptionDetail> bulkUpdateList = new List<Entity.IssueArchiveProductSubscriptionDetail>();
            foreach (Entity.IssueArchiveProductSubscriptionDetail x in list)
            {
                counter++;
                processedCount++;
                bulkUpdateList.Add(x);
                if (processedCount == total || counter == BatchSize)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            returnList.AddRange(DataAccess.IssueArchiveProductSubscriptionDetail.IssueArchiveProductSubscriptionDetailUpdateBulkSql(client, bulkUpdateList));

                            scope.Complete();
                        }
                        catch (Exception)
                        {
                            scope.Dispose();
                        }
                    }
                    counter = 0;
                    bulkUpdateList = new List<Entity.IssueArchiveProductSubscriptionDetail>();
                }
            }

            return returnList;
        }
    }
}
