using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using ReportEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class GroupStatisticsReportProxy : IGroupStatisticsReportProxy
    {
        public IList<ReportEntities.GroupStatisticsReport> Get(
            int groupId,
            DateTime startDate,
            DateTime endDate)
        {
            return GroupStatisticsReport.Get(groupId, startDate, endDate);
        }
    }
}
