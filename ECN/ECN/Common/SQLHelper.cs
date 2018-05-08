using System;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;

namespace ecn.common.classes 
{
    public class SQLHelper
    {

        #region GET DATATABLE
        public static DataTable getDataTable(string SQL, string connString)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("DataTable");
            ds.Tables.Add(dt);
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(SQL, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            ds.Tables["DataTable"].Load(dr);

            //SqlDataAdapter adapter = new SqlDataAdapter(SQL, connString);
            //adapter.SelectCommand.CommandTimeout = 0;			
            //adapter.Fill(ds, "DataTable");
            //adapter.SelectCommand.Connection.Close();
            //adapter.Dispose();
            return ds.Tables["DataTable"];
        }

        public static DataTable getDataTable(SqlCommand cmd, string connString)
        {
            SqlConnection conn = new SqlConnection(connString);
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataSet ds = new DataSet();
            //ds.Tables[0].TableName = "spresult";
            conn.Open();

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dt = new DataTable();//ds.Tables[0];
            dt.Load(dr);
            //da.Fill(ds, "spresult");
            conn.Close();

            return dt;
        }
        #endregion

        #region GET XML

        public static string executeXmlReader(string SQL, string RootNode, string connString)
        {
            DataSet ds = new DataSet(RootNode);

            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(SQL, conn);
            cmd.CommandTimeout = 0;

            ds.ReadXml(cmd.ExecuteXmlReader());

            return ds.GetXml();
        }

        #endregion


        #region GET DATASET
        public static DataSet getDataSet(SqlCommand cmd, string connString)
        {
            SqlConnection conn = new SqlConnection(connString);
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            //ds.Tables.Add(dt);
            //ds.Load(dr, LoadOption.OverwriteChanges,dt);

            da.Fill(ds);
            conn.Close();
            return ds;
        }

        public static DataSet getDataSet(string sql, string connString)
        {
            SqlConnection conn = new SqlConnection(connString);

            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            conn.Close();
            //SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            conn.Open();
            ds.Load(dr, LoadOption.OverwriteChanges, dt);
            //da.Fill(ds);
            conn.Close();
            return ds;
        }
        #endregion

        #region GET CODE VALUES from Code TABLE.
        public static string getCodeDisplayValue(string codeType, string codeValue, string connString)
        {
            string value = "";
            try
            {
                string sqlQuery =
                    "SELECT CodeDisplay FROM Code " +
                    "WHERE CodeValue = '" + codeValue + "' AND CodeType = '" + codeType + "';";
                DataTable dt = getDataTable(sqlQuery, connString);

                foreach (DataRow dr in dt.Rows)
                {
                    value = dr["codeDisplay"].ToString();
                }
            }
            catch
            {
                return "";
            }
            return value;
        }
        #endregion

        #region GET CODE VALUES from Code TABLE.
        public static string getMediaValue(string AssociatedField, int AssociatedID, string MediaTypeCode, string connString)
        {
            String codeDisplay = "";

            try
            {
                string sqlQuery =
                    "SELECT * FROM media " +
                    "WHERE AssociatedField='" + AssociatedField + "'" +
                    "AND AssociatedID='" + AssociatedID + "' " +
                    "AND MediaTypeCode='" + MediaTypeCode + "' " +
                    "ORDER BY MediaTypeCode;";
                DataTable dt = getDataTable(sqlQuery, connString);

                foreach (DataRow dr in dt.Rows)
                {
                    String MediaName = dr["MediaName"].ToString();
                    String FilePointer = dr["FilePointer"].ToString();
                    String URLPointer = dr["URLPointer"].ToString();
                    String PointerTypeCode = dr["PointerTypeCode"].ToString();
                    String TheFile = "";
                    if (FilePointer != null)
                    {
                        TheFile = FilePointer;
                    }
                    else
                    {
                        TheFile = URLPointer;
                    }
                    if (MediaTypeCode.Equals("link"))
                    {
                        if (PointerTypeCode.Equals("inline"))
                        {
                            codeDisplay = "<a href='" + TheFile + "'>" + MediaName + "</a><BR>";
                        }
                        if (PointerTypeCode.Equals("popup"))
                        {
                            codeDisplay = "<a href='" + TheFile + "' target='_new'>" + MediaName + "</a><BR>";
                        }
                    }
                    if (MediaTypeCode.Equals("photo"))
                    {
                        if (PointerTypeCode.Equals("inline"))
                        {
                            codeDisplay = "<img src='" + TheFile + "' alt='" + MediaName + "'><BR>";
                        }
                        if (PointerTypeCode.Equals("popup"))
                        {
                            codeDisplay = "<a href='" + TheFile + "' target='_new'>" + MediaName + "</a><BR>";
                        }
                    }
                    if (MediaTypeCode.Equals("logo"))
                    {
                        if (PointerTypeCode.Equals("inline"))
                        {
                            codeDisplay = "<img src='" + TheFile + "' alt='" + MediaName + "'><BR>";
                        }
                        if (PointerTypeCode.Equals("popup"))
                        {
                            codeDisplay = "<a href='" + TheFile + "' target='_new'>" + MediaName + "</a><BR>";
                        }
                    }
                    if (MediaTypeCode.Equals("doc"))
                    {
                        if (PointerTypeCode.Equals("inline"))
                        {
                            codeDisplay = "<a href='" + TheFile + "'>" + MediaName + "</a><BR>";
                        }
                        if (PointerTypeCode.Equals("popup"))
                        {
                            codeDisplay = "<a href='" + TheFile + "' target='_new'>" + MediaName + "</a><BR>";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string devnull = e.ToString();
            }
            return codeDisplay;
        }
        #endregion

        #region SQL EXECUTE, EXECUTE SCLAR
        public static int execute(string sql, string connString)
        {
            SqlCommand cmd = new SqlCommand(sql, new SqlConnection(connString));
            cmd.Connection.Open();
            int success = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return success;
        }

        public static int execute(SqlCommand cmd, string connString)
        {
            SqlConnection conn = new SqlConnection(connString);
            cmd.Connection = conn;
            conn.Open();
            int success = cmd.ExecuteNonQuery();
            conn.Close();
            return success;
        }

        public static object executeScalar(string sql, string connString)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = conn;
            cmd.Connection.Open();
            object obj = cmd.ExecuteScalar();
            conn.Close();
            return obj;
        }

        public static object executeScalar(SqlCommand cmd, string connString)
        {
            SqlConnection conn = new SqlConnection(connString);
            cmd.Connection = conn;
            cmd.Connection.Open();
            object obj = cmd.ExecuteScalar();
            conn.Close();
            return obj;
        }

        public static SqlDataReader executeReader(string sql, string connString)
        {
            SqlCommand cmd = new SqlCommand(sql, new SqlConnection(connString));
            cmd.Connection.Open();
            return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }
        #endregion

    }
}
