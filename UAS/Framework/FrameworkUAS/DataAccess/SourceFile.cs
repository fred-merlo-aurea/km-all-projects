using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class SourceFile
    {
        public static List<Entity.SourceFile> Select()
        {
            List<Entity.SourceFile> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SourceFile_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.SourceFile> Select(string sfIds)
        {
            List<Entity.SourceFile> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SourceFile_Select_SourceFileIds";
            cmd.Parameters.AddWithValue("@sfIds", sfIds);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.SourceFile> SelectByFileType(int clientID, string fileType)
        {
            List<Entity.SourceFile> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SourceFile_Select_ClientId_FileType";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@DatabaseFileType", fileType);

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.SourceFile> Select(bool isDeleted)
        {
            List<Entity.SourceFile> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SourceFile_Select_IsDeleted";
            cmd.Parameters.AddWithValue("@IsDeleted", isDeleted);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.SourceFile> Select(int clientID, bool isDeleted)
        {
            List<Entity.SourceFile> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SourceFile_Select_ClientID_IsDeleted";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@IsDeleted", isDeleted);

            retItem = GetList(cmd);
            return retItem;
        }
        //public static List<Entity.SourceFile> Select(int clientID)
        //{
        //    List<Entity.SourceFile> retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_SourceFile_Select_ClientName";
        //    cmd.Parameters.AddWithValue("@ClientID", clientID);

        //    retItem = GetList(cmd);
        //    return retItem;
        //}

        public static List<Entity.SourceFile> Select(int clientID)
        {
            List<Entity.SourceFile> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SourceFile_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);

            retItem = GetList(cmd);
            return retItem;
        }

        public static Entity.SourceFile Select(int clientId, string fileName)
        {
            Entity.SourceFile retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SourceFile_Select_SearchClientFileMapping";
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            cmd.Parameters.AddWithValue("@FileName", fileName);

            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.SourceFile SelectSourceFileID(int sourceFileID)
        {
            Entity.SourceFile retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SourceFile_Select_SourceFileID";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);

            retItem = Get(cmd);
            return retItem;
        }

        private static Entity.SourceFile Get(SqlCommand cmd)
        {
            Entity.SourceFile retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SourceFile();
                        DynamicBuilder<Entity.SourceFile> builder = DynamicBuilder<Entity.SourceFile>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
        private static List<Entity.SourceFile> GetList(SqlCommand cmd)
        {
            List<Entity.SourceFile> retList = new List<Entity.SourceFile>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.SourceFile retItem = new Entity.SourceFile();
                        DynamicBuilder<Entity.SourceFile> builder = DynamicBuilder<Entity.SourceFile>.CreateBuilder(rdr);
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
            }
            catch{ }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }

        public static int Save(Entity.SourceFile x, bool defaultRules = true)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SourceFile_Save";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", x.SourceFileID));
            cmd.Parameters.Add(new SqlParameter("@FileRecurrenceTypeId", x.FileRecurrenceTypeId));
            cmd.Parameters.Add(new SqlParameter("@DatabaseFileTypeId", x.DatabaseFileTypeId));
            cmd.Parameters.Add(new SqlParameter("@FileName", x.FileName));
            cmd.Parameters.Add(new SqlParameter("@ClientID", x.ClientID));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", (object)x.PublicationID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", x.IsDeleted));
            cmd.Parameters.Add(new SqlParameter("@IsIgnored", x.IsIgnored));
            cmd.Parameters.Add(new SqlParameter("@FileSnippetID", x.FileSnippetID));
            cmd.Parameters.Add(new SqlParameter("@Extension", x.Extension));
            cmd.Parameters.Add(new SqlParameter("@IsDQMReady", x.IsDQMReady));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Delimiter", x.Delimiter));
            cmd.Parameters.Add(new SqlParameter("@IsTextQualifier", x.IsTextQualifier));
            cmd.Parameters.Add(new SqlParameter("@IsSpecialFile", x.IsSpecialFile));
            cmd.Parameters.Add(new SqlParameter("@ServiceID", x.ServiceID));
            cmd.Parameters.Add(new SqlParameter("@ServiceFeatureID", (object)x.ServiceFeatureID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MasterGroupID", x.MasterGroupID));
            cmd.Parameters.Add(new SqlParameter("@UseRealTimeGeocoding", x.UseRealTimeGeocoding));
            cmd.Parameters.Add(new SqlParameter("@ClientCustomProcedureID", x.ClientCustomProcedureID));
            cmd.Parameters.Add(new SqlParameter("@SpecialFileResultID", x.SpecialFileResultID));
            cmd.Parameters.Add(new SqlParameter("@QDateFormat", x.QDateFormat));
            cmd.Parameters.Add(new SqlParameter("@BatchSize", x.BatchSize));
            cmd.Parameters.Add(new SqlParameter("@IsPasswordProtected", x.IsPasswordProtected));
            cmd.Parameters.Add(new SqlParameter("@ProtectionPassword", x.ProtectionPassword));
            cmd.Parameters.Add(new SqlParameter("@NotifyEmailList", x.NotifyEmailList));
            cmd.Parameters.Add(new SqlParameter("@IsBillable ", x.IsBillable));
            cmd.Parameters.Add(new SqlParameter("@Notes", x.Notes));
            cmd.Parameters.Add(new SqlParameter("@defaultRules", defaultRules));
            cmd.Parameters.Add(new SqlParameter("@IsFullFile ", x.IsFullFile));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
        public static bool IsFileNameUnique(int clientId, string fileName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SourceFile_IsFileNameUnique";
            cmd.Parameters.AddWithValue("@clientId", clientId);
            cmd.Parameters.AddWithValue("@fileName", fileName);
            int fileNameCount = Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
            if (fileNameCount > 0)
                return false;
            else
                return true;
        }
        public static int Delete(int SourceFieldID, int ClientID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SourceFile_Delete_SourceFileByID";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", SourceFieldID));
            cmd.Parameters.Add(new SqlParameter("@ClientID", ClientID));            

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
        public static int Delete(int SourceFieldID, bool IsDeleted, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SourceFile_IsDelete_SourceFileID";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", SourceFieldID));
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", IsDeleted));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", UserID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static List<Entity.SourceFile> SelectPaging(int clientID, int currentPage, int pageSize, int serviceId, string type, int pubID, int fileTypeId, string filename, string sortField, string sortDirection)
        {
            List<Entity.SourceFile> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SourceFile_Paging_FileType_Product_FileName";            
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@CurrentPage", currentPage);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@ServiceID", serviceId);
            cmd.Parameters.AddWithValue("@Type", type);
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.Parameters.AddWithValue("@FileTypeID", fileTypeId);
            cmd.Parameters.AddWithValue("@FileName", filename);
            cmd.Parameters.AddWithValue("@SortField", sortField);
            cmd.Parameters.AddWithValue("@SortDirection", sortDirection);

            retItem = GetList(cmd);
            return retItem;
        }

        public static int SelectPagingCount(int clientID, int serviceId, string type, int pubID, int fileTypeId, string filename)
        {            
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SourceFile_Paging_Count_FileType_Product_FileName";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@ServiceID", serviceId);
            cmd.Parameters.AddWithValue("@Type", type);
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.Parameters.AddWithValue("@FileTypeID", fileTypeId);
            cmd.Parameters.AddWithValue("@FileName", filename);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
