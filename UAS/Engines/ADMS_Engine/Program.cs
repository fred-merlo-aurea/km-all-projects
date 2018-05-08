using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using ADMS;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using KM.Common.Import;
//using System.Reflection;

namespace ADMS_Engine
{
    class Program
    {
        #region variables
        public readonly string engine = "ADMS";
        public readonly FrameworkUAS.BusinessLogic.EngineLog elWrk = new FrameworkUAS.BusinessLogic.EngineLog();
        public readonly KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
        public readonly KMPlatform.BusinessLogic.ApplicationLog appLogWorker = new KMPlatform.BusinessLogic.ApplicationLog();
        public readonly FrameworkUAS.BusinessLogic.AdmsLog admsWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
        public FrameworkUAS.Entity.AdmsLog admsLog { get; set; }
        private FrameworkUAS.Entity.EngineLog _engineLog;
        public FrameworkUAS.Entity.EngineLog engineLog
        {
            get
            {
                if (_engineLog == null)
                    _engineLog = elWrk.Select(client.ClientID, engine);
                return _engineLog;
            }
            set
            {
                _engineLog = value;
            }
        }
        private KMPlatform.Entity.Client _client;
        public KMPlatform.Entity.Client client
        {
            get
            {
                if (_client == null)
                {
                    KMPlatform.BusinessLogic.Client cWrk = new KMPlatform.BusinessLogic.Client();
                    _client = cWrk.SelectFtpFolder(System.Configuration.ConfigurationManager.AppSettings["EngineClient"].ToString(), true);
                }
                return _client;
            }
            set
            {
                _client = value;
            }
        }
        public string processCode
        {
            get
            {
                if (string.IsNullOrEmpty(processCode))
                    processCode = admsLog.ProcessCode;
                return processCode;
            }
            set { processCode = value; }
        }
        #endregion

