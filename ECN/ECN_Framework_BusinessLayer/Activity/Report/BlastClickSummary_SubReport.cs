using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    public class BlastClickSummary_SubReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.BlastClickSummary_SubReport> Get(int BlastID)
        {
            List<ECN_Framework_Entities.Activity.Report.BlastClickSummary_SubReport> ret = new List<ECN_Framework_Entities.Activity.Report.BlastClickSummary_SubReport>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ret = ECN_Framework_DataLayer.Activity.Report.BlastClickSummary_SubReport.Get(BlastID);
                scope.Complete();
            }
            return ret;
        }
    }
}
