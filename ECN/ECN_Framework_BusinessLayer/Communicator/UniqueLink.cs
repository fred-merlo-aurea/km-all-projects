using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class UniqueLink
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.UniqueLink;

        public static int Save(ECN_Framework_Entities.Communicator.UniqueLink ul)
        {
            int retID = -1;
            using(TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Communicator.UniqueLink.Save(ul);
                scope.Complete();

            }
            return retID;
        }

        public static ECN_Framework_Entities.Communicator.UniqueLink GetByBlastID_UniqueID(int BlastID, string UniqueID)
        {
            ECN_Framework_Entities.Communicator.UniqueLink retUL = new ECN_Framework_Entities.Communicator.UniqueLink();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retUL = ECN_Framework_DataLayer.Communicator.UniqueLink.GetByBlastID_UniqueID(BlastID, UniqueID);
                scope.Complete();
            }
            return retUL;
        }

        public static List<ECN_Framework_Entities.Communicator.UniqueLink> GetByBlastID(int blastID)
        {
            List<ECN_Framework_Entities.Communicator.UniqueLink> retList = new List<ECN_Framework_Entities.Communicator.UniqueLink>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.UniqueLink.GetByBlastID(blastID);
                scope.Complete();
            }
            return retList;
        }

        public static Dictionary<string, int> GetDictionaryByBlastID(int blastID)
        {
            Dictionary<string, int> retList = new Dictionary<string, int>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.UniqueLink.GetDictionaryByBlastID(blastID);
                scope.Complete();
            }
            return retList;
        }

    }
}
