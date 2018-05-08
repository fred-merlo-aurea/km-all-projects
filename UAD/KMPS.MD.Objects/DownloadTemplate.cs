using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using FrameworkUAD.Entity;
using KM.Common;
using KMPlatform.Object;
using UADDownloadTemplate = FrameworkUAD.DataAccess.DownloadTemplate;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class DownloadTemplate : IDownloadTemplate
    {
        public DownloadTemplate() { }

        #region Properties
        [DataMember]
        public int DownloadTemplateID { get; set; }
        [DataMember]
        public string DownloadTemplateName { get; set; }
        [DataMember]
        public int BrandID { get; set; }
        [DataMember]
        public int PubID { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        #endregion

        #region Data
        public static List<DownloadTemplate> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<DownloadTemplate> templates = (List<DownloadTemplate>)CacheUtil.GetFromCache("DOWNLOADTEMPLATE", DatabaseName);

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

        public static List<DownloadTemplate> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<DownloadTemplate> retList = new List<DownloadTemplate>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_DownloadTemplate_Select", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<DownloadTemplate> builder = DynamicBuilder<DownloadTemplate>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    DownloadTemplate x = builder.Build(rdr);
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

        public static DownloadTemplate GetByID(KMPlatform.Object.ClientConnections clientconnection, int DownloadTemplateID)
        {
            return GetAll(clientconnection).Find(x => x.DownloadTemplateID == DownloadTemplateID);
        }

        public static List<DownloadTemplate> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int BrandID)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == BrandID);
        }

        public static List<DownloadTemplate> GetNotInBrand(KMPlatform.Object.ClientConnections clientconnection)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == 0);
        }

        public static List<DownloadTemplate> GetByPubIDBrandID(KMPlatform.Object.ClientConnections clientconnection, int PubID, int BrandID)
        {
            List<DownloadTemplate> retList = new List<DownloadTemplate>();
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
                DynamicBuilder<DownloadTemplate> builder = DynamicBuilder<DownloadTemplate>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    DownloadTemplate x = builder.Build(rdr);
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
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static void DeleteCache(ClientConnections clientConnections)
        {
            UADDownloadTemplate.DeleteCache(clientConnections);
        }
        #endregion

        #region CRUD
        public static int Save(ClientConnections clientConnections, IDownloadTemplate downloadTemplate)
        {
            return UADDownloadTemplate.Save(clientConnections, downloadTemplate);
        }

        public static void Delete(ClientConnections clientConnections, int downloadTemplateId, int userId)
        {
            UADDownloadTemplate.Delete(clientConnections, downloadTemplateId, userId);
        }
        #endregion
    }
}