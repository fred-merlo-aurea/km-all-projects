using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Collector
{
    [Serializable]
    public class GridStatements
    {
        public static List<ECN_Framework_Entities.Collector.GridStatements> GetByQuestionID(int QuestionID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Collector.GridStatements> itemList = new List<ECN_Framework_Entities.Collector.GridStatements>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Collector.GridStatements.GetByQuestionID(QuestionID);
                scope.Complete();
            }
            return itemList;
        }
    }
}
