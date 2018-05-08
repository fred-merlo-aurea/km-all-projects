using System;
using System.Collections.Generic;

namespace ECN_Framework_BusinessLayer.Activity.Report.Interfaces
{
    public interface IEmailPerformanceByDomainReportProxy
    {
        IList<ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain> Get(
            int customerId,
            DateTime startDate,
            DateTime endDate,
            bool drillDownOther);
    }
}