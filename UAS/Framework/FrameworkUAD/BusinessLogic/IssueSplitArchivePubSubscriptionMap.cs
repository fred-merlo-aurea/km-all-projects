using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.BusinessLogic
{
    public class IssueSplitArchivePubSubscriptionMap
    {
        public  List<Entity.IssueSplitArchivePubSubscriptionMap> SelectIssueSplitPubsMapping(int issueSplitID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueSplitArchivePubSubscriptionMap> x = DataAccess.IssueSplitArchivePubSubscriptionMap.SelectIssueSplitPubsMapping(issueSplitID, client);
            return x;
        }

        public  int MoveSplitRecords(int ToIssueSplitID,int FromIssueSplitID,int MovedRecordCount, DataTable dtIssueSplitPubs, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.IssueSplitArchivePubSubscriptionMap.MoveSplitRecords(ToIssueSplitID, FromIssueSplitID, MovedRecordCount, dtIssueSplitPubs, client);

        }
    }
}
