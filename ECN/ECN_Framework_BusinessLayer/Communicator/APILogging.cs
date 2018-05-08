using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class APILogging
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.APILogging;

        public static int Insert(ECN_Framework_Entities.Communicator.APILogging log)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                log.APILogID = ECN_Framework_DataLayer.Communicator.APILogging.Insert(log);
                scope.Complete();
            }

            return log.APILogID;
        }

        public static void UpdateLog(int APILogID, int? LogID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.APILogging.UpdateLog(APILogID, LogID);
                scope.Complete();
            }
        }
    }
}
