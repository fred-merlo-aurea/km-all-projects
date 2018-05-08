using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class IssueArchiveProductSubscription
    {
        public List<Entity.IssueArchiveProductSubscription> SelectIssue(int issueID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueArchiveProductSubscription> x = DataAccess.IssueArchiveProductSubscription.SelectIssue(issueID, client);
            return x;
        }

        public List<Entity.IssueArchiveProductSubscription> SelectPaging(int page, int pageSize, int issueID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueArchiveProductSubscription> x = null;
            x = DataAccess.IssueArchiveProductSubscription.SelectPaging(page, pageSize, issueID, client);
            return x;
        }

        public List<Entity.IssueArchiveProductSubscription> SelectForUpdate(int productID, int issueid, List<int> pubsubs, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueArchiveProductSubscription> retItem = null;
            string s = "<XML>";
            pubsubs.ForEach(x => s += "<S><ID>" + x.ToString() + "</ID></S>");
            s += "</XML>";
            retItem = DataAccess.IssueArchiveProductSubscription.SelectForUpdate(productID, issueid, s, client);

            return retItem;
        }

        public int SelectCount(int issueID, KMPlatform.Object.ClientConnections client)
        {
            int x = 0;
            x = DataAccess.IssueArchiveProductSubscription.SelectCount(issueID, client);
            return x;
        }

        public bool SaveBulkSqlInsert(List<Entity.IssueArchiveProductSubscription> list, KMPlatform.Object.ClientConnections client)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.IssueArchiveProductSubscription.SaveBulkSqlInsert(list, client);
                scope.Complete();
            }

            return complete;
        }

        public int Save(Entity.IssueArchiveProductSubscription x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.IssueArchiveSubscriptionId = DataAccess.IssueArchiveProductSubscription.Save(x, client);
                scope.Complete();
            }

            return x.IssueArchiveSubscriptionId;
        }

        public int SaveAll(Entity.IssueArchiveProductSubscription x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.IssueArchiveSubscriptionId = DataAccess.IssueArchiveProductSubscription.SaveAll(x, client);
                scope.Complete();
            }

            return x.IssueArchiveSubscriptionId;
        }
    }
}
