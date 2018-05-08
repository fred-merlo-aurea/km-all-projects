using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_BusinessLayer.Communicator.Report
{
    [Serializable]
    public class ListSizeOvertimeReport
    {
        public static List<ECN_Framework_Entities.Communicator.Report.ListSizeOvertimeReport> Get(int groupID, DateTime startdate, DateTime enddate)
        {
            return ECN_Framework_DataLayer.Communicator.Report.ListSizeOvertimeReport.Get(groupID, startdate, enddate);
        }
    }
}
