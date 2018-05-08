using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Collector
{
    [Serializable]
    public class ResponseOptions
    {
        public static List<ECN_Framework_Entities.Collector.ResponseOptions> GetByQuestionID(int QuestionID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Collector.ResponseOptions> itemList = new List<ECN_Framework_Entities.Collector.ResponseOptions>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Collector.ResponseOptions.GetByQuestionID(QuestionID);
                scope.Complete();
            }
            return itemList;
        }

    }
}
