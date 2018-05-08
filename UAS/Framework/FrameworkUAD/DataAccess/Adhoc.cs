using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class Adhoc
    {
        public static List<Entity.Adhoc> SelectAll(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Adhoc> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Adhoc_Select_All";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Adhoc> SelectCategoryID(int categoryID, int brandID, int pubID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Adhoc> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Adhoc_Select_CategoryID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@CategoryID", categoryID);
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            cmd.Parameters.AddWithValue("@PubID", pubID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Adhoc> GetByCategoryID(int categoryID, int brandID, int pubID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Adhoc> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_Adhoc_Select_CategoryID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@CategoryID", categoryID);
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            cmd.Parameters.AddWithValue("@PubID", pubID);
            retItem = GetList(cmd);
            return retItem;
        }

        public static Entity.Adhoc Get(SqlCommand cmd)
        {
            Entity.Adhoc retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Adhoc();
                        DynamicBuilder<Entity.Adhoc> builder = DynamicBuilder<Entity.Adhoc>.CreateBuilder(rdr);
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
        public static List<Entity.Adhoc> GetList(SqlCommand cmd)
        {
            List<Entity.Adhoc> retList = new List<Entity.Adhoc>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.Adhoc retItem = new Entity.Adhoc();
                        DynamicBuilder<Entity.Adhoc> builder = DynamicBuilder<Entity.Adhoc>.CreateBuilder(rdr);
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
        public static int Save(Entity.Adhoc x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Adhoc_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@AdhocID", x.AdhocID);
            cmd.Parameters.AddWithValue("@AdhocName", (object)x.AdhocName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CategoryID", (object)x.CategoryID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SortOrder", (object)x.SortOrder ?? DBNull.Value);
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static bool Delete(int categoryID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Adhoc_Delete_CategoryID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@CategoryID", categoryID);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool Delete_AdHoc(int adhocID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Adhoc_Delete";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@AdhocID", adhocID);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}
