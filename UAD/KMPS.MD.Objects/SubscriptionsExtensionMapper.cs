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
    public class SubscriptionsExtensionMapper
    {
        private const string SubscriptionsExtensionMapperDeleteCommandText = "e_SubscriptionsExtensionMapper_Delete";
        private const string SubscriptionsExtensionMapperIdParameterName = "@SubscriptionsExtensionMapperId";
        private const string SubscriptionsExtensionMapperValidateDeleteOrInactiveCommandText = "e_SubscriptionsExtensionMapper_Validate_DeleteorInActive";

        #region Properties
        public int SubscriptionsExtensionMapperId { get; set; }
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
        public static List<SubscriptionsExtensionMapper> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<SubscriptionsExtensionMapper> subscriptionsextensionmappers = (List<SubscriptionsExtensionMapper>)CacheUtil.GetFromCache("SUBSCRIPTIONSEXTENSIONMAPPER", DatabaseName);

                if (subscriptionsextensionmappers == null)
                {
                    subscriptionsextensionmappers = GetData(clientconnection);

                    CacheUtil.AddToCache("SUBSCRIPTIONSEXTENSIONMAPPER", subscriptionsextensionmappers, DatabaseName);
                }

                return subscriptionsextensionmappers;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        private static List<SubscriptionsExtensionMapper> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<SubscriptionsExtensionMapper> retList = new List<SubscriptionsExtensionMapper>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_SubscriptionsExtensionMapper_Select_All", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<SubscriptionsExtensionMapper> builder = DynamicBuilder<SubscriptionsExtensionMapper>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    SubscriptionsExtensionMapper x = builder.Build(rdr);
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


        public static List<SubscriptionsExtensionMapper> GetActive(KMPlatform.Object.ClientConnections clientconnection)
        {
            var SubscriptionsExtensionMapperList = GetAll(clientconnection);

            var SEQuery = (from s in SubscriptionsExtensionMapperList
                                    where s.Active == true 
                                    select s);

            return SEQuery.ToList();
        }

        public static SubscriptionsExtensionMapper GetByID(KMPlatform.Object.ClientConnections clientconnection, int subscriptionsExtensionMapperId)
        {
            return GetAll(clientconnection).Find(x => x.SubscriptionsExtensionMapperId == subscriptionsExtensionMapperId); 
        }

        public static bool ExistsByName(KMPlatform.Object.ClientConnections clientconnection, string name)
        {
            SqlCommand cmd = new SqlCommand("e_SubscriptionsExtensionMapper_Exists_ByName");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name);
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static bool ExistsByIDName(KMPlatform.Object.ClientConnections clientconnection, int subscriptionsExtensionMapperID, string name)
        {
            SqlCommand cmd = new SqlCommand("e_SubscriptionsExtensionMapper_Exists_ByIDName");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SubscriptionsExtensionMapperID", subscriptionsExtensionMapperID);
            cmd.Parameters.AddWithValue("@Name", name);
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }
        #endregion

        public static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("SUBSCRIPTIONSEXTENSIONMAPPER", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("SUBSCRIPTIONSEXTENSIONMAPPER", DatabaseName);
                }
            }
        }

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, int subscriptionsExtensionMapperId, string customField, string dataType, bool active)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_SubscriptionsExtensionMapper_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(SubscriptionsExtensionMapperIdParameterName, subscriptionsExtensionMapperId);
            cmd.Parameters.AddWithValue("@CustomField", customField);
            cmd.Parameters.AddWithValue("@CustomFieldDataType", dataType);
            cmd.Parameters.AddWithValue("@Active", active);
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static NameValueCollection ValidationForDeleteorInActive(ClientConnections clientConnections, int subscriptionsExtensionMapperId)
        {
            var result = UADUtilities.ValidateDeleteOrInActive(
                clientConnections,
                SubscriptionsExtensionMapperValidateDeleteOrInactiveCommandText,
                GetCommandParameter(subscriptionsExtensionMapperId),
                FilterExportScheduleProcessor.ProcessDataReader);

            return result;
        }

        public static void Delete(ClientConnections clientConnections, int subscriptionsExtensionMapperId)
        {
            DeleteCache(clientConnections);
            UADUtilities.Delete(clientConnections, SubscriptionsExtensionMapperDeleteCommandText, GetCommandParameter(subscriptionsExtensionMapperId));
        }

        private static IEnumerable<SqlParameter> GetCommandParameter(int subscriptionExtensionMapperId)
        {
            return new List<SqlParameter>()
            {
                new SqlParameter(SubscriptionsExtensionMapperIdParameterName, subscriptionExtensionMapperId)
            };
        }

        #endregion
    }
}