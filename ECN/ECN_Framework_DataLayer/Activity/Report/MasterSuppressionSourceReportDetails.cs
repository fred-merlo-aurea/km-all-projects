using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReportEntites = ECN_Framework_Entities.Activity.Report;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity.Report
{
    [Serializable]
    public class MasterSuppressionSourceReportDetails
    {
        public static DataSet GetDetails(int customerID, string unsubscribeCodeID, int currentPage, int pageSize, string sortDirection, string sortedColumn, DateTime fromDate, DateTime toDate)
        {
            var reportDataList = new List<ReportEntites.MasterSuppressionSourceReportDetails>();
            DataSet dsReport = null;
            string sqlQuery = "rptGetMasterSuppressionSourceReportDetails";

            using (SqlCommand cmd = new SqlCommand(sqlQuery))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters.AddWithValue("@UnsubscribeCode", unsubscribeCodeID);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@CurrentPage", currentPage);
                cmd.Parameters.AddWithValue("@SortDirection", sortDirection);
                cmd.Parameters.AddWithValue("@SortedColumn", sortedColumn);
                cmd.Parameters.AddWithValue("@FromDate", fromDate.ToShortDateString());
                cmd.Parameters.AddWithValue("@ToDate", toDate.ToShortDateString());
                
                dsReport = DataFunctions.GetDataSet(cmd, DataFunctions.ConnectionString.Activity.ToString());
            }

            return dsReport;
        }

        public static List<ReportEntites.MasterSuppressionSourceReportDetails> GetAllRecords(int customerID)
        {
            var reportDataList = new List<ReportEntites.MasterSuppressionSourceReportDetails>();

            string sqlQuery = "rptGetAllMasterSuppressionSourceReportDetails";

            using (SqlCommand cmd = new SqlCommand(sqlQuery))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);

                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
                {
                    if (rdr != null)
                    {
                        var builder = DynamicBuilder<ReportEntites.MasterSuppressionSourceReportDetails>.CreateBuilder(rdr);

                        while (rdr.Read())
                        {
                            var dataRecord = builder.Build(rdr);
                            reportDataList.Add(dataRecord);
                        }
                    }
                }
            }

            return reportDataList;
        }

        public static List<ReportEntites.MasterSuppressionSourceReportDetails> GetFilteredRecords(int customerID, string unsubscribeCodeID, DateTime fromDate, DateTime toDate)
        {
            var reportDataList = new List<ReportEntites.MasterSuppressionSourceReportDetails>();

            string sqlQuery = "rptGetFilteredMasterSuppressionSourceReportDetails";

            using (SqlCommand cmd = new SqlCommand(sqlQuery))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters.AddWithValue("@UnsubscribeCode", unsubscribeCodeID);
                cmd.Parameters.AddWithValue("@FromDate", fromDate.ToShortDateString());
                cmd.Parameters.AddWithValue("@ToDate", toDate.ToShortDateString());

                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
                {
                    if (rdr != null)
                    {
                        var builder = DynamicBuilder<ReportEntites.MasterSuppressionSourceReportDetails>.CreateBuilder(rdr);

                        while (rdr.Read())
                        {
                            var dataRecord = builder.Build(rdr);
                            reportDataList.Add(dataRecord);
                        }
                    }
                }
            }

            return reportDataList;
        }
    }
}
