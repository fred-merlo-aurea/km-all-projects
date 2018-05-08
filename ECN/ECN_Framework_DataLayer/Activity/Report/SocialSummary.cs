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
    public class SocialSummary
    {
        public static List<ECN_Framework_Entities.Activity.Report.SocialSummary> GetSocialSummaryByBlastID(int blastID, int customerID)
        {
            List<ECN_Framework_Entities.Activity.Report.SocialSummary> summaryList = new List<ECN_Framework_Entities.Activity.Report.SocialSummary>();
            ECN_Framework_Entities.Activity.Report.SocialSummary summary = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_BlastActivitySocial_SocialSummary";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);

            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString());
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    summary = new ECN_Framework_Entities.Activity.Report.SocialSummary();
                    summary.ID = blastID;
                    summary.IsBlastGroup = false;
                    //summary.BlastID = Convert.ToInt32(row["BlastID"].ToString());
                    summary.ReportImagePath = row["ReportImagePath"].ToString();
                    if (dt.Rows[0]["Share"] != DBNull.Value)
                    {
                        summary.Share = Convert.ToInt32(row["Share"].ToString());
                    }
                    if (dt.Rows[0]["Click"] != DBNull.Value)
                    {
                        summary.Click = Convert.ToInt32(row["Click"].ToString());
                    }

                    KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));
                    StringBuilder reportPath = new StringBuilder();
                    reportPath.Append("social.aspx?");
                    string queryString = "BlastID=" + blastID.ToString() + "&m=" + row["SocialMediaID"].ToString();
                    string encryptedQuery = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, ec));
                    reportPath.Append(encryptedQuery);
                    summary.ReportPath = reportPath.ToString();

                    summaryList.Add(summary);
                }
            }
            return summaryList;
        }

        public static List<ECN_Framework_Entities.Activity.Report.SocialSummary> GetSocialSummaryByCampaignItemID(int campaignItemID, int customerID)
        {
            List<ECN_Framework_Entities.Activity.Report.SocialSummary> summaryList = new List<ECN_Framework_Entities.Activity.Report.SocialSummary>();
            ECN_Framework_Entities.Activity.Report.SocialSummary summary = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_BlastActivitySocial_SocialSummary";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);

            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString());
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bool alreadyExists = false;
                    int indexToRemove = 0;
                    summary = new ECN_Framework_Entities.Activity.Report.SocialSummary();
                    summary.Share = 0;
                    summary.Click = 0;
                    if(summaryList.Find(x => x.SocialMediaID == Convert.ToInt32(row["SocialMediaID"].ToString())) != null)
                    {
                        summary = summaryList.Find(x => x.SocialMediaID == Convert.ToInt32(row["SocialMediaID"].ToString()));
                        alreadyExists = true;
                        indexToRemove = summaryList.IndexOf(summary);
                    }
                    summary.SocialMediaID = Convert.ToInt32(row["SocialMediaID"].ToString());
                    summary.ID = campaignItemID;
                    summary.IsBlastGroup = true;
                    //summary.BlastID = Convert.ToInt32(row["BlastID"].ToString());
                    summary.ReportImagePath = row["ReportImagePath"].ToString();
                    if (dt.Rows[0]["Share"] != DBNull.Value)
                    {
                        summary.Share += Convert.ToInt32(row["Share"].ToString());
                    }
                    if (dt.Rows[0]["Click"] != DBNull.Value)
                    {
                        summary.Click += Convert.ToInt32(row["Click"].ToString());
                    }

                    KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));
                    StringBuilder reportPath = new StringBuilder();
                    reportPath.Append("social.aspx?");
                    string queryString = "CampaignItemID=" + campaignItemID.ToString() + "&m=" + row["SocialMediaID"].ToString();
                    string encryptedQuery = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, ec));
                    reportPath.Append(encryptedQuery);
                    summary.ReportPath = reportPath.ToString();
                    if(!alreadyExists)
                        summaryList.Add(summary);
                    else
                    {
                        summaryList.RemoveAt(indexToRemove);
                        summaryList.Add(summary);
                    }
                }
            }
            return summaryList;
        }
    }
}
