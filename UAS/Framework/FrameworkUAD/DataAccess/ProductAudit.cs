using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class ProductAudit
    {
        public static List<Entity.ProductAudit> Select(int productId, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ProductAudit> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductAudit_Select_ProductId";
            cmd.Parameters.AddWithValue("@ProductId", productId);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.ProductAudit Get(SqlCommand cmd)
        {
            Entity.ProductAudit retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ProductAudit();
                        DynamicBuilder<Entity.ProductAudit> builder = DynamicBuilder<Entity.ProductAudit>.CreateBuilder(rdr);
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
        public static List<Entity.ProductAudit> GetList(SqlCommand cmd)
        {
            List<Entity.ProductAudit> retList = new List<Entity.ProductAudit>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.ProductAudit retItem = new Entity.ProductAudit();
                        DynamicBuilder<Entity.ProductAudit> builder = DynamicBuilder<Entity.ProductAudit>.CreateBuilder(rdr);
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
        public static bool Save(Entity.ProductAudit x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProductAudit_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ProductAuditId", x.ProductAuditId);
            cmd.Parameters.AddWithValue("@ProductId", x.ProductId);
            cmd.Parameters.AddWithValue("@AuditField", x.AuditField);
            cmd.Parameters.AddWithValue("@FieldMappingTypeId", x.FieldMappingTypeId);
            cmd.Parameters.AddWithValue("@ResponseGroupID", x.ResponseGroupID);
            cmd.Parameters.AddWithValue("@SubscriptionsExtensionMapperID", x.SubscriptionsExtensionMapperID);
            cmd.Parameters.AddWithValue("@IsActive", x.IsActive);
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedByUserID", x.CreatedByUserID);
            cmd.Parameters.AddWithValue("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}
