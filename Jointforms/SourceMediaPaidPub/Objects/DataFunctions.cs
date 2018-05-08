using System;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;

namespace PaidPub.Objects
{
    public class DataFunctions
    {
        public static string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        public static string con_communicator = ConfigurationManager.ConnectionStrings["ecn5_communicator"].ConnectionString;
        public static string con_accounts = ConfigurationManager.ConnectionStrings["ecn5_accounts"].ConnectionString;

        public static DataTable GetDataTable(SqlCommand cmd)
        {
            SqlConnection conn = new SqlConnection(connStr);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            conn.Open();
            cmd.Connection = conn;
            da.Fill(ds, "spresult");
            conn.Close();
            DataTable dt = ds.Tables[0];
            return dt;
        }

        public static DataSet GetDataSet(SqlCommand cmd)
        {
            SqlConnection conn = new SqlConnection(connStr);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            conn.Open();
            cmd.Connection = conn;
            da.Fill(ds);
            conn.Close();
            return ds;
        }

        public static DataTable GetDataTable(string db, SqlCommand cmd)
        {
            SqlConnection conn;
            switch (db)
            {
                case "accounts":
                    conn = new SqlConnection(con_accounts);
                    break;

                case "communicator":
                    conn = new SqlConnection(con_communicator);
                    break;


                default:
                    conn = new SqlConnection(connStr);
                    break;
            }

            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            conn.Open();
            cmd.Connection = conn;
            da.Fill(ds, "spresult");
            conn.Close();
            DataTable dt = ds.Tables[0];
            return dt;
        }



        //generic functions (method overload 1)
        public static DataTable GetDataTable(string SQL)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(SQL, connStr);
            //adapter.SelectCommand.CommandTimeout = 0;			
            adapter.Fill(ds, "DataTable");
            adapter.SelectCommand.Connection.Close();
            //adapter.Dispose();
            return ds.Tables["DataTable"];
        }

        // method used for Data Mapper 
        //OverLoad the method with 2 params, by passing sql & the connString (method overload 2 )
        public static DataTable GetDataTable(string SQL, string connString)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(SQL, connString);
            //adapter.SelectCommand.CommandTimeout = 0;
            adapter.Fill(ds, "DataTable");
            adapter.SelectCommand.Connection.Close();
            //adapter.Dispose();
            return ds.Tables["DataTable"];
        }


        public static int Execute(string SQL)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(SQL, conn);
            cmd.Connection.Open();
            int success = cmd.ExecuteNonQuery();
            conn.Close();
            return success;
        }

        public static int Execute(SqlCommand cmd)
        {
            SqlConnection conn = new SqlConnection(connStr);
            cmd.Connection = conn;
            conn.Open();
            int success = cmd.ExecuteNonQuery();
            conn.Close();
            return success;
        }

        public static int Execute(string db, string SQL)
        {
            SqlConnection conn;
            switch (db)
            {
                case "accounts":
                    conn = new SqlConnection(con_accounts);
                    break;

                case "communicator":
                    conn = new SqlConnection(con_communicator);
                    break;

                default:
                    conn = new SqlConnection(connStr);
                    break;
            }

            SqlCommand cmd = new SqlCommand(SQL, conn);
            cmd.Connection.Open();
            int success = cmd.ExecuteNonQuery();
            conn.Close();
            return success;
        }

        public static int Execute(string db, SqlCommand cmd)
        {

            SqlConnection conn;
            switch (db)
            {
                case "accounts":
                    conn = new SqlConnection(con_accounts);
                    break;

                case "communicator":
                    conn = new SqlConnection(con_communicator);
                    break;

                default:
                    conn = new SqlConnection(connStr);
                    break;
            }
            cmd.Connection = conn;
            cmd.Connection.Open();
            int success = cmd.ExecuteNonQuery();
            conn.Close();
            return success;
        }

        public static object ExecuteScalar(string db, string SQL)
        {
            SqlConnection conn;
            switch (db)
            {
                case "accounts":
                    conn = new SqlConnection(con_accounts);
                    break;

                case "communicator":
                    conn = new SqlConnection(con_communicator);
                    break;

                default:
                    conn = new SqlConnection(connStr);
                    break;
            }

            SqlCommand cmd = new SqlCommand(SQL, conn);
            cmd.CommandTimeout = 6000;
            cmd.Connection.Open();
            object obj = cmd.ExecuteScalar();
            conn.Close();
            return obj;
        }

        public static object ExecuteScalar(string SQL)
        {
            return ExecuteScalar("default", SQL);
        }

        public static object ExecuteScalar(SqlCommand cmd)
        {
            SqlConnection conn = new SqlConnection(connStr);
            cmd.Connection = conn;
            cmd.Connection.Open();
            object obj = cmd.ExecuteScalar();
            conn.Close();
            return obj;
        }

        public static object ExecuteScalar(string db, SqlCommand cmd)
        {
            SqlConnection conn;
            switch (db)
            {
                case "accounts":
                    conn = new SqlConnection(con_accounts);
                    break;

                case "communicator":
                    conn = new SqlConnection(con_communicator);
                    break;

                default:
                    conn = new SqlConnection(connStr);
                    break;
            }

            cmd.Connection = conn;
            cmd.Connection.Open();
            object obj = cmd.ExecuteScalar();
            conn.Close();
            return obj;
        }


        public static string GetConnectionString()
        {
            return connStr;
        }

        public static string GetConnectionString(string db)
        {
            switch (db)
            {
                case "accounts":
                    return con_accounts;

                case "communicator":
                    return con_communicator;

                default:
                    return connStr;
            }
        }

        public static string CleanString(string DirtyOne)
        {
            string CleanOne = DirtyOne.Replace("'", "''");
            CleanOne = CleanOne.Replace("’", "''");
            CleanOne = CleanOne.Replace("–", "-");
            CleanOne = CleanOne.Replace("“", "\"");
            CleanOne = CleanOne.Replace("”", "\"");
            CleanOne = CleanOne.Replace("…", "...");
            return CleanOne;
        }

    }
}
