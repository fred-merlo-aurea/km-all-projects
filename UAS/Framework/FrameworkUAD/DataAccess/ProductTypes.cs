using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KMPlatform.Object;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class ProductTypes
    {
        public static bool ExistsByIDPubTypeDisplayName(int pubTypeID, string pubTypeDisplayName, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductTypes_Exists_ByIDPubTypeDisplayName";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@PubTypeID", pubTypeID);
            cmd.Parameters.AddWithValue("@PubTypeDisplayName", pubTypeDisplayName);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }

        public static bool ExistsByPubTypeID(int pubTypeID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Product_Exists_ByPubTypeID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@PubTypeID", pubTypeID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }

        public static List<Entity.ProductTypes> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductTypes_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(client);

                List<Entity.ProductTypes> pubTypes = (List<Entity.ProductTypes>)CacheUtil.GetFromCache("PUBTYPES", DatabaseName);

                if (pubTypes == null)
                {
                    pubTypes = GetList(cmd);

                    CacheUtil.AddToCache("PUBTYPES", pubTypes, DatabaseName);
                }

                return pubTypes;
            }
            else
            {
                return GetList(cmd);
            }
        }

        public static List<Entity.ProductTypes> SelectByBrand(int brandID, ClientConnections client)
        {
            List<Entity.ProductTypes> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductTypes_Select_BrandID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            retItem = GetList(cmd);
            return retItem;
        }

        public static Entity.ProductTypes SelectByID(int pubTypeID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductTypes_Select_ID";
            cmd.Parameters.AddWithValue("@PubTypeID", pubTypeID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Get(cmd);
        }

        public static void DeleteCache(KMPlatform.Object.ClientConnections client)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(client);

                if (CacheUtil.GetFromCache("PUBTYPES", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("PUBTYPES", DatabaseName);
                }
            }
        }

        public static int Save(Entity.ProductTypes x, KMPlatform.Object.ClientConnections client)
        {
            DeleteCache(client);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductTypes_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@PubTypeID", x.PubTypeID);
            cmd.Parameters.AddWithValue("@PubTypeDisplayName", x.PubTypeDisplayName);
            cmd.Parameters.AddWithValue("@ColumnReference", x.ColumnReference);
            cmd.Parameters.AddWithValue("@IsActive", x.IsActive);
            cmd.Parameters.AddWithValue("@SortOrder", x.SortOrder);
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool Delete(KMPlatform.Object.ClientConnections client, int pubTypeID)
        {
            DeleteCache(client);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductTypes_Delete";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@PubTypeID", pubTypeID);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static Entity.ProductTypes Get(SqlCommand cmd)
        {
            Entity.ProductTypes retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ProductTypes();
                        DynamicBuilder<Entity.ProductTypes> builder = DynamicBuilder<Entity.ProductTypes>.CreateBuilder(rdr);
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
        public static List<Entity.ProductTypes> GetList(SqlCommand cmd)
        {
            List<Entity.ProductTypes> retList = new List<Entity.ProductTypes>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.ProductTypes retItem = new Entity.ProductTypes();
                        DynamicBuilder<Entity.ProductTypes> builder = DynamicBuilder<Entity.ProductTypes>.CreateBuilder(rdr);
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
    }
}
