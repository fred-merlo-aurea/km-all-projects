using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity.Report
{
    [Serializable]
    public class PerformanceByDayAndTimeReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.PerformanceByDayAndTimeReport> Get(int customerID, DateTime startDate, DateTime endDate, string filterOne, int filterOneValue, string filterTwo, int? filterTwoValue)
        {
            List<ECN_Framework_Entities.Activity.Report.PerformanceByDayAndTimeReport> retList = new List<ECN_Framework_Entities.Activity.Report.PerformanceByDayAndTimeReport>();
            string sqlQuery = "v_PerformanceByDayAndTimeReport";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@customerID", customerID);
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);
            cmd.Parameters.AddWithValue("@filterOne", filterOne);
            cmd.Parameters.AddWithValue("@filterTwo", filterTwo);
            cmd.Parameters.AddWithValue("@filterOneVal", filterOneValue);
            cmd.Parameters.AddWithValue("@filterTwoVal", filterTwoValue);


            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.PerformanceByDayAndTimeReport>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Activity.Report.PerformanceByDayAndTimeReport x = builder.Build(rdr);
                        retList.Add(x);
                    }

                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }
    }
}
