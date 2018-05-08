using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Excel;
using KM.Common;
using KM.Common.Entity;
using KM.Common.Functions;

namespace MAF.NorthStarImport
{
    public class Program
    {
        public const int MaxCacheSize = 2097152;
        public const int BufferSize = 2048;
        private const string ImportFilesMainExceptionMessage = "KMPS.ImportFiles.Main Bubble-Up Exception";
        private const string LogProcessFlatFileTemplate = "{0}-START--ProcessFlatFile()-- " + " File: {1} --";
        private const string ReasonSubscribe = "Subscribe";
        private const string ReasonBounced = "Bounced";
        private const string ReasonSpamComplaint = "SPAM Complaint";
        private const char DelimComma = ',';
        private const string LogLineSeparator = "=========================================================";
        private const string LogTotalErrorInFileTemplate = "Total Records in File : {0}";
        private const string ImportUnsubscribeMethodName = "ImportUnsubscribe";
        private const string ImportSpamComplaintsMethodName = "ImportSpamComplaints";
        private const string XmlOpeningTag = "<XML>";
        private const string XmlClosingTag = "</XML>";
        int BatchSize;
        StreamWriter customerLog;
        StreamWriter mainLog;
        int KMCommon_Application;
        string AppName = "KMPS.MAF.NorthStarImport";
        bool isDownloadComplete;
        CustomerConfig CustomerList;
        Customer CurrentCustomer;
        List<Subscribe> SubscriberList;
        List<UpdateEmailStatus> UndeliverableEmailsList;
        List<UpdateEmailStatus> SpamComplaintList;
        List<Unsubscribe> UnsubscribeList;
        bool WriteXML;
        
        private void LogExceptionForMethodName(Exception exception, string methodName)
        {
            var builder = FormatException(exception, $"{methodName}s");
            ApplicationLog.LogCriticalError(
                exception,
                $"{AppName}.Program.{methodName}",
                KMCommon_Application,
                $"{AppName}: Unhandled Exception");

            CustomerLogWrite(builder.ToString());
        }

        private void WriteXmlForMethod(string methodName, StringBuilder xmlBuilder)
        {
            if (WriteXML)
            {
                CustomerLogWrite($"{methodName} : {DateTime.Now} Send to DB");
                CustomerLogWrite("~~XML~~");
                CustomerLogWrite(xmlBuilder.ToString());
                CustomerLogWrite("~~XML~~");
            }
        }

