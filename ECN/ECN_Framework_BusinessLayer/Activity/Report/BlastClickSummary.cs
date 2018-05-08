using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    public class BlastClickSummary
    {
        public static List<ECN_Framework_Entities.Activity.Report.BlastClickSummary> Get(int BlastID)
        {
            List<ECN_Framework_Entities.Activity.Report.BlastClickSummary> retList = new List<ECN_Framework_Entities.Activity.Report.BlastClickSummary>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Activity.Report.BlastClickSummary.Get(BlastID);
                scope.Complete();
            }
            return retList;
        }

    }
}
