using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;
using KM.Common.Functions;
using StringFunctions = Core_AMS.Utilities.StringFunctions;
using KMCommonDataFunctions = KM.Common.DataFunctions;

namespace FrameworkUAS.DataAccess
{
    public class DataFunctions
    {
        #region DataTables

        public static DataTable GetDataTableViaAdapter(SqlCommand cmd, string connectionStringName)
        {
            cmd = KMCommonDataFunctions.MinDateCheck(cmd);
            cmd = KMCommonDataFunctions.MinTimeCheck(cmd);
            cmd.Connection = KMCommonDataFunctions.GetSqlConnection(connectionStringName);
            cmd.CommandTimeout = 0;
            //command.Connection.Open();

            SqlDataAdapter myAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("Table Data");
            myAdapter.Fill(dt);
            cmd.Connection.Close();
            cmd.Connection.Dispose();
            myAdapter.Dispose();
            return dt;
        }

        public static DataTable GetDataTable(string commandText, string connectionStringName)
        {
            return KMCommonDataFunctions.GetDataTable(commandText, KMCommonDataFunctions.GetSqlConnection(connectionStringName));
        }

        #endregion
        #region Executes

        public static object ExecuteScalar(string sql, string connectionStringName)
        {
            object obj = null;
            SqlCommand cmd = new SqlCommand(sql);
            try
            {
                using (cmd)
                {
                    cmd.CommandTimeout = 0;
                    cmd.Connection = KMCommonDataFunctions.GetSqlConnection(connectionStringName);
                    cmd.Connection.Open();
                    obj = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
            return obj;
        }

        /// <summary>
        /// must have SqlCommand.Connection already defined and attached to SqlCommand object
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var result = KMCommonDataFunctions.ExecuteScalar(command, false);
            return result;
        }

        #endregion

        public static T Get<T>(SqlCommand cmd) where T : new()
        {
            //var retItem = (T) Activator.CreateInstance(typeof(T));
            T retItem = new T();
            try
            {
                using (SqlDataReader rdr = KMCommonDataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        DynamicBuilder<T> builder = DynamicBuilder<T>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch
            {
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
     
        public static List<T> GetList<T>(SqlCommand cmd)
        {
            var list = KMCommonDataFunctions.GetList<T>(cmd, ConnectionString.UAS.ToString());
            return list;
        }
    }

    public static class DynamicHelper
    {
        #region Get / GetList
        public static List<T> GetList<T>(this SqlCommand cmd, ConnectionString connString)
        {
            List<T> retList = new List<T>();
            try
            {
                using (SqlDataReader rdr = KMCommonDataFunctions.ExecuteReader(cmd, connString.ToString()))
                {
                    if (rdr != null)
                    {
                        T retItem = default(T);
                        DynamicBuilder<T> builder = DynamicBuilder<T>.CreateBuilder(rdr);
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

        public static List<T> GetList<T>(this SqlCommand cmd, string connString)
        {
            return SqlCommandExtensions.GetList<T>(cmd, connString);
        }

        public static List<T> GetList<T>(this SqlCommand cmd, KMPlatform.Entity.Client client)
        {
            List<T> retList = new List<T>();
            try
            {
                cmd.Connection = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);
                using (SqlDataReader rdr = KMCommonDataFunctions.ExecuteReaderNullIfEmpty(cmd))
                {
                    if (rdr != null)
                    {
                        T retItem = default(T);
                        DynamicBuilder<T> builder = DynamicBuilder<T>.CreateBuilder(rdr);
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

        public static List<T> GetList<T>(this SqlCommand cmd, KMPlatform.Object.ClientConnections client)
        {
            return SqlCommandExtensions.GetList<T>(cmd, client);
        }

        public static T Get<T>(SqlCommand cmd, ConnectionString connString) where T : new()
        {
            T retItem = new T();

            using (SqlDataReader rdr = KMCommonDataFunctions.ExecuteReader(cmd, connString.ToString()))
            {
                if (rdr != null)
                {
                    DynamicBuilder<T> builder = DynamicBuilder<T>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();

            return retItem;
        }
        public static T Get<T>(SqlCommand cmd, string connString) where T : new()
        {
            T retItem = new T();

            using (SqlDataReader rdr = KMCommonDataFunctions.ExecuteReader(cmd, connString))
            {
                if (rdr != null)
                {
                    DynamicBuilder<T> builder = DynamicBuilder<T>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();

            return retItem;
        }
        public static T Get<T>(SqlCommand cmd, KMPlatform.Entity.Client client) where T : new()
        {
            T retItem = new T();

            cmd.Connection = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);
            using (SqlDataReader rdr = KMCommonDataFunctions.ExecuteReaderNullIfEmpty(cmd))
            {
                if (rdr != null)
                {
                    DynamicBuilder<T> builder = DynamicBuilder<T>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();

            return retItem;
        }
        public static T Get<T>(SqlCommand cmd, KMPlatform.Object.ClientConnections client) where T : new()
        {
            T retItem = new T();

            cmd.Connection = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);
            using (SqlDataReader rdr = KMCommonDataFunctions.ExecuteReaderNullIfEmpty(cmd))
            {
                if (rdr != null)
                {
                    DynamicBuilder<T> builder = DynamicBuilder<T>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();

            return retItem;
        }
        #endregion
    }
}
