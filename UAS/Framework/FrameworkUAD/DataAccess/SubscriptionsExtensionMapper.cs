using KM.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FrameworkUAD.DataAccess
{
    public class SubscriptionsExtensionMapper
    {
        public static bool ExistsByCustomField(string customField, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionsExtensionMapper_Exists_ByCustomField";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@CustomField", customField);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }
        public static bool ExistsByIDCustomField(int subscriptionsExtensionMapperID, string customField, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionsExtensionMapper_Exists_ByIDCustomField";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@SubscriptionsExtensionMapperID", subscriptionsExtensionMapperID);
            cmd.Parameters.AddWithValue("@CustomField", customField);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }
        public static List<Entity.SubscriptionsExtensionMapper> SelectAll(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionsExtensionMapper_Select_All";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(client); 

                List<Entity.SubscriptionsExtensionMapper> subExtensionMapper = (List<Entity.SubscriptionsExtensionMapper>)CacheUtil.GetFromCache("SUBSCRIPTIONSEXTENSIONMAPPER", DatabaseName);

                if (subExtensionMapper == null)
                {
                    subExtensionMapper = GetList(cmd); 

                    CacheUtil.AddToCache("SUBSCRIPTIONSEXTENSIONMAPPER", subExtensionMapper, DatabaseName);
                }

                return subExtensionMapper;
            }
            else
            {
                return GetList(cmd); 
            }
        }
        public static Entity.SubscriptionsExtensionMapper SelectByID(int subscriptionsExtensionMapperID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionsExtensionMapper_Select_ID";
            cmd.Parameters.AddWithValue("@SubscriptionsExtensionMapperID", subscriptionsExtensionMapperID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Get(cmd);
        }
        public static NameValueCollection ValidationForDeleteorInActive(int subscriptionsExtensionMapperID, KMPlatform.Object.ClientConnections client)
        {
            NameValueCollection nvReturn = new NameValueCollection();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "e_SubscriptionsExtensionMapper_Validate_DeleteorInActive";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandTimeout = 0;
            cmd.Parameters.Add(new SqlParameter("@SubscriptionsExtensionMapperId", subscriptionsExtensionMapperID));
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        while (rdr.Read())
                        {
                            //nvReturn.Add(rdr["Reference"].ToString() + " : ", rdr["Reference"].ToString().ToUpper() == "FILTER EXPORT SCHEDULE" ? "<a href='../main/FilterExport.aspx?FilterScheduleId=" + rdr["ReferenceID2"].ToString() + "&FilterID=" + rdr["ReferenceID1"].ToString() + "'>" + rdr["ReferenceName"].ToString() + "</a>" : rdr["ReferenceName"].ToString());

                            if (nvReturn[rdr["Reference"].ToString() + " : "] != null)
                                nvReturn.Set(rdr["Reference"].ToString() + " : ", nvReturn.Get(rdr["Reference"].ToString() + " : ") + ", " + rdr["ReferenceName"].ToString());
                            else
                                nvReturn.Add(rdr["Reference"].ToString() + " : ", rdr["ReferenceName"].ToString());
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
            return nvReturn;
        }
        public static Entity.SubscriptionsExtensionMapper Get(SqlCommand cmd)
        {
            Entity.SubscriptionsExtensionMapper retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubscriptionsExtensionMapper();
                        DynamicBuilder<Entity.SubscriptionsExtensionMapper> builder = DynamicBuilder<Entity.SubscriptionsExtensionMapper>.CreateBuilder(rdr);
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
        public static List<Entity.SubscriptionsExtensionMapper> GetList(SqlCommand cmd)
        {
            List<Entity.SubscriptionsExtensionMapper> retList = new List<Entity.SubscriptionsExtensionMapper>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    Entity.SubscriptionsExtensionMapper retItem = new Entity.SubscriptionsExtensionMapper();
                    DynamicBuilder<Entity.SubscriptionsExtensionMapper> builder = DynamicBuilder<Entity.SubscriptionsExtensionMapper>.CreateBuilder(rdr);
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

            return retList;
        }
        public static void DeleteCache(KMPlatform.Object.ClientConnections client)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(client);

                if (CacheUtil.GetFromCache("SUBSCRIPTIONSEXTENSIONMAPPER", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("SUBSCRIPTIONSEXTENSIONMAPPER", DatabaseName);
                }
            }
        }
        public static int Save(Entity.SubscriptionsExtensionMapper x, KMPlatform.Object.ClientConnections client)
        {
            DeleteCache(client);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionsExtensionMapper_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@SubscriptionsExtensionMapperId", x.SubscriptionsExtensionMapperID);
            cmd.Parameters.AddWithValue("@StandardField", x.StandardField);
            cmd.Parameters.AddWithValue("@CustomField", x.CustomField);
            cmd.Parameters.AddWithValue("@CustomFieldDataType", x.CustomFieldDataType);
            cmd.Parameters.AddWithValue("@Active", x.Active);
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static void Delete(int SubscriptionsExtensionMapperID, KMPlatform.Object.ClientConnections client)
        {
            DeleteCache(client);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionsExtensionMapper_Delete";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@SubscriptionsExtensionMapperId", SubscriptionsExtensionMapperID));
            DataFunctions.Execute(cmd);
        }
    }
}
