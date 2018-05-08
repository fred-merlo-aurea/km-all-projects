using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class ReportGroups
    {
        public bool ExistsByIDName(int reportGroupID, int responseGroupID, string name, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.ReportGroups.ExistsByIDName(reportGroupID, responseGroupID, name, client);
            return exists;
        }

        public List<Entity.ReportGroups> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ReportGroups> retList = null;
            retList = DataAccess.ReportGroups.Select(client);
            return retList;
        }

        public int Save(KMPlatform.Object.ClientConnections client, Entity.ReportGroups x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.ReportGroupID = DataAccess.ReportGroups.Save(client, x);
                scope.Complete();
            }

            return x.ReportGroupID;
        }
    }
}
