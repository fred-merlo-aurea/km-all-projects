using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    public partial class CampaignItemSocialMedia
    {
        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemSocialMedia cism)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemSocialMedia_Save";
            if (cism.CampaignItemSocialMediaID > 0)
                cmd.Parameters.AddWithValue("@CampaignItemSocialMediaID", cism.CampaignItemSocialMediaID);
            cmd.Parameters.AddWithValue("@CampaignItemID", cism.CampaignItemID);
            cmd.Parameters.AddWithValue("@SocialMediaID", cism.SocialMediaID);
            cmd.Parameters.AddWithValue("@SimpleShareDetailID", cism.SimpleShareDetailID);
            cmd.Parameters.AddWithValue("@SocialMediaAuthID", cism.SocialMediaAuthID);
            cmd.Parameters.AddWithValue("@Status", cism.Status);
            if (!string.IsNullOrEmpty(cism.LastErrorCode.ToString()))
                cmd.Parameters.AddWithValue("@LastErrorCode", cism.LastErrorCode);
            if(cism.PageID != null)
                cmd.Parameters.AddWithValue("@PageID", cism.PageID);
            if(cism.PageAccessToken != null)
                cmd.Parameters.AddWithValue("@PageAccessToken", cism.PageAccessToken);
            if (!string.IsNullOrEmpty(cism.PostID))
                cmd.Parameters.AddWithValue("@PostID", cism.PostID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());

        }

        public static void Delete(int CampaignItemID, string SimpleOrSubscriber)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemSocialMedia_Delete_CampaignItemID";

            cmd.Parameters.AddWithValue("@CampaignItemID", CampaignItemID);
            cmd.Parameters.AddWithValue("@SimpleOrSubscriber", SimpleOrSubscriber);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia> GetByCampaignItemID(int CampaignItemID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemSocialMedia_Select_CampaignItemID";
            cmd.Parameters.AddWithValue("@CampaignItemID", CampaignItemID);

            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia> GetByStatus(ECN_Framework_Common.Objects.Communicator.Enums.SocialMediaStatusType status)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemSocialMedia_Select_ByStatus";
            cmd.Parameters.AddWithValue("@Status", status.ToString());

            var tmp = status.ToString();

            return GetList(cmd);
        }


        private static List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemSocialMedia retItem = new ECN_Framework_Entities.Communicator.CampaignItemSocialMedia();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            retList.Add(retItem);
                        }
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        private static ECN_Framework_Entities.Communicator.CampaignItemSocialMedia Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.CampaignItemSocialMedia retItem = new ECN_Framework_Entities.Communicator.CampaignItemSocialMedia();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.CampaignItemSocialMedia();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItemSocialMedia GetByCampaignItemSocialMediaID(int campaignItemSocialMediaID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemSocialMedia_Select_CampaignItemSocialMediaID";
            cmd.Parameters.AddWithValue("@CampaignItemSocialMediaID", campaignItemSocialMediaID);
            return Get(cmd);
        }

    }
}