        static void Main()
        {
            Program p = new Program();
            
            p.Start();
        }
        void Start()
        {
            SetMainLog(ConfigurationManager.AppSettings["MainLog"].ToString() + "_" + DateTime.Now.ToString("MM-dd-yyyy") + ".log");
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
                    SetCustomerLog(c.LogPath.ToString() + c.CustomerName + "_" + DateTime.Now.ToString("MM-dd-yyyy") + ".log");
                    
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
                        var builder = new StringBuilder();
                        builder.AppendLine("Customer: " + c.CustomerName);
                        builder.Append("Step: " + step);

                        var log = StringFunctions.FormatException(ex, builder.ToString());

                        MainLogWrite(log);
                        CustomerLogWrite(log);
                        KM.Common.EmailFunctions.NotifyAdmin("KMPS.ImportFiles.Main Customer Exception", log);
                    }
                    CustomerLogWrite("End Customer Process");
                }
            }
            catch (Exception ex)
            {
                var log = KM.Common.StringFunctions.FormatException(ex);
                MainLogWrite(log);
                KM.Common.EmailFunctions.NotifyAdmin(ImportFilesMainExceptionMessage, log);
            }

            MainLogWrite("Done Instance");
        }

        private void SetCustomerLog(string logFile)
        {
            customerLog = new StreamWriter(logFile, true);
        }

        private void SetMainLog(string logFile)
        {
            mainLog = new StreamWriter(logFile, true);
        }

        #region Configuration Methods
        void CreateCustomerList()
        {
            string path = ConfigurationManager.AppSettings["CustomerConfigFile"].ToString();
            XmlSerializer serializer = new XmlSerializer(typeof(CustomerConfig));//, new XmlRootAttribute("Mappings"));
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
        #endregion

        #region Download Import File from FTP
        void DownloadFiles(Process p)
        {
            try
            {
                string ftpFolder = p.FtpFolder;
                if (!string.IsNullOrEmpty(ftpFolder) && !ftpFolder.StartsWith("/"))
                    ftpFolder = "/" + ftpFolder;

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(p.FTP_Site + ftpFolder);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(p.FTP_User, p.FTP_Password);

                FtpWebResponse dirResponse = (FtpWebResponse)request.GetResponse();
                Stream dirResponseStream = dirResponse.GetResponseStream();
                StreamReader dirReader = new StreamReader(dirResponseStream);
                List<string> dirs = new List<string>();
                while (dirReader.EndOfStream == false)
                {
                    //dirs.Add(dirReader.ReadLine());
                    string info = dirReader.ReadLine();
                    if (info.Contains(".") && info.ToLower().StartsWith(p.FilePrefix.ToLower()))
                        dirs.Add(info);
                }
                string dirMsg = "File check complete for " + p.FTP_Site + ftpFolder + ", status " + dirResponse.StatusDescription;
                dirReader.Close();
                dirResponse.Close();
                request.Abort();

                string ftpURL = p.FTP_Site + ftpFolder + "/";
                string destPath = CurrentCustomer.FilePath + p.FileFolder + @"\";
                if (Directory.Exists(destPath) == false)
                    Directory.CreateDirectory(destPath);

                if (dirs.Count == 0)
                    throw new Exception("Customer: " + CurrentCustomer.CustomerName + " - " + p.ProcessType + ": No File in FTP Folder;");

                foreach (string file in dirs)
                {
                    DownLoad(ftpURL, file, p.FTP_User, p.FTP_Password, destPath);

                    if (p.DeleteFTPFile)
                    {
                        //delete file from FTP Site
                        string ftpFullPath = ftpURL + file;
                        FtpWebRequest rDelete = (FtpWebRequest)WebRequest.Create(ftpFullPath);
                        rDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                        rDelete.Credentials = new NetworkCredential(p.FTP_User, p.FTP_Password);
                        FtpWebResponse responseDelete = (FtpWebResponse)rDelete.GetResponse();

                        Stream streamDelete = responseDelete.GetResponseStream();
                        if (responseDelete.StatusCode != FtpStatusCode.FileActionOK)
                            KM.Common.EmailFunctions.NotifyAdmin("KMPS.Importfiles File Download Error", "File not deleted from " + CurrentCustomer.CustomerName + " FTP: Status Code - " + responseDelete.StatusCode.ToString() + " :: Status Description - " + responseDelete.StatusDescription.ToString());

                        responseDelete.Close();
                    }
                }

                isDownloadComplete = true;
            }
            catch (Exception ex)
            {
                LogCustomerExeception(ex, "FileDownload");
            }
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

        void DownLoad(string url, string file, string username, string password, string downloadPath)
        {
            var ftpDownload = new FtpFunctions(url, username, password);
            ftpDownload.DownloadCached(file, downloadPath);
        }

        #endregion

        #region Process Files
        void Execute(Process myProcess)
        {
            try
            {
                foreach (FileInfo fi in GetFiles(CurrentCustomer.FilePath + myProcess.FileFolder))
                {
                    try
                    {
                        if (fi.Extension.ToLower().Equals("." + FileExtension.xls.ToString()))
                        {
                            ProcessXLS(fi, myProcess);
                        }
                        else if (fi.Extension.ToLower().Equals("." + FileExtension.xlxs.ToString()))
                        {
                            ProcessXLXS(fi, myProcess);
                        }
                        else
                        {
                            ProcessFlatFile(fi, myProcess);
                        }

                        ImportData(myProcess);
                    }
                    catch (Exception ex)
                    {
                        ArchiveFile(fi, myProcess);
                        KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "KMPS.ImportFiles.Program.Execute - " + myProcess.ProcessType.ToString(), KMCommon_Application, "KMPS.ImportFiles: " + myProcess.ProcessType.ToString() + " Unhandled Exception", -1, -1);
                        CustomerLogWrite(StringFunctions.FormatException(ex));
                    }
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, AppName + ".Program.Execute - " + myProcess.ProcessType.ToString(), KMCommon_Application, AppName + ".Program.Execute - " + myProcess.ProcessType.ToString() + ": Unhandled Exception", -1, -1);
                CustomerLogWrite(StringFunctions.FormatException(ex));
            }
        }

        private FileInfo[] GetFiles(string path)
        {
            var directory = new DirectoryInfo(path);
            return directory.GetFiles();
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

        private void ProcessXLS(FileInfo file, Process myProcess)
        {
            ProcessExcel(file, myProcess, ProcessXLS);
        }

        private void ProcessXLXS(FileInfo file, Process myProcess)
        {
            ProcessExcel(file, myProcess, ProcessXLSX);
        }

        private void ProcessExcel(FileInfo file, Process myProcess, Func<FileStream, DataSet> processExcelFunc)
        {
            Guard.NotNull(myProcess, nameof(myProcess));

            List<KeyValuePair<int, string>> columnIndexList;
            var xlsDataTable = ProcessExcelParseFile(file, processExcelFunc, out columnIndexList);

            ArchiveFile(file, myProcess);
            ProcessExcelSetListObjects(myProcess);

            var rowIndex = 0;
            foreach (DataRow dataRow in xlsDataTable.Rows)
            {
                var newSub = new Subscribe();
                var upEmailStatus = new UpdateEmailStatus();
                var spamComplaint = new UpdateEmailStatus();
                var unSub = new Unsubscribe();
                var spamDate = DateTime.Now;
                var unSubDate = DateTime.Now;

                foreach (var columnIndex in columnIndexList)
                {
                    var columnValue = dataRow[columnIndex.Key].ToString();
                    if (myProcess.HeaderMap.Exists(
                        x => x.FileField.Equals(columnIndex.Value, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        var ifHeader = myProcess.HeaderMap.Single(
                            x => x.FileField.Equals(columnIndex.Value, StringComparison.CurrentCultureIgnoreCase));

                        if (myProcess.ProcessType.Equals(ProcessType.Subscribe.ToString()))
                        {
                            ProcessExcelSubscribe(ifHeader, newSub, columnValue);
                        }
                        else if (myProcess.ProcessType.Equals(ProcessType.UndeliverableEmails.ToString()))
                        {
                            ProcessExcelUndeliverableEmails(ifHeader, upEmailStatus, columnValue);
                        }
                        else if (myProcess.ProcessType.Equals(ProcessType.SpamComplaints.ToString()))
                        {
                            spamDate = ProcessExcelSpamComplaints(ifHeader, spamComplaint, columnValue, spamDate);
                        }
                        else if (myProcess.ProcessType.Equals(ProcessType.Unsubscribe.ToString()))
                        {
                            unSubDate = ProcessExcellUnsubscribe(ifHeader, unSub, columnValue, unSubDate);
                        }
                    }
                }

                var flatFileNewObjectArgs = new FileNewObjectArgs(
                    myProcess,
                    newSub,
                    upEmailStatus,
                    spamComplaint,
                    spamDate,
                    unSub,
                    unSubDate);
                AddNewObject(flatFileNewObjectArgs);

                rowIndex++;
            }
        }

        private static void ProcessExcelUndeliverableEmails(
            Header ifHeader, 
            UpdateEmailStatus upEmailStatus,
            string columnValue)
        {
            Guard.NotNull(ifHeader, nameof(ifHeader));
            Guard.NotNull(upEmailStatus, nameof(upEmailStatus));

            if (ifHeader.MFField.Equals(
                MFFields.Email.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                upEmailStatus.EmailAddress = columnValue;
            }
        }

        private static DateTime ProcessExcelSpamComplaints(
            Header ifHeader, 
            UpdateEmailStatus spamComplaint, 
            string columnValue,
            DateTime spamDate)
        {
            Guard.NotNull(ifHeader, nameof(ifHeader));
            Guard.NotNull(spamComplaint, nameof(spamComplaint));

            if (ifHeader.MFField.Equals(
                MFFields.Email.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                spamComplaint.EmailAddress = columnValue;
            }

            if (ifHeader.MFField.Equals(
                MFFields.StatusUpdatedDate.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                DateTime.TryParse(columnValue, out spamDate);
            }

            return spamDate;
        }

        private static DateTime ProcessExcellUnsubscribe(
            Header ifHeader, 
            Unsubscribe unSub, 
            string columnValue,
            DateTime unSubDate)
        {
            Guard.NotNull(ifHeader, nameof(ifHeader));
            Guard.NotNull(unSub, nameof(unSub));

            if (ifHeader.MFField.Equals(
                MFFields.Email.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                unSub.EmailAddress = columnValue;
            }

            if (ifHeader.MFField.Equals(
                MFFields.PubCode.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                unSub.PubCode = columnValue;
            }

            if (ifHeader.MFField.Equals(
                MFFields.StatusUpdatedDate.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                DateTime.TryParse(columnValue, out unSubDate);
            }

            if (ifHeader.MFField.Equals(
                MFFields.StatusUpdatedReason.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                unSub.Reason = columnValue;
            }

            return unSubDate;
        }

        private static void ProcessExcelSubscribe(Header ifHeader, Subscribe newSub, string columnValue)
        {
            Guard.NotNull(ifHeader, nameof(ifHeader));
            Guard.NotNull(newSub, nameof(newSub));

            if (ifHeader.MFField.Equals(
                MFFields.Email.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                newSub.EmailAddress = columnValue;
            }

            if (ifHeader.MFField.Equals(
                MFFields.PubCode.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                newSub.PubCode = columnValue;
            }

            if (ifHeader.MFField.EndsWith(MFFields.FName.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                try
                {
                    newSub.FirstName = columnValue.Split(DelimComma).Last();
                }
                catch(Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    newSub.FirstName = columnValue;
                }
            }

            if (ifHeader.MFField.StartsWith(MFFields.LName.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                try
                {
                    newSub.LastName = columnValue.Split(DelimComma).First();
                }
                catch(Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    newSub.LastName = columnValue;
                }
            }
        }

        private void ProcessExcelSetListObjects(Process myProcess)
        {
            Guard.NotNull(myProcess, nameof(myProcess));

            if (myProcess.ProcessType.Equals(ProcessType.Subscribe.ToString()))
            {
                SubscriberList = new List<Subscribe>();
            }
            else if (myProcess.ProcessType.Equals(ProcessType.UndeliverableEmails.ToString()))
            {
                UndeliverableEmailsList = new List<UpdateEmailStatus>();
            }
            else if (myProcess.ProcessType.Equals(ProcessType.SpamComplaints.ToString()))
            {
                SpamComplaintList = new List<UpdateEmailStatus>();
            }
            else if (myProcess.ProcessType.Equals(ProcessType.Unsubscribe.ToString()))
            {
                UnsubscribeList = new List<Unsubscribe>();
            }
        }

        private static DataTable ProcessExcelParseFile(
            FileSystemInfo file, 
            Func<FileStream, DataSet> processExcelFunc, 
            out List<KeyValuePair<int, string>> columnIndexList)
        {
            Guard.NotNull(file, nameof(file));
            Guard.NotNull(processExcelFunc, nameof(processExcelFunc));

            var stream = File.Open(file.FullName, FileMode.Open, FileAccess.Read);
            columnIndexList = new List<KeyValuePair<int, string>>();
            var xlsDataTable = processExcelFunc(stream).Tables[0];

            var index = 0;
            foreach (DataColumn dc in xlsDataTable.Columns)
            {
                dc.ColumnName = dc.ColumnName.Trim();
                columnIndexList.Add(new KeyValuePair<int, string>(index, dc.ColumnName));
                index++;
            }

            return xlsDataTable;
        }

        private void ProcessFlatFile(FileInfo file, Process myProcess)
        {
            Guard.NotNull(file, nameof(file));
            Guard.NotNull(myProcess, nameof(myProcess));

            var logMessage = string.Format(
                LogProcessFlatFileTemplate,
                DateTime.Now,
                file.Name);
            CustomerLogWrite(logMessage);

            using (var dtFile = LoadFile(file, myProcess))
            {
                var totalRecords = dtFile.Rows.Count;
                LogTotalRecords(totalRecords);

                var columnIndexList = new Dictionary<int, string>();
                var index = 0;
                foreach (DataColumn c in dtFile.Columns)
                {
                    columnIndexList.Add(index, c.ColumnName);
                    index++;
                }

                ArchiveFile(file, myProcess);

                if (myProcess.ProcessType.Equals(ProcessType.Subscribe.ToString()))
                {
                    SubscriberList = new List<Subscribe>();
                }
                else if (myProcess.ProcessType.Equals(ProcessType.UndeliverableEmails.ToString()))
                {
                    UndeliverableEmailsList = new List<UpdateEmailStatus>();
                }
                else if (myProcess.ProcessType.Equals(ProcessType.SpamComplaints.ToString()))
                {
                    SpamComplaintList = new List<UpdateEmailStatus>();
                }
                else if (myProcess.ProcessType.Equals(ProcessType.Unsubscribe.ToString()))
                {
                    UnsubscribeList = new List<Unsubscribe>();
                }

                FlatFileCreateImportObjectList(myProcess, dtFile, columnIndexList);
            }
        }

        private void FlatFileCreateImportObjectList(Process myProcess, DataTable dtFile, Dictionary<int, string> columnIndexList)
        {
            var rowIndex = 0;
            foreach (DataRow row in dtFile.Rows)
            {
                var newSub = new Subscribe();
                var upEmailStatus = new UpdateEmailStatus();
                var spamComplaint = new UpdateEmailStatus();
                var unSub = new Unsubscribe();
                var spamDate = DateTime.Now;
                var unSubDate = DateTime.Now;

                foreach (var columnIndex in columnIndexList)
                {
                    var columnValue = row[columnIndex.Value].ToString();
                    if (myProcess.HeaderMap.Exists(
                        x => x.FileField.Equals(columnIndex.Value, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        var ifHeader = myProcess.HeaderMap.Single(
                            x => x.FileField.Equals(columnIndex.Value, StringComparison.CurrentCultureIgnoreCase));
                        if (!ifHeader.Ignore)
                        {
                            if (myProcess.ProcessType.Equals(ProcessType.Subscribe.ToString()))
                            {
                                FlatFileCreateImportObjectListSubscribe(ifHeader, newSub, columnValue);
                            }
                            else if (myProcess.ProcessType.Equals(ProcessType.UndeliverableEmails.ToString()))
                            {
                                if (ifHeader.MFField.Equals(MFFields.Email.ToString(),
                                    StringComparison.CurrentCultureIgnoreCase))
                                {
                                    upEmailStatus.EmailAddress = columnValue;
                                }
                            }
                            else if (myProcess.ProcessType.Equals(ProcessType.SpamComplaints.ToString()))
                            {
                                spamDate = FlatFileCreateImportObjectListSpamComplaints(
                                    ifHeader, spamComplaint, columnValue, spamDate);
                            }
                            else if (myProcess.ProcessType.Equals(ProcessType.Unsubscribe.ToString()))
                            {
                                unSubDate = FlatFileCreateImportObjectListUnsibscribe(
                                    ifHeader, unSub, columnValue, unSubDate);
                            }
                        }
                    }
                }

                var flatFileNewObjectArgs = new FileNewObjectArgs(
                    myProcess, 
                    newSub, 
                    upEmailStatus, 
                    spamComplaint, 
                    spamDate, 
                    unSub, 
                    unSubDate);
                AddNewObject(flatFileNewObjectArgs);

                rowIndex++;
            }
        }

        private void AddNewObject(FileNewObjectArgs flatFileNewObjectArgs)
        {
            Guard.NotNull(flatFileNewObjectArgs, nameof(flatFileNewObjectArgs));

            if (flatFileNewObjectArgs.MyProcess.ProcessType.Equals(ProcessType.Subscribe.ToString()))
            {
                flatFileNewObjectArgs.NewSub.Reason = ReasonSubscribe;
                flatFileNewObjectArgs.NewSub.RequestDate = DateTime.Now;
                SubscriberList.Add(flatFileNewObjectArgs.NewSub);
            }
            else if (flatFileNewObjectArgs.MyProcess.ProcessType.Equals(ProcessType.UndeliverableEmails.ToString()))
            {
                flatFileNewObjectArgs.UpEmailStatus.EmailStatus = EmailStatusType.Bounced.ToString();
                flatFileNewObjectArgs.UpEmailStatus.UpdateReason = ReasonBounced;
                flatFileNewObjectArgs.UpEmailStatus.UpdateDate = DateTime.Now;
                UndeliverableEmailsList.Add(flatFileNewObjectArgs.UpEmailStatus);
            }
            else if (flatFileNewObjectArgs.MyProcess.ProcessType.Equals(ProcessType.SpamComplaints.ToString()))
            {
                flatFileNewObjectArgs.SpamComplaint.EmailStatus = EmailStatusType.Spam.ToString();
                flatFileNewObjectArgs.SpamComplaint.UpdateReason = ReasonSpamComplaint;
                flatFileNewObjectArgs.SpamComplaint.UpdateDate = flatFileNewObjectArgs.SpamDate;
                SpamComplaintList.Add(flatFileNewObjectArgs.SpamComplaint);
            }
            else if (flatFileNewObjectArgs.MyProcess.ProcessType.Equals(ProcessType.Unsubscribe.ToString()))
            {
                flatFileNewObjectArgs.UnSub.RequestDate = flatFileNewObjectArgs.UnSubDate;
                UnsubscribeList.Add(flatFileNewObjectArgs.UnSub);
            }
        }

        private static DateTime FlatFileCreateImportObjectListSpamComplaints(
            Header ifHeader, 
            UpdateEmailStatus spamComplaint, 
            string columnValue, 
            DateTime spamDate)
        {
            if (ifHeader.MFField.Equals(MFFields.Email.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                spamComplaint.EmailAddress = columnValue;
            }

            if (ifHeader.MFField.Equals(
                MFFields.StatusUpdatedDate.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                DateTime.TryParse(columnValue, out spamDate);
            }

            return spamDate;
        }

        private static DateTime FlatFileCreateImportObjectListUnsibscribe(
            Header ifHeader, 
            Unsubscribe unSub,
            string columnValue, 
            DateTime unSubDate)
        {
            if (ifHeader.MFField.Equals(MFFields.Email.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                unSub.EmailAddress = columnValue;
            }

            if (ifHeader.MFField.Equals(MFFields.PubCode.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                unSub.PubCode = columnValue;
            }

            if (ifHeader.MFField.Equals(
                MFFields.StatusUpdatedDate.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                DateTime.TryParse(columnValue, out unSubDate);
            }

            if (ifHeader.MFField.Equals(
                MFFields.StatusUpdatedReason.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                unSub.Reason = columnValue;
            }

            return unSubDate;
        }

        private static void FlatFileCreateImportObjectListSubscribe(
            Header ifHeader, Subscribe newSub, string columnValue)
        {
            if (ifHeader.MFField.Equals(MFFields.Email.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                newSub.EmailAddress = columnValue;
            }

            if (ifHeader.MFField.Equals(MFFields.PubCode.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                newSub.PubCode = columnValue;
            }

            if (ifHeader.MFField.EndsWith(MFFields.FName.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                try
                {
                    newSub.FirstName = columnValue.Split(DelimComma).Last();
                }
                catch(Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    newSub.FirstName = columnValue;
                }
            }

            if (ifHeader.MFField.StartsWith(MFFields.LName.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                try
                {
                    newSub.LastName = columnValue.Split(DelimComma).First();
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    newSub.LastName = columnValue;
                }
            }
        }

        private void LogTotalRecords(int totalRecords)
        {
            CustomerLogWrite(string.Empty);
            CustomerLogWrite(LogLineSeparator);
            CustomerLogWrite(string.Format(LogTotalErrorInFileTemplate, totalRecords));
            CustomerLogWrite(LogLineSeparator);
            CustomerLogWrite(string.Empty);
        }

        DataTable LoadFile(FileInfo file, Process myProcess)
        {
            Microsoft.VisualBasic.FileIO.TextFieldParser tfp = new Microsoft.VisualBasic.FileIO.TextFieldParser(file.FullName);
            tfp.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
            ColumnDelimiter delimiter = GetColumnDelimiter(myProcess.ColumnDelimiter.ToLower());
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

            if (myProcess.IsQuoteEncapsulated == true)
                tfp.HasFieldsEnclosedInQuotes = true;
            else
                tfp.HasFieldsEnclosedInQuotes = false;

            //convert to dataset:
            DataSet ds = new DataSet();
            ds.Tables.Add("Data");

            String[] stringRow = tfp.ReadFields();
            foreach (String field in stringRow)
            {
                ds.Tables[0].Columns.Add(field, Type.GetType("System.String"));
            }

            //int recordcount = 0;

            //populate with data:
            while (!tfp.EndOfData)
            {
                stringRow = tfp.ReadFields();
                ds.Tables[0].Rows.Add(stringRow);
                //recordcount++;
                //CustomerLogWrite(string.Format("Processing Record : {0}", recordcount));
            }

            tfp.Close();
            tfp.Dispose();

            return ds.Tables["Data"];
        }
        #endregion
        #region move file - archive the file
        void ArchiveFile(FileInfo file, Process myProcess)
        {
            if (File.Exists(file.FullName) && myProcess != null && myProcess.FileFolder != null)
            {
                if (!Directory.Exists(CurrentCustomer.FileArchive + myProcess.FileFolder))
                    Directory.CreateDirectory(CurrentCustomer.FileArchive + myProcess.FileFolder);

                string fileName = DateTime.Now.ToString("MM-dd-yyyy") + "_" + file.Name;
                string movePath = CurrentCustomer.FileArchive + myProcess.FileFolder + fileName;
                try
                {
                    File.Move(file.FullName, movePath);
                }
                catch { }
            }
        }
        #endregion
        #region Import to MasterFile
        void ImportData(Process myProcess)
        {
            #region ProcessType.Unsubscribe
            if (myProcess.ProcessType.Equals(ProcessType.Unsubscribe.ToString()))
            {
                try
                {
                    ImportUnsubscribe(CurrentCustomer.SQL);
                }
                catch (Exception ex)
                {
                    var sbLog = new StringBuilder();
                    sbLog.AppendLine("Customer Exception - " + DateTime.Now);
                    sbLog.AppendLine("Customer: " + CurrentCustomer.CustomerName);
                    sbLog.AppendLine("Process: " + myProcess.ProcessType);

                    var log = StringFunctions.FormatException(ex, sbLog.ToString());
                    
                    MainLogWrite(log);
                    CustomerLogWrite(log);
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "KMPS.ImportFiles.ImportToMF - Unsubscribe", KMCommon_Application, "KMPS.ImportFiles: Customer Exception", -1, CurrentCustomer.CustomerID);
                }
            }
            #endregion
            #region ProcessType.SpamComplaints
            else if (myProcess.ProcessType.Equals(ProcessType.SpamComplaints.ToString()))
            {
                try
                {
                    ImportSpamComplaints(CurrentCustomer.SQL);
                }
                catch (Exception ex)
                {
                    var sbLog = new StringBuilder();
                    sbLog.AppendLine("Customer Exception - " + DateTime.Now);
                    sbLog.AppendLine("Customer: " + CurrentCustomer.CustomerName);
                    sbLog.AppendLine("Process: " + myProcess.ProcessType);

                    var log = StringFunctions.FormatException(ex, sbLog.ToString());
                    
                    MainLogWrite(log);
                    CustomerLogWrite(log);
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "KMPS.ImportFiles.ImportToMF - SpamComplaints", KMCommon_Application, "KMPS.ImportFiles: Customer Exception", -1, CurrentCustomer.CustomerID);
                }
            }
            #endregion
            #region ProcessType.UndeliverableEmails
            else if (myProcess.ProcessType.Equals(ProcessType.UndeliverableEmails.ToString()))
            {
                try
                {
                    ImportUndeliverableEmails(CurrentCustomer.SQL);
                }
                catch (Exception ex)
                {
                    var sbLog = new StringBuilder();
                    sbLog.AppendLine("Customer Exception - " + DateTime.Now);
                    sbLog.AppendLine("Customer: " + CurrentCustomer.CustomerName);
                    sbLog.AppendLine("Process: " + myProcess.ProcessType);

                    var log = StringFunctions.FormatException(ex, sbLog.ToString());
                    
                    MainLogWrite(log);
                    CustomerLogWrite(log);
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "KMPS.ImportFiles.ImportToMF - UndeliverableEmails", KMCommon_Application, "KMPS.ImportFiles: Customer Exception", -1, CurrentCustomer.CustomerID);
                }
            }
            #endregion
            #region ProcessType.Subscribe
            else if (myProcess.ProcessType.Equals(ProcessType.Subscribe.ToString()))
            {
                try
                {
                    ImportSubscribe(CurrentCustomer.SQL);
                }
                catch (Exception ex)
                {
                    var sbLog = new StringBuilder();
                    sbLog.AppendLine("Customer Exception - " + DateTime.Now);
                    sbLog.AppendLine("Customer: " + CurrentCustomer.CustomerName);
                    sbLog.AppendLine("Process: " + myProcess.ProcessType);

                    var log = StringFunctions.FormatException(ex, sbLog.ToString());
                    
                    MainLogWrite(log);
                    CustomerLogWrite(log);
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "KMPS.ImportFiles.ImportToMF - Subscribe", KMCommon_Application, "KMPS.ImportFiles: Customer Exception", -1, CurrentCustomer.CustomerID);
                }
            }
            #endregion
        }

        private void ImportUnsubscribe(string sqlConn)
        {
            // send unsubList to a method to process by batching records into groups of 5k xml file - send to sproc
            var totalRecords = UnsubscribeList.Count;
            var processed = 0;

            while (processed < totalRecords)
            {
                var listBatch = UnsubscribeList.Skip(processed)
                    .Take(BatchSize);

                var xmlBuilder = new StringBuilder();
                xmlBuilder.Append(XmlOpeningTag);
                foreach (var unsubscribe in listBatch)
                {
                    var pubCode = unsubscribe.PubCode ?? string.Empty;
                    var email = unsubscribe.EmailAddress ?? string.Empty;
                    var reason = string.IsNullOrEmpty(unsubscribe.Reason) 
                                     ? string.Empty 
                                     : unsubscribe.Reason;
                    
                    xmlBuilder.Append("<Emails>")
                        .Append($"<EmailAddress>{CleanXMLString(email)}</EmailAddress>")
                        .Append($"<PubCode>{CleanXMLString(pubCode)}</PubCode>")
                        .Append($"<Reason>{CleanXMLString(reason)}</Reason>")
                        .Append($"<RequestDate>{unsubscribe.RequestDate}</RequestDate>")
                        .Append("</Emails>");

                    processed++;
                }

                xmlBuilder.Append(XmlClosingTag);

                try
                {
                    WriteXmlForMethod(ImportUnsubscribeMethodName, xmlBuilder);

                    Data.ImportUnsubscribes(xmlBuilder.ToString(), sqlConn);
                    CustomerLogWrite($"{ImportUnsubscribeMethodName} : {processed} of {totalRecords} / {DateTime.Now} Done DB");
                }
                catch (Exception exception)
                {
                    LogExceptionForMethodName(exception, ImportUnsubscribeMethodName);
                }
            }
        }

        private void ImportSpamComplaints(string sqlConn)
        {
            // for each email in Subscriptions and PubSubscriptions set to SPAM
            // send unsubList to a method to process by batching records into groups of 5k xml file - send to sproc
            var totalRecords = SpamComplaintList.Count;
            var processed = 0;

            while (processed < totalRecords)
            {
                var listBatch = SpamComplaintList.Skip(processed)
                    .Take(BatchSize);

                var xmlBuilder = new StringBuilder();
                xmlBuilder.Append(XmlOpeningTag);
                foreach (var emailStatus in listBatch)
                {
                    var email = emailStatus.EmailAddress ?? string.Empty;
                    var reason = emailStatus.UpdateReason ?? string.Empty;
                    xmlBuilder.Append("<Emails>")
                        .Append($"<EmailAddress>{CleanXMLString(email)}</EmailAddress>")
                        .Append($"<UpdateDate>{emailStatus.UpdateDate}</UpdateDate>")
                        .Append($"<UpdateReason>{CleanXMLString(reason)}</UpdateReason>")
                        .Append("</Emails>");

                    processed++;
                }

                xmlBuilder.Append(XmlClosingTag);

                try
                {
                    WriteXmlForMethod(ImportSpamComplaintsMethodName, xmlBuilder);

                    Data.UpdateEmailStatus(xmlBuilder.ToString(), EmailStatusType.Spam.ToString(), sqlConn);
                    CustomerLogWrite($"{ImportSpamComplaintsMethodName} : {DateTime.Now} Done DB");
                }
                catch (Exception exception)
                {
                    LogExceptionForMethodName(exception, ImportSpamComplaintsMethodName);
                }
            }
        }
        void ImportUndeliverableEmails(string sqlConn)
        {
            //for each email in Subscriptions and PubSubscriptions set to BOUNCED
            //send unsubList to a method to process by batching records into groups of 5k xml file - send to sproc
            int totalRecords = UndeliverableEmailsList.Count;
            int processed = 0;


            while (processed < totalRecords)
            {
                List<UpdateEmailStatus> listBatch = UndeliverableEmailsList.Skip(processed).Take(BatchSize).ToList();
                StringBuilder xml = new StringBuilder();
                xml.Append("<XML>");
                foreach (UpdateEmailStatus x in listBatch)
                {
                    string email = x.EmailAddress == null ? string.Empty : x.EmailAddress;
                    string reason = x.UpdateReason == null ? string.Empty : x.UpdateReason;

                    
                    xml.Append("<Emails>");
                    xml.Append("<EmailAddress>" + CleanXMLString(email) + "</EmailAddress>");
                    xml.Append("<UpdateDate>" + x.UpdateDate + "</UpdateDate>");
                    xml.Append("<UpdateReason>" + CleanXMLString(reason) + "</UpdateReason>");
                    xml.Append("</Emails>");

                    processed++;
                }
                xml.Append("</XML>");

                try
                {
                    if (WriteXML == true)
                    {
                        CustomerLogWrite("ImportUndeliverableemails : " + DateTime.Now.ToString() + " Send to DB");
                        CustomerLogWrite("~~XML~~");
                        CustomerLogWrite(xml.ToString());
                        CustomerLogWrite("~~XML~~");
                    }
                    Data.UpdateEmailStatus(xml.ToString(), EmailStatusType.Bounced.ToString(), sqlConn);
                    CustomerLogWrite("ImportUndeliverableemails : " + processed.ToString() + " of " + totalRecords.ToString() + " / " + DateTime.Now.ToString() + " Done DB");
                }
                catch (Exception ex)
                {
                    StringBuilder sbLog = FormatException(ex, "ImportUndeliverableemails");
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, AppName + ".Program.ImportUndeliverableemails", KMCommon_Application, AppName + ": Unhandled Exception", -1, -1);
                    CustomerLogWrite(sbLog.ToString());
                }
            }
        }
        void ImportSubscribe(string sqlConn)
        {
            //send unsubList to a method to process by batching records into groups of 5k xml file - send to sproc
            int totalRecords = SubscriberList.Count;
            int processed = 0;
            int batchcount = 0;

            while (processed < totalRecords)
            {
                List<Subscribe> listBatch = SubscriberList.Skip(processed).Take(BatchSize).ToList();
                StringBuilder xml = new StringBuilder();
                xml.Append("<XML>");
                foreach (Subscribe s in listBatch)
                {
                    //CustomerLogWrite("ImportSubscribe : " + processed.ToString() + " of " + totalRecords.ToString());
                    string pubCode = s.PubCode == null ? string.Empty : s.PubCode;
                    string email = s.EmailAddress == null ? string.Empty : s.EmailAddress;
                    string fName = s.FirstName == null ? string.Empty : s.FirstName;
                    string lName = s.LastName == null ? string.Empty : s.LastName;
                    string reason = s.Reason == null ? string.Empty : s.Reason;

                    xml.Append("<Emails>");
                    xml.Append("<PubCode>" + CleanXMLString(pubCode) + "</PubCode>");
                    xml.Append("<EmailAddress>" + CleanXMLString(email) + "</EmailAddress>");
                    xml.Append("<FirstName>" + CleanXMLString(fName) + "</FirstName>");
                    xml.Append("<LastName>" + CleanXMLString(lName) + "</LastName>");
                    xml.Append("<Reason>" + CleanXMLString(reason) + "</Reason>");
                    xml.Append("<RequestDate>" + s.RequestDate + "</RequestDate>");
                    xml.Append("</Emails>");

                    processed++;
                }
                xml.Append("</XML>");

                try
                {
                    if (WriteXML == true)
                    {
                        CustomerLogWrite("ImportSubscribe : " + DateTime.Now.ToString() + " Send to DB");
                        CustomerLogWrite("~~XML~~");
                        CustomerLogWrite(xml.ToString());
                        CustomerLogWrite("~~XML~~");
                    }
                    batchcount++;

                    CustomerLogWrite("batchcount : " + batchcount);

                    Data.ImportSubscribes(xml.ToString(), sqlConn);
                    CustomerLogWrite("ImportSubscribe : " + DateTime.Now.ToString() + " Done DB");
                }
                catch (Exception ex)
                {
                    StringBuilder sbLog = FormatException(ex, "ImportSubscribe");
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, AppName + ".Program.ImportSubscribe", KMCommon_Application, AppName + ": Unhandled Exception", -1, -1);
                    CustomerLogWrite(sbLog.ToString());
                }
            }
        }
        #endregion

        #region Helpers
        public static StringBuilder FormatException(Exception ex, string method)
        {
            StringBuilder sbLog = new StringBuilder();
            sbLog.AppendLine("**********************");
            sbLog.AppendLine("Exception - " + DateTime.Now.ToString());
            sbLog.AppendLine("Method: " + method);
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

        private static string CleanXMLString(string dirty)
        {
            return XMLFunctions.EscapeXmlString(dirty);
        }

        private static void LogWrite(StreamWriter stream, string text)
        {
            Console.WriteLine(text);

            stream.AutoFlush = true;
            stream.WriteLine($"{DateTime.Now} {text}");
            stream.Flush();
        }

        private void CustomerLogWrite(string text)
        {
            LogWrite(customerLog, text);
        }

        private void MainLogWrite(string text)
        {
            LogWrite(mainLog, text);
        }

        public enum EmailStatusType
        {
            Active,
            Bounced,
            Invalid,
            MasterSuppressed,
            Spam,
            UnSubscribe
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
            if (ex.Message.Contains("No File in FTP Folder;"))
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

    #region Objects
    public class Unsubscribe
    {
        public Unsubscribe() { }
        public string EmailAddress { get; set; }
        public DateTime RequestDate { get; set; }
        public string PubCode { get; set; }
        public string Reason { get; set; }
    }
    public class Subscribe
    {
        public Subscribe() { }
        public string PubCode { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RequestDate { get; set; }
        public string Reason { get; set; }
    }
    public class UpdateEmailStatus
    {
        public UpdateEmailStatus() { }
        public string EmailAddress { get; set; }
        public string EmailStatus { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateReason { get; set; }
    }
    #endregion

    #region xml file CustomerConfig classes
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRoot("Customers")]
    public class CustomerConfig
    {
        [XmlElement("Customer")]
        public List<Customer> Customers { get; set; }
    }
    public class Customer
    {
        public Customer() { }

        [XmlElement("CustomerID")]
        public int CustomerID { get; set; }
        [XmlElement("CustomerName")]
        public string CustomerName { get; set; }
        [XmlElement("SQL")]
        public string SQL { get; set; }
        [XmlElement("FilePath")]
        public string FilePath { get; set; }
        [XmlElement("FileArchive")]
        public string FileArchive { get; set; }
        [XmlElement("LogPath")]
        public string LogPath { get; set; }
        [XmlElement("MissFilePath")]
        public string MissFilePath { get; set; }
        [XmlElement("BadFilePath")]
        public string BadFilePath { get; set; }
        [XmlArray("Processes"), XmlArrayItem("Process")]
        public List<Process> Processes { get; set; }
    }
    [XmlRoot("Process")]
    public class Process
    {
        public Process() { }

        [XmlElement("ProcessType")]
        public string ProcessType { get; set; }
        [XmlElement("IsEnabled")]
        public bool IsEnabled { get; set; }
        [XmlElement("FTP_Site")]
        public string FTP_Site { get; set; }
        [XmlElement("FTP_User")]
        public string FTP_User { get; set; }
        [XmlElement("FTP_Password")]
        public string FTP_Password { get; set; }
        [XmlElement("FtpFolder")]
        public string FtpFolder { get; set; }
        [XmlElement("FileFolder")]
        public string FileFolder { get; set; }
        [XmlElement("FilePrefix")]
        public string FilePrefix { get; set; }
        [XmlElement("FileExtension")]
        public string FileExtension { get; set; }
        [XmlElement("IsQuoteEncapsulated")]
        public bool IsQuoteEncapsulated { get; set; }
        [XmlElement("DeleteFTPFile")]
        public bool DeleteFTPFile { get; set; }
        [XmlElement("ColumnDelimiter")]
        public string ColumnDelimiter { get; set; }
        [XmlArray("HeaderMap"), XmlArrayItem("Header")]
        public List<Header> HeaderMap { get; set; }
    }
    [XmlRoot("Header")]
    public class Header
    {
        public Header() { }
        #region Properties
        [XmlElement("FileField")]
        public string FileField { get; set; }

        [XmlElement("MFField")]
        public string MFField { get; set; }

        [XmlElement("Ignore")]
        public bool Ignore { get; set; }

        public int RowIndex { get; set; }

        public string Value { get; set; }
        #endregion
    }
    #endregion

    #region Enums
    public enum MFFields
    {
        StatusUpdatedDate,
        Email,
        PubCode,
        StatusUpdatedReason,
        FName,
        LName
    }
    public enum ProcessType
    {
        Unsubscribe,
        UndeliverableEmails,
        SpamComplaints,
        Subscribe
    }
    public enum FileExtension
    {
        xls,
        xlxs,
        csv,
        txt,
        xml
    }
    public enum ColumnDelimiter
    {
        comma,
        tab,
        semicolon,
        colon,
        tild
    }
    #endregion
}
