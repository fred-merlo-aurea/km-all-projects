using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class BlastRSS
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.BlastRSS;

        public static int Save(ECN_Framework_Entities.Communicator.BlastRSS rss)
        {
            int retID = -1;
            using(TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Communicator.BlastRSS.Save(rss);
                scope.Complete();
            }
            return retID;
        }

        public static List<ECN_Framework_Entities.Communicator.BlastRSS> GetByBlastID(int blastID)
        {
            List<ECN_Framework_Entities.Communicator.BlastRSS> rss = new List<ECN_Framework_Entities.Communicator.BlastRSS>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                rss = ECN_Framework_DataLayer.Communicator.BlastRSS.GetByBlastID(blastID);
                scope.Complete();
            }
            return rss;
        }

        public static ECN_Framework_Entities.Communicator.BlastRSS GetByBlastID_FeedID(int blastID, int feedID)
        {
            ECN_Framework_Entities.Communicator.BlastRSS rss = new ECN_Framework_Entities.Communicator.BlastRSS();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                rss = ECN_Framework_DataLayer.Communicator.BlastRSS.GetByBlastID_FeedID(blastID, feedID);
                scope.Complete();
            }
            return rss;
        }

        public static bool ExistsByBlastID(int blastID)
        {
            bool exists = false;
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.BlastRSS.ExistsByBlastID(blastID);
                scope.Complete();
            }
            return exists;
        }

        public static bool ExistsByBlastID_FeedID(int blastID, int feedID)
        {
            bool exists = false;
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.BlastRSS.ExistsByBlastID_FeedID(blastID, feedID);
                scope.Complete();
            }
            return exists;
        }
    }
}
