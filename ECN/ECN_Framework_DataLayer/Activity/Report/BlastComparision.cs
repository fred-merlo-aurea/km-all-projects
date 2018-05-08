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
    public class BlastComparision
    {
        public static List<ECN_Framework_Entities.Activity.Report.BlastComparision> Get(string blastIDs)
        {
            List<ECN_Framework_Entities.Activity.Report.BlastComparision> bcList = new List<ECN_Framework_Entities.Activity.Report.BlastComparision>();
            ECN_Framework_Entities.Activity.Report.BlastComparision bcobject = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_GetBlastReportComparision";
            cmd.Parameters.AddWithValue("@blastIDs", blastIDs);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.BlastComparision>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    bcobject = new ECN_Framework_Entities.Activity.Report.BlastComparision();
                    bcobject = builder.Build(rdr);
                    bcList.Add(bcobject);
                }
                rdr.Close();
                rdr.Dispose();
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return bcList;
        }
    }
}
