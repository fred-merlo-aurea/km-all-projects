using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;
using System.Web;
using System.Data.OleDb;		// for reading Excel file
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text;
using ECN_Framework_Entities.Communicator;
using System.Net;
using System.Net.Mail;
using ECN_Framework_Common.Functions;
using ECN_Framework_DataLayer;
using KM.Common;
using KM.Common.Extensions;

namespace ecn.communicator.engines
{
    class DataImporter
    {
        private const string TextFileExtension = ".txt";
        private const string ExcelFileExtension = ".xls";
        private const string XmlFileExtension = ".xml";
        private const string DefaultImporterId = "4";
        private const string EngineStarted = "---Started the engine-------";
        private const string EcnUserAccessKey = "ECNUserAccessKey";
        private const string GetImportersList = "Getting the importers list";
        private const string ChannelIdPattern = "%%channelID%%";
        private const string CustomerIdPattern = "%%customerID%%";
        private const string Sftp = "SFTP";
        private const string ConnectSftpSite = "* Connecting to SFTP site";
        private const string Success = "succeded";
        private const string NoFileToDownload = "* [ATTENTION] :: NO FILE TO DOWNLOAD";
        private const string Ftp = "FTP";
        private const string ConnectFtpSite = "* Connecting to FTP site";
        private const string FtpLoggedIn = "* FTP Logged in";
        private const string FtpFilesRetrieved = "* list of files retrieved from FTP location";
        private const string DownloadComplete = "* Downloaded Complete";
        private const string FtpConnectionClosed = "* FTP Connection Closed";
        private const string ColumnNamesEmailsTable = "* ColumnNames extraced from Emails Table";
        private const string NameDistinct = "DISTINCT";
        private const string Space = " ";
        private const string NameSelect = "SELECT";
        private const string SelectTop1 = "SELECT TOP 1";
        private const string ColumnNameExternalDatabase = "* ColumnNames extraced from External DB Table File";
        private const string End = "-END-----------------------------------------";
        private const string KmCommonApplication = "KMCommon_Application";
        private const string NoImportsToRun = "No imports to run";
        public static string importerID = "";
        public static string server = ""; // FTP / DB server
        public static string user = ""; // User Name to connect to the server
        public static string pass = ""; // Password to connect to the server 
        public static string changeDir_sqlQuery = "";
        public static string channelID = "";
        public static string customerID = "";
        public static string table_fileToImport = "";
        public static string groupID = "";
        public static string appendUDFData = "";
        public static string importName = "";
        public static string importType = ""; // FTP (OR) DataBase - sql server / oracle etc.,
        public static string db_fileToImport = ""; // Database / xls file
        public static string tbl_sheetToImport = "";
        public static string importFrequency = "";
        public static string importSetting = "";
        public static string activeStatus = "";
        public static string dateAdded = "";
        public static string dateUpdated = "";
        public static string adminEmail = "";
        public static string log = "";
        public static string fileToDownload = "";
        public static string custDownloadPath = "";
        public static string xlsFilePath = "";
        public static string LogFile = "";
        public static string SecureConnectionData = "";

        public static List<string> ext_FIL_TBL_ColumnHeadings;
        public static List<string> emailsColumnHeadings;
        public static ArrayList filesArrayList;
        public static string[] filesList;
        public static IEnumerator xlsColListEnum = null;
        public static IEnumerator emailColumns = null;
        public static IEnumerator txtColListEnum = null;
        public static string dbConnStr = "";
        public static string commDB = "";

        public static StreamWriter logFile;

        public static ECN_Framework_Common.Functions.FtpFunctions ftpClient;
        public static Hashtable hUpdatedRecords = new Hashtable();

        public static KMPlatform.Entity.User ECNuser = new KMPlatform.Entity.User();


        private static string dt = System.DateTime.Now.ToString("MM/dd/yyyy");
        private static string time = System.DateTime.Now.ToString("HH") + ":00:00";

        public static void Main(string[] args)
        {
            const string CurrentMethodName = "ECNExternalSrcDataImport.Main";

            try
            {
                try
                {
                    importerID = args[0];
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.ToString());
                    importerID = DefaultImporterId;
                }

                Console.WriteLine(EngineStarted);
                ECNuser = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings[EcnUserAccessKey], false);

                Console.WriteLine(GetImportersList);
                var importers = GetImporters();

                Console.WriteLine(importers.Count.ToString());
                log = Path.Combine(Path.Combine(Environment.CurrentDirectory, "log"), $"{dt.Replace("/", "_")}_{time.Replace(":", "_")}.log");

                if (!Directory.Exists($"{Environment.CurrentDirectory}\\log"))
                {
                    Directory.CreateDirectory($"{Environment.CurrentDirectory}\\log");
                }

