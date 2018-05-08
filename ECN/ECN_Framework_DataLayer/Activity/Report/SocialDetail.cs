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
    public class SocialDetail
    {
        public static List<ECN_Framework_Entities.Activity.Report.SocialDetail> GetSocialDetailByBlastID(int blastID, int customerID)
        {
            List<ECN_Framework_Entities.Activity.Report.SocialDetail> detailList = new List<ECN_Framework_Entities.Activity.Report.SocialDetail>();
            ECN_Framework_Entities.Activity.Report.SocialDetail detail = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_BlastActivitySocial_GetSocialDetail";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);

            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString());
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    detail = new ECN_Framework_Entities.Activity.Report.SocialDetail();
                    detail.BlastID = blastID;
                    detail.DisplayName = row["DisplayName"].ToString();
                    detail.EmailAddress = row["EmailAddress"].ToString();
                    detail.SocialMediaID = Convert.ToInt32(row["SocialMediaID"].ToString());
                    if (dt.Rows[0]["Click"] != DBNull.Value)
                    {
                        detail.Click = Convert.ToInt32(row["Click"].ToString());
                    }

                    detailList.Add(detail);
                }
            }
            return detailList;
        }

        public static List<ECN_Framework_Entities.Activity.Report.SocialDetail> GetSocialDetailByCampaignItemID(int campaignItemID, int customerID)
        {
            List<ECN_Framework_Entities.Activity.Report.SocialDetail> detailList = new List<ECN_Framework_Entities.Activity.Report.SocialDetail>();
            ECN_Framework_Entities.Activity.Report.SocialDetail detail = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "rpt_BlastActivitySocial_GetSocialDetail";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);

            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString());
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    detail = new ECN_Framework_Entities.Activity.Report.SocialDetail();
                    detail.BlastID = Convert.ToInt32(row["BlastID"].ToString());
                    detail.DisplayName = row["DisplayName"].ToString();
                    detail.EmailAddress = row["EmailAddress"].ToString();
                    detail.SocialMediaID = Convert.ToInt32(row["SocialMediaID"].ToString());
                    if (dt.Rows[0]["Click"] != DBNull.Value)
                    {
                        detail.Click = Convert.ToInt32(row["Click"].ToString());
                    }

                    detailList.Add(detail);
                }
            }
            return detailList;
        }
    }
}
