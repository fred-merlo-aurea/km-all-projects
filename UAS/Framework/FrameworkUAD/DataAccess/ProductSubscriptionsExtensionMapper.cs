using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class ProductSubscriptionsExtensionMapper
    {
        public static bool ExistsByCustomField(string customField, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscriptionsExtensionMapper_Exists_ByCustomField";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@CustomField", customField);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }
        public static bool ExistsByIDCustomField(int pubSubscriptionsExtensionMapperID, int pubID, string customField, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscriptionsExtensionMapper_Exists_ByIDCustomFieldPubID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@PubSubscriptionsExtensionMapperID", pubSubscriptionsExtensionMapperID);
            cmd.Parameters.AddWithValue("@CustomField", customField);
            cmd.Parameters.AddWithValue("@PubID", pubID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }
        public static List<Entity.ProductSubscriptionsExtensionMapper> SelectAll(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscriptionsExtensionMapper_Select_All";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static Entity.ProductSubscriptionsExtensionMapper SelectByID(int pubSubscriptionsExtensionMapperID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscriptionsExtensionMapper_Select_ID";
            cmd.Parameters.AddWithValue("@PubSubscriptionsExtensionMapperID", pubSubscriptionsExtensionMapperID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Get(cmd);
        }
        public static DataSet SelectBySearch(int PubID, string Name, string SearchCriteria, int CurrentPage, int PageSize, string SortDirection, string SortColumn, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscriptionsExtensionMapper_Select_Search";
            cmd.Parameters.Add(new SqlParameter("@PubID", PubID));
            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            cmd.Parameters.Add(new SqlParameter("@SearchCriteria", SearchCriteria));
            cmd.Parameters.Add(new SqlParameter("@CurrentPage", CurrentPage));
            cmd.Parameters.Add(new SqlParameter("@PageSize", PageSize));
            cmd.Parameters.Add(new SqlParameter("@SortDirection", SortDirection));
            cmd.Parameters.Add(new SqlParameter("@SortColumn", SortColumn));
            return KM.Common.DataFunctions.GetDataSet(cmd, DataFunctions.GetClientSqlConnection(client).ConnectionString.ToString());
        }
        public static NameValueCollection ValidationForDeleteorInActive(int pubSubscriptionsExtensionMapperID, KMPlatform.Object.ClientConnections client)
        {
            NameValueCollection nvReturn = new NameValueCollection();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "e_SubscriptionsExtensionMapper_Validate_DeleteorInActive";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandTimeout = 0;
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionsExtensionMapperId", pubSubscriptionsExtensionMapperID));
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
        public static Entity.ProductSubscriptionsExtensionMapper Get(SqlCommand cmd)
        {
            Entity.ProductSubscriptionsExtensionMapper retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ProductSubscriptionsExtensionMapper();
                        DynamicBuilder<Entity.ProductSubscriptionsExtensionMapper> builder = DynamicBuilder<Entity.ProductSubscriptionsExtensionMapper>.CreateBuilder(rdr);
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
        public static List<Entity.ProductSubscriptionsExtensionMapper> GetList(SqlCommand cmd)
        {
            List<Entity.ProductSubscriptionsExtensionMapper> retList = new List<Entity.ProductSubscriptionsExtensionMapper>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.ProductSubscriptionsExtensionMapper retItem = new Entity.ProductSubscriptionsExtensionMapper();
                        DynamicBuilder<Entity.ProductSubscriptionsExtensionMapper> builder = DynamicBuilder<Entity.ProductSubscriptionsExtensionMapper>.CreateBuilder(rdr);
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
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
        public static int Save(Entity.ProductSubscriptionsExtensionMapper x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscriptionsExtensionMapper_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@PubSubscriptionsExtensionMapperID", x.PubSubscriptionsExtensionMapperID);
            cmd.Parameters.AddWithValue("@PubID", x.PubID);
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
        public static void Delete(int pubSubscriptionsExtensionMapperID, int pubID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductSubscriptionsExtensionMapper_Delete";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@PubSubscriptionsExtensionMapperId", pubSubscriptionsExtensionMapperID));
            cmd.Parameters.AddWithValue("@PubID", pubID);
            DataFunctions.Execute(cmd);
        }
    }
}
