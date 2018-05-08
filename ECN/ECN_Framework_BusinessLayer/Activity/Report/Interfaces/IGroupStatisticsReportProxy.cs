using System;
using System.Collections.Generic;

namespace ECN_Framework_BusinessLayer.Activity.Report.Interfaces
{
    public interface IGroupStatisticsReportProxy
    {
        IList<ECN_Framework_Entities.Activity.Report.GroupStatisticsReport> Get(
            int groupId,
            DateTime startDate,
            DateTime endDate);
    }
}