using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_DataLayer.Activity.Report
{
    [Serializable]
    public class SocialShareChart
    {
        public static List<ECN_Framework_Entities.Activity.Report.SocialShareChart> GetChartSharesByBlastID(int blastID, int customerID)
        {
            List<ECN_Framework_Entities.Activity.Report.SocialShareChart> chartList = new List<ECN_Framework_Entities.Activity.Report.SocialShareChart>();
            ECN_Framework_Entities.Activity.Report.SocialShareChart chart = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_BlastActivitySocial_GetChartShares";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);

            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString());
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    chart = new ECN_Framework_Entities.Activity.Report.SocialShareChart();
                    chart.DisplayName = row["DisplayName"].ToString();
                    if (dt.Rows[0]["Share"] != DBNull.Value)
                    {
                        chart.Share = Convert.ToInt32(row["Share"].ToString());
                    }

                    chartList.Add(chart);
                }
            }
            return chartList;
        }

        public static List<ECN_Framework_Entities.Activity.Report.SocialShareChart> GetChartSharesByCampaignItemID(int campaignItemID, int customerID)
        {
            List<ECN_Framework_Entities.Activity.Report.SocialShareChart> chartList = new List<ECN_Framework_Entities.Activity.Report.SocialShareChart>();
            ECN_Framework_Entities.Activity.Report.SocialShareChart chart = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_BlastActivitySocial_GetChartShares";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);

            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString());
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    chart = new ECN_Framework_Entities.Activity.Report.SocialShareChart();
                    chart.DisplayName = row["DisplayName"].ToString();
                    if (dt.Rows[0]["Share"] != DBNull.Value)
                    {
                        chart.Share = Convert.ToInt32(row["Share"].ToString());
                    }

                    chartList.Add(chart);
                }
            }
            return chartList;
        }

        public static List<ECN_Framework_Entities.Activity.Report.SocialShareChart> GetChartPreviewsByBlastID(int blastID, int customerID)
        {
            List<ECN_Framework_Entities.Activity.Report.SocialShareChart> chartList = new List<ECN_Framework_Entities.Activity.Report.SocialShareChart>();
            ECN_Framework_Entities.Activity.Report.SocialShareChart chart = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_BlastActivitySocial_GetChartPreviews";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);

            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString());
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    chart = new ECN_Framework_Entities.Activity.Report.SocialShareChart();
                    chart.DisplayName = row["DisplayName"].ToString();
                    if (dt.Rows[0]["Share"] != DBNull.Value)
                    {
                        chart.Share = Convert.ToInt32(row["Share"].ToString());
                    }

                    chartList.Add(chart);
                }
            }
            return chartList;
        }

        public static List<ECN_Framework_Entities.Activity.Report.SocialShareChart> GetChartPreviewsByCampaignItemID(int campaignItemID, int customerID)
        {
            List<ECN_Framework_Entities.Activity.Report.SocialShareChart> chartList = new List<ECN_Framework_Entities.Activity.Report.SocialShareChart>();
            ECN_Framework_Entities.Activity.Report.SocialShareChart chart = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_BlastActivitySocial_GetChartPreviews";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);

            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString());
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    chart = new ECN_Framework_Entities.Activity.Report.SocialShareChart();
                    chart.DisplayName = row["DisplayName"].ToString();
                    if (dt.Rows[0]["Share"] != DBNull.Value)
                    {
                        chart.Share = Convert.ToInt32(row["Share"].ToString());
                    }

                    chartList.Add(chart);
                }
            }
            return chartList;
        }
    }
}
