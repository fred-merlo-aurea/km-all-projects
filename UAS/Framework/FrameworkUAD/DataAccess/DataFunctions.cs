using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Functions;
using StringFunctions = Core_AMS.Utilities.StringFunctions;
using KMCommonDataFunctions = KM.Common.DataFunctions;
using UASDataFunctions = FrameworkUAS.DataAccess.DataFunctions;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class DataFunctions
    {
        private const string AppSettingsIsDemo = "IsDemo";

        private const string AppSettingsIsNetworkDeployed = "IsNetworkDeployed";

        private const string DeployedConnectionStringIPPart = "10.10";

        private const string ConnectionStringIPPart = "216.17";

        #region DataTables

        public static DataTable GetDataTable(string commandText, SqlConnection connection)
        {
            return KMCommonDataFunctions.GetDataTable(commandText, connection);
        }

        public static DataTable GetDataTable(SqlCommand cmd, SqlConnection connection)
        {
            cmd.Connection = connection;
            cmd.CommandTimeout = 0;

            try
            {
                connection.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    return table;
                }
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion
        #region Executes
        /// <summary>
        /// must have SqlCommand.Connection already defined and attached to SqlCommand object
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static int Execute(SqlCommand cmd)
        {
            int success = 0;
            if (cmd.Connection != null)
            {
                try
                {
                    cmd = KM.Common.DataFunctions.MinDateCheck(cmd);
                    cmd = KM.Common.DataFunctions.MinTimeCheck(cmd);
                    using (cmd)
                    {
                        cmd.CommandTimeout = 0;
                        cmd.Connection.Open();
                        success = cmd.ExecuteNonQuery();
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
            }
            return success;
        }

        /// <summary>
        /// must have SqlCommand.Connection already defined and attached to SqlCommand object
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlCommand command)
        {
            var result = KMCommonDataFunctions.ExecuteScalar(command, false);
            return result;
        }

        /// <summary>
        /// must have SqlCommand.Connection already defined and attached to SqlCommand object
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlCommand command)
        {
            var reader = KMCommonDataFunctions.ExecuteReaderNullIfEmpty(command);
            return reader;
        }
        #endregion
        #region SQL Helpers
        public static string GetDBName(KMPlatform.Object.ClientConnections clientconnection)
        {
            bool isDemo = false;
            bool.TryParse(ConfigurationManager.AppSettings["IsDemo"].ToString(), out isDemo);
            bool isNetworkDeployed = false;
            bool.TryParse(ConfigurationManager.AppSettings["IsNetworkDeployed"].ToString(), out isNetworkDeployed);
            string dbConn = string.Empty;

            if (isDemo == false)
                dbConn = clientconnection.ClientLiveDBConnectionString;
            else
                dbConn = clientconnection.ClientTestDBConnectionString;

            System.Data.SqlClient.SqlConnectionStringBuilder connBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder();

            connBuilder.ConnectionString = dbConn;

            return connBuilder.InitialCatalog;
        }

        public static string CleanSerializedXML(string dirtyXML)
        {
            string returnClean = dirtyXML.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/FrameworkUAS.Entity\"", "").Replace("xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/FrameworkUAD.Entity\"","");
            return returnClean;
        }

        public static string CleanSerializedXML(string dirtyXML, string nameSpace)
        {
            string returnClean = dirtyXML.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/" + nameSpace + "\"", "").Replace("xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/FrameworkUAD.Entity\"","");
            return returnClean;
        }

        //public static string CleanString(string DirtyOne)
        //{
        //    string CleanOne = StringFunctions.Replace(DirtyOne, "'", "''");
        //    CleanOne = StringFunctions.Replace(CleanOne, "’", "''");
        //    CleanOne = StringFunctions.Replace(CleanOne, "–", "-");
        //    CleanOne = StringFunctions.Replace(CleanOne, "“", "\"");
        //    CleanOne = StringFunctions.Replace(CleanOne, "”", "\"");
        //    CleanOne = StringFunctions.Replace(CleanOne, "…", "...");
        //    return CleanOne;
        //}
        #endregion

        public static SqlConnection GetClientSqlConnection(KMPlatform.Entity.Client client)
        {
            return GetClientSqlConnection(client.ClientTestDBConnectionString, client.ClientLiveDBConnectionString);
        }

        public static SqlConnection GetClientSqlConnection(KMPlatform.Object.ClientConnections client)
        {
            return GetClientSqlConnection(client.ClientTestDBConnectionString, client.ClientLiveDBConnectionString);
        }

        private static SqlConnection GetClientSqlConnection(string demoDbConnectionString, string liveDbConnectionString)
        {
            bool isDemo;
            bool.TryParse(ConfigurationManager.AppSettings[AppSettingsIsDemo], out isDemo);

            bool isNetworkDeployed;
            bool.TryParse(ConfigurationManager.AppSettings[AppSettingsIsNetworkDeployed], out isNetworkDeployed);

            var connectionString = isDemo 
                                   ? demoDbConnectionString 
                                   : liveDbConnectionString;

            if (isNetworkDeployed)
            {
                connectionString = connectionString.Replace(ConnectionStringIPPart, DeployedConnectionStringIPPart);
            }

            return new SqlConnection(connectionString);
        }

        public enum ConnectionString
        {
            UAS,
            UAD_Lookup,
            UAD_Master,
            DataLoad,
            ECN_Activity,
            ECN_Accounts,
            ECN_Charity,
            ECN_Collector,
            ECN_Communicator,
            ECN_Creator,
            ECN_DomainTracker,
            ECN_Publisher,
            ECN_Warehouse,
            KMCommon,
            KMPlatform,
            SubGenData
        }

        public static T Get<T>(SqlCommand cmd, KMPlatform.Object.ClientConnections client)
        {
            cmd.Connection = GetClientSqlConnection(client);
            var retItem = (T) Activator.CreateInstance(typeof(T));
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
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
        public static List<T> GetList<T>(SqlCommand cmd, KMPlatform.Object.ClientConnections client)
        {
            cmd.Connection = GetClientSqlConnection(client);
            List<T> retList = new List<T>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        DynamicBuilder<T> builder = DynamicBuilder<T>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            var retItem = builder.Build(rdr);
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
    }
}
