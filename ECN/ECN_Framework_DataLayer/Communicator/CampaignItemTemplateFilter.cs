using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;
using FrameworkCampaignItemBlastFilter = ECN_Framework_Entities.Communicator.CampaignItemBlastFilter;
using ECN_Framework_DataLayer.Communicator.Helpers;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class CampaignItemTemplateFilter
    {
        public static int Save(int CampaignItemTemplateID, int GroupID, int FilterID, bool IsDeleted, KMPlatform.Entity.User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplateFilter_Save";
            cmd.Parameters.AddWithValue("@CampaignItemTemplateID", CampaignItemTemplateID);
            cmd.Parameters.AddWithValue("@GroupID", GroupID);
            cmd.Parameters.AddWithValue("@FilterID", FilterID);
            cmd.Parameters.AddWithValue("@IsDeleted", IsDeleted);
            cmd.Parameters.AddWithValue("@CreatedUserID", user.UserID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> GetByCampaignItemTemplateIDAndGroupID(int CampaignItemTemplateID, int GroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplateFilter_Select_CampaignItemTemplateID";
            cmd.Parameters.AddWithValue("@CampaignItemTemplateID", CampaignItemTemplateID);
            cmd.Parameters.AddWithValue("@GroupID", GroupID);

            return GetList(cmd);
        }

        public static void DeleteByCampaignItemTemplateID(int CampaignItemTemplateID, KMPlatform.Entity.User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemTemplateFilter_Delete_CampaignItemTemplateID";

            cmd.Parameters.AddWithValue("@CampaignItemTemplateID", CampaignItemTemplateID);
            cmd.Parameters.AddWithValue("@UpdatedUserID", user.UserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static List<FrameworkCampaignItemBlastFilter> GetList(SqlCommand sqlCommand)
        {
            return SqlCommandCommunicator<FrameworkCampaignItemBlastFilter>.GetList(sqlCommand);
        }
    }
}
