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
    public class Group
    {
        private static string _CacheRegion = "Group";
        public static bool Exists(int groupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Exists_ByID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }
        public static bool IsArchived(int groupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_IsArchived_ByID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }
        public static bool ExistsByGroupNameByCustomerID(string GroupName, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand("Select * from [Groups] WHERE GroupName = @GroupName AND CustomerID = @CustomerID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@GroupName", GroupName);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }
        public static bool Exists(int groupID, string groupName, int folderID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Exists_ByName";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@GroupName", groupName);
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool CheckForExistingSeedlist(int? groupID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_CheckForExistingSeedlist";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            if(groupID != null && groupID > 0)
                cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool SuppressionGroupsExists(string groupIDs, int customerID)
        {
            char[] delimiter = { ',' };
            string[] strGroups = groupIDs.Split(delimiter);
            int groupCount = 0;
            for (int j = 0; j < strGroups.Length; j++)
            {
                try
                {
                    groupCount++;
                }
                catch
                {
                }
            }
            if (strGroups.Length > 0)
            {
                Int32 countActual = Convert.ToInt32(DataFunctions.ExecuteScalar("SELECT COUNT(GroupID) FROM [Groups] with (nolock) WHERE GroupID in (" + groupIDs + ") AND CustomerID = " + customerID, DataFunctions.ConnectionString.Communicator.ToString()));
                if (countActual == groupCount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static ECN_Framework_Entities.Communicator.Group GetByGroupID(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Select_GroupID";
            cmd.Parameters.AddWithValue("@groupID", groupID);

            ECN_Framework_Entities.Communicator.Group retItem = null;

            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = KM.Common.CacheUtil.IsCacheEnabled();
            }
            catch (Exception) { }

            if (isCacheEnabled)
            {
                retItem = (ECN_Framework_Entities.Communicator.Group)KM.Common.CacheUtil.GetFromCache(groupID.ToString(), _CacheRegion);
                if (retItem == null)
                {
                    retItem = Get(cmd);
                    if(retItem != null)
                        KM.Common.CacheUtil.AddToCache(groupID.ToString(), retItem, _CacheRegion);
                }
            }
            else
                retItem = Get(cmd);
            return retItem;
        }

        public static ECN_Framework_Entities.Communicator.Group GetMasterSuppressionGroup(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Select_GetMasterSuppression";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.Group GetSeedListByCustomerID(int customerID)       
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Select_GetSeedLists";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Get(cmd);
        }


        public static List<ECN_Framework_Entities.Communicator.Group> GetByCustomerID(int customerID,string archiveFilter = "active")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@ArchiveFilter", archiveFilter);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.Group> GetByFolderIDCustomerID(int folderID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Select_FolderID_CustomerID";
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.Group> GetByFolderID(int folderID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Select_FolderID";
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.Group> GetByGroupSearch(string name, int? folderID, int customerID, int userID, bool? archived = null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Select_Search";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            if (ECN_Framework_Common.Functions.StringFunctions.HasValue(name))
            {
                cmd.Parameters.AddWithValue("@GroupName", name);
            }
            if (folderID != null)
            {
                cmd.Parameters.AddWithValue("@FolderID", folderID);
            }
            if (archived != null)
            {
                cmd.Parameters.AddWithValue("@Archived", archived.Value);
            }
            return GetList(cmd);
        }
        
        private static ECN_Framework_Entities.Communicator.Group Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.Group retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.Group();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Group>.CreateBuilder(rdr);
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
        private static List<ECN_Framework_Entities.Communicator.Group> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.Group> retList = new List<ECN_Framework_Entities.Communicator.Group>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.Group retItem = new ECN_Framework_Entities.Communicator.Group();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Group>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);

                        retList.Add(retItem);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static bool FolderUsed(int folderID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Count_FolderID";
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool GroupUsedInDomainTracking(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Count_DomainTracking";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool GroupUsedInJointForms(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Count_JointForms";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool GroupUsedInSurveys(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Count_Surveys";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool GroupUsedInDigitalEditions(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Count_DigitalEditions";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool GroupUsedInSubMgmt(int groupID, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Exists_SubMgmt";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool GroupUsedInPubNewsletters(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Count_PubNewsletters";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool GroupUsedInMasterSuppression(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Count_MasterSuppression";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool GroupUsedInCampaignItemTemplate(int groupID, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Exists_CampaignItemTemplate";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static int Save(ECN_Framework_Entities.Communicator.Group group)
        {
            if (group.GroupID > 0)
                DeleteCache(group.GroupID);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Save";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", group.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@FolderID", (object)group.FolderID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GroupName", group.GroupName));
            cmd.Parameters.Add(new SqlParameter("@GroupDescription", group.GroupDescription));
            cmd.Parameters.Add(new SqlParameter("@OwnerTypeCode", group.OwnerTypeCode));
            cmd.Parameters.Add(new SqlParameter("@MasterSupression", (object)group.MasterSupression ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PublicFolder", (object)group.PublicFolder ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@OptinHTML", group.OptinHTML));
            cmd.Parameters.Add(new SqlParameter("@OptinFields", group.OptinFields));
            cmd.Parameters.Add(new SqlParameter("@AllowUDFHistory", group.AllowUDFHistory));
            cmd.Parameters.Add(new SqlParameter("@IsSeedList", (object)group.IsSeedList ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GroupID", group.GroupID));
            if (group.GroupID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)group.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)group.CreatedUserID ?? DBNull.Value));
            cmd.Parameters.AddWithValue("@Archived", (object)group.Archived ?? DBNull.Value);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());            
        }

        public static void Archive(int groupID,bool archive, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Archive";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@Archive", archive);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void Delete(int groupID, int customerID, int userID)
        {
            DeleteCache(groupID);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Group_Delete";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString()); 
        }

        private static void DeleteCache(int groupID)
        {
            if (KM.Common.CacheUtil.IsCacheEnabled())
            {
                if (KM.Common.CacheUtil.GetFromCache(groupID.ToString(), _CacheRegion) != null)
                    KM.Common.CacheUtil.RemoveFromCache(groupID.ToString(), _CacheRegion);
            }
        }

        public static DataTable GetByGroupName(int baseChannelID, string groupName, string searchCriteria, int folderID, bool allFolders, int customerID, int userID,string sortColumn, string sortDirection, int currentPage = 1,int pageSize = 15,  string archiveFilter = "all", int? SubscriberLimit = null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Group_Get_GroupName";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@GroupName", groupName);
            cmd.Parameters.AddWithValue("@SearchCriteria", searchCriteria);
            if (!allFolders)
                cmd.Parameters.AddWithValue("@FolderID", folderID);
            cmd.Parameters.AddWithValue("@CurrentPage", currentPage);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@ArchiveFilter", archiveFilter);
            if (SubscriberLimit.HasValue)
                cmd.Parameters.AddWithValue("@SubscriberLimit", SubscriberLimit.Value);

            cmd.Parameters.AddWithValue("@SortColumn", sortColumn);
            cmd.Parameters.AddWithValue("@SortDirection", sortDirection);

            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        

        public static DataTable GetByProfileName(int baseChannelID, string groupName, string searchCriteria, int folderID, int currentPage, int pageSize, bool allFolders, int customerID, int userID, string sortColumn, string sortDirection, string archiveFilter = "all", int? SubscriberLimit = null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Group_Get_ProfileName";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@ProfileName", groupName);
            cmd.Parameters.AddWithValue("@SearchCriteria", searchCriteria);
            if (!allFolders)
                cmd.Parameters.AddWithValue("@FolderID", folderID);
            cmd.Parameters.AddWithValue("@ArchiveFilter", archiveFilter);
            cmd.Parameters.AddWithValue("@CurrentPage", currentPage);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            if (SubscriberLimit.HasValue)
                cmd.Parameters.AddWithValue("@SubscriberLimit", SubscriberLimit.Value);

            cmd.Parameters.AddWithValue("@SortColumn", sortColumn);
            cmd.Parameters.AddWithValue("@SortDirection", sortDirection);

            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetSubscribers(int baseChannelID, int customerID, int userID, int folderID, int pageIndex, int pageSize, bool allFolders = false, string archiveFilter = "all", int? SubscriberLimit = null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Group_Get";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            if(!allFolders)
                cmd.Parameters.AddWithValue("@FolderID", folderID);

            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@CurrentPage", pageIndex);

            cmd.Parameters.AddWithValue("@ArchiveFilter", archiveFilter);

            if (SubscriberLimit.HasValue)
                cmd.Parameters.AddWithValue("@SubscriberLimit", SubscriberLimit.Value);

            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetTransactional(int customerID,string searchField,string searchWhere, string searchCriteria, int pageIndex, int pageSize, int folderID, bool allFolders, string ArchiveFilter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Group_GetTransactional_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@SearchWhere", searchWhere);
            cmd.Parameters.AddWithValue("@SearchCriteria", searchCriteria);
            cmd.Parameters.AddWithValue("@SearchField", searchField);
            cmd.Parameters.AddWithValue("@AllFolders", allFolders);
            cmd.Parameters.AddWithValue("@ArchiveFilter", ArchiveFilter);
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetNONTransactional(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Group_GetNONTransactional_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetGroupDR(int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Group_GetGroupDR";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataTable GetMSByDateRangeForCustomers(string startDate, string endDate, string customerIDs)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Group_GetMSByDateRangeForCustomers";
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            cmd.Parameters.AddWithValue("@CustomerIDs", customerIDs);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
