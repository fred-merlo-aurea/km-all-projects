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
    public class BlastReportDetail
    {
        public static List<ECN_Framework_Entities.Activity.Report.BlastReportDetail> Get(int blastID)
        {
            List<ECN_Framework_Entities.Activity.Report.BlastReportDetail> retList = new List<ECN_Framework_Entities.Activity.Report.BlastReportDetail>();
            string sqlQuery = "rptBlastReportDetail";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BlastID", blastID);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {

                var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.BlastReportDetail>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    ECN_Framework_Entities.Activity.Report.BlastReportDetail x = builder.Build(rdr);
                    retList.Add(x);
                }
                rdr.Close();
                rdr.Dispose();
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }
    }
}
