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
    public class CampaignItemTemplateOptoutGroup
    {
        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemTemplateOptoutGroup citg, KMPlatform.Entity.User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplateOptoutGroup_Save";
            cmd.Parameters.AddWithValue("@CampaignItemTemplateOptOutGroupId", citg.CampaignItemTemplateOptOutGroupID);
            cmd.Parameters.AddWithValue("@CampaignItemTemplateID", citg.CampaignItemTemplateID);
            cmd.Parameters.AddWithValue("@IsDeleted", citg.IsDeleted);
            cmd.Parameters.AddWithValue("@GroupID", citg.GroupID);
            if (citg.CampaignItemTemplateOptOutGroupID > 0)
                cmd.Parameters.AddWithValue("@UpdatedUserID", user.UserID);
            else
                cmd.Parameters.AddWithValue("@CreatedUserID", user.UserID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemTemplateOptoutGroup> GetByCampaignItemTemplateID(int CampaignItemTemplateID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplateOptoutGroup_Select_CampaignItemTemplateID";
            cmd.Parameters.AddWithValue("@CampaignItemTemplateID", CampaignItemTemplateID);

           return GetList(cmd);
        }

        public static void DeleteByCampaignItemTemplateID(int CampaignItemTemplateID, KMPlatform.Entity.User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplateOptoutGroup_Delete_CampaignItemTemplateID";

            cmd.Parameters.AddWithValue("@CampaignItemTemplateID", CampaignItemTemplateID);
            cmd.Parameters.AddWithValue("@UpdatedUserID", user.UserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.CampaignItemTemplateOptoutGroup> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTemplateOptoutGroup> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemTemplateOptoutGroup>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemTemplateOptoutGroup retItem = new ECN_Framework_Entities.Communicator.CampaignItemTemplateOptoutGroup();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemTemplateOptoutGroup>.CreateBuilder(rdr);
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
