using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class FilterSegmentation
    {
        public static List<Entity.FilterSegmentation> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterSegmentation_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(client);
                List<Entity.FilterSegmentation> retItem = (List<Entity.FilterSegmentation>)CacheUtil.GetFromCache("FilterSegmentation", DatabaseName);
                if (retItem == null)
                {
                    retItem = GetList(cmd);
                    CacheUtil.AddToCache("FilterSegmentation", retItem, DatabaseName);
                }

                return retItem;
            }
            else
            {
                return GetList(cmd);
            }
        }

        public static Entity.FilterSegmentation SelectByID(int filterSegmentationID, KMPlatform.Object.ClientConnections client)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                return Select(client).Find(x => x.FilterSegmentationID == filterSegmentationID);
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_FilterSegmentation_Select_FilterSegmentationID";
                cmd.Connection = DataFunctions.GetClientSqlConnection(client);
                cmd.Parameters.Add(new SqlParameter("@FilterSegmentationID", filterSegmentationID));
                return Get(cmd);
            }
        }

        public static DataTable SelectViewTypeUserID(int @UserID, int @PubID, int @BrandID, string @ViewType, bool @IsAdmin, int FilterCategoryID, string SearchText, string SearchCriteria, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterSegmentation_Select_ViewType_UserID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@UserID", @UserID));
            cmd.Parameters.Add(new SqlParameter("@PubID", @PubID));
            cmd.Parameters.Add(new SqlParameter("@BrandID", @BrandID));
            cmd.Parameters.Add(new SqlParameter("@ViewType", @ViewType));
            cmd.Parameters.Add(new SqlParameter("@IsAdmin", @IsAdmin));
            cmd.Parameters.Add(new SqlParameter("@FilterCategoryID", @FilterCategoryID));
            cmd.Parameters.Add(new SqlParameter("@SearchText", @SearchText));
            cmd.Parameters.Add(new SqlParameter("@SearchCriteria", @SearchCriteria));
            return KM.Common.DataFunctions.GetDataTableViaAdapter(cmd);
        }

        public static bool ExistsByIDName(int filterSegmentationID, string name, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterSegmentation_Exists_ByIDName";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@FilterSegmentationID", filterSegmentationID));
            cmd.Parameters.Add(new SqlParameter("@FilterSegmentationName", name));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }

        public static Entity.FilterSegmentation Get(SqlCommand cmd)
        {
            Entity.FilterSegmentation retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.FilterSegmentation();
                        DynamicBuilder<Entity.FilterSegmentation> builder = DynamicBuilder<Entity.FilterSegmentation>.CreateBuilder(rdr);
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

        public static List<Entity.FilterSegmentation> GetList(SqlCommand cmd)
        {
            List<Entity.FilterSegmentation> retList = new List<Entity.FilterSegmentation>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.FilterSegmentation retItem = new Entity.FilterSegmentation();
                        DynamicBuilder<Entity.FilterSegmentation> builder = DynamicBuilder<Entity.FilterSegmentation>.CreateBuilder(rdr);
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

        public static int Save(Entity.FilterSegmentation x, KMPlatform.Object.ClientConnections client)
        {
            DeleteCache(client);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterSegmentation_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@FilterSegmentationID", x.FilterSegmentationID);
            cmd.Parameters.AddWithValue("@FilterSegmentationName", x.FilterSegmentationName);
            cmd.Parameters.AddWithValue("@Notes", x.Notes);
            cmd.Parameters.AddWithValue("@FilterID", x.FilterID);
            cmd.Parameters.AddWithValue("@IsDeleted", x.IsDeleted);
            cmd.Parameters.AddWithValue("@CreatedUserID", (object)x.CreatedUserID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedDate", (object)x.CreatedDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedDate", (object)x.UpdatedDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdatedUserID", (object)x.UpdatedUserID ?? DBNull.Value);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool Delete(int filterSegmentationID, int userID, KMPlatform.Object.ClientConnections client)
        {
            DeleteCache(client);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterSegmentation_Delete";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@FilterSegmentationID", filterSegmentationID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        private static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("FilterSegmentation", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("FilterSegmentation", DatabaseName);
                }
            }
        }
    }
}
