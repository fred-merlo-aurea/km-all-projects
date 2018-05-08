using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Communicator.Helpers;
using KM.Common;
using CommContent = ECN_Framework_Entities.Communicator.Content;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class Content
    {
        private const string ProcedureContentSelectTitle = "v_Content_Select_Title";

        public static bool CreatedUserExists(int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_Content_IsCreater";
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataTable dtResult = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Accounts.ToString());
            string result = dtResult.DefaultView[0][0].ToString();
            return result.ToUpper() == "TRUE";
        }

        public static ECN_Framework_Entities.Communicator.Content GetByContentID(int contentID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Content_Select_ContentID";
            cmd.Parameters.AddWithValue("@ContentID", contentID);
            return Get(cmd);
        }
        public static bool GetValidatedStatusByContentID(int contentID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Content_ValidatedStatus_ContentID";
            cmd.Parameters.AddWithValue("@ContentID", contentID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }      

        public static List<ECN_Framework_Entities.Communicator.Content> GetByFolderID(int folderID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Content_Select_FolderID";
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.Content> GetByFolderIDCustomerID(int folderID, int CustomerID, string archiveFilter = "active")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Content_Select_FolderID_CustomerID";
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@ArchiveFilter", archiveFilter);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.Content> GetByLayoutID(int layoutID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Content_Select_LayoutID";
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.Content> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.Content> retList = new List<ECN_Framework_Entities.Communicator.Content>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.Content retItem = new ECN_Framework_Entities.Communicator.Content();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Content>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.Content Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.Content retItem = null;
            
            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.Content();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Content>.CreateBuilder(rdr);
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

        //private static ECN_Framework_Entities.Communicator.Content GetFromAccounts(SqlCommand cmd)
        //{
        //    ECN_Framework_Entities.Communicator.Content retItem = null;

        //    using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
        //    {
        //        if (rdr != null)
        //        {
        //            retItem = new ECN_Framework_Entities.Communicator.Content();
        //            var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Content>.CreateBuilder(rdr);
        //            while (rdr.Read())
        //            {
        //                retItem = builder.Build(rdr);
        //            }
        //            rdr.Close();
        //            rdr.Dispose();
        //        }
        //    }
        //    cmd.Connection.Close();
        //    cmd.Dispose();
        //    return retItem;
        //}

        public static bool Exists(int contentID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Content_Exists_ByID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@ContentID", contentID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int contentID, string contentTitle, int folderID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Content_Exists_ByName";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@ContentTitle", contentTitle);
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            cmd.Parameters.AddWithValue("@ContentID", contentID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(string contentTitle, int customerID, int? contentID)//if contentid is not null we will exclude it
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            if (contentID == null)
            {
                cmd.CommandText = "if exists (select top 1 ContentID FROM Content WHERE CustomerID=@CustomerID and ContentTitle = @ContentTitle and IsDeleted = 0) select 1 else select 0";
            }
            else
            {
                cmd.CommandText = "if exists (select top 1 ContentID FROM Content WHERE CustomerID=@CustomerID and ContentTitle = @ContentTitle and ContentID != ContentID and IsDeleted = 0) select 1 else select 0";
                cmd.Parameters.AddWithValue("@ContentID", contentID.Value);
            }
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@ContentTitle", contentTitle);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static int Save(ECN_Framework_Entities.Communicator.Content content)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Content_Save";
            cmd.Parameters.Add(new SqlParameter("@ContentID", (object)content.ContentID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ContentTitle", content.ContentTitle));
            cmd.Parameters.Add(new SqlParameter("@ContentTypeCode", content.ContentTypeCode));
            cmd.Parameters.Add(new SqlParameter("@LockedFlag", content.LockedFlag));
            cmd.Parameters.Add(new SqlParameter("@FolderID", content.FolderID));
            cmd.Parameters.Add(new SqlParameter("@ContentSource", content.ContentSource));
            cmd.Parameters.Add(new SqlParameter("@ContentMobile", content.ContentMobile));
            cmd.Parameters.Add(new SqlParameter("@ContentText", content.ContentText));
            cmd.Parameters.Add(new SqlParameter("@ContentURL", content.ContentURL));
            cmd.Parameters.Add(new SqlParameter("@ContentFilePointer", content.ContentFilePointer));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", content.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@Sharing", content.Sharing));
            cmd.Parameters.Add(new SqlParameter("@UseWYSIWYGeditor", content.UseWYSIWYGeditor));
            if (content.ContentID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)content.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)content.CreatedUserID ?? DBNull.Value));
            cmd.Parameters.AddWithValue("@Archived", (object)content.Archived ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Validated", (object)content.IsValidated ?? DBNull.Value);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static bool FolderUsed(int folderID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 ContentID from Content where FolderID = @FolderID and IsDeleted = 0) select 1 else select 0";
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static void Delete(int layoutID, int customerID, int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Content_Delete";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@ContentID", layoutID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static DataSet GetByContentTitle(string title, int? folderID, int? ValidatedOnly,int customerID , int? userID, DateTime? updatedDateFrom, DateTime? updatedDateTo, int baseChannelID, int currentPage, int pageSize, string sortDirection, string sortColumn,string archiveFilter)
        {
            var readAndFillParams = new FillLayoutContentArgs
            {
                ContentTitle = title,
                FolderId = folderID,
                UserId = userID,
                UpdatedDateFrom = updatedDateFrom,
                UpdatedDateTo = updatedDateTo,
                CustomerId = customerID,
                BaseChannelId = baseChannelID,
                CurrentPage = currentPage,
                PageSize = pageSize,
                SortDirection = sortDirection,
                SortColumn = sortColumn,
                ValidatedOnly = ValidatedOnly,
                ArchiveFilter = archiveFilter
            };

            var cmd = SqlParameterHelper<FillLayoutContentArgs>.CreateCommand(readAndFillParams, ProcedureContentSelectTitle);
            return DataFunctions.GetDataSet(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static List<CommContent> GetListByContentTitle(
            string title,
            int? folderId,
            int customerId,
            int? userId,
            DateTime? updatedDateFrom,
            DateTime? updatedDateTo,
            int baseChannelId,
            int currentPage,
            int pageSize,
            string sortDirection,
            string sortColumn,
            string archiveFilter)
        {
            var contentList = new List<CommContent>();
            var readAndFillParams = new FillLayoutContentArgs
            {
                ContentTitle = title,
                FolderId = folderId,
                UserId = userId,
                UpdatedDateFrom = updatedDateFrom,
                UpdatedDateTo = updatedDateTo,
                CustomerId = customerId,
                BaseChannelId = baseChannelId,
                CurrentPage = currentPage,
                PageSize = pageSize,
                SortDirection = sortDirection,
                SortColumn = sortColumn,
                ArchiveFilter = archiveFilter
            };

            var command = SqlParameterHelper<FillLayoutContentArgs>.CreateCommand(readAndFillParams, ProcedureContentSelectTitle);
            AddContentListItems(contentList, command);

            return contentList;
        }

        public static void AddContentListItems(IList<CommContent> contentList, SqlCommand command)
        {
            Guard.NotNull(contentList, nameof(contentList));
            Guard.NotNull(command, nameof(command));

            try
            {
                using (var reader = DataFunctions.ExecuteReader(command, DataFunctions.ConnectionString.Communicator.ToString()))
                {
                    if (reader != null)
                    {
                        var contentItem = new CommContent();
                        var builder = DynamicBuilder<CommContent>.CreateBuilder(reader);
                        while (reader.Read())
                        {
                            contentItem = builder.Build(reader);
                            if (contentItem != null)
                            {
                                contentList.Add(contentItem);
                            }
                        }
                    }
                }
            }
            finally
            {
                command.Connection?.Close(); 
                command.Dispose(); 
            }
        }

        public static List<ECN_Framework_Entities.Communicator.Content> GetByContentSearch(string title, int? folderID, int customerID, int? userID, DateTime? updatedDateFrom, DateTime? updatedDateTo,
            bool? archived = null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Content_Select_Search";
            if (title.Trim().Length > 0)
            {
                cmd.Parameters.AddWithValue("@ContentTitle", title);
            }
            if (folderID != null)
            {
                cmd.Parameters.AddWithValue("@FolderID", folderID);
            }
            if (userID != null)
            {
                cmd.Parameters.AddWithValue("@UserID", userID);
            }
            if (updatedDateFrom != null)
            {
                cmd.Parameters.AddWithValue("@UpdatedDateFrom", updatedDateFrom);
            }
            if (updatedDateTo != null)
            {
                cmd.Parameters.AddWithValue("@UpdatedDateTo", updatedDateTo);
            }
            if (archived != null)
            {
                cmd.Parameters.AddWithValue("@Archived", archived.Value);
            }
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return GetList(cmd);

        }
    }
}
