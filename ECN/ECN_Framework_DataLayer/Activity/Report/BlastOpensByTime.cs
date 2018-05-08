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
    public class BlastOpensByTime
    {
        public static List<ECN_Framework_Entities.Activity.Report.BlastOpensByTime> GetByBlastID(int blastID)
        {
            ECN_Framework_Entities.Activity.Report.BlastOpensByTime open = null;
            List<ECN_Framework_Entities.Activity.Report.BlastOpensByTime> openList = new List<ECN_Framework_Entities.Activity.Report.BlastOpensByTime>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_OpensByTime_fromBlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.BlastOpensByTime>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    open = new ECN_Framework_Entities.Activity.Report.BlastOpensByTime();
                    open = builder.Build(rdr);
                    openList.Add(open);
                }
                rdr.Close();
                rdr.Dispose();
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return openList;
        }

        public static List<ECN_Framework_Entities.Activity.Report.BlastOpensByTime> GetByCampaignItemID(int campaingItemID)
        {
            ECN_Framework_Entities.Activity.Report.BlastOpensByTime open = null;
            List<ECN_Framework_Entities.Activity.Report.BlastOpensByTime> openList = new List<ECN_Framework_Entities.Activity.Report.BlastOpensByTime>();


            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_OpensByTime_fromCampaignItem";
            cmd.Parameters.AddWithValue("@CampaignItemID", campaingItemID);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.BlastOpensByTime>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    open = new ECN_Framework_Entities.Activity.Report.BlastOpensByTime();
                    open = builder.Build(rdr);
                    openList.Add(open);
                }
                rdr.Close();
                rdr.Dispose();
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return openList;
        }
    }
}
