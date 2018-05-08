using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using Excel;
using KM.Common;
using KM.Common.Utilities.Logging;
using KMPS.ActivityImport.Entity;
using Microsoft.VisualBasic.FileIO;
using static KMPS.MD.Objects.Enums;

namespace KMPS.ActivityImport
{
    public class Program
    {
        // 2M bytes.
        public const int MaxCacheSize = 2097152;
        // 2K bytes.
        public const int BufferSize = 2048;
        private const string ParseStringListFormatString = "Parse String List : {0} of {1}";
        private const string BooleanYes = "y";
        private const string BooleanNo = "n";
        private const string EmailStatusPropertyName = "EmailStatus";
        private const string StartProcessFlatFileFormatString = "{0}-START--ProcessFlatFile()--  File: {1} --";
        private static readonly DateTime DefaultDateValue = new DateTime(1900, 1, 1);

        StreamWriter customerLog;
        StreamWriter mainLog;
        int KMCommon_Application = int.Parse(ConfigurationManager.AppSettings["KMCommon_Application"].ToString());

        string AppName = "KMPS.ActivityImport";
        bool isDownloadComplete;
        CustomerConfig CustomerList;
        Customer CurrentCustomer;
        List<OpenFile> OpenFileImports;
        List<ClickFile> ClickFileImports;
        List<TopicFile> TopicFileImports;
        List<VisitFile> VisitFileImports;
        List<StatusUpdateFile> StatusUpdateFileImports;

        bool WriteXML;
        int BatchSize;


