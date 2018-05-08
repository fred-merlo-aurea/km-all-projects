using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class ClientGroupSecurityGroupMap
    {
        public static List<Entity.ClientGroupSecurityGroupMap> Select()
        {
            List<Entity.ClientGroupSecurityGroupMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupSecurityGroupMap_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ClientGroupSecurityGroupMap> SelectForClientGroup(int clientGroupID)
        {
            List<Entity.ClientGroupSecurityGroupMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupSecurityGroupMap_Select_ClientGroupID";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ClientGroupSecurityGroupMap> SelectForSecurityGroup(int securityGroupID)
        {
            List<Entity.ClientGroupSecurityGroupMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupSecurityGroupMap_Select_SecurityGroupID";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.ClientGroupSecurityGroupMap Get(SqlCommand cmd)
        {
            Entity.ClientGroupSecurityGroupMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ClientGroupSecurityGroupMap();
                        DynamicBuilder<Entity.ClientGroupSecurityGroupMap> builder = DynamicBuilder<Entity.ClientGroupSecurityGroupMap>.CreateBuilder(rdr);
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
        private static List<Entity.ClientGroupSecurityGroupMap> GetList(SqlCommand cmd)
        {
            List<Entity.ClientGroupSecurityGroupMap> retList = new List<Entity.ClientGroupSecurityGroupMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ClientGroupSecurityGroupMap retItem = new Entity.ClientGroupSecurityGroupMap();
                        DynamicBuilder<Entity.ClientGroupSecurityGroupMap> builder = DynamicBuilder<Entity.ClientGroupSecurityGroupMap>.CreateBuilder(rdr);
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
        public static int Save(Entity.ClientGroupSecurityGroupMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupSecurityGroupMap_Save";
            cmd.Parameters.Add(new SqlParameter("@ClientGroupSecurityGroupMapID", x.ClientGroupSecurityGroupMapID));
            cmd.Parameters.Add(new SqlParameter("@ClientGroupID", x.ClientGroupID));
            cmd.Parameters.Add(new SqlParameter("@SecurityGroupID", x.SecurityGroupID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()).ToString());
        }
    }
}
