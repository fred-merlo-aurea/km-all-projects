using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class ClientGroup
    {
        public static List<Entity.ClientGroup> Select()
        {
            List<Entity.ClientGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroup_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ClientGroup> SelectForAMS()
        {
            List<Entity.ClientGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroup_SelectForAMS";

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ClientGroup> SelectClient(int clientID)
        {
            List<Entity.ClientGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroup_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ClientGroup> SelectForUser(int userID)
        {
            List<Entity.ClientGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroup_Select_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ClientGroup> SelectForUserAuthorization(int userID)
        {
            List<Entity.ClientGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroup_SelectForUserAuthorization_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.ClientGroup Select(int clientGroupID)
        {
            Entity.ClientGroup retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroup_Select_ClientGroupID";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);
            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.ClientGroup Get(SqlCommand cmd)
        {
            Entity.ClientGroup retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ClientGroup();
                        var builder = DynamicBuilder<Entity.ClientGroup>.CreateBuilder(rdr);
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
        public static List<Entity.ClientGroup> GetList(SqlCommand cmd)
        {
            List<Entity.ClientGroup> retList = new List<Entity.ClientGroup>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ClientGroup retItem = new Entity.ClientGroup();
                        var builder = DynamicBuilder<Entity.ClientGroup>.CreateBuilder(rdr);
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
        public static int Save(Entity.ClientGroup x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroup_Save";
            cmd.Parameters.Add(new SqlParameter("@ClientGroupID", x.ClientGroupID));
            cmd.Parameters.Add(new SqlParameter("@ClientGroupName", x.ClientGroupName));
            cmd.Parameters.Add(new SqlParameter("@ClientGroupDescription", x.ClientGroupDescription));
            cmd.Parameters.Add(new SqlParameter("@Color", x.Color));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }

        public static List<Entity.ClientGroup> SelectForAtLeastCustomerAdmin(int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroup_Select_User_CustAdmin";
            cmd.Parameters.AddWithValue("@UserID", userID);

            return GetList(cmd);
        }
    }
}
