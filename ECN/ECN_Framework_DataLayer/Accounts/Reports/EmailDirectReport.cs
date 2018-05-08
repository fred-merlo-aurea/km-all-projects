using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ECN_Framework_DataLayer.Accounts.Reports
{
    [Serializable]

    public class EmailDirectReport
    {
        public static DataTable Get(int BaseChannelID, string CustomerIDs, DateTime StartDate, DateTime EndDate)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailDirectReport";

            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
            cmd.Parameters.AddWithValue("@CustomerIDs", CustomerIDs);
            cmd.Parameters.AddWithValue("@StartDate", StartDate.ToShortDateString());
            cmd.Parameters.AddWithValue("@EndDate", EndDate.ToShortDateString());

            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }
    }
}