        static void Main()
        {
            Program p = new Program();

            p.Start();
        }
        void Start()
        {
            mainLog = new StreamWriter(ConfigurationManager.AppSettings["MainLog"].ToString() + AppName + "_" + DateTime.Now.ToString("MM-dd-yyyy") + ".log", true);
            MainLogWrite("Start Instance");

            BatchSize = 500;
            int.TryParse(ConfigurationManager.AppSettings["BatchSize"].ToString(), out BatchSize);
            WriteXML = false;
            bool.TryParse(ConfigurationManager.AppSettings["WriteXML"].ToString(), out WriteXML);

            try//use mainLog at this level
            {
                MainLogWrite("Start CreateCustomerList");
                CreateCustomerList();
                MainLogWrite("Done CreateCustomerList");
                foreach (Customer c in CustomerList.Customers)
                {
                    customerLog = new StreamWriter(c.LogPath.ToString() + c.CustomerName + "_" + DateTime.Now.ToString("MM-dd-yyyy") + ".log", true);
                    CurrentCustomer = c;
                    string step = string.Empty;
                    CustomerLogWrite("Start Customer Process");
                    try
                    {
                        bool doFileDownload = true;
                        bool.TryParse(ConfigurationManager.AppSettings["DownloadFiles"].ToString(), out doFileDownload);

                        foreach (Process p in c.Processes)
                        {
                            isDownloadComplete = false;

                            step = p.ProcessType;

                            if (doFileDownload == true && p.IsEnabled == true)
                            {
                                MainLogWrite("Start File Download for Customer: " + c.CustomerName + " - " + step);
                                CustomerLogWrite("Start File Download - " + step);
                                DownloadFiles(p);
                                MainLogWrite("Done File Download for Customer: " + c.CustomerName + " - " + step);
                                CustomerLogWrite("Done File Download - " + step);
                            }
                            else
                                isDownloadComplete = true;

                            if (isDownloadComplete == true)
                            {
                                if (p.IsEnabled == true)
                                {
                                    MainLogWrite("Start " + step + " for Customer: " + c.CustomerName);
                                    CustomerLogWrite("Start " + step);
                                    Execute(p);
                                    CustomerLogWrite("End " + step);
                                    MainLogWrite("End " + step + " for Customer: " + c.CustomerName);
                                }
                            }
                        }
                        MainLogWrite("End File Imports for Customer: " + c.CustomerName);
                        CustomerLogWrite("End File Imports");
                        step = string.Empty;

                    }
                    catch (Exception ex)
                    {
                        LogMainExeception(ex, "Main.Step: " + step);
                    }
                    CustomerLogWrite("End Customer Process");
                }
            }
            catch (Exception ex)
            {
                LogMainExeception(ex, "Main");
            }
            MainLogWrite("Done Instance");
        }
        void EmailBadFile(FileInfo file)
        {
            string date = DateTime.Now.ToString("MM-dd-yyyy");
            string badPath = CurrentCustomer.BadFilePath + date + "_" + file.Name;

            if (File.Exists(badPath))
                File.Delete(badPath);
            File.Move(file.FullName, badPath);

            SmtpClient smtpServer = new SmtpClient(ConfigurationManager.AppSettings["KMCommon_SmtpServer"]);
            MailMessage message = new MailMessage();
            message.Priority = MailPriority.High;
            message.IsBodyHtml = false;
            message.To.Add(ConfigurationManager.AppSettings["EmailManager"].ToString());
            message.Bcc.Add(ConfigurationManager.AppSettings["EmailBCC"].ToString());
            MailAddress msgSender = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"].ToString());
            message.From = msgSender;
            message.Subject = "KMPS.ActivityImport - Bad Customer File - " + CurrentCustomer.CustomerName;
            message.Body = "Bad Customer File - " + CurrentCustomer.CustomerName;
            Attachment att = new Attachment(badPath);
            message.Attachments.Add(att);

            smtpServer.Send(message);
        }
        void CreateCustomerList()
        {
            string path = ConfigurationManager.AppSettings["ConfigFile"].ToString() + "\\CustomerConfig.xml";
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CustomerConfig));//, new XmlRootAttribute("Mappings"));
            StreamReader reader = new StreamReader(path);
            CustomerList = (CustomerConfig)serializer.Deserialize(reader);
            foreach (Customer c in CustomerList.Customers)
            {
                if (!c.FileArchive.EndsWith(@"\"))
                    c.FileArchive += @"\";
                if (!c.FilePath.EndsWith(@"\"))
                    c.FilePath += @"\";
                if (!c.LogPath.EndsWith(@"\"))
                    c.LogPath += @"\";

                foreach (Process p in c.Processes)
                {
                    if (!p.FileFolder.EndsWith(@"\"))
                        p.FileFolder += @"\";
                }
            }
        }
        #region Step 1: Download Import File from FTP
        void DownloadFiles(Process p)
        {
            try
            {
                string ftpFolder = p.FtpFolder;

                if (!string.IsNullOrEmpty(ftpFolder) && !ftpFolder.StartsWith("/"))
                    ftpFolder = "/" + ftpFolder;

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(CurrentCustomer.FTP_Site + ftpFolder);
                request.Method = WebRequestMethods.Ftp.ListDirectory;//ListDirectoryDetails  ListDirectory

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(CurrentCustomer.FTP_User, CurrentCustomer.FTP_Password);

                FtpWebResponse dirResponse = (FtpWebResponse)request.GetResponse();
                Stream dirResponseStream = dirResponse.GetResponseStream();
                StreamReader dirReader = new StreamReader(dirResponseStream);
                List<string> dirs = new List<string>();
                while (dirReader.EndOfStream == false)
                {
                    string info = dirReader.ReadLine();
                    if (info.Contains(".") && info.ToLower().StartsWith(p.FilePrefix.ToLower()))
                        dirs.Add(info);
                }
                string dirMsg = "File check complete for " + CurrentCustomer.FTP_Site + ftpFolder + ", status " + dirResponse.StatusDescription;
                dirReader.Close();
                dirResponse.Close();
                request.Abort();

                string ftpURL = CurrentCustomer.FTP_Site + ftpFolder + "/";
                string destPath = CurrentCustomer.FilePath + p.FileFolder + @"\";
                if (Directory.Exists(destPath) == false)
                    Directory.CreateDirectory(destPath);

                if (dirs.Count == 0)
                    throw new Exception("Customer: " + CurrentCustomer.CustomerName + " - " + p.ProcessType + ": No File in FTP Folder;");

                foreach (string file in dirs)
                {
                    DownLoad(ftpURL, file, destPath);

                    //delete file from FTP Site
                    string ftpFullPath = ftpURL + file;
                    FtpWebRequest rDelete = (FtpWebRequest)WebRequest.Create(ftpFullPath);
                    rDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                    rDelete.Credentials = new NetworkCredential(CurrentCustomer.FTP_User, CurrentCustomer.FTP_Password);
                    FtpWebResponse responseDelete = (FtpWebResponse)rDelete.GetResponse();

                    Stream streamDelete = responseDelete.GetResponseStream();
                    if (responseDelete.StatusCode != FtpStatusCode.FileActionOK)
                        KM.Common.EmailFunctions.NotifyAdmin("KMPS.ActivityImport File Download Error", "File not deleted from " + CurrentCustomer.CustomerName + " FTP: Status Code - " + responseDelete.StatusCode.ToString() + " :: Status Description - " + responseDelete.StatusDescription.ToString());

                    responseDelete.Close();
                }

                isDownloadComplete = true;
            }
            #region Exception
            catch (Exception ex)
            {
                LogCustomerExeception(ex, "FileDownload");
            }
            #endregion
        }
        void WriteCacheToFile(MemoryStream downloadCache, string downloadPath, int cachedSize)
        {
            using (FileStream fileStream = new FileStream(downloadPath, FileMode.Append))
            {
                byte[] cacheContent = new byte[cachedSize];
                downloadCache.Seek(0, SeekOrigin.Begin);
                downloadCache.Read(cacheContent, 0, cachedSize);
                fileStream.Write(cacheContent, 0, cachedSize);
            }
        }
        void DownLoad(string url, string file, string downloadPath)
        {
            // Create a request to the file to be  downloaded.
            FtpWebRequest request = WebRequest.Create(url + file) as FtpWebRequest;
            request.Credentials = new NetworkCredential(CurrentCustomer.FTP_User, CurrentCustomer.FTP_Password);

            // Download file.
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            FtpWebResponse response = null;
            Stream responseStream = null;
            MemoryStream downloadCache = null;

            try
            {
                // Retrieve the response from the server and get the response stream.
                response = request.GetResponse() as FtpWebResponse;
                responseStream = response.GetResponseStream();

                // Cache data in memory.
                downloadCache = new MemoryStream(MaxCacheSize);
                byte[] downloadBuffer = new byte[BufferSize];

                int bytesSize = 0;
                int cachedSize = 0;

                // Download the file until the download is completed.
                while (true)
                {
                    // Read a buffer of data from the stream.
                    bytesSize = responseStream.Read(downloadBuffer, 0, downloadBuffer.Length);

                    // If the cache is full, or the download is completed, write the data in cache to local file.
                    if (bytesSize == 0
                        || MaxCacheSize < cachedSize + bytesSize)
                    {
                        try
                        {
                            string destPath = downloadPath + file;
                            // Write the data in cache to local file.
                            WriteCacheToFile(downloadCache, destPath, cachedSize);

                            // Stop downloading the file if the download is paused, canceled or completed. 
                            if (bytesSize == 0)
                                break;

                            // Reset cache.
                            downloadCache.Seek(0, SeekOrigin.Begin);
                            cachedSize = 0;
                        }
                        catch (Exception ex)
                        {
                            string msg = string.Format(
                                "There is an error while downloading {0}. "
                                + " See InnerException for detailed error. ",
                                url + file);
                            ApplicationException errorException = new ApplicationException(msg, ex);

                            return;
                        }

                    }

                    // Write the data from the buffer to the cache in memory.
                    downloadCache.Write(downloadBuffer, 0, bytesSize);
                    cachedSize += bytesSize;
                }
            }
            finally
            {
                if (response != null)
                    response.Close();

                if (responseStream != null)
                    responseStream.Close();

                if (downloadCache != null)
                    downloadCache.Close();
            }
        }
        #endregion

        #region Process Files
        void Execute(Process myProcess)
        {
            try
            {
                if (CurrentCustomer != null && CurrentCustomer.FilePath != null && myProcess != null && myProcess.FileFolder != null)
                {
                    DirectoryInfo di = new DirectoryInfo(CurrentCustomer.FilePath + myProcess.FileFolder);
                    foreach (FileInfo fi in di.GetFiles())
                    {
                        TopicFileImports = new List<TopicFile>();
                        ClickFileImports = new List<ClickFile>();
                        OpenFileImports = new List<OpenFile>();
                        VisitFileImports = new List<VisitFile>();
                        StatusUpdateFileImports = new List<StatusUpdateFile>();

                        try
                        {
                            if (fi.Extension.ToLower().Equals("." + FileExtension.xls))
                            {
                                ProcessXLS(fi, myProcess);
                            }
                            else if (fi.Extension.ToLower().Equals("." + FileExtension.xlxs))
                            {
                                ProcessXlxs(fi, myProcess);
                            }
                            else if (fi.Extension.ToLower().Equals("." + FileExtension.xml))
                            {
                                ProcessXML(fi, myProcess);
                            }
                            else
                            {
                                ProcessFlatFile(fi, myProcess);
                            }

                            ImportData(myProcess);
                        }
                        catch (Exception ex)
                        {
                            LogCustomerExeception(ex, "Execute: " + fi.FullName);
                        }

                        TopicFileImports = null;
                        ClickFileImports = null;
                        OpenFileImports = null;
                        VisitFileImports = null;
                        StatusUpdateFileImports = null;
                    }
                }
            }
            catch (Exception ex)
            {
                LogCustomerExeception(ex, "Execute");
            }
        }

        DataSet ProcessXLS(FileStream stream)
        {
            IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();
            return result;
        }
        DataSet ProcessXLSX(FileStream stream)
        {
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();
            return result;
        }
        void ProcessXLS(FileInfo file, Process myProcess)
        {
            #region parse file
            FileStream stream = File.Open(file.FullName, FileMode.Open, FileAccess.Read);
            DataTable xlsDataTable = new DataTable();
            DataSet ds = new DataSet();
            List<KeyValuePair<int, string>> ColumnIndexList = new List<KeyValuePair<int, string>>();

            xlsDataTable = ProcessXLS(stream).Tables[0];
            int index = 0;
            foreach (DataColumn dc in xlsDataTable.Columns)
            {
                dc.ColumnName = dc.ColumnName.Trim();
                ColumnIndexList.Add(new KeyValuePair<int, string>(index, dc.ColumnName));
                index++;
            }
            #endregion
            #region move file - archive the file
            if (!Directory.Exists(CurrentCustomer.FileArchive + myProcess.FileFolder))
                Directory.CreateDirectory(CurrentCustomer.FileArchive + myProcess.FileFolder);

            string fileName = DateTime.Now.ToString("MM-dd-yyyy") + "_" + file.Name;
            string movePath = CurrentCustomer.FileArchive + myProcess.FileFolder + fileName;
            File.Move(file.FullName, movePath);
            #endregion
            #region create import object list
            ClickFileImports = new List<ClickFile>();
            OpenFileImports = new List<OpenFile>();
            TopicFileImports = new List<TopicFile>();
            VisitFileImports = new List<VisitFile>();
            StatusUpdateFileImports = new List<StatusUpdateFile>();

            int importRowCount = xlsDataTable.Rows.Count;
            int rowIndex = 0;
            List<System.Reflection.PropertyInfo> propList = new List<System.Reflection.PropertyInfo>();
            if (myProcess.ProcessType.ToLower().Equals(ProcessType.ClickImport.ToString().ToLower()))
                propList = typeof(ClickFile).GetProperties().ToList();
            else if (myProcess.ProcessType.ToLower().Equals(ProcessType.OpenImport.ToString().ToLower()))
                propList = typeof(OpenFile).GetProperties().ToList();
            else if (myProcess.ProcessType.ToLower().Equals(ProcessType.TopicImport.ToString().ToLower()))
                propList = typeof(TopicFile).GetProperties().ToList();
            else if (myProcess.ProcessType.ToLower().Equals(ProcessType.VisitImport.ToString().ToLower()))
                propList = typeof(VisitFile).GetProperties().ToList();
            else if (myProcess.ProcessType.ToLower().Equals(ProcessType.StatusUpdateImport.ToString().ToLower()))
                propList = typeof(StatusUpdateFile).GetProperties().ToList();

            foreach (DataRow xlsDR in xlsDataTable.Rows)
            {
                CustomerLogWrite("Parse String List : " + rowIndex.ToString() + " of " + importRowCount.ToString());
                Subscription newSub = new Subscription();
                OpenFile newOF = new OpenFile();
                ClickFile newCF = new ClickFile();
                TopicFile newTF = new TopicFile();
                VisitFile newVF = new VisitFile();
                StatusUpdateFile newSUF = new StatusUpdateFile();

                foreach (KeyValuePair<int, string> kvp in ColumnIndexList)
                {
                    string columnValue = xlsDR[kvp.Key].ToString();
                    if (propList.Exists(x => x.Name.Equals(kvp.Value, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        string propName = propList.Single(x => x.Name.Equals(kvp.Value, StringComparison.CurrentCultureIgnoreCase)).Name;
                        #region OpenImport
                        if (myProcess.ProcessType.Equals(ProcessType.OpenImport.ToString()))
                        {
                            string propType = newOF.GetType().GetProperty(propName).PropertyType.ToString();
                            if (propType.Equals("System.Int32", StringComparison.CurrentCultureIgnoreCase))
                            {
                                int intColValue;
                                int.TryParse(columnValue, out intColValue);
                                newOF.GetType().GetProperty(propName).SetValue(newOF, intColValue);
                            }
                            else if (propType.Equals("System.DateTime", StringComparison.CurrentCultureIgnoreCase))
                            {
                                DateTime dtColValue = DateTime.Parse("1/1/1900");
                                DateTime.TryParse(columnValue, out dtColValue);
                                if (dtColValue == DateTime.MinValue)
                                    dtColValue = DateTime.Parse("1/1/1900");
                                newOF.GetType().GetProperty(propName).SetValue(newOF, dtColValue);
                            }
                            else if (propType.Equals("System.Boolean", StringComparison.CurrentCultureIgnoreCase))
                            {
                                bool bColValue;
                                bool.TryParse(columnValue, out bColValue);
                                if (columnValue.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                                    bColValue = true;
                                if (columnValue.Equals("n", StringComparison.CurrentCultureIgnoreCase))
                                    bColValue = false;

                                newOF.GetType().GetProperty(propName).SetValue(newOF, bColValue);
                            }
                            else if (propType.Equals("System.Double", StringComparison.CurrentCultureIgnoreCase))
                            {
                                double dblColValue;
                                double.TryParse(columnValue, out dblColValue);
                                newOF.GetType().GetProperty(propName).SetValue(newOF, dblColValue);
                            }
                            else
                            {
                                newOF.GetType().GetProperty(propName).SetValue(newOF, columnValue);
                            }
                        }
                        #endregion
                        #region ClickImport
                        else if (myProcess.ProcessType.Equals(ProcessType.ClickImport.ToString()))
                        {
                            string propType = newCF.GetType().GetProperty(propName).PropertyType.ToString();
                            if (propType.Equals("System.Int32", StringComparison.CurrentCultureIgnoreCase))
                            {
                                int intColValue;
                                int.TryParse(columnValue, out intColValue);
                                newCF.GetType().GetProperty(propName).SetValue(newCF, intColValue);
                            }
                            else if (propType.Equals("System.DateTime", StringComparison.CurrentCultureIgnoreCase))
                            {
                                DateTime dtColValue = DateTime.Parse("1/1/1900");
                                DateTime.TryParse(columnValue, out dtColValue);
                                if (dtColValue == DateTime.MinValue)
                                    dtColValue = DateTime.Parse("1/1/1900");
                                newCF.GetType().GetProperty(propName).SetValue(newCF, dtColValue);
                            }
                            else if (propType.Equals("System.Boolean", StringComparison.CurrentCultureIgnoreCase))
                            {
                                bool bColValue;
                                bool.TryParse(columnValue, out bColValue);
                                if (columnValue.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                                    bColValue = true;
                                if (columnValue.Equals("n", StringComparison.CurrentCultureIgnoreCase))
                                    bColValue = false;

                                newCF.GetType().GetProperty(propName).SetValue(newCF, bColValue);
                            }
                            else if (propType.Equals("System.Double", StringComparison.CurrentCultureIgnoreCase))
                            {
                                double dblColValue;
                                double.TryParse(columnValue, out dblColValue);
                                newCF.GetType().GetProperty(propName).SetValue(newCF, dblColValue);
                            }
                            else
                            {
                                newCF.GetType().GetProperty(propName).SetValue(newCF, columnValue);
                            }
                        }
                        #endregion
                        #region TopicImport
                        else if (myProcess.ProcessType.Equals(ProcessType.TopicImport.ToString()))
                        {
                            string propType = newTF.GetType().GetProperty(propName).PropertyType.ToString();
                            if (propType.Equals("System.Int32", StringComparison.CurrentCultureIgnoreCase))
                            {
                                int intColValue;
                                int.TryParse(columnValue, out intColValue);
                                newTF.GetType().GetProperty(propName).SetValue(newTF, intColValue);
                            }
                            else if (propType.Equals("System.DateTime", StringComparison.CurrentCultureIgnoreCase))
                            {
                                DateTime dtColValue = DateTime.Parse("1/1/1900");
                                DateTime.TryParse(columnValue, out dtColValue);
                                if (dtColValue == DateTime.MinValue)
                                    dtColValue = DateTime.Parse("1/1/1900");
                                newTF.GetType().GetProperty(propName).SetValue(newTF, dtColValue);
                            }
                            else if (propType.Equals("System.Boolean", StringComparison.CurrentCultureIgnoreCase))
                            {
                                bool bColValue;
                                bool.TryParse(columnValue, out bColValue);
                                if (columnValue.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                                    bColValue = true;
                                if (columnValue.Equals("n", StringComparison.CurrentCultureIgnoreCase))
                                    bColValue = false;

                                newTF.GetType().GetProperty(propName).SetValue(newTF, bColValue);
                            }
                            else if (propType.Equals("System.Double", StringComparison.CurrentCultureIgnoreCase))
                            {
                                double dblColValue;
                                double.TryParse(columnValue, out dblColValue);
                                newTF.GetType().GetProperty(propName).SetValue(newTF, dblColValue);
                            }
                            else
                            {
                                newTF.GetType().GetProperty(propName).SetValue(newTF, columnValue);
                            }
                        }
                        #endregion
                        #region VisitImport
                        else if (myProcess.ProcessType.Equals(ProcessType.VisitImport.ToString()))
                        {
                            string propType = newVF.GetType().GetProperty(propName).PropertyType.ToString();
                            if (propType.Equals("System.Int32", StringComparison.CurrentCultureIgnoreCase))
                            {
                                int intColValue;
                                int.TryParse(columnValue, out intColValue);
                                newVF.GetType().GetProperty(propName).SetValue(newVF, intColValue);
                            }
                            else if (propType.Equals("System.DateTime", StringComparison.CurrentCultureIgnoreCase))
                            {
                                DateTime dtColValue = DateTime.Parse("1/1/1900");
                                DateTime.TryParse(columnValue, out dtColValue);
                                if (dtColValue == DateTime.MinValue)
                                    dtColValue = DateTime.Parse("1/1/1900");
                                newVF.GetType().GetProperty(propName).SetValue(newVF, dtColValue);
                            }
                            else if (propType.Equals("System.Boolean", StringComparison.CurrentCultureIgnoreCase))
                            {
                                bool bColValue;
                                bool.TryParse(columnValue, out bColValue);
                                if (columnValue.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                                    bColValue = true;
                                if (columnValue.Equals("n", StringComparison.CurrentCultureIgnoreCase))
                                    bColValue = false;

                                newVF.GetType().GetProperty(propName).SetValue(newVF, bColValue);
                            }
                            else if (propType.Equals("System.Double", StringComparison.CurrentCultureIgnoreCase))
                            {
                                double dblColValue;
                                double.TryParse(columnValue, out dblColValue);
                                newVF.GetType().GetProperty(propName).SetValue(newVF, dblColValue);
                            }
                            else
                            {
                                newVF.GetType().GetProperty(propName).SetValue(newVF, columnValue);
                            }
                        }
                        #endregion
                        #region StatusUpdate Import
                        else if (myProcess.ProcessType.Equals(ProcessType.StatusUpdateImport.ToString()))
                        {
                            string propType = newSUF.GetType().GetProperty(propName).PropertyType.ToString();
                            if (propName.Equals("EmailStatus", StringComparison.CurrentCultureIgnoreCase))
                            {
                                newSUF.GetType().GetProperty(propName).SetValue(newSUF, (EmailStatus)Enum.Parse(typeof(EmailStatus), columnValue.ToString()));
                            }
                            else
                            {
                                newSUF.GetType().GetProperty(propName).SetValue(newSUF, columnValue);
                            }
                        }
                        #endregion
                    }
                }
                // BUG :: should add myProcess.ProcessType.ToLower() to make the if condition pass & valid
                if (myProcess.ProcessType.Equals(ProcessType.ClickImport.ToString().ToLower()))
                    ClickFileImports.Add(newCF);
                else if (myProcess.ProcessType.Equals(ProcessType.OpenImport.ToString().ToLower()))
                    OpenFileImports.Add(newOF);
                else if (myProcess.ProcessType.Equals(ProcessType.TopicImport.ToString().ToLower()))
                    TopicFileImports.Add(newTF);
                else if (myProcess.ProcessType.Equals(ProcessType.VisitImport.ToString().ToLower()))
                    VisitFileImports.Add(newVF);
                else if (myProcess.ProcessType.Equals(ProcessType.StatusUpdateImport.ToString().ToLower()))
                    StatusUpdateFileImports.Add(newSUF);

                rowIndex++;
            }
            #endregion
        }

        private void ProcessXlxs(FileSystemInfo fileSystemInfo, Process entityProcess)
        {
            DataTable xlsDataTable = null;
            try
            {
                var columnIndexList = ParseFile(fileSystemInfo, out xlsDataTable)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                ArchiveFile(fileSystemInfo, entityProcess);

                var importRowCount = xlsDataTable.Rows.Count;
           
                var properties = GetProperties(entityProcess);

                ClickFileImports = new List<ClickFile>();
                OpenFileImports = new List<OpenFile>();
                TopicFileImports = new List<TopicFile>();
                VisitFileImports = new List<VisitFile>();
                StatusUpdateFileImports = new List<StatusUpdateFile>();

                var rowIndex = 0;
                foreach (DataRow dataRow in xlsDataTable.Rows)
                {
                    CustomerLogWrite(string.Format(ParseStringListFormatString, rowIndex, importRowCount));
                    ProcessDataRow(entityProcess, columnIndexList, properties, dataRow, false);

                    rowIndex++;
                }
            }
            finally
            {
                xlsDataTable?.Dispose();
            }
        }

        private static void SetFileProperties(
            Process entityProcess,
            IEnumerable<PropertyInfo> properties,
            KeyValuePair<int, string> valuePair,
            string columnValue,
            OpenFile openFile,
            ClickFile clickFile,
            TopicFile topicFile,
            VisitFile visitFile,
            StatusUpdateFile statusUpdateFile)
        {
            var propertyName = properties.Single(propertyInfo => propertyInfo.Name.Equals(valuePair.Value, StringComparison.OrdinalIgnoreCase))
                .Name;
            var processType = entityProcess.ProcessType;

            if (processType.Equals(ProcessType.OpenImport.ToString()))
            {
                SetFileProperty(
                    openFile,
                    openFile.GetType().GetProperty(propertyName),
                    columnValue);
            }
            else if (processType.Equals(ProcessType.ClickImport.ToString()))
            {
                SetFileProperty(
                    clickFile,
                    clickFile.GetType().GetProperty(propertyName),
                    columnValue);
            }
            else if (processType.Equals(ProcessType.TopicImport.ToString()))
            {
                SetFileProperty(
                    topicFile,
                    topicFile.GetType().GetProperty(propertyName),
                    columnValue);
            }
            else if (processType.Equals(ProcessType.VisitImport.ToString()))
            {
                SetFileProperty(
                    visitFile,
                    visitFile.GetType().GetProperty(propertyName),
                    columnValue);
            }
            else if (processType.Equals(ProcessType.StatusUpdateImport.ToString()))
            {
                StatusUpdateImport(statusUpdateFile, propertyName, columnValue);
            }
        }

        private void AddToImport(
            Process entityProcess,
            ClickFile clickFile,
            OpenFile openfile,
            TopicFile topicFile,
            VisitFile visitFile,
            StatusUpdateFile statusUpdateFile,
            bool useVisitImport)
        {
            var processType = entityProcess.ProcessType;

            if (processType.Equals(ProcessType.ClickImport.ToString()))
            {
                ClickFileImports.Add(clickFile);
            }
            else if (processType.Equals(ProcessType.OpenImport.ToString()))
            {
                OpenFileImports.Add(openfile);
            }
            else if (processType.Equals(ProcessType.TopicImport.ToString()))
            {
                TopicFileImports.Add(topicFile);
            }
            // BUG: should check for ProcessType.VisitImport not ProcessType.OpenImport
            else if (processType.Equals(useVisitImport 
                                            ? ProcessType.VisitImport.ToString() 
                                            : ProcessType.OpenImport.ToString()))
            {
                VisitFileImports.Add(visitFile);
            }
            else if (processType.Equals(ProcessType.StatusUpdateImport.ToString()))
            {
                StatusUpdateFileImports.Add(statusUpdateFile);
            }
        }

        private static void StatusUpdateImport(StatusUpdateFile statusUpdateFile, string propertyName, string columnValue)
        {
            object newValue = columnValue;

            if (propertyName.Equals(EmailStatusPropertyName, StringComparison.OrdinalIgnoreCase))
            {
                newValue = (EmailStatus)Enum.Parse(typeof(EmailStatus), columnValue);
            }
                
            statusUpdateFile.GetType()
                .GetProperty(propertyName)
                ?.SetValue(statusUpdateFile, newValue);
        }

        private static void SetFileProperty(object instanceObject, PropertyInfo propertyInfo, string columnValue)
        {
            var propertyType = propertyInfo.PropertyType;
            object newValue = columnValue;

            if (propertyType == typeof(int))
            {
                int intColValue;
                if (int.TryParse(columnValue, out intColValue))
                {
                    newValue = intColValue;
                }
            }
            else if (propertyType == typeof(DateTime))
            {
                DateTime dateTimeValue;
                DateTime.TryParse(columnValue, out dateTimeValue);
                if (dateTimeValue == DateTime.MinValue)
                {
                    dateTimeValue = DefaultDateValue;
                }

                newValue = dateTimeValue;
            }
            else if (propertyType == typeof(bool))
            {
                if (columnValue.Equals(BooleanYes, StringComparison.OrdinalIgnoreCase))
                {
                    newValue = true;
                }
                else if (columnValue.Equals(BooleanNo, StringComparison.OrdinalIgnoreCase))
                {
                    newValue = false;
                }
            }
            else if (propertyType == typeof(double))
            {
                double doubleValue;
                if (double.TryParse(columnValue, out doubleValue))
                {
                    newValue = doubleValue;
                }
            }

            propertyInfo.SetValue(instanceObject, newValue);
        }

        private void ArchiveFile(FileSystemInfo fileSystemInfo, Process entityProcess)
        {
            var directoryPath = CurrentCustomer.FileArchive + entityProcess.FileFolder;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = $"{DateTime.Now:MM-dd-yyyy}_{fileSystemInfo.Name}";
            var movePath = $"{directoryPath}{fileName}";
            File.Move(fileSystemInfo.FullName, movePath);
        }

        private IEnumerable<KeyValuePair<int, string>> ParseFile(FileSystemInfo fileSystemInfo, out DataTable xlsDataTable)
        {
            var stream = File.Open(fileSystemInfo.FullName, FileMode.Open, FileAccess.Read);
            var columnIndexList = new List<KeyValuePair<int, string>>();

            xlsDataTable = ProcessXLSX(stream).Tables[0];
            var index = 0;
            foreach (DataColumn dataColumn in xlsDataTable.Columns)
            {
                dataColumn.ColumnName = dataColumn.ColumnName.Trim();
                columnIndexList.Add(new KeyValuePair<int, string>(index, dataColumn.ColumnName));
                index++;
            }

            return columnIndexList;
        }

        private static List<PropertyInfo> GetProperties(Process entityProcess)
        {
            var properties = new List<PropertyInfo>();
            var processType = entityProcess.ProcessType;

            if (processType.Equals(ProcessType.ClickImport.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                properties = typeof(ClickFile).GetProperties().ToList();
            }
            else if (processType.Equals(ProcessType.OpenImport.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                properties = typeof(OpenFile).GetProperties().ToList();
            }
            else if (processType.Equals(ProcessType.TopicImport.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                properties = typeof(TopicFile).GetProperties().ToList();
            }
            else if (processType.Equals(ProcessType.VisitImport.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                properties = typeof(VisitFile).GetProperties().ToList();
            }
            else if (processType.Equals(ProcessType.StatusUpdateImport.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                properties = typeof(StatusUpdateFile).GetProperties().ToList();
            }

            return properties;
        }

        void ProcessXML(FileInfo file, Process myProcess)
        {

        }

        private void ProcessFlatFile(FileInfo file, Process entityProcess)
        {
            CustomerLogWrite(string.Format(StartProcessFlatFileFormatString, DateTime.Now, file.Name));

            using (var dataTable = LoadFile(file, entityProcess))
            {
                var totalRecords = dataTable.Rows.Count;
                if (totalRecords == 0)
                {
                    EmailBadFile(file);
                }
                else
                {
                    LogTotalRecordsInFile(totalRecords);

                    // lets create a column name index map
                    var columnIndexList = new Dictionary<int, string>();
                    var index = 0;
                    foreach (DataColumn dataColumn in dataTable.Columns)
                    {
                        columnIndexList.Add(index, dataColumn.ColumnName);
                        index++;
                    }

                    ArchiveFile(file, entityProcess);

                    ClickFileImports = new List<ClickFile>();
                    OpenFileImports = new List<OpenFile>();
                    TopicFileImports = new List<TopicFile>();
                    VisitFileImports = new List<VisitFile>();
                    StatusUpdateFileImports = new List<StatusUpdateFile>();

                    var properties = GetProperties(entityProcess);

                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        ProcessDataRow(entityProcess, columnIndexList, properties, dataRow);
                    }
                }
            }
        }

        private void LogTotalRecordsInFile(int totalRecords)
        {
            CustomerLogWrite(string.Empty);
            CustomerLogWrite("=========================================================");
            CustomerLogWrite($"Total Records in File : {totalRecords}");
            CustomerLogWrite("=========================================================");
            CustomerLogWrite(string.Empty);
        }

        private void ProcessDataRow(
            Process entityProcess,
            Dictionary<int, string> columnIndexList,
            List<PropertyInfo> properties,
            DataRow dataRow,
            bool useVisitImport = true)
        {
            var openFile = new OpenFile();
            var clickFile = new ClickFile();
            var topicFile = new TopicFile();
            var visitFile = new VisitFile();
            var statusUpdateFile = new StatusUpdateFile();

            foreach (var valuePair in columnIndexList)
            {
                if (!properties.Exists(propertyInfo => propertyInfo.Name.Equals(valuePair.Value, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                var columnValue = dataRow[valuePair.Value].ToString();
                SetFileProperties(
                    entityProcess,
                    properties,
                    valuePair,
                    columnValue,
                    openFile,
                    clickFile,
                    topicFile,
                    visitFile,
                    statusUpdateFile);
            }

            AddToImport(entityProcess, clickFile, openFile, topicFile, visitFile, statusUpdateFile, useVisitImport);
        }

        DataTable LoadFile(FileInfo file, Process myProcess)
        {
            var tfp = new TextFieldParser(file.FullName);
            tfp.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
            var delimiter = GetColumnDelimiter(myProcess.ColumnDelimiter.ToLower());
            if (delimiter == ColumnDelimiter.comma)
                tfp.SetDelimiters(",");
            else if (delimiter == ColumnDelimiter.semicolon)
                tfp.SetDelimiters(";");
            else if (delimiter == ColumnDelimiter.tab)
                tfp.SetDelimiters("\t");
            else if (delimiter == ColumnDelimiter.colon)
                tfp.SetDelimiters(":");
            else if (delimiter == ColumnDelimiter.tild)
                tfp.SetDelimiters("~");

            //if (myProcess.IsQuoteEncapsulated == true)
            tfp.HasFieldsEnclosedInQuotes = true;
            //else
            //    tfp.HasFieldsEnclosedInQuotes = false;

            //convert to dataset:
            DataSet ds = new DataSet();
            ds.Tables.Add("Data");

            try
            {
                var stringRow = tfp.ReadFields();
                foreach (var field in stringRow)
                {
                    ds.Tables[0].Columns.Add(field, Type.GetType("System.String"));
                }

                //populate with data:
                while (!tfp.EndOfData)
                {
                    stringRow = tfp.ReadFields();
                    ds.Tables[0].Rows.Add(stringRow);
                }

                tfp.Close();
            }
            catch (Exception ex)
            {
                CustomerLogWrite(ex.Message);
            }
            finally { tfp.Close(); }


            return ds.Tables["Data"];
        }

        #endregion

        private void ImportData(Process myProcess)
        {
            if (myProcess?.ProcessType == null)
            {
                return;
            }

            if (myProcess.ProcessType.Equals(ProcessType.ClickImport.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                ImportClickData();

            }
            else if (myProcess.ProcessType.Equals(ProcessType.OpenImport.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                ImportOpenData();
            }
            else if (myProcess.ProcessType.Equals(ProcessType.TopicImport.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                ImportTopicData();
            }
            else if (myProcess.ProcessType.Equals(ProcessType.VisitImport.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                ImportVisitData();
            }
            else if (myProcess.ProcessType.Equals(ProcessType.StatusUpdateImport.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                ImportStatusUpdateData();
            }
        }

        private void ImportClickData()
        {
            if (ClickFileImports == null)
            {
                return;
            }

            var batchCount = 0;
            var total = ClickFileImports.Count;
            CustomerLogWrite($"Total ClickFileImports: {total}");
            var counter = 0;
            var processedCount = 0;
            var xml = new StringBuilder();
            foreach (var record in ClickFileImports)
            {
                xml.AppendLine("<ClickActivity>");
                xml.AppendLine($"<ClickTime>{record.ClickTime.ToString().Trim()}</ClickTime>");
                xml.AppendLine($"<EmailAddress>{CleanXMLString(record.EmailAddress.Trim())}</EmailAddress>");
                xml.AppendLine($"<PubCode>{CleanXMLString(record.Pubcode.Trim())}</PubCode>");
                xml.AppendLine($"<ClickURL>{CleanXMLString(record.ClickURL.Trim())}</ClickURL>");
                xml.AppendLine($"<Alias>{CleanXMLString(record.Alias.Trim())}</Alias>");
                xml.AppendLine($"<BlastID>{record.BlastID}</BlastID>");
                xml.AppendLine($"<Subject>{CleanXMLString(record.Subject.Trim())}</Subject>");
                xml.AppendLine($"<SendTime>{(record.SendTime == DateTime.Parse("1/1/1900") ? "" : record.SendTime.ToString())}</SendTime>");
                xml.AppendLine("</ClickActivity>");

                counter++;
                processedCount++;

                if (processedCount == total || counter == BatchSize)
                {
                    if (WriteXML)
                    {
                        CustomerLogWrite($"ClickFileImports : {DateTime.Now} Send to DB");
                        CustomerLogWrite("~~XML~~");
                        CustomerLogWrite($"<XML>{xml}</XML>");
                        CustomerLogWrite("~~XML~~");
                    }

                    SubscriberClickActivity.Import($"<XML>{xml}</XML>", CurrentCustomer.SQL);
                    xml = new StringBuilder();
                    counter = 0;
                    batchCount++;
                    CustomerLogWrite($"Batch : {batchCount}");
                }
            }
        }

        private void ImportOpenData()
        {
            if (OpenFileImports == null)
            {
                return;
            }

            var batchCount = 0;
            var total = OpenFileImports.Count;
            CustomerLogWrite($"Total OpenFileImports: {total}");
            var counter = 0;
            var processedCount = 0;
            var xml = new StringBuilder();
            foreach (var record in OpenFileImports)
            {
                xml.AppendLine("<OpenActivity>");
                xml.AppendLine($"<OpenTime>{record.OpenTime.ToString().Trim()}</OpenTime>");
                xml.AppendLine($"<EmailAddress>{CleanXMLString(record.EmailAddress.Trim())}</EmailAddress>");
                xml.AppendLine($"<PubCode>{CleanXMLString(record.Pubcode.Trim())}</PubCode>");
                xml.AppendLine($"<Subject>{CleanXMLString(record.Subject.Trim())}</Subject>");
                xml.AppendLine($"<BlastID>{record.BlastID}</BlastID>");
                xml.AppendLine($"<Subject>{CleanXMLString(record.Subject.Trim())}</Subject>");
                xml.AppendLine($"<SendTime>{(record.SendTime == DefaultDateValue ? string.Empty : record.SendTime.ToString())}</SendTime>");
                xml.AppendLine("</OpenActivity>");

                counter++;
                processedCount++;

                if (processedCount == total || counter == BatchSize)
                {
                    if (WriteXML)
                    {
                        CustomerLogWrite($"OpenFileImports : {DateTime.Now} Send to DB");
                        CustomerLogWrite("~~XML~~");
                        CustomerLogWrite($"<XML>{xml}</XML>");
                        CustomerLogWrite("~~XML~~");
                    }

                    SubscriberOpenActivity.Import($"<XML>{xml}</XML>", CurrentCustomer.SQL);
                    xml = new StringBuilder();
                    counter = 0;
                    batchCount++;
                    CustomerLogWrite($"Batch : {batchCount}");
                }
            }
        }

        private void ImportTopicData()
        {
            if (TopicFileImports == null)
            {
                return;
            }

            var batchCount = 0;
            var total = TopicFileImports.Count;
            CustomerLogWrite($"Total TopicFileImports: {total}");
            var counter = 0;
            var processedCount = 0;
            var xml = new StringBuilder();

            foreach (var record in TopicFileImports)
            {
                xml.AppendLine("<Topic>");
                xml.AppendLine($"<ActivityDatetime>{record.ActivityDateTime.ToString().Trim()}</ActivityDatetime>");
                xml.AppendLine($"<EmailAddress>{CleanXMLString(record.EmailAddress.Trim())}</EmailAddress>");
                xml.AppendLine($"<PubCode>{CleanXMLString(record.Pubcode.Trim())}</PubCode>");
                xml.AppendLine($"<TopicCode>{CleanXMLString(record.TopicCode.Trim())}</TopicCode>");
                xml.AppendLine($"<TopicCodeText>{CleanXMLString(record.TopicCodeText.Trim())}</TopicCodeText>");
                xml.AppendLine("</Topic>");

                counter++;
                processedCount++;

                if (processedCount == total || counter == BatchSize)
                {
                    if (WriteXML)
                    {
                        CustomerLogWrite($"TopicFileImports : {DateTime.Now} Send to DB");
                        CustomerLogWrite("~~XML~~");
                        CustomerLogWrite($"<XML>{xml}</XML>");
                        CustomerLogWrite("~~XML~~");
                    }

                    TopicActivity.Import($"<XML>{xml}</XML>", CurrentCustomer.SQL);
                    xml = new StringBuilder();
                    counter = 0;
                    batchCount++;
                    CustomerLogWrite($"Batch : {batchCount}");
                }
            }
        }

        private void ImportVisitData()
        {
            if (VisitFileImports == null)
            {
                return;
            }

            var batchCount = 0;
            var total = VisitFileImports.Count;
            CustomerLogWrite($"Total VisitFileImports: {total}");
            var counter = 0;
            var processedCount = 0;
            var xml = new StringBuilder();

            foreach (var record in VisitFileImports)
            {
                xml.AppendLine("<Visit>");
                xml.AppendLine($"<EmailAddress>{CleanXMLString(record.EmailAddress.Trim())}</EmailAddress>");
                xml.AppendLine($"<Visittime>{record.VisitTime.ToString().Trim()}</Visittime>");
                xml.AppendLine($"<URL>{CleanXMLString(record.URL.Trim())}</URL>");
                xml.AppendLine("</Visit>");

                counter++;
                processedCount++;

                if (processedCount == total || counter == BatchSize)
                {
                    if (WriteXML)
                    {
                        CustomerLogWrite($"VisitFileImports : {DateTime.Now} Send to DB");
                        CustomerLogWrite("~~XML~~");
                        CustomerLogWrite($"<XML>{xml}</XML>");
                        CustomerLogWrite("~~XML~~");
                    }

                    SubscriberVisitActivity.Import($"<XML>{xml}</XML>", CurrentCustomer.SQL);
                    xml = new StringBuilder();
                    counter = 0;
                    batchCount++;
                    CustomerLogWrite($"Batch : {batchCount}");
                }
            }
        }

        private void ImportStatusUpdateData()
        {
            if (StatusUpdateFileImports == null)
            {
                return;
            }

            var batchCount = 0;
            var total = StatusUpdateFileImports.Count;
            CustomerLogWrite($"Total StatusUpdateFileImports: {total}");
            var counter = 0;
            var processedCount = 0;
            var xml = new StringBuilder();

            foreach (var record in StatusUpdateFileImports)
            {
                xml.AppendLine("<StatusUpdate>");
                xml.AppendLine($"<EmailAddress>{CleanXMLString(record.EmailAddress.Trim())}</EmailAddress>");
                xml.AppendLine($"<EmailStatusID>{(int)record.EmailStatus}</EmailStatusID>");
                xml.AppendLine("</StatusUpdate>");

                counter++;
                processedCount++;

                if (processedCount == total || counter == BatchSize)
                {
                    if (WriteXML)
                    {
                        CustomerLogWrite($"StatusUpdateFileImports : {DateTime.Now} Send to DB");
                        CustomerLogWrite("~~XML~~");
                        CustomerLogWrite($"<XML>{xml}</XML>");
                        CustomerLogWrite("~~XML~~");
                    }

                    SubscriberStatusUpdate.Import($"<XML>{xml}</XML>", CurrentCustomer.SQL);
                    xml = new StringBuilder();
                    counter = 0;
                    batchCount++;
                    CustomerLogWrite($"Batch : {batchCount}");
                }
            }
        }

        #region Helpers
        public void MainLogWrite(string text)
        {
            Console.WriteLine(text);
            StreamLogger.LogAndFlushWriter(mainLog, text);
        }
        public void MainLogWrite(StringBuilder text)
        {
            MainLogWrite(text.ToString());
        }
        public void CustomerLogWrite(string text)
        {
            Console.WriteLine(text);
            StreamLogger.LogAndFlushWriter(customerLog, text);
        }
        public void CustomerLogWrite(StringBuilder text)
        {
            CustomerLogWrite(text.ToString());
        }
        public StringBuilder FormatException(Exception ex)
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

            return sbLog;
        }
        public string FormatZip(string zip)
        {
            if (!string.IsNullOrEmpty(zip))
            {
                if (zip.Length == 4)
                    zip = "0" + zip;
                if (zip.Length > 5)
                    zip = zip.Substring(0, 5);

                int zipCheck = 0;
                if (int.TryParse(zip.ToString(), out zipCheck) == false)
                {
                    int zipFirst = 0;
                    if (int.TryParse(zip.Substring(0, 1).ToString(), out zipFirst) == false)
                    {
                        zip = "0" + zip.Substring(1, 4);
                    }
                    else
                    {
                        zip = "0" + zip.Substring(0, 4);
                    }
                }
            }
            return zip.Trim();
        }
        string CleanXMLString(string dirty)
        {
            dirty = dirty.Replace("\0", string.Empty);
            if (string.IsNullOrEmpty(dirty))
                dirty = string.Empty;
            dirty = dirty.Trim();
            dirty = dirty.Replace("&", "&amp;");
            dirty = dirty.Replace("\"", "&quot;");
            dirty = dirty.Replace("<", "&lt;");
            dirty = dirty.Replace(">", "&gt;");
            dirty = dirty.Replace("'", "").Replace("’", "");
            byte[] bytes = Encoding.Default.GetBytes(dirty);
            return Encoding.UTF8.GetString(bytes); 
        }
        public ColumnDelimiter GetColumnDelimiter(string delimiter)
        {
            try
            {
                return (ColumnDelimiter)System.Enum.Parse(typeof(ColumnDelimiter), delimiter, true);
            }
            catch { return ColumnDelimiter.comma; }
        }
        public void LogCustomerExeception(Exception ex, string method)
        {
            if(ex.Message.Contains("No File in FTP Folder;"))
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, AppName + "." + method, KMCommon_Application, AppName + ": Unhandled Exception", -1, -1);
            else
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, AppName + "." + method, KMCommon_Application, AppName + ": Unhandled Exception", -1, -1);
            CustomerLogWrite(StringFunctions.FormatException(ex));
        }
        public void LogMainExeception(Exception ex, string method)
        {
            KM.Common.Entity.ApplicationLog.LogCriticalError(ex, AppName + "." + method, KMCommon_Application, AppName + ": Unhandled Exception", -1, -1);
            MainLogWrite(StringFunctions.FormatException(ex));
        }
        #endregion
    }
}
