using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class IssueComp
    {
        public List<Entity.IssueComp> SelectIssue(int issueID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueComp> x = DataAccess.IssueComp.SelectIssue(issueID, client);
            return x;
        }

        public int Save(Entity.IssueComp x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.IssueCompId = DataAccess.IssueComp.Save(x, client);
                scope.Complete();
            }

            return x.IssueCompId;
        }

        public bool JobSaveComplimentary(KMPlatform.Object.ClientConnections client, string processCode, int publicationID, int sourceFileID )
        {
            return DataAccess.IssueComp.JobSaveComplimentary(client, processCode, publicationID, sourceFileID);
        }
    }
}
