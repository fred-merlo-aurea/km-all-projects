using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class BlastEngines
    {
        public static DataTable GetBlastEngineStatus()
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Communicator.BlastEngines.GetBlastEngineStatus();
                scope.Complete();
            }

            return dt;
        }
    }
}