                logFile = new StreamWriter(new FileStream(log, FileMode.Append));

                if (importers.Count > 0)
                {
                    ProcessImporters(importers, CurrentMethodName);

                    logFile.Close();
                }
                else
                {
                    writeToLog(NoImportsToRun);
                    logFile.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                SendNotification(ex.Message);
            }
        }

        private static void ProcessImporters(List<int> importers, string currentMethodName)
        {
            foreach (var impId in importers)
            {
                try
                {
                    Console.WriteLine($"Started for import: {impId}");

                    extractJobSettings();
                    extractImportSettings(impId.ToString());

                    Console.WriteLine($"Extracted all the settings for import : {impId}");

                    custDownloadPath = custDownloadPath.Replace(ChannelIdPattern, channelID)
                        .Replace(CustomerIdPattern, customerID);

                    if (!Directory.Exists(custDownloadPath))
                    {
                        Directory.CreateDirectory(custDownloadPath);
                    }

                    writeToLog(string.Empty);
                    writeToLog(string.Empty);

                    Console.WriteLine($"Start writing to log for path : {log}");
                    writeToLog($"{DateTime.Now}-START--------------------------------------");

                    if (importType.EqualsIgnoreCase(Sftp))
                    {
                        ImportTypeSftp();
                    }
                    else if (importType.EqualsIgnoreCase(Ftp))
                    {
                        ImportTypeFtp();
                    }
                    else
                    {
                        if (changeDir_sqlQuery.Length > 0)
                        {
                            extractCoumnNamesFromEmailsTable();
                            writeToLog(ColumnNamesEmailsTable);

                            var tempChangeDirSqlQuery = changeDir_sqlQuery;
                            tempChangeDirSqlQuery = tempChangeDirSqlQuery.Replace(NameDistinct, Space);
                            tempChangeDirSqlQuery = tempChangeDirSqlQuery.Replace(NameSelect, SelectTop1);
                            extractCoumnNamesFromDBTable(tempChangeDirSqlQuery);
                            writeToLog(ColumnNameExternalDatabase);

                            extractDataFromFileAndImport(string.Empty, string.Empty);
                        }
                    }

                    writeToLog(End);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred:  {ex.Message}");
                    writeToLog($"Error Importing : {impId}");
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, currentMethodName, Convert.ToInt32(ConfigurationManager.AppSettings[KmCommonApplication]));
                }
            }
        }

        private static void ImportTypeFtp()
        {
            writeToLog(ConnectFtpSite);
            ftpClient = new FtpFunctions(server, user, pass, string.Empty);
            writeToLog(FtpLoggedIn);

            if (changeDir_sqlQuery.Length != 0)
            {
                ftpClient = new FtpFunctions($"{server}/{changeDir_sqlQuery}", user, pass, string.Empty);
            }

            filesList = getFilesList(db_fileToImport);
            filesArrayList = new ArrayList();

            for (var j = 0; j < filesList.Length; j++)
            {
                if (filesList[j].Length != 0)
                {
                    filesArrayList.Add(filesList[j]);
                }
            }

            writeToLog(FtpFilesRetrieved);
            filesArrayList.Sort();

            fileToDownload = filesArrayList.Count != 0
                                 ? filesArrayList[filesArrayList.Count - 1].ToString()
                                 : string.Empty;

            writeToLog($"* File to download from FTP location: {fileToDownload}");
            writeToLog($"* File Path where to store {fileToDownload}.ToUpper() : {custDownloadPath}");

            if (fileToDownload.Length != 0)
            {
                var fileToSave = fileToDownload;
                var exts = fileToDownload.Split('.');

                if (exts.Length >= 2)
                {
                    fileToSave = $"{exts[exts.Length - 2]}_{DateTime.Now:MMddyyyy}.{exts[exts.Length - 1]}";
                }

                fileToSave = downloadFile(fileToDownload, fileToSave);
                writeToLog(DownloadComplete);
                writeToLog(FtpConnectionClosed);

                ImportFile(fileToSave, db_fileToImport);
            }
            else
            {
                var result = NoFileToDownload;
                writeToLog(result);
                writeToLog(FtpConnectionClosed);
                SendNotification(result, adminEmail);
            }
        }

        private static void ImportTypeSftp()
        {
            writeToLog(ConnectSftpSite);

            ftpClient = new ECN_Framework_Common.Functions.FtpFunctions(server, user, pass, SecureConnectionData);

            var remoteFilePath = $"{changeDir_sqlQuery.TrimEnd('/')}/{db_fileToImport.TrimStart('/')}";
            var saveFilePath = db_fileToImport;
            var exts = db_fileToImport.Split('.');

            if (exts.Length >= 2)
            {
                saveFilePath = $"{exts[exts.Length - 2]}_{DateTime.Now:MMddyyyy}.{exts[exts.Length - 1]}";
            }

            var result = downloadFile_SFTP(remoteFilePath, ref saveFilePath);

            if (!result.Contains(Success))
            {
                ftpClient.CloseSFTPSession();
                writeToLog(NoFileToDownload);
                SendNotification(result, adminEmail);
            }
            else
            {
                ImportFile(saveFilePath, db_fileToImport, true);
            }
        }

        private static List<int> GetImporters()
        {
            List<int> importerIds = new List<int>();

            SqlCommand cmdimport = new SqlCommand();
            cmdimport.CommandType = CommandType.StoredProcedure;
            cmdimport.CommandText = "spGetDataImportEngines";
            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(cmdimport, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());

            foreach (DataRow dr in dt.Rows)
                importerIds.Add(Convert.ToInt32(dr["ImporterID"]));

            return importerIds;
        }

        private static void SendNotification(string messsage, string to = "default")
        {
            if (to == "default")
                to = ConfigurationManager.AppSettings["NOTIFICATION_EMAIL"].ToString();
            string from = ConfigurationManager.AppSettings["NOTIFICATION_EMAIL_FROM"].ToString();

            string subject = "Alert : Data import Engine.";
            string body = "Error with Data Import Engine. Details: " + messsage;
            string[] mails = to.Split(',');

            SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTP_SERVER"].ToString());
            MailMessage message = new MailMessage();
            for (int i = 0; i < mails.Length; i++)
            {
                message.To.Add(mails[i]);
            }
            
            message.From = new MailAddress(from);
            message.Body = body;
            message.Subject = subject;
            smtp.Send(message);
            //email.SimpleSend(to,from,subject,body);

        }

        #region extract the values from the XML settings file for Job Settings
        //extract the values from the XML settings file
        private static void extractJobSettings()
        {
            dbConnStr = ConfigurationManager.AppSettings["connString"].ToString();
            commDB = ConfigurationManager.AppSettings["commDB"].ToString();
            custDownloadPath = ConfigurationManager.AppSettings["custDownloadPath"].ToString();
        }
        #endregion

        #region extract the values from the UDFImportSchedule Table for the ImportID
        //extract the values from the UDFImportSchedule Table for the ImportID
        private static void extractImportSettings(string importerID)
        {
            string sqlQuery = " SELECT * " +
                " FROM UDFImportSchedule " +
                " WHERE ImporterID = " + importerID;

            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sqlQuery, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());
            foreach (DataRow dr in dt.Rows)
            {
                channelID = dr["ChannelID"].ToString();
                customerID = dr["CustomerID"].ToString();
                groupID = dr["ImportGroupID"].ToString();
                appendUDFData = dr["AppendData"].ToString();
                importName = dr["ImportName"].ToString();
                importType = dr["ImportType"].ToString();
                server = dr["SiteAddress"].ToString();
                user = dr["UserName"].ToString();
                pass = dr["Password"].ToString();
                changeDir_sqlQuery = dr["Directory_Query"].ToString();
                db_fileToImport = dr["Database_FileName"].ToString();
                tbl_sheetToImport = dr["Table_Sheet"].ToString();
                importFrequency = dr["ImportFrequency"].ToString();
                importSetting = dr["ImportSetting"].ToString();
                table_fileToImport = dr["ImportDateTime"].ToString();
                activeStatus = dr["Active"].ToString();
                dateAdded = dr["DateAdded"].ToString();
                dateUpdated = dr["DateUpdated"].ToString();
                adminEmail = dr["AdminEmail"].ToString();
                try
                {
                    SecureConnectionData = dr["SecureConnectionData"].ToString();
                }
                catch { }
            }
        }
        #endregion

        #region extract Column Names from From EmailsTable
        //extract Column Names from From XLSFile / Table
        public static void extractCoumnNamesFromEmailsTable()
        {
            string sqlString = "select TOP 1 * from Emails";
            DataTable dataTable = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sqlString, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());
            emailColumns = dataTable.Columns.GetEnumerator();
            emailsColumnHeadings = new List<string>();
            while (emailColumns.MoveNext())
            {
                emailsColumnHeadings.Add(emailColumns.Current.ToString().ToLower());
            }

            emailsColumnHeadings.Add("subscribetype");
            //for (int i = 0; i < emailsColumnHeadings.Count; i++)
            //{
            //    emailsColumnHeadings[i] = emailsColumnHeadings[i].ToString().ToLower();
            //}

            writeToLog("* # of Columns in Emails Table : " + emailsColumnHeadings.Count);
        }
        #endregion

        #region extract Column Names from From TXTFile / XLSFile / SQL Table

        public static void extractColumnNamesFromTXTFile(string path, string fileName, string delimiter)
        {
            writeToLog("* IN extractColumnNamesFromTXTFile : ");

            string filedelimiter = ",";

            ext_FIL_TBL_ColumnHeadings = new List<string>();

            DataSet ds = new DataSet();

            try
            {
                if (delimiter.ToLower() == "tab")
                    filedelimiter = "\t";

                //Open the file in a stream reader.
                StreamReader s = new StreamReader(path + fileName);

                //Split the first line into the columns       
                string[] columns = s.ReadLine().Split(filedelimiter.ToCharArray());
                ds.Tables.Add();

                foreach (string col in columns)
                {
                    //add only not exists. 
                    if (!ds.Tables[0].Columns.Contains(col))
                        ds.Tables[0].Columns.Add(col);
                }

                int countColumns = ds.Tables[0].Columns.Count;
                //int totalRecs = ds.Tables[0].Rows.Count;

                IEnumerator txtFileHeaderEnum = ds.Tables[0].Columns.GetEnumerator();

                while (txtFileHeaderEnum.MoveNext())
                {
                    ext_FIL_TBL_ColumnHeadings.Add(txtFileHeaderEnum.Current.ToString().ToLower());
                }


                //for (int i = 0; i < ext_FIL_TBL_ColumnHeadings.Count; i++)
                //{
                //    ext_FIL_TBL_ColumnHeadings[i] = ext_FIL_TBL_ColumnHeadings[i].ToString().ToLower();
                //}

                writeToLog("* # of Columns in XLS file : " + ext_FIL_TBL_ColumnHeadings.Count);
                //writeToLog("* # of Records in XLS file : " + totalRecs);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "extractColumnNamesFromTXTFile", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                writeToLog("* [ERROR] IN extractColumnNamesFromXLSFile : " + ex.Message);
                string exx = ex.ToString();
            }
        }

        //extract Column Names from From XLSFile / Table
        public static void extractColumnNamesFromXLSFile(string file_db, string tbl_sheet)
        {
            writeToLog("* IN extractColumnNamesFromXLSFile : ");

            //get the full path for the file
            if (importType.Equals("FTP"))
            {
                tbl_sheet = "[" + tbl_sheet + "$]";
            }

            string connString = "";
            ext_FIL_TBL_ColumnHeadings = new List<string>();
            try
            {
                connString = getExtDatasource(file_db);
                OleDbDataAdapter oleAdapter = new OleDbDataAdapter("SELECT * FROM " + tbl_sheet, connString);
                DataSet dataset = new DataSet();
                oleAdapter.Fill(dataset, "ExcelTable"); //fill the adapter with rows only from the linenumber specified
                int countColumns = dataset.Tables["ExcelTable"].Columns.Count;
                int totalRecs = dataset.Tables["ExcelTable"].Rows.Count;
                DataTable dt = dataset.Tables["ExcelTable"];
                IEnumerator xlsColumnHeadingENum = ECN_Framework_Common.Functions.DataTableFunctions.GetDataTableColumns(dt).GetEnumerator();
                while (xlsColumnHeadingENum.MoveNext())
                {
                    ext_FIL_TBL_ColumnHeadings.Add(xlsColumnHeadingENum.Current.ToString().ToLower());
                }


                //for (int i = 0; i < ext_FIL_TBL_ColumnHeadings.Count; i++)
                //{
                //    ext_FIL_TBL_ColumnHeadings[i] = ext_FIL_TBL_ColumnHeadings[i].ToString().ToLower();
                //}

                writeToLog("* # of Columns in XLS file : " + ext_FIL_TBL_ColumnHeadings.Count);
                writeToLog("* # of Records in XLS file : " + totalRecs);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "extractColumnNamesFromXLSFile", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                writeToLog("* [ERROR] IN extractColumnNamesFromXLSFile : " + ex.Message);
                string exx = ex.ToString();
            }
        }

        public static void extractColumnNamesFromXMLFile(string file_db)
        {
            ext_FIL_TBL_ColumnHeadings = new List<string>();
            try
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml(file_db);
                int countColumns = dataset.Tables[0].Columns.Count;
                int totalRecs = dataset.Tables[0].Rows.Count;
                DataTable dt = dataset.Tables[0];
                txtColListEnum = ECN_Framework_Common.Functions.DataTableFunctions.GetDataTableColumns(dt).GetEnumerator();

                while (txtColListEnum.MoveNext())
                {
                    ext_FIL_TBL_ColumnHeadings.Add(txtColListEnum.Current.ToString().ToLower());
                }

                //for (int i = 0; i < ext_FIL_TBL_ColumnHeadings.Count; i++)
                //{
                //    ext_FIL_TBL_ColumnHeadings[i] = ext_FIL_TBL_ColumnHeadings[i].ToString().ToLower();
                //}

                writeToLog("* # of Columns in XML file : " + ext_FIL_TBL_ColumnHeadings.Count);
                writeToLog("* # of Records in XML file : " + totalRecs);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "extractColumnNamesFromXMLFile", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                string exx = ex.ToString();
            }
        }

        public static void extractCoumnNamesFromDBTable(string sql)
        {
            //get the full path for the file
            string connString = "";
            ext_FIL_TBL_ColumnHeadings = new List<string>();
            try
            {
                connString = getExtDatasource("");
                string sqlString = sql;
                DataTable dataTable = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sqlString, connString);
                IEnumerator dbTableEnum = ECN_Framework_Common.Functions.DataTableFunctions.GetDataTableColumns(dataTable).GetEnumerator();

                while (dbTableEnum.MoveNext())
                {
                    ext_FIL_TBL_ColumnHeadings.Add(dbTableEnum.Current.ToString().ToLower());
                }

                //for (int i = 0; i < ext_FIL_TBL_ColumnHeadings.Count; i++)
                //{
                //    ext_FIL_TBL_ColumnHeadings[i] = ext_FIL_TBL_ColumnHeadings[i].ToString().ToLower();
                //}

                writeToLog("* # of Columns in Emails Table : " + ext_FIL_TBL_ColumnHeadings.Count);
                writeToLog("* # of Columns in DB Table file : " + ext_FIL_TBL_ColumnHeadings.Count);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "extractColumnNamesFromDBTable", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                string exx = ex.ToString();
            }
        }
        #endregion

        #region compare ColumnNames with UDFs in Group
        public static bool CompareColumnsWithEmails_UDFTable(string groupID)
        {
            bool compareGood = false;

            ECN_Framework_Entities.Communicator.Group gs = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(Convert.ToInt32(groupID));

            Hashtable UDFHash = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetGroupUDFHashtable_NoAccessCheck(gs.GroupID);
            Hashtable finalUDFHash = new Hashtable();
            foreach (DictionaryEntry de in UDFHash)
            {
                finalUDFHash.Add(de.Key, de.Value.ToString().ToLower());//UDFHash.SetByIndex(i, UDFHash.GetByIndex(i).ToString().ToLower());
            }

            xlsColListEnum = ext_FIL_TBL_ColumnHeadings.GetEnumerator();
            while (xlsColListEnum.MoveNext())
            {
                string current = xlsColListEnum.Current.ToString().ToLower();

                if (!(current.StartsWith("user_")))
                {
                    if (emailsColumnHeadings.Contains(current.ToString()))
                    {
                        compareGood = true;
                    }
                    else
                    {
                        writeToLog("* [ERROR] ColumnNames did not match the Emails Table - " + current);
                        compareGood = false;
                        break;
                    }
                }
                else
                {
                    current = current.Replace("user_", "");
                    if (finalUDFHash.ContainsValue(current.ToString()))
                    {
                        compareGood = true;
                    }
                    else
                    {
                        writeToLog("* [ERROR] ColumnNames did not match the UDF table - " + current);
                        compareGood = false;
                        break;
                    }
                }
            }

            return compareGood;
        }
        #endregion

        # region IMPORT DATA

        #region extract Data From XML File and Import in to ECN
        private static void extractDataFromXMLFileAndImport(string file_db)
        {
            DataSet dataset = new DataSet();
            dataset.ReadXml(file_db);
            DataTable dt = dataset.Tables[0];

            ImportData(dt);

        }
        #endregion

        #region extract Data From XLSFile and Import in to ECN
        //extract Data From XLSFile and Import in to ECN


        public static void extractDataFromTXTFileAndImport(string path, string fileName, string delimiter)
        {
            int columncount = 0;
            int baddata = 0;

            string filedelimiter = ",";

            DataSet ds = new DataSet();
            try
            {
                if (delimiter.ToLower() == "tab")
                    filedelimiter = "\t";

                //Open the file in a stream reader.
                StreamReader s = new StreamReader(path + fileName);

                //Split the first line into the columns       
                string[] columns = s.ReadLine().Split(filedelimiter.ToCharArray());
                ds.Tables.Add();

                foreach (string col in columns)
                {
                    //add only not exists. 
                    if (!ds.Tables[0].Columns.Contains(col))
                        ds.Tables[0].Columns.Add(col);
                }
                columncount = ds.Tables[0].Columns.Count;


                //Read the data in the file.        
                string AllData = s.ReadToEnd();
                string[] rows = AllData.Split("\r\n".ToCharArray(),  StringSplitOptions.RemoveEmptyEntries);

                foreach (string row in rows)
                {
                    //Split the row at the delimiter.
                    string[] items = row.Split(filedelimiter.ToCharArray());

                    if (columncount != items.Length)
                    {
                        baddata++;
                        //writeToLog("* [Baddata count ]:" + items[0].ToString());
                    }
                    else
                    {
                        ds.Tables[0].Rows.Add(items);
                    }
                }
                writeToLog("* [Baddata count ]:" + baddata);
                ImportData(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "extractDataFromTXTFileAndImport", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                writeToLog("* [ERROR]:Error occured while fetching data from the TXT File." + ex.ToString());
            }
        }

        public static void extractDataFromFileAndImport(string file_db, string tbl_sheet)
        {

            DataTable dt = new DataTable();
            try
            {
                if (importType.Equals("FTP"))
                {
                    tbl_sheet = "[" + tbl_sheet + "$]";

                    OleDbDataAdapter oleAdapter = new OleDbDataAdapter("SELECT * FROM " + tbl_sheet, getExtDatasource(file_db));
                    DataSet dataset = new DataSet();

                    oleAdapter.Fill(dataset, "ExcelTable"); //fill the adapter with rows only from the linenumber specified

                    dt = dataset.Tables["ExcelTable"];
                }
                else
                {
                    dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(changeDir_sqlQuery, getExtDatasource(""));
                }

                ImportData(dt);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "extractDataFromFileAndImport", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                writeToLog("* [ERROR]:Error occured while fetching data from the Excel File / DB server." + ex.ToString());
            }
        }
        #endregion

        public static void ImportData(DataTable dtFile)
        {
            StringBuilder xmlUDF = new StringBuilder("");
            StringBuilder xmlProfile = new StringBuilder("");

            writeToLog("* Total # of Records : " + dtFile.Rows.Count);

            DateTime startDateTime = DateTime.Now;
            try
            {
                Hashtable hGDFFields = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetGroupUDFHashtable_NoAccessCheck(Convert.ToInt32(groupID));
                Hashtable finalUDFHash = new Hashtable();
                foreach (DictionaryEntry de in hGDFFields)
                {
                    finalUDFHash.Add("user_" + de.Value.ToString().ToLower(), de.Key);//UDFHash.SetByIndex(i, UDFHash.GetByIndex(i).ToString().ToLower());
                }
                bool bRowCreated = false;

                for (int cnt = 0; cnt < dtFile.Rows.Count; cnt++)
                {

                    DataRow drFile = dtFile.Rows[cnt];

                    bRowCreated = false;

                    xmlProfile.Append("<Emails>");

                    foreach (DataColumn dcFile in dtFile.Columns)
                    {
                        if (dcFile.ColumnName.ToLower().IndexOf("user_") == -1)
                        {
                            if (dcFile.ColumnName.ToLower().Equals("subscribetype"))
                            {
                                xmlProfile.Append("<subscribetypecode>" + CleanXMLString(drFile[dcFile.ColumnName].ToString()) + "</subscribetypecode>");
                            }
                            else
                            {
                                if (drFile[dcFile.ColumnName].ToString().Length > 0)
                                {
                                    xmlProfile.Append("<" + dcFile.ColumnName.ToLower() + ">" + CleanXMLString(drFile[dcFile.ColumnName].ToString()) + "</" + dcFile.ColumnName.ToLower() + ">");
                                }
                            }
                        }

                        if (hGDFFields.Count > 0)
                        {
                            if (dcFile.ColumnName.ToLower().IndexOf("user_") > -1)
                            {
                                if (!bRowCreated)
                                {
                                    xmlUDF.Append("<row>");
                                    xmlUDF.Append("<ea>" + CleanXMLString(drFile["emailaddress"].ToString()) + "</ea>");
                                    bRowCreated = true;
                                }

                                if (drFile[dcFile.ColumnName.ToLower()].ToString().Length > 0)
                                {
                                    xmlUDF.Append("<udf id=\"" + finalUDFHash[dcFile.ColumnName.ToLower()].ToString() + "\">");

                                    xmlUDF.Append("<v><![CDATA[" + CleanXMLString(drFile[dcFile.ColumnName.ToLower()].ToString()) + "]]></v>");

                                    xmlUDF.Append("</udf>");
                                }
                            }
                        }
                    }
                    xmlProfile.Append("</Emails>");

                    if (bRowCreated)
                        xmlUDF.Append("</row>");

                    if ((cnt != 0) && (cnt % 5000 == 0) || (cnt == dtFile.Rows.Count - 1))
                    {

                        UpdateToDB(Convert.ToInt32(customerID), Convert.ToInt32(groupID), "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>", false, ECNuser);
                        writeToLog(cnt + " Completed.");

                        xmlProfile = new StringBuilder("");
                        xmlUDF = new StringBuilder("");
                    }

                }
                hGDFFields.Clear();

                if (hUpdatedRecords.Count > 0)
                {
                    foreach (DictionaryEntry de in hUpdatedRecords)
                    {
                        if (de.Key.ToString() == "T")
                            writeToLog("Total Records in the File = " + de.Value.ToString());
                        else if (de.Key.ToString() == "I")
                            writeToLog("New = " + de.Value.ToString());
                        else if (de.Key.ToString() == "U")
                            writeToLog("Changed = " + de.Value.ToString());
                        else if (de.Key.ToString() == "D")
                            writeToLog("Duplicate(s) = " + de.Value.ToString());
                        else if (de.Key.ToString() == "S")
                            writeToLog("Skipped = " + de.Value.ToString());
                        else if (de.Key.ToString() == "M")
                            writeToLog("Master Suppressed = " + de.Value.ToString());
                    }
                    hUpdatedRecords = new Hashtable();
                    TimeSpan duration = DateTime.Now - startDateTime;

                    writeToLog("Time to Import : " + duration.Hours + ":" + duration.Minutes + ":" + duration.Seconds);
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "ImportData", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                writeToLog("ERROR: " + ex.Message);
            }
        }

        private static string CleanXMLString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text;
        }

        private static Hashtable GetGroupDataFields(int groupID)
        {
            string sqlstmt = " SELECT * FROM GroupDatafields WHERE GroupID=" + groupID;

            DataTable emailstable = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sqlstmt, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());

            Hashtable fields = new Hashtable();
            foreach (DataRow dr in emailstable.Rows)
                fields.Add("user_" + dr["ShortName"].ToString().ToLower(), Convert.ToInt32(dr["GroupDataFieldsID"]));

            return fields;
        }

        private static void UpdateToDB(int CustomerID, int GroupID, string xmlProfile, string xmlUDF, bool EmailaddressOnly, KMPlatform.Entity.User ECNuser)
        {

            DataTable dtRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(ECNuser, CustomerID, GroupID, xmlProfile, xmlUDF, "HTML", "S", EmailaddressOnly, "", "ECNExternalSrcDataImport.UpdateToDB");

            if (dtRecords.Rows.Count > 0)
            {
                foreach (DataRow dr in dtRecords.Rows)
                {
                    if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                        hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(), Convert.ToInt32(dr["Counts"]));
                    else
                    {
                        int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                        hUpdatedRecords[dr["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr["Counts"]);
                    }
                }

            }
        }

        #endregion


        //static bool DownloadFile(string downloadFile, string saveFile)
        //{
        //    string saveFilePath = "";
        //    saveFile = saveFile.Replace("/", "-");
        //    saveFile = saveFile.Replace(" ", "-");
        //    saveFile = saveFile.Replace(":", "-");
        //    saveFilePath = custDownloadPath + saveFile;

        //    WebClient request = new WebClient();
        //    request.Credentials = new NetworkCredential(ConfigurationSettings.AppSettings["username"].ToString(), ConfigurationSettings.AppSettings["password"].ToString());

        //    byte[] fileData = request.DownloadData(ConfigurationSettings.AppSettings["ftpServer"].ToString() + ConfigurationSettings.AppSettings["ImportFileName"].ToString());
        //    writeToLog("Download Done : File Size: " + fileData.Length);
        //    if (fileData.Length > 0)
        //    {
        //        if (!Directory.Exists(custDownloadPath))
        //            Directory.CreateDirectory(custDownloadPath);

        //        if (File.Exists(custDownloadPath + saveFilePath))
        //            File.Delete(custDownloadPath + saveFilePath);

        //        writeToLog("Creating File: " + custDownloadPath + saveFilePath);
        //        FileStream fs = File.Create(saveFilePath + saveFilePath);
        //        fs.Write(fileData, 0, fileData.Length);
        //        fs.Close();
        //        writeToLog("File Successfully Created: " + DateTime.Now);
        //        return true;
        //    }
        //    else
        //        return false;
        //}

        #region Download the xls file
        //Download the xls file
        public static string downloadFile(string downloadFile, string saveFile)
        {
            string saveFilePath = "";
            try
            {
                saveFile = saveFile.Replace("/", "-");
                saveFile = saveFile.Replace(" ", "-");
                saveFile = saveFile.Replace(":", "-");
                saveFilePath = custDownloadPath + saveFile;

                if (File.Exists(custDownloadPath + saveFile))
                {
                    File.Delete(custDownloadPath + saveFile);
                }

                ftpClient.Download(downloadFile, saveFilePath);

                try
                {
                    ftpClient.Delete(downloadFile);
                }
                catch (Exception ex)
                {
                    writeToLog("* [ERROR] Exception occured When Deleting File(): \n" + ex);
                }

            }
            catch (Exception ex)
            {
                writeToLog("* [ERROR] Exception occured in downloadFile(): \n" + ex);
            }
            return saveFilePath;
        }

        public static string downloadFile_SFTP(string downloadFile, ref string saveFile)
        {
            string saveFilePath = "";
            string message = "";
            try
            {
                saveFile = saveFile.Replace("/", "-");
                saveFile = saveFile.Replace(" ", "-");
                saveFile = saveFile.Replace(":", "-");
                saveFilePath = custDownloadPath + saveFile;

                if (File.Exists(custDownloadPath + saveFile))
                {
                    File.Delete(custDownloadPath + saveFile);
                }

                message = ftpClient.Download_SFTP(downloadFile, saveFilePath, false);
                saveFile = saveFilePath;
                return message;
            }
            catch (Exception ex)
            {
                
                return ex.Message;
            }
        }
        #endregion

        #region list the XLS files
        //Upload the XLS file
        public static string[] getFilesList(string mask)
        {
            String[] list = new string[] { };
            try
            {
                list = ftpClient.DirectoryListSimple(mask);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "getFilesList", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()));
                writeToLog("* [ERROR] Exception occured in getFilesList(): \n" + ex);
            }
            return list;
        }
        #endregion

        #region get the Connection String to connect to XLS file
        //get the Connection String to connect to XLS file
        public static string getExtDatasource(string file)
        {
            string dataSrc = "";

            switch (importType)
            {
                case "FTP":
                    dataSrc = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + file + ";Extended Properties='Excel 8.0;HDR=Yes;'";
                    break;
                case "SQLServer":
                    dataSrc = "server=" + server + ";uid=" + user + ";pwd=" + pass + ";database=" + db_fileToImport;
                    break;
                default:
                    dataSrc = "";
                    break;
            }

            return dataSrc;
        }
        #endregion

        #region Write to log file.
        //Write to log file
        public static void writeToLog(string text)
        {
            logFile.AutoFlush = true;
            logFile.WriteLine(DateTime.Now + " >> " + text);
            logFile.Flush();
        }
        #endregion

        private static void ImportFile(string fileToSavePath, string fileToImportPath, bool closeFtpConnection = false)
        {
            const string columnExtractionInfoMessage = "* ColumnNames extracted from Emails Table";
            const string columnExtractionInfoMessageFormat = "* ColumnNames extracted from {0} File";
            const string columnMatchingInfoMessage = "* [ERROR] ColumnNames did not match the Emails Table / UDF table";

            extractCoumnNamesFromEmailsTable();
            writeToLog(columnExtractionInfoMessage);

            var columnsMatch = CompareColumnsWithEmails_UDFTable(groupID);
            var extension = Path.GetExtension(fileToImportPath) ?? String.Empty;
            if (extension.Equals(TextFileExtension, StringComparison.OrdinalIgnoreCase))
            {
                var fileName = fileToSavePath.Replace(custDownloadPath, String.Empty);
                extractColumnNamesFromTXTFile(custDownloadPath, fileName, importSetting);
                var message = String.Format(columnExtractionInfoMessageFormat, TextFileExtension.ToUpper());
                writeToLog(message);
                if (columnsMatch)
                {
                    extractDataFromTXTFileAndImport(custDownloadPath, fileName, importSetting);
                }
            }
            else if (extension.Equals(ExcelFileExtension, StringComparison.OrdinalIgnoreCase))
            {
                extractColumnNamesFromXLSFile(fileToSavePath, tbl_sheetToImport);
                var message = String.Format(columnExtractionInfoMessageFormat, ExcelFileExtension.ToUpper());
                writeToLog(message);
                if (columnsMatch)
                {
                    extractDataFromFileAndImport(fileToSavePath, tbl_sheetToImport);
                }
            }
            else if (extension.Equals(XmlFileExtension, StringComparison.OrdinalIgnoreCase))
            {
                extractColumnNamesFromXMLFile(fileToSavePath);
                var message = String.Format(columnExtractionInfoMessageFormat, XmlFileExtension.ToUpper());
                writeToLog(message);
                if (columnsMatch)
                {
                    extractDataFromXMLFileAndImport(fileToSavePath);
                }
            }

            if (columnsMatch)
            {
                return;
            }

            if (closeFtpConnection)
            {
                ftpClient.CloseSFTPSession();
            }
            writeToLog(columnMatchingInfoMessage);
        }
    }
}

