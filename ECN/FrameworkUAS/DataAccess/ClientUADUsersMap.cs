using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class ClientUADUsersMap
    {
        public static List<Entity.ClientUADUsersMap> Select()
        {
            List<Entity.ClientUADUsersMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientUADUsersMap_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ClientUADUsersMap> SelectClient(int clientID)
        {
            List<Entity.ClientUADUsersMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientUADUsersMap_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ClientUADUsersMap> SelectUser(int userID)
        {
            List<Entity.ClientUADUsersMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientUADUsersMap_Select_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.ClientUADUsersMap Select(int clientID, int userID)
        {
            Entity.ClientUADUsersMap retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientUADUsersMap_Select_ClientID_UserID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            retItem = Get(cmd);
            return retItem;
        }

        public static Entity.ClientUADUsersMap Get(SqlCommand cmd)
        {
            Entity.ClientUADUsersMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ClientUADUsersMap();
                        var builder = DynamicBuilder<Entity.ClientUADUsersMap>.CreateBuilder(rdr);
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

        public static List<Entity.ClientUADUsersMap> GetList(SqlCommand cmd)
        {
            List<Entity.ClientUADUsersMap> retList = new List<Entity.ClientUADUsersMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ClientUADUsersMap retItem = new Entity.ClientUADUsersMap();
                        var builder = DynamicBuilder<Entity.ClientUADUsersMap>.CreateBuilder(rdr);
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

        public static bool Save(Entity.ClientUADUsersMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientUADUsersMap_Save";
            cmd.Parameters.Add(new SqlParameter("@ClientID", x.ClientID));
            cmd.Parameters.Add(new SqlParameter("@UserID", x.UserID));

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.KMPlatform.ToString());
        }
        public static bool Save(int clientID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientUADUsersMap_Save";
            cmd.Parameters.Add(new SqlParameter("@ClientID", clientID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.KMPlatform.ToString());
        }
    }
}
