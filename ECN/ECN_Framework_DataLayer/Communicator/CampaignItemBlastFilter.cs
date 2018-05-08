using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;
using ECN_Framework_DataLayer.Communicator.Helpers;
using FrameworkCampaignItemBlastFilter = ECN_Framework_Entities.Communicator.CampaignItemBlastFilter;

namespace ECN_Framework_DataLayer.Communicator
{
    public class CampaignItemBlastFilter
    {
        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> GetByCampaignItemBlastID(int campaignItemBlastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemBlastFilter_Select_CampaignItemBlastID";
            cmd.Parameters.AddWithValue("@CampaignItemBlastID", campaignItemBlastID);

            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> GetByCampaignItemSuppressionID(int campaignItemSuppressionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemBlastFilter_Select_CampaignItemSuppressionID";
            cmd.Parameters.AddWithValue("@CampaignItemSuppressionID", campaignItemSuppressionID);

            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> GetByCampaignItemTestBlastID(int campaignItemTestBlastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemBlastFilter_Select_CampaignItemTestBlastID";
            cmd.Parameters.AddWithValue("@CampaignItemTestBlastID", campaignItemTestBlastID);

            return GetList(cmd);
        }

        public static bool SelectByFilterIDCanDelete(int filterID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemBlastFilter_Select_FilterID_CanDelete";
            cmd.Parameters.AddWithValue("@FilterID", filterID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemBlastFilter_Save";
            if(cibf.CampaignItemBlastFilterID > 0)
                cmd.Parameters.AddWithValue("@CampaignItemBlastFilterID", cibf.CampaignItemBlastFilterID );
            if(cibf.CampaignItemBlastID != null)
                cmd.Parameters.AddWithValue("@CampaignItemBlastID", cibf.CampaignItemBlastID);
            if (cibf.CampaignItemSuppresionID != null)
                cmd.Parameters.AddWithValue("@CampaignItemSuppressionID", cibf.CampaignItemSuppresionID);
            if (cibf.CampaignItemTestBlastID != null)
                cmd.Parameters.AddWithValue("@CampaignItemTestBlastID", cibf.CampaignItemTestBlastID);

            cmd.Parameters.AddWithValue("@IsDeleted", cibf.IsDeleted);
            if(cibf.SmartSegmentID != null)
            {
                cmd.Parameters.AddWithValue("@SmartSegmentID", cibf.SmartSegmentID);
                cmd.Parameters.AddWithValue("@RefBlastIDs", cibf.RefBlastIDs);
            }
            else if(cibf.FilterID != null)
            {
                cmd.Parameters.AddWithValue("@FilterID", cibf.FilterID);
            }

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());

        }

        public static void DeleteByCampaignItemBlastID(int CampaignItemBlastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemBlastFilter_Delete_CampaignItemBlastID";
            cmd.Parameters.AddWithValue("@CampaignItemBlastID", CampaignItemBlastID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void DeleteByCampaignItemSuppressionID(int CampaignItemSuppressionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItemBlastFilter_Delete_CampaignItemSuppressionID";
            cmd.Parameters.AddWithValue("@CampaignItemSuppressionID", CampaignItemSuppressionID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static List<FrameworkCampaignItemBlastFilter> GetList(SqlCommand sqlCommand)
        {
            return SqlCommandCommunicator<FrameworkCampaignItemBlastFilter>.GetList(sqlCommand);
        }
    }
}
