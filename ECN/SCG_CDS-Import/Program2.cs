//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.IO;
//using System.Data;
//using System.Net;
//using System.Configuration;
//using System.Runtime.Serialization;
//using System.Xml.Linq;
//using System.Xml.Serialization;
//using System.Net.Mail;
//using System.Threading;

//namespace SCG_CDS_Import
//{
//    class Program
//    {
//        static int KMCommon_Application;
//        static List<KeyValuePair<int, string>> hUDFFields = new List<KeyValuePair<int, string>>();
//        public static StreamWriter logFile;
//        public static StreamWriter importFileLog;

//        // 2M bytes.
//        public const int MaxCacheSize = 2097152;
//        // 2K bytes.
//        public const int BufferSize = 2048;
//        static bool isDownloadComplete;
//        static ImportFiles importFiles;
//        static List<Header> importValueList;
//        static int ImportRowCount;

//        static List<TransactionType> TransactionReport;
//        static List<KeyValuePair<int, string>> ColumnIndexList;

//        static void Main(string[] args)
//        {
//            logFile = new StreamWriter(new FileStream(ConfigurationManager.AppSettings["LogPath"] + "SCG_CDS_Import_" + DateTime.Now.ToString("MM-dd-yyyy") + ".log", FileMode.Append));
//            WriteToLog(""); WriteToLog("");
//            WriteToLog(DateTime.Now + "-START--------------------------------------");

//            try
//            {
//                string path = ConfigurationManager.AppSettings["MappingFilePath"] + "\\FileName_GroupID_Mapping.xml";
//                XmlSerializer serializer = new XmlSerializer(typeof(ImportFiles));//, new XmlRootAttribute("Mappings"));
//                StreamReader reader = new StreamReader(path);
//                importFiles = (ImportFiles)serializer.Deserialize(reader);
//                if (importFiles.Files.Count > 0)
//                {
//                    AuthKeyCheck();

//                    KMCommon_Application = -1;
//                    int.TryParse(ConfigurationManager.AppSettings["KMCommon_Application"].ToString(), out KMCommon_Application);

//                    bool doFileDownload = true;
//                    bool.TryParse(ConfigurationManager.AppSettings["DownloadFiles"].ToString(), out doFileDownload);
//                    if (doFileDownload == true)
//                        DownloadFiles();
//                    else
//                        isDownloadComplete = true;

//                    if (isDownloadComplete == true)
//                    {
//                        TransactionReport = new List<TransactionType>();
//                        ProcessFiles();
//                        CreateReport();
//                    }
//                }
//                else
//                {
//                    KM.Common.EmailFunctions.NotifyAdmin("SCG_CDS-Import - EmptyXML", DateTime.Now.ToString() + " - " + "No data in xml file after Deserialization");
//                    WriteToLog(DateTime.Now.ToString() + " - " + "SCG_CDS-Import - EmptyXML : No data in xml file after Deserialization");
//                }
//            }
//            catch (Exception ex)
//            {
//                StringBuilder sbLog = new StringBuilder();
//                sbLog.AppendLine("**********************");
//                sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
//                sbLog.AppendLine("-- Message --");
//                if (ex.Message != null)
//                    sbLog.AppendLine(ex.Message);
//                sbLog.AppendLine("-- InnerException --");
//                if (ex.InnerException != null)
//                    sbLog.AppendLine(ex.InnerException.ToString());
//                sbLog.AppendLine("-- Stack Trace --");
//                if (ex.StackTrace != null)
//                    sbLog.AppendLine(ex.StackTrace);
//                sbLog.AppendLine("**********************");
//                KM.Common.EmailFunctions.NotifyAdmin("SCG_CDS-Import", sbLog.ToString());
//                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SCG_CDS-Import.Program.Main", KMCommon_Application, "SCG_CDS-Import: Unhandled Exception", -1, -1);
//                WriteToLog(sbLog.ToString());
//            }
//        }

//        static void AuthKeyCheck()
//        {
//            foreach (ImportFile f in importFiles.Files)
//            {
//                if (string.IsNullOrEmpty(f.AuthKey))
//                {
//                    var custs = ECN_Framework_DataLayer.Accounts.User.GetByCustomerID(f.CustomerID);
//                    foreach (var c in custs)
//                    {
//                        if (!string.IsNullOrEmpty(c.AccessKey.ToString()))
//                        {
//                            f.AuthKey = c.AccessKey.ToString();
//                            break;
//                        }
//                    }
//                }
//            }
//        }
//        #region Step 1: Download Import File from FTP
//        static void DownloadFiles()
//        {
//            try
//            {
//                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["FTPSite"].ToString());
//                request.Method = WebRequestMethods.Ftp.ListDirectory;

//                // This example assumes the FTP site uses anonymous logon.
//                string user = System.Configuration.ConfigurationManager.AppSettings["FTPUser"].ToString();
//                string pass = System.Configuration.ConfigurationManager.AppSettings["FTPPassword"].ToString();
//                request.Credentials = new NetworkCredential(user, pass);

//                FtpWebResponse dirResponse = (FtpWebResponse)request.GetResponse();
//                Stream dirResponseStream = dirResponse.GetResponseStream();
//                StreamReader dirReader = new StreamReader(dirResponseStream);
//                List<string> dirs = new List<string>();
//                while (dirReader.EndOfStream == false)
//                {
//                    dirs.Add(dirReader.ReadLine());
//                }
//                string dirMsg = "File check complete, status " + dirResponse.StatusDescription;
//                WriteToLog(dirMsg);
//                dirReader.Close();
//                dirResponse.Close();
//                request.Abort();

//                string ftpURL = ConfigurationManager.AppSettings["FTPSite"].ToString();
//                string destPath = ConfigurationManager.AppSettings["FilePath"].ToString();
//                foreach (string file in dirs)
//                {
//                    DownLoad(ftpURL, file, destPath);

//                    //delete file from FTP Site
//                    string ftpFullPath = ftpURL + file;
//                    FtpWebRequest rDelete = (FtpWebRequest)WebRequest.Create(ftpFullPath);
//                    rDelete.Method = WebRequestMethods.Ftp.DeleteFile;
//                    rDelete.Credentials = new NetworkCredential(user, pass);
//                    FtpWebResponse responseDelete = (FtpWebResponse)rDelete.GetResponse();

//                    Stream streamDelete = responseDelete.GetResponseStream();
//                    if (responseDelete.StatusCode != FtpStatusCode.FileActionOK)
//                        KM.Common.EmailFunctions.NotifyAdmin("SCG File Download Error", "File not deleted from SCG FTP:: Status Code - " + responseDelete.StatusCode.ToString() + " :: Status Description - " + responseDelete.StatusDescription.ToString());

//                    responseDelete.Close();
//                }
//                isDownloadComplete = true;
//            }
//            catch (Exception ex)
//            {
//                isDownloadComplete = false;

//                StringBuilder sbLog = new StringBuilder();
//                sbLog.AppendLine("**********************");
//                sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
//                sbLog.AppendLine("-- Message --");
//                if (ex.Message != null)
//                    sbLog.AppendLine(ex.Message);
//                sbLog.AppendLine("-- InnerException --");
//                if (ex.InnerException != null)
//                    sbLog.AppendLine(ex.InnerException.ToString());
//                sbLog.AppendLine("-- Stack Trace --");
//                if (ex.StackTrace != null)
//                    sbLog.AppendLine(ex.StackTrace);
//                sbLog.AppendLine("**********************");

//                WriteToLog(sbLog.ToString());
//                KM.Common.EmailFunctions.NotifyAdmin("SCG File Download Error", sbLog.ToString());
//            }
//        }
//        static void WriteCacheToFile(MemoryStream downloadCache, string downloadPath, int cachedSize)
//        {
//            using (FileStream fileStream = new FileStream(downloadPath, FileMode.Append))
//            {
//                byte[] cacheContent = new byte[cachedSize];
//                downloadCache.Seek(0, SeekOrigin.Begin);
//                downloadCache.Read(cacheContent, 0, cachedSize);
//                fileStream.Write(cacheContent, 0, cachedSize);
//            }
//        }
//        static void DownLoad(string url, string file, string downloadPath)
//        {
//            // Create a request to the file to be  downloaded.
//            FtpWebRequest request = WebRequest.Create(url + file) as FtpWebRequest;
//            string user = ConfigurationManager.AppSettings["FTPUser"].ToString();
//            string pass = ConfigurationManager.AppSettings["FTPPassword"].ToString();
//            request.Credentials = new NetworkCredential(user, pass);

//            // Download file.
//            request.Method = WebRequestMethods.Ftp.DownloadFile;

//            FtpWebResponse response = null;
//            Stream responseStream = null;
//            MemoryStream downloadCache = null;

//            try
//            {
//                // Retrieve the response from the server and get the response stream.
//                response = request.GetResponse() as FtpWebResponse;
//                responseStream = response.GetResponseStream();

//                // Cache data in memory.
//                downloadCache = new MemoryStream(MaxCacheSize);
//                byte[] downloadBuffer = new byte[BufferSize];

//                int bytesSize = 0;
//                int cachedSize = 0;

//                // Download the file until the download is completed.
//                while (true)
//                {
//                    // Read a buffer of data from the stream.
//                    bytesSize = responseStream.Read(downloadBuffer, 0, downloadBuffer.Length);

