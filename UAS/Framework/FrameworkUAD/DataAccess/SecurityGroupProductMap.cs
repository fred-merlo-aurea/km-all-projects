using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class SecurityGroupProductMap
    {
        public static List<Entity.SecurityGroupProductMap> SelectForProduct(KMPlatform.Object.ClientConnections client, int productID)
        {
            List<Entity.SecurityGroupProductMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_SecurityGroupProductMap_Select_ProductID";
            cmd.Parameters.AddWithValue("@ProductID", productID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.SecurityGroupProductMap> SelectForSecurityGroup(KMPlatform.Object.ClientConnections client, int securityGroupID)
        {
            List<Entity.SecurityGroupProductMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_SecurityGroupProductMap_Select_securityGroupID";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);

            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.SecurityGroupProductMap Get(SqlCommand cmd)
        {
            Entity.SecurityGroupProductMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SecurityGroupProductMap();
                        DynamicBuilder<Entity.SecurityGroupProductMap> builder = DynamicBuilder<Entity.SecurityGroupProductMap>.CreateBuilder(rdr);
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
        private static List<Entity.SecurityGroupProductMap> GetList(SqlCommand cmd)
        {
            List<Entity.SecurityGroupProductMap> retList = new List<Entity.SecurityGroupProductMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.SecurityGroupProductMap retItem = new Entity.SecurityGroupProductMap();
                        DynamicBuilder<Entity.SecurityGroupProductMap> builder = DynamicBuilder<Entity.SecurityGroupProductMap>.CreateBuilder(rdr);
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

        public static int Save(KMPlatform.Object.ClientConnections client, Entity.SecurityGroupProductMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_SecurityGroupProductMap_Save";
            cmd.Parameters.Add(new SqlParameter("@SecurityGroupProductMapID", x.SecurityGroupProductMapID));
            cmd.Parameters.Add(new SqlParameter("@ProductID", x.ProductID));
            cmd.Parameters.Add(new SqlParameter("@SecurityGroupID", x.SecurityGroupID));
            cmd.Parameters.Add(new SqlParameter("@HasAccess", x.HasAccess));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.UAS.ToString()));
        }
    }
}
