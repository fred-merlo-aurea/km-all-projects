using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;

using KM.Common.Functions;
using KM.Common.Data;

namespace KM.Common
{
    public class DataFunctions
    {
        public static readonly string DbNameMaster = "master";
        public static readonly string XmlHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
        public static readonly string XmlNsFrameworkUasEntity = 
            " xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/FrameworkUAS.Entity\"";
        public static readonly string XmlNsKMPlatformEntity =
            " xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/KMPlatform.Entity\"";
        public static readonly string TableData = "Table Data";
        public static readonly string AppSettingKmCommon = "KMCommon";
        public static readonly string AppSettingIsDemo = "IsDemo";
        public static readonly string AppSettingIsNetworkDeployed = "IsNetworkDeployed";
        public static readonly string IpPart241 = "241";
        public static readonly string IpPart251 = "251";
        public static readonly string IpPart198 = "198";
        public static readonly string IpPart21617 = "216.17";
        public static readonly string IpPart1010 = "10.10";

        public static string CleanString(string dirtyString)
        {
            var cleanString = StringFunctions.Replace(dirtyString, "'", "''");
            cleanString = StringFunctions.Replace(cleanString, "’", "''");
            cleanString = StringFunctions.Replace(cleanString, "–", "-");
            cleanString = StringFunctions.Replace(cleanString, "“", "\"");
            cleanString = StringFunctions.Replace(cleanString, "”", "\"");
            cleanString = StringFunctions.Replace(cleanString, "…", "...");
            return cleanString;
        }

