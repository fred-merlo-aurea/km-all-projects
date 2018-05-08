using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class ProductGroup
    {
        public static List<Entity.ProductGroup> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ProductGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductGroups_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        public static int Save(Entity.ProductGroup x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductGroups_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@PubID", x.PubID);
            cmd.Parameters.AddWithValue("@GroupID", x.GroupID);
            cmd.Parameters.AddWithValue("@Name", x.Name);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool Delete(KMPlatform.Object.ClientConnections client, int pubID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductGroups_Delete";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@PubID", pubID);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static Entity.ProductGroup Get(SqlCommand cmd)
        {
            Entity.ProductGroup retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ProductGroup();
                        DynamicBuilder<Entity.ProductGroup> builder = DynamicBuilder<Entity.ProductGroup>.CreateBuilder(rdr);
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
        public static List<Entity.ProductGroup> GetList(SqlCommand cmd)
        {
            List<Entity.ProductGroup> retList = new List<Entity.ProductGroup>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.ProductGroup retItem = new Entity.ProductGroup();
                        DynamicBuilder<Entity.ProductGroup> builder = DynamicBuilder<Entity.ProductGroup>.CreateBuilder(rdr);
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
