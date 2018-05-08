using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ECN_Framework_DataLayer.Communicator.Report
{
    [Serializable]
    public class Deliverability
    {
        public static DataTable Get(string startDate, string endDate)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("rpt_Deliverability");
            cmd.Parameters.AddWithValue("@startdate", startDate);
            cmd.Parameters.AddWithValue("@enddate", endDate);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }

        public static DataTable GetByIP(string startDate, string endDate, string ipFilter)
        {
            SqlCommand cmd = new SqlCommand("rpt_Deliverability");
            cmd.Parameters.AddWithValue("@startdate", startDate);
            cmd.Parameters.AddWithValue("@enddate", endDate);
            if (!ipFilter.Equals(""))
            {
                cmd.Parameters.AddWithValue("@ip", ipFilter);
            }
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }
    }
}
