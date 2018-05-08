using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;


namespace ECN_Framework_DataLayer.Activity.Report
{
        [Serializable]
    public class CampaignStatisticsReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.CampaignStatisticsReport> Get(int CampaignID, DateTime startdate, DateTime enddate, int? GroupID = null)
        {
            List<ECN_Framework_Entities.Activity.Report.CampaignStatisticsReport> retList = new List<ECN_Framework_Entities.Activity.Report.CampaignStatisticsReport>();
            string sqlQuery = "rpt_CampaignStatistics";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CampaignID", CampaignID);
            cmd.Parameters.AddWithValue("@StartDate", startdate);
            cmd.Parameters.AddWithValue("@EndDate", enddate);
            if (GroupID != null)
                cmd.Parameters.AddWithValue("@GroupID", GroupID);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.CampaignStatisticsReport>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Activity.Report.CampaignStatisticsReport x = builder.Build(rdr);
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
        public static DataTable GetDT(int CampaignID, DateTime startdate, DateTime enddate, int? GroupID = null)
        {
            string sqlQuery = "rpt_CampaignStatistics";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CampaignID", CampaignID);
            cmd.Parameters.AddWithValue("@StartDate", startdate);
            cmd.Parameters.AddWithValue("@EndDate", enddate);
            if (GroupID != null)
                cmd.Parameters.AddWithValue("@GroupID", GroupID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString()); 
        }

    }
    
}
