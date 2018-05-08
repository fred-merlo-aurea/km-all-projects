using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FrameworkUAD.Entity;
using KM.Common;
using KMPlatform.Object;

namespace FrameworkUAD.DataAccess
{
    public class DownloadTemplate
    {
        private const string DownloadTemplateDeleteCommandText = "e_DownloadTemplate_Delete";
        private const string DownloadTemplateSaveCommandText = "e_DownloadTemplate_Save";
        private const string DownloadTemplateIdKey = "@DownloadTemplateID";
        private const string UserIdKey = "@UserID";
        private const string DownloadTemplateNameKey = "@DownloadTemplateName";
        private const string BrandIdKey = "@BrandID";
        private const string ProductIdKey = "@PubID";
        private const string IsDeletedKey = "@IsDeleted";
        private const string CreatedUserIdKey = "@CreatedUserID";
        private const string CreatedDateKey = "@CreatedDate";
        private const string UpdatedUserIdKey = "@UpdatedUserID";
        private const string UpdatedDateKey = "@UpdatedDate";
        private const string DownloadTemplateCacheKey = "DOWNLOADTEMPLATE";

        #region Data
        public static List<Entity.DownloadTemplate> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<Entity.DownloadTemplate> templates = (List<Entity.DownloadTemplate>) CacheUtil.GetFromCache("DOWNLOADTEMPLATE", DatabaseName);

                if (templates == null)
                {
                    templates = GetData(clientconnection);

                    CacheUtil.AddToCache("DOWNLOADTEMPLATE", templates, DatabaseName);
                }

                return templates;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        private static List<Entity.DownloadTemplate> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Entity.DownloadTemplate> retList = new List<Entity.DownloadTemplate>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_DownloadTemplate_Select", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Entity.DownloadTemplate> builder = DynamicBuilder<Entity.DownloadTemplate>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                Entity.DownloadTemplate x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        public static Entity.DownloadTemplate GetByID(KMPlatform.Object.ClientConnections clientconnection, int DownloadTemplateID)
        {
            return GetAll(clientconnection).Find(x => x.DownloadTemplateID == DownloadTemplateID);
        }

        public static List<Entity.DownloadTemplate> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int BrandID)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == BrandID);
        }

        public static List<Entity.DownloadTemplate> GetNotInBrand(KMPlatform.Object.ClientConnections clientconnection)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == 0);
        }

        public static List<Entity.DownloadTemplate> GetByPubIDBrandID(KMPlatform.Object.ClientConnections clientconnection, int PubID, int BrandID)
        {
            List<Entity.DownloadTemplate> retList = new List<Entity.DownloadTemplate>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_DownloadTemplate_Select_PubID_BrandID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PubID", PubID));
            cmd.Parameters.Add(new SqlParameter("@BrandID", BrandID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Entity.DownloadTemplate> builder = DynamicBuilder<Entity.DownloadTemplate>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Entity.DownloadTemplate x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        public static bool ExistsByDownloadTemplateName(KMPlatform.Object.ClientConnections clientconnection, int DownloadTemplateID, string DownloadTemplateName)
        {
            SqlCommand cmd = new SqlCommand("e_DownloadTemplate_Exists_TemplateName");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@DownloadTemplateName", DownloadTemplateName));
            cmd.Parameters.Add(new SqlParameter("@DownloadTemplateID", DownloadTemplateID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd))>0 ? true : false;
        }

        public static void DeleteCache(ClientConnections clientConnections)
        {
            Utilities.DeleteCache(clientConnections, DownloadTemplateCacheKey);
        }
        #endregion

        #region CRUD
        public static int Save(ClientConnections clientConnections, IDownloadTemplate downloadTemplate)
        {
            DeleteCache(clientConnections);
            return Utilities.Save(
                clientConnections,
                DownloadTemplateSaveCommandText,
                GetCommandParameter(downloadTemplate));
        }

        public static void Delete(ClientConnections clientConnections, int downloadTemplateId, int userId)
        {
            DeleteCache(clientConnections);
            Utilities.Delete(
                clientConnections,
                DownloadTemplateDeleteCommandText,
                GetCommandParameter(downloadTemplateId, userId));
        }
        
        private static IEnumerable<IDbDataParameter> GetCommandParameter(IDownloadTemplate downloadTemplate)
        {
            return new List<SqlParameter>()
                   {
                       new SqlParameter(DownloadTemplateIdKey, downloadTemplate.DownloadTemplateID),
                       new SqlParameter(DownloadTemplateNameKey, downloadTemplate.DownloadTemplateName),
                       new SqlParameter(CreatedUserIdKey, downloadTemplate.CreatedUserID),
                       new SqlParameter(CreatedDateKey, downloadTemplate.CreatedDate),
                       new SqlParameter(UpdatedUserIdKey, downloadTemplate.UpdatedUserID),
                       new SqlParameter(UpdatedDateKey, downloadTemplate.UpdatedDate),
                       new SqlParameter(IsDeletedKey, downloadTemplate.IsDeleted),
                       new SqlParameter(BrandIdKey, downloadTemplate.BrandID),
                       new SqlParameter(ProductIdKey, downloadTemplate.PubID),
                   };
        }

        private static IEnumerable<IDbDataParameter> GetCommandParameter(int downloadTemplateId, int userId)
        {
            return new List<SqlParameter>()
                   {
                       new SqlParameter(DownloadTemplateIdKey, downloadTemplateId),
                       new SqlParameter(UserIdKey, userId)
                   };
        }
        #endregion
    }
}