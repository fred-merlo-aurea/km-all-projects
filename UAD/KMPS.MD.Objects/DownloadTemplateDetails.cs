using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class DownloadTemplateDetails
    {
        public DownloadTemplateDetails() { }

        #region Properties
        [DataMember]
        public int DownloadTemplateDetailsID { get; set; }
        [DataMember]
        public int DownloadTemplateID { get; set; }
        [DataMember]
        public string ExportColumn { get; set; }
        [DataMember]
        public bool IsDescription { get; set; }
        [DataMember]
        public string FieldCase { get; set; }
        #endregion

        #region Data
        public static List<DownloadTemplateDetails> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<DownloadTemplateDetails> templatedetails = (List<DownloadTemplateDetails>)CacheUtil.GetFromCache("DOWNLOADTEMPLATEDETAILS", DatabaseName);

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

        public static List<DownloadTemplateDetails> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<DownloadTemplateDetails> retList = new List<DownloadTemplateDetails>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_DownloadTemplateDetails_Select", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<DownloadTemplateDetails> builder = DynamicBuilder<DownloadTemplateDetails>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    DownloadTemplateDetails x = builder.Build(rdr);
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

        public static List<DownloadTemplateDetails> GetByDownloadTemplateID(KMPlatform.Object.ClientConnections clientconnection, int DownloadTemplateID)
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
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, DownloadTemplateDetails t)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_DownloadTemplateDetails_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@DownloadTemplateID", t.DownloadTemplateID));
            cmd.Parameters.Add(new SqlParameter("@ExportColumn", t.ExportColumn));
            cmd.Parameters.Add(new SqlParameter("@IsDescription", t.IsDescription));
            cmd.Parameters.Add(new SqlParameter("@FieldCase", (object)t.FieldCase ?? DBNull.Value));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int DownloadTemplateID)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_DownloadTemplateDetails_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@DownloadTemplateID", DownloadTemplateID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}
