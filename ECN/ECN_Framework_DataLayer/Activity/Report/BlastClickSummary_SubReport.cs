using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity.Report
{
    public class BlastClickSummary_SubReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.BlastClickSummary_SubReport> Get(int BlastID)
        {
            List<ECN_Framework_Entities.Activity.Report.BlastClickSummary_SubReport> ret = new List<ECN_Framework_Entities.Activity.Report.BlastClickSummary_SubReport>();
            string sqlQuery = "spBlastClicksSummary_SubReport";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BlastID", BlastID);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.BlastClickSummary_SubReport>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Activity.Report.BlastClickSummary_SubReport x = builder.Build(rdr);
                        ret.Add(x);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return ret;
        }
    }
}
