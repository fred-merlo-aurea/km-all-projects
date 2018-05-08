using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class FileProcessingStat
    {
        public static bool NightlyInsert(DateTime processDate, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileProcessingStat_NightlyInsert";
            cmd.Parameters.AddWithValue("@ProcessDate", processDate);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static Entity.FileProcessingStat Select(DateTime processDate, KMPlatform.Object.ClientConnections client)
        {
            Entity.FileProcessingStat retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileProcessingStat_Select_ProcessDate";
            cmd.Parameters.AddWithValue("@ProcessDate", processDate);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = Get(cmd);
            return retItem;
        }
        public static List<Entity.FileProcessingStat> SelectDateRange(DateTime startDate, DateTime endDate, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.FileProcessingStat> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileProcessingStat_Select_DateRange";
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.FileProcessingStat Get(SqlCommand cmd)
        {
            Entity.FileProcessingStat retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.FileProcessingStat();
                        DynamicBuilder<Entity.FileProcessingStat> builder = DynamicBuilder<Entity.FileProcessingStat>.CreateBuilder(rdr);
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
        public static List<Entity.FileProcessingStat> GetList(SqlCommand cmd)
        {
            List<Entity.FileProcessingStat> retList = new List<Entity.FileProcessingStat>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.FileProcessingStat retItem = new Entity.FileProcessingStat();
                        DynamicBuilder<Entity.FileProcessingStat> builder = DynamicBuilder<Entity.FileProcessingStat>.CreateBuilder(rdr);
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


        public static FrameworkUAS.Entity.FileProcessingStat GetFileProcessingStats(DateTime processDate, int clientId, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAS.Entity.FileProcessingStat retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_GetFileProcessingStats";
            cmd.Parameters.Add(new SqlParameter("@ProcessDate", processDate));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = Get(cmd, clientId);
            return retItem;
        }

        public static FrameworkUAS.Entity.FileProcessingStat Get(SqlCommand cmd, int clientId)
        {
            FrameworkUAS.Entity.FileProcessingStat retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new FrameworkUAS.Entity.FileProcessingStat();
                        DynamicBuilder<FrameworkUAS.Entity.FileProcessingStat> builder = DynamicBuilder<FrameworkUAS.Entity.FileProcessingStat>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                                retItem.ClientId = clientId;
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
        public static List<FrameworkUAS.Entity.FileProcessingStat> GetList(SqlCommand cmd, int clientId)
        {
            List<FrameworkUAS.Entity.FileProcessingStat> retList = new List<FrameworkUAS.Entity.FileProcessingStat>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        FrameworkUAS.Entity.FileProcessingStat retItem = new FrameworkUAS.Entity.FileProcessingStat();
                        DynamicBuilder<FrameworkUAS.Entity.FileProcessingStat> builder = DynamicBuilder<FrameworkUAS.Entity.FileProcessingStat>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                retItem.ClientId = clientId;
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
    }
}
