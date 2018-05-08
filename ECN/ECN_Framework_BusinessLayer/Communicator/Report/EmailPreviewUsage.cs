using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator.Report
{
    public class EmailPreviewUsage
    {
        public static List<ECN_Framework_Entities.Communicator.Report.EmailPreviewUsage> GetUsageDetailsAutomated(string customerIDs, DateTime startDate, DateTime endDate)
        {
            List<ECN_Framework_Entities.Communicator.Report.EmailPreviewUsage> epList = new List<ECN_Framework_Entities.Communicator.Report.EmailPreviewUsage>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                epList = ECN_Framework_DataLayer.Communicator.Report.EmailPreviewUsage.GetUsageDetailsAutomated(customerIDs, startDate, endDate);
                scope.Complete();
            }
            return epList;
        }
    }
}
