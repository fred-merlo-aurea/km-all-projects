using System;
using System.Collections.Generic;
using FrameworkEntities = ECN_Framework_Entities.Communicator.Report;

namespace ECN_Framework_BusinessLayer.Communicator.Report.Interfaces
{
    public interface IListSizeOvertimeReportProxy
    {
        IList<FrameworkEntities.ListSizeOvertimeReport> Get(int groupId, DateTime startDate, DateTime endDate);
    }
}