//                    // If the cache is full, or the download is completed, write the data in cache to local file.
//                    if (bytesSize == 0
//                        || MaxCacheSize < cachedSize + bytesSize)
//                    {
//                        try
//                        {
//                            string destPath = downloadPath + file;
//                            if (File.Exists(destPath))
//                                File.Delete(destPath);
//                            // Write the data in cache to local file.
//                            WriteCacheToFile(downloadCache, destPath, cachedSize);

//                            // Stop downloading the file if the download is paused, canceled or completed. 
//                            if (bytesSize == 0)
//                                break;

//                            // Reset cache.
//                            downloadCache.Seek(0, SeekOrigin.Begin);
//                            cachedSize = 0;
//                        }
//                        catch (Exception ex)
//                        {
//                            string msg = string.Format(
//                                "There is an error while downloading {0}. "
//                                + " See InnerException for detailed error. ",
//                                url + file);
//                            ApplicationException errorException = new ApplicationException(msg, ex);

//                            return;
//                        }

//                    }

//                    // Write the data from the buffer to the cache in memory.
//                    downloadCache.Write(downloadBuffer, 0, bytesSize);
//                    cachedSize += bytesSize;
//                }
//            }
//            finally
//            {
//                if (response != null)
//                    response.Close();

//                if (responseStream != null)
//                    responseStream.Close();

//                if (downloadCache != null)
//                    downloadCache.Close();
//            }
//        }
//        #endregion
//        #region Step 2: Process files
//        static void ProcessFiles()
//        {
//            DirectoryInfo di = new DirectoryInfo(ConfigurationManager.AppSettings["FilePath"].ToString());
//            foreach (FileInfo fi in di.GetFiles())
//            {
//                string fileName = fi.Name.Replace(fi.Extension, "");
//                try
//                {
//                    //**Headers columns from import files CAN BE mapped to same UDF in ECN so must account for concatination

//                    //based on potential file types need to have serveral parsing methods - comma seperated (csv or txt), xls, xlsx, xml, tab seperated (csv or txt)

//                    ImportFile importFile = importFiles.Files.SingleOrDefault(x => x.FileName.ToString().Equals(fileName, StringComparison.CurrentCultureIgnoreCase));
//                    importFileLog = new StreamWriter(new FileStream(ConfigurationManager.AppSettings["LogPath"] + "SCG_CDS_Import_" + fileName + "_" + DateTime.Now.ToString("MM-dd-yyyy") + ".log", FileMode.Append));

//                    if (importFile != null)
//                    {
//                        hUDFFields = GetGroupDataFields(importFile.GroupID);

//                        if (fi.Extension.ToLower().Equals(".xls"))
//                            ProcessXLS(fi.FullName, importFile);
//                        else if (fi.Extension.ToLower().Equals(".xlxs"))
//                            ProcessXLXS(fi.FullName, importFile);
//                        else if (fi.Extension.ToLower().Equals(".xml"))
//                            ProcessXML(fi.FullName, importFile);
//                        else
//                            ProcessFlatFile(fi.FullName, importFile);
//                    }
//                    else
//                    {
//                        KM.Common.EmailFunctions.NotifyAdmin("SCG_CDS-Import - ProcessFiles", DateTime.Now.ToString() + " - " + "No matching import file name in xml mapping file for " + fi.Name);
//                        WriteToImportFileLog(DateTime.Now.ToString() + " - " + "SCG_CDS-Import - ProcessFiles : No matching import file name in xml mapping file for " + fi.Name);
//                    }

//                }
//                catch (Exception ex)
//                {
//                    StringBuilder sbLog = new StringBuilder();
//                    sbLog.AppendLine("**********************");
//                    sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
//                    sbLog.AppendLine("-- Message --");
//                    if (ex.Message != null)
//                        sbLog.AppendLine(ex.Message);
//                    sbLog.AppendLine("-- InnerException --");
//                    if (ex.InnerException != null)
//                        sbLog.AppendLine(ex.InnerException.ToString());
//                    sbLog.AppendLine("-- Stack Trace --");
//                    if (ex.StackTrace != null)
//                        sbLog.AppendLine(ex.StackTrace);
//                    sbLog.AppendLine("**********************");
//                    KM.Common.EmailFunctions.NotifyAdmin("SCG_CDS-Import", sbLog.ToString());
//                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SCG_CDS-Import.Program.ProcessFiles", KMCommon_Application, "SCG_CDS-Import: Unhandled Exception", -1, -1);
//                    WriteToLog(sbLog.ToString());
//                }
//                string date = "_" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Year.ToString();
//                File.Move(fi.FullName, ConfigurationManager.AppSettings["FileArchive"].ToString() + fileName + date + ".txt");
//            }
//        }
//        static List<KeyValuePair<int, string>> GetGroupDataFields(int groupID)
//        {
//            string sqlstmt = " SELECT * FROM GroupDatafields WHERE GroupID=" + groupID + " and IsDeleted = 0";

//            DataTable emailstable = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sqlstmt, "Communicator");

//            List<KeyValuePair<int, string>> fields = new List<KeyValuePair<int, string>>();
//            foreach (DataRow dr in emailstable.Rows)
//            {
//                KeyValuePair<int, string> kvp = new KeyValuePair<int, string>(Convert.ToInt32(dr["GroupDataFieldsID"]), dr["ShortName"].ToString().ToLower());
//                fields.Add(kvp);
//            }

//            return fields;
//        }

//        static ImportFile ProcessXLS(string file, ImportFile importFile)
//        {
//            return importFile;
//        }
//        static ImportFile ProcessXLXS(string file, ImportFile importFile)
//        {
//            return importFile;
//        }
//        static ImportFile ProcessXML(string file, ImportFile importFile)
//        {
//            return importFile;
//        }
//        static bool VerifyColumnHeaders(string[] columns, ImportFile importFile)
//        {
//            bool match = true;

//            foreach (Header h in importFile.HeaderMap)
//            {
//                bool found = false;
//                foreach (string s in columns)
//                {
//                    if (s.Equals(h.FileField, StringComparison.CurrentCultureIgnoreCase))
//                    {
//                        found = true;
//                        break;
//                    }
//                }

//                if (found == false)
//                {
//                    match = false;
//                    return match;
//                }
//            }
//            return match;
//        }
//        static bool VerifyColumnCount(string line)
//        {
//            bool isValid = true;
//            string[] columns = line.Split('~');
//            if (ColumnIndexList.Count != columns.Length)
//            {
//                isValid = false;
//                WriteToImportFileLog("Invalid line column count: " + line);
//                KM.Common.EmailFunctions.NotifyAdmin("SCG_CDS-Import - Dirty File", "Invalid line column count: " + line);
//            }

//            return isValid;
//        }
//        static void ProcessFlatFile(string file, ImportFile importFile)
//        {
//            WriteToImportFileLog(DateTime.Now + "-START--ProcessFlatFile()-- " + " File: " + importFile.FileName + " --");

//            importValueList = new List<Header>();
//            ImportRowCount = 0;
//            try
//            {
//                bool isValidFile = true;

//                ColumnIndexList = new List<KeyValuePair<int, string>>();
//                List<string> listS = new List<string>();

//                StreamReader sr = new StreamReader(file);
//                bool isFirstLine = true;
//                string line;
//                while ((line = sr.ReadLine()) != null)
//                {
//                    try
//                    {
//                        line = line.Replace("~", " ");

//                        if (importFile.IsQuoteEncapsulated == true)
//                        {
//                            if (importFile.ColumnDelimiter.Equals("comma", StringComparison.CurrentCultureIgnoreCase))
//                            {
//                                line = line.Replace("\",\"", "~");
//                            }

//                            line = line.Replace("\"", "");
//                        }

//                        if (!importFile.ColumnDelimiter.Equals("comma", StringComparison.CurrentCultureIgnoreCase))
//                        {
//                            //replace whatever the deliminator is with a comma
//                        }

//                        if (isFirstLine == true)
//                        {
//                            //lets create a column name index map
//                            string[] columns = line.Split('~');
//                            isValidFile = VerifyColumnHeaders(columns, importFile);
//                            if (isValidFile == false)
//                                break;
//                            int index = 0;
//                            foreach (string c in columns)
//                            {
//                                ColumnIndexList.Add(new KeyValuePair<int, string>(index, c));
//                                index++;
//                            }
//                        }
//                        else
//                        {
//                            if (!string.IsNullOrEmpty(line))
//                            {
//                                isValidFile = VerifyColumnCount(line);
//                                if (isValidFile == false)
//                                    break;
//                                else
//                                    listS.Add(line);
//                            }
//                        }

//                        isFirstLine = false;
//                    }
//                    catch
//                    {
//                        isValidFile = false;
//                    }
//                }

//                sr.Close();
//                sr.Dispose();
//                if (isValidFile == false)
//                {
//                    //email bad file to Corey and exit
//                    EmailBadFile(file);
//                }
//                else
//                {
//                    ImportRowCount = listS.Count;
//                    WriteToImportFileLog("Total rows to import: " + ImportRowCount.ToString());
//                    int counter = 0;
//                    int batch = 200;
//                    List<Task<List<Header>>> tasks = new List<Task<List<Header>>>();

//                    while (counter < ImportRowCount)
//                    {
//                        List<string> subBatch = listS.Skip(counter).Take(batch).ToList();
//                        int startIndex = counter;

//                        tasks.Add(Task<List<Header>>.Factory.StartNew(() =>
//                        {
//                            List<Header> headers = new List<Header>();
//                            Program p = new Program();
//                            headers = p.ReturnImportList(subBatch, importFile, startIndex);
//                            return headers;
//                        }
//                        ));
//                        counter = counter + batch;
//                    }

