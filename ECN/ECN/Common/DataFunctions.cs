using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ECN.Common.Interfaces;
using ECN.Common.Helpers;
using CommonStringFunctions = KM.Common.StringFunctions;

namespace ecn.common.classes
{
    public class DataFunctions
    {
        public static string connStr = ConfigurationManager.AppSettings["connString"];
        public static string accountsdb = ConfigurationManager.AppSettings["accountsdb"];
        public static string communicatordb = ConfigurationManager.AppSettings["communicatordb"];
        public static string con_creator = ConfigurationManager.AppSettings["cre"];
        public static string con_collector = ConfigurationManager.AppSettings["col"];
        public static string con_communicator = ConfigurationManager.AppSettings["com"];
        public static string con_accounts = ConfigurationManager.AppSettings["act"];
        public static string con_charity = ConfigurationManager.AppSettings["chr"];
        public static string con_publisher = ConfigurationManager.AppSettings["pub"];
        public static string con_misc = ConfigurationManager.AppSettings["misc"];
        public static string con_activity = ConfigurationManager.AppSettings["activity"];
        private const string DbAccounts = "accounts";
        private const string DbCollector = "collector";
        private const string DbCreator = "creator";
        private const string DbCommunicator = "communicator";
        private const string DbCharity = "charity";
        private const string DbPublisher = "publisher";
        private const string DbMisc = "misc";
        private const string DbActivity = "activity";
        public static string product_name = ConfigurationManager.AppSettings["product_name"];

        private static IDatabaseFunctions DatabaseFunctions;
        public DataFunctions()
        {
            DatabaseFunctions = new DatabaseFunctionsAdapter();
        }
        public DataFunctions(IDatabaseFunctions databaseFunctions)
        {
            DatabaseFunctions = databaseFunctions;
        }

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

