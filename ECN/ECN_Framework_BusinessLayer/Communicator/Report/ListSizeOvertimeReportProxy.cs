using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Communicator.Report.Interfaces;
using FrameworkEntities = ECN_Framework_Entities.Communicator.Report;

namespace ECN_Framework_BusinessLayer.Communicator.Report
{
    [Serializable]
    public class ListSizeOvertimeReportProxy : IListSizeOvertimeReportProxy
    {
        public IList<FrameworkEntities.ListSizeOvertimeReport> Get(int groupId, DateTime startDate, DateTime endDate)
        {
            return ListSizeOvertimeReport.Get(groupId, startDate, endDate);
        }
    }
}
