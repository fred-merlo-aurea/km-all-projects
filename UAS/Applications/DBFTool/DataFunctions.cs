using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.IO;
using System.Text;

namespace DBFtoUAD_Circ_Migration
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

            try
            {
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
            }
            catch {}
            return ds.Tables["DataTable"];
        }
        public static DataTable getDataTable(SqlCommand cmd, SqlConnection conn)
        {

            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataSet ds = new DataSet();
            //ds.Tables[0].TableName = "spresult";

            try
            {
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
            catch{}

            return null;
        }

        #endregion

        #region SQL EXECUTE, EXECUTE SCLAR

        public static int execute_codeSheetValidation(int sourceFileId, string processCode, SqlConnection conn)
        {

            SqlCommand cmd = new SqlCommand("job_CodesheetValidation", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@sourcefileID", sourceFileId);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection.Open();
            int success = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return success;
        }

        public static int execute_subscriberFinal_save(string fileType, string processCode, SqlConnection conn)
        {

            SqlCommand cmd = new SqlCommand("e_SubscriberFinal_SaveDQMClean", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@FileType", fileType);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection.Open();
            int success = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return success;
        }

        public static int execute_data_matching(int sourceFileId, string processCode, SqlConnection conn)
        {

            SqlCommand cmd = new SqlCommand("job_DataMatching", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileId);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection.Open();
            int success = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return success;
        }

        public static int execute_remove_by_pubcode(string pubCode, SqlConnection conn)
        {

            SqlCommand cmd = new SqlCommand("job_ADMS_Remove_By_PubCode", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@PubCode", pubCode);
            cmd.Connection.Open();
            int success = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return success;
        }

        public static int execute_ImportFromUAS(string processCode, string fileType, SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("e_ImportFromUAS", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@FileType", fileType);
            cmd.Connection.Open();
            int success = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return success;
        }

        public static void ImportSubscribers(string xmlImport, SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("job_ImportSubscriber", conn);

            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.AddWithValue("@MagazineID", MagazineID);
            cmd.Parameters.AddWithValue("@XML", xmlImport);

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static void ImportSubscriberDemographics(string xmlImport, SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("job_ImportSubscriberDemographics", conn);

            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@XML", xmlImport);

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static int execute(SqlCommand cmd, SqlConnection conn)
        {

            cmd.Connection = conn;
            conn.Open();
            int success = cmd.ExecuteNonQuery();
            conn.Close();
            return success;
        }

        public static object executeScalar(string sql, SqlConnection conn)
        {

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = conn;
            cmd.Connection.Open();
            object obj = cmd.ExecuteScalar();
            conn.Close();
            return obj;
        }

        #endregion
        #region GET DICTIONARY
        public static Dictionary<string, int> getQSource(SqlConnection conn)
        {
            Dictionary<string, int> Qsource = new Dictionary<string, int>(); ;

            //SqlConnection conn = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand("SELECT CodeID, CodeValue FROM UAD_Lookup..Code WHERE CodeTypeID = (SELECT CodeTypeId FROM UAD_Lookup..CodeType where CodeTypeName = 'Qualification Source')", conn);

            cmd.Connection.Open();
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (rdr.Read())
            {
                Qsource.Add(rdr["CodeValue"].ToString(), Convert.ToInt32(rdr["CodeID"]));
            }

            conn.Close();

            return Qsource;
        }
        #endregion
        public static void CreateCSVFromDataTable(DataTable dt, string createFileName, bool deleteExisting = true)
        {
            #region File Setup
            createFileName = createFileName.Replace(@"/", "");
            string origFile = createFileName;
            if (deleteExisting == true)
            {
                if (File.Exists(origFile))
                    File.Delete(origFile);
            }
            System.IO.FileInfo file = new System.IO.FileInfo(origFile);
            file.Directory.Create();

            FileConfiguration fileConfig = new FileConfiguration()
            {
                FileColumnDelimiter = "comma",
                IsQuoteEncapsulated = true
            };
            #endregion
            #region Variables

            char delim = ',';
            ColumnDelimiter delimiter = GetColumnDelimiter(fileConfig.FileColumnDelimiter.ToLower());
            if (delimiter == ColumnDelimiter.comma)
                delim = ',';
            else if (delimiter == ColumnDelimiter.semicolon)
                delim = ';';
            else if (delimiter == ColumnDelimiter.tab)
                delim = '\t';
            else if (delimiter == ColumnDelimiter.colon)
                delim = ':';
            else if (delimiter == ColumnDelimiter.tild)
                delim = '~';
            string appendString = "\"" + delim + "\"";
            List<string> origList = new List<string>();

            StringBuilder sbOrigFile = new StringBuilder();
            if (fileConfig.IsQuoteEncapsulated)
                sbOrigFile.Append('"');
            #endregion
            #region Add Headers
            if (dt != null && !File.Exists(origFile))
            {
                foreach (DataColumn drO in dt.Columns)
                {
                    //create the headers
                    if (fileConfig.IsQuoteEncapsulated)
                        sbOrigFile.Append(drO.ColumnName + appendString);
                    else
                        sbOrigFile.Append(drO.ColumnName + delim);
                }
                if (fileConfig.IsQuoteEncapsulated)
                    origList.Add(sbOrigFile.ToString().TrimEnd('"').TrimEnd(delim));
                else
                    origList.Add(sbOrigFile.ToString().TrimEnd(delim));


                File.WriteAllLines(origFile, origList);
                origList = new List<string>();
            }
            #endregion
            #region Add Data
            foreach (DataRow otl in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (Type.GetTypeCode(dc.DataType) == TypeCode.String && dc.ReadOnly == false)
                        otl[dc.ColumnName] = otl[dc.ColumnName].ToString().Replace('\r', ' ').Replace('\n', ' ');
                }
            }

            foreach (DataRow otl in dt.Rows)
            {
                origList = new List<string>();
                //create the orginal valid file
                if (fileConfig.IsQuoteEncapsulated)
                    origList.Add('"' + string.Join(appendString, otl.ItemArray.Select(p => p.ToString().Replace('\r', ' ').Replace('\n', ' ').Trim().TrimEnd('\r', '\n').Replace("\"", "")).ToArray()) + '"');//.TrimEnd('"'));//.TrimEnd(delim));
                else
                    origList.Add(string.Join(delim.ToString(), otl.ItemArray.Select(p => p.ToString().Replace('\r', ' ').Replace('\n', ' ').Trim().TrimEnd('\r', '\n')).ToArray()));

                File.AppendAllLines(origFile, origList);
            }
            #endregion
        }

        public static ColumnDelimiter GetColumnDelimiter(string delimiter)
        {
            try
            {
                return (ColumnDelimiter)System.Enum.Parse(typeof(ColumnDelimiter), delimiter, true);
            }
            catch { return ColumnDelimiter.comma; }
        }
    }

    public class FileConfiguration
    {
        public FileConfiguration() { }

        public string FileFolder { get; set; }
        public string FileExtension { get; set; }
        public bool IsQuoteEncapsulated { get; set; }
        public string FileColumnDelimiter { get; set; }
        public int ColumnCount { get; set; }
        public string ColumnHeaders { get; set; }
    }

    public enum ColumnDelimiter
    {
        comma,
        tab,
        semicolon,
        colon,
        tild
    }
}
