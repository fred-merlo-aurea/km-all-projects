using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    public partial class SimpleShareDetail
    {
        public static List<ECN_Framework_Entities.Communicator.SimpleShareDetail> GetByCampaignItemID(int CampaignItemID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SimpleShareDetail_Select_CampaignItemID";
            cmd.Parameters.AddWithValue("@CampaignItemID", CampaignItemID);

            return GetList(cmd);
        }

        public static void DeleteFromCampaignItem(int SocialMediaAuthID, int CampaignItemID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SimpleShareDetail_Delete_AuthID_CampaignItemID";
            cmd.Parameters.AddWithValue("@SocialMediaAuthID", SocialMediaAuthID);
            cmd.Parameters.AddWithValue("@CampaignItemID", CampaignItemID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static ECN_Framework_Entities.Communicator.SimpleShareDetail GetBySimpleShareDetailID(int SimpleShareDetailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SimpleShareDetail_Select_SimpleShareDetailID";
            cmd.Parameters.AddWithValue("@SimpleShareDetailID", SimpleShareDetailID);

            return Get(cmd);
        }


        public static int Save(ECN_Framework_Entities.Communicator.SimpleShareDetail ssd)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SimpleShareDetail_Save";
            if (ssd.SimpleShareDetailID > 0)
            {
                cmd.Parameters.AddWithValue("@SimpleShareDetailID", ssd.SimpleShareDetailID);
                cmd.Parameters.AddWithValue("@UpdatedUserID", ssd.UpdatedUserID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CreatedUserID", ssd.CreatedUserID);
            }
            cmd.Parameters.AddWithValue("@SocialMediaID", ssd.SocialMediaID);
            cmd.Parameters.AddWithValue("@SocialMediaAuthID", ssd.SocialMediaAuthID);
            cmd.Parameters.AddWithValue("@Title", ssd.Title);
            cmd.Parameters.AddWithValue("@SubTitle", ssd.SubTitle);
            cmd.Parameters.AddWithValue("@ImagePath", ssd.ImagePath);
            cmd.Parameters.AddWithValue("@Content", ssd.Content);
            cmd.Parameters.AddWithValue("@UseThumbnail", ssd.UseThumbnail);
            cmd.Parameters.AddWithValue("@IsDeleted", ssd.IsDeleted);
            cmd.Parameters.AddWithValue("@PageID", ssd.PageID);
            cmd.Parameters.AddWithValue("@PageAccessToken", ssd.PageAccessToken);
            return Convert.ToInt32(ECN_Framework_DataLayer.DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }


        private static List<ECN_Framework_Entities.Communicator.SimpleShareDetail> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.SimpleShareDetail> retList = new List<ECN_Framework_Entities.Communicator.SimpleShareDetail>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.SimpleShareDetail retItem = new ECN_Framework_Entities.Communicator.SimpleShareDetail();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.SimpleShareDetail>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.SimpleShareDetail Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.SimpleShareDetail retItem = new ECN_Framework_Entities.Communicator.SimpleShareDetail();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.SimpleShareDetail();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.SimpleShareDetail>.CreateBuilder(rdr);
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
