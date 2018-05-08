using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class Reports
    {
        public static List<ECN_Framework_Entities.Communicator.Reports> Get(KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.Reports> ReportsList = new List<ECN_Framework_Entities.Communicator.Reports>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ReportsList = ECN_Framework_DataLayer.Communicator.Reports.Get();
                scope.Complete();
            }
            return ReportsList;
        }

        public static ECN_Framework_Entities.Communicator.Reports GetByReportID(int ReportID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Reports report = new ECN_Framework_Entities.Communicator.Reports();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                report = ECN_Framework_DataLayer.Communicator.Reports.GetByReportID(ReportID);
                scope.Complete();
            }
            return report;
        }
        public static ECN_Framework_Entities.Communicator.Reports GetByReportName(string reportName, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.Reports report = new ECN_Framework_Entities.Communicator.Reports();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                report = ECN_Framework_DataLayer.Communicator.Reports.GetByReportName(reportName);
                scope.Complete();
            }
            return report;
        }
    }
}
