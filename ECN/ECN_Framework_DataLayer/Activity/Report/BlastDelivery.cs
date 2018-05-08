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
    public class BlastDelivery
    {
        public static List<ECN_Framework_Entities.Activity.Report.BlastDelivery> Get(string CustomerIDs, DateTime startdate, DateTime enddate, bool unique)
        {
            List<ECN_Framework_Entities.Activity.Report.BlastDelivery> retList = new List<ECN_Framework_Entities.Activity.Report.BlastDelivery>();
            string sqlQuery = "spBlastDeliveryReport";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@customerID", CustomerIDs);
            cmd.Parameters.AddWithValue("@startdate", startdate);
            cmd.Parameters.AddWithValue("@enddate", enddate);
            cmd.Parameters.AddWithValue("@Unique", unique);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.BlastDelivery>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Activity.Report.BlastDelivery x = builder.Build(rdr);
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

        public static DataTable Get(string CustomerIDs, DateTime startdate, DateTime enddate)
        {
            string sqlQuery = "spBlastDeliveryReport";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@customerID", CustomerIDs);
            cmd.Parameters.AddWithValue("@startdate", startdate);
            cmd.Parameters.AddWithValue("@enddate", enddate);
            cmd.Parameters.AddWithValue("@Unique", false);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());;
        }
    }
}
