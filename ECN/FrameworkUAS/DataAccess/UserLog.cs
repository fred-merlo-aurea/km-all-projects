using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class UserLog
    {
        public static List<Entity.UserLog> Select()
        {
            List<Entity.UserLog> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserLog_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.UserLog Select(int userLogID)
        {
            Entity.UserLog retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserLog_Select_UserLogID";
            cmd.Parameters.Add(new SqlParameter("@UserLogID", userLogID));

            retItem = Get(cmd);
            return retItem;
        }

        private static Entity.UserLog Get(SqlCommand cmd)
        {
            Entity.UserLog retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.UserLog();
                        var builder = DynamicBuilder<Entity.UserLog>.CreateBuilder(rdr);
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
        private static List<Entity.UserLog> GetList(SqlCommand cmd)
        {
            List<Entity.UserLog> retList = new List<Entity.UserLog>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.UserLog retItem = new Entity.UserLog();
                        var builder = DynamicBuilder<Entity.UserLog>.CreateBuilder(rdr);
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

        private static List<KMPlatform.Entity.UserLog> GetListUserLogID(SqlCommand cmd)
        {
            List<KMPlatform.Entity.UserLog> retList = new List<KMPlatform.Entity.UserLog>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        KMPlatform.Entity.UserLog retItem = new KMPlatform.Entity.UserLog();
                        var builder = DynamicBuilder<KMPlatform.Entity.UserLog>.CreateBuilder(rdr);
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

        public static int Save(Entity.UserLog x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserLog_Save";
            cmd.Parameters.Add(new SqlParameter("@ApplicationID", x.ApplicationID));
            cmd.Parameters.Add(new SqlParameter("@UserLogTypeID", x.UserLogTypeID));
            cmd.Parameters.Add(new SqlParameter("@UserID", x.UserID));
            cmd.Parameters.Add(new SqlParameter("@Object", x.Object));
            cmd.Parameters.Add(new SqlParameter("@FromObjectValues", x.FromObjectValues));
            cmd.Parameters.Add(new SqlParameter("@ToObjectValues", x.ToObjectValues));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@ClientID", x.ClientID));
            cmd.Parameters.Add(new SqlParameter("@GroupTransactionCode", x.GroupTransactionCode));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }

        public static List<KMPlatform.Entity.UserLog> SaveBulkInsert(string xml, KMPlatform.Entity.Client client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserLog_Bulk_Save";
            cmd.Parameters.Add(new SqlParameter("@xml", xml));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetListUserLogID(cmd);
        }
    }
}
