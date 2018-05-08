using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReportEntities = ECN_Framework_Entities.Activity.Report;
using ReportDataLayer = ECN_Framework_DataLayer.Activity.Report;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class MasterSuppressionSourceReport
    {
        public static List<ReportEntities.MasterSuppressionSourceReport> GetMaster(int customerID, DateTime FromDate, DateTime ToDate)
        {
            return ReportDataLayer.MasterSuppressionSourceReport.GetMaster(customerID, FromDate, ToDate);
        }
    }
}
