using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class CampaignItemTemplateGroup
    {
        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup citg, KMPlatform.Entity.User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplateGroup_Save";
            cmd.Parameters.AddWithValue("@CampaignItemTemplateGroupID", citg.CampaignItemTemplateGroupID);
            cmd.Parameters.AddWithValue("@CampaignItemTemplateID", citg.CampaignItemTemplateID);
            cmd.Parameters.AddWithValue("@IsDeleted", citg.IsDeleted);
            cmd.Parameters.AddWithValue("@GroupID", citg.GroupID);
            if (citg.CampaignItemTemplateGroupID > 0)
                cmd.Parameters.AddWithValue("@UpdatedUserID", user.UserID);
            else
                cmd.Parameters.AddWithValue("@CreatedUserID", user.UserID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup> GetByCampaignItemTemplateID(int CampaignItemTemplateID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplateGroup_Select_CampaignItemTemplateID";
            cmd.Parameters.AddWithValue("@CampaignItemTemplateID", CampaignItemTemplateID);

           return GetList(cmd);
        }

        public static void DeleteByCampaignItemTemplateID(int CampaignItemTemplateID, KMPlatform.Entity.User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplateGroup_Delete_CampaignItemTemplateID";

            cmd.Parameters.AddWithValue("@CampaignItemTemplateID", CampaignItemTemplateID);
            cmd.Parameters.AddWithValue("@UpdatedUserID", user.UserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup retItem = new ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup>.CreateBuilder(rdr);
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
    }
}