//                    Console.WriteLine(DateTime.Now + " Start multithread processing....wait");
//                    Task.WaitAll(tasks.ToArray());
//                    Console.WriteLine(DateTime.Now + " Done multithread processing....wait");
//                    int taskCount = 1;
//                    int batchCount = 0;

//                    List<EmailClean> dirtyEmailList = new List<EmailClean>();
//                    foreach (Task<List<Header>> t in tasks)
//                    {
//                        List<EmailClean> cleanEmail = new List<EmailClean>();
//                        Program p = new Program();
//                        int min = t.Result.Min(x => x.RowIndex);
//                        int max = t.Result.Max(x => x.RowIndex);
//                        cleanEmail = p.ReturnDirtyEmailList(t.Result, min, max);
//                        dirtyEmailList.AddRange(cleanEmail);
//                    }

//                    Console.WriteLine(DateTime.Now + " Start clean email....wait");

//                    foreach (EmailClean ec in dirtyEmailList)
//                    {
//                        if (string.IsNullOrEmpty(ec.EmailAddress))
//                            ec.EmailAddress = ec.CRD + "-" + ec.SubscriberID + "@" + ec.PubCode + ".kmpsgroup.com";
//                    }

//                    List<string> emails = new List<string>();
//                    foreach (EmailClean ec in dirtyEmailList)
//                    {
//                        if (!emails.Contains(ec.EmailAddress))
//                        {
//                            emails.Add(ec.EmailAddress);
//                        }
//                        else
//                        {
//                            ec.EmailAddress = ec.CRD + "-" + ec.SubscriberID + "@" + ec.PubCode + ".kmpsgroup.com";
//                            emails.Add(ec.EmailAddress);
//                        }
//                    }

//                    Console.WriteLine(DateTime.Now + " End clean email....wait");

//                    int zCount = 1;
//                    foreach (Task<List<Header>> t in tasks)
//                    {
//                        Console.WriteLine("Set Email: " + zCount.ToString() + " of " + tasks.Count().ToString());
//                        foreach (Header h in t.Result.Where(x => x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase)).ToList())
//                        {
//                            EmailClean c = dirtyEmailList.Single(x => x.RowIndex == h.RowIndex);
//                            h.Value = c.EmailAddress;
//                        }

//                        foreach (Header h in t.Result)
//                        {
//                            if (h.IsUDF == true && h.ECNField.Equals("DEMO7", StringComparison.CurrentCultureIgnoreCase))
//                            {
//                                if (h.Value.Equals("D", StringComparison.CurrentCultureIgnoreCase))
//                                    h.Value = "B";
//                                else if (h.Value.Equals("B", StringComparison.CurrentCultureIgnoreCase))
//                                    h.Value = "C";
//                                else if (h.Value.Equals("P", StringComparison.CurrentCultureIgnoreCase))
//                                    h.Value = "A";
//                                else
//                                    h.Value = "A";
//                            }
//                        }
//                        zCount++;
//                    }

//                    //update ecn
//                    foreach (Task<List<Header>> t in tasks)
//                    {
//                        int min = t.Result.Min(x => x.RowIndex);
//                        int max = t.Result.Max(x => x.RowIndex);

//                        batchCount += t.Result.Count / importFile.HeaderMap.Count;

//                        StringBuilder log = new StringBuilder();
//                        log.AppendLine("*****TASK*****");
//                        log.AppendLine("TaskCount: " + taskCount.ToString());
//                        log.AppendLine("Min: " + min.ToString());
//                        log.AppendLine("Max: " + max.ToString());
//                        log.AppendLine("TaskRows: " + t.Result.Count.ToString());
//                        log.AppendLine("BatchCount: " + batchCount.ToString());

//                        WriteToImportFileLog(log.ToString());

//                        ImportToEcn(t.Result, importFile, min);

//                        taskCount++;
//                    }
//                }

//                #region Old - non threaded
//                //foreach (string s in listS)
//                //{
//                //    Console.WriteLine(rowIndex.ToString() + " of " + ImportRowCount.ToString());
//                //    string[] values = s.Split(',');
//                //    foreach (KeyValuePair<int, string> kvp in ColumnIndexList)
//                //    {
//                //        string columnValue = values.ElementAt(kvp.Key);
//                //        if (importFile.HeaderMap.Exists(x => x.FileField.Equals(kvp.Value, StringComparison.CurrentCultureIgnoreCase)))
//                //        {
//                //            Header ifHeader = importFile.HeaderMap.Single(x => x.FileField.Equals(kvp.Value, StringComparison.CurrentCultureIgnoreCase));

//                //            if (importValueList.Count(x => x.ECNField.Equals(ifHeader.ECNField) && x.RowIndex == rowIndex) == 0)
//                //            {
//                //                Header valueHeader = new Header();
//                //                valueHeader.RowIndex = rowIndex;
//                //                valueHeader.Value = CleanXMLString(columnValue);
//                //                valueHeader.ECNField = ifHeader.ECNField;
//                //                valueHeader.FileField = ifHeader.FileField;
//                //                valueHeader.IsUDF = ifHeader.IsUDF;
//                //                importValueList.Add(valueHeader);
//                //            }
//                //            else
//                //            {
//                //                importValueList.Single(x => x.ECNField.Equals(ifHeader.ECNField) && x.RowIndex == rowIndex).Value += "," + columnValue;
//                //            }
//                //        }
//                //    }
//                //    rowIndex++;
//                //}
//                #endregion
//            }
//            catch (Exception ex)
//            {
//                StringBuilder sbLog = new StringBuilder();
//                sbLog.AppendLine("**********************");
//                sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
//                sbLog.AppendLine("-- Message --");
//                if (ex.Message != null)
//                    sbLog.AppendLine(ex.Message);
//                sbLog.AppendLine("-- InnerException --");
//                if (ex.InnerException != null)
//                    sbLog.AppendLine(ex.InnerException.ToString());
//                sbLog.AppendLine("-- Stack Trace --");
//                if (ex.StackTrace != null)
//                    sbLog.AppendLine(ex.StackTrace);
//                sbLog.AppendLine("**********************");
//                KM.Common.EmailFunctions.NotifyAdmin("SCG_CDS-Import", sbLog.ToString());
//                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SCG_CDS-Import.Program.ProcessFlatFile", KMCommon_Application, "SCG_CDS-Import: Unhandled Exception", -1, importFile.CustomerID);
//                WriteToImportFileLog(sbLog.ToString());
//            }
//        }
//        static void AddToImportList(List<string> importRows, ImportFile importFile, int startIndex)
//        {
//            List<Header> tempImport = new List<Header>();
//            int rowIndex = startIndex;
//            foreach (string s in importRows)
//            {
//                if (!string.IsNullOrEmpty(s))
//                {
//                    string[] values = s.Split(',');
//                    foreach (KeyValuePair<int, string> kvp in ColumnIndexList)//list of column headers from the parse file process
//                    {
//                        string columnValue = values.ElementAt(kvp.Key);
//                        if (importFile.HeaderMap.Exists(x => x.FileField.Equals(kvp.Value, StringComparison.CurrentCultureIgnoreCase)))
//                        {
//                            Header ifHeader = importFile.HeaderMap.Single(x => x.FileField.Equals(kvp.Value, StringComparison.CurrentCultureIgnoreCase));

//                            if (tempImport.Count(x => x.ECNField.Equals(ifHeader.ECNField) && x.RowIndex == rowIndex) == 0)
//                            {
//                                Header valueHeader = new Header();
//                                valueHeader.RowIndex = rowIndex;
//                                valueHeader.Value = CleanXMLString(columnValue);
//                                valueHeader.ECNField = ifHeader.ECNField;
//                                valueHeader.FileField = ifHeader.FileField;
//                                valueHeader.IsUDF = ifHeader.IsUDF;
//                                tempImport.Add(valueHeader);
//                            }
//                            else
//                            {
//                                tempImport.Single(x => x.ECNField.Equals(ifHeader.ECNField) && x.RowIndex == rowIndex).Value += "," + CleanXMLString(columnValue);
//                            }
//                        }
//                    }
//                }
//                rowIndex++;
//            }
//            importValueList.AddRange(tempImport);
//        }
//        private List<Header> ReturnImportList(List<string> importRows, ImportFile importFile, int startIndex)
//        {
//            List<Header> tempImport = new List<Header>();
//            int rowIndex = startIndex;
//            foreach (string s in importRows)
//            {
//                if (!string.IsNullOrEmpty(s))
//                {
//                    string[] values = s.Split('~');
//                    foreach (KeyValuePair<int, string> kvp in ColumnIndexList)//list of column headers from the parse file process
//                    {
//                        string columnValue = values.ElementAt(kvp.Key);
//                        if (importFile.HeaderMap.Exists(x => x.FileField.Equals(kvp.Value, StringComparison.CurrentCultureIgnoreCase)))
//                        {
//                            Header ifHeader = importFile.HeaderMap.Single(x => x.FileField.Equals(kvp.Value, StringComparison.CurrentCultureIgnoreCase));

