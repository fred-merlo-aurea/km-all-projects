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
    public class ChampionAuditReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.ChampionAuditReport> Get(int CustomerID, DateTime startdate, DateTime enddate)
        {
            List<ECN_Framework_Entities.Activity.Report.ChampionAuditReport> retList = new List<ECN_Framework_Entities.Activity.Report.ChampionAuditReport>();
            string sqlQuery = "v_ChampionAuditReport";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@customerID", CustomerID);
            cmd.Parameters.AddWithValue("@startdate", startdate);
            cmd.Parameters.AddWithValue("@enddate", enddate);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.ChampionAuditReport>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Activity.Report.ChampionAuditReport x = builder.Build(rdr);
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
