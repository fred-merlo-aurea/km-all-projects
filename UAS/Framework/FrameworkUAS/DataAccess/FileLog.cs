using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class FileLog
    {
        #region File Log Entity
        public static List<Entity.FileLog> SelectClient(int clientID)
        {
            List<Entity.FileLog> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileLog_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.FileLog> SelectProcessCode(string processCode)
        {
            List<Entity.FileLog> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileLog_Select_ProcessCode";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);

            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.FileLog SelectTopOneProcessCode(string processCode)
        {
            Entity.FileLog retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileLog_Select_TopOne_ProcessCode";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);

            retItem = Get(cmd);
            return retItem;
        }

        public static List<Entity.FileLog> SelectFileLog(int SourceFileID, string ProcessCode)
        {
            List<Entity.FileLog> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileLog_Select_FileLog";
            cmd.Parameters.AddWithValue("@SourceFileID", SourceFileID);
            cmd.Parameters.AddWithValue("@ProcessCode", ProcessCode);

            retItem = GetList(cmd);
            return retItem;
        }

        private static Entity.FileLog Get(SqlCommand cmd)
        {
            Entity.FileLog retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.FileLog();
                        DynamicBuilder<Entity.FileLog> builder = DynamicBuilder<Entity.FileLog>.CreateBuilder(rdr);
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
        private static List<Entity.FileLog> GetList(SqlCommand cmd)
        {
            List<Entity.FileLog> retList = new List<Entity.FileLog>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.FileLog retItem = new Entity.FileLog();
                        DynamicBuilder<Entity.FileLog> builder = DynamicBuilder<Entity.FileLog>.CreateBuilder(rdr);
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

        public static bool Save(Entity.FileLog x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FileLog_Save";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", x.SourceFileID));
            cmd.Parameters.Add(new SqlParameter("@FileStatusTypeID", x.FileStatusTypeID));
            cmd.Parameters.Add(new SqlParameter("@Message", x.Message));
            cmd.Parameters.Add(new SqlParameter("@LogDate", x.LogDate));
            cmd.Parameters.Add(new SqlParameter("@LogTime", x.LogTime));
            cmd.Parameters.Add(new SqlParameter("@ProcessCode", x.ProcessCode));

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        #endregion

        #region File Log Object
        public static List<Object.FileLog> SelectDistinctProcessCodePerSourceFile(int ClientID)
        {
            List<Object.FileLog> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_FileLog_SelectDistinctProcessCodePerSourceFile";
            cmd.Parameters.AddWithValue("@ClientID", ClientID);
            retItem = GetObjectFileLogList(cmd);

            return retItem;
        }
        private static Object.FileLog GetObjectFileLog(SqlCommand cmd)
        {
            Object.FileLog retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.FileLog();
                        DynamicBuilder<Object.FileLog> builder = DynamicBuilder<Object.FileLog>.CreateBuilder(rdr);
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
        private static List<Object.FileLog> GetObjectFileLogList(SqlCommand cmd)
        {
            List<Object.FileLog> retList = new List<Object.FileLog>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Object.FileLog retItem = new Object.FileLog();
                        DynamicBuilder<Object.FileLog> builder = DynamicBuilder<Object.FileLog>.CreateBuilder(rdr);
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

        #endregion
    }
}