//                            if (tempImport.Count(x => x.ECNField.Equals(ifHeader.ECNField) && x.RowIndex == rowIndex) == 0)
//                            {
//                                Header valueHeader = new Header();
//                                valueHeader.RowIndex = rowIndex;
//                                valueHeader.Value = CleanXMLString(columnValue);
//                                valueHeader.ECNField = ifHeader.ECNField;
//                                valueHeader.FileField = ifHeader.FileField;
//                                valueHeader.IsUDF = ifHeader.IsUDF;
//                                tempImport.Add(valueHeader);
//                            }
//                            else
//                            {
//                                tempImport.Single(x => x.ECNField.Equals(ifHeader.ECNField) && x.RowIndex == rowIndex).Value += "," + CleanXMLString(columnValue);
//                            }
//                        }
//                    }
//                }
//                rowIndex++;
//            }

//            return tempImport;
//        }
//        static void EmailBadFile(string file)
//        {
//            int lastSlash = file.LastIndexOf("\\");//41
//            int totalLen = file.Length;//62
//            string fileName = file.Substring(lastSlash + 1, totalLen - (lastSlash + 1));
//            string badPath = ConfigurationManager.AppSettings["BadFiles"] + fileName;

//            File.Move(file, badPath);

//            SmtpClient smtpServer = new SmtpClient(ConfigurationManager.AppSettings["KMCommon_SmtpServer"]);
//            MailMessage message = new MailMessage();
//            message.Priority = MailPriority.High;
//            message.IsBodyHtml = false;
//            message.To.Add(ConfigurationManager.AppSettings["EmailManager"].ToString());
//            message.Bcc.Add(ConfigurationManager.AppSettings["EmailBCC"].ToString());
//            MailAddress msgSender = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"].ToString());
//            message.From = msgSender;
//            message.Subject = "SCG_CDS-Import - Bad File";
//            message.Body = "Bad Customer File";
//            Attachment att = new Attachment(badPath);
//            message.Attachments.Add(att);

//            smtpServer.Send(message);

//            //move bad files
//            //get just the file name itself

//        }
//        #endregion

//        #region Send to ECN
//        static void ImportToEcn(List<Header> results, ImportFile importFile, int rowIndex)
//        {
//            int counter = 1;
//            int processedCount = 0;
//            //int rowIndex = 0;

//            StringBuilder xmlProfile = new StringBuilder();
//            StringBuilder xmlUDF = new StringBuilder();

//            int batchCount = results.Count / importFile.HeaderMap.Count;

//            try
//            {
//                while (counter <= batchCount)
//                {
//                    WriteToImportFileLog(counter.ToString() + " of " + batchCount.ToString());
//                    List<Header> currentRowValueList = results.Where(x => x.RowIndex == rowIndex).ToList();
//                    List<Header> crProfileList = currentRowValueList.Where(x => x.IsUDF == false).ToList();
//                    List<Header> crUdfList = currentRowValueList.Where(x => x.IsUDF == true).ToList();

//                    xmlProfile.AppendLine("<Emails>");
//                    StringBuilder user1 = new StringBuilder();
//                    foreach (Header hv in crProfileList)
//                    {
//                        if (hv.ECNField.ToString().Equals("voice", StringComparison.CurrentCultureIgnoreCase) || hv.ECNField.ToString().Equals("fax", StringComparison.CurrentCultureIgnoreCase) || hv.ECNField.ToString().Equals("mobile", StringComparison.CurrentCultureIgnoreCase))
//                            hv.Value = CleanPhoneNumber(hv.Value);
//                        if (hv.FileField.Equals("TRN", StringComparison.CurrentCultureIgnoreCase))
//                        {
//                            TransactionType t = new TransactionType();
//                            t.Pub = importFile.FileName;
//                            t.AddCount = 0;
//                            t.ChangeCount = 0;
//                            t.DeleteCount = 0;

//                            if (hv.Value.ToString().Equals("D", StringComparison.CurrentCultureIgnoreCase))
//                                t.DeleteCount = 1;
//                            else if (hv.Value.ToString().Equals("C", StringComparison.CurrentCultureIgnoreCase))
//                                t.ChangeCount = 1;
//                            else if (hv.Value.ToString().Equals("A", StringComparison.CurrentCultureIgnoreCase))
//                                t.AddCount = 1;

//                            TransactionReport.Add(t);

//                            if (hv.Value.ToString().Equals("D", StringComparison.CurrentCultureIgnoreCase))
//                                hv.Value = "U";
//                            else
//                                hv.Value = string.Empty;
//                        }
//                        xmlProfile.AppendLine("<" + hv.ECNField.ToLower() + ">" + hv.Value + "</" + hv.ECNField.ToLower() + ">");
//                    }
//                    try
//                    {
//                        user1.Append(crProfileList.SingleOrDefault(x => x.ECNField.ToString().Equals(ECN_ProfileFields.FirstName.ToString(), StringComparison.CurrentCultureIgnoreCase)).Value);
//                    }
//                    catch { }
//                    user1.Append(" ");
//                    try
//                    {
//                        user1.Append(crProfileList.SingleOrDefault(x => x.ECNField.ToString().Equals(ECN_ProfileFields.LastName.ToString(), StringComparison.CurrentCultureIgnoreCase)).Value);
//                    }
//                    catch { }
//                    user1.Append(" ");
//                    try
//                    {
//                        user1.Append(crProfileList.SingleOrDefault(x => x.ECNField.ToString().Equals(ECN_ProfileFields.State.ToString(), StringComparison.CurrentCultureIgnoreCase)).Value);
//                    }
//                    catch { }

//                    xmlProfile.AppendLine("<notes>" + "<![CDATA[ [SCG_CDS-Import Engine] [" + DateTime.Now.ToString() + "] ]]> " + "</notes>");
//                    xmlProfile.AppendLine("<user1>" + user1 + "</user1>");
//                    xmlProfile.AppendLine("</Emails>");

//                    xmlUDF.AppendLine("<row>");
//                    string email = string.Empty;
//                    if (crProfileList.Exists(x => x.ECNField.ToString().Equals(ECN_ProfileFields.EmailAddress.ToString(), StringComparison.CurrentCultureIgnoreCase)))
//                        email = crProfileList.FirstOrDefault(x => x.ECNField.ToString().Equals(ECN_ProfileFields.EmailAddress.ToString(), StringComparison.CurrentCultureIgnoreCase)).Value.ToString();
//                    xmlUDF.AppendLine("<ea kv=\"" + user1 + "\">" + email + "</ea>");

//                    foreach (Header hv in crUdfList)
//                    {
//                        KeyValuePair<int, string> kvp = hUDFFields.SingleOrDefault(x => x.Value.ToString().Equals(hv.ECNField.ToString(), StringComparison.CurrentCultureIgnoreCase));
//                        if (kvp.Key > 0)
//                        {
//                            //may have to check the xmlUDF stringbuilder for the existance of the udf id and if it exists append to the value
//                            xmlUDF.AppendLine("<udf id=\"" + kvp.Key.ToString() + "\"><v>" + hv.Value + "</v></udf>");
//                        }
//                    }
//                    xmlUDF.AppendLine("</row>");

//                    //last
//                    counter++;
//                    rowIndex++;
//                    processedCount++;

//                    //if counter is 5000 then update
//                    if (processedCount == batchCount || processedCount == 5000)
//                    {
//                        UpdateToDB(xmlProfile.ToString(), xmlUDF.ToString(), importFile);
//                        xmlProfile = new StringBuilder();
//                        xmlUDF = new StringBuilder();
//                        processedCount = 0;
//                    }
//                }

//                if (xmlProfile.Length > 0)
//                {
//                    UpdateToDB(xmlProfile.ToString(), xmlUDF.ToString(), importFile);
//                    xmlProfile = new StringBuilder();
//                    xmlUDF = new StringBuilder();
//                }
//                WriteToImportFileLog(DateTime.Now + "-END--ImportToECN() " + " File: " + importFile.FileName + " --");
//            }
//            catch (Exception ex)
//            {
//                StringBuilder sbLog = new StringBuilder();
//                sbLog.AppendLine("**********************");
//                sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
//                sbLog.AppendLine("-- Message --");
//                if (ex.Message != null)
//                    sbLog.AppendLine(ex.Message);
//                sbLog.AppendLine("-- InnerException --");
//                if (ex.InnerException != null)
//                    sbLog.AppendLine(ex.InnerException.ToString());
//                sbLog.AppendLine("-- Stack Trace --");
//                if (ex.StackTrace != null)
//                    sbLog.AppendLine(ex.StackTrace);
//                sbLog.AppendLine("**********************");
//                KM.Common.EmailFunctions.NotifyAdmin("SCG_CDS-Import", sbLog.ToString());
//                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SCG_CDS-Import.Program.ImportToECN", KMCommon_Application, "SCG_CDS-Import: Unhandled Exception", -1, importFile.CustomerID);
//                WriteToImportFileLog(sbLog.ToString());
//            }
//        }
//        static void SetDemo7Values(ImportFile importFile)
//        {
//            try
//            {
//                foreach (Header h in importValueList.Where(x => x.IsUDF == true && x.ECNField.Equals("DEMO7", StringComparison.CurrentCultureIgnoreCase)).ToList())
//                {
//                    if (h.Value.Equals("D", StringComparison.CurrentCultureIgnoreCase))
//                        h.Value = "B";
//                    else if (h.Value.Equals("B", StringComparison.CurrentCultureIgnoreCase))
//                        h.Value = "C";
//                    else if (h.Value.Equals("P", StringComparison.CurrentCultureIgnoreCase))
//                        h.Value = "A";
//                    else
//                        h.Value = "A";
//                }
//            }
//            catch (Exception ex)
//            {
//                StringBuilder sbLog = new StringBuilder();
//                sbLog.AppendLine("**********************");
//                sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
//                sbLog.AppendLine("-- Message --");
//                if (ex.Message != null)
//                    sbLog.AppendLine(ex.Message);
//                sbLog.AppendLine("-- InnerException --");
//                if (ex.InnerException != null)
//                    sbLog.AppendLine(ex.InnerException.ToString());
//                sbLog.AppendLine("-- Stack Trace --");
//                if (ex.StackTrace != null)
//                    sbLog.AppendLine(ex.StackTrace);
//                sbLog.AppendLine("**********************");
//                WriteToImportFileLog(sbLog.ToString());
//            }

