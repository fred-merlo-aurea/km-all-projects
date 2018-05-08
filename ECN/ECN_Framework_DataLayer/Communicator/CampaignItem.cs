using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Communicator.Helpers;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    [DataContract]
    public class CampaignItem
    {
        private const string ProcedureCampaignItemDeleteSingle = "e_CampaignItem_Delete_Single";
        private const string ProcedureCampaignItemDeleteAll = "e_CampaignItem_Delete_All";

        public static bool Exists(int campaignID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 ci.CampaignItemID FROM CampaignItem ci WITH (NOLOCK) " +
                              "JOIN Campaign ca WITH (NOLOCK) ON ci.CampaignID = ca.CampaignID " +
                              "JOIN ecn5_accounts..Customer c with (nolock) on ca.CustomerID = c.CustomerID " +
                              "WHERE ci.CampaignID = @CampaignID AND c.CustomerID = @CustomerID and ci.IsDeleted = 0 and ca.IsDeleted = 0 and c.IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignID", campaignID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int campaignID, int campaignItemID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 ci.CampaignItemID FROM CampaignItem ci WITH (NOLOCK) " +
                              "JOIN Campaign ca WITH (NOLOCK) ON ci.CampaignID = ca.CampaignID " +
                              "JOIN ecn5_accounts..Customer c with (nolock) on ca.CustomerID = c.CustomerID " +
                              "WHERE ci.CampaignID = @CampaignID AND ci.CampaignItemID = @CampaignItemID AND c.CustomerID = @CustomerID and ci.IsDeleted = 0 and ca.IsDeleted = 0 and c.IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignID", campaignID);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(string campaignItemName, int campaignID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 ci.CampaignItemID FROM CampaignItem ci WITH (NOLOCK) " +
                              "JOIN Campaign ca WITH (NOLOCK) ON ci.CampaignID = ca.CampaignID " +
                              "JOIN ecn5_accounts..Customer c with (nolock) on ca.CustomerID = c.CustomerID " +
                              "WHERE ci.CampaignID = @CampaignID AND c.CustomerID = @CustomerID and ci.CampaignItemName = @CampaignItemName and ci.IsDeleted = 0 and ca.IsDeleted = 0 and c.IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@CampaignItemName", campaignItemName);
            cmd.Parameters.AddWithValue("@CampaignID", campaignID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItem item)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItem_Save";
            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", (object)item.CampaignItemID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CampaignID", (object)item.CampaignID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CampaignItemName", item.CampaignItemName));
            cmd.Parameters.Add(new SqlParameter("@CampaignItemType", item.CampaignItemType));
            cmd.Parameters.Add(new SqlParameter("@FromName", item.FromName));
            cmd.Parameters.Add(new SqlParameter("@FromEmail", item.FromEmail));
            cmd.Parameters.Add(new SqlParameter("@ReplyTo", item.ReplyTo));
            cmd.Parameters.Add(new SqlParameter("@SendTime", (object)item.SendTime ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PageWatchID", (object)item.PageWatchID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SampleID", (object)item.SampleID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastScheduleID", (object)item.BlastScheduleID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@OverrideAmount", (object)item.OverrideAmount ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@OverrideIsAmount", (object)item.OverrideIsAmount ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastField1", (object)item.BlastField1 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastField2", (object)item.BlastField2 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastField3", (object)item.BlastField3 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastField4", (object)item.BlastField4 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BlastField5", (object)item.BlastField5 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CampaignItemFormatType", item.CampaignItemFormatType));
            cmd.Parameters.Add(new SqlParameter("@CompletedStep", (object)item.CompletedStep ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsHidden", (object)item.IsHidden ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CampaignItemNameOriginal", item.CampaignItemNameOriginal));
            cmd.Parameters.Add(new SqlParameter("@NodeID", item.NodeID));
            cmd.Parameters.Add(new SqlParameter("@CampaignItemIDOriginal", (object)item.CampaignItemIDOriginal ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CampaignItemTemplateID", (object)item.CampaignItemTemplateID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SFCampaignID", (object)item.SFCampaignID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@EnableCacheBuster", (object)item.EnableCacheBuster ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IgnoreSuppression", (object)item.IgnoreSuppression ?? DBNull.Value));
            
            if (item.CampaignItemID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)item.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)item.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItem> GetByCampaignID(int campaignID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItem_Select_CampaignID";
            cmd.Parameters.AddWithValue("@CampaignID", campaignID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItem> GetByStatus(int customerID, ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode status)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_CampaignItem_Select_Status";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@Status", status.ToString());
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetByCampaignItemID(int campaignItemID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItem_Select_CampaignItemID";
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetByNodeID(string nodeID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItem_Select_NodeID";
            cmd.Parameters.AddWithValue("@NodeID", nodeID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetByCampaignItemTestBlastID(int campaignItemTestBlastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItem_Select_CampaignItemTestBlastID";
            cmd.Parameters.AddWithValue("@CampaignItemTestBlastID", campaignItemTestBlastID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetByCampaignItemBlastID(int campaignItemBlastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItem_Select_CampaignItemBlastID";
            cmd.Parameters.AddWithValue("@CampaignItemBlastID", campaignItemBlastID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetByBlastID(int blastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItem_Select_BlastID";
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Communicator.CampaignItem Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.CampaignItem retItem = null;

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
                {
                    if (rdr != null && rdr.HasRows)
                    {
                        try
                        {
                            retItem = new ECN_Framework_Entities.Communicator.CampaignItem();
                            var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItem>.CreateBuilder(rdr);
                            while (rdr.Read())
                            {
                                retItem = builder.Build(rdr);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            if (rdr != null)
                            {
                                rdr.Close();
                                rdr.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return retItem;
        }

        private static List<ECN_Framework_Entities.Communicator.CampaignItem> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItem> retList = new List<ECN_Framework_Entities.Communicator.CampaignItem>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.CampaignItem retItem = new ECN_Framework_Entities.Communicator.CampaignItem();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CampaignItem>.CreateBuilder(rdr);
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

        public static void Delete(int campaignID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryCampaignItem(
                campaignID, null, customerID, userID , ProcedureCampaignItemDeleteAll);
        }

        public static void Delete(int campaignID, int campaignItemID, int customerID, int userID)
        {
            CommunicatorMethodsHelper.ExecuteNonQueryCampaignItem(
                campaignID, campaignItemID, customerID, userID , ProcedureCampaignItemDeleteSingle);
        }

        public static DataTable GetByStatus(int customerID, string status)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_CampaignItem_Select_Status";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@Status", status);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetSentCampaignItems(string campaignName, string campaignItemName, string emailSubject, string layoutName, string groupName, int blastID, DateTime searchFrom, DateTime searchTo,
                                                     int? userID, bool? testBlast, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_CampaignItem_Select_Sent";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            if (campaignName != null && campaignName.Trim().Length > 0)
                cmd.Parameters.Add(new SqlParameter("@CampaignName", campaignName));
            if (campaignItemName != null && campaignItemName.Trim().Length > 0)
                cmd.Parameters.Add(new SqlParameter("@CampaignItemName", campaignItemName));
            if (emailSubject != null && emailSubject.Trim().Length > 0)
                cmd.Parameters.Add(new SqlParameter("@EmailSubject", emailSubject));
            if (layoutName != null && layoutName.Trim().Length > 0)
                cmd.Parameters.Add(new SqlParameter("@LayoutName", layoutName));
            if (groupName != null && groupName.Trim().Length > 0)
                cmd.Parameters.Add(new SqlParameter("@GroupName", groupName));
            if (blastID > 0)
                cmd.Parameters.Add(new SqlParameter("@BlastID", blastID.ToString()));
            cmd.Parameters.Add(new SqlParameter("@FromDate", searchFrom));
            cmd.Parameters.Add(new SqlParameter("@ToDate", searchTo));
            if (userID != null && userID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            if (testBlast != null)
                cmd.Parameters.Add(new SqlParameter("@TestBlast", testBlast));
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void SetIsHidden(int campaignItemID, bool isHidden, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItem_UpdateIsHidden";
            cmd.Parameters.AddWithValue("@IsHidden", isHidden);
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static ECN_Framework_Entities.Communicator.CampaignItem GetBySampleID(int SampleID, string Type)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItem_Select_SampleID";
            cmd.Parameters.AddWithValue("@SampleID", SampleID);
            cmd.Parameters.AddWithValue("@Type", Type);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItem> GetSentSaleforceCampaignItems(int Days)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItem_Select_Saleforce";
            cmd.Parameters.AddWithValue("@days", Days);
            return GetList(cmd);
        }

        public static bool HasEmailActvity(int CampaignItemID, int days)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItem_Exists_HasEmailActvity"; 
            cmd.Parameters.AddWithValue("@CampaignItemID", CampaignItemID);
            cmd.Parameters.AddWithValue("@days", days);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static DataTable GetCampaignItemsForCampaignSearch(int campaignID, string status, string name, int pageSize, int pageIndex)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_CampaignItem_Search";

            cmd.Parameters.AddWithValue("@CampaignID", campaignID);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex);

            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetPendingCampaignItems_NONRecurring(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_CampaignItem_Pending_NonRecurring";

            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void UpdateSendTime(int campaignItemID, DateTime newSendTime)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CampaignItem_UpdateSendTime";

            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            cmd.Parameters.AddWithValue("@SendTime", newSendTime);

             DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItem> UsedAsSmartSegment(int blastID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItem> retList = new List<ECN_Framework_Entities.Communicator.CampaignItem>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Blast_UsedAsSmartSegment";
            cmd.Parameters.AddWithValue("@BlastID", blastID);

            retList = GetList(cmd);
           
            return retList;
        }
    }
}
