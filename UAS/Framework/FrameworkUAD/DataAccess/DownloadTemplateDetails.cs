using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using KM.Common;
namespace FrameworkUAD.DataAccess
{
    public class DownloadTemplateDetails
    {
        #region Data
        public static List<Entity.DownloadTemplateDetails> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<Entity.DownloadTemplateDetails> templatedetails = (List<Entity.DownloadTemplateDetails>) CacheUtil.GetFromCache("DOWNLOADTEMPLATEDETAILS", DatabaseName);

                if (templatedetails == null)
                {
                    templatedetails = GetData(clientconnection);

                    CacheUtil.AddToCache("DOWNLOADTEMPLATEDETAILS", templatedetails, DatabaseName);
                }

                return templatedetails;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        private static List<Entity.DownloadTemplateDetails> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Entity.DownloadTemplateDetails> retList = new List<Entity.DownloadTemplateDetails>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_DownloadTemplateDetails_Select", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Entity.DownloadTemplateDetails> builder = DynamicBuilder<Entity.DownloadTemplateDetails>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Entity.DownloadTemplateDetails x = builder.Build(rdr);
                    if (x.IsDescription)
                        x.ExportColumn = x.ExportColumn + "_Description";
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

        public static List<Entity.DownloadTemplateDetails> GetByDownloadTemplateID(KMPlatform.Object.ClientConnections clientconnection, int DownloadTemplateID)
        {
            return GetAll(clientconnection).FindAll(x => x.DownloadTemplateID == DownloadTemplateID);
        }

        public static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("DOWNLOADTEMPLATEDETAILS", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("DOWNLOADTEMPLATEDETAILS", DatabaseName);
                }
            }
        }

        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, Entity.DownloadTemplateDetails t)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_DownloadTemplateDetails_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            cmd.Parameters.Add(new SqlParameter("@DownloadTemplateID", t.DownloadTemplateID));
            cmd.Parameters.Add(new SqlParameter("@ExportColumn", t.ExportColumn));
            cmd.Parameters.Add(new SqlParameter("@IsDescription", t.IsDescription));
            cmd.Parameters.Add(new SqlParameter("@FieldCase", (object) t.FieldCase ?? DBNull.Value));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int DownloadTemplateID)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_DownloadTemplateDetails_Delete");
            cmd.Connection = DataFunctions.GetClientSqlConnection(clientconnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@DownloadTemplateID", DownloadTemplateID));
            DataFunctions.ExecuteScalar(cmd);
        }
        #endregion
    }
}