//        }
//        static void RemoveDuplicateEmails()
//        {
//            List<EmailClean> listEC = new List<EmailClean>();
//            int counter = 1;
//            int rowIndex = 0;
//            try
//            {
//                while (counter <= ImportRowCount)
//                {
//                    WriteToImportFileLog(counter.ToString() + " of " + ImportRowCount.ToString());
//                    EmailClean ec = new EmailClean();
//                    List<Header> pMaster = importValueList.Where(x => x.RowIndex == rowIndex && x.IsUDF == false).ToList();

//                    Header hEmail = pMaster.Single(x => x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase));//importValueList.Single(x => x.RowIndex == rowIndex && x.IsUDF == false && x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase));
//                    ec.EmailAddress = hEmail.Value;
//                    //foreach (Header h in importValueList.Where(x => x.RowIndex == rowIndex && x.IsUDF == false && x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase)).ToList())
//                    //{
//                    //    ec.EmailAddress = h.Value;
//                    //}

//                    Header hCRD = pMaster.Single(x => x.ECNField.Equals("CRD", StringComparison.CurrentCultureIgnoreCase));//importValueList.Single(x => x.RowIndex == rowIndex && x.IsUDF == false && x.ECNField.Equals("CRD", StringComparison.CurrentCultureIgnoreCase));
//                    ec.CRD = hCRD.Value;

//                    //foreach (Header h in importValueList.Where(x => x.RowIndex == rowIndex && x.IsUDF == false && x.ECNField.Equals("CRD", StringComparison.CurrentCultureIgnoreCase)).ToList())
//                    //{
//                    //    ec.CRD = h.Value;
//                    //}

//                    Header hPub = pMaster.Single(x => x.ECNField.Equals("PUB", StringComparison.CurrentCultureIgnoreCase));//importValueList.Single(x => x.RowIndex == rowIndex && x.IsUDF == false && x.ECNField.Equals("PUB", StringComparison.CurrentCultureIgnoreCase));
//                    ec.PubCode = hPub.Value;
//                    //foreach (Header h in importValueList.Where(x => x.RowIndex == rowIndex && x.IsUDF == false && x.ECNField.Equals("PUB", StringComparison.CurrentCultureIgnoreCase)).ToList())
//                    //{
//                    //    ec.EmailAddress = h.Value;
//                    //}     

//                    Header hSub = importValueList.Single(x => x.RowIndex == rowIndex && x.IsUDF == true && x.ECNField.Equals("CDSID", StringComparison.CurrentCultureIgnoreCase));
//                    ec.SubscriberID = hSub.Value;
//                    //foreach (Header h in importValueList.Where(x => x.RowIndex == rowIndex && x.IsUDF == true && x.ECNField.Equals("CDSID",StringComparison.CurrentCultureIgnoreCase)).ToList())
//                    //{
//                    //    ec.SubscriberID = h.Value;
//                    //}

//                    ec.RowIndex = rowIndex;
//                    listEC.Add(ec);

//                    counter++;
//                    rowIndex++;
//                }

//                WriteToImportFileLog("Create clean list");
//                foreach (EmailClean ec in listEC)
//                {
//                    if (string.IsNullOrEmpty(ec.EmailAddress))
//                        ec.EmailAddress = ec.CRD + "-" + ec.SubscriberID + "@" + ec.PubCode + ".kmpsgroup.com";
//                }

//                WriteToImportFileLog("Ensure unique email");
//                List<string> emails = new List<string>();
//                foreach (EmailClean ec in listEC)
//                {
//                    Header hv = importValueList.Single(x => x.RowIndex == ec.RowIndex && x.IsUDF == false && x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase));
//                    if (!emails.Contains(ec.EmailAddress))
//                    {
//                        emails.Add(ec.EmailAddress);
//                        hv.Value = ec.EmailAddress;
//                    }
//                    else
//                    {
//                        ec.EmailAddress = ec.CRD + "-" + ec.SubscriberID + "@" + ec.PubCode + ".kmpsgroup.com";
//                        emails.Add(ec.EmailAddress);
//                        hv.Value = ec.EmailAddress;
//                    }

//                    importValueList.Single(x => x.RowIndex == ec.RowIndex && x.IsUDF == false && x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase)).Value = hv.Value;
//                }
//            }
//            catch (Exception ex)
//            {
//                StringBuilder sbLog = new StringBuilder();
//                sbLog.AppendLine("**********************");
//                sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
//                sbLog.AppendLine("-- Message --");
//                if (ex.Message != null)
//                    sbLog.AppendLine(ex.Message);
//                sbLog.AppendLine("-- InnerException --");
//                if (ex.InnerException != null)
//                    sbLog.AppendLine(ex.InnerException.ToString());
//                sbLog.AppendLine("-- Stack Trace --");
//                if (ex.StackTrace != null)
//                    sbLog.AppendLine(ex.StackTrace);
//                sbLog.AppendLine("**********************");
//                WriteToImportFileLog(sbLog.ToString());
//            }
//            //now update email address in master list
//            //List<Header> currentRowValueList = importValueList.Where(x => x.RowIndex == rowIndex).ToList();
//            //foreach (EmailClean ec in listEC)
//            //{
//            //    foreach (Header hv in importValueList.Where(x => x.RowIndex == ec.RowIndex && x.IsUDF == false && x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase)).ToList())
//            //    {
//            //        hv.Value = ec.EmailAddress;
//            //    }
//            //}
//        }
//        static void RemoveDuplicateEmails(int startIndex, int stopIndex)
//        {
//            List<EmailClean> listEC = new List<EmailClean>();
//            int counter = startIndex;
//            int rowIndex = startIndex;
//            try
//            {
//                while (counter <= stopIndex)
//                {
//                    if (importValueList.Exists(x => x.RowIndex == rowIndex))
//                    {
//                        EmailClean ec = new EmailClean();
//                        List<Header> pMaster = importValueList.Where(x => x.RowIndex == rowIndex && x.IsUDF == false).ToList();

//                        Header hEmail = pMaster.Single(x => x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase));//importValueList.Single(x => x.RowIndex == rowIndex && x.IsUDF == false && x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase));
//                        ec.EmailAddress = hEmail.Value;

//                        Header hCRD = pMaster.Single(x => x.ECNField.Equals("CRD", StringComparison.CurrentCultureIgnoreCase));//importValueList.Single(x => x.RowIndex == rowIndex && x.IsUDF == false && x.ECNField.Equals("CRD", StringComparison.CurrentCultureIgnoreCase));
//                        ec.CRD = hCRD.Value;

//                        Header hPub = pMaster.Single(x => x.ECNField.Equals("PUB", StringComparison.CurrentCultureIgnoreCase));//importValueList.Single(x => x.RowIndex == rowIndex && x.IsUDF == false && x.ECNField.Equals("PUB", StringComparison.CurrentCultureIgnoreCase));
//                        ec.PubCode = hPub.Value;

//                        Header hSub = importValueList.Single(x => x.RowIndex == rowIndex && x.IsUDF == true && x.ECNField.Equals("CDSID", StringComparison.CurrentCultureIgnoreCase));
//                        ec.SubscriberID = hSub.Value;

//                        ec.RowIndex = rowIndex;
//                        listEC.Add(ec);
//                    }
//                    counter++;
//                    rowIndex++;
//                }

//                foreach (EmailClean ec in listEC)
//                {
//                    if (string.IsNullOrEmpty(ec.EmailAddress))
//                        ec.EmailAddress = ec.CRD + "-" + ec.SubscriberID + "@" + ec.PubCode + ".kmpsgroup.com";
//                }

//                List<string> emails = new List<string>();
//                foreach (EmailClean ec in listEC)
//                {
//                    Header hv = importValueList.Single(x => x.RowIndex == ec.RowIndex && x.IsUDF == false && x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase));
//                    if (!emails.Contains(ec.EmailAddress))
//                    {
//                        emails.Add(ec.EmailAddress);
//                        hv.Value = ec.EmailAddress;
//                    }
//                    else
//                    {
//                        ec.EmailAddress = ec.CRD + "-" + ec.SubscriberID + "@" + ec.PubCode + ".kmpsgroup.com";
//                        emails.Add(ec.EmailAddress);
//                        hv.Value = ec.EmailAddress;
//                    }

