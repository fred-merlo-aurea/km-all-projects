using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class IssueCompDetail
    {
        public List<int> GetByFilter(string xml, string adHocXml, int issueCompID, KMPlatform.Object.ClientConnections client)
        {
            List<int> retList = new List<int>();
            retList = DataAccess.IssueCompDetail.GetByFilter(xml, adHocXml, issueCompID, client);
            return retList;
        }

        public List<Entity.IssueCompDetail> Select(int issueCompID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueCompDetail> x = DataAccess.IssueCompDetail.Select(issueCompID, client);
            return x;
        }

        public bool Clear(int issueCompID, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.IssueCompDetail.Clear(issueCompID, client);
        }

        public int Save(Entity.IssueCompDetail x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.IssueCompDetailId = DataAccess.IssueCompDetail.Save(x, client);
                scope.Complete();
            }

            return x.IssueCompDetailId;
        }

        public DataTable Select_For_Export(int issueID, string cols, List<int> subs, KMPlatform.Object.ClientConnections client)
        {
            DataTable dtCMS = new DataTable();
            string s = "<XML>";
            subs.ForEach(x => s += "<S><ID>" + x.ToString() + "</ID></S>");
            s += "</XML>";
            dtCMS = DataAccess.IssueCompDetail.Select_For_Export(issueID, cols, s, client);
            return dtCMS;
        }
    }
}
