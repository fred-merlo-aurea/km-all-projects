using System;
using System.Collections.Generic;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class MarketingAutomationHistory
    {
        public static List<ECN_Framework_Entities.Communicator.MarketingAutomationHistory> GetByMAID(int MAID)
        {
            List<ECN_Framework_Entities.Communicator.MarketingAutomationHistory> retList = new List<ECN_Framework_Entities.Communicator.MarketingAutomationHistory>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.MarketingAutomationHistory.SelectByMarketingAutomationID(MAID);
                scope.Complete();
            }
            return retList;
        }

        public static void Insert(int MAID, int UserID, string Action)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.MarketingAutomationHistory.Insert(MAID, UserID, Action);
                scope.Complete();
            }
        }
    }
}