//                    importValueList.Single(x => x.RowIndex == ec.RowIndex && x.IsUDF == false && x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase)).Value = hv.Value;
//                }
//            }
//            catch (Exception ex)
//            {
//                StringBuilder sbLog = new StringBuilder();
//                sbLog.AppendLine("**********************");
//                sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
//                sbLog.AppendLine("-- Message --");
//                if (ex.Message != null)
//                    sbLog.AppendLine(ex.Message);
//                sbLog.AppendLine("-- InnerException --");
//                if (ex.InnerException != null)
//                    sbLog.AppendLine(ex.InnerException.ToString());
//                sbLog.AppendLine("-- Stack Trace --");
//                if (ex.StackTrace != null)
//                    sbLog.AppendLine(ex.StackTrace);
//                sbLog.AppendLine("**********************");
//                WriteToImportFileLog(sbLog.ToString());
//            }
//        }
//        private List<EmailClean> ReturnDirtyEmailList(List<Header> headers, int startIndex, int stopIndex)
//        {
//            //int startIndex, int stopIndex,
//            List<EmailClean> listEC = new List<EmailClean>();
//            int counter = startIndex;
//            int rowIndex = startIndex;

//            try
//            {
//                while (counter <= stopIndex)//stopIndex
//                {

//                    if (headers.Exists(x => x.RowIndex == rowIndex))
//                    {
//                        EmailClean ec = new EmailClean();
//                        List<Header> pMaster = headers.Where(x => x.RowIndex == rowIndex && x.IsUDF == false).ToList();

//                        Header hEmail = pMaster.Single(x => x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase));//importValueList.Single(x => x.RowIndex == rowIndex && x.IsUDF == false && x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase));
//                        ec.EmailAddress = hEmail.Value;

//                        Header hCRD = pMaster.Single(x => x.ECNField.Equals("CRD", StringComparison.CurrentCultureIgnoreCase));//importValueList.Single(x => x.RowIndex == rowIndex && x.IsUDF == false && x.ECNField.Equals("CRD", StringComparison.CurrentCultureIgnoreCase));
//                        ec.CRD = hCRD.Value;

//                        Header hPub = pMaster.Single(x => x.ECNField.Equals("PUB", StringComparison.CurrentCultureIgnoreCase));//importValueList.Single(x => x.RowIndex == rowIndex && x.IsUDF == false && x.ECNField.Equals("PUB", StringComparison.CurrentCultureIgnoreCase));
//                        ec.PubCode = hPub.Value;

//                        Header hSub = headers.Single(x => x.RowIndex == rowIndex && x.IsUDF == true && x.ECNField.Equals("CDSID", StringComparison.CurrentCultureIgnoreCase));
//                        ec.SubscriberID = hSub.Value;

//                        ec.RowIndex = rowIndex;
//                        listEC.Add(ec);
//                    }
//                    counter++;
//                    rowIndex++;
//                }
//            }
//            catch (Exception ex)
//            {
//                StringBuilder sbLog = new StringBuilder();
//                sbLog.AppendLine("**********************");
//                sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
//                sbLog.AppendLine("-- Message --");
//                if (ex.Message != null)
//                    sbLog.AppendLine(ex.Message);
//                sbLog.AppendLine("-- InnerException --");
//                if (ex.InnerException != null)
//                    sbLog.AppendLine(ex.InnerException.ToString());
//                sbLog.AppendLine("-- Stack Trace --");
//                if (ex.StackTrace != null)
//                    sbLog.AppendLine(ex.StackTrace);
//                sbLog.AppendLine("**********************");
//                WriteToImportFileLog(sbLog.ToString());
//            }

//            return listEC;
//        }
//        private List<EmailClean> CleanEmails(List<Header> headers, int startIndex, int stopIndex)
//        {
//            //int startIndex, int stopIndex,
//            List<EmailClean> listEC = new List<EmailClean>();
//            int counter = startIndex;
//            int rowIndex = startIndex;

//            try
//            {
//                while (counter <= stopIndex)//stopIndex
//                {

//                    if (headers.Exists(x => x.RowIndex == rowIndex))
//                    {
//                        EmailClean ec = new EmailClean();
//                        List<Header> pMaster = headers.Where(x => x.RowIndex == rowIndex && x.IsUDF == false).ToList();

//                        Header hEmail = pMaster.Single(x => x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase));//importValueList.Single(x => x.RowIndex == rowIndex && x.IsUDF == false && x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase));
//                        ec.EmailAddress = hEmail.Value;

//                        Header hCRD = pMaster.Single(x => x.ECNField.Equals("CRD", StringComparison.CurrentCultureIgnoreCase));//importValueList.Single(x => x.RowIndex == rowIndex && x.IsUDF == false && x.ECNField.Equals("CRD", StringComparison.CurrentCultureIgnoreCase));
//                        ec.CRD = hCRD.Value;

//                        Header hPub = pMaster.Single(x => x.ECNField.Equals("PUB", StringComparison.CurrentCultureIgnoreCase));//importValueList.Single(x => x.RowIndex == rowIndex && x.IsUDF == false && x.ECNField.Equals("PUB", StringComparison.CurrentCultureIgnoreCase));
//                        ec.PubCode = hPub.Value;

//                        Header hSub = headers.Single(x => x.RowIndex == rowIndex && x.IsUDF == true && x.ECNField.Equals("CDSID", StringComparison.CurrentCultureIgnoreCase));
//                        ec.SubscriberID = hSub.Value;

//                        ec.RowIndex = rowIndex;
//                        listEC.Add(ec);
//                    }
//                    counter++;
//                    rowIndex++;
//                }

//                foreach (EmailClean ec in listEC)
//                {
//                    if (string.IsNullOrEmpty(ec.EmailAddress))
//                        ec.EmailAddress = ec.CRD + "-" + ec.SubscriberID + "@" + ec.PubCode + ".kmpsgroup.com";
//                }

//                List<string> emails = new List<string>();
//                foreach (EmailClean ec in listEC)
//                {
//                    //Header hv = headers.Single(x => x.RowIndex == ec.RowIndex && x.IsUDF == false && x.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase));
//                    if (!emails.Contains(ec.EmailAddress))
//                    {
//                        emails.Add(ec.EmailAddress);
//                        //hv.Value = ec.EmailAddress;
//                    }
//                    else
//                    {
//                        ec.EmailAddress = ec.CRD + "-" + ec.SubscriberID + "@" + ec.PubCode + ".kmpsgroup.com";
//                        emails.Add(ec.EmailAddress);
//                        //hv.Value = ec.EmailAddress;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                StringBuilder sbLog = new StringBuilder();
//                sbLog.AppendLine("**********************");
//                sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
//                sbLog.AppendLine("-- Message --");
//                if (ex.Message != null)
//                    sbLog.AppendLine(ex.Message);
//                sbLog.AppendLine("-- InnerException --");
//                if (ex.InnerException != null)
//                    sbLog.AppendLine(ex.InnerException.ToString());
//                sbLog.AppendLine("-- Stack Trace --");
//                if (ex.StackTrace != null)
//                    sbLog.AppendLine(ex.StackTrace);
//                sbLog.AppendLine("**********************");
//                WriteToImportFileLog(sbLog.ToString());
//            }

//            return listEC;
//        }
//        static void ImportToECN(ImportFile importFile)
//        {
//            if (importValueList != null || importValueList.Count == 0)
//            {
//                //Console.WriteLine(DateTime.Now + " Preparing threads for EmailClean");
//                //int startIndex = 0;
//                //int batch = 200;

//                //List<Task<List<EmailClean>>> tasks = new List<Task<List<EmailClean>>>();
//                //while (startIndex < ImportRowCount)
//                //{
//                //    int threadStartIndex = startIndex;
//                //    int threadStopIndex = threadStartIndex + batch;//200 - 401 - 602
//                //    List<Header> headers = importValueList.Where(x => x.RowIndex >= threadStartIndex && x.RowIndex <= threadStopIndex).ToList();//0-200, 201-401, 402-602, 
//                //    //do this in a new thread
//                //    tasks.Add(Task<List<EmailClean>>.Factory.StartNew(() =>
//                //        {
//                //            List<EmailClean> cleanEmail = new List<EmailClean>();
//                //            Program p = new Program();
//                //            cleanEmail = p.CleanEmails(headers, threadStartIndex, threadStopIndex);//threadStartIndex, threadStopIndex,
//                //            return cleanEmail;
//                //        }
//                //        ));
//                //    startIndex = threadStopIndex + 1;//201 402 
//                //}
//                //Console.WriteLine(DateTime.Now + " Start CleanEmails multithread processing....wait");
//                //Task.WaitAll(tasks.ToArray());
//                //Console.WriteLine(DateTime.Now + " Done CleanEmails multithread processing....wait");

//                //Console.WriteLine(DateTime.Now + " Start loop....wait");
//                //int totalTasks = tasks.Count;
//                //int countTask = 1;
//                //foreach (Task<List<EmailClean>> t in tasks)
//                //{
//                //    Console.WriteLine("task " + countTask.ToString() + " of " + totalTasks.ToString());
//                //    foreach (EmailClean c in t.Result)
//                //    {
//                //        foreach (Header h in importValueList)
//                //        {
//                //            if (h.RowIndex == c.RowIndex && h.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase))
//                //            {
//                //                h.Value = c.EmailAddress;
//                //                break;
//                //            }
//                //        }
//                //        //importValueList.Where(x => x.RowIndex == c.RowIndex).ToList().Single(y => y.ECNField.Equals("emailaddress", StringComparison.CurrentCultureIgnoreCase)).Value = c.EmailAddress;
//                //    }
//                //    countTask++;
//                //}

//                //Console.WriteLine(DateTime.Now + " End loop....wait");

//                WriteToImportFileLog(DateTime.Now + "-START--SetDemo7Values()-- ");
//                SetDemo7Values(importFile);
//                WriteToImportFileLog(DateTime.Now + "-END--SetDemo7Values()-- ");

