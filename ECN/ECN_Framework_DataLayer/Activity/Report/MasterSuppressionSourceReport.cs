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
    public class MasterSuppressionSourceReport
    {
        public static List<ReportEntites.MasterSuppressionSourceReport> GetMaster(int customerID, DateTime FromDate, DateTime ToDate)
        {
            var reportDataList = new List<ReportEntites.MasterSuppressionSourceReport>();

            string sqlQuery = "rptGetMasterSuppressionSourceReport";

            using (SqlCommand cmd = new SqlCommand(sqlQuery))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters.AddWithValue("@FromDate", FromDate.ToShortDateString());
                cmd.Parameters.AddWithValue("@ToDate", ToDate.ToShortDateString());

                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Activity.ToString()))
                {
                    if (rdr != null)
                    {
                        var builder = DynamicBuilder<ReportEntites.MasterSuppressionSourceReport>.CreateBuilder(rdr);
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
