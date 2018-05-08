using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using System.Configuration;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class BlastLinks
    {
        public static int GetMaxLinkColumnLength()
        {
            int maxLength = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                maxLength = ECN_Framework_DataLayer.Communicator.BlastLinks.GetMaxLinkColumnLength();
                scope.Complete();
            }
            return maxLength;
        }
    }
}
