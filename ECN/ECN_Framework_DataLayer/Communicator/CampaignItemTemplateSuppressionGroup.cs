using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class CampaignItemTemplateSuppressionGroup
    {
        public static List<ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup> GetByCampaignItemTemplateID(int CampaignItemTemplateID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplateSuppressionGroup_Select_CampaignItemTemplateID";
            cmd.Parameters.AddWithValue("@CampaignItemTemplateID", CampaignItemTemplateID);
            return GetList(cmd);
            
        }

        public static void Save(ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup CampaignItemTemplateSuppressionGroup)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplateSuppressionGroup_Save";
            cmd.Parameters.Add(new SqlParameter("@CampaignItemTemplateID", CampaignItemTemplateSuppressionGroup.CampaignItemTemplateID));
            cmd.Parameters.Add(new SqlParameter("@GroupID", CampaignItemTemplateSuppressionGroup.GroupID));
            cmd.Parameters.Add(new SqlParameter("@CreatedUserID", (object)CampaignItemTemplateSuppressionGroup.CreatedUserID ?? DBNull.Value));
            DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void DeleteByCampaignItemTemplateID(int CampaignItemTemplateID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplateSuppressionGroup_Delete_CampaignItemTemplateID";
            cmd.Parameters.AddWithValue("@CampaignItemTemplateID", CampaignItemTemplateID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int CampaignItemTemplateSuppressionGroupID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplateSuppressionGroup_Delete";
            cmd.Parameters.AddWithValue("@CampaignItemTemplateSuppressionGroupID", CampaignItemTemplateSuppressionGroupID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup retItem = new ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup>.CreateBuilder(rdr);
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
