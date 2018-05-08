using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    public class CampaignItemMetaTag
    {
        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemMetaTag cimt)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemMetaTag_Save";
            if (cimt.CampaignItemMetaTagID > 0)
            {
                cmd.Parameters.AddWithValue("@CampaignItemMetaTagID", cimt.CampaignItemMetaTagID);
                cmd.Parameters.AddWithValue("@UpdatedUserID", cimt.UpdatedUserID);
            }
            else
                cmd.Parameters.AddWithValue("@CreatedUserID", cimt.CreatedUserID);

            cmd.Parameters.AddWithValue("@CampaignItemID", cimt.CampaignItemID);
            cmd.Parameters.AddWithValue("@Property", cimt.Property);
            cmd.Parameters.AddWithValue("@Content", cimt.Content);
            cmd.Parameters.AddWithValue("@IsDeleted", cimt.IsDeleted);
            cmd.Parameters.AddWithValue("@SocialMediaID", cimt.SocialMediaID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete_CampaignItemID(int CampaignItemID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemMetaTag_Delete_CampaignItemID";
            cmd.Parameters.AddWithValue("@CampaignItemID", CampaignItemID);
            cmd.Parameters.AddWithValue("@UpdatedUserID", UserID);
            
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete_SocialMediaID_CampaignItemID(int SocialMediaID, int CampaignItemID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemMetaTag_Delete_SocialMediaID_CampaignItemID";
            cmd.Parameters.AddWithValue("@SocialMediaID", SocialMediaID);
            cmd.Parameters.AddWithValue("@CampaignItemID", CampaignItemID);
            cmd.Parameters.AddWithValue("@UpdatedUserID", UserID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
        public static List<ECN_Framework_Entities.Communicator.CampaignItemMetaTag> GetByCampaignItemID(int CampaignItemID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemMetaTag_Select_CampaignItemID";
            cmd.Parameters.AddWithValue("@CampaignItemID", CampaignItemID);

            return GetList(cmd);

        }

        private static List<ECN_Framework_Entities.Communicator.CampaignItemMetaTag> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemMetaTag> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemMetaTag>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemMetaTag retItem = new ECN_Framework_Entities.Communicator.CampaignItemMetaTag();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemMetaTag>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.CampaignItemMetaTag Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.CampaignItemMetaTag retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.CampaignItemMetaTag();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemMetaTag>.CreateBuilder(rdr);
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
    }
}
