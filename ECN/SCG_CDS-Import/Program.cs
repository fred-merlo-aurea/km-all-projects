using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Net;
using System.Configuration;
using System.Xml.Serialization;
using System.Net.Mail;
using ECN_Framework_BusinessLayer.Accounts;
using ECN_Framework_DataLayer.Communicator;
using KM.Common;
using KMPlatform.BusinessLogic;
using KM.Common.Functions;

namespace SCG_CDS_Import
{
    class Program
    {
        public static readonly int MaxCacheSize = 2097152; // 2M bytes.
        public static readonly int BufferSize = 2048; // 2K bytes.
        private const string SeparatorLine = "________________________________________________________________________________________________________";
        private const string XmlFrame = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{0}</XML>";
        private const string EcnResultsHeader = "***ECN-Results***";
        private const string DataPackEndMarker = "*****************";
        private const string XmlProfileSectionMarker = "***XMLPROFILE***";
        private const string XmlUdfSectionMarker = "***XMLUDF***";
        private const string HtmlFormatType = "html";
        private const string SubscribeTypeCode = "S";
        private const string WriteXmlLogParamName = "WriteXMLtoLog";
        private const string INMFieldName = "INM";
        private const string WhiteSpace = " ";
        private const string SubFieldName = "SUB";
        private const string UpdFieldName = "UPD";
        private const string UptFieldName = "UPT";
        private const string BatchCountParamName = "BatchCount";
        private const string VDTFieldName = "VDT";
        private const string CNTFieldName = "CNT";
        private const string ZipFieldName = "ZIP";
        private const string IgnoreExceptionLogMessageTemplate = "{0}:{1} fired on {2} parse. Ignored.";
        private const string ECNFieldQDate = "qdate";
        private const string ECNFieldForZip = "forzip";
        private const string ECNFieldCat = "cat";
        private const string ECNFieldXAct = "xact";
        private const string ECNFieldDemo7 = "demo7";
        private const string ECNFieldFirstName = "firstname";
        private const string ECNFieldLastName = "lastname";
        private const string ECNFieldVoice = "voice";
        private const string ECNFieldFax = "fax";
        private const string ECNFieldMobile = "mobile";
        private const string ECNFieldEmail = "emailaddress";
        private const string ECNFieldSubscribeTypeCode = "subscribetypecode";
        private const string ECNFieldZip = "zip";
        private static int KMCommon_Application;
        private static Dictionary<string, int> dUDFFields = new Dictionary<string, int>();
        public static StreamWriter logFile;
        public static StreamWriter importFileLog;        
        private static bool isDownloadComplete;
        private static ImportFiles importFiles;
        private const string SCGDownloadFileErrorSubject = "SCG File Download Error";
        private const string AppKeyFTPUser = "FTPUser";
        private const string AppKeyFTPPassword = "FTPPassword";
        private static List<TransactionType> TransactionReport;

        static void Main(string[] args)
        {
            logFile = new StreamWriter(new FileStream(ConfigurationManager.AppSettings["LogPath"] + "SCG_CDS_Import_" + DateTime.Now.ToString("MM-dd-yyyy") + ".log", FileMode.Append));
            WriteToLog("");
            WriteToLog(DateTime.Now + "- START IMPORT");
            WriteToLog("");

            try
            {
                string path = ConfigurationManager.AppSettings["MappingFilePath"] + "\\FileName_GroupID_Mapping.xml";
                XmlSerializer serializer = new XmlSerializer(typeof(ImportFiles));//, new XmlRootAttribute("Mappings"));
                StreamReader reader = new StreamReader(path);
                importFiles = (ImportFiles)serializer.Deserialize(reader);
                if (importFiles.Files.Count > 0)
                {
                    AuthKeyCheck();

                    KMCommon_Application = -1;
                    int.TryParse(ConfigurationManager.AppSettings["KMCommon_Application"].ToString(), out KMCommon_Application);

                    bool doFileDownload = true;
                    bool.TryParse(ConfigurationManager.AppSettings["DownloadFiles"].ToString(), out doFileDownload);
                    if (doFileDownload == true)
                        DownloadFiles();
                    else
                        isDownloadComplete = true;

                    if (isDownloadComplete == true)
                    {
                        TransactionReport = new List<TransactionType>();
                        ProcessFiles();
                        CreateReport();
                    }
                }
                else
                {
                    KM.Common.EmailFunctions.NotifyAdmin("SCG_CDS-Import - EmptyXML", DateTime.Now.ToString() + " - " + "No data in xml file after Deserialization");
                    WriteToLog(DateTime.Now.ToString() + " - " + "SCG_CDS-Import - EmptyXML : No data in xml file after Deserialization");
                }
            }
            catch (Exception ex)
            {
                StringBuilder sbLog = new StringBuilder();
                sbLog.AppendLine("**********************");
                sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
                sbLog.AppendLine("-- Message --");
                if (ex.Message != null)
                    sbLog.AppendLine(ex.Message);
                sbLog.AppendLine("-- InnerException --");
                if (ex.InnerException != null)
                    sbLog.AppendLine(ex.InnerException.ToString());
                sbLog.AppendLine("-- Stack Trace --");
                if (ex.StackTrace != null)
                    sbLog.AppendLine(ex.StackTrace);
                sbLog.AppendLine("**********************");
                KM.Common.EmailFunctions.NotifyAdmin("SCG_CDS-Import", sbLog.ToString());
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SCG_CDS-Import.Program.Main", KMCommon_Application, "SCG_CDS-Import: Unhandled Exception", -1, -1);
                WriteToLog(sbLog.ToString());
            }
            finally
            {
                WriteToLog("");
                WriteToLog(DateTime.Now + "- END IMPORT");
                WriteToLog("");
            }
        }