        private static void Main(string[] arg)
        {
            Program startADMS = new Program();
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionLog;
            Console.Title = ConfigurationManager.AppSettings["EngineClient"].ToString() + " - ADMS Engine";
            //var currentAssembly = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), true);
            ////change assembly description
            //typeof(AssemblyDescriptionAttribute).GetField("m_description", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(currentAssembly[0], Console.Title);
           

            //arg = new string[1];
            //arg[0] = "geo";

            #region Testing methods
            //startADMS.SplitFile();
            //startADMS.GeoCode();
            //startADMS.RunDQM();
            //startADMS.ClientMethods();
            //startADMS.Features();
            #endregion
            if (arg.Length > 0)
            {
                if (arg[0].Equals("geo", StringComparison.CurrentCultureIgnoreCase))
                {
                    startADMS.GeoCode();
                }
            }
            else
                startADMS.StartFileSystemMonitor();
        }
        #region testing methods
        private void Features()
        {
            //ADMS.Services.Feature.ConsensusDimension cd = new ADMS.Services.Feature.ConsensusDimension();

            //KMPlatform.BusinessLogic.Client cData = new KMPlatform.BusinessLogic.Client();
            //FrameworkUAS.BusinessLogic.FileStatus fsData = new FrameworkUAS.BusinessLogic.FileStatus();
            //FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
            //KMPlatform.Entity.Client client = cData.Select(8, true);
            //FrameworkUAS.Entity.SourceFile sourceFile = sfData.SelectSourceFileID(740, false);
            //FrameworkUAS.Entity.FileStatus fileStatus = fsData.SelectFile(sourceFile.SourceFileID).FirstOrDefault();
            //System.IO.FileInfo file = new FileInfo("C:\\ADMS\\ConDimTest.csv");

            //cd.ProcessFile(client, fileStatus, sourceFile, file);
        }
        void RunDQM()
        {
            //BillTurner bootstrapper;
            //bootstrapper = new BillTurner();
            //bootstrapper.Initialize();

            //string processCode = Core_AMS.Utilities.StringFunctions.GenerateProcessCode();
            //DQMCleaner dqm = new DQMCleaner();
            //KMPlatform.BusinessLogic.Client cData = new KMPlatform.BusinessLogic.Client();
            //FrameworkUAS.BusinessLogic.FileStatus fsData = new FrameworkUAS.BusinessLogic.FileStatus();
            //FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
            //List<int> sfIDs = new List<int>();
            ////sfIDs.Add(581);
            //sfIDs.Add(576);



            //sfIDs.Add(169);
            //sfIDs.Add(170);
            //sfIDs.Add(172);
            //sfIDs.Add(174);
            //sfIDs.Add(175);
            //sfIDs.Add(176);
            //sfIDs.Add(179);
            //sfIDs.Add(192);
            //sfIDs.Add(194);
            //sfIDs.Add(196);
            //sfIDs.Add(321);
            //sfIDs.Add(324);
            //sfIDs.Add(328);
            //sfIDs.Add(329);
            //sfIDs.Add(330);
            //sfIDs.Add(331);
            //sfIDs.Add(337);
            //sfIDs.Add(341);
            //sfIDs.Add(344);
            //sfIDs.Add(354);
            //sfIDs.Add(356);
            //sfIDs.Add(365);
            //sfIDs.Add(366);
            //sfIDs.Add(375);
            //sfIDs.Add(377);
            //sfIDs.Add(378);
            //sfIDs.Add(382);
            //sfIDs.Add(383);
            //sfIDs.Add(386);
            //sfIDs.Add(388);
            //sfIDs.Add(390);
            //sfIDs.Add(397);
            //sfIDs.Add(405);
            //sfIDs.Add(471);
            //sfIDs.Add(474);
            //sfIDs.Add(480);
            //sfIDs.Add(481);
            //sfIDs.Add(482);
            //sfIDs.Add(483);
            //sfIDs.Add(487);
            //sfIDs.Add(499);
            //sfIDs.Add(551);
            //sfIDs.Add(553);
            //sfIDs.Add(555);
            //sfIDs.Add(594);

            //KMPlatform.Entity.Client client = cData.Select(23, true);
            //foreach (int s in sfIDs)
            //{
            //    ConsoleMessage("Running " + s.ToString() + " : " + DateTime.Now.ToString(), processCode);

            //    FrameworkUAS.Entity.SourceFile sourceFile = sfData.SelectSourceFileID(s, true);
            //    FrameworkUAS.Entity.FileStatus fileStatus = fsData.SelectFileAndStatus(sourceFile.SourceFileID, 7).First();


            //    dqm.RunStandardization(client, fileStatus, sourceFile, processCode);
            //}
            //ConsoleMessage("Done " + DateTime.Now.ToString(), processCode);
            //Console.ReadKey();
        }
        void ClientMethods()
        {
            string processCode = Core_AMS.Utilities.StringFunctions.GenerateProcessCode();
            //Logging logger;
            //ADMS.ClientMethods.Haymarket h = new ADMS.ClientMethods.Haymarket();
            //KMPlatform.BusinessLogic.Client cData = new KMPlatform.BusinessLogic.Client();
            //ConsoleMessage("Getting Clients: " + DateTime.Now.ToString(), processCode);
            //KMPlatform.Entity.Client client = cData.Select(23, true);
            //KMPlatform.Entity.ClientSpecialFile csf = client.ClientSpecialFiles.Single(x => x.FileName.Equals("CMS.zip"));
            FileInfo file = new FileInfo("C:\\CMS.zip");
            try
            {
                //h.CreateCMSFileSQL(client, file, csf);
                //h.CreateCMSFile(client, file, csf);
            }
            catch (Exception ex)
            {
                //Logging.LogXMLIssue(ex);
                //logger = new Logging();
                //logger.LogIssue(ex);

                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".ClientMethods", app, string.Empty);
            }
        }
        void SplitFile()
        {
            FrameworkUAD.BusinessLogic.ImportVessel ivWorker = new FrameworkUAD.BusinessLogic.ImportVessel();
            Core_AMS.Utilities.FileWorker fileWorker = new Core_AMS.Utilities.FileWorker();
            //Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();
            System.IO.FileInfo file = new FileInfo("C:\\SqlBackup\\FIXED_Haymarket_Medical.CSV");
            //System.Data.DataTable dt = fw.GetData(file);

            FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
            FrameworkUAS.Entity.SourceFile sourceFile = sfData.SelectSourceFileID(576);

            string pubFile = "C:\\SqlBackup\\todo_2.csv";
            int fileTotalRowCount = fileWorker.GetRowCount(file);
            //int fileRowProcessedCount = 0;
            int fileRowBatch = 949500;

            FrameworkUAD.Object.ImportVessel dataIV = new FrameworkUAD.Object.ImportVessel();
            var fileConfig = new FileConfiguration()
            {
                FileColumnDelimiter = sourceFile.Delimiter,
                IsQuoteEncapsulated = sourceFile.IsTextQualifier,
            };

            dataIV = ivWorker.GetImportVessel(file, 941001, fileRowBatch, fileConfig);

            //ff.CreateCSVFromDataTable(dataIV.DataOriginal, pubFile, false);
            Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
            Workbook wb = ef.GetWorkbook(dataIV.DataOriginal, "Matching Demos");
            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
            using (FileStream output = new FileStream(pubFile, FileMode.Create))
            {
                formatProvider.Export(wb, output);
            }
        }
        #endregion
        static void UnhandledExceptionLog(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception) e.ExceptionObject;
            KMPlatform.BusinessLogic.ApplicationLog appLogWorker = new KMPlatform.BusinessLogic.ApplicationLog();
            KMPlatform.BusinessLogic.Client cWrk = new KMPlatform.BusinessLogic.Client();
            KMPlatform.Entity.Client client = cWrk.SelectFtpFolder(System.Configuration.ConfigurationManager.AppSettings["EngineClient"].ToString(), true);

