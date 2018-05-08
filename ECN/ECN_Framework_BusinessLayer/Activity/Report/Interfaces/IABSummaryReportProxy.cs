using System;
using System.Collections.Generic;
using FrameworkEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_BusinessLayer.Activity.Report.Interfaces
{
    public interface IABSummaryReportProxy
    {
        IList<FrameworkEntities.ABSummaryReport> Get(
            int customerId,
            DateTime startDate,
            DateTime endDate);
    }
}