        static void AuthKeyCheck()
        {
            foreach (ImportFile f in importFiles.Files)
            {
                if (string.IsNullOrEmpty(f.AuthKey))
                {


                    var custs = KMPlatform.BusinessLogic.User.GetByCustomerID(f.CustomerID);
                    foreach (var c in custs)
                    {
                        if (!string.IsNullOrEmpty(c.AccessKey.ToString()))
                        {
                            f.AuthKey = c.AccessKey.ToString();
                            break;
                        }
                    }
                }
            }
        }
        #region Step 1: Download Import File from FTP
        static void DownloadFiles()
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["FTPSite"].ToString());
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                // This example assumes the FTP site uses anonymous logon.
                string user = System.Configuration.ConfigurationManager.AppSettings["FTPUser"].ToString();
                string pass = System.Configuration.ConfigurationManager.AppSettings["FTPPassword"].ToString();
                request.Credentials = new NetworkCredential(user, pass);

                FtpWebResponse dirResponse = (FtpWebResponse)request.GetResponse();
                Stream dirResponseStream = dirResponse.GetResponseStream();
                StreamReader dirReader = new StreamReader(dirResponseStream);
                List<string> dirs = new List<string>();
                while (dirReader.EndOfStream == false)
                {
                    dirs.Add(dirReader.ReadLine());
                }
                string dirMsg = "File check complete, status " + dirResponse.StatusDescription;
                WriteToLog(dirMsg);
                dirReader.Close();
                dirResponse.Close();
                request.Abort();

