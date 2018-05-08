using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;
using System.Transactions;
namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class DataDumpReport
    {
        public static DataTable GetDataTable(int CustomerID, DateTime StartDate, DateTime EndDate, string GroupIDs)
        {
            DataTable dt = new DataTable();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.Report.DataDumpReport.GetDataTable(CustomerID, StartDate, EndDate, GroupIDs);
                scope.Complete();
            }
            return dt;

        }

        public static List<ECN_Framework_Entities.Activity.Report.DataDumpReport> GetList(int CustomerID, DateTime StartDate, DateTime EndDate, string GroupIDs)
        {
            List<ECN_Framework_Entities.Activity.Report.DataDumpReport> retList = new List<ECN_Framework_Entities.Activity.Report.DataDumpReport>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Activity.Report.DataDumpReport.GetList(CustomerID, StartDate, EndDate, GroupIDs);
                scope.Complete();
            }
            return retList;
        }
    }
}
