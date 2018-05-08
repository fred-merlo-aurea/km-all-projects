using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using FrameworkEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class ABSummaryReportProxy : IABSummaryReportProxy
    {
        public IList<FrameworkEntities.ABSummaryReport> Get(
            int customerId, 
            DateTime startDate, 
            DateTime endDate)
        { 
            return ABSummaryReport.Get(customerId, startDate, endDate);
        }
    }
}