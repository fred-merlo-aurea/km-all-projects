using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Accounts.Report
{
    [Serializable]
    
    public class EmailDirectReport
    {
        public static DataTable Get(int BaseChannelID, string CustomerIDs, DateTime StartDate, DateTime EndDate)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Accounts.Reports.EmailDirectReport.Get(BaseChannelID, CustomerIDs, StartDate, EndDate);
                scope.Complete();
            }
            return dt;
        }
    }
}
