using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    [Serializable]
    public class APILogging
    {
        public static int Insert(FrameworkUAD.Entity.APILogging log, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                log.APILogID = FrameworkUAD.DataAccess.APILogging.Insert(log, client);
                scope.Complete();
            }

            return log.APILogID;
        }

        public static bool UpdateLog(int APILogID, int? LogID, KMPlatform.Object.ClientConnections client)
        {
            return FrameworkUAD.DataAccess.APILogging.UpdateLog(APILogID, LogID, client);
        }
    }
}
