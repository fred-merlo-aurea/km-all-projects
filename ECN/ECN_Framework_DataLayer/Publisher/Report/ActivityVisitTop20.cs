using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework_DataLayer.Publisher.Report
{
    [Serializable]
    public class ActivityVisitTop20
    {
        public static List<ECN_Framework_Entities.Publisher.Report.ActivityVisitTop20> GetList(int editionID, int blastID)
        {
            List<ECN_Framework_Entities.Publisher.Report.ActivityVisitTop20> retList = new List<ECN_Framework_Entities.Publisher.Report.ActivityVisitTop20>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_GetActivity_VisitsTop20";
            cmd.Parameters.AddWithValue("@EditionID", editionID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.Report.ActivityVisitTop20>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Publisher.Report.ActivityVisitTop20 x = builder.Build(rdr);
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
