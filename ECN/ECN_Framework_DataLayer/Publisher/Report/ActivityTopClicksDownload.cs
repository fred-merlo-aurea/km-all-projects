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
    public class ActivityTopClicksDownload
    {
        public static List<ECN_Framework_Entities.Publisher.Report.ActivityTopClicksDownload> GetList(int editionID, int blastID, int linkID, int TopCount, string type)
        {
            List<ECN_Framework_Entities.Publisher.Report.ActivityTopClicksDownload> retList = new List<ECN_Framework_Entities.Publisher.Report.ActivityTopClicksDownload>();

            string sqlQuery = "sp_GetActivity_TopClicks_download";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EditionID", editionID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@LinkID", linkID);
            cmd.Parameters.AddWithValue("@TopCount", TopCount);
            cmd.Parameters.AddWithValue("@type", type);

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.Report.ActivityTopClicksDownload>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Publisher.Report.ActivityTopClicksDownload x = builder.Build(rdr);
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
