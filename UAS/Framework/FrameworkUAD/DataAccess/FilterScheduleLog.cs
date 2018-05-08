using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class FilterScheduleLog
    {
        public static List<Entity.FilterScheduleLog> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.FilterScheduleLog> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterScheduleLog_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.FilterScheduleLog Get(SqlCommand cmd)
        {
            Entity.FilterScheduleLog retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.FilterScheduleLog();
                        DynamicBuilder<Entity.FilterScheduleLog> builder = DynamicBuilder<Entity.FilterScheduleLog>.CreateBuilder(rdr);
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
        private static List<Entity.FilterScheduleLog> GetList(SqlCommand cmd)
        {
            List<Entity.FilterScheduleLog> retList = new List<Entity.FilterScheduleLog>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.FilterScheduleLog retItem = new Entity.FilterScheduleLog();
                        DynamicBuilder<Entity.FilterScheduleLog> builder = DynamicBuilder<Entity.FilterScheduleLog>.CreateBuilder(rdr);
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
        public static int Save(KMPlatform.Object.ClientConnections client, Entity.FilterScheduleLog x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FilterScheduleLog_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@FilterScheduleID", x.FilterScheduleID);
            cmd.Parameters.AddWithValue("@StartDate", x.StartDate);
            cmd.Parameters.AddWithValue("@StartTime", x.StartTime);
            cmd.Parameters.AddWithValue("@FileName", x.FileName);
            cmd.Parameters.AddWithValue("@DownloadCount", x.DownloadCount);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

    }
}
