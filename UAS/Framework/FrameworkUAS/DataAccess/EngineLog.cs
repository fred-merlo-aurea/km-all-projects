using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class EngineLog
    {
        public static List<Entity.EngineLog> Select()
        {
            List<Entity.EngineLog> x = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EngineLog_Select";
            x = GetList(cmd);
            return x;
        }
        private static List<Entity.EngineLog> GetList(SqlCommand cmd)
        {
            List<Entity.EngineLog> retList = new List<Entity.EngineLog>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.EngineLog retItem = new Entity.EngineLog();
                        DynamicBuilder<Entity.EngineLog> builder = DynamicBuilder<Entity.EngineLog>.CreateBuilder(rdr);
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

        public static bool Save(Entity.EngineLog x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EngineLog_Save";
            cmd.Parameters.AddWithValue("@EngineLogId", x.EngineLogId);
            cmd.Parameters.AddWithValue("@ClientId", x.ClientId);
            cmd.Parameters.AddWithValue("@Engine", x.Engine);
            cmd.Parameters.AddWithValue("@CurrentStatus", x.CurrentStatus);
            cmd.Parameters.AddWithValue("@LastRefreshDate", x.LastRefreshDate);
            cmd.Parameters.AddWithValue("@LastRefreshTime", x.LastRefreshTime);
            cmd.Parameters.AddWithValue("@IsRunning", x.IsRunning);
            cmd.Parameters.AddWithValue("@LastRunningCheckDate", x.LastRunningCheckDate);
            cmd.Parameters.AddWithValue("@LastRunningCheckTime", x.LastRunningCheckTime);
            cmd.Parameters.AddWithValue("@DateUpdated", x.DateUpdated);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool UpdateRefresh(int engineLogId, string currentStatus="")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EngineLog_UpdateRefresh_EngineLogId";
            cmd.Parameters.AddWithValue("@EngineLogId", engineLogId);
            cmd.Parameters.AddWithValue("@CurrentStatus", currentStatus);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool UpdateRefresh(int clientId, string engine, string currentStatus = "")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EngineLog_UpdateRefresh";
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            cmd.Parameters.AddWithValue("@Engine", engine);
            cmd.Parameters.AddWithValue("@CurrentStatus", currentStatus);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        
        public static bool UpdateIsRunning(int engineLogId, bool isRunning, string currentStatus = "")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EngineLog_UpdateIsRunning_EngineLogId";
            cmd.Parameters.AddWithValue("@EngineLogId", engineLogId);
            cmd.Parameters.AddWithValue("@IsRunning", isRunning);
            cmd.Parameters.AddWithValue("@CurrentStatus", currentStatus);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool UpdateIsRunning(int clientId, string engine, bool isRunning, string currentStatus = "")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EngineLog_UpdateIsRunning";
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            cmd.Parameters.AddWithValue("@Engine", engine);
            cmd.Parameters.AddWithValue("@IsRunning", isRunning);
            cmd.Parameters.AddWithValue("@CurrentStatus", currentStatus);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        
    }
}
