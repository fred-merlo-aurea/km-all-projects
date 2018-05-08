using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KMPlatform.Object;

namespace FrameworkUAD.DataAccess
{
    public class Brand
    {
        public static List<Entity.Brand> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Brand> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Brand_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.Brand> SelectByUserID(int UserID, ClientConnections client)
        {
            List<Entity.Brand> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Brand_Select_UserID";
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.Brand> SelectByPubID(int PubID, ClientConnections client)
        {
            List<Entity.Brand> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Brand_Select_PubID";
            cmd.Parameters.AddWithValue("@PubID", PubID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        public static Entity.Brand Get(SqlCommand cmd)
        {
            Entity.Brand retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Brand();
                        DynamicBuilder<Entity.Brand> builder = DynamicBuilder<Entity.Brand>.CreateBuilder(rdr);
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
        public static List<Entity.Brand> GetList(SqlCommand cmd)
        {
            List<Entity.Brand> retList = new List<Entity.Brand>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.Brand retItem = new Entity.Brand();
                        DynamicBuilder<Entity.Brand> builder = DynamicBuilder<Entity.Brand>.CreateBuilder(rdr);
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
        public static int Save(Entity.Brand x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Brand_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@BrandID", x.BrandID);
            cmd.Parameters.AddWithValue("@BrandName", x.BrandName);
            cmd.Parameters.AddWithValue("@Logo", x.Logo);
            cmd.Parameters.AddWithValue("@IsBrandGroup", x.IsBrandGroup);
            cmd.Parameters.AddWithValue("@IsDeleted", x.IsDeleted);
            cmd.Parameters.Add(new SqlParameter("@CreatedUserID", x.CreatedUserID));
            cmd.Parameters.Add(new SqlParameter("@CreatedDate", x.CreatedDate));
            cmd.Parameters.Add(new SqlParameter("@UpdatedDate", (object)x.UpdatedDate ?? DBNull.Value));            
            cmd.Parameters.Add(new SqlParameter("@UpdatedUserID", (object)x.UpdatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}
