using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class UserClientSecurityGroupMap
    {
        public static List<Entity.UserClientSecurityGroupMap> Select()
        {
            List<Entity.UserClientSecurityGroupMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserClientSecurityGroupMap_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.UserClientSecurityGroupMap> SelectForUser(int userID)
        {
            List<Entity.UserClientSecurityGroupMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserClientSecurityGroupMap_Select_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.UserClientSecurityGroupMap> SelectForUserAuthorization(int userID)
        {
            List<Entity.UserClientSecurityGroupMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserClientSecurityGroupMap_SelectForUserAuthorization_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.UserClientSecurityGroupMap> SelectForClient(int clientID)
        {
            List<Entity.UserClientSecurityGroupMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserClientSecurityGroupMap_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.UserClientSecurityGroupMap> SelectForSecurityGroup(int securityGroupID)
        {
            List<Entity.UserClientSecurityGroupMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserClientSecurityGroupMap_Select_SecurityGroupID";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);

            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.UserClientSecurityGroupMap Get(SqlCommand cmd)
        {
            Entity.UserClientSecurityGroupMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.UserClientSecurityGroupMap();
                        var builder = DynamicBuilder<Entity.UserClientSecurityGroupMap>.CreateBuilder(rdr);
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
        private static List<Entity.UserClientSecurityGroupMap> GetList(SqlCommand cmd)
        {
            List<Entity.UserClientSecurityGroupMap> retList = new List<Entity.UserClientSecurityGroupMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.UserClientSecurityGroupMap retItem = new Entity.UserClientSecurityGroupMap();
                        var builder = DynamicBuilder<Entity.UserClientSecurityGroupMap>.CreateBuilder(rdr);
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

        public static int Save(Entity.UserClientSecurityGroupMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserClientSecurityGroupMap_Save";
            cmd.Parameters.Add(new SqlParameter("@UserClientSecurityGroupMapID", x.UserClientSecurityGroupMapID));
            cmd.Parameters.Add(new SqlParameter("@UserID", x.UserID));
            cmd.Parameters.Add(new SqlParameter("@ClientID", x.ClientID));
            cmd.Parameters.Add(new SqlParameter("@SecurityGroupID", x.SecurityGroupID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@InactiveReason", x.InactiveReason));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }

        public static void Delete(int UserClientSecurityGroupMapID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserClientSecurityGroupMap_Delete";
            cmd.Parameters.AddWithValue("@UserClientSecurityGroupMapID", UserClientSecurityGroupMapID);

            KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.KMPlatform.ToString());
        }
    }
}
