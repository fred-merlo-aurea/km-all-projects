using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class WaveMailingDetail
    {
        public List<Entity.WaveMailingDetail> SelectIssue(int issueID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.WaveMailingDetail> x = DataAccess.WaveMailingDetail.SelectIssue(issueID, client);
            return x;
        }

        public bool UpdateOriginalSubInfo(int productID, int userID, KMPlatform.Object.ClientConnections client)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.WaveMailingDetail.UpdateOriginalSubInfo(productID, userID, client);
                scope.Complete();
            }

            return complete;
        }

        public int Save(Entity.WaveMailingDetail waveDetail, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                waveDetail.WaveMailingDetailID = DataAccess.WaveMailingDetail.Save(waveDetail, client);
                scope.Complete();
            }

            return waveDetail.WaveMailingDetailID;
        }
    }
}