//                WriteToImportFileLog(DateTime.Now + "-START--ImportToECN()-- " + " File: " + importFile.FileName + " --");
//                WriteToImportFileLog("Total profiles to import: " + ImportRowCount.ToString());
//                int counter = 1;
//                int processedCount = 0;
//                int rowIndex = 0;

//                StringBuilder xmlProfile = new StringBuilder();
//                StringBuilder xmlUDF = new StringBuilder();

//                try
//                {
//                    while (counter <= ImportRowCount)
//                    {
//                        WriteToImportFileLog(counter.ToString() + " of " + ImportRowCount.ToString());
//                        List<Header> currentRowValueList = importValueList.Where(x => x.RowIndex == rowIndex).ToList();
//                        List<Header> crProfileList = currentRowValueList.Where(x => x.IsUDF == false).ToList();
//                        List<Header> crUdfList = currentRowValueList.Where(x => x.IsUDF == true).ToList();

//                        xmlProfile.AppendLine("<Emails>");
//                        StringBuilder user1 = new StringBuilder();
//                        foreach (Header hv in crProfileList)
//                        {
//                            if (hv.ECNField.ToString().Equals("voice", StringComparison.CurrentCultureIgnoreCase) || hv.ECNField.ToString().Equals("fax", StringComparison.CurrentCultureIgnoreCase) || hv.ECNField.ToString().Equals("mobile", StringComparison.CurrentCultureIgnoreCase))
//                                hv.Value = CleanPhoneNumber(hv.Value);
//                            if (hv.FileField.Equals("TRN", StringComparison.CurrentCultureIgnoreCase))
//                            {
//                                TransactionType t = new TransactionType();
//                                t.Pub = importFile.FileName;
//                                t.AddCount = 0;
//                                t.ChangeCount = 0;
//                                t.DeleteCount = 0;

//                                if (hv.Value.ToString().Equals("D", StringComparison.CurrentCultureIgnoreCase))
//                                    t.DeleteCount = 1;
//                                else if (hv.Value.ToString().Equals("C", StringComparison.CurrentCultureIgnoreCase))
//                                    t.ChangeCount = 1;
//                                else if (hv.Value.ToString().Equals("A", StringComparison.CurrentCultureIgnoreCase))
//                                    t.AddCount = 1;

//                                TransactionReport.Add(t);

//                                if (hv.Value.ToString().Equals("D", StringComparison.CurrentCultureIgnoreCase))
//                                    hv.Value = "U";
//                                else
//                                    hv.Value = string.Empty;
//                            }
//                            xmlProfile.AppendLine("<" + hv.ECNField.ToLower() + ">" + hv.Value + "</" + hv.ECNField.ToLower() + ">");
//                        }
//                        try
//                        {
//                            user1.Append(crProfileList.SingleOrDefault(x => x.ECNField.ToString().Equals(ECN_ProfileFields.FirstName.ToString(), StringComparison.CurrentCultureIgnoreCase)).Value);
//                        }
//                        catch { }
//                        user1.Append(" ");
//                        try
//                        {
//                            user1.Append(crProfileList.SingleOrDefault(x => x.ECNField.ToString().Equals(ECN_ProfileFields.LastName.ToString(), StringComparison.CurrentCultureIgnoreCase)).Value);
//                        }
//                        catch { }
//                        user1.Append(" ");
//                        try
//                        {
//                            user1.Append(crProfileList.SingleOrDefault(x => x.ECNField.ToString().Equals(ECN_ProfileFields.State.ToString(), StringComparison.CurrentCultureIgnoreCase)).Value);
//                        }
//                        catch { }

//                        xmlProfile.AppendLine("<notes>" + "<![CDATA[ [SCG_CDS-Import Engine] [" + DateTime.Now.ToString() + "] ]]> " + "</notes>");
//                        xmlProfile.AppendLine("<user1>" + user1 + "</user1>");
//                        xmlProfile.AppendLine("</Emails>");

//                        xmlUDF.AppendLine("<row>");
//                        string email = string.Empty;
//                        if (crProfileList.Exists(x => x.ECNField.ToString().Equals(ECN_ProfileFields.EmailAddress.ToString(), StringComparison.CurrentCultureIgnoreCase)))
//                            email = crProfileList.FirstOrDefault(x => x.ECNField.ToString().Equals(ECN_ProfileFields.EmailAddress.ToString(), StringComparison.CurrentCultureIgnoreCase)).Value.ToString();
//                        xmlUDF.AppendLine("<ea kv=\"" + user1 + "\">" + email + "</ea>");

//                        foreach (Header hv in crUdfList)
//                        {
//                            KeyValuePair<int, string> kvp = hUDFFields.SingleOrDefault(x => x.Value.ToString().Equals(hv.ECNField.ToString(), StringComparison.CurrentCultureIgnoreCase));
//                            if (kvp.Key > 0)
//                            {
//                                //may have to check the xmlUDF stringbuilder for the existance of the udf id and if it exists append to the value
//                                xmlUDF.AppendLine("<udf id=\"" + kvp.Key.ToString() + "\"><v>" + hv.Value + "</v></udf>");
//                            }
//                        }
//                        xmlUDF.AppendLine("</row>");

//                        //last
//                        counter++;
//                        rowIndex++;
//                        processedCount++;

//                        //if counter is 5000 then update
//                        if (processedCount == ImportRowCount || processedCount == 5000)
//                        {
//                            UpdateToDB(xmlProfile.ToString(), xmlUDF.ToString(), importFile);
//                            xmlProfile = new StringBuilder();
//                            xmlUDF = new StringBuilder();
//                            processedCount = 0;
//                        }
//                    }

//                    if (xmlProfile.Length > 0)
//                    {
//                        UpdateToDB(xmlProfile.ToString(), xmlUDF.ToString(), importFile);
//                        xmlProfile = new StringBuilder();
//                        xmlUDF = new StringBuilder();
//                    }
//                    WriteToImportFileLog(DateTime.Now + "-END--ImportToECN() " + " File: " + importFile.FileName + " --");
//                }
//                catch (Exception ex)
//                {
//                    StringBuilder sbLog = new StringBuilder();
//                    sbLog.AppendLine("**********************");
//                    sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
//                    sbLog.AppendLine("-- Message --");
//                    if (ex.Message != null)
//                        sbLog.AppendLine(ex.Message);
//                    sbLog.AppendLine("-- InnerException --");
//                    if (ex.InnerException != null)
//                        sbLog.AppendLine(ex.InnerException.ToString());
//                    sbLog.AppendLine("-- Stack Trace --");
//                    if (ex.StackTrace != null)
//                        sbLog.AppendLine(ex.StackTrace);
//                    sbLog.AppendLine("**********************");
//                    KM.Common.EmailFunctions.NotifyAdmin("SCG_CDS-Import", sbLog.ToString());
//                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SCG_CDS-Import.Program.ImportToECN", KMCommon_Application, "SCG_CDS-Import: Unhandled Exception", -1, importFile.CustomerID);
//                    WriteToImportFileLog(sbLog.ToString());
//                }
//            }
//            else
//                WriteToImportFileLog("importValueList was null or Count = 0 - FileName: " + importFile.FileName);

//        }
//        static void UpdateToDB(string xmlProfile, string xmlUDF, ImportFile importFile)
//        {
//            WriteToImportFileLog(DateTime.Now + "-START--UpdateToDB()------------------------------------");
//            xmlProfile = xmlProfile.Replace("&", "and");
//            xmlUDF = xmlUDF.Replace("&", "and");
//            WriteToImportFileLog("***XMLPROFILE***");
//            WriteToImportFileLog(xmlProfile);
//            WriteToImportFileLog("***XMLPROFILE***");
//            WriteToImportFileLog("***XMLUDF***");
//            WriteToImportFileLog(xmlUDF);
//            WriteToImportFileLog("***XMLUDF***");
//            try
//            {
//                List<ECN_Framework_Entities.Accounts.User> userList = ECN_Framework_BusinessLayer.Accounts.User.GetByCustomerID(importFile.CustomerID);
//                ECN_Framework_Entities.Accounts.User superUser = new ECN_Framework_Entities.Accounts.User();
//                foreach (ECN_Framework_Entities.Accounts.User user in userList)
//                {
//                    if (user.IsChannelAdmin)
//                    {
//                        superUser = user;
//                        break;
//                    }
//                    if (user.IsAdmin)
//                    {
//                        superUser = user;
//                        break;
//                    }
//                    if (ECN_Framework_BusinessLayer.Communicator.EmailGroup.HasPermission(ECN_Framework_Common.Objects.Communicator.Enums.EntityRights.Edit, user))
//                    {
//                        superUser = user;
//                        break;
//                    }
//                }
//                //DataTable dt = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmailsWithDupes(superUser,importFile.GroupID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>", "html", "S", false, "user1", false);

