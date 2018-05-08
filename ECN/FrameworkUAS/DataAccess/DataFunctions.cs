using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Functions;
using KMPlatform.Object;
using KMCommonDataFunctions = KM.Common.DataFunctions;

namespace KMPlatform.DataAccess
{
    public class DataFunctions
    {
        #region Executes

        public static int Execute(SqlCommand cmd, string connectionStringName)
        {
            int success = 0;
            try
            {
                using (SqlConnection conn = KM.Common.DataFunctions.GetSqlConnection(connectionStringName))
                {
                    cmd = KMCommonDataFunctions.MinDateCheck(cmd);
                    cmd = KMCommonDataFunctions.MinTimeCheck(cmd);
                    cmd.Connection = conn;
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
            return success;
        }
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
                    cmd = KMCommonDataFunctions.MinDateCheck(cmd);
                    cmd = KMCommonDataFunctions.MinTimeCheck(cmd);
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

        public static object ExecuteScalar(string sql, string connectionStringName)
        {
            object obj = null;
            SqlCommand cmd = new SqlCommand(sql);
            try
            {
                using (cmd)
                {
                    cmd.CommandTimeout = 0;
                    cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(connectionStringName);
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
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlCommand command)
        {
            var result = KMCommonDataFunctions.ExecuteScalar(command, false);
            return result;
        }

        /// <summary>
        /// must have SqlCommand.Connection already defined and attached to SqlCommand object
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlCommand command)
        {
            var reader = KMCommonDataFunctions.ExecuteReaderNullIfEmpty(command);
            return reader;
        }
        #endregion

        public static SqlConnection GetClientSqlConnection(Entity.Client client)
        {
            var isDemo = false;
            bool.TryParse(ConfigurationManager.AppSettings[KMCommonDataFunctions.AppSettingIsDemo], out isDemo);
            var isNetworkDeployed = false;
            bool.TryParse(
                ConfigurationManager.AppSettings[KMCommonDataFunctions.AppSettingIsNetworkDeployed], 
                out isNetworkDeployed);
            var dbConn = string.Empty;

            if (!isDemo)
            {
                dbConn = client.ClientLiveDBConnectionString;
            }
            else
            {
                dbConn = client.ClientTestDBConnectionString;
            }

            if (isNetworkDeployed)
            {
                dbConn = dbConn.Replace(KMCommonDataFunctions.IpPart21617, KMCommonDataFunctions.IpPart1010);
            }

            return new SqlConnection(dbConn);
        }

        public static SqlConnection GetClientSqlConnection(ClientConnections client)
        {
            var isDemo = false;
            bool.TryParse(ConfigurationManager.AppSettings[KMCommonDataFunctions.AppSettingIsDemo], out isDemo);
            var isNetworkDeployed = false;
            bool.TryParse(
                ConfigurationManager.AppSettings[KMCommonDataFunctions.AppSettingIsNetworkDeployed],
                out isNetworkDeployed);
            var dbConn = string.Empty;

            if (!isDemo)
            {
                dbConn = client.ClientLiveDBConnectionString;
            }
            else
            {
                dbConn = client.ClientTestDBConnectionString;
            }

            if (isNetworkDeployed)
            {
                dbConn = dbConn.Replace(KMCommonDataFunctions.IpPart21617, KMCommonDataFunctions.IpPart1010);
            }

            return new SqlConnection(dbConn);
        }
    }
}
