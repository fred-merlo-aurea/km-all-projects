using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using KMPlatform.Object;
using static ECN_Framework_DataLayer.DataFunctions;
using CommonDataFunctions = KM.Common.DataFunctions;
using CommonStringFunctions = KM.Common.StringFunctions;

namespace KMPS.MD.Objects
{
    public class DataFunctions
    {
        #region GET XML

        public static string executeXmlReader(string SQL, string RootNode, SqlConnection conn)
        {
            DataSet ds = new DataSet(RootNode);

            conn.Open();
            SqlCommand cmd = new SqlCommand(SQL, conn);
            cmd.CommandTimeout = 0;

            ds.ReadXml(cmd.ExecuteXmlReader());
            conn.Close();
            return ds.GetXml();
        }

        #endregion
        #region GET DATATABLE

        public static DataTable getDataTable(string SQL, SqlConnection conn)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("DataTable");
            ds.Tables.Add(dt);

            SqlCommand cmd = new SqlCommand(SQL, conn);
            cmd.CommandTimeout = 0;
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            ds.Tables["DataTable"].Load(dr);
            conn.Close();
            //SqlDataAdapter adapter = new SqlDataAdapter(SQL, connString);
            //adapter.SelectCommand.CommandTimeout = 0;			
            //adapter.Fill(ds, "DataTable");
            //adapter.SelectCommand.Connection.Close();
            //adapter.Dispose();
            return ds.Tables["DataTable"];
        }
        public static DataTable getDataTable(SqlCommand cmd, SqlConnection conn)
        {

            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataSet ds = new DataSet();
            //ds.Tables[0].TableName = "spresult";

            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            conn.Open();

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dt = new DataTable();//ds.Tables[0];
            dt.Load(dr);
            //da.Fill(ds, "spresult");
            conn.Close();

            return dt;
        }

        #endregion

        #region SQL EXECUTE, EXECUTE SCLAR

        public static int execute(SqlCommand cmd, SqlConnection conn)
        {

            cmd.Connection = conn;
            conn.Open();
            int success = cmd.ExecuteNonQuery();
            conn.Close();
            return success;
        }

        [Obsolete("Use KM.Common.DataFunctions.ExecuteScalar() method instead")]
        public static object executeScalar(SqlCommand cmd, SqlConnection conn)
        {
            return CommonDataFunctions.ExecuteScalar(cmd, conn);
        }

        public static int ExecuteScalar(
            ClientConnections clientConnections,
            string commandText,
            IEnumerable<SqlParameter> sqlParameters)
        {
            using (var sqlConnection = GetClientSqlConnection(clientConnections))
            {
                using (var sqlCommand = CreateStoredProcedureSqlCommand(commandText, sqlConnection, sqlParameters))
                {
                    return Convert.ToInt32(CommonDataFunctions.ExecuteScalar(sqlCommand, sqlConnection));
                }
            }
        }

        #endregion

        public static SqlConnection GetSqlConnection1(string connectionStringName)
        {
            string connectionString = connectionStringName;
            bool isDemo = false;
            bool isNetworkDeployed = false;

            try
            {
                if (ConfigurationManager.ConnectionStrings[connectionStringName.ToString()] != null)
                {
                    connectionString = ConfigurationManager.ConnectionStrings[connectionStringName.ToString()].ToString();

                    bool.TryParse(ConfigurationManager.AppSettings["IsDemo"].ToString(), out isDemo);
                    bool.TryParse(ConfigurationManager.AppSettings["IsNetworkDeployed"].ToString(), out isNetworkDeployed);

                    if (isDemo == false)
                    {
                        if (connectionStringName.Equals(connectionStringName.Equals(ConnectionString.UAS.ToString())))
                            connectionString = connectionString.Replace("241", "251");
                        else
                            connectionString = connectionString.Replace("241", "198");
                    }

                    if (isNetworkDeployed == true)
                        connectionString = connectionString.Replace("216.17", "10.10");
                }
            }
            catch { }

            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public static string GetSubDomain()
        {
            string server = (string)System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            return GetSubDomain(server);
        }

        public static string GetSubDomain(string serverName)
        {
            string subDomain = "";
            int length = serverName.Split('.').Length;
            if (length > 2)
            {
                int first = serverName.IndexOf(".");
                subDomain = serverName.Substring(0, first);
            }
            else if (serverName.Trim().ToUpper() == "LOCALHOST")
            {
                subDomain = "localhost";
            }
            return subDomain;
        }

        public static SqlConnection GetClientSqlConnection(KMPlatform.Object.ClientConnections clientconnection)
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

            if (isNetworkDeployed == true)
                dbConn = dbConn.Replace("216.17", "10.10");

            return new SqlConnection(dbConn);
        }

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
             
        public static string CleanString(string text)
        {
            return CommonStringFunctions.CleanString(text);
        }

        public static SqlCommand CreateStoredProcedureSqlCommand(
            string commandText,
            SqlConnection sqlConnection,
            IEnumerable<SqlParameter> sqlParameter)
        {
            var sqlCommand = new SqlCommand(commandText)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 0,
                Connection = sqlConnection
            };

            foreach (var parameter in sqlParameter)
            {
                sqlCommand.Parameters.Add(parameter);
            }

            return sqlCommand;
        }

        public static SqlCommand CreateTextSqlCommand(string commandText, SqlConnection sqlConnection, IEnumerable<SqlParameter> sqlParameter)
        {
            var sqlCommand = new SqlCommand(commandText)
            {
                CommandType = CommandType.Text,
                CommandTimeout = 0,
                Connection = sqlConnection
            };

            foreach (var parameter in sqlParameter)
            {
                sqlCommand.Parameters.Add(parameter);
            }

            return sqlCommand;
        }
    }
}
