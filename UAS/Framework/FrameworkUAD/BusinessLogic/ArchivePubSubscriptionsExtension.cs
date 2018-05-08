using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class ArchivePubSubscriptionsExtension
    {
        public List<Entity.ArchivePubSubscriptionsExtension> SelectForUpdate(int productID, int issueid, List<int> pubsubs, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ArchivePubSubscriptionsExtension> retItem = null;
            string s = "<XML>";
            pubsubs.ForEach(x => s += "<S><ID>" + x.ToString() + "</ID></S>");
            s += "</XML>";
            retItem = DataAccess.ArchivePubSubscriptionsExtension.SelectForUpdate(productID, issueid, s, client);

            return retItem;
        }

        public List<FrameworkUAD.Object.PubSubscriptionAdHoc> GetArchiveAdhocs(KMPlatform.Object.ClientConnections client, int pubSubID, int productID, int issueID)
        {
            DataTable result = new DataTable();
            List<FrameworkUAD.Object.PubSubscriptionAdHoc> lst = new List<Object.PubSubscriptionAdHoc>();
            result = DataAccess.ArchivePubSubscriptionsExtension.GetArchiveAdhocs(client, pubSubID, productID, issueID);
            Dictionary<string, string> adhocs = new Dictionary<string, string>();
            foreach (DataRow dr in result.Rows)
            {
                foreach (DataColumn dc in result.Columns)
                {
                    lst.Add(new FrameworkUAD.Object.PubSubscriptionAdHoc(dc.ColumnName, dr[dc].ToString()));
                }
            }
            return lst;
        }

        public bool Save(List<FrameworkUAD.Object.PubSubscriptionAdHoc> x, int issueArchiveSubscriptionID, int pubID, KMPlatform.Object.ClientConnections client)
        {
            bool save = true;
            using (TransactionScope scope = new TransactionScope())
            {
                save = DataAccess.ArchivePubSubscriptionsExtension.Save(client, issueArchiveSubscriptionID, pubID, x);
                scope.Complete();
            }

            return save;
        }
    }
}