                string ftpURL = ConfigurationManager.AppSettings["FTPSite"].ToString();
                string destPath = ConfigurationManager.AppSettings["FilePath"].ToString();
                foreach (string file in dirs)
                {
                    if (file.EndsWith(".TXT"))
                    {
                        DownLoad(ftpURL, file, destPath);

                        //delete file from FTP Site
                        string ftpFullPath = ftpURL + file;
                        FtpWebRequest rDelete = (FtpWebRequest)WebRequest.Create(ftpFullPath);
                        rDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                        rDelete.Credentials = new NetworkCredential(user, pass);
                        FtpWebResponse responseDelete = (FtpWebResponse)rDelete.GetResponse();

                        Stream streamDelete = responseDelete.GetResponseStream();
                        if (responseDelete.StatusCode != FtpStatusCode.FileActionOK)
                            KM.Common.EmailFunctions.NotifyAdmin("SCG File Download Error", "File not deleted from SCG FTP:: Status Code - " + responseDelete.StatusCode.ToString() + " :: Status Description - " + responseDelete.StatusDescription.ToString());

                        responseDelete.Close();
                    }
                }
                isDownloadComplete = true;
            }
            catch (Exception ex)
            {
                isDownloadComplete = false;
                var exceptionMessage = FileFunctions.BuildDownloadExceptionMessage(ex);
                WriteToLog(exceptionMessage);
                EmailFunctions.NotifyAdmin(SCGDownloadFileErrorSubject, exceptionMessage);
            }
        }

        static void WriteCacheToFile(MemoryStream downloadCache, string downloadPath, int cachedSize)
        {
            using (FileStream fileStream = new FileStream(downloadPath, FileMode.Append))
            {
                byte[] cacheContent = new byte[cachedSize];
                downloadCache.Seek(0, SeekOrigin.Begin);
                downloadCache.Read(cacheContent, 0, cachedSize);
                fileStream.Write(cacheContent, 0, cachedSize);
            }
        }

        static void DownLoad(string url, string file, string downloadPath)
        {
            var username = ConfigurationManager.AppSettings[AppKeyFTPUser].ToString();
            var password = ConfigurationManager.AppSettings[AppKeyFTPPassword].ToString();

            var ftpDownload = new FtpFunctions(url, username, password);
            WriteToLog(String.Format("Requesting file : {0}{1} ", url, file));
            ftpDownload.DownloadCached(
                file, 
                downloadPath, 
                WriteToLog, 
                " - SCG_CDS-Import - Error getting FTP file : ");
        }
        #endregion
        #region Step 2: Process files
        static void ProcessFiles()
        {
            DirectoryInfo di = new DirectoryInfo(ConfigurationManager.AppSettings["FilePath"].ToString());
            foreach (FileInfo fi in di.GetFiles("*.txt"))
            {
                string fileName = fi.Name.Replace(fi.Extension, "");
                string date = "_" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Year.ToString();
                WriteToLog(SeparatorLine);
                WriteToLog("");

                WriteToLog(String.Format("Starting file : {0} ", fi.Name));

                WriteToLog("");

                try
                {
                    //**Fields columns from import files CAN BE mapped to same UDF in ECN so must account for concatination

                    //based on potential file types need to have serveral parsing methods - comma seperated (csv or txt), xls, xlsx, xml, tab seperated (csv or txt)

                    ImportFile importFile = importFiles.Files.SingleOrDefault(x => x.FileName.ToString().Equals(fileName, StringComparison.CurrentCultureIgnoreCase));
                    importFileLog = new StreamWriter(new FileStream(ConfigurationManager.AppSettings["LogPath"] + "SCG_CDS_Import_" + fileName + "_" + DateTime.Now.ToString("MM-dd-yyyy") + ".log", FileMode.Append));

                    if (importFile != null)
                    {
                        GetGroupDataFields(importFile.GroupID);

                        if (fi.Extension.ToLower().Equals(".xls"))
                            ProcessXLS(fi.FullName, importFile);
                        else if (fi.Extension.ToLower().Equals(".xlxs"))
                            ProcessXLXS(fi.FullName, importFile);
                        else if (fi.Extension.ToLower().Equals(".xml"))
                            ProcessXML(fi.FullName, importFile);
                        else
                            ProcessFlatFile(fi, importFile);
                    }
                    else
                    {
                        KM.Common.EmailFunctions.NotifyAdmin("SCG_CDS-Import - ProcessFiles", DateTime.Now.ToString() + " - " + "No matching import file name in xml mapping file for " + fi.Name);
                        WriteToImportFileLog(DateTime.Now.ToString() + " - " + "SCG_CDS-Import - ProcessFiles : No matching import file name in xml mapping file for " + fi.Name);
                    }

                }
                catch (Exception ex)
                {
                    StringBuilder sbLog = new StringBuilder();
                    sbLog.AppendLine("**********************");
                    sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
                    sbLog.AppendLine("-- Message --");
                    if (ex.Message != null)
                        sbLog.AppendLine(ex.Message);
                    sbLog.AppendLine("-- InnerException --");
                    if (ex.InnerException != null)
                        sbLog.AppendLine(ex.InnerException.ToString());
                    sbLog.AppendLine("-- Stack Trace --");
                    if (ex.StackTrace != null)
                        sbLog.AppendLine(ex.StackTrace);
                    sbLog.AppendLine("**********************");
                    KM.Common.EmailFunctions.NotifyAdmin("SCG_CDS-Import", sbLog.ToString());
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SCG_CDS-Import.Program.ProcessFiles", KMCommon_Application, "SCG_CDS-Import: Unhandled Exception", -1, -1);
                    WriteToLog(sbLog.ToString());
                }

                File.Move(fi.FullName, ConfigurationManager.AppSettings["FileArchive"].ToString() + fileName + date + ".txt");


                WriteToImportFileLog(String.Format("Moved file to {0} ", ConfigurationManager.AppSettings["FileArchive"].ToString() + fileName + date + ".txt"));

                WriteToImportFileLog(String.Format("Completed file : {0} ", fileName));
                WriteToImportFileLog("");
                WriteToImportFileLog(SeparatorLine);
                WriteToImportFileLog("");

                WriteToLog(String.Format("Moved file to {0} ", ConfigurationManager.AppSettings["FileArchive"].ToString() + fileName + date + ".txt"));

                WriteToLog(String.Format("Completed file to {0} ", fileName));
                WriteToLog("");

            }
        }
        static void GetGroupDataFields(int groupID)
        {
            string sqlstmt = " SELECT GroupDataFieldsID,ShortName  FROM GroupDatafields WHERE GroupID=" + groupID + " and IsDeleted = 0";

            DataTable emailstable = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sqlstmt, "Communicator");

            dUDFFields = new Dictionary<string, int>();
            foreach (DataRow dr in emailstable.Rows)
            {
                dUDFFields.Add(dr["ShortName"].ToString().ToLower(), Convert.ToInt32(dr["GroupDataFieldsID"]));
            }

        }

        static ImportFile ProcessXLS(string file, ImportFile importFile)
        {
            return importFile;
        }
        static ImportFile ProcessXLXS(string file, ImportFile importFile)
        {
            return importFile;
        }
        static ImportFile ProcessXML(string file, ImportFile importFile)
        {
            return importFile;
        }


        static void ProcessFlatFile(FileInfo file, ImportFile importFile)
        {
            WriteToImportFileLog(DateTime.Now + "-START--ProcessFlatFile()-- " + " File: " + importFile.FileName + " --");
            DataTable dtFile = null;
            Dictionary<string, string> dCDSData = null;
            Dictionary<string, string> dCDSDataLatest = null;
            try
            {
                ConstructSchema(file);

                string columns = string.Empty;

                foreach (Field f in importFile.Fields)
                {
                    if (!f.Ignore && f.FileField.Trim().Length > 0)
                        columns += (columns == string.Empty ? "" : ",") + f.FileField;
                }


                dtFile = ParseFile(file.FullName, columns);

                dCDSData = ProcessData(dtFile, importFile);
                dCDSDataLatest = GetMostRecent(dtFile);

                int totalRecords = dCDSData.Count;
                int totalAutogenerated = dCDSData.Where(x => x.Value.EndsWith(".kmpsgroup.com", StringComparison.CurrentCultureIgnoreCase)).ToList().Count;

                WriteToImportFileLog("");
                WriteToImportFileLog("=========================================================");
                WriteToImportFileLog(string.Format("Total Records in File : {0}", totalRecords));
                WriteToImportFileLog(string.Format("Total AutoGenerated Emails : {0}", totalAutogenerated));
                WriteToImportFileLog(string.Format("Total Records with Email : {0}", (totalRecords - totalAutogenerated)));
                WriteToImportFileLog("=========================================================");
                WriteToImportFileLog("");

                WriteToLog("");
                WriteToLog("=========================================================");
                WriteToLog(string.Format("Total Records in File : {0}", totalRecords));
                WriteToLog(string.Format("Total AutoGenerated Emails : {0}", totalAutogenerated));
                WriteToLog(string.Format("Total Records with Email : {0}", (totalRecords - totalAutogenerated)));
                WriteToLog("=========================================================");
                WriteToLog("");


                ImportToEcn(dtFile, importFile, dCDSData, dCDSDataLatest);

                dCDSData = null;
                dCDSDataLatest = null;
                dtFile.Dispose();

            }
            catch (Exception ex)
            {
                StringBuilder sbLog = new StringBuilder();
                sbLog.AppendLine("**********************");
                sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
                sbLog.AppendLine("-- Message --");
                if (ex.Message != null)
                    sbLog.AppendLine(ex.Message);
                sbLog.AppendLine("-- InnerException --");
                if (ex.InnerException != null)
                    sbLog.AppendLine(ex.InnerException.ToString());
                sbLog.AppendLine("-- Stack Trace --");
                if (ex.StackTrace != null)
                    sbLog.AppendLine(ex.StackTrace);
                sbLog.AppendLine("**********************");
                KM.Common.EmailFunctions.NotifyAdmin("SCG_CDS-Import", sbLog.ToString());
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SCG_CDS-Import.Program.ProcessFlatFile", KMCommon_Application, "SCG_CDS-Import: Unhandled Exception", -1, importFile.CustomerID);
                WriteToImportFileLog(sbLog.ToString());
            }
            finally
            {
                if (dtFile != null)
                    dtFile.Dispose();

                dCDSData = null;
            }
        }

        public static Dictionary<string, string> GetMostRecent(DataTable dt)
        {
            Dictionary<string, string> dCDSDataLatest = new Dictionary<string, string>();


            if (dt.Rows.Count > 0)
            {

                //loop thru the list --Build sub, latestdate combo
                foreach (DataRow currentrow in dt.Rows)
                {
                    string email = string.Empty;

                    if (!dCDSDataLatest.ContainsKey(currentrow[SubFieldName].ToString()))
                        dCDSDataLatest.Add(currentrow[SubFieldName].ToString(), currentrow[UpdFieldName].ToString().Trim() + WhiteSpace + currentrow[UptFieldName].ToString().Trim());
                    else
                    {
                        //check the DateTime /Timeout  & update value
                        string[] old = dCDSDataLatest[currentrow[SubFieldName].ToString()].Split(' ');
                        int oldDate = Convert.ToInt32(old[0]);
                        int oldTime = Convert.ToInt32(old[1]);
                        int newDate = Convert.ToInt32(currentrow[UpdFieldName].ToString());
                        int newTime = Convert.ToInt32(currentrow[UptFieldName].ToString());
                        //Both of these will update the dict with the most recent sub/updatedDateTime
                        if (oldDate < newDate)
                        {
                            dCDSDataLatest[currentrow[SubFieldName].ToString()] = currentrow[UpdFieldName].ToString() + WhiteSpace + currentrow[UptFieldName].ToString();
                        }
                        else if (oldDate == newDate && oldTime <= newTime)
                        {
                            dCDSDataLatest[currentrow[SubFieldName].ToString()] = currentrow[UpdFieldName].ToString() + WhiteSpace + currentrow[UptFieldName].ToString();
                        }

                    }
                }
            }
            return dCDSDataLatest;
        }


        public static Dictionary<string, string> ProcessData(DataTable dt, ImportFile importFile)
        {
            Dictionary<string, string> dCDSData = new Dictionary<string, string>();

            HashSet<string> emails = new HashSet<string>();

            Dictionary<string, string> dExistingRecords = getExistingRecords(importFile.GroupID);

            try
            {
                WriteToImportFileLog(String.Format("Total Records to be processed : {0}", dt.Rows.Count));

                foreach (DataRow currentrow in dt.Rows)
                {
                    string email = string.Empty;

                    ///TODO - Use dExistsingRecord to check if Email or CDSID already exists
                    ///if CDSID already exists - check the emailaddress 
                    ///if Emailaddress is changed - update emailaddress in EmailTable.

                    if (string.IsNullOrEmpty(currentrow["EML"].ToString()) || emails.Contains(currentrow["EML"].ToString().ToLower().Trim()))
                        email = currentrow["CRD"].ToString() + "-" + currentrow[SubFieldName].ToString() + "@" + currentrow["PUB"].ToString() + ".kmpsgroup.com";
                    else
                        email = currentrow["EML"].ToString();

                    emails.Add(email.ToLower().Trim());

                    if (!dCDSData.ContainsKey(currentrow[SubFieldName].ToString()))
                        dCDSData.Add(currentrow[SubFieldName].ToString(), email.Trim().ToLower());
                }

                //foreach (KeyValuePair<string, string> kv in dCDSData)
                //{ 
                //    if (dExistingRecords.ContainsKey(kv.Key))
                //    {
                //        if (kv.Value.ToLower() != dExistingRecords[kv.Key].ToLower())
                //        {
                //            if (!dCDSData.ContainsValue(dExistingRecords[kv.Key].ToLower()))
                //            {
                //                dCDSData[kv.Key] = dExistingRecords[kv.Key].ToLower();
                //            }

                //        }
                //    }
                //}

                Dictionary<string, string> dMatched = (from kvp1 in dCDSData
                                                       join kvp2 in dExistingRecords on kvp1.Key equals kvp2.Key //&& kvp1.Value != kvp2.Value
                                                       select new { k = kvp1.Key, v = kvp2.Value }).ToDictionary(x => x.k, y => y.v);

                foreach (KeyValuePair<string, string> kv in dMatched)
                {
                    if (dCDSData[kv.Key].ToLower() != kv.Value.ToLower())
                    {
                        if (!dCDSData.ContainsValue(kv.Value.ToLower()))
                        {
                            dCDSData[kv.Key] = kv.Value.ToLower();
                        }

                    }
                }



            }
            catch (Exception ex)
            {
                WriteToImportFileLog(String.Format("Error : {0}", ex.Message));
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SCG_CDS-Import.Program.ProcessData", KMCommon_Application, "SCG_CDS-Import: Unhandled Exception", -1, importFile.CustomerID);
                throw ex;
            }


            return dCDSData;

        }

        public static Dictionary<string, string> getExistingRecords(int groupID)
        {
            Dictionary<string, string> dExistingRecords = new Dictionary<string, string>();

            string sqlstmt = " select emailaddress, datavalue as CDSID from GroupDatafields gdf with (NOLOCK) join EmailDataValues edv with (NOLOCK) on gdf.GroupDatafieldsID = edv.GroupDatafieldsID join Emails e on e.EmailID = edv.EmailID where gdf.GroupID = " + groupID + " and ShortName like 'CDSID' and isnull(datavalue, '') <> ''";

            DataTable dtCDSID = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sqlstmt, "Communicator");

            foreach (DataRow currentrow in dtCDSID.Rows)
            {
                if (!dExistingRecords.ContainsKey(currentrow["CDSID"].ToString()))
                    dExistingRecords.Add(currentrow["CDSID"].ToString(), currentrow["Emailaddress"].ToString().ToLower());
            }

            return dExistingRecords;
        }

        public static void ConstructSchema(FileInfo file)
        {
            try
            {
                WriteToImportFileLog("ConstructSchema File: " + file.Name + WhiteSpace + DateTime.Now);
                StringBuilder schema = new StringBuilder();
                DataTable data = LoadFile(file);
                schema.AppendLine("[" + file.Name + "]");
                schema.AppendLine("ColNameHeader=True");
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    schema.AppendLine("col" + (i + 1).ToString() + "=" + data.Columns[i].ColumnName + " Text");
                }
                string schemaFileName = ConfigurationManager.AppSettings["FilePath"].ToString() + "Schema.ini";
                TextWriter tw = new StreamWriter(schemaFileName);
                tw.WriteLine(schema.ToString());
                tw.Close();
            }
            catch (Exception ex)
            {
                WriteToImportFileLog("Error creating schema: " + file);

                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SCG_CDS-Import.Program.ConstructSchema", KMCommon_Application, "SCG_CDS-Import: Unhandled Exception", -1, -1);
                throw ex;
            }
        }

        public static DataTable ParseFile(string path, string columns)
        {
            WriteToImportFileLog($"ParseFile File: " + Path.GetFileName(path) + WhiteSpace + DateTime.Now);
            string header = "Yes";
            string sql = string.Empty;
            string pathOnly = string.Empty;
            string fileName = string.Empty;

            DataTable dataTable = null;
            try
            {
                pathOnly = Path.GetDirectoryName(path);
                fileName = Path.GetFileName(path);
                sql = @"SELECT " + (columns == string.Empty ? "*" : columns) + " FROM [" + fileName + "]";

                OleDbConnection connection = new OleDbConnection(
                        @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
                        ";Extended Properties=\"Text;HDR=" + header + "\"");

                OleDbCommand command = new OleDbCommand(sql, connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                dataTable = new DataTable();
                dataTable.Locale = CultureInfo.CurrentCulture;
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                WriteToImportFileLog("Error parsing: " + path);
                WriteToImportFileLog("Error Message: " + ex.Message);
                WriteToImportFileLog("Skipping File: " + path);
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SCG_CDS-Import.Program.ParseFile", KMCommon_Application, "SCG_CDS-Import: Unhandled Exception", -1, -1);
                throw ex;
            }
            WriteToImportFileLog("End Parsing : " + DateTime.Now);
            return dataTable;
        }

        public static DataTable LoadFile(FileInfo file)
        {
            string sqlString = "Select top 1 * FROM [" + file.Name + "];";
            string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="
                + file.DirectoryName + ";" + "Extended Properties='text;HDR=YES;'";
            DataTable theCSV = new DataTable();

            using (OleDbConnection conn = new OleDbConnection(conStr))
            {
                using (OleDbCommand comm = new OleDbCommand(sqlString, conn))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(comm))
                    {
                        adapter.Fill(theCSV);
                    }
                }
            }
            return theCSV;
        }

        #endregion

        #region Send to ECN

        private static void ImportToEcn(DataTable dtFile, ImportFile importFile, IReadOnlyDictionary<string, string> dCdsData, Dictionary<string, string> dCdsDataLatest)
        {
            var batchCount = 1;
            var counter = 0;
            var xmlProfile = new StringBuilder();
            var xmlUdf = new StringBuilder();

            try
            {
                var totalbatchCount = dCdsData.Count / int.Parse(ConfigurationManager.AppSettings[BatchCountParamName]);

                var lProfileList = importFile.Fields.Where(x => !x.IsUDF && !x.Ignore && x.ECNField.Length > 0).ToList();
                var lUdfList = importFile.Fields.Where(x => x.IsUDF && !x.Ignore && x.ECNField.Length > 0).ToList();

                foreach (DataRow currentrow in dtFile.Rows)
                {
                    if (!dCdsDataLatest.Contains(new KeyValuePair<string, string>(
                        currentrow[SubFieldName].ToString(),
                        currentrow[UpdFieldName] + WhiteSpace + currentrow[UptFieldName])))
                    {
                        continue;
                    }

                    xmlProfile.AppendLine("<Emails>");
                    var email = dCdsData.ContainsKey(currentrow[SubFieldName].ToString())
                        ? dCdsData[currentrow[SubFieldName].ToString()]
                        : string.Empty;

                    BuildXmlForProfileList(importFile, lProfileList, currentrow, email, xmlProfile);

                    xmlProfile.AppendLine($"<notes><![CDATA[ [SCG_CDS-Import Engine] [{DateTime.Now.ToString(CultureInfo.CurrentCulture)}] ]]> </notes>");

                    xmlProfile.AppendLine("</Emails>");

                    xmlUdf.AppendLine("<row>");
                    xmlUdf.AppendLine($"<ea>{CleanXMLString(email)}</ea>");

                    BuildXmlForUdfList(lUdfList, currentrow, xmlUdf);
                    xmlUdf.AppendLine("</row>");

                    if (++counter == int.Parse(ConfigurationManager.AppSettings[BatchCountParamName]))
                    {
                        WriteToImportFileLog($"Batch # {batchCount} of {totalbatchCount}");

                        UpdateToDb(xmlProfile.ToString(), xmlUdf.ToString(), importFile);
                        xmlProfile = new StringBuilder();
                        xmlUdf = new StringBuilder();
                        batchCount++;
                        counter = 0;
                    }
                }

                if (xmlProfile.Length > 0)
                {
                    UpdateToDb(xmlProfile.ToString(), xmlUdf.ToString(), importFile);
                }
                WriteToImportFileLog($"{DateTime.Now}-END--ImportToECN() File: {importFile.FileName} --");
            }
            catch (Exception ex)
            {
                LogImportToEcnException(importFile, ex);
                throw;
            }
        }

        private static void LogImportToEcnException(ImportFile importFile, Exception ex)
        {
            var sbLog = new StringBuilder();
            sbLog.AppendLine(DataPackEndMarker);
            sbLog.AppendLine($"Exception - {DateTime.Now}");
            sbLog.AppendLine("-- Message --");
            if (ex.Message != null)
            {
                sbLog.AppendLine(ex.Message);
            }

            sbLog.AppendLine("-- InnerException --");
            if (ex.InnerException != null)
            {
                sbLog.AppendLine(ex.InnerException.ToString());
            }

            sbLog.AppendLine("-- Stack Trace --");
            if (ex.StackTrace != null)
            {
                sbLog.AppendLine(ex.StackTrace);
            }

            sbLog.AppendLine(DataPackEndMarker);
            EmailFunctions.NotifyAdmin("SCG_CDS-Import", sbLog.ToString());
            KM.Common.Entity.ApplicationLog.LogCriticalError(ex,
                "SCG_CDS-Import.Program.ImportToECN",
                KMCommon_Application,
                "SCG_CDS-Import: Unhandled Exception",
                -1,
                importFile.CustomerID);

            WriteToImportFileLog(sbLog.ToString());
        }

        private static void BuildXmlForUdfList(IEnumerable<Field> lUdfList, DataRow currentrow, StringBuilder xmlUdf)
        {
            foreach (var field in lUdfList)
            {
                var kvp = dUDFFields.SingleOrDefault(x =>
                    x.Key.Equals(field.ECNField.ToString(), StringComparison.CurrentCultureIgnoreCase));
                if (kvp.Value == 0)
                {
                    continue;
                }

                var udfFieldValue = string.Empty;
                if (field.FileField.Trim().Length > 0)
                {
                    udfFieldValue = currentrow[field.FileField].ToString();
                }
                else if (!string.IsNullOrWhiteSpace(field.CombineFields))
                {
                    var sCombineFields = field.CombineFields.Trim();
                    var incomingFields = sCombineFields.Split(',');

                    udfFieldValue = incomingFields.Where(s => !string.IsNullOrWhiteSpace(s))
                        .Aggregate(udfFieldValue,
                            (current, s) =>
                                current +
                                (current == string.Empty
                                    ? currentrow[s].ToString()
                                    : $",{currentrow[s]}"));
                }

                ParseEcnFieldValueForUdf(currentrow, field, ref udfFieldValue);

                if (udfFieldValue.Trim().Length > 0)
                {
                    xmlUdf.AppendLine($"<udf id=\"{kvp.Value}\"><v>{CleanXMLString(udfFieldValue)}</v></udf>");
                }
            }
        }

        private static void ParseEcnFieldValueForUdf(DataRow currentrow, Field field, ref string udfFieldValue)
        {
            switch (field.ECNField.ToLower())
            {
                case ECNFieldQDate:
                    if (currentrow[VDTFieldName].ToString().Length == 6)
                    {
                        var month = currentrow[VDTFieldName].ToString().Substring(4, 2);
                        var year = currentrow[VDTFieldName].ToString().Substring(0, 4);
                        udfFieldValue = $"{month}/01/{year}";
                    }

                    break;
                case ECNFieldForZip:
                    if (currentrow[CNTFieldName].ToString().Trim().Length > 0)
                    {
                        udfFieldValue = currentrow[ZipFieldName].ToString().Trim();
                    }

                    break;
                case ECNFieldCat:
                    switch (currentrow[field.FileField].ToString().ToUpper())
                    {
                        case "Q":
                        case "E":
                            udfFieldValue = "10";
                            break;
                        case "R":
                            udfFieldValue = "70";
                            break;
                        default:
                            udfFieldValue = "10";
                            break;
                    }

                    break;
                case ECNFieldXAct:
                    switch (currentrow[field.FileField].ToString().ToUpper())
                    {
                        case "Q":
                        case "R":
                            udfFieldValue = "10";
                            break;
                        case "E":
                            udfFieldValue = "38";
                            break;
                        default:
                            udfFieldValue = "10";
                            break;
                    }

                    break;
                case ECNFieldDemo7:
                    switch (currentrow[field.FileField].ToString().ToUpper())
                    {
                        case "D":
                            udfFieldValue = "B";
                            break;
                        case "P":
                            udfFieldValue = "A";
                            break;
                        case "B":
                            udfFieldValue = "C";
                            break;
                        default:
                            udfFieldValue = "A";
                            break;
                    }

                    break;
                default:
                    udfFieldValue = udfFieldValue.Replace("/", ",");
                    break;
            }
        }

        private static void BuildXmlForProfileList(
            ImportFile importFile,
            IEnumerable<Field> lProfileList,
            DataRow currentrow,
            string email,
            StringBuilder xmlProfile)
        {
            foreach (var field in lProfileList)
            {
                var ecnFieldName = field.ECNField;
                var ecnFieldValue = string.Empty;

                if (field.FileField.Trim().Length > 0)
                {
                    ecnFieldValue = currentrow[field.FileField].ToString();
                }
                else if (!string.IsNullOrWhiteSpace(field.CombineFields))
                {
                    var sCombineFields = field.CombineFields.Trim();
                    var incomingFields = sCombineFields.Split(',');

                    ecnFieldValue = incomingFields.Where(s => !string.IsNullOrWhiteSpace(s))
                        .Aggregate(ecnFieldValue,
                            (current, s) =>
                                current +
                                (current == string.Empty
                                    ? currentrow[s].ToString()
                                    : $",{currentrow[s]}"));
                }

                ParseEcnFieldValueForProfileUpdate(importFile, currentrow, email, field, ref ecnFieldValue);
                if (ecnFieldValue.Trim().Length <= 0)
                {
                    continue;
                }

                var ecnFieldTag = ecnFieldName.ToLower();
                xmlProfile.AppendLine($"<{ecnFieldTag}>{CleanXMLString(ecnFieldValue)}</{ecnFieldTag}>");
            }
        }

        private static void ParseEcnFieldValueForProfileUpdate(
            ImportFile importFile,
            DataRow currentrow,
            string email,
            Field field,
            ref string ecnFieldValue)
        {
            switch (field.ECNField.ToLower())
            {
                case ECNFieldFirstName:
                    GetNameValue(currentrow,
                        field,
                        ref ecnFieldValue, 
                        token => token.Substring(0,token.IndexOf(WhiteSpace, StringComparison.Ordinal)));
                    break;
                case ECNFieldLastName:
                    GetNameValue(currentrow,
                        field,
                        ref ecnFieldValue,
                        token => token.Substring(token.IndexOf(WhiteSpace, StringComparison.Ordinal)));
                    break;
                case ECNFieldVoice:
                case ECNFieldFax:
                case ECNFieldMobile:
                    ecnFieldValue = CleanPhoneNumber(ecnFieldValue);
                    break;
                case ECNFieldEmail:
                    ecnFieldValue = email;
                    break;
                case ECNFieldSubscribeTypeCode:
                    ProcessSubscribeTypeCode(importFile, ref ecnFieldValue);
                    break;
                case ECNFieldZip:
                    if (currentrow[CNTFieldName].ToString().Trim().Length > 0)
                    {
                        ecnFieldValue = string.Empty;
                    }

                    break;
            }
        }

        private static void ProcessSubscribeTypeCode(ImportFile importFile, ref string ecnFieldValue)
        {
            if (ecnFieldValue.Equals("D", StringComparison.CurrentCultureIgnoreCase))
            {
                ecnFieldValue = "U";
            }

            var t = new TransactionType
            {
                Pub = importFile.FileName,
                AddCount = 0,
                ChangeCount = 0,
                DeleteCount = 0
            };

            if (ecnFieldValue.Equals("U", StringComparison.CurrentCultureIgnoreCase))
            {
                t.DeleteCount = 1;
            }
            else if (ecnFieldValue.Equals("C", StringComparison.CurrentCultureIgnoreCase))
            {
                t.ChangeCount = 1;
            }
            else if (ecnFieldValue.Equals("A", StringComparison.CurrentCultureIgnoreCase))
            {
                t.AddCount = 1;
            }

            TransactionReport.Add(t);
        }

        private static void GetNameValue(DataRow currentrow, Field field, ref string ecnFieldValue, Func<string, string> splitter)
        {
            if (field.FileField.Trim().Length != 0)
            {
                return;
            }

            try
            {
                if (currentrow[INMFieldName].ToString().IndexOf(WhiteSpace, StringComparison.Ordinal) > 0)
                {
                    var token = currentrow[INMFieldName].ToString();
                    ecnFieldValue = splitter(token).Trim();
                }
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format(IgnoreExceptionLogMessageTemplate,
                    ex.GetType().Name,
                    ex.Message,
                    field.ECNField));
            }
        }

        private static void UpdateToDb(string xmlProfile, string xmlUdf, ImportFile importFile)
        {
            LogUpdateToDbStart();
            xmlProfile = xmlProfile.Replace("&", "and");
            xmlUdf = xmlUdf.Replace("&", "and");

            if (bool.Parse(ConfigurationManager.AppSettings[WriteXmlLogParamName]))
            {
                LogUpdateToDbXml(xmlProfile, xmlUdf);
            }

            try
            {
                var superUser = LoadSuperUserProfile(importFile);

                var importEmails = EmailGroup.ImportEmails(
                    superUser.UserID,
                    importFile.CustomerID,
                    importFile.GroupID,
                    string.Format(XmlFrame, xmlProfile),
                    string.Format(XmlFrame, xmlUdf),
                    HtmlFormatType,
                    SubscribeTypeCode,
                    false);
                WriteImportedEmailsToLogFile(importEmails);
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SCG_CDS-Import.Program.UpdateToDB", KMCommon_Application, "SCG_CDS-Import: ImportEmails", -1, importFile.CustomerID);

                WriteExceptionToLogFile(xmlProfile, xmlUdf, ex);
            }
            LogUpdateToDbEnd();
        }

        private static void WriteExceptionToLogFile(string xmlProfile, string xmlUdf, Exception ex)
        {
            WriteToImportFileLog(StringFunctions.FormatException(ex));
            WriteToImportFileLog(SeparatorLine);
            LogUpdateToDbXml(xmlProfile, xmlUdf);
        }

        private static void WriteImportedEmailsToLogFile(DataTable importEmails)
        {
            try
            {
                var ecnResults = new StringBuilder();
                ecnResults.AppendLine(EcnResultsHeader);
                foreach (DataRow row in importEmails.Rows)
                {
                    var rowResults = new StringBuilder();
                    foreach (var column in row.ItemArray)
                    {
                        rowResults.Append(column);
                    }

                    ecnResults.AppendLine(rowResults.ToString());
                }

                ecnResults.AppendLine(DataPackEndMarker);
                WriteToImportFileLog(ecnResults.ToString());
            }
            catch (Exception ex)
            {
                WriteToImportFileLog(StringFunctions.FormatException(ex));
            }
        }

        private static KMPlatform.Entity.User LoadSuperUserProfile(ImportFile importFile)
        {
            var userList = User.GetByCustomerID(importFile.CustomerID);
            var superUser = new KMPlatform.Entity.User();
            var customer = Customer.GetByCustomerID(importFile.CustomerID, true);
            var baseChannel = BaseChannel.GetByBaseChannelID(customer.BaseChannelID.Value);
            foreach (var user in userList)
            {
                user.CurrentClient = new Client().Select(customer.PlatformClientID, true);
                user.CurrentClientGroup = new ClientGroup().Select(baseChannel.PlatformClientGroupID);
                user.CurrentSecurityGroup = new SecurityGroup().Select(user.UserID, user.CurrentClient.ClientID, false);
                if (KM.Platform.User.IsAdministrator(user))
                {
                    superUser = new User().SelectUser(user.UserID);
                    superUser.CurrentClient = new Client().Select(customer.PlatformClientID, true);
                    superUser.CurrentClientGroup = new ClientGroup().Select(baseChannel.PlatformClientGroupID);
                    superUser.CurrentSecurityGroup =
                        new SecurityGroup().Select(superUser.UserID, superUser.CurrentClient.ClientID, false);
                    break;
                }

                if (KM.Platform.User.HasAccess(user,
                    KMPlatform.Enums.Services.EMAILMARKETING,
                    KMPlatform.Enums.ServiceFeatures.Email,
                    KMPlatform.Enums.Access.ImportEmails))
                {
                    superUser = new User().SelectUser(user.UserID);
                    superUser.CurrentClient = new Client().Select(customer.PlatformClientID, true);
                    superUser.CurrentClientGroup = new ClientGroup().Select(baseChannel.PlatformClientGroupID);
                    superUser.CurrentSecurityGroup =
                        new SecurityGroup().Select(superUser.UserID, superUser.CurrentClient.ClientID, false);
                    break;
                }
            }

            return superUser;
        }

        private static void LogUpdateToDbXml(string xmlProfile, string xmlUdf)
        {
            WriteToImportFileLog(XmlProfileSectionMarker);
            WriteToImportFileLog(xmlProfile);
            WriteToImportFileLog(XmlProfileSectionMarker);
            WriteToImportFileLog(XmlUdfSectionMarker);
            WriteToImportFileLog(xmlUdf);
            WriteToImportFileLog(XmlUdfSectionMarker);
        }

        private static void LogUpdateToDbStart()
        {
            WriteToImportFileLog(SeparatorLine);
            WriteToImportFileLog($"{DateTime.Now}-START--UpdateToDB()-");
        }

        private static void LogUpdateToDbEnd()
        {
            WriteToImportFileLog($"{DateTime.Now}-END--UpdateToDB()");
            WriteToImportFileLog(SeparatorLine);
            WriteToImportFileLog(string.Empty);
        }

        #endregion

        #region Report
        static void CreateReport()
        {
            //TFS item 2057 SGC reports
            //csv  output is perfect, please include header rows (no quotes with the data)
            //please auto email the report each week to jhughes@sgcmail.com, corey.mcmahon@TeamKM.com, and danielle.hoffman@TeamKM.com
            var GroupedReport = TransactionReport.GroupBy(x => x.Pub).ToList();
            List<TransactionType> GroupCount = new List<TransactionType>();
            foreach (var x in GroupedReport)
            {
                var pubGroup = TransactionReport.Where(y => y.Pub == x.Key).ToList();
                int addCount = 0;
                int changeCount = 0;
                int deleteCount = 0;
                foreach (var p in pubGroup)
                {
                    addCount += p.AddCount;
                    changeCount += p.ChangeCount;
                    deleteCount += p.DeleteCount;
                }
                TransactionType t = new TransactionType();
                t.Pub = x.Key;
                t.AddCount = addCount;
                t.ChangeCount = changeCount;
                t.DeleteCount = deleteCount;

                GroupCount.Add(t);
            }

            StringBuilder sbFile = new StringBuilder();
            sbFile.AppendLine("Pub,AddCount,ChangeCount,DeleteCount");

            foreach (var gc in GroupCount)
            {
                StringBuilder sbDetail = new StringBuilder();
                sbDetail.Append(gc.Pub + ",");
                sbDetail.Append(gc.AddCount.ToString() + ",");
                sbDetail.Append(gc.ChangeCount.ToString() + ",");
                sbDetail.Append(gc.DeleteCount.ToString());

                sbFile.AppendLine(sbDetail.ToString());
            }

            WriteReportFile(sbFile.ToString());
            EmailReport(sbFile.ToString());
        }
        static void WriteReportFile(string text)
        {
            StreamWriter reportFile;
            reportFile = new StreamWriter(new FileStream(System.Configuration.ConfigurationManager.AppSettings["ReportFilePath"] + "TransactionCounts_" + DateTime.Now.ToString("MM-dd-yyyy") + ".csv", System.IO.FileMode.Append));

            reportFile.AutoFlush = true;
            reportFile.WriteLine(text);
            reportFile.Flush();
            reportFile.Close();
        }
        static void EmailReport(string report)
        {
            string reportName = "TransactionCounts_" + DateTime.Now.ToString("MM-dd-yyyy") + ".csv";

            SmtpClient smtpServer = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["KMCommon_SmtpServer"]);
            MailMessage message = new MailMessage();
            message.Priority = MailPriority.High;
            message.IsBodyHtml = false;
            message.To.Add(System.Configuration.ConfigurationManager.AppSettings["ReportEmailList"].ToString());
            message.Bcc.Add(System.Configuration.ConfigurationManager.AppSettings["EmailBCC"].ToString());
            MailAddress msgSender = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailFrom"].ToString());
            message.From = msgSender;
            message.Subject = "Transaction Count Report";
            message.Body = "Transaction Count Report is attached";

            Attachment att = Attachment.CreateAttachmentFromString(report, reportName);
            message.Attachments.Add(att);
            smtpServer.Send(message);
        }
        #endregion


        static string CleanXMLString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            //text = text.Replace("'", "");
            return text.Trim();
        }
        static string CleanPhoneNumber(string dirty)
        {
            string clean = dirty.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "").Replace(WhiteSpace, "").ToString();
            return clean.Trim();
        }
        static string CleanCity(string dirty)
        {
            string clean = dirty;
            if (dirty.Contains(","))
            {
                int totalLength = dirty.Length;
                int commaIndex = dirty.IndexOf(",");
                int remove = totalLength - commaIndex;
                clean = dirty.Remove(commaIndex, remove);
            }
            return clean.Trim();
        }
        public static void WriteToLog(string text)
        {
            Console.WriteLine(text.ToString());

            logFile.AutoFlush = true;
            logFile.WriteLine(DateTime.Now.ToString() + WhiteSpace + text);
            logFile.Flush();
        }
        public static void WriteToImportFileLog(string text)
        {
            Console.WriteLine(text.ToString());

            importFileLog.AutoFlush = true;
            importFileLog.WriteLine(DateTime.Now.ToString() + WhiteSpace + text);
            importFileLog.Flush();
        }


    }

}