        public static DataSet GetDataSet(SqlCommand command, string connectionStringName)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            try
            {
                command.Connection = GetSqlConnection(connectionStringName);
                ConfigureAndOpenCommand(command);

                var adapter = new SqlDataAdapter(command);
                var dataSet = new DataSet();
                adapter.Fill(dataSet);
                return dataSet;
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public static DataTable GetDataTable(string query, string connectionStringName)
        {
            var connection = GetSqlConnection(connectionStringName);
            var table = GetDataTable(query, connection);
            return table;
        }

        public static DataTable GetDataTable(string query, SqlConnection connection)
        {
            using (var command = new SqlCommand(query, connection))
            {
                try
                {
                    ConfigureAndOpenCommand(command);

                    var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    var table = new DataTable();
                    table.Load(reader);

                    return table;
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        public static DataTable GetDataTable(SqlCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            try
            {
                ConfigureAndOpenCommand(command);

                var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                var table = new DataTable();
                table.Load(reader);

                return table;
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public static DataTable GetDataTable(SqlCommand command, string connectionStringName)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            try
            {
                command.Connection = GetSqlConnection(connectionStringName);
                var table = GetDataTable(command);
                return table;
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public static DataTable GetDataTableViaAdapter(SqlCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            ConfigureCommand(command);

            var adapter = new SqlDataAdapter(command);
            var table = new DataTable(TableData);
            adapter.Fill(table);

            return table;
        }

        public static DataTable GetDataTableViaAdapter(SqlCommand command, string connectionStringName)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.Connection = GetSqlConnection(connectionStringName);

            var table = GetDataTableViaAdapter(command);
            return table;
        }

        public static int Execute(string query)
        {
            using (var command = new SqlCommand(query))
            {
                var result = Execute(command);
                return result;
            }
        }

        public static int Execute(SqlCommand command)
        {
            try
            {
                command.Connection = GetSqlConnection();

                ConfigureAndOpenCommand(command);

                var result = command.ExecuteNonQuery();
                return result;
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public static int Execute(string sqlQuery, SqlConnection connection)
        {
            if (String.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentException(String.Format("{0} couldn't be empty", nameof(sqlQuery)));
            }

            using (var command = new SqlCommand(sqlQuery, connection))
            {
                return Execute(command, connection);
            }
        }

        public static int Execute(string sqlQuery, string connectionStringName)
        {
            var connection = GetSqlConnection(connectionStringName);
            var result = Execute(sqlQuery, connection);
            return result;
        }

        public static int Execute(SqlCommand cmd, string connectionStringName)
        {
            using (var connection = GetSqlConnection(connectionStringName))
            {
                cmd.Connection = connection;
                var result = Execute(cmd, connection);
                return result;
            }
        }

        public static int Execute(SqlCommand command, SqlConnection connection)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            command.Connection = connection;
            try
            {
                MinDateCheck(command);
                MinTimeCheck(command);
                command.CommandTimeout = 0;
                connection.Open();

                var success = command.ExecuteNonQuery();
                return success;
            }
            finally
            {
                connection.Close();
            }
        }

        public static object ExecuteScalar(SqlCommand command, bool setDefaultConnection = true)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (setDefaultConnection)
            {
                command.Connection = GetSqlConnection();
            }

            ConfigureAndOpenCommand(command);

            var obj = command.ExecuteScalar();
            command.Connection.Close();
            return obj;
        }

        public static object ExecuteScalar(string sqlQuery, SqlConnection connection)
        {
            if (String.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentException(String.Format("{0} couldn't be empty", nameof(sqlQuery)));
            }

            var cmd = new SqlCommand(sqlQuery);
            return ExecuteScalar(cmd, connection);
        }

        public static object ExecuteScalar(SqlCommand command, string connectionStringName)
        {
            using (var connection = GetSqlConnection(connectionStringName))
            {
                var result = ExecuteScalar(command, connection);
                return result;
            }
        }

        public static object ExecuteScalar(SqlCommand command, SqlConnection connection)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            command.Connection = connection;
            try
            {
                ConfigureAndOpenCommand(command);
                var scalar = command.ExecuteScalar();
                return scalar;
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool ExecuteNonQuery(string sql, string connectionStringName)
        {
            if (String.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException(String.Format("{0} should be non-empty.", nameof(sql)));
            }

            var results = 0;
            var command = new SqlCommand(sql);
            try
            {
                command.Connection = GetSqlConnection(connectionStringName);

                ConfigureAndOpenCommand(command);
                results = command.ExecuteNonQuery();
            }
            finally
            {
                command.Connection.Close();
                command.Dispose();
            }

            return results > 0;
        }

        public static bool ExecuteNonQuery(SqlCommand command, string connectionStringName)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var results = 0;
            var conn = GetSqlConnection(connectionStringName);
            try
            {
                command.Connection = conn;

                ConfigureAndOpenCommand(command);
                results = command.ExecuteNonQuery();
            }
            finally
            {
                command.Connection.Close();
                command.Dispose();
            }

            return results > 0;
        }

        public static bool ExecuteNonQuery(SqlCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var results = 0;
            if (command.Connection != null)
            {
                try
                {
                    ConfigureAndOpenCommand(command);
                    results = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }

            return results > 0;
        }

        public static SqlDataReader ExecuteReader(string sql, string connectionStringName)
        {
            if (String.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException(String.Format("{0} should be non-empty.", nameof(sql)));
            }

            var command = new SqlCommand(sql, GetSqlConnection(connectionStringName));

            var reader = ExecuteReader(command, connectionStringName);
            return reader;
        }

        public static SqlDataReader ExecuteReader(SqlCommand command, string connectionStringName)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.Connection = GetSqlConnection(connectionStringName);

            ConfigureAndOpenCommand(command);

            var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

            if (reader?.HasRows != true)
            {
                reader = null;
            }

            return reader;
        }

        public static SqlDataReader ExecuteReader(SqlCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.Connection = GetSqlConnection();

            ConfigureAndOpenCommand(command);

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// must have <see cref="SqlCommand.Connection"/> already defined and attached to SqlCommand object
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReaderNullIfEmpty(SqlCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            SqlDataReader reader = null;
            if (command.Connection != null)
            {
                ConfigureAndOpenCommand(command);

                reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                if (reader?.HasRows != true)
                {
                    reader = null;
                }
            }
            return reader;
        }

        public static SqlDataReader ExecuteReader(string sqlQuery)
        {
            using (var cmd = new SqlCommand(sqlQuery, GetSqlConnection()))
            {
                cmd.CommandTimeout = 0;
                cmd.Connection.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public static SqlDataReader ExecuteReader(string sqlQuery, SqlConnection connection, int? timeout = null)
        {
            if (String.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentException(String.Format("{0} couldn't be empty", nameof(sqlQuery)));
            }

            var command = new SqlCommand(sqlQuery, connection);
            return ExecuteReader(command, connection);
        }

        public static SqlDataReader ExecuteReader(SqlCommand command, SqlConnection connection, int? timeout = null)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            command.Connection = connection;

            if (timeout.HasValue)
            {
                command.CommandTimeout = timeout.Value;
            }

            command.Connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Used in Data Export... Need to get the connection string UAD_Master and replace with actual dbName.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connectionStringName"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlCommand command, string connectionStringName, string dbName)
        {
            var connection = GetSqlConnection(connectionStringName);
            
            var connectionStringReplaced = connection.ConnectionString.Replace(DbNameMaster, dbName);
            var sqlConn = new SqlConnection(connectionStringReplaced);
            command.Connection = sqlConn;

            var reader = ExecuteReaderNullIfEmpty(command);
            return reader;
        }

        public static SqlCommand MinDateCheck(SqlCommand command)
        {
            foreach (SqlParameter parameter in command.Parameters)
            {
                if (parameter?.Value != null)
                {
                    if (parameter.Value.ToString().Equals(DateTime.MinValue.ToString()))
                    {
                        parameter.Value = DateTimeFunctions.GetMinDate().ToString();
                    }
                }
            }
            return command;
        }

        public static SqlCommand MinTimeCheck(SqlCommand command)
        {
            foreach (SqlParameter parameter in command.Parameters)
            {
                if (parameter?.Value != null)
                {
                    if (parameter.Value.ToString().Equals(TimeSpan.MinValue.ToString()))
                    {
                        parameter.Value = DateTimeFunctions.GetMinTime().ToString();
                    }
                }
            }
            return command;
        }

        public static SqlConnection GetSqlConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[AppSettingKmCommon].ToString();

            var connection = new SqlConnection(connectionString);
            return connection;
        }

        public static SqlConnection GetSqlConnection(string connectionStringName)
        {
            var connectionString = connectionStringName;
            var isDemo = false;
            var isNetworkDeployed = false;

            try
            {
                if (ConfigurationManager.ConnectionStrings[connectionStringName] != null)
                {
                    connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ToString();

                    Boolean.TryParse(ConfigurationManager.AppSettings[AppSettingIsDemo], out isDemo);
                    Boolean.TryParse(ConfigurationManager.AppSettings[AppSettingIsNetworkDeployed], out isNetworkDeployed);

                    if (isDemo == false)
                    {
                        if (connectionStringName.Equals(ConnectionString.UAD_Master.ToString()) ||
                            connectionStringName.Equals(ConnectionString.UAS.ToString()))
                        {
                            connectionString = connectionString.Replace(IpPart241, IpPart251);
                        }
                        else
                        {
                            connectionString = connectionString.Replace(IpPart241, IpPart198);
                        }
                    }

                    if (isNetworkDeployed)
                    {
                        connectionString = connectionString.Replace(IpPart21617, IpPart1010);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("GetSqlConnection error: {0}", ex);
            }

            var connection = new SqlConnection(connectionString);
            return connection;
        }

        public static List<T> GetList<T>(SqlCommand cmd, string connectionStringName)
        {
            var retList = new List<T>();
            try
            {
                using (var reader = ExecuteReader(cmd, connectionStringName))
                {
                    if (reader != null)
                    {
                        var builder = DynamicBuilder<T>.CreateBuilder(reader);
                        while (reader.Read())
                        {
                            var retItem = builder.Build(reader);
                            if (retItem != null)
                            {
                                retList.Add(retItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to build {0} instance: {1}.", typeof(T), ex);
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }

        public static List<T> GetList<T>(SqlCommand cmd)
        {
            var retList = new List<T>();
            try
            {
                using (var reader = ExecuteReaderNullIfEmpty(cmd))
                {
                    if (reader != null)
                    {
                        var builder = DynamicBuilder<T>.CreateBuilder(reader);
                        while (reader.Read())
                        {
                            var retItem = builder.Build(reader);
                            if (retItem != null)
                            {
                                retList.Add(retItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to build {0} instance: {1}.", typeof(T), ex);
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }

            return retList;
        }

        private static void ConfigureAndOpenCommand(SqlCommand command)
        {
            ConfigureCommand(command);
            command.Connection.Open();
        }

        private static void ConfigureCommand(SqlCommand command)
        {
            EnsureMinDateTime(command);
            command.CommandTimeout = 0;
        }

        private static void EnsureMinDateTime(SqlCommand command)
        {
            MinDateCheck(command);
            MinTimeCheck(command);
        }

        public static string CleanSerializedXML(string dirtyXml)
        {
            if (dirtyXml == null)
            {
                throw new ArgumentNullException(nameof(dirtyXml));
            }

            var returnClean =
                dirtyXml
                    .Replace(XmlHeader, String.Empty)
                    .Replace(XmlNsFrameworkUasEntity, String.Empty)
                    .Replace(XmlNsKMPlatformEntity, String.Empty);
            return returnClean;
        }
    }
}
