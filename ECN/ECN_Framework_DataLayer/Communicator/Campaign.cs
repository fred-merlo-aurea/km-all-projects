using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;
using ECN_Framework_DataLayer.Communicator.Helpers;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class Campaign
    {
        public static ECN_Framework_Entities.Communicator.Campaign GetByCampaignID(int campaignID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Campaign_Select_CampaignID";
            cmd.Parameters.AddWithValue("@CampaignID", campaignID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.Campaign GetByCampaignName(string campaignName, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Campaign_Select_CampaignName";
            cmd.Parameters.AddWithValue("@CampaignName", campaignName);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.Campaign> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Campaign_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.Campaign> Search(int customerID, string criteria)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Campaign_Search";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@Criteria", criteria);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.Campaign> GetByCustomerID_NonArchived(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Campaign_Select_CustomerID_NonArchived";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.Campaign GetByCampaignItemID(int campaignItemID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Campaign_Select_CampaignItemID";
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.Campaign GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Campaign_Select_BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return Get(cmd);
        }

        public static DataTable GetCampaignDetailsForManageCampaigns(int customerID, string campaignName, string archiveFilter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Campaign_Select_ManageCampaigns";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignName", campaignName);
            cmd.Parameters.AddWithValue("@ArchiveFilter", archiveFilter);

            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.Campaign> GetList(SqlCommand cmd)
        {
            return SqlCommandCommunicator<ECN_Framework_Entities.Communicator.Campaign>.GetList(cmd);
        }

        private static ECN_Framework_Entities.Communicator.Campaign Get(SqlCommand cmd)
        {
            return SqlCommandCommunicator<ECN_Framework_Entities.Communicator.Campaign>.Get(cmd);
        }

        public static bool Exists(int campaignID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 CampaignID from Campaign with(nolock) where CustomerID = @CustomerID and CampaignID = @CampaignID and IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignID", campaignID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int campaignID,string campaignName, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 CampaignID from Campaign with(nolock) where CustomerID = @CustomerID and CampaignName = @CampaignName and CampaignID <> ISNULL(@CampaignID, 0) and IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignName", campaignName);
            cmd.Parameters.AddWithValue("@CampaignID", campaignID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static int Save(ECN_Framework_Entities.Communicator.Campaign campaign)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Campaign_Save";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)campaign.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CampaignName", campaign.CampaignName));
            cmd.Parameters.Add(new SqlParameter("@DripDesign", campaign.DripDesign));
            cmd.Parameters.Add(new SqlParameter("@CampaignID", (object)campaign.CampaignID ?? DBNull.Value));
            if(campaign.CampaignID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)campaign.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)campaign.CreatedUserID ?? DBNull.Value));
            
            cmd.Parameters.AddWithValue("@IsArchived", campaign.IsArchived.HasValue ? campaign.IsArchived.Value : false);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int campaignID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Campaign_Delete";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignID", campaignID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
       
    }
}
