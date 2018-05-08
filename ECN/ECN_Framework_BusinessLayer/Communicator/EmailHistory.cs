using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class EmailHistory
    {
        public static int FindMergedEmailID(int oldEmailID)
        {
            int retID = -1;
            using(TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Communicator.EmailHistory.FindMergedEmailID(oldEmailID);
                scope.Complete();
            }
            return retID;

        }
    }
}
