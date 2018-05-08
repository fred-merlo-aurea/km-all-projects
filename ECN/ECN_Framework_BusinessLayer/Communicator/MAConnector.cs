using System;
using System.Collections.Generic;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class MAConnector
    {
        public static int Save(ECN_Framework_Entities.Communicator.MAConnector MAC)
        {
            int retID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Communicator.MAConnector.Save(MAC);
                scope.Complete();
            }

            return retID;
        }

        public static void Delete(int MACID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.MAConnector.Delete(MACID);
                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.Communicator.MAConnector> GetByMarketingAutomationID(int MAID)
        {
            List<ECN_Framework_Entities.Communicator.MAConnector> retList = new List<ECN_Framework_Entities.Communicator.MAConnector>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.MAConnector.GetByMarketingAutomationID(MAID);
                scope.Complete();
            }

            return retList;
        }
    }
}
