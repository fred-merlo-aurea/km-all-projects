using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class SecurityGroupBrandMap
    {
        public static List<Entity.SecurityGroupBrandMap> SelectForBrand(KMPlatform.Object.ClientConnections client, int brandID)
        {
            List<Entity.SecurityGroupBrandMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_SecurityGroupBrandMap_Select_BrandID";
            cmd.Parameters.AddWithValue("@BrandID", brandID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.SecurityGroupBrandMap> SelectForSecurityGroup(KMPlatform.Object.ClientConnections client, int securityGroupID)
        {
            List<Entity.SecurityGroupBrandMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_SecurityGroupBrandMap_Select_securityGroupID";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);

            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.SecurityGroupBrandMap Get(SqlCommand cmd)
        {
            Entity.SecurityGroupBrandMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SecurityGroupBrandMap();
                        DynamicBuilder<Entity.SecurityGroupBrandMap> builder = DynamicBuilder<Entity.SecurityGroupBrandMap>.CreateBuilder(rdr);
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
        private static List<Entity.SecurityGroupBrandMap> GetList(SqlCommand cmd)
        {
            List<Entity.SecurityGroupBrandMap> retList = new List<Entity.SecurityGroupBrandMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.SecurityGroupBrandMap retItem = new Entity.SecurityGroupBrandMap();
                        DynamicBuilder<Entity.SecurityGroupBrandMap> builder = DynamicBuilder<Entity.SecurityGroupBrandMap>.CreateBuilder(rdr);
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

        public static int Save(KMPlatform.Object.ClientConnections client, Entity.SecurityGroupBrandMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.CommandText = "e_SecurityGroupBrandMap_Save";
            cmd.Parameters.Add(new SqlParameter("@SecurityGroupBrandMapID", x.SecurityGroupBrandMapID));
            cmd.Parameters.Add(new SqlParameter("@BrandID", x.BrandID));
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
