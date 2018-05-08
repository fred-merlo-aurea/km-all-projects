using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class Issue
    {
        public List<Entity.Issue> SelectPublication(int publicationID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Issue> x = DataAccess.Issue.SelectPublication(publicationID,client);
            return x;
        }

        public List<Entity.Issue> SelectPublisher(int publisherID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Issue> x = DataAccess.Issue.SelectPublisher(publisherID,client);
            return x;
        }

        public List<Entity.Issue> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Issue> x = DataAccess.Issue.Select(client);
            return x;
        }

        public bool ArchiveAll(int productID, int issueID, Dictionary<int, string> imb, Dictionary<int, string> compImb, KMPlatform.Object.ClientConnections client)
        {
            bool complete = false;
            int batchSize = 1000;
            int imbTotal = imb.Count;
            int compImbTotal = compImb.Count;
            int counter = 0;
            int processed = 0;
            string imbSeq = "<XML>";
            string compImbSeq = "<XML>";
            foreach (KeyValuePair<int, string> kv in imb)
            {
                counter++;
                processed++;
                imbSeq += "<PS><ID>" + kv.Key + "</ID><IMB>" + kv.Value + "</IMB></PS>";
                if(processed == imbTotal || counter == batchSize)
                {
                    imbSeq += "</XML>";
                    compImbSeq += "</XML>";
                    using (TransactionScope scope = new TransactionScope())
                    {
                        complete = DataAccess.Issue.ArchiveAllIMB(productID, issueID, imbSeq, client);
                        scope.Complete();
                    }
                    imbSeq = "<XML>";
                    compImbSeq = "<XML>";
                    counter = 0;
                }
            }
            counter = 0;
            processed = 0;
            compImbSeq = "<XML>";
            if (compImb.Count > 0)
            {
                foreach (KeyValuePair<int, string> kv in compImb)
                {
                    counter++;
                    processed++;
                    compImbSeq += "<IS><ID>" + kv.Key + "</ID><IMB>" + kv.Value + "</IMB></IS>";
                    if (processed == compImbTotal || counter == batchSize)
                    {
                        compImbSeq += "</XML>";
                        using (TransactionScope scope = new TransactionScope())
                        {
                            complete = DataAccess.IssueCompDetail.ArchiveAll(productID, issueID, compImbSeq, client);
                            scope.Complete();
                        }
                        compImbSeq = "<XML>";
                        counter = 0;
                    }
                }
            }
            //This will archive all remaining records that were not exported with IMBSequence #s.
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.Issue.ArchiveAll(productID, issueID, client);
                DataAccess.ProductSubscription.ClearIMBSeq(productID, client);
                scope.Complete();
            }

            return complete;
        }

        public int Save(Entity.Issue x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.IssueId = DataAccess.Issue.Save(x, client);
                scope.Complete();
            }

            return x.IssueId;
        }

        public bool BulkInsertSubGenIDs(List<Entity.IssueCloseSubGenMap> ids, KMPlatform.Object.ClientConnections client)
        {
            bool result = false;
            using (TransactionScope scope = new TransactionScope())
            {
                result = DataAccess.Issue.BulkInsertSubGenIDs(ids,client);
                scope.Complete();
            }

            return result;
        }
        public bool ValidateArchive(int pubId, int issueId, KMPlatform.Object.ClientConnections client)
        {
            bool result = false;

            result = DataAccess.Issue.ValidateArchive(pubId,issueId,client);

            return result;
            
        }
        public bool RollBackIssue(int pubId, int issueId, int origIMB, KMPlatform.Object.ClientConnections client)
        {
            bool result = false;
        
            result = DataAccess.Issue.RollBackIssue(pubId, issueId, origIMB, client);

            return result;

        }
    }
}