            StringBuilder sbDetail = new StringBuilder();
            sbDetail.AppendLine("Client: " + client.FtpFolder);
            sbDetail.AppendLine("ExceptionObject: " + e.ExceptionObject.ToString());

            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
            if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
            appLogWorker.LogCriticalError(Core_AMS.Utilities.StringFunctions.FormatException(ex), "ADMS_Engine.Program.UnhandledExceptionLog", app, sbDetail.ToString(), client.ClientID);

            //restart the application - task scheduler should restart
            //System.Diagnostics.Process.Start("ADMS_Engine.exe");
            Environment.Exit(1);
        }
        public void ConsoleMessage(string message, string processCode = "", bool createLog = true, int sourceFileId = 0, int updatedByUserId = 0)
        {
            if (string.IsNullOrEmpty(processCode) && admsLog == null)
                return;
            int fileStatusTypeId = 0;
            if (admsLog != null)
            {
                if (string.IsNullOrEmpty(processCode))
                    processCode = admsLog.ProcessCode;
                if (sourceFileId == 0)
                    sourceFileId = admsLog.SourceFileId;
                fileStatusTypeId = admsLog.FileStatusId;
            }
            admsWrk.UpdateStatusMessage(processCode, message, updatedByUserId, createLog, sourceFileId, fileStatusTypeId);
        }
        public void LogError(Exception ex, KMPlatform.Entity.Client client, string msg, bool removeThread = true, bool removeQue = true)
        {
            #region Log Error
            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            try
            {
                if (removeThread)
                    ThreadDictionary.Remove(admsLog.ThreadId);
                //if (removeQue)
                //    ADMSProcessingQue.RemoveClientFile(client, admsLog.ImportFile);
                ConsoleMessage(formatException);
            }
            catch { }

            StringBuilder sbDetail = new StringBuilder();
            try
            {
                sbDetail.AppendLine("Client: " + client.FtpFolder);
                if (admsLog != null && admsLog.ImportFile != null)
                    sbDetail.AppendLine("File: " + admsLog.ImportFile.FullName);
                sbDetail.AppendLine("SourceFileID: " + admsLog != null ? admsLog.SourceFileId.ToString() : string.Empty);
                sbDetail.AppendLine("Thread: " + admsLog != null ? admsLog.ThreadId.ToString() : string.Empty);
                sbDetail.AppendLine("ProcessCode: " + admsLog != null ? admsLog.ProcessCode.ToString() : string.Empty);
                sbDetail.AppendLine(msg);
            }
            catch { }

            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
            if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
            appLogWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".LogError", app, sbDetail.ToString());
            #endregion
        }

        void GeoCode()
        {
            //string processCode = "615HY3pls64I_04072015_20:16:34";// Core_AMS.Utilities.StringFunctions.GenerateProcessCode();
            KMPlatform.BusinessLogic.Client cData = new KMPlatform.BusinessLogic.Client();
            List<KMPlatform.Entity.Client> clients = new List<KMPlatform.Entity.Client>(); //cData.Select(false).ToList();
            List<string> engineClients = ConfigurationManager.AppSettings["EngineClients"].Split(',').ToList();
            foreach (string c in engineClients)
                clients.Add(cData.Select(c, false));

            foreach (var c in clients)
            {
                if (c.IsActive == true)
                {
                    FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
                    List<FrameworkUAS.Entity.SourceFile> files = sfData.Select(c.ClientID, false).ToList();
                    foreach (var f in files)
                    {
                        if (f.SourceFileID == 1622)
                        {
                            if (f.IsDeleted == false && f.IsDQMReady == true && f.IsIgnored == false)
                            {
                                try
                                {
                                    //AddressClean ac = new AddressClean();
                                    //ac.AddressStandardize(c, f.SourceFileID, processCode, FrameworkUAS.BusinessLogic.Enums.FileStatusTypes.GeoCode);
                                }
                                catch (Exception ex)
                                {
                                    //string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                    //ConsoleMessage(msg, processCode, true, f.SourceFileID);

                                    KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
                                    KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                                    string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                    alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".GeoCode", app, string.Empty);
                                }
                            }
                        }
                    }

                }
            }
        }
        void StartFileSystemMonitor()
        {
            //var domain = AppDomain.CurrentDomain;
            //domain.ProcessExit += new EventHandler(domain_ProcessExit);
            //SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
            admsLog = new FrameworkUAS.Entity.AdmsLog();

            DateTime startTime = DateTime.Now;
            bool updateMode = false;
            bool.TryParse(ConfigurationManager.AppSettings["IsUpdateMode"].ToString(), out updateMode);
            int updateMinutes = 30;
            int.TryParse(ConfigurationManager.AppSettings["UpdateRefreshMinutes"].ToString(), out updateMinutes);

            BillTurner bootstrapper;
            try
            {
                elWrk.UpdateIsRunning(engineLog.EngineLogId, true);

                bootstrapper = new BillTurner();
                try
                {
                    bootstrapper.Initialize();
                }
                catch(Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".bootstrapper.Initialize()", false, false);
                }
                if (updateMode == false)
                    System.Console.ReadKey();
                else
                {
#if DEBUG
                    Console.WriteLine("drop file");
#endif
                    while (1 == 1)
                    {
                        int count = 0;
                        try
                        {
                            count = ThreadDictionary.activeThreads.Count;
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, client, this.GetType().Name.ToString() + ".StartFileSystemMonitor - threadcount", false, false);
                        }
                        //FEATURE:  should track last Refreshed date and if > 12 hours send an email notice that ADMS not refreshed for over 12 hours
                        if (DateTime.Now.TimeOfDay >= startTime.AddMinutes(updateMinutes).TimeOfDay)
                        {
                            startTime = DateTime.Now;
                            if (count == 0)
                            {
                                elWrk.UpdateRefresh(engineLog.EngineLogId);
                                bootstrapper.ReactivateWatcher();
                            }
                        }
                        System.Threading.Thread.Sleep(60000);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".StartFileSystemMonitor", false, false);
            }
        }
        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception) args.ExceptionObject;
            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
            KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            alWorker.LogCriticalError(formatException, "Program.MyHandler", app, "Client: " + ConfigurationManager.AppSettings["EngineClient"].ToString());
        }

        static void domain_ProcessExit(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("ADMS_Engine.exe");
        }
        static void domain_DomainUnload(object sender, EventArgs e)
        {

        }
        #region DEPRICATED 8/17/16 WAGNER : notification if engine crashes/stops running
        //[DllImport("Kernel32")]
        //public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        //// A delegate type to be used as the handler routine
        //// for SetConsoleCtrlHandler.
        //public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        //// An enumerated type for the control messages
        //// sent to the handler routine.
        //public enum CtrlTypes
        //{
        //    CTRL_C_EVENT = 0,
        //    CTRL_BREAK_EVENT,
        //    CTRL_CLOSE_EVENT,
        //    CTRL_LOGOFF_EVENT = 5,
        //    CTRL_SHUTDOWN_EVENT
        //}

        ///// <summary>
        ///// commenting this out oon 8/17/16 Wagner
        ///// when we deploy new ADMS engine with logging will be running as scheduled Task and will configure to restart if close
        ///// </summary>
        ///// <param name="ctrlType"></param>
        ///// <returns></returns>
        //private bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        //{

        //    //// Put your own handler here
        //    //MailMessage message = new MailMessage();
        //    ////foreach (string sendTo in sendToList)

        //    //message.To.Add(Core.ADMS.Settings.AdminEmail);
        //    //string subject = "ADMS Closed on server: " + ConfigurationManager.AppSettings["EngineServer"].ToString();

        //    //message.Subject = subject;
        //    //message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
        //    //message.Priority = MailPriority.High;
        //    //message.Body = "ADMS closed unexpectedly on server: " + ConfigurationManager.AppSettings["EngineServer"].ToString() + ". Please check that it has restarted correctly.";

        //    //SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);

        //    //smtp.Send(message);
        //    return true;
        //}

        #endregion
    }
}