        public static DataSet GetDataSet(string db, SqlCommand cmd)
        {   
            var conn = GetConnectionStringObject(db);

            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            conn.Open();
            cmd.Connection = conn;
            da.Fill(ds);
            conn.Close();
            return ds;
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

        public static DataTable GetDataTable(string dbName, SqlCommand cmd)
        {
            var conn = GetConnectionStringObject(dbName);

            DataTable dt = null;
            using (conn)
            {
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    cmd.Connection.Open();
                    da.Fill(ds, "spresult");
                    dt = ds.Tables[0];
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

        //generic functions (method overload 1)
        public static DataTable GetDataTable(string SQL)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(SQL, connStr);
            adapter.SelectCommand.CommandTimeout = 0;
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
            adapter.SelectCommand.CommandTimeout = 0;
            adapter.Fill(ds, "DataTable");
            adapter.SelectCommand.Connection.Close();
            adapter.Dispose();
            return ds.Tables["DataTable"];
        }


        // method used for Data Mapper 
        // Datatable as a param (method overload 1)
        public static ArrayList GetDataTableColumns(DataTable dataTable)
        {
            var columnHeadings = new ArrayList();
            for (var index = 0; index < dataTable.Columns.Count; index++)
            {
                columnHeadings.Add(dataTable.Columns[index].ColumnName);
            }

            return columnHeadings;
        }

        public static string CodeValue(string codeType, string codeValue)
        {
            string codeDisplay = "";

            try
            {
                string sqlQuery =
                    "SELECT CodeDisplay FROM code " +
                    "WHERE codeValue = '" + codeValue + "'" +
                    "AND codeType = '" + codeType + "';";
                DataTable dt = GetDataTable(sqlQuery);

                foreach (DataRow dr in dt.Rows)
                {
                    codeDisplay = dr["codeDisplay"].ToString();
                }

            }
            catch (Exception e)
            {
                string devnull = e.ToString();
            }
            return codeDisplay;
        }

        public static int Execute(string SQL)
        {
            int success = 0;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                try
                {
                    cmd.Connection.Open();
                    success = cmd.ExecuteNonQuery();
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
            return success;
            //SqlConnection conn = new SqlConnection(connStr);
            //SqlCommand cmd = new SqlCommand(SQL, conn);
            //cmd.Connection.Open();
            //int success = cmd.ExecuteNonQuery();
            //conn.Close();
            //return success;
        }

        public static int Execute(SqlCommand cmd)
        {
            int success = 0;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                try
                {
                    cmd.Connection.Open();
                    success = cmd.ExecuteNonQuery();
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
            return success;

            //SqlConnection conn = new SqlConnection(connStr);
            //cmd.Connection = conn;
            //conn.Open();
            //int success = cmd.ExecuteNonQuery();
            //conn.Close();
            //return success;
        }

        public static SqlConnection GetSqlConnection()
        {
            string environment = ConfigurationManager.AppSettings["Environment"].ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["Database_" + environment].ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public static int Execute(string dbName, string commandText)
        {
            var connection = GetConnectionStringObject(dbName);
            var command = new SqlCommand(commandText, connection);

            DatabaseFunctions.OpenConnection(command);
            var success = DatabaseFunctions.ExecuteNonQuery(command);
            DatabaseFunctions.CloseConnection(connection);
            return success;
        }

        private static SqlConnection GetConnectionStringObject(string dbName)
        {
            var connectionString = GetConnectionString(dbName);
            var connection = new SqlConnection(connectionString);
            return connection;
        }

        public static int Execute(string db, SqlCommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            using (var conn = GetConnectionStringObject(db))
            {
                conn.Open();
                cmd.Connection = conn;
                var success = cmd.ExecuteNonQuery();
                return success;
            }
        }

        public static object ExecuteScalar(string db, string SQL)
        {
            var conn = GetConnectionStringObject(db);

            object obj = null;
            using (conn)
            {
                SqlCommand cmd = new SqlCommand(SQL, conn);
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

        public static object ExecuteScalar(string SQL)
        {
            return ExecuteScalar("default", SQL);
        }

        public static object ExecuteScalar(SqlCommand cmd)
        {
            //SqlConnection conn = new SqlConnection(connStr);
            //cmd.Connection = conn;
            //cmd.Connection.Open();
            //object obj = cmd.ExecuteScalar();
            //conn.Close();
            //return obj;


            object obj = null;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
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

        public static SqlDataReader ExecuteReader(SqlCommand cmd)
        {
            cmd = MinDateCheck(cmd);
            cmd.Connection = new SqlConnection(connStr);
            cmd.Connection.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        private static SqlCommand MinDateCheck(SqlCommand cmd)
        {
            foreach (SqlParameter p in cmd.Parameters)
            {
                if (p != null && p.Value != null)
                {
                    if (p.Value.ToString().Equals(DateTime.MinValue.ToString()))
                    {
                        p.Value = GetMinDate().ToString();
                    }
                }
            }
            
            return cmd;
        }

        private static DateTime GetMinDate()
        {
            DateTime minDate = Convert.ToDateTime("1/1/1900");
            return minDate;
        }

        public static object ExecuteScalar(string dbName, SqlCommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            using (var conn = GetConnectionStringObject(dbName))
            {
                conn.Open();
                cmd.Connection = conn;
                var scalar = cmd.ExecuteScalar();
                return scalar;
            }
        }

        //predefined functions
        public static string GetContent(int ContentID)
        {
            string ContentSource = "";
            string ContentText = "";
            string ContentURL = "";
            string ContentTypeCode = "";

            if (ContentID > 0)
            {
                DataTable dt = GetDataTable("SELECT * " + "FROM " + communicatordb + ".dbo.Content WHERE ContentID = " + ContentID);

                if (dt.Rows.Count > 0)
                {
                    ContentTypeCode = dt.Rows[0]["ContentTypeCode"].ToString();
                    ContentSource = dt.Rows[0]["ContentSource"].ToString();
                    ContentText = dt.Rows[0]["ContentText"].ToString();
                    ContentURL = dt.Rows[0]["ContentURL"].ToString();
                }
            }

            switch (ContentTypeCode.ToLower())
            {
                case "html":
                    return ContentSource;
                case "text":
                    return ContentText;
                case "feed":
                    return TemplateFunctions.getWebFeed(ContentURL);
                default:
                    return ContentSource;
            }
        }

        //WGH - For mobile support
        public static string GetContent(int ContentID, bool IsMobile)
        {
            string ContentSource = "";
            string ContentMobile = "";
            string ContentText = "";
            string ContentURL = "";
            string ContentTypeCode = "";

            if (ContentID > 0)
            {
                DataTable dt = GetDataTable("SELECT * " + "FROM " + communicatordb + ".dbo.Content WHERE ContentID = " + ContentID + " and IsDeleted = 0");

                if (dt.Rows.Count > 0)
                {
                    ContentTypeCode = dt.Rows[0]["ContentTypeCode"].ToString();
                    ContentSource = dt.Rows[0]["ContentSource"].ToString();
                    ContentMobile = dt.Rows[0]["ContentMobile"].ToString();
                    ContentText = dt.Rows[0]["ContentText"].ToString();
                    ContentURL = dt.Rows[0]["ContentURL"].ToString();
                }
            }

            switch (ContentTypeCode.ToLower())
            {
                case "html":
                    if (IsMobile)
                    {
                        if (ContentMobile.Length > 0)
                        {
                            return ContentMobile;
                        }
                        else
                        {
                            return ContentSource;
                        }
                    }
                    else
                    {
                        return ContentSource;
                    }
                case "text":
                    return ContentText;
                case "feed":
                    return TemplateFunctions.getWebFeed(ContentURL);
                default:
                    return ContentSource;
            }
        }

        public static string GetTextContent(int ContentID)
        {
            string ContentText = "";
            string ContentURL = "";
            string ContentTypeCode = "";

            if (ContentID > 0)
            {
                DataTable dt = GetDataTable("SELECT * FROM Content WHERE ContentID=" + ContentID + " and IsDeleted = 0");

                if (dt.Rows.Count > 0)
                {
                    ContentTypeCode = dt.Rows[0]["ContentTypeCode"].ToString();
                    ContentText = dt.Rows[0]["ContentText"].ToString();
                    ContentURL = dt.Rows[0]["ContentURL"].ToString();
                }
            }

            switch (ContentTypeCode.ToLower())
            {
                case "html":
                    return ContentText;
                case "text":
                    return ContentText;
                case "feed":
                    string urlfeed = TemplateFunctions.getWebFeed(ContentURL);
                    var strippedurl = CommonStringFunctions.CleanHtmlString(urlfeed);
                    return strippedurl;
                default:
                    return ContentText;
            }
        }

        public static string GetConnectionString()
        {
            return connStr;
        }

        public static string GetConnectionString(string dbName)
        {
            switch (dbName)
            {
                case DbAccounts:
                    return con_accounts;
                case DbCollector:
                    return con_collector;
                case DbCreator:
                    return con_creator;
                case DbCommunicator:
                    return con_communicator;
                case DbCharity:
                    return con_charity;
                case DbPublisher:
                    return con_publisher;
                case DbMisc:
                    return con_misc;
                case DbActivity:
                    return con_activity;
                default:
                    return connStr;
            }
        }

        public static string GetUser(int UserID)
        {
            if (UserID == 0)
                return string.Empty;

            try
            {
                return ExecuteScalar("SELECT UserName FROM " + accountsdb + ".dbo.Users  WHERE UserID=" + UserID).ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetPassword(int UserID)
        {
            if (UserID == 0)
                return string.Empty;

            try
            {
                return ExecuteScalar("SELECT Password FROM " + accountsdb + ".dbo.Users WHERE UserID=" + UserID).ToString();
            }
            catch
            {
                return string.Empty;
            }
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

        public static string StripString(string text)
        {
            return StringFunctions.Remove(text, CommonStringFunctions.GetNonAlphaCharacters());
        }

        public static string MediaValue(string AssociatedField, int AssociatedID, string MediaTypeCode)
        {
            var codeDisplay = String.Empty;

            try
            {
                string sqlQuery =
                    "SELECT * FROM media " +
                    "WHERE AssociatedField='" + AssociatedField + "'" +
                    "AND AssociatedID='" + AssociatedID + "' " +
                    "AND MediaTypeCode='" + MediaTypeCode + "' " +
                    "ORDER BY MediaTypeCode;";
                DataTable dt = GetDataTable(sqlQuery);

                foreach (DataRow dr in dt.Rows)
                {
                    var MediaName = dr["MediaName"].ToString();
                    var FilePointer = dr["FilePointer"].ToString();
                    var URLPointer = dr["URLPointer"].ToString();
                    var PointerTypeCode = dr["PointerTypeCode"].ToString();
                    var TheFile = "";
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
    }
}
