using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Accounts
{
    public class TotalBlastsForDay
    {
        public static List<ECN_Framework_Entities.Accounts.Report.TotalBlastsForDay> GetReport(DateTime startDate)
        {
            List<ECN_Framework_Entities.Accounts.Report.TotalBlastsForDay> retList = new List<ECN_Framework_Entities.Accounts.Report.TotalBlastsForDay>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Accounts.TotalBlastsForDay.Get(startDate);
                scope.Complete();
            }
            return retList;
        }
    }
}