//                DataTable dt = ECN_Framework_DataLayer.Communicator.EmailGroup.ImportEmails(superUser.UserID, superUser.CustomerID.Value, importFile.GroupID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>", "html", "S", false);
//                try
//                {
//                    StringBuilder ecnResults = new StringBuilder();
//                    ecnResults.AppendLine("***ECN-Results***");
//                    foreach (DataRow dr in dt.Rows)
//                    {
//                        StringBuilder rowResults = new StringBuilder();
//                        foreach (var dc in dr.ItemArray)
//                        {
//                            rowResults.Append(dc.ToString());
//                        }
//                        ecnResults.AppendLine(rowResults.ToString());
//                    }
//                    ecnResults.AppendLine("*****************");
//                    WriteToImportFileLog(ecnResults.ToString());
//                }
//                catch (Exception ex)
//                {
//                    StringBuilder sbLog = new StringBuilder();
//                    sbLog.AppendLine("*****Error Logging Results******");
//                    sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
//                    sbLog.AppendLine("-- Message --");
//                    if (ex.Message != null)
//                        sbLog.AppendLine(ex.Message);
//                    sbLog.AppendLine("-- InnerException --");
//                    if (ex.InnerException != null)
//                        sbLog.AppendLine(ex.InnerException.ToString());
//                    sbLog.AppendLine("-- Stack Trace --");
//                    if (ex.StackTrace != null)
//                        sbLog.AppendLine(ex.StackTrace);
//                    sbLog.AppendLine("**********************");
//                    WriteToImportFileLog(sbLog.ToString());
//                }
//            }
//            catch (Exception ex)
//            {
//                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "SCG_CDS-Import.Program.UpdateToDB", KMCommon_Application, "SCG_CDS-Import: ImportEmailsWithDupes", -1, importFile.CustomerID);

//                StringBuilder sbLog = new StringBuilder();
//                sbLog.AppendLine("**********************");
//                sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
//                sbLog.AppendLine("-- Message --");
//                if (ex.Message != null)
//                    sbLog.AppendLine(ex.Message);
//                sbLog.AppendLine("-- InnerException --");
//                if (ex.InnerException != null)
//                    sbLog.AppendLine(ex.InnerException.ToString());
//                sbLog.AppendLine("-- Stack Trace --");
//                if (ex.StackTrace != null)
//                    sbLog.AppendLine(ex.StackTrace);
//                sbLog.AppendLine("**********************");
//                WriteToImportFileLog(sbLog.ToString());
//            }
//            WriteToImportFileLog(DateTime.Now + "-END--UpdateToDB()------------------------------------");
//        }
//        #endregion

//        #region Report
//        static void CreateReport()
//        {
//            //TFS item 2057 SGC reports
//            //csv  output is perfect, please include header rows (no quotes with the data)
//            //please auto email the report each week to jhughes@sgcmail.com, corey.mcmahon@TeamKM.com, and danielle.hoffman@TeamKM.com
//            var GroupedReport = TransactionReport.GroupBy(x => x.Pub).ToList();
//            List<TransactionType> GroupCount = new List<TransactionType>();
//            foreach (var x in GroupedReport)
//            {
//                var pubGroup = TransactionReport.Where(y => y.Pub == x.Key).ToList();
//                int addCount = 0;
//                int changeCount = 0;
//                int deleteCount = 0;
//                foreach (var p in pubGroup)
//                {
//                    addCount += p.AddCount;
//                    changeCount += p.ChangeCount;
//                    deleteCount += p.DeleteCount;
//                }
//                TransactionType t = new TransactionType();
//                t.Pub = x.Key;
//                t.AddCount = addCount;
//                t.ChangeCount = changeCount;
//                t.DeleteCount = deleteCount;

//                GroupCount.Add(t);
//            }

//            StringBuilder sbFile = new StringBuilder();
//            sbFile.AppendLine("Pub,AddCount,ChangeCount,DeleteCount");

//            foreach (var gc in GroupCount)
//            {
//                StringBuilder sbDetail = new StringBuilder();
//                sbDetail.Append(gc.Pub + ",");
//                sbDetail.Append(gc.AddCount.ToString() + ",");
//                sbDetail.Append(gc.ChangeCount.ToString() + ",");
//                sbDetail.Append(gc.DeleteCount.ToString());

//                sbFile.AppendLine(sbDetail.ToString());
//            }

//            WriteReportFile(sbFile.ToString());
//            EmailReport(sbFile.ToString());
//        }
//        static void WriteReportFile(string text)
//        {
//            StreamWriter reportFile;
//            reportFile = new StreamWriter(new FileStream(System.Configuration.ConfigurationManager.AppSettings["ReportFilePath"] + "TransactionCounts_" + DateTime.Now.ToString("MM-dd-yyyy") + ".csv", System.IO.FileMode.Append));

//            reportFile.AutoFlush = true;
//            reportFile.WriteLine(text);
//            reportFile.Flush();
//            reportFile.Close();
//        }
//        static void EmailReport(string report)
//        {
//            string reportName = "TransactionCounts_" + DateTime.Now.ToString("MM-dd-yyyy") + ".csv";

//            SmtpClient smtpServer = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["KMCommon_SmtpServer"]);
//            MailMessage message = new MailMessage();
//            message.Priority = MailPriority.High;
//            message.IsBodyHtml = false;
//            message.To.Add(System.Configuration.ConfigurationManager.AppSettings["ReportEmailList"].ToString());
//            message.Bcc.Add(System.Configuration.ConfigurationManager.AppSettings["EmailBCC"].ToString());
//            MailAddress msgSender = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailFrom"].ToString());
//            message.From = msgSender;
//            message.Subject = "Transaction Count Report";
//            message.Body = "Transaction Count Report is attached";

//            Attachment att = Attachment.CreateAttachmentFromString(report, reportName);
//            message.Attachments.Add(att);
//            smtpServer.Send(message);
//        }
//        #endregion


//        static string CleanXMLString(string text)
//        {
//            text = text.Replace("&", "&amp;");
//            text = text.Replace("\"", "&quot;");
//            text = text.Replace("<", "&lt;");
//            text = text.Replace(">", "&gt;");
//            text = text.Replace("'", "");
//            return text.Trim();
//        }
//        static string CleanPhoneNumber(string dirty)
//        {
//            string clean = dirty.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "").Replace(" ", "").ToString();
//            return clean.Trim();
//        }
//        static string CleanCity(string dirty)
//        {
//            string clean = dirty;
//            if (dirty.Contains(","))
//            {
//                int totalLength = dirty.Length;
//                int commaIndex = dirty.IndexOf(",");
//                int remove = totalLength - commaIndex;
//                clean = dirty.Remove(commaIndex, remove);
//            }
//            return clean.Trim();
//        }
//        public static void WriteToLog(string text)
//        {
//            Console.WriteLine(text.ToString());

//            logFile.AutoFlush = true;
//            logFile.WriteLine(DateTime.Now.ToString() + " " + text);
//            logFile.Flush();
//        }
//        public static void WriteToImportFileLog(string text)
//        {
//            Console.WriteLine(text.ToString());

//            importFileLog.AutoFlush = true;
//            importFileLog.WriteLine(DateTime.Now.ToString() + " " + text);
//            importFileLog.Flush();
//        }

//        public enum ECN_ProfileFields
//        {
//            FirstName,
//            LastName,
//            Title,
//            Company,
//            Address,
//            Address2,
//            City,
//            State,
//            Zip,
//            Country,
//            Voice,
//            Fax,
//            Mobile,
//            EmailAddress
//        }
//    }

//    public class EmailClean
//    {
//        public EmailClean() { }

//        public string EmailAddress { get; set; }
//        public string CRD { get; set; }
//        public string SubscriberID { get; set; }
//        public string PubCode { get; set; }
//        public int RowIndex { get; set; }
//    }

//    [Serializable]
//    [DataContract]
//    [XmlTypeAttribute(AnonymousType = true)]
//    [XmlRoot("ImportFiles")]
//    public class ImportFiles
//    {
//        [DataMember]
//        [XmlElement("ImportFile")]
//        public List<ImportFile> Files { get; set; }
//    }

//    [Serializable]
//    [DataContract]
//    public class ImportFile
//    {
//        public ImportFile() { }
//        #region Properties
//        [DataMember]
//        [XmlElement("GroupID")]
//        public int GroupID { get; set; }

//        [DataMember]
//        [XmlElement("GroupName")]
//        public string GroupName { get; set; }

//        [DataMember]
//        [XmlElement("CustomerID")]
//        public int CustomerID { get; set; }

//        [DataMember]
//        [XmlElement("AuthKey")]
//        public string AuthKey { get; set; }

//        [DataMember]
//        [XmlElement("FileName")]
//        public string FileName { get; set; }

//        [DataMember]
//        [XmlElement("FileExtension")]
//        public string FileExtension { get; set; }

//        [DataMember]
//        [XmlElement("ColumnDelimiter")]
//        public string ColumnDelimiter { get; set; }

//        [DataMember]
//        [XmlElement("IsQuoteEncapsulated")]
//        public bool IsQuoteEncapsulated { get; set; }

//        [DataMember]
//        [XmlArray("HeaderMap"), XmlArrayItem("Header")]
//        public List<Header> HeaderMap { get; set; }
//        #endregion
//    }

//    [Serializable]
//    [DataContract]
//    [XmlRoot("Header")]
//    public class Header
//    {
//        public Header() { }
//        #region Properties
//        [DataMember]
//        [XmlElement("FileField")]
//        public string FileField { get; set; }

//        [DataMember]
//        [XmlElement("ECNField")]
//        public string ECNField { get; set; }

//        [DataMember]
//        [XmlElement("IsUDF")]
//        public bool IsUDF { get; set; }

//        [DataMember]
//        public int RowIndex { get; set; }

//        [DataMember]
//        public string Value { get; set; }
//        #endregion
//    }

//    public class TransactionType
//    {
//        public TransactionType() { }

//        public string Pub { get; set; }
//        public int AddCount { get; set; }
//        public int ChangeCount { get; set; }
//        public int DeleteCount { get; set; }
//    }
//}
