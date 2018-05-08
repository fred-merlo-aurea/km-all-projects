using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class ClientGroupClientMap
    {
        public static List<Entity.ClientGroupClientMap> Select()
        {
            List<Entity.ClientGroupClientMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupClientMap_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ClientGroupClientMap> SelectForClientGroup(int clientGroupID)
        {
            List<Entity.ClientGroupClientMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupClientMap_Select_ClientGroupID";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ClientGroupClientMap> SelectForClientID(int clientID)
        {
            List<Entity.ClientGroupClientMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupClientMap_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.ClientGroupClientMap Get(SqlCommand cmd)
        {
            Entity.ClientGroupClientMap retItem = null;
            try
            {
                using (var rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ClientGroupClientMap();
                        var builder = DynamicBuilder<Entity.ClientGroupClientMap>.CreateBuilder(rdr);
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
        private static List<Entity.ClientGroupClientMap> GetList(SqlCommand cmd)
        {
            List<Entity.ClientGroupClientMap> retList = new List<Entity.ClientGroupClientMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ClientGroupClientMap retItem = new Entity.ClientGroupClientMap();
                        var builder = DynamicBuilder<Entity.ClientGroupClientMap>.CreateBuilder(rdr);
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
        public static int Save(Entity.ClientGroupClientMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupClientMap_Save";
            cmd.Parameters.Add(new SqlParameter("@ClientGroupClientMapID", x.ClientGroupClientMapID));
            cmd.Parameters.Add(new SqlParameter("@ClientGroupID", x.ClientGroupID));
            cmd.Parameters.Add(new SqlParameter("@ClientID", x.ClientID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()).ToString());
        }
    }
}
