using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using FrameworkEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class EmailPerformanceByDomainReportProxy : IEmailPerformanceByDomainReportProxy
    {
        public IList<FrameworkEntities.EmailPerformanceByDomain> Get(
            int customerId,
            DateTime startDate,
            DateTime endDate,
            bool drillDownOther)
        {
            return EmailPerformanceByDomainReport.Get(customerId, startDate, endDate, drillDownOther);
        }
    }
}
