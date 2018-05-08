using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Publisher
{
    [Serializable]    
    public class Frequency
    {
        public static List<ECN_Framework_Entities.Publisher.Frequency> GetAll()
        {
            List<ECN_Framework_Entities.Publisher.Frequency> lFrequency = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                lFrequency = ECN_Framework_DataLayer.Publisher.Frequency.GetAll();
                scope.Complete();
            }

            return lFrequency;
        }
    }
}
