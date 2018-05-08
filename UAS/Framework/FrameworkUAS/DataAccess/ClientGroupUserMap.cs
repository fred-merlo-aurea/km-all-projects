using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class ClientGroupUserMap
    {
        public static List<Entity.ClientGroupUserMap> Select()
        {
            List<Entity.ClientGroupUserMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupUserMap_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ClientGroupUserMap> SelectForClientGroup(int clientGroupID)
        {
            List<Entity.ClientGroupUserMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupUserMap_Select_ClientGroupID";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ClientGroupUserMap> SelectForUser(int userID)
        {
            List<Entity.ClientGroupUserMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupUserMap_Select_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.ClientGroupUserMap Get(SqlCommand cmd)
        {
            Entity.ClientGroupUserMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ClientGroupUserMap();
                        DynamicBuilder<Entity.ClientGroupUserMap> builder = DynamicBuilder<Entity.ClientGroupUserMap>.CreateBuilder(rdr);
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
        private static List<Entity.ClientGroupUserMap> GetList(SqlCommand cmd)
        {
            List<Entity.ClientGroupUserMap> retList = new List<Entity.ClientGroupUserMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ClientGroupUserMap retItem = new Entity.ClientGroupUserMap();
                        DynamicBuilder<Entity.ClientGroupUserMap> builder = DynamicBuilder<Entity.ClientGroupUserMap>.CreateBuilder(rdr);
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
        public static int Save(Entity.ClientGroupUserMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupUserMap_Save";
            cmd.Parameters.Add(new SqlParameter("@ClientGroupUserMapID", x.ClientGroupUserMapID));
            cmd.Parameters.Add(new SqlParameter("@ClientGroupID", x.ClientGroupID));
            cmd.Parameters.Add(new SqlParameter("@UserID", x.UserID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}
