using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReportEntities = ECN_Framework_Entities.Activity.Report;
using ReportDataLayer = ECN_Framework_DataLayer.Activity.Report;
using System.Data;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class MasterSuppressionSourceReportDetails
    {
        public static DataSet GetDetails(int customerID, string unsubscribeCode, int currentPage, int pageSize, string sortDirection, string sortColumn, DateTime fromDate, DateTime toDate)
        {
            return ReportDataLayer.MasterSuppressionSourceReportDetails.GetDetails(customerID, unsubscribeCode, currentPage, pageSize, sortDirection, sortColumn, fromDate, toDate);
        }

        public static List<ReportEntities.MasterSuppressionSourceReportDetails> GetAllRecords(int customerID)
        {
            return ReportDataLayer.MasterSuppressionSourceReportDetails.GetAllRecords(customerID);
        }

        public static List<ReportEntities.MasterSuppressionSourceReportDetails> GetFilteredRecords(int customerID, string unsubscribeCode, DateTime fromDate, DateTime toDate)
        {
            return ReportDataLayer.MasterSuppressionSourceReportDetails.GetFilteredRecords(customerID, unsubscribeCode, fromDate, toDate);
        }
    }
}
