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
    public class BounceByDomain
    {
        public static List<ECN_Framework_Entities.Activity.Report.BounceByDomain> Get(int blastID, int campaignItemID)
        {
            List<ECN_Framework_Entities.Activity.Report.BounceByDomain> bcList = new List<ECN_Framework_Entities.Activity.Report.BounceByDomain>();
            ECN_Framework_Entities.Activity.Report.BounceByDomain bcobject = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_GetBounceReportByDomain";
            if (blastID != 0)
                cmd.Parameters.AddWithValue("@blastIDs", blastID.ToString());
            else
                cmd.Parameters.AddWithValue("@blastIDs", "");

            if (campaignItemID != 0)
                cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID.ToString());
            else
                cmd.Parameters.AddWithValue("@CampaignItemID", "");

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.BounceByDomain>.CreateBuilder(rdr);

                    while (rdr.Read())
                    {
                        bcobject = new ECN_Framework_Entities.Activity.Report.BounceByDomain();
                        bcobject = builder.Build(rdr);
                        bcList.Add(bcobject);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return bcList;
        }
    }
}
