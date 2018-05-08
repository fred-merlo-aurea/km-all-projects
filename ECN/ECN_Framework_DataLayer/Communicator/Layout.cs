using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using ECN_Framework_DataLayer.Communicator.Helpers;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class Layout
    {
        private static string _CacheRegion = "Layout";
        private const string ProcedureLayoutSelectSearch = "e_Layout_Select_Search";
        private const string ProcedureLayoutExistsByName = "e_Layout_Exists_ByName";
        private const string ProcedureLayoutExistsContentId = "e_Layout_Exists_ContentID";
        private const string ProcedureLayoutIsValidatedById = "e_Layout_IsValidated_ByID";
        private const string ProcedureLayoutExistsTemplateId = "e_Layout_Exists_TemplateID";

        public static bool CreatedUserExists(int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_Layout_IsCreater";
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataTable dtResult = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            string result = dtResult.DefaultView[0][0].ToString();
            return result.ToUpper() == "TRUE";
        }

        public static bool Exists(int layoutID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Layout_Exists_ByID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }
        public static bool IsArchived(int layoutID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Layout_IsArchived_ByID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }
        public static bool IsValidated(int layoutID, int customerID)
        {
            var readAndFillParams = new FillCommunicatorArgs {LayoutId = layoutID, CustomerId = customerID};
            return CommunicatorMethodsHelper.ExecuteScalar(readAndFillParams, ProcedureLayoutIsValidatedById) > 0;
        }
        public static bool Exists(int layoutID, string layoutName, int folderID, int customerID)
        {
            var readAndFillParams = new FillCommunicatorArgs
            {
                CustomerId = customerID,
                LayoutName = layoutName,
                FolderId = folderID,
                LayoutId = layoutID
            };
            return CommunicatorMethodsHelper.ExecuteScalar(readAndFillParams, ProcedureLayoutExistsByName) > 0;
        }

        public static bool ContentUsedInLayout(int contentID)
        {
            var readAndFillParams = new FillCommunicatorArgs {ContentId = contentID};
            return CommunicatorMethodsHelper.ExecuteScalar(readAndFillParams, ProcedureLayoutExistsContentId) > 0;
        }

        public static bool TemplateUsedInLayout(int templateID)
        {
            var readAndFillParams = new FillCommunicatorArgs {TemplateId = templateID};
            return CommunicatorMethodsHelper.ExecuteScalar(readAndFillParams, ProcedureLayoutExistsTemplateId) > 0;
        }

        public static bool MessageTypeUsedInLayout(int messageTypeID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Layout_Exists_MessageTypeID";
            cmd.Parameters.AddWithValue("@MessageTypeID", messageTypeID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Communicator.Layout> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Layout_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.Layout GetByLayoutID(int layoutID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Layout_Select_LayoutID";
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);

            ECN_Framework_Entities.Communicator.Layout retItem = null;

            bool isCacheEnabled = false;
            try
            {
                isCacheEnabled = KM.Common.CacheUtil.IsCacheEnabled();
            }
            catch (Exception) { }

            if (isCacheEnabled)
            {
                retItem = (ECN_Framework_Entities.Communicator.Layout)KM.Common.CacheUtil.GetFromCache(layoutID.ToString(), _CacheRegion);
                if(retItem == null)
                {
                    retItem = Get(cmd);
                    if(retItem != null)
                        KM.Common.CacheUtil.AddToCache(layoutID.ToString(), retItem, _CacheRegion);
                }
            }  
            else
                retItem = Get(cmd);
            return retItem;
        }

        public static List<ECN_Framework_Entities.Communicator.Layout> GetByFolderIDCustomerID(int folderID, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Layout_Select_FolderID_CustomerID";
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Communicator.Layout Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.Layout retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.Layout();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Layout>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.Layout> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.Layout> retList = new List<ECN_Framework_Entities.Communicator.Layout>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.Layout retItem = new ECN_Framework_Entities.Communicator.Layout();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Layout>.CreateBuilder(rdr);
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

        public static bool FolderUsed(int folderID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Layout_Count_FolderID";
            cmd.Parameters.AddWithValue("@FolderID", folderID);
            
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }
        
        public static int Save(ECN_Framework_Entities.Communicator.Layout layout)
        {
            if(layout.LayoutID > 0)
                DeleteCache(layout.LayoutID);
            
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Layout_Save";
            cmd.Parameters.Add(new SqlParameter("@LayoutID", layout.LayoutID));
            cmd.Parameters.Add(new SqlParameter("@TemplateID", (object)layout.TemplateID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)layout.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FolderID", (object)layout.FolderID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@LayoutName", layout.LayoutName));
            cmd.Parameters.Add(new SqlParameter("@ContentSlot1", (object)layout.ContentSlot1 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ContentSlot2", (object)layout.ContentSlot2 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ContentSlot3", (object)layout.ContentSlot3 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ContentSlot4", (object)layout.ContentSlot4 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ContentSlot5", (object)layout.ContentSlot5 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ContentSlot6", (object)layout.ContentSlot6 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ContentSlot7", (object)layout.ContentSlot7 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ContentSlot8", (object)layout.ContentSlot8 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ContentSlot9", (object)layout.ContentSlot9 ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@TableOptions", layout.TableOptions));
            cmd.Parameters.Add(new SqlParameter("@DisplayAddress", layout.DisplayAddress));
            cmd.Parameters.Add(new SqlParameter("@SetupCost", layout.SetupCost));
            cmd.Parameters.Add(new SqlParameter("@OutboundCost", layout.OutboundCost));
            cmd.Parameters.Add(new SqlParameter("@InboundCost", layout.InboundCost));
            cmd.Parameters.Add(new SqlParameter("@DesignCost", layout.DesignCost));
            cmd.Parameters.Add(new SqlParameter("@OtherCost", layout.OtherCost));
            cmd.Parameters.Add(new SqlParameter("@MessageTypeID", (object)layout.MessageTypeID ?? DBNull.Value));
            if (layout.LayoutID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)layout.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)layout.CreatedUserID ?? DBNull.Value));
            cmd.Parameters.AddWithValue("@Archived", layout.Archived.Value);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int layoutID, int customerID, int userID)
        {
            DeleteCache(layoutID);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Layout_Delete";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static void DeleteCache(int layoutID)
        {
            if (KM.Common.CacheUtil.IsCacheEnabled())
            {
                if (KM.Common.CacheUtil.GetFromCache(layoutID.ToString(), _CacheRegion) != null)
                    KM.Common.CacheUtil.RemoveFromCache(layoutID.ToString(), _CacheRegion);
            }
        }

        public static DataSet GetByLayoutName(string name, int? folderID, int customerID, int? userID,int ValidatedOnly,DateTime? updatedDateFrom, DateTime? updatedDateTo, int baseChannelID, int CurrentPage, int PageSize, string SortDirection, string SortColumn,string ArchiveFilter)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Layout_Select_Name";
            if (name.Trim().Length > 0)
            {
                cmd.Parameters.AddWithValue("@LayoutName", name);
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
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            
            cmd.Parameters.AddWithValue("@CurrentPage", CurrentPage);
            cmd.Parameters.AddWithValue("@PageSize", PageSize);
            cmd.Parameters.AddWithValue("@SortDirection", SortDirection);
            cmd.Parameters.AddWithValue("@SortColumn", SortColumn);
            cmd.Parameters.AddWithValue("@ArchiveFilter", ArchiveFilter);
            cmd.Parameters.AddWithValue("@ValidatedOnly", ValidatedOnly);
            return DataFunctions.GetDataSet(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.Layout> GetByLayoutSearch(string name, int? folderID, int customerID, int? userID, DateTime? updatedDateFrom, DateTime? updatedDateTo,
             bool? archived = null)
        {
            var readAndFillParams = new FillLayoutContentArgs
            {
                LayoutName = name,
                FolderId = folderID,
                UserId = userID,
                UpdatedDateFrom = updatedDateFrom,
                UpdatedDateTo = updatedDateTo,
                Archived = archived,
                CustomerId = customerID
            };
            var command = SqlParameterHelper<FillLayoutContentArgs>.CreateCommand(readAndFillParams, ProcedureLayoutSelectSearch);

            return GetList(command);
        }

        public static DataTable GetByLayoutID(int layoutID, int customerID, int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Layout_LayoutID";
            cmd.Parameters.AddWithValue("@LayoutID", layoutID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static System.Collections.Generic.List<string> ValidateLayoutContent(int layoutID)
        {
            System.Collections.Generic.List<string> listLY = new System.Collections.Generic.List<string>();
            StringBuilder sbLY = new StringBuilder();
            sbLY.Append(" select ContentSource as Content from Layout ");
            sbLY.Append(" join Content on  Content.ContentID = Layout.ContentSlot1 or Content.ContentID = Layout.ContentSlot2 or  Content.ContentID = Layout.ContentSlot3 or ");
            sbLY.Append(" Content.ContentID = Layout.ContentSlot4 or Content.ContentID = Layout.ContentSlot5 or Content.ContentID = Layout.ContentSlot6 or ");
            sbLY.Append(" Content.ContentID = Layout.ContentSlot7 or Content.ContentID = Layout.ContentSlot8 or Content.ContentID = Layout.ContentSlot9  ");
            sbLY.Append(" where layout.layoutID in (" + layoutID + ") and Layout.IsDeleted = 0 and Content.IsDeleted = 0 union all ");
            sbLY.Append(" select ContentText as Content from Layout ");
            sbLY.Append(" join Content on  Content.ContentID = Layout.ContentSlot1 or Content.ContentID = Layout.ContentSlot2 or  Content.ContentID = Layout.ContentSlot3 or ");
            sbLY.Append(" Content.ContentID = Layout.ContentSlot4 or Content.ContentID = Layout.ContentSlot5 or Content.ContentID = Layout.ContentSlot6 or ");
            sbLY.Append(" Content.ContentID = Layout.ContentSlot7 or Content.ContentID = Layout.ContentSlot8 or Content.ContentID = Layout.ContentSlot9 ");
            sbLY.Append(" where layout.layoutID in (" + layoutID + ") and Layout.IsDeleted = 0 and Content.IsDeleted = 0 union all ");
            sbLY.Append(" select ContentMobile as Content from Layout ");
            sbLY.Append(" join Content on  Content.ContentID = Layout.ContentSlot1 or Content.ContentID = Layout.ContentSlot2 or  Content.ContentID = Layout.ContentSlot3 or ");
            sbLY.Append(" Content.ContentID = Layout.ContentSlot4 or Content.ContentID = Layout.ContentSlot5 or Content.ContentID = Layout.ContentSlot6 or ");
            sbLY.Append(" Content.ContentID = Layout.ContentSlot7 or Content.ContentID = Layout.ContentSlot8 or Content.ContentID = Layout.ContentSlot9 ");
            sbLY.Append(" where layout.layoutID in (" + layoutID + ") and Layout.IsDeleted = 0 and Content.IsDeleted = 0");
            
            DataTable dtLY = new DataTable();
            dtLY = DataFunctions.GetDataTable(sbLY.ToString(), DataFunctions.ConnectionString.Communicator.ToString());

            foreach (DataRow dr in dtLY.Rows)
            {
                listLY.Add(dr["Content"].ToString().ToLower());
            }

            return listLY;
        }

        public static DataTable GetLayoutDR(int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_Layout_GetLayoutDR";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
