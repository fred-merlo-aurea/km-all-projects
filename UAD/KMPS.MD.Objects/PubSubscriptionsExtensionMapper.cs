using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;
using KMPlatform.Object;
using UADUtilities = FrameworkUAD.DataAccess.Utilities;

namespace KMPS.MD.Objects
{
    [Serializable]
    public class PubSubscriptionsExtensionMapper
    {
        private const string PubSubscriptionsExtensionMapperDeleteCommandText = "e_PubSubscriptionsExtensionMapper_Delete";
        private const string PubSubscriptionsExtensionMapperValidateDeleteOrInactive = "e_PubSubscriptionsExtensionMapper_Validate_DeleteorInActive";
        private const string PubSubscriptionsExtensionMapperIdKey = "@PubSubscriptionsExtensionMapperId";
        private const string ProductIdKey = "@PubID";

        #region Properties
        public int PubSubscriptionsExtensionMapperId { get; set; }
        public int PubID { get; set; }
        public string StandardField { get; set; }
        public string CustomField { get; set; }
        public string CustomFieldDataType { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int CreatedByUserID { get; set; }
        public int UpdatedByUserID { get; set; }
        #endregion

        #region Data
        public static List<PubSubscriptionsExtensionMapper> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<PubSubscriptionsExtensionMapper> PubSubscriptionsExtensionMappers = (List<PubSubscriptionsExtensionMapper>)CacheUtil.GetFromCache("PUBSUBSCRIPTIONSEXTENSIONMAPPER", DatabaseName);

                if (PubSubscriptionsExtensionMappers == null)
                {
                    PubSubscriptionsExtensionMappers = GetData(clientconnection);

                    CacheUtil.AddToCache("PUBSUBSCRIPTIONSEXTENSIONMAPPER", PubSubscriptionsExtensionMappers, DatabaseName);
                }

                return PubSubscriptionsExtensionMappers;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        private static List<PubSubscriptionsExtensionMapper> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<PubSubscriptionsExtensionMapper> retList = new List<PubSubscriptionsExtensionMapper>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_PubSubscriptionsExtensionMapper_Select", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<PubSubscriptionsExtensionMapper> builder = DynamicBuilder<PubSubscriptionsExtensionMapper>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    PubSubscriptionsExtensionMapper x = builder.Build(rdr);
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


        public static List<PubSubscriptionsExtensionMapper> GetActiveByPubID(KMPlatform.Object.ClientConnections clientconnection, int pubID)
        {
            var PubSubscriptionsExtensionMapperList = GetAll(clientconnection);

            var SEQuery = (from s in PubSubscriptionsExtensionMapperList
                           where s.Active == true && s.PubID == @pubID
                                    select s);

            return SEQuery.ToList();
        }

        public static List<PubSubscriptionsExtensionMapper> GetByPubID(KMPlatform.Object.ClientConnections clientconnection, int pubID)
        {
            return GetAll(clientconnection).FindAll(x => x.PubID == pubID);
        }

        public static PubSubscriptionsExtensionMapper GetByID(KMPlatform.Object.ClientConnections clientconnection, int pubSubscriptionsExtensionMapperId)
        {
            return GetAll(clientconnection).Find(x => x.PubSubscriptionsExtensionMapperId == pubSubscriptionsExtensionMapperId);
        }

        public static bool ExistsCustomField(KMPlatform.Object.ClientConnections clientconnection, string customField)
        {
            SqlCommand cmd = new SqlCommand("e_PubSubscriptionsExtensionMapper_Exists_ByCustomField");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CustomField", customField));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static bool ExistsByIDCustomField(KMPlatform.Object.ClientConnections clientconnection, int pubSubscriptionsExtensionMapperID, string customField, int PubID)
        {
            SqlCommand cmd = new SqlCommand("e_PubSubscriptionsExtensionMapper_Exists_ByIDCustomField");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionsExtensionMapperID", pubSubscriptionsExtensionMapperID));
            cmd.Parameters.Add(new SqlParameter("@CustomField", customField));
            cmd.Parameters.Add(new SqlParameter("@PubID", PubID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }
        #endregion

        public static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("PUBSUBSCRIPTIONSEXTENSIONMAPPER", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("PUBSUBSCRIPTIONSEXTENSIONMAPPER", DatabaseName);
                }
            }
        }

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, PubSubscriptionsExtensionMapper p)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_PubSubscriptionsExtensionMapper_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionsExtensionMapperId", p.PubSubscriptionsExtensionMapperId));
            cmd.Parameters.Add(new SqlParameter("@PubID", p.PubID));
            cmd.Parameters.Add(new SqlParameter("@CustomField", p.CustomField));
            cmd.Parameters.Add(new SqlParameter("@CustomFieldDataType", p.CustomFieldDataType));
            cmd.Parameters.Add(new SqlParameter("@Active", p.Active));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", p.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", p.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", p.UpdatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", p.DateUpdated));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static NameValueCollection ValidationForDeleteorInActive(ClientConnections clientConnections, int pubSubscriptionsExtensionMapperId, int productId)
        {
            var result = UADUtilities.ValidateDeleteOrInActive(
                clientConnections,
                PubSubscriptionsExtensionMapperValidateDeleteOrInactive,
                GetCommandParameter(pubSubscriptionsExtensionMapperId, productId),
                FilterExportScheduleProcessor.ProcessDataReader);

            return result;
        }

        public static void Delete(ClientConnections clientConnections, int pubSubscriptionsExtensionMapperId, int productId)
        {
            DeleteCache(clientConnections);
            UADUtilities.Delete(
                clientConnections,
                PubSubscriptionsExtensionMapperDeleteCommandText, 
                GetCommandParameter(pubSubscriptionsExtensionMapperId, productId));
        }

        private static IEnumerable<SqlParameter> GetCommandParameter(int pubSubscriptionExtensionMapperId, int productId)
        {
            return new List<SqlParameter>()
            {
                new SqlParameter(PubSubscriptionsExtensionMapperIdKey, pubSubscriptionExtensionMapperId),
                new SqlParameter(ProductIdKey, productId)
            };
        }

        #endregion
    }
}
