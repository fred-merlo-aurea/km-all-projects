using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Core_AMS.Utilities;
using FrameworkSubGen.Entity;
using FrameworkUAD.Entity;
using KM.Common;
using KM.Common.Functions;
using KM.Common.Import;
using CoreStringFunctions = Core_AMS.Utilities.StringFunctions;
using Enums = FrameworkSubGen.Entity.Enums;
using KMBusiness = KMPlatform.BusinessLogic;
using FileFunctions = Core_AMS.Utilities.FileFunctions;
using ImportError = FrameworkSubGen.Object.ImportError;
using StringFunctions = KM.Common.StringFunctions;

namespace AMS_Operations
{
    class Operations
    {
        public static readonly int DeleteFileSleep = 5000;
        public static readonly int DeleteFileAttempts = 10;
        private static List<FrameworkSubGen.Entity.Account> accounts;
        private List<FrameworkSubGen.Entity.Publication> publications;
        private List<FrameworkSubGen.Object.ImportError> importErrors;
        private string runningArg;

        static void Main(string[] args)
        {
            try
            {
                Operations ops = new Operations();
                System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
                //this code is for local testing
                // args = new string[1];
                //args[0] = "SATGNTIOTRANS";//CDC SGSubscriberFile
                //args[1] = "File";//Acceptable values are File or DQM - File will create 2 output files, DQM will run through ADMS-DQM process
                if (args.Length > 0)
                {
                    string RunWhat = args[0].ToUpper();

                    #region ClientArchive
                    if (RunWhat.Equals("CLIENTARCHIVE"))
                    {
                        ops.ClientArchive();
                    }
                    #endregion
                    #region ProcessingStats
                    if (RunWhat.Equals("PROCESSINGSTATS"))
                        ops.ProcessingStats();
                    #endregion
                    #region SourceMediaPrinterFileExport
                    if (RunWhat.Equals("SOURCEMEDIAPRINTERFILEEXPORT"))
                    {
                        ops.SourceMediaPrinterFileExport();
                    }
                    #endregion
                    #region File transfer
                    if (RunWhat.Equals("FILETRANSFER") && args.Length > 2)
                    {
                        if (args.Length == 4)
                            ops.FileTransfer(args[1], args[2], args[3]);
                        else if (args.Length == 3)
                            ops.FileTransfer(args[1], args[2]);
                    }
                    #endregion
                    #region SAE file transfer
                    if (RunWhat.Equals("SAEFTPTRANSFER"))
                    {
                        ops.SAEFTPTransfer();
                    }
                    #endregion
                    #region HW file transfer
                    if (RunWhat.Equals("HWFTPTRANSFER"))
                    {
                        ops.HWFTPTransfer();
                    }
                    #endregion
                    #region Move files
                    if (RunWhat.Equals("MOVEFILES"))
                    {
                        ops.MoveFiles();
                    }
                    #endregion
                    #region Validate
                    if (RunWhat.Equals("VALIDATEFILES"))
                    {
                        ops.Validate();
                    }
                    #endregion
                    #region create NoValue answer for CodeSheets
                    if (RunWhat.Equals("NOVALUE"))
                    {
                        ops.NoValueSetup();
                    }
                    #endregion
                    #region SubGen Integration
                    string outPut = "DQM";//Acceptable values are File or DQM - File will create 2 output files, DQM will run through ADMS-DQM process
                    if (args.Length > 1)
                    {
                        if (args[1] != null)
                            outPut = args[1].ToString();
                    }
                    if (args[0].Equals("SGOrderFile", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ops.runningArg = "SGOrderFile";
                        accounts = new List<FrameworkSubGen.Entity.Account>();
                        ops.publications = new List<FrameworkSubGen.Entity.Publication>();
                        ops.importErrors = new List<FrameworkSubGen.Object.ImportError>();
                        ops.LoadAccountsFromDataBase();
                        ops.SubGenOrderFileImport(outPut);
                        //always last step
                        ops.EmailErrorLog();
                    }
                    if (args[0].Equals("SGSubscriberFile", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ops.runningArg = "SGSubscriberFile";
                        accounts = new List<FrameworkSubGen.Entity.Account>();
                        ops.publications = new List<FrameworkSubGen.Entity.Publication>();
                        ops.importErrors = new List<FrameworkSubGen.Object.ImportError>();
                        ops.LoadAccountsFromDataBase();
                        ops.SubGenSubscriberFileImport(outPut);
                        //always last step
                        ops.EmailErrorLog();
                    }
                    if (args[0].Equals("CDC", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ops.runningArg = "CDC";
                        ops.WireUpRestart();
                        Console.WriteLine("Starting CDC");
                        //ops.NightlyDataSync();
                        accounts = new List<FrameworkSubGen.Entity.Account>();
                        ops.publications = new List<FrameworkSubGen.Entity.Publication>();
                        ops.importErrors = new List<FrameworkSubGen.Object.ImportError>();
                        ops.LoadAccountsFromDataBase();
                        ops.SubGenToUAD();
                        //always last step
                        ops.EmailErrorLog();
                    }
                    if (args[0].Equals("SubGenHourlySync", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ops.runningArg = "SubGenHourlySync";
                        ops.HourlyDataSync();
                    }
                    #endregion
                    #region Clear server tempfiles
                    if(RunWhat.Equals("CLEARTEMP"))
                    {
                        ops.ClearTempFiles();
                    }
                    #endregion
                    #region Stagnito File Transfer
                    if (RunWhat.Equals("SATGNTIOTRANS"))
                    {
                        ops.StagnitoFileTransfer();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Operations;
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, "Operations Unhandled error", app);
            }
        }
        #region Functions
        #region Engine shutdown notification
        #region notification if engine crashes/stops running
        void WireUpRestart()
        {
            var domain = AppDomain.CurrentDomain;
            domain.ProcessExit += new EventHandler(restart_CDC);
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
        }
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);
        // A delegate type to be used as the handler routine for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);
        // An enumerated type for the control messages sent to the handler routine.
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }
        private bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            // Put your own handler here
            MailMessage message = new MailMessage();
            message.To.Add(Core.ADMS.Settings.AdminEmail);
            string subject = "AMS_Operations - " + runningArg + " closed on server: " + ConfigurationManager.AppSettings["EngineServer"].ToString();

            message.Subject = subject;
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.Priority = MailPriority.High;
            message.Body = "AMS_Operations - " + runningArg + " closed unexpectedly on server: " + ConfigurationManager.AppSettings["EngineServer"].ToString() + ". Please check that it has restarted correctly.";

            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);

            smtp.Send(message);
            return true;
        }
        #endregion
        #endregion
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Operations;
            KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
            Exception ex = (Exception)e.ExceptionObject;
            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            Console.Write(formatException);
            alWorker.LogCriticalError(formatException, "Operations.UnhandledExceptionTrapper", app);
        }
        static void restart_CDC(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("AMS_Operations.exe CDC");
        }
        public void ClientArchive()
        {
            try
            {
                string[] fileEntries;
                string[] innerFolders;
                string[] filePath;
                //string[] oldFiles;

                DateTime time = DateTime.Now;

                string tempFolderExtention = @"\HistoricalBackupTemp";
                string format = "M-d-yyyy";
                string fileName;
                string newFile;
                string path = OperationsSettings.ClientArchivePath;
                string backupDate = (time.ToString(format));

                fileEntries = Directory.GetDirectories(path);

                foreach (string f in fileEntries)
                {
                    Directory.CreateDirectory(f + tempFolderExtention);
                    CopyFolder(f, f + tempFolderExtention, true);

                    filePath = f.Split('\\');
                    fileName = filePath[filePath.Length - 1];
                    newFile = f + "\\HistoricalBackups" + "\\" + fileName + backupDate + ".zip";
                    if (!File.Exists(newFile) && Directory.Exists(f + "\\HistoricalBackups"))
                    {
                        ZipFile.CreateFromDirectory(f + tempFolderExtention, newFile);
                        Directory.Delete(f + tempFolderExtention, true);

                        innerFolders = Directory.GetDirectories(f);
                        foreach (String fo in innerFolders)
                        {
                            if (!fo.Contains("Backup"))
                            {
                                var dir = new DirectoryInfo(fo);
                                FileInfo[] oldFile = dir.GetFiles().Where(x => x.CreationTime < DateTime.Now.AddDays(-14)).ToArray();                                
                                //oldFiles = Directory.GetFiles(fo);
                                foreach (FileInfo of in oldFile)
                                {
                                    bool isLocked = true;
                                    int tryCount = 0;
                                    do
                                    {
                                        try
                                        {
                                            File.Delete(of.FullName);
                                            isLocked = false;
                                        }
                                        catch (IOException)
                                        {
                                            tryCount++;
                                            Thread.Sleep(5000);
                                        }
                                    }
                                    while (isLocked && tryCount <= 10);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Operations;
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".MoveFiles", app, string.Empty);
            }
        }
        public void ProcessingStats()
        {
            //Go through each LIVE uad and insert days stats to FileProcessingStat table
            //after each client insert client stats to UAS FileProcessingStat table
            KMPlatform.BusinessLogic.Client cworker = new KMPlatform.BusinessLogic.Client();
            List<KMPlatform.Entity.Client> clients = cworker.Select().Where(x => x.IsActive == true).Where(x => x.IsAMS == true).ToList();
            FrameworkUAD.BusinessLogic.FileProcessingStat uadFpsWorker = new FrameworkUAD.BusinessLogic.FileProcessingStat();
            FrameworkUAS.BusinessLogic.FileProcessingStat uasFpsWorker = new FrameworkUAS.BusinessLogic.FileProcessingStat();
            int counter = 1;
            foreach (KMPlatform.Entity.Client c in clients)
            {
                Console.WriteLine("Processing client " + c.FtpFolder + " - " + counter.ToString() + " of " + clients.Count.ToString());
                DateTime runDate = DateTime.Now;
                uadFpsWorker.NightlyInsert(runDate, c.ClientConnections);
                FrameworkUAS.Entity.FileProcessingStat uasFPS = uadFpsWorker.GetFileProcessingStats(runDate, c.ClientID, c.ClientConnections);
                if (uasFPS != null && uasFPS.FileCount > 0)
                    uasFpsWorker.Save(uasFPS);
                counter++;
            }
        }
        public void SourceMediaPrinterFileExport()
        {
            try
            {
                //create data table
                DataTable dataTable = new DataTable();

                KMPlatform.BusinessLogic.Client cWorker = new KMPlatform.BusinessLogic.Client();
                KMPlatform.Entity.Client client = new KMPlatform.Entity.Client();
                client = cWorker.Select().Single(x => x.ClientID == 51);
                //testing

                var connString = client.ClientLiveDBConnectionString;

                string query = "exec ccp_SourceMedia_NightlyExport";

                SqlConnection conn = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                }
                catch
                {
                    var i = 1;
                }
                // create data adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // this will query your database and return the result to your datatable
                da.Fill(dataTable);
                conn.Close();
                da.Dispose();

                //foreach
                foreach (DataRow dr in dataTable.Rows)
                {
                    string sequence = dr["sequenceid"].ToString();

                    dr.SetField("keyline", KeyLineComputation.Compute(sequence));

                }

                //save file
                Core_AMS.Utilities.FileFunctions filefunctions = new Core_AMS.Utilities.FileFunctions();

                filefunctions.CreateCSVFromDataTable(dataTable, OperationsSettings.SourceMediaPrinterFileExportPath);
            }
            catch (Exception ex)
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Operations;
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".SourceMediaPrinterFileExport", app, string.Empty);
            }
        }
        public void FileTransfer(string sourcePath, string targetPath, string fileNameToMove = "all")
        {
            try
            {
                string fileName = string.Empty;
                string destFile = string.Empty;

                if (System.IO.Directory.Exists(sourcePath))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcePath);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        fileName = System.IO.Path.GetFileName(s);
                        Console.WriteLine(fileName);
                        if (fileName.ToLower() == fileNameToMove.ToLower() || fileNameToMove == "all")
                        {
                            destFile = System.IO.Path.Combine(targetPath, fileName);
                            System.IO.File.Copy(s, destFile, true);
                            DeleteFileCycle(s);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogFileException(ex, $".{nameof(FileTransfer)}");
            }
        }
        public void SAEFTPTransfer()
        {
            try
            {
                string fileName = string.Empty;
                var sourcePath = OperationsSettings.SAETransferSourcePath;
                var targetPath = OperationsSettings.SAETransferTargetPath;

                string destFile = string.Empty;

                if (System.IO.Directory.Exists(sourcePath))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcePath);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        fileName = System.IO.Path.GetFileName(s);
                        destFile = System.IO.Path.Combine(targetPath, fileName);
                        System.IO.File.Copy(s, destFile, true);
                        System.IO.File.Delete(s);
                    }
                }
            }
            catch (Exception ex)
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Operations;
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".SAEFTPTransfer", app, string.Empty);
            }
        }
        public void HWFTPTransfer()
        {
            try
            {
                string fileName = string.Empty;
                var sourcePath = OperationsSettings.HWPublishingTransferSourcePath;
                var targetPath = OperationsSettings.HWPublishingTransferTargetPath;

                string destFile = string.Empty;

                if (System.IO.Directory.Exists(sourcePath))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcePath);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        fileName = System.IO.Path.GetFileName(s);
                        Console.WriteLine(fileName);
                        if (fileName == "HW_UserData.csv")
                        {
                            destFile = System.IO.Path.Combine(targetPath, fileName);
                            System.IO.File.Copy(s, destFile, true);
                            System.IO.File.Delete(s);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Operations;
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".HWFTPTransfer", app, string.Empty);
            }
        }
        public void MoveFiles()
        {
            try
            {
                string[] fileEntries;
                string[] innerFolders;
                string[] oldFiles;

                Console.WriteLine("Starting copy");
                Operations ops = new Operations();
                //ops.CopyFolder(@"C:\\ADMS\Client Archive", @"C:\\ADMS\Client Archive2");
                ops.CopyFolder(OperationsSettings.MoveFileSourcePath, OperationsSettings.MoveFileTargetPath);

                fileEntries = Directory.GetDirectories(OperationsSettings.MoveFileSourcePath);

                foreach (string f in fileEntries)
                {
                    //Console.WriteLine(f + " Deleted");
                    innerFolders = Directory.GetDirectories(f);
                    foreach (String fo in innerFolders)
                    {
                        if (!fo.Contains("Backup"))
                        {
                            oldFiles = Directory.GetFiles(fo);
                            foreach (String of in oldFiles)
                            {
                                DeleteFileCycle(of);
                                Console.WriteLine(of + " Deleted");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var operationName = ".MoveFiles";
                LogFileException(ex, operationName);
            }
        }

        public static void LogFileException(Exception exception, string operationName)
        {
            var application = KMBusiness.Enums.Applications.AMS_Operations;
            var alWorker = new KMBusiness.ApplicationLog();
            var formatException = CoreStringFunctions.FormatException(exception);
            alWorker.LogCriticalError(
                formatException, 
                $"{typeof(Operations).Name}{operationName}", 
                application, 
                string.Empty);
        }

        public static void DeleteFileCycle(string fileName)
        {
            var isLocked = true;
            var tryCount = 0;
            do
            {
                try
                {
                    File.Delete(fileName);
                    isLocked = false;
                }
                catch (IOException)
                {
                    tryCount++;
                    Thread.Sleep(DeleteFileSleep);
                }
            }
            while (isLocked && tryCount <= DeleteFileAttempts);
        }

        public void Validate()
        {
            try
            {
                Console.WriteLine("Start");
                Operations ops = new Operations();
                //string path = @"C:\ADMS\ftp_repository";
                string path = OperationsSettings.ValidationRootPath;

                string[] fileEntries;

                KMPlatform.BusinessLogic.Client clientWorker = new KMPlatform.BusinessLogic.Client();
                List<KMPlatform.Entity.Client> clientList = new List<KMPlatform.Entity.Client>();
                Console.WriteLine("worker created");
                try
                {
                    clientList = clientWorker.Select(false).Where(x => x.IsAMS == true).ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Console.WriteLine("client list done");

                Console.WriteLine("pre get files");
                fileEntries = Directory.GetDirectories(path);
                Console.WriteLine("Files retrived");

                //KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
                foreach (KMPlatform.Entity.Client myClient in clientList)
                {
                    foreach (string f in fileEntries)
                    {
                        if (f.ToLower().Contains(myClient.FtpFolder.ToLower()))
                        {
                            Console.WriteLine("Start validate dir");
                            ops.ValidateDirectories(f, myClient);
                            //System.Threading.Thread t = new System.Threading.Thread(new ThreadStart(() => ops.ValidateDirectories(f, myClient)));
                            //t.Start();
                        }
                    }
                    //new thread

                }

            }
            catch (Exception ex)
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.AMS_Operations;
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".MoveFiles", app, string.Empty);
            }
        }
        public void ClearTempFiles()
        {
            string[] folders;
            string[] iFolders;
            //string path = @"C:\Users\micah.matheson\AppData\Local\Temp";
            string path = OperationsSettings.ClearTempPath;

            folders = Directory.GetDirectories(path);
            foreach (String fo in folders)
            {
                DateTime updateTime = new DateTime();
                updateTime = Directory.GetLastWriteTime(fo);
                if (updateTime < DateTime.Now.AddDays(-2) && fo.Contains(@"TMP_Z"))
                {
                    Directory.Delete(fo,true);
                }
                else if(fo.EndsWith("2"))
                {
                    iFolders = Directory.GetDirectories(fo);
                    foreach (String f in iFolders)
                    {
                        DateTime updatedTime = new DateTime();
                        updateTime = Directory.GetLastWriteTime(f);
                        if (updatedTime < DateTime.Now.AddDays(-2) && f.Contains(@"TMP_Z"))
                        {
                            Directory.Delete(f,true);
                        }
                    }
                }
            }

        }

        public void LogError(Exception ex, string method)
        {
            LogHelper.LogError(ex, method, GetType().ToString());
        }

        /// <summary>
        /// for every client for every MasterGroup create a MasterCodeSheet value of xxNVxx which will be used as the answer for any missing required Custom Field
        /// for every client for every ResponseGroup create a CodeSheet value of xxNVxx which will be used as the answer for any missing required Custom Field
        /// </summary>
        public void NoValueSetup()
        {
            KMPlatform.BusinessLogic.Client cWrk = new KMPlatform.BusinessLogic.Client();
            List<KMPlatform.Entity.Client> clients = cWrk.Select().Where(x => x.IsAMS == true).ToList();
            foreach (var c in clients)
            {
                try
                {
                    FrameworkUAD.BusinessLogic.CodeSheet csWrk = new FrameworkUAD.BusinessLogic.CodeSheet();
                    csWrk.CreateNoValueRespones(c.ClientConnections);
                    FrameworkUAD.BusinessLogic.MasterCodeSheet msWrk = new FrameworkUAD.BusinessLogic.MasterCodeSheet();
                    msWrk.CreateNoValueRespones(c.ClientConnections);
                }
                catch { }
            }
        }
        public void StagnitoFileTransfer()
        {
            string[] files = GetFileList();
            foreach (string file in files)
            {
                Download(file);
                DeleteFile(file);
            }
        }
  
        #endregion
        #region Helper Methods
        public void CopyFolder(string sourceFolder, string destFolder, bool moveFilesWithinDateAndNow = false)
        {
            try
            {
                if (!Directory.Exists(destFolder))
                    Directory.CreateDirectory(destFolder);

                //string[] files = Directory.GetFiles(sourceFolder);
                var dir = new DirectoryInfo(sourceFolder);
                FileInfo[] files = dir.GetFiles();
                if (moveFilesWithinDateAndNow)
                    files = files.Where(x => x.CreationTime < DateTime.Now.AddDays(-14)).ToArray();

                foreach (FileInfo file in files)
                {
                    string name = Path.GetFileName(file.FullName);
                    string dest = Path.Combine(destFolder, name);
                    File.Copy(file.FullName, dest, true);
                }
                string[] folders = Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = Path.GetFileName(folder);
                    if (!folder.Contains("Backup"))
                    {
                        string dest = Path.Combine(destFolder, name);
                        CopyFolder(folder, dest, moveFilesWithinDateAndNow);
                        Console.WriteLine(folder + " copied");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public void ValidateDirectories(string f, KMPlatform.Entity.Client client)
        {
            string[] folders;
            string[] innerFolders;
            string[] files;

            folders = Directory.GetDirectories(f);

            foreach (String fo in folders)
            {
                Console.WriteLine(fo);
                if (fo.Contains("ADMS"))
                {

                    innerFolders = Directory.GetDirectories(fo);
                    if (!innerFolders.Contains(fo + @"\FileValidator"))
                    {
                        Directory.CreateDirectory(fo + @"\FileValidator");
                    }
                    foreach (string inF in innerFolders)
                    {
                        Console.WriteLine(inF);
                        if (inF.Contains("FileValidator"))
                        {
                            files = Directory.GetFiles(inF);
                            foreach (String file in files)
                            {
                                Console.WriteLine(file);
                                // new thread
                                System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
                                FrameworkUAS.BusinessLogic.SourceFile sfWorker = new FrameworkUAS.BusinessLogic.SourceFile();
                                Console.WriteLine("source file worker created");
                                List<FrameworkUAS.Entity.SourceFile> sourceFileList = sfWorker.Select(false);

                                Console.WriteLine("source file list created");
                                string fileName = fileInfo.Name.Replace(fileInfo.Extension, "").ToLower();

                                FrameworkUAS.Entity.SourceFile sourceFile = null;

                                if (sourceFileList.Exists(x => fileName.StartsWith(x.FileName.ToLower()) && x.IsDeleted == false && x.ClientID == client.ClientID))
                                {
                                    sourceFile = sourceFileList.FirstOrDefault(x => fileName.StartsWith(x.FileName.ToLower()) && x.IsDeleted == false && x.ClientID == client.ClientID);
                                }

                                FileValidator fv = new FileValidator();
                                if (sourceFile != null)
                                {
                                    Console.WriteLine("Validate");

                                    fv.ValidateFileAsObject(fileInfo, client, sourceFile);
                                    Console.WriteLine("Validate done");
                                    bool islocked = true;
                                    do
                                    {
                                        try
                                        {
                                            File.Delete(file);
                                            islocked = false;
                                        }
                                        catch (IOException)
                                        {
                                            Thread.Sleep(5000);
                                        }
                                    }
                                    while (islocked);

                                }
                            }
                        }
                    }
                }
            }
        }
        public string[] GetFileList()
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(OperationsSettings.StagnitoFTPPath));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(OperationsSettings.StagnitoFTPUser, OperationsSettings.StagnitoFTPPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = null;
                reqFTP.KeepAlive = false;
                reqFTP.UsePassive = false;
                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                downloadFiles = null;
                return downloadFiles;
            }
        }
        private void Download(string file)
        {
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri($"{OperationsSettings.StagnitoFTPPath}{file}"));
                reqFTP.Credentials = new NetworkCredential(OperationsSettings.StagnitoFTPUser, OperationsSettings.StagnitoFTPPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;
                reqFTP.UsePassive = false;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream responseStream = response.GetResponseStream();
                var writeStream = new FileStream($"{OperationsSettings.StagnitoDownloadTargetPath}{file}", FileMode.Create);
                int Length = 2048;
                Byte[] buffer = new Byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);
                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                }
                writeStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                LogError(ex, ex.Message);
            }
        }
        private string DeleteFile(string fileName)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri($"{OperationsSettings.StagnitoFTPPath}{fileName}"));
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(OperationsSettings.StagnitoFTPUser, OperationsSettings.StagnitoFTPPassword);
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response.StatusDescription;
            }
        }
            #endregion

        #region SubGen Integration

        private void HourlyDataSync()
        {
            accounts = new List<Account>();
            LoadAccountsFromDataBase();

            foreach (var account in accounts.Where(x => x.KMClientId > 0))
            {
                var sgClient = Enums.GetClient(account.company_name.Trim());
                var subGenSync = new SubGenSynchronization();
                subGenSync.HourlySyncSubGenToKM(sgClient, account);
                subGenSync.HourlySyncKMToSubGen(account, sgClient);
            }
        }

        void LoadAccountsFromDataBase()
        {
            try
            {
                accounts = new FrameworkSubGen.BusinessLogic.Account().Select().Where(x => x.active).ToList();
            }
            catch (Exception ex)
            {
                var msg = StringFunctions.FormatException(ex);
                var alWrk = new KMPlatform.BusinessLogic.ApplicationLog();
                alWrk.LogCriticalError(
                    msg,
                    "AMS_Operations.GetAccountsDataBase",
                    KMPlatform.BusinessLogic.Enums.Applications.AMS_Operations,
                    "SubGen Integration");
            }
        }
        #region Import files from SubGen to UAD
        void SubGenOrderFileImport(string outPut)
        {
            //SubGen.BusinessLogic.Account aWorker = new SubGen.BusinessLogic.Account();
            //List<SubGen.Entity.Account> accounts = aWorker.Select();

            FrameworkSubGen.BusinessLogic.Publication pubWorker = new FrameworkSubGen.BusinessLogic.Publication();
            publications = pubWorker.Select();

            string directory = ConfigurationManager.AppSettings["SubGenOrderFileImportFolder"].ToString();
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            FileInfo[] files = dirInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                var fileConfig = new FileConfiguration();
                fileConfig.IsQuoteEncapsulated = true;
                fileConfig.FileColumnDelimiter = "comma";
                FrameworkUAD.BusinessLogic.ImportFile import = new FrameworkUAD.BusinessLogic.ImportFile();
                FrameworkUAD.Object.ImportFile fileData = import.GetImportFile(file, fileConfig);

                //convert to List of OrderFile
                //remove spaces from Headers
                foreach (var columnName in fileData.HeadersOriginal.Keys)
                {
                    fileData.HeadersTransformed.Add(columnName.ToString().Replace(" ", ""), fileData.HeadersOriginal[columnName.ToString()].ToString());
                }
                //"Order ID","Order Item ID",SubscriberID,"Subscriber First Name","Subscriber Last Name","Shipping First Name","Shipping Last Name","Shipping Address Line 1","Shipping City","Shipping State","Shipping Postal Code","Shipping Country","Billing First Name","Billing Last Name","Billing Address Line 1","Billing City","Billing State","Billing Postal Code","Billing Country","Created by Rep","Order Date","Product Name","Publication Name","Publication Product Code","Publication Revenue Code","Subscription Offer Name","Starter Issues","Total Issues",Copies,Quantity,"Parent Order Item ID","Fulfilled Date","Refunded Date","Sub Total","Tax Total","Grand Total","Product ID"
                foreach (var key in fileData.DataOriginal.Keys)
                {
                    StringDictionary myRow = fileData.DataOriginal[key];//column name/data
                    StringDictionary tranRow = new StringDictionary();
                    foreach (var c in myRow.Keys)
                    {
                        tranRow.Add(c.ToString().Replace(" ", ""), myRow[c.ToString()].ToString());
                    }
                    fileData.DataTransformed.Add(key, tranRow);
                }
                //drop original data to lighten the load
                fileData.HeadersOriginal = new StringDictionary();
                fileData.DataOriginal = new Dictionary<int, StringDictionary>();
                //convert to a List of ImportOrder for insert/update
                List<FrameworkSubGen.Entity.ImportOrder> ioList = new List<FrameworkSubGen.Entity.ImportOrder>();
                foreach (var rowNumber in fileData.DataTransformed.Keys)
                {
                    StringDictionary myRow = fileData.DataTransformed[rowNumber];//column name/data
                    FrameworkSubGen.Entity.ImportOrder io = new FrameworkSubGen.Entity.ImportOrder();
                    foreach (var columnName in fileData.HeadersTransformed.Keys)
                    {
                        #region set properties
                        switch (columnName.ToString())
                        {
                            case "orderid":
                                int orderId = 0;
                                int.TryParse(myRow[columnName.ToString()].ToString(), out orderId);
                                io.OrderID = orderId;
                                break;
                            case "orderitemid":
                                int oiId = 0;
                                int.TryParse(myRow[columnName.ToString()].ToString(), out oiId);
                                io.OrderItemID = oiId;
                                break;
                            case "subscriberid":
                                int sid = 0;
                                int.TryParse(myRow[columnName.ToString()].ToString(), out sid);
                                io.SubscriberID = sid;
                                break;
                            case "subscriberfirstname":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "subscriberlastname":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "shippingfirstname":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "shippinglastname":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "shippingaddressline1":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "shippingcity":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "shippingstate":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "shippingpostalcode":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "shippingcountry":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "billingfirstname":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "billinglastname":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "billingaddressline1":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "billingcity":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "billingstate":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "billingpostalcode":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "billingcountry":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "createdbyrep":
                                io.SubscriberFirstName = myRow[columnName.ToString()].ToString();
                                break;
                            case "orderdate":
                                DateTime oDate;
                                DateTime.TryParse(myRow[columnName.ToString()].ToString(), out oDate);
                                if (oDate != null)
                                    io.OrderDate = oDate;
                                else
                                    io.OrderDate = null;
                                break;
                            case "productname":
                                io.ProductName = myRow[columnName.ToString()].ToString();
                                break;
                            case "publicationname":
                                io.PublicationName = myRow[columnName.ToString()].ToString();
                                break;
                            case "publicationproductcode":
                                int ppc = 0;
                                int.TryParse(myRow[columnName.ToString()].ToString(), out ppc);
                                io.PublicationProductCode = ppc;
                                break;
                            case "publicationrevenuecode":
                                int prc = 0;
                                int.TryParse(myRow[columnName.ToString()].ToString(), out prc);
                                io.PublicationRevenueCode = prc;
                                break;
                            case "subscriptionoffername":
                                io.SubscriptionOfferName = myRow[columnName.ToString()].ToString();
                                break;
                            case "starterissues":
                                bool si = false;
                                bool.TryParse(myRow[columnName.ToString()].ToString(), out si);
                                io.StarterIssues = si;
                                break;
                            case "totalissues":
                                int ti = 0;
                                int.TryParse(myRow[columnName.ToString()].ToString(), out ti);
                                io.TotalIssues = ti;
                                break;
                            case "copies":
                                int c = 0;
                                int.TryParse(myRow[columnName.ToString()].ToString(), out c);
                                io.Copies = c;
                                break;
                            case "quantity":
                                int q = 0;
                                int.TryParse(myRow[columnName.ToString()].ToString(), out q);
                                io.Quantity = q;
                                break;
                            case "parentorderitemid":
                                int poid = 0;
                                int.TryParse(myRow[columnName.ToString()].ToString(), out poid);
                                io.ParentOrderItemID = poid;
                                break;
                            case "fulfilleddate":
                                DateTime fDate;
                                DateTime.TryParse(myRow[columnName.ToString()].ToString(), out fDate);
                                if (fDate != null)
                                    io.FulfilledDate = fDate;
                                else
                                    io.FulfilledDate = null;
                                break;
                            case "refundeddate":
                                DateTime rDate;
                                DateTime.TryParse(myRow[columnName.ToString()].ToString(), out rDate);
                                if (rDate != null)
                                    io.RefundedDate = rDate;
                                else
                                    io.RefundedDate = null;
                                break;
                            case "subtotal":
                                double st = 0;
                                double.TryParse(myRow[columnName.ToString()].ToString(), out st);
                                io.SubTotal = st;
                                break;
                            case "taxtotal":
                                double tt = 0;
                                double.TryParse(myRow[columnName.ToString()].ToString(), out tt);
                                io.TaxTotal = tt;
                                break;
                            case "grandtotal":
                                double gt = 0;
                                double.TryParse(myRow[columnName.ToString()].ToString(), out gt);
                                io.GrandTotal = gt;
                                break;
                            case "productid":
                                int pid = 0;
                                int.TryParse(myRow[columnName.ToString()].ToString(), out pid);
                                io.ProductID = pid;
                                break;
                        }
                        #endregion
                    }
                    if (publications.Exists(x => x.publication_id == io.ProductID))
                        io.account_id = publications.Single(x => x.publication_id == io.ProductID).account_id;
                    ioList.Add(io);
                }

                FrameworkSubGen.BusinessLogic.ImportOrder ioWorker = new FrameworkSubGen.BusinessLogic.ImportOrder();
                ioWorker.SaveBulkXml(ioList);
            }
        }
        void SubGenSubscriberFileImport(string outPut)
        {
            FrameworkSubGen.BusinessLogic.Publication pubWorker = new FrameworkSubGen.BusinessLogic.Publication();
            publications = pubWorker.Select();

            string directory = ConfigurationManager.AppSettings["SubGenSubscriberFileImportFolder"].ToString();
            if (!Directory.Exists(directory + "Done\\"))
                Directory.CreateDirectory(directory + "Done\\");

            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            FileInfo[] files = dirInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                try
                {
                    ConsoleMessage("File: " + file.Name);
                    var fileConfig = new FileConfiguration();
                    fileConfig.IsQuoteEncapsulated = true;
                    fileConfig.FileColumnDelimiter = "comma";
                    FrameworkUAD.BusinessLogic.ImportFile import = new FrameworkUAD.BusinessLogic.ImportFile();
                    FrameworkUAD.Object.ImportFile fileData = import.GetImportFile(file, fileConfig);
                    if (fileData.HasError == true)
                    {
                        foreach (var e in fileData.ImportErrors)
                            LogImportError("SubGenSubscriberFileImport", e.FormattedException, null, file, null, null);
                    }
                    else
                    {
                        #region File Parsing
                        //remove spaces from Headers
                        foreach (var columnName in fileData.HeadersOriginal.Keys)
                        {
                            try
                            {
                                string col = columnName.ToString().Replace(" ", "").Replace("/", "_");
                                fileData.HeadersTransformed.Add(col, fileData.HeadersOriginal[columnName.ToString()].ToString());
                            }
                            catch (Exception ex)
                            {
                                string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                LogImportError("SubGenSubscriberFileImport", Core_AMS.Utilities.StringFunctions.FormatException(ex));
                            }
                        }
                        ConsoleMessage("Headers done");
                        foreach (var key in fileData.DataOriginal.Keys)
                        {
                            try
                            {
                                StringDictionary myRow = fileData.DataOriginal[key];//column name/data
                                StringDictionary tranRow = new StringDictionary();
                                foreach (var c in myRow.Keys)
                                {
                                    string col = c.ToString().Replace(" ", "").Replace("/", "_");
                                    tranRow.Add(col, myRow[c.ToString()].ToString());
                                }
                                fileData.DataTransformed.Add(key, tranRow);
                            }
                            catch (Exception ex)
                            {
                                LogImportError("SubGenSubscriberFileImport", Core_AMS.Utilities.StringFunctions.FormatException(ex));
                            }
                        }
                        ConsoleMessage("Transformed Headers done");
                        #endregion
                        #region Object.ImportFile to SubGen.Entity.ImportSubscriber
                        //list of columns in app.config
                        List<string> columns = ConfigurationManager.AppSettings["SubGenSubscriberFileColumns"].ToString().Split(',').ToList();

                        //drop original data to lighten the load
                        fileData.HeadersOriginal = new StringDictionary();
                        fileData.DataOriginal = new Dictionary<int, StringDictionary>();
                        //convert to a List of ImportSubscriber for insert/update
                        List<FrameworkSubGen.Entity.ImportSubscriber> isList = new List<FrameworkSubGen.Entity.ImportSubscriber>();
                        foreach (var rowNumber in fileData.DataTransformed.Keys)
                        {
                            StringDictionary myRow = null;
                            try
                            {
                                ConsoleMessage("Process file row " + rowNumber.ToString() + " of " + fileData.DataTransformed.Keys.Count.ToString());
                                FrameworkSubGen.Entity.ImportDimension dim = new FrameworkSubGen.Entity.ImportDimension();

                                myRow = fileData.DataTransformed[rowNumber];//column name/data
                                FrameworkSubGen.Entity.ImportSubscriber iSub = new FrameworkSubGen.Entity.ImportSubscriber();
                                foreach (var columnName in fileData.HeadersTransformed.Keys)
                                {
                                    if (columns.Exists(x => x.Equals(columnName.ToString(), StringComparison.CurrentCultureIgnoreCase)))
                                    {
                                        #region set properties
                                        switch (columnName.ToString())
                                        {
                                            case "systemsubscriberid":
                                                int ssid = 0;
                                                int.TryParse(myRow[columnName.ToString()].ToString(), out ssid);
                                                iSub.SystemSubscriberID = ssid;
                                                break;
                                            case "islead":
                                                bool lead = false;
                                                bool.TryParse(myRow[columnName.ToString()].ToString(), out lead);
                                                iSub.IsLead = lead;
                                                break;
                                            case "renewalcode_customerid":
                                                iSub.RenewalCode_CustomerID = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "subscriberaccountfirstname":
                                                iSub.SubscriberAccountFirstName = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "subscriberaccountlastname":
                                                iSub.SubscriberAccountLastName = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "subscriberemail":
                                                iSub.SubscriberEmail = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "subscriberphone":
                                                iSub.SubscriberPhone = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "subscribersource":
                                                iSub.SubscriberSource = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "subscriptiongeniusmailingaddressid":
                                                int maid = 0;
                                                int.TryParse(myRow[columnName.ToString()].ToString(), out maid);
                                                iSub.SubscriptionGeniusMailingAddressID = maid;
                                                break;
                                            case "mailingaddressfirstname":
                                                iSub.MailingAddressFirstName = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "mailingaddresslastname":
                                                iSub.MailingAddressLastName = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "mailingaddresstitle":
                                                iSub.MailingAddressTitle = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "mailingaddressline1":
                                                iSub.MailingAddressLine1 = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "mailingaddressline2":
                                                iSub.MailingAddressLine2 = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "mailingaddresscity":
                                                iSub.MailingAddressCity = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "mailingaddressstate":
                                                iSub.MailingAddressState = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "mailingaddresszip":
                                                iSub.MailingAddressZip = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "mailingaddresscompany":
                                                iSub.MailingAddressCompany = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "mailingaddresscountry":
                                                iSub.MailingAddressCountry = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "systembillingaddressid":
                                                int baid = 0;
                                                int.TryParse(myRow[columnName.ToString()].ToString(), out baid);
                                                iSub.SystemBillingAddressID = baid;
                                                break;
                                            case "billingaddressfirstname":
                                                iSub.BillingAddressFirstName = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "billingaddresslastname":
                                                iSub.BillingAddressLastName = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "publicationname":
                                                iSub.PublicationName = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "billingaddressline1":
                                                iSub.BillingAddressLine1 = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "billingaddressline2":
                                                iSub.BillingAddressLine2 = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "billingaddresscity":
                                                iSub.BillingAddressCity = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "billingaddressstate":
                                                iSub.BillingAddressState = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "billingaddresszip":
                                                iSub.BillingAddressZip = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "billingaddresscompany":
                                                iSub.BillingAddressCompany = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "billingaddresscountry":
                                                iSub.BillingAddressCountry = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "issuesleft":
                                                int left = 0;
                                                int.TryParse(myRow[columnName.ToString()].ToString(), out left);
                                                iSub.IssuesLeft = left;
                                                break;
                                            case "unearnedrevenue":
                                                double unRev = 0;
                                                double.TryParse(myRow[columnName.ToString()].ToString(), out unRev);
                                                iSub.UnearnedRevenue = unRev;
                                                break;
                                            case "copies_seats":
                                                int copy = 0;
                                                int.TryParse(myRow[columnName.ToString()].ToString(), out copy);
                                                iSub.Copies = copy;
                                                break;
                                            case "subscriptionid":
                                                int subid = 0;
                                                int.TryParse(myRow[columnName.ToString()].ToString(), out subid);
                                                iSub.SubscriptionID = subid;
                                                break;
                                            case "parentsubscriptionid":
                                                int psubid = 0;
                                                int.TryParse(myRow[columnName.ToString()].ToString(), out subid);
                                                iSub.ParentSubscriptionID = psubid;
                                                break;
                                            case "issitelicensemaster":
                                                bool isMaster = false;
                                                bool.TryParse(myRow[columnName.ToString()].ToString(), out isMaster);
                                                iSub.IsSiteLicenseMaster = isMaster;
                                                break;
                                            case "issitelicenseseat":
                                                bool isSeat = false;
                                                bool.TryParse(myRow[columnName.ToString()].ToString(), out isSeat);
                                                iSub.IsSiteLicenseSeat = isSeat;
                                                break;
                                            case "subscriptioncreateddate":
                                                DateTime scdDate;
                                                DateTime.TryParse(myRow[columnName.ToString()].ToString(), out scdDate);
                                                if (scdDate != null)
                                                    iSub.SubscriptionCreatedDate = scdDate;
                                                else
                                                    iSub.SubscriptionCreatedDate = DateTime.Now;
                                                break;
                                            case "subscriptionrenewdate":
                                                DateTime srdDate;
                                                DateTime.TryParse(myRow[columnName.ToString()].ToString(), out srdDate);
                                                if (srdDate != null)
                                                    iSub.SubscriptionRenewDate = srdDate;
                                                else
                                                    iSub.SubscriptionRenewDate = DateTime.Now;
                                                break;
                                            case "subscriptionexpiredate":
                                                DateTime sedDate;
                                                DateTime.TryParse(myRow[columnName.ToString()].ToString(), out sedDate);
                                                if (sedDate != null)
                                                    iSub.SubscriptionExpireDate = sedDate;
                                                else
                                                    iSub.SubscriptionExpireDate = DateTime.Now;
                                                break;
                                            case "subscriptionlastqualifieddate":
                                                DateTime slqddDate;
                                                DateTime.TryParse(myRow[columnName.ToString()].ToString(), out slqddDate);
                                                if (slqddDate != null)
                                                    iSub.SubscriptionLastQualifiedDate = slqddDate;
                                                else
                                                    iSub.SubscriptionLastQualifiedDate = DateTime.Now;
                                                break;
                                            case "subscriptiontype":
                                                iSub.SubscriptionType = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "auditcategoryname":
                                                iSub.AuditCategoryName = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "auditcategorycode":
                                                iSub.AuditCategoryCode = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "auditrequesttypename":
                                                iSub.AuditRequestTypeName = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "auditrequesttypecode":
                                                iSub.AuditRequestTypeCode = myRow[columnName.ToString()].ToString();
                                                break;
                                            case "transactionid":
                                                int tid = 0;
                                                int.TryParse(myRow[columnName.ToString()].ToString(), out tid);
                                                iSub.TransactionID = tid;
                                                break;
                                            
                                            case "publicationid":
                                                int pid = 0;
                                                int.TryParse(myRow[columnName.ToString()].ToString(), out pid);
                                                string pubName = myRow["publicationname"].ToString();
                                                if (pid == 0 && !string.IsNullOrEmpty(myRow["publicationname"].ToString()))
                                                {
                                                    if (publications.Exists(x => x.name.Trim().Equals(myRow["publicationname"].ToString().Trim(), StringComparison.CurrentCultureIgnoreCase)))
                                                        pid = publications.Single(x => x.name.Trim().Equals(myRow["publicationname"].ToString().Trim(), StringComparison.CurrentCultureIgnoreCase)).publication_id;
                                                }
                                                iSub.PublicationID = pid;
                                                break;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        string dimValue = myRow[columnName.ToString()].ToString();
                                        if (!string.IsNullOrEmpty(dimValue))
                                        {
                                            dim.Dimensions.Add(new FrameworkSubGen.Entity.ImportDimensionDetail(0, columnName.ToString(), dimValue, iSub.SystemSubscriberID, iSub.SubscriptionID, iSub.PublicationID));
                                        }
                                    }
                                }

                                if (publications.Exists(x => x.publication_id == iSub.PublicationID))
                                {
                                    dim.account_id = publications.Single(x => x.publication_id == iSub.PublicationID).account_id;
                                    iSub.account_id = publications.Single(x => x.publication_id == iSub.PublicationID).account_id;
                                }

                                dim.PublicationID = iSub.PublicationID;
                                dim.SubscriptionID = iSub.SubscriptionID;
                                dim.SystemSubscriberID = iSub.SystemSubscriberID;
                                foreach (FrameworkSubGen.Entity.ImportDimensionDetail idd in dim.Dimensions)
                                {
                                    idd.SubscriptionID = iSub.SubscriptionID;
                                    idd.SystemSubscriberID = iSub.SystemSubscriberID;
                                    idd.PublicationID = iSub.PublicationID;
                                }
                                iSub.Dimensions = dim;
                                isList.Add(iSub);
                            }
                            catch (Exception ex)
                            {
                                LogImportError("SubGenSubscriberFileImport", Core_AMS.Utilities.StringFunctions.FormatException(ex), myRow);
                            }
                        }
                        #endregion
                        #region run DQM or create files
                        if (isList.Count > 0)
                        {
                            //if TransactionID has not been set lets set based on business rule
                            //need to decide TransactionID based on a set of Busniess Rules - see AMS: Transaction Types sheet
                            //look at file field "Subscription Expire Date"
                            //if date > current date then it is an active so set to 40
                            //if date <= current date then it is an InActive so set to 60
                            foreach (FrameworkSubGen.Entity.ImportSubscriber iSub in isList)
                            {
                                if (iSub.TransactionID == 0)
                                {
                                    if (iSub.SubscriptionExpireDate.Date > DateTime.Now.Date)
                                        iSub.TransactionID = 40;
                                    else
                                        iSub.TransactionID = 60;
                                }
                            }

                            FrameworkSubGen.BusinessLogic.ImportSubscriber isWorker = new FrameworkSubGen.BusinessLogic.ImportSubscriber();
                            ConsoleMessage("Start - insert to ImportSubscriber/ImportDimension/ImportDimensionDetail");
                            isWorker.JobImportSubscriberFile(isList);//this will put data in ImportSubscriber/ImportDimension/ImportDimensionDetail
                            isList = isWorker.RemoveXmlFormatting(isList);
                            ConsoleMessage("Done - insert to ImportSubscriber/ImportDimension/ImportDimensionDetail");
                            int accountId = isList.First().account_id;
                            if (accountId == 0)
                            {
                                //lets grab a distinct list of PubCodes then loop through until we can get a match for accountId
                                List<int> pubIds = isList.Select(x => x.PublicationID).Distinct().ToList();
                                foreach (int pubId in pubIds)
                                {
                                    if (publications.Exists(x => x.publication_id == pubId))
                                    {
                                        accountId = publications.Single(x => x.publication_id == pubId).account_id;
                                        break;
                                    }
                                }
                            }

                            if (accountId > 0)
                            {
                                FrameworkSubGen.Entity.Account account = accounts.Single(x => x.account_id == accountId);
                                if (outPut.Equals("File", StringComparison.CurrentCultureIgnoreCase))
                                    CreateFile(account, file.Name.Replace(file.Extension, ""), isList);
                                else if (outPut.Equals("DQM", StringComparison.CurrentCultureIgnoreCase))
                                    ADMS_DQM(account, isList);

                                ConsoleMessage("Move file to Done");
                                if (File.Exists(directory + "Done\\" + file.Name))
                                    File.Delete(directory + "Done\\" + file.Name);
                                File.Move(file.FullName, directory + "Done\\" + file.Name);
                                ConsoleMessage("File - " + file.Name + " done");
                            }
                            else
                                LogImportError("SubGenSubscriberFileImport", "Not able to determine SubGen AccountId");
                        }
                        else
                            LogImportError("SubGenSubscriberFileImport", "SubGen file had no records");
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    LogImportError("SubGenSubscriberFileImport", Core_AMS.Utilities.StringFunctions.FormatException(ex), null, file);
                }
            }
        }
        #endregion

        #region Final Steps - Create File or run ADMS.DQM

        private void CreateFile(Account account, string fileName, List<ImportSubscriber> isList)
        {
            //step 1 get all SubGenData.ImportSubscriber/SubGenData.ImportDimensionDetail data where IsMergedToUAD = false
            var isWorker = new FrameworkSubGen.BusinessLogic.ImportSubscriber();
            if (isList == null || isList.Count == 0)
            {
                return;
            }

            try
            {
                ConsoleMessage("Start - Convert_ImportSubscriber_to_SubscriberTrans");
                var converter = new ImportSubscriberConverter(ConsoleMessage, LogImportError, publications);
                var stList = converter.Convert_ImportSubscriber_to_SubscriberTrans(isList, account);
                ConsoleMessage("Done - Convert_ImportSubscriber_to_SubscriberTrans");
                var writeFileWorker = new FileWriter(ConsoleMessage, LogImportError);

                var dtValid = writeFileWorker.CreateSaveFileDataTable(stList);
                var dtBad = dtValid.Clone();

                var cWorker = new KMPlatform.BusinessLogic.Client();
                var client = cWorker.Select(account.KMClientId);
                var rgWrk = new FrameworkUAD.BusinessLogic.ResponseGroup();
                var rgList = rgWrk.Select(client.ClientConnections);
                var rowCounter = 1;
                foreach (var st in stList)
                {
                    rowCounter = writeFileWorker.CreateSaveFileDataTableRow(rowCounter, stList, st, rgList, dtValid, dtBad);
                }

                dtValid.AcceptChanges();

                writeFileWorker.WriteSaveFileDataTable(fileName, isList, dtValid, dtBad, isWorker);
            }
            catch (Exception ex)
            {
                var msg = StringFunctions.FormatException(ex);
                LogImportError("CreateFile", msg, null, null, null, null);
            }
        }

        void ADMS_DQM(FrameworkSubGen.Entity.Account account, List<FrameworkSubGen.Entity.ImportSubscriber> isList)
        {
            //step 1 transform to FrameworkUAD.Entity.SubscriberTransformed/SubscriberDemographicTransformed
            ConsoleMessage("Start - Convert_ImportSubscriber_to_SubscriberTrans");
            var importSubscriberConverter = new ImportSubscriberConverter(ConsoleMessage, LogImportError, publications);
            List<SubscriberTransformed> stList = importSubscriberConverter.Convert_ImportSubscriber_to_SubscriberTrans(isList, account);
            ConsoleMessage("Done - Convert_ImportSubscriber_to_SubscriberTrans");

            //step 2 call FrameworkUAD.BusinessLogic.SaveBulkSqlInsert
            FrameworkUAD.BusinessLogic.SubscriberTransformed stWorker = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
            KMPlatform.BusinessLogic.Client cWorker = new KMPlatform.BusinessLogic.Client();
            KMPlatform.Entity.Client client = cWorker.Select(account.KMClientId);
            if (stList.Count > 0)
            {
                stWorker.SaveBulkSqlInsert(stList, client.ClientConnections, false);
                string processCode = stList.FirstOrDefault().ProcessCode;
                //step 3 call FrameworkUAS.BusinessLogic.DQMQue.Save 
                #region Add to DQM Batch
                bool isDemo = true;
                if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("IsDemo"))
                    bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IsDemo"].ToString(), out isDemo);
                if (stList.First().SourceFileID > 0)
                {
                    FrameworkUAS.Entity.DQMQue q = new FrameworkUAS.Entity.DQMQue(processCode, client.ClientID, isDemo, false, stList.First().SourceFileID);//need a dummy SourceFile 'Paid_CDC_Import'
                    FrameworkUAS.BusinessLogic.DQMQue dqmWorker = new FrameworkUAS.BusinessLogic.DQMQue();
                    dqmWorker.Save(q);

                    //step 4 foreach ImportSubscriber.SubscriptionID set IsMergedToUAD = true, DateMergedToUAD DateTime.Now (send xml list of id's)
                    FrameworkSubGen.BusinessLogic.ImportSubscriber isWorker = new FrameworkSubGen.BusinessLogic.ImportSubscriber();
                    isWorker.UpdateMergedToUAD(isList);
                }
            }
                #endregion
        }
        #endregion
        #region Email errors
        private void LogImportError(string method, string errorMsg, StringDictionary myRow = null, FileInfo file = null, ImportSubscriber iSub = null, SubscriberTransformed st = null)
        {
            importErrors = importErrors ?? new List<ImportError>();

            var importError = new ImportError
            {
                DataRow = myRow,
                ErrorMsg = errorMsg,
                ImportFile = file,
                Method = method,
                SubGenImportSubscriber = iSub,
                UADSubscriberTransformed = st
            };

            importErrors.Add(importError);
        }

        public void EmailErrorLog()
        {
            Guard.NotNull(importErrors, nameof(importErrors));

            var ieErrors = importErrors
                .Distinct()
                .Select(e => e.ErrorMsg)
                .Distinct()
                .ToList();
            
            var sbMessageBody = new StringBuilder();
            sbMessageBody.AppendLine($"Total Distinct Error count: {ieErrors.Count}");
            foreach (var ie in ieErrors)
            {
                sbMessageBody.AppendLine(ie);
            }

            const string fileImportDirConfigKey = "SubGenSubscriberFileImportFolder";
            var fileImportDir = ConfigurationManager.AppSettings[fileImportDirConfigKey];

            const string errorDirName = "ErrorFile";
            Directory.CreateDirectory(fileImportDir + errorDirName);

            const string timeFormat = "MMddyyyy_HH-mm-ss";
            var strTime = DateTime.Now.ToString(timeFormat);

            var message = new MailMessage();

            // Add attachment for import subscriber
            const string importSubscriberFileName = "_ImportSubscriber_BadRecords.csv";
            var importFullFileName = Path.Combine(fileImportDir, errorDirName, strTime + importSubscriberFileName);
            EmailAddAttachment(message, importFullFileName, ie => ie.SubGenImportSubscriber);
            
            // Add attachment for transformed subscriber
            const string subscriberTransformedFileName = "_SubscriberTransformed_BadRecords.csv";
            var transformedFullFileName = Path.Combine(fileImportDir, errorDirName, strTime + subscriberTransformedFileName);
            EmailAddAttachment(message, transformedFullFileName, ie => ie.UADSubscriberTransformed);

            // Add attachment for import errors
            const string fileRecordsFileName = "_FileRecords.csv";
            var fileRecordsFullFileName = Path.Combine(fileImportDir, errorDirName, strTime + fileRecordsFileName);
            EmailAddAttachment(message, fileRecordsFullFileName);

            // Save email body to file
            const string bodyDirName = "ResultRecap";
            const string bodyFileName = "_ResultRecap.txt";
            Directory.CreateDirectory(fileImportDir + bodyDirName);
            var bodyFullFileName = Path.Combine(fileImportDir, bodyDirName, strTime + bodyFileName);
            var fileFunctions = new FileFunctions();
            fileFunctions.CreateFile(bodyFullFileName, sbMessageBody.ToString());

            SendEmail(message, sbMessageBody);
        }

        private void SendEmail(MailMessage message, StringBuilder sbDist)
        {
            try
            {
                var subject = ConfigurationManager.AppSettings["ErrorSubject"];
                message.Subject = subject;
                message.To.Add(ConfigurationManager.AppSettings["ErrorNotification"]);
                message.CC.Add(Core.ADMS.Settings.AdminEmail);
                message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
                message.IsBodyHtml = false;
                message.Body = sbDist.ToString();
                message.Priority = MailPriority.High;

                var smtp = new SmtpClient(Core.ADMS.Settings.SMTP);
                smtp.SendCompleted += SendCompletedCallback;

                smtp.Send(message);
            }
            catch (Exception ex)
            {
                var errorMessage = StringFunctions.FormatException(ex);
                Trace.TraceError(errorMessage);
            }
        }

        private void EmailAddAttachment(MailMessage message, string fileRecordsFullFileName)
        {
            var sbDataRow = GetImportErrorsDataRow();
            if (sbDataRow.Length == 0)
            {
                return;
            }

            try
            {
                var details = Attachment.CreateAttachmentFromString(sbDataRow.ToString(), fileRecordsFullFileName);
                message.Attachments.Add(details);
            }
            catch (Exception ex)
            {
                var errorMessage = StringFunctions.FormatException(ex);
                Trace.TraceError(errorMessage);
            }
        }

        private StringBuilder GetImportErrorsDataRow()
        {
            var sbDataRow = new StringBuilder();

            StringBuilder sbDataRowHeaders = null;
            foreach (var importError in importErrors)
            {
                var sbDataRowDetail = new StringBuilder();
                if (sbDataRowHeaders == null)
                {
                    sbDataRowHeaders = new StringBuilder();
                    if (importError.DataRow != null)
                    {
                        foreach (string colName in importError.DataRow.Keys)
                        {
                            sbDataRowHeaders.Append($"\"{colName}\",");
                        }

                        sbDataRow.AppendLine(sbDataRowHeaders.ToString().Trim().TrimEnd(','));
                    }
                }

                if (importError.DataRow != null)
                {
                    foreach (string colName in importError.DataRow.Keys)
                    {
                        sbDataRowDetail.Append($"\"{importError.DataRow[colName]}\",");
                    }
                }

                sbDataRow.AppendLine(sbDataRowDetail.ToString().Trim().TrimEnd(','));
            }

            return sbDataRow;
        }

        private void EmailAddAttachment(MailMessage message, string fullFileName, Func<ImportError, object> getErrorProperty)
        {
            var records = importErrors
                .Where(e => getErrorProperty(e) != null)
                .Select(getErrorProperty)
                .ToList();

            if (!records.Any())
            {
                return;
            }

            try
            {
                var dataTable = DataTableFunctions.ToDataTable(records);
                var fileWorker = new FileFunctions();
                fileWorker.CreateCSVFromDataTable(dataTable, fullFileName);
                message.Attachments.Add(new Attachment(fullFileName));
            }
            catch (Exception e)
            {
                var errorMessage = StringFunctions.FormatException(e);
                Trace.TraceError(errorMessage);
            }
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                ConsoleMessage(string.Format("[{0}] Send canceled.", token));
            }
            if (e.Error != null)
            {
                ConsoleMessage(string.Format("[{0}] {1}", token, e.Error.ToString()));
            }
            else
            {
                ConsoleMessage("Message sent.");
            }
        }
        void ConsoleMessage(string msg)
        {
            Console.WriteLine(msg + " " + DateTime.Now.ToString());
        }
        #endregion
        #region SubGen to UAD
        public void SubGenToUAD()
        {
            var syncWorker = new SubGenUadSync(ConsoleMessage, LogError, CDCtoImportSubscriber, accounts);
            syncWorker.SubGenToUad();
        }
        public Dictionary<int, int> GetSubPubs(FrameworkSubGen.Entity.ChangeDataCapture changeData, FrameworkSubGen.Entity.Enums.Client sgClient)
        {
            Dictionary<int, int> dictSubPub = new Dictionary<int, int>();

            List<int> subscriberIds = new List<int>();
            changeData.purchases.Select(x => x.subscriber_id).Distinct().ToList().ForEach(y => subscriberIds.Add(y));
            subscriberIds = subscriberIds.Distinct().ToList();

            //first see how many we can get from Subscriptions
            foreach (int i in subscriberIds)
            {
                if (changeData.subscriptions.Exists(x => x.subscriber_id == i))
                    dictSubPub.Add(i, changeData.subscriptions.Single(x => x.subscriber_id == i).publication_id);
            }
            foreach (KeyValuePair<int, int> kvp in dictSubPub)
                subscriberIds.Remove(kvp.Key);

            //now my list of subscirberIds are the ones we need to call for to get publication_id
            foreach (int i in subscriberIds)
            {
                FrameworkSubGen.BusinessLogic.API.Subscription subWorker = new FrameworkSubGen.BusinessLogic.API.Subscription();
                List<FrameworkSubGen.Entity.Subscription> iSubscriptions = subWorker.GetSubscriptions(sgClient, true, FrameworkSubGen.Entity.Enums.SubscriptionType.Both,
                    -1, -1, -1, -1, -1, -1, -1, 0, i);
                iSubscriptions.AddRange(subWorker.GetSubscriptions(sgClient, true, FrameworkSubGen.Entity.Enums.SubscriptionType.Digital,
                    -1, -1, -1, -1, -1, -1, -1, 0, i));
                iSubscriptions.AddRange(subWorker.GetSubscriptions(sgClient, true, FrameworkSubGen.Entity.Enums.SubscriptionType.Print,
                    -1, -1, -1, -1, -1, -1, -1, 0, i));

                //now iSubscriptions should have all possible

            }

            return dictSubPub;
        }

        public void CDCtoImportSubscriber(FrameworkSubGen.Entity.ChangeDataCapture changeData, FrameworkSubGen.Entity.Enums.Client sgClient, FrameworkSubGen.Entity.Account account)
        {
            KMPlatform.BusinessLogic.Client cWorker = new KMPlatform.BusinessLogic.Client();
            KMPlatform.Entity.Client client = cWorker.Select(account.KMClientId);

            List<FrameworkSubGen.Entity.ImportSubscriber> isList = new List<FrameworkSubGen.Entity.ImportSubscriber>();
            FrameworkSubGen.BusinessLogic.Publication pubWorker = new FrameworkSubGen.BusinessLogic.Publication();
            publications = pubWorker.Select();

            //I think better approach will be to convert changeData to ImportSubscriber/ImportDimension/ImportDimensionDetail
            //then convert to SubscriberTransformed --> save to UAD --> create DQMQue entry
            #region Subscription
            if (changeData.subscriptions != null && changeData.subscriptions.Count > 0)
            {
                ConsoleMessage("Processing CDC subscriptions " + DateTime.Now.ToString());
                int subCount = 1;
                foreach (FrameworkSubGen.Entity.Subscription sub in changeData.subscriptions)
                {
                    ConsoleMessage("Subscription " + subCount.ToString() + " of " + changeData.subscriptions.Count.ToString());
                    #region CDC to ImportSubscriber
                    try
                    {
                        FrameworkSubGen.Entity.ImportSubscriber iSub = new FrameworkSubGen.Entity.ImportSubscriber();
                        //changeData.addresses
                        //changeData.custom_fields
                        //changeData.purchases - this has to be done seperately i think
                        //changeData.subscribers

                        iSub.account_id = sub.account_id;
                        iSub.SubscriptionID = sub.subscription_id;
                        iSub.SystemSubscriberID = sub.subscriber_id;
                        FrameworkSubGen.Entity.Subscriber subscriber = null;
                        if (changeData.subscribers.Exists(x => x.subscriber_id == sub.subscriber_id))
                        {
                            subscriber = changeData.subscribers.Single(x => x.subscriber_id == sub.subscriber_id);
                            //lets remove subscriber from changeData.subscribers
                            changeData.subscribers.Remove(subscriber);
                        }
                        else
                        {
                            //we need to call SG and get Subscriber
                            FrameworkSubGen.BusinessLogic.API.Subscriber subWorker = new FrameworkSubGen.BusinessLogic.API.Subscriber();
                            subscriber = subWorker.GetSubscriber(sgClient, sub.subscriber_id);
                        }

                        iSub.SystemBillingAddressID = sub.billing_address_id;
                        iSub.SubscriptionGeniusMailingAddressID = sub.mailing_address_id;
                        FrameworkSubGen.Entity.Address mailingAddress = null;
                        FrameworkSubGen.Entity.Address billingAddress = null;
                        //lets see if we have the address in our address CDC object
                        if (changeData.addresses.Exists(x => x.address_id == sub.billing_address_id))
                        {
                            billingAddress = changeData.addresses.Single(x => x.address_id == sub.billing_address_id);
                            //lets remove billingAddress from changeData.addresses
                            changeData.addresses.Remove(billingAddress);
                        }
                        else
                        {
                            //we need to call SG and get billing address
                            FrameworkSubGen.BusinessLogic.API.Address adWorker = new FrameworkSubGen.BusinessLogic.API.Address();
                            billingAddress = adWorker.GetAddress(sgClient, sub.billing_address_id);
                        }

                        if (sub.billing_address_id == sub.mailing_address_id)
                        {
                            mailingAddress = billingAddress;
                        }
                        else if (changeData.addresses.Exists(x => x.address_id == sub.mailing_address_id))
                        {
                            mailingAddress = changeData.addresses.Single(x => x.address_id == sub.mailing_address_id);
                            //lets remove mailingAddress from changeData.addresses
                            changeData.addresses.Remove(mailingAddress);
                        }
                        else
                        {
                            //we need to call SG and get mailing address
                            FrameworkSubGen.BusinessLogic.API.Address adWorker = new FrameworkSubGen.BusinessLogic.API.Address();
                            mailingAddress = adWorker.GetAddress(sgClient, sub.mailing_address_id);
                        }


                        iSub.PublicationID = sub.publication_id;
                        if (publications.Exists(x => x.publication_id == sub.publication_id))
                            iSub.PublicationName = publications.Single(x => x.publication_id == sub.publication_id).name;
                        else
                            iSub.PublicationName = string.Empty;

                        iSub.IsLead = false;
                        iSub.RenewalCode_CustomerID = subscriber.renewal_code;
                        iSub.IssuesLeft = sub.paid_issues_left;
                        iSub.UnearnedRevenue = sub.unearned_revenue;
                        iSub.IssuesLeft = sub.copies;
                        iSub.SubscriptionCreatedDate = sub.date_created;
                        iSub.SubscriptionRenewDate = sub.date_last_renewed;
                        iSub.SubscriptionExpireDate = sub.date_expired;
                        iSub.SubscriptionLastQualifiedDate = DateTime.Now;
                        iSub.SubscriptionType = sub.type.ToString();
                        //what is sub.audit_classification
                        iSub.AuditCategoryName = string.Empty;
                        iSub.AuditCategoryCode = string.Empty;
                        iSub.AuditRequestTypeName = sub.audit_request_type;
                        iSub.AuditRequestTypeCode = string.Empty;
                        //need to decide TransactionID based on a set of Busniess Rules - see AMS: Transaction Types sheet
                        //look at file field "Subscription Expire Date"
                        //if date > current date then it is an active so set to 40
                        //if date <= current date then it is an InActive so set to 60
                        DateTime minDate = Convert.ToDateTime("1/1/0001");
                        if (sub.date_expired.Date > DateTime.Now.Date || sub.date_expired.Date == minDate)
                            iSub.TransactionID = 40;
                        else
                            iSub.TransactionID = 60;

                        //get this from Subscriber object
                        iSub.SubscriberAccountFirstName = subscriber.first_name;
                        iSub.SubscriberAccountLastName = subscriber.last_name;
                        iSub.SubscriberEmail = subscriber.email;
                        iSub.SubscriberPhone = string.Empty;
                        iSub.SubscriberSource = subscriber.source;

                        //get this from Address object
                        iSub.MailingAddressFirstName = mailingAddress.first_name;
                        iSub.MailingAddressLastName = mailingAddress.last_name;
                        iSub.MailingAddressTitle = string.Empty;
                        iSub.MailingAddressLine1 = mailingAddress.address;
                        iSub.MailingAddressCity = mailingAddress.city;
                        iSub.MailingAddressState = mailingAddress.state;
                        iSub.MailingAddressZip = mailingAddress.zip_code;
                        iSub.MailingAddressCompany = mailingAddress.company;
                        iSub.MailingAddressCountry = mailingAddress.country;

                        //get this from Address object
                        iSub.BillingAddressFirstName = billingAddress.first_name;
                        iSub.BillingAddressLastName = billingAddress.last_name;
                        iSub.BillingAddressLine1 = billingAddress.address;
                        iSub.BillingAddressCity = billingAddress.city;
                        iSub.BillingAddressState = billingAddress.state;
                        iSub.BillingAddressZip = billingAddress.zip_code;
                        iSub.BillingAddressCompany = billingAddress.company;
                        iSub.BillingAddressCountry = billingAddress.country;

                        #region handle the CustomFields
                        ConsoleMessage("Processing CDC custom_fields " + DateTime.Now.ToString());
                        FrameworkSubGen.BusinessLogic.API.CustomField cfWrk = new FrameworkSubGen.BusinessLogic.API.CustomField();

                        List<FrameworkSubGen.Entity.CustomField> subCFlist = cfWrk.GetCustomFieldsForSubscriber(sgClient, iSub.SystemSubscriberID);
                        FrameworkSubGen.BusinessLogic.API.Subscriber ssW = new FrameworkSubGen.BusinessLogic.API.Subscriber();
                        FrameworkSubGen.Entity.Subscriber checkSub = ssW.GetSubscriber(sgClient, iSub.SystemSubscriberID);
                        FrameworkSubGen.BusinessLogic.API.Subscription sW = new FrameworkSubGen.BusinessLogic.API.Subscription();
                        FrameworkSubGen.Entity.Subscription mag = sW.GetSubscription(sgClient, iSub.SubscriptionID);

                        if (subCFlist.Count > 0)
                        {
                            List<FrameworkSubGen.Entity.CustomField> clientCFs = cfWrk.GetCustomFields(sgClient);
                            FrameworkSubGen.Entity.ImportDimension dim = new FrameworkSubGen.Entity.ImportDimension();
                            dim.PublicationID = iSub.PublicationID;
                            dim.SubscriptionID = iSub.SubscriptionID;
                            dim.SystemSubscriberID = iSub.SystemSubscriberID;
                            dim.account_id = iSub.account_id;
                            bool hasValidCF = false;
                            foreach (FrameworkSubGen.Entity.CustomField cf in subCFlist)
                            {
                                //remCF.Add(cf);
                                //This will have ALL CF's - make sure to only use those that have an actual answer
                                if (cf.text_value != null || cf.value_options != null || cf.value_options.Count > 0)
                                {
                                    hasValidCF = true;
                                    List<FrameworkSubGen.Entity.SubscriberDemographic> sdList = new List<FrameworkSubGen.Entity.SubscriberDemographic>();
                                    List<FrameworkSubGen.Entity.SubscriberDemographicDetail> sddList = new List<FrameworkSubGen.Entity.SubscriberDemographicDetail>();
                                    try
                                    {
                                        FrameworkSubGen.Entity.SubscriberDemographic sd = new FrameworkSubGen.Entity.SubscriberDemographic();
                                        sd.account_id = account.account_id;
                                        sd.DateCreated = DateTime.Now;
                                        sd.field_id = cf.field_id;
                                        sd.IsProcessed = false;
                                        sd.subscriber_id = cf.subscriber_id;
                                        sd.text_value = cf.text_value;

                                        if (cf.text_value != null)
                                        {
                                            FrameworkSubGen.Entity.ImportDimensionDetail otherCF = new FrameworkSubGen.Entity.ImportDimensionDetail();
                                            otherCF.SubscriptionID = iSub.SubscriptionID;
                                            otherCF.SystemSubscriberID = iSub.SystemSubscriberID;
                                            otherCF.PublicationID = iSub.PublicationID;
                                            if (clientCFs.Exists(x => x.field_id == cf.field_id))
                                                otherCF.DimensionField = clientCFs.Single(x => x.field_id == cf.field_id).name;
                                            else
                                                otherCF.DimensionField = cf.name;
                                            otherCF.DimensionValue = cf.text_value;

                                            dim.Dimensions.Add(otherCF);
                                        }

                                        foreach (var vo in cf.value_options.Where(x => x.value != null))
                                        {
                                            FrameworkSubGen.Entity.SubscriberDemographicDetail sdd = new FrameworkSubGen.Entity.SubscriberDemographicDetail();
                                            sdd.account_id = account.account_id;
                                            sdd.DateCreated = DateTime.Now;
                                            sdd.field_id = vo.field_id;
                                            sdd.IsProcessed = false;
                                            sdd.option_id = vo.option_id;
                                            sdd.subscriber_id = cf.subscriber_id;
                                            sdd.value = vo.value;
                                            sddList.Add(sdd);

                                            FrameworkSubGen.Entity.ImportDimensionDetail idd = new FrameworkSubGen.Entity.ImportDimensionDetail();
                                            idd.SubscriptionID = iSub.SubscriptionID;
                                            idd.SystemSubscriberID = iSub.SystemSubscriberID;
                                            idd.PublicationID = iSub.PublicationID;
                                            if (clientCFs.Exists(x => x.field_id == cf.field_id))
                                                idd.DimensionField = clientCFs.Single(x => x.field_id == cf.field_id).name;
                                            else
                                                idd.DimensionField = cf.name;
                                            idd.DimensionValue = vo.value;

                                            dim.Dimensions.Add(idd);
                                        }
                                        sd.subDemoDetails.AddRange(sddList);
                                        sdList.Add(sd);
                                    }
                                    catch (Exception ex)
                                    {
                                        LogError(ex, this.GetType().Name.ToString());
                                    }
                                    //not do anything at this time - 03/28/2016 - per JB
                                    //log for now

                                    try
                                    {
                                        FrameworkSubGen.BusinessLogic.SubscriberDemographic sdWrk = new FrameworkSubGen.BusinessLogic.SubscriberDemographic();
                                        FrameworkSubGen.BusinessLogic.SubscriberDemographicDetail sddWrk = new FrameworkSubGen.BusinessLogic.SubscriberDemographicDetail();
                                        if (sdList.Count > 0)
                                            sdWrk.Save(sdList);
                                        if (sddList.Count > 0)
                                            sddWrk.Save(sddList);
                                    }
                                    catch (Exception ex)
                                    {
                                        LogError(ex, this.GetType().Name.ToString());
                                    }
                                }
                            }
                            if (hasValidCF == true)
                                iSub.Dimensions = dim;
                        }
                        //foreach (FrameworkSubGen.Entity.CustomField cf in remCF)
                        //    changeData.custom_fields.Remove(cf);
                        #endregion

                        FrameworkSubGen.BusinessLogic.Payment pWrk = new FrameworkSubGen.BusinessLogic.Payment();
                        FrameworkSubGen.Entity.Payment pay = pWrk.Select(sub.subscriber_id, sub.date_created);
                        if (pay != null && pay.order_id > 0)
                            pWrk.Update_SubscriptionId(pay.order_id, sub.subscription_id);

                        isList.Add(iSub);
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, this.GetType().Name.ToString());
                    }
                    #endregion
                    subCount++;
                }
                if (isList.Count > 0)
                    ADMS_DQM(account, isList);

                FrameworkSubGen.BusinessLogic.Subscription subWrk = new FrameworkSubGen.BusinessLogic.Subscription();
                subWrk.SaveBulkXml(changeData.subscriptions);
            }
            #endregion
            #region Purchases
            //not sure if need to deal with this.
            //need to test creating a new subscription and see what all is there for cdc
            //just log data for now.
            //save in SubGenData.Purchase table - new column IsProcessed / ProcessedDate
            try
            {
                if (changeData.purchases != null && changeData.purchases.Count > 0)
                {
                    ConsoleMessage("Processing CDC purchases " + DateTime.Now.ToString());
                    FrameworkSubGen.BusinessLogic.Purchase pWrk = new FrameworkSubGen.BusinessLogic.Purchase();
                    pWrk.SaveBulkXml(changeData.purchases);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, this.GetType().Name.ToString());
            }
            #endregion
            #region Subscriber
            //no tracking - just update our values that do not match - first, last, email
            //pass to uad sproc - Subscriptions and PubSubscriptions 
            try
            {
                if (changeData.subscribers != null && changeData.subscribers.Count > 0)
                {
                    ConsoleMessage("Processing CDC subscribers " + DateTime.Now.ToString());
                    FrameworkSubGen.BusinessLogic.Subscriber sWrk = new FrameworkSubGen.BusinessLogic.Subscriber();
                    sWrk.SaveBulkXml(changeData.subscribers);
                    string xml = sWrk.GetXmlForUpdateClientUAD(changeData.subscribers);
                    FrameworkUAD.BusinessLogic.SubGen sgWrk = new FrameworkUAD.BusinessLogic.SubGen();
                    sgWrk.SubGen_Subscriber_Update(xml, client.ClientConnections);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, this.GetType().Name.ToString());
            }
            #endregion
            #region Addresses
            try
            {
                if (changeData.addresses != null && changeData.addresses.Count > 0)
                {
                    ConsoleMessage("Processing CDC addresses " + DateTime.Now.ToString());
                    //address information changed so update SubGenData.Address by address_id
                    FrameworkSubGen.BusinessLogic.Address aWrk = new FrameworkSubGen.BusinessLogic.Address();
                    aWrk.SaveBulkXml(changeData.addresses);

                    //create object for xml
                    //do an address change for ALL PAID Products for SubscriberId
                    //change TransactionCodeId to whatever it is for paid address change   21
                    //117	3	Address Change Only (Active Paid)	21
                    string xml = aWrk.GetXml(changeData.addresses);
                    FrameworkUAD.BusinessLogic.SubGen sgWrk = new FrameworkUAD.BusinessLogic.SubGen();
                    sgWrk.SubGen_Address_Update(xml, client.ClientConnections);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, this.GetType().Name.ToString());
            }
            #endregion
            #region Subscriber Demographics
            if (changeData.custom_fields != null && changeData.custom_fields.Count > 0)
            {
                ConsoleMessage("Processing CDC custom_fields " + DateTime.Now.ToString());
                List<FrameworkSubGen.Entity.SubscriberDemographic> sdList = new List<FrameworkSubGen.Entity.SubscriberDemographic>();
                List<FrameworkSubGen.Entity.SubscriberDemographicDetail> sddList = new List<FrameworkSubGen.Entity.SubscriberDemographicDetail>();
                foreach (var cdc in changeData.custom_fields)
                {
                    try
                    {
                        FrameworkSubGen.Entity.SubscriberDemographic sd = new FrameworkSubGen.Entity.SubscriberDemographic();
                        sd.account_id = account.account_id;
                        sd.DateCreated = DateTime.Now;
                        sd.field_id = cdc.field_id;
                        sd.IsProcessed = false;
                        sd.subscriber_id = cdc.subscriber_id;
                        sd.text_value = cdc.text_value;
                        sdList.Add(sd);

                        foreach (var vo in cdc.value_options)
                        {
                            FrameworkSubGen.Entity.SubscriberDemographicDetail sdd = new FrameworkSubGen.Entity.SubscriberDemographicDetail();
                            sdd.account_id = account.account_id;
                            sdd.DateCreated = DateTime.Now;
                            sdd.field_id = vo.field_id;
                            sdd.IsProcessed = false;
                            sdd.option_id = vo.option_id;
                            sdd.subscriber_id = cdc.subscriber_id;
                            sdd.value = vo.value;
                            sddList.Add(sdd);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, this.GetType().Name.ToString());
                    }
                    //not do anything at this time - 03/28/2016 - per JB
                    //log for now
                }
                try
                {
                    FrameworkSubGen.BusinessLogic.SubscriberDemographic sdWrk = new FrameworkSubGen.BusinessLogic.SubscriberDemographic();
                    FrameworkSubGen.BusinessLogic.SubscriberDemographicDetail sddWrk = new FrameworkSubGen.BusinessLogic.SubscriberDemographicDetail();
                    if (sdList.Count > 0)
                        sdWrk.Save(sdList);
                    if (sddList.Count > 0)
                        sddWrk.Save(sddList);
                }
                catch (Exception ex)
                {
                    LogError(ex, this.GetType().Name.ToString());
                }
            }
            #endregion
            #region Bundles
            try
            {
                if (changeData.bundles != null && changeData.bundles.Count > 0)
                {
                    ConsoleMessage("Processing CDC bundles " + DateTime.Now.ToString());
                    //changeData.bundles this is only a list of bundle_id
                    FrameworkSubGen.BusinessLogic.API.Bundle apiB = new FrameworkSubGen.BusinessLogic.API.Bundle();
                    List<FrameworkSubGen.Entity.Bundle> cdcBundles = new List<FrameworkSubGen.Entity.Bundle>();
                    foreach (var b in changeData.bundles)
                    {
                        if (b.bundle_id > 0)
                        {
                            FrameworkSubGen.Entity.Bundle bundle = apiB.GetBundle(sgClient, b);
                            if (bundle != null)
                            {
                                if (!cdcBundles.Contains(bundle))
                                    cdcBundles.Add(bundle);
                            }
                        }
                    }
                    FrameworkSubGen.BusinessLogic.Bundle bWrk = new FrameworkSubGen.BusinessLogic.Bundle();
                    bWrk.SaveBulkXml(cdcBundles, account.account_id);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, this.GetType().Name.ToString());
            }
            #endregion
        }
        #endregion
        #endregion
    }
}
