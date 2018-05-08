using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity.Report
{
    [Serializable]
    public class GroupStatisticsReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.GroupStatisticsReport> Get(int groupID, DateTime startdate, DateTime enddate)
        {
            List<ECN_Framework_Entities.Activity.Report.GroupStatisticsReport> retList = new List<ECN_Framework_Entities.Activity.Report.GroupStatisticsReport>();
            string sqlQuery = "spGroupStatisticsReport";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupID", groupID);
            cmd.Parameters.AddWithValue("@startdate", startdate);
            cmd.Parameters.AddWithValue("@enddate", enddate);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.GroupStatisticsReport>.CreateBuilder(rdr);

                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Activity.Report.GroupStatisticsReport x = builder.Build(rdr);
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
