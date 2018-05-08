using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class IssueSplit
    {
        public List<Entity.IssueSplit> SelectIssueID(int issueID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueSplit> x = DataAccess.IssueSplit.SelectIssueID(issueID, client);
            return x;
        }
       
        public bool ClearIssue(int issueID, KMPlatform.Object.ClientConnections client)
        {
            bool x = DataAccess.IssueSplit.ClearIssue(issueID, client);
            return x;
        }

        public bool Save(List<Entity.IssueSplit> x, int issueID, KMPlatform.Object.ClientConnections client)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                ClearIssue(issueID, client);
                foreach (Entity.IssueSplit split in x)
                {
                    split.FilterId = DataAccess.Filter.Save(split.Filter, client);
                    split.IssueSplitId = DataAccess.IssueSplit.Save(split, client);
                }
                scope.Complete();
                complete = true;
            }

            return complete;
        }
        public  int GetSubscriberCopiesCount(DataTable dtIssueSplitPubs, KMPlatform.Object.ClientConnections client)
        {
           return Convert.ToInt32(DataAccess.IssueSplit.GetSubscriberCopiesCount(dtIssueSplitPubs, client));

        }

        public int SaveNew(Entity.IssueSplit x, DataTable dtIssueSplitPubs, KMPlatform.Object.ClientConnections client)
        {
            int IssueSplitId = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                x.IssueSplitId = DataAccess.IssueSplit.SaveNew(x, dtIssueSplitPubs, client);
                scope.Complete();
                IssueSplitId = x.IssueSplitId;
            }
            return IssueSplitId;
        }
        public int UpdateIssueSplit(Entity.IssueSplit x, KMPlatform.Object.ClientConnections client)
        {
            int IssueSplitId = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                IssueSplitId = DataAccess.IssueSplit.UpdateIssueSplit(x, client);
                scope.Complete();
                
            }
            return IssueSplitId;
        }
    }
}
