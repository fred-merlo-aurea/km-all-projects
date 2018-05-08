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
    public class DataDumpReport
    {
        public static DataTable GetDataTable(int CustomerID, DateTime StartDate, DateTime EndDate, string GroupIDs)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_GroupAttribute";

            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@StartDate", StartDate.ToShortDateString());
            cmd.Parameters.AddWithValue("@EndDate", EndDate.ToShortDateString());
            cmd.Parameters.AddWithValue("@GroupIDs", GroupIDs);

            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());

        }

        public static List<ECN_Framework_Entities.Activity.Report.DataDumpReport> GetList(int CustomerID, DateTime StartDate, DateTime EndDate, string GroupIDs)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_GroupAttribute";

            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@StartDate", StartDate.ToShortDateString());
            cmd.Parameters.AddWithValue("@EndDate", EndDate.ToShortDateString());
            cmd.Parameters.AddWithValue("@GroupIDs", GroupIDs);
            
            List<ECN_Framework_Entities.Activity.Report.DataDumpReport> retList = new List<ECN_Framework_Entities.Activity.Report.DataDumpReport>();

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.DataDumpReport>.CreateBuilder(rdr);

                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Activity.Report.DataDumpReport x = builder.Build(rdr);
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
