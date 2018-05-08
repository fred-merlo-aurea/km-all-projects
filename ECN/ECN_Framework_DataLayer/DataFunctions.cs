using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using ECN_Framework_Common.Functions;

using KMCommonDataFunctions =  KM.Common.DataFunctions;

namespace ECN_Framework_DataLayer
{
    [Serializable]
    public class DataFunctions
    {
        public static SqlCommand BuildCommand(
            SqlConnection connection,
            string cmdText,
            Dictionary<string, object> cmdParams,
            CommandType cmdType = CommandType.StoredProcedure,
            int cmdTimeOut = 0)
        {
            var cmd = connection.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandTimeout = 0;
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;
            foreach (var p in cmdParams)
            {
                cmd.Parameters.AddWithValue(p.Key, p.Value);
            }
            return cmd;
        }

        public static DataSet GetDataSet(SqlCommand cmd, string connectionStringName)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = DataFunctions.GetSqlConnection(connectionStringName))
            {
            cmd = KMCommonDataFunctions.MinDateCheck(cmd);
            cmd = KMCommonDataFunctions.MinTimeCheck(cmd);
                cmd.Connection = conn;
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.Connection.Open();
            da.Fill(ds);
            cmd.Connection.Close();
            }
            return ds;
        }

        public static DataTable GetDataTable(SqlCommand cmd, string connectionStringName)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DataFunctions.GetSqlConnection(connectionStringName))
            {
            cmd = KMCommonDataFunctions.MinDateCheck(cmd);
            cmd = KMCommonDataFunctions.MinTimeCheck(cmd);
                cmd.Connection = conn;
            cmd.CommandTimeout = 0;
                try
                {
            cmd.Connection.Open();
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            dt.Load(dr);
                }
                catch (Exception)
                {                    
                    throw;
                }
                finally
                {
                    if (cmd != null)
                    {
            cmd.Connection.Close();
                        cmd.Dispose();
                    }
                }
            }

            return dt;
        }

        public static DataTable GetDataTable(string sql, string connectionStringName)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DataFunctions.GetSqlConnection(connectionStringName))
            {
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Connection = conn;
            cmd.CommandTimeout = 0;
            cmd.Connection.Open();
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            dt.Load(dr);
            cmd.Connection.Close();
            }
            return dt;
        }

        public static int Execute(string sql, string connectionStringName)
        {
            int success = 0;
            using (SqlConnection conn = DataFunctions.GetSqlConnection(connectionStringName))
            {
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Connection = conn;
            cmd.CommandTimeout = 0;
            cmd.Connection.Open();
                success = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            }
            return success;
        }
        public static int Execute(SqlCommand cmd, string connectionStringName)
        {
            int success = 0;
            using (SqlConnection conn = DataFunctions.GetSqlConnection(connectionStringName))
            {
                cmd = KMCommonDataFunctions.MinDateCheck(cmd);
                cmd = KMCommonDataFunctions.MinTimeCheck(cmd);
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.Connection.Open();
                success = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            return success;
        }
        public static object ExecuteScalar(string sql, string connectionStringName)
        {
            object obj = null;
            using (SqlConnection conn = DataFunctions.GetSqlConnection(connectionStringName))
            {
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandTimeout = 0;
                cmd.Connection = conn;
            cmd.Connection.Open();
                obj = cmd.ExecuteScalar();
            cmd.Connection.Close();
            }
            return obj;
        }
        public static object ExecuteScalar(SqlCommand cmd, string connectionStringName)
        {
            object obj = null;
            using (SqlConnection conn = DataFunctions.GetSqlConnection(connectionStringName))
            {
                cmd = KMCommonDataFunctions.MinDateCheck(cmd);
                cmd = KMCommonDataFunctions.MinTimeCheck(cmd);
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                try
                {
                cmd.Connection.Open();
                obj = cmd.ExecuteScalar();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (cmd != null)
                    {
                cmd.Connection.Close();
                        cmd.Dispose();
                    }
                }
            }
            return obj;
        }

        public static bool ExecuteNonQuery(SqlCommand cmd, string connectionStringName)
        {
            var result = KMCommonDataFunctions.ExecuteNonQuery(cmd, connectionStringName);
            return result;
        }

        public static SqlDataReader ExecuteReader(SqlCommand cmd, string connectionStringName)
        {
            var reader = KMCommonDataFunctions.ExecuteReader(cmd, connectionStringName);
            return reader;
        }

        //Connection String
        public static SqlConnection GetSqlConnection(string connectionStringName)
        {
            string connectionString;
            bool isDemo = false;
            bool.TryParse(ConfigurationManager.AppSettings["IsDemo"].ToString(), out isDemo);

            if (isDemo == true)
                connectionString = ConfigurationManager.ConnectionStrings[connectionStringName.ToString()].ToString();
            else
                connectionString = ConfigurationManager.ConnectionStrings[connectionStringName.ToString()].ToString();

            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public enum ConnectionString
        {
            Accounts,
            Activity,
            Charity,
            Collector,
            Communicator,
            Creator,
            DomainTracker,
            Misc,
            Publisher,
            Temp,
            FormDesigner,
            UAS,
            Content
        }


        public static string CleanString(string DirtyOne)
        {
            string CleanOne = StringFunctions.Replace(DirtyOne, "'", "''");
            CleanOne = StringFunctions.Replace(CleanOne, "’", "''");
            CleanOne = StringFunctions.Replace(CleanOne, "–", "-");
            CleanOne = StringFunctions.Replace(CleanOne, "“", "\"");
            CleanOne = StringFunctions.Replace(CleanOne, "”", "\"");
            CleanOne = StringFunctions.Replace(CleanOne, "…", "...");
            return CleanOne;
        }

        public static string GetNewGUID()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT GUID = NewID()";
            return ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString();
        }

    }
}
