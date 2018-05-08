using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Runtime.InteropServices;

namespace ServiceQueMonitor
{
    class Program
    {
        #region variables
        //readonly string engine = "SQM";
        public static List<KMPlatform.Entity.Service> services;
        readonly FrameworkUAS.BusinessLogic.EngineLog elWrk = new FrameworkUAS.BusinessLogic.EngineLog();
        readonly KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.Service_Que_Monitor;
        readonly KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
        readonly static FrameworkUAS.BusinessLogic.AdmsLog admsWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
        public static string processCode { get; set; }

        private Dictionary<int, FrameworkUAS.Object.ClientAdditionalProperties> _clientAdditionalProperties;
        Dictionary<int, FrameworkUAS.Object.ClientAdditionalProperties> clientAdditionalProperties
        {
            get
            {
                if (_clientAdditionalProperties == null)
                {
                    _clientAdditionalProperties = new Dictionary<int, FrameworkUAS.Object.ClientAdditionalProperties>();
                }
                return _clientAdditionalProperties;
            }
            set
            {
                _clientAdditionalProperties = value;
            }
        }

        private FrameworkUAS.Entity.EngineLog _engineLog;
        FrameworkUAS.Entity.EngineLog engineLog
        {
            get
            {
                if (_engineLog == null)
                {
                    bool isADMS = false;
                    string engine = "SQM";
                    bool.TryParse(ConfigurationManager.AppSettings["IsADMS"].ToString(), out isADMS);
                    if (isADMS)
                        engine = "ADMS_DQM";

                    _engineLog = elWrk.Select(client.ClientID, engine);
                }
                return _engineLog;
            }
            set
            {
                _engineLog = value;
            }
        }
        private KMPlatform.Entity.Client _client;
        KMPlatform.Entity.Client client
        {
            get
            {
                if (_client == null)
                {
                    KMPlatform.BusinessLogic.Client cWrk = new KMPlatform.BusinessLogic.Client();
                    _client = cWrk.SelectFtpFolder(ConfigurationManager.AppSettings["EngineClient"].ToString(), true);
                }
                return _client;
            }
            set
            {
                _client = value;
            }
        }
        private static FrameworkUAS.Entity.AdmsLog _admsLog;
        public static FrameworkUAS.Entity.AdmsLog admsLog
        {
            get
            {
                if (_admsLog == null)
                {
                    _admsLog = admsWrk.Select(processCode);
                }

                return _admsLog;
            }
            set
            {
                _admsLog = value;
            }
        }
        #endregion

        List<FrameworkUAD_Lookup.Entity.Code> databaseFileTypes { get; set; }

        static void Main()
        {
            Program start = new Program();
            Console.Title = ConfigurationManager.AppSettings["EngineClient"].ToString() + " - SQM Engine";
            start.DQMQueMonitor();
        }

        void DQMQueMonitor()
        {
            FrameworkUAS.BusinessLogic.SourceFile sf = new FrameworkUAS.BusinessLogic.SourceFile();
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            //WireUpRestart();
            DateTime startTime = DateTime.Now;
            int executeDQMInSeconds = 60;
            int.TryParse(ConfigurationManager.AppSettings["ExecuteDQMInSeconds"].ToString(), out executeDQMInSeconds);
            int timeout = executeDQMInSeconds * 1000;

            bool isADMS = false;
            bool.TryParse(ConfigurationManager.AppSettings["IsADMS"].ToString(), out isADMS);

            FrameworkUAS.BusinessLogic.Enums.Engine engineEnum = FrameworkUAS.BusinessLogic.Enums.Engine.SQM;
            bool.TryParse(ConfigurationManager.AppSettings["IsADMS"].ToString(), out isADMS);
            if (isADMS)
                engineEnum = FrameworkUAS.BusinessLogic.Enums.Engine.ADMS_DQM;

            bool isDemo = true;
            bool.TryParse(ConfigurationManager.AppSettings["IsDemo"].ToString(), out isDemo);

            //int clientID = 0;
            //int.TryParse(ConfigurationManager.AppSettings["EngineClient"].ToString(), out clientID);
            //KMPlatform.BusinessLogic.Client clientWorker = new KMPlatform.BusinessLogic.Client();
            //KMPlatform.Entity.Client client = clientWorker.Select(clientID);

            KMPlatform.BusinessLogic.Client cWrk = new KMPlatform.BusinessLogic.Client();
            KMPlatform.Entity.Client client = cWrk.SelectFtpFolder(System.Configuration.ConfigurationManager.AppSettings["EngineClient"].ToString(), true);

            if (clientAdditionalProperties == null)
                clientAdditionalProperties = new Dictionary<int, FrameworkUAS.Object.ClientAdditionalProperties>();

            FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWorker = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
            var caps = capWorker.SetObjects(client.ClientID, false);
            if (clientAdditionalProperties != null && clientAdditionalProperties.ToList().Exists(x => x.Key == client.ClientID))
                clientAdditionalProperties.Remove(client.ClientID);
            clientAdditionalProperties.Add(client.ClientID, caps);

            try
            {
                //AdmsLogCleanUpIncompleteFiles will call a sproc that will clean up any files that previously were stopped mid process because of an engine shutdown.
                //This code is for updating ADMSLog files that weren't fully completed so they are properly removed from the MVC Dashboard File Status. This will also
                //provide a message as to why it was incomplete. IE Engine shutoff or Error Threshold reached to display in the MVC Dashboard File History tab            
                FrameworkUAS.BusinessLogic.AdmsLog admsLogWorker = new FrameworkUAS.BusinessLogic.AdmsLog();
                admsLogWorker.AdmsLogCleanUp(client.ClientID, isADMS);
            }
            catch (Exception ex)
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.Service_Que_Monitor;
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".DQMQueMonitor", app, string.Empty, client.ClientID);
            }

            FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            databaseFileTypes = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Database_File).ToList();

            while (1 == 1)
            {
                try
                {
                    startTime = DateTime.Now;

                    try
                    {
                        List<FrameworkUAS.Entity.DQMQue> files = new List<FrameworkUAS.Entity.DQMQue>();
                        List<FrameworkUAS.Entity.SourceFile> sourceFiles = clientAdditionalProperties[client.ClientID].SourceFilesList;// new List<FrameworkUAS.Entity.SourceFile>();
                        FrameworkUAS.BusinessLogic.DQMQue dqmQueWorker = new FrameworkUAS.BusinessLogic.DQMQue();

                        files = dqmQueWorker.Select(client.ClientID, isDemo, isADMS).Where(x => x.IsCompleted == false && x.IsQued == false).ToList();
                        //sourceFiles = sfWrk.Select(client.ClientID,true,false);
                        if (files.Count > 0)
                        {
                            #region Process files
                            #region split files into Circ / Other lists
                            //order files so that Circ files always process first
                            if (services == null || services.Count == 0)
                            {
                                KMPlatform.BusinessLogic.Service sworker = new KMPlatform.BusinessLogic.Service();
                                services = sworker.Select(false).ToList();
                            }
                            int circ = services.SingleOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase)).ServiceID;
                            List<FrameworkUAS.Entity.SourceFile> circSourceFiles = new List<FrameworkUAS.Entity.SourceFile>();
                            if (clientAdditionalProperties != null && clientAdditionalProperties.ContainsKey(client.ClientID))
                            {
                                circSourceFiles = clientAdditionalProperties[client.ClientID].SourceFilesList.Where(x => x.ServiceID == circ).ToList();
                            }
                            else
                            {
                                FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                                FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                                cap = capWork.SetObjects(client.ClientID, false);
                                circSourceFiles = cap.SourceFilesList.Where(x => x.ServiceID == circ).ToList();
                            }

                            List<FrameworkUAS.Entity.DQMQue> circDQMFiles = (from cf in circSourceFiles
                                                                                join d in files on cf.SourceFileID equals d.SourceFileId
                                                                                select d).ToList();

                            List<FrameworkUAS.Entity.DQMQue> otherDQMFiles = (from f in files
                                                                                where !(from c in circSourceFiles
                                                                                        select c.SourceFileID).Contains(f.SourceFileId)
                                                                                select f).ToList();
                            #endregion

                            int processed = 1;
                            int totalFiles = files.Count;
                            if (totalFiles > 0)
                            {
                                #region Circ
                                foreach (FrameworkUAS.Entity.DQMQue f in circDQMFiles)
                                {
                                    try
                                    {
                                        #region Process Circ Files
                                        elWrk.SaveEngineLog("DQM Processing Circ file " + processed + " of " + totalFiles, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.SQM, engineLog);
                                        bool processFile = true;
                                        FrameworkUAS.Entity.SourceFile sourceFile = new FrameworkUAS.Entity.SourceFile();
                                        if (clientAdditionalProperties != null && clientAdditionalProperties.ContainsKey(client.ClientID))
                                        {
                                            sourceFile = sf.SelectSourceFileID(f.SourceFileId, true);
                                        }
                                        else
                                        {
                                            FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                                            FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                                            cap = capWork.SetObjects(client.ClientID, false);
                                            sourceFile = sf.SelectSourceFileID(f.SourceFileId, true);
                                        }

                                        if (sourceFile == null)
                                        {
                                            //Attempt to get new list in case engine was on and didn't update before running a new mapped file
                                            FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
                                            if (clientAdditionalProperties != null && clientAdditionalProperties.ContainsKey(client.ClientID))
                                            {
                                                clientAdditionalProperties[client.ClientID].SourceFilesList = sfData.Select(client.ClientID, true).Where(x => x.IsDeleted == false).ToList();
                                                sourceFile = sf.SelectSourceFileID(f.SourceFileId, true);
                                            }
                                            else
                                            {
                                                FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                                                FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                                                cap = capWork.SetObjects(client.ClientID, false);
                                                sourceFile = sf.SelectSourceFileID(f.SourceFileId, true);
                                            }
                                            if (sourceFile == null)
                                            {
                                                elWrk.SaveEngineLog("Source file was null - ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                                                continue;
                                            }
                                        }

                                        FrameworkUAD_Lookup.Entity.Code dbft = databaseFileTypes.SingleOrDefault(x => x.CodeId == sourceFile.DatabaseFileTypeId);
                                        FrameworkUAD_Lookup.Enums.FileTypes dft = FrameworkUAD_Lookup.Enums.GetDatabaseFileType(dbft.CodeName);
                                        //get PubCodes
                                        FrameworkUAD.BusinessLogic.SubscriberTransformed stWorker = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
                                        List<string> pubs = stWorker.GetDistinctPubCodes(client.ClientConnections, f.ProcessCode);

                                        if (dft == FrameworkUAD_Lookup.Enums.FileTypes.ACS ||
                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.Field_Update ||
                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_2YR ||
                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_3YR ||
                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_Other ||
                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.NCOA ||
                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.QuickFill ||
                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Long_Form ||
                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Short_Form)
                                        {
                                            foreach (string p in pubs)
                                            {
                                                FrameworkUAD.BusinessLogic.Product prodWorker = new FrameworkUAD.BusinessLogic.Product();
                                                FrameworkUAD.Entity.Product prod = prodWorker.Select(p, client.ClientConnections);

                                                if (prod != null)
                                                {
                                                    //this is for WebForms and SubGen
                                                    if (prod.AllowDataEntry == true ||
                                                        (prod.KMImportAllowed == false && prod.ClientImportAllowed == false))
                                                    {
                                                        elWrk.SaveEngineLog("Product Locked. Skipping - ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                                                        processFile = false;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    //pubCode does not exist so do not process file - will just mark as done and remove from Que
                                                    //will also log that not done
                                                    FrameworkUAS.BusinessLogic.DQMQue dqmWrk = new FrameworkUAS.BusinessLogic.DQMQue();
                                                    dqmWrk.UpdateComplete(f.ProcessCode, true, "Product code does not exist for " + p, f.SourceFileId);
                                                    processFile = false;
                                                    break;
                                                }
                                            }
                                        }
                                        else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Complimentary)
                                        {
                                            foreach (string p in pubs)
                                            {
                                                FrameworkUAD.BusinessLogic.Product prodWorker = new FrameworkUAD.BusinessLogic.Product();
                                                FrameworkUAD.Entity.Product prod = prodWorker.Select(p, client.ClientConnections);

                                                if (prod != null)
                                                {
                                                    //this is for WebForms and SubGen
                                                    if (prod.AllowDataEntry == true ||
                                                        prod.KMImportAllowed == true ||
                                                        prod.ClientImportAllowed == true ||
                                                        prod.AddRemoveAllowed == true)
                                                    {
                                                        elWrk.SaveEngineLog("Product Locked. Skipping - ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                                                        processFile = false;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    //pubCode does not exist so do not process file - will just mark as done and remove from Que
                                                    //will also log that not done
                                                    FrameworkUAS.BusinessLogic.DQMQue dqmWrk = new FrameworkUAS.BusinessLogic.DQMQue();
                                                    dqmWrk.UpdateComplete(f.ProcessCode, true, "Product code does not exist for " + p, f.SourceFileId);
                                                    processFile = false;
                                                    break;
                                                }
                                            }
                                        }
                                        else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms ||
                                                    dft == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms_Short_Form ||
                                                    dft == FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction)
                                        {
                                            foreach (string p in pubs)
                                            {
                                                FrameworkUAD.BusinessLogic.Product prodWorker = new FrameworkUAD.BusinessLogic.Product();
                                                FrameworkUAD.Entity.Product prod = prodWorker.Select(p, client.ClientConnections);

                                                if (prod != null)
                                                {
                                                    //this is for WebForms and SubGen
                                                    if (prod.AllowDataEntry == false && prod.KMImportAllowed == false && prod.ClientImportAllowed == false)
                                                    {
                                                        elWrk.SaveEngineLog("Product Locked. Skipping - ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                                                        processFile = false;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    //pubCode does not exist so do not process file - will just mark as done and remove from Que
                                                    //will also log that not done
                                                    FrameworkUAS.BusinessLogic.DQMQue dqmWrk = new FrameworkUAS.BusinessLogic.DQMQue();
                                                    dqmWrk.UpdateComplete(f.ProcessCode, true, "Product code does not exist for " + p, f.SourceFileId);
                                                    processFile = false;
                                                    break;
                                                }
                                            }
                                        }

                                        if (processFile)
                                        {
                                            f.IsQued = true;
                                            f.DateQued = DateTime.Now;
                                            dqmQueWorker.Save(f);

                                            ProcessFile(client, f, sourceFile);
                                            processed++;

                                            elWrk.SaveEngineLog("Finished DQM for Circ file " + client.FtpFolder + " " + DateTime.Now.TimeOfDay.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                                        }
                                        else
                                        {
                                            elWrk.SaveEngineLog("Skipped DQM for Circ file " + client.FtpFolder + " " + DateTime.Now.TimeOfDay.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                                        }
                                        #endregion
                                    }
                                    catch (Exception ex)
                                    {
                                        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
                                        if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                                            app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                                        KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                                        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                        alWorker.LogCriticalError(formatException, "ADMS.ADMSProcessingQue.RunDQMForClient_1", app, string.Empty);
                                    }
                                }
                                #endregion
                                #region Other
                                foreach (FrameworkUAS.Entity.DQMQue f in otherDQMFiles)
                                {
                                    try
                                    {
                                        #region Check for any new circ files
                                        List<FrameworkUAS.Entity.DQMQue> newFiles = new List<FrameworkUAS.Entity.DQMQue>();
                                        var dqmQueList = dqmQueWorker.Select(client.ClientID, isDemo, isADMS).Where(x => x.IsCompleted == false && x.IsQued == false).ToList(); ;
                                        if (dqmQueList != null && dqmQueList.Count > 0)
                                        {
                                            newFiles = dqmQueList.Where(x => x.IsCompleted == false && x.IsQued == false).ToList();
                                            List<FrameworkUAS.Entity.SourceFile> newCircSourceFiles = new List<FrameworkUAS.Entity.SourceFile>();
                                            try
                                            {
                                                if (clientAdditionalProperties != null && clientAdditionalProperties.ContainsKey(client.ClientID))
                                                {
                                                    newCircSourceFiles = clientAdditionalProperties[client.ClientID].SourceFilesList.Where(x => x.ServiceID == circ).ToList();
                                                }
                                                else
                                                {
                                                    FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                                                    FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                                                    cap = capWork.SetObjects(client.ClientID, false);
                                                    newCircSourceFiles = cap.SourceFilesList.Where(x => x.ServiceID == circ).ToList();
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
                                                if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                                                    app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                                                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                                                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                                alWorker.LogCriticalError(formatException, "ADMS.ADMSProcessingQue.OtherCircCheck", app, string.Empty);
                                            }

                                            List<FrameworkUAS.Entity.DQMQue> newCircDQMFiles = (from cf in newCircSourceFiles
                                                                                                join d in newFiles on cf.SourceFileID equals d.SourceFileId
                                                                                                select d).ToList();

                                            if (newCircDQMFiles.Count > 0)
                                            {
                                                totalFiles += newCircDQMFiles.Count;

                                                foreach (FrameworkUAS.Entity.DQMQue cdf in newCircDQMFiles)
                                                {
                                                    #region Process Circ Files
                                                    elWrk.SaveEngineLog("DQM Processing Circ file " + processed + " of " + totalFiles, client.ClientID, engineEnum, engineLog);
                                                    bool processFile = true;
                                                    FrameworkUAS.Entity.SourceFile newSourceFile = new FrameworkUAS.Entity.SourceFile();
                                                    if (clientAdditionalProperties != null && clientAdditionalProperties.ContainsKey(client.ClientID))
                                                    {
                                                        newSourceFile = sf.SelectSourceFileID(cdf.SourceFileId, true);
                                                    }
                                                    else
                                                    {
                                                        FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                                                        FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                                                        cap = capWork.SetObjects(client.ClientID, false);
                                                        newSourceFile = sf.SelectSourceFileID(cdf.SourceFileId, true);
                                                    }

                                                    if (newSourceFile == null)
                                                    {
                                                        elWrk.SaveEngineLog("Source file was null - ProcessCode: " + cdf.ProcessCode, client.ClientID, engineEnum, engineLog);
                                                        continue;
                                                    }

                                                    FrameworkUAD_Lookup.Entity.Code dbft = databaseFileTypes.SingleOrDefault(x => x.CodeId == newSourceFile.DatabaseFileTypeId);
                                                    FrameworkUAD_Lookup.Enums.FileTypes dft = FrameworkUAD_Lookup.Enums.GetDatabaseFileType(dbft.CodeName);
                                                    //get PubCodes
                                                    FrameworkUAD.BusinessLogic.SubscriberTransformed stWorker = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
                                                    List<string> pubs = stWorker.GetDistinctPubCodes(client.ClientConnections, cdf.ProcessCode);

                                                    if (dft == FrameworkUAD_Lookup.Enums.FileTypes.ACS ||
                                                        dft == FrameworkUAD_Lookup.Enums.FileTypes.Field_Update ||
                                                        dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_2YR ||
                                                        dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_3YR ||
                                                        dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_Other ||
                                                        dft == FrameworkUAD_Lookup.Enums.FileTypes.NCOA ||
                                                        dft == FrameworkUAD_Lookup.Enums.FileTypes.QuickFill ||
                                                        dft == FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Long_Form ||
                                                        dft == FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Short_Form)
                                                    {
                                                        foreach (string p in pubs)
                                                        {
                                                            FrameworkUAD.BusinessLogic.Product prodWorker = new FrameworkUAD.BusinessLogic.Product();
                                                            FrameworkUAD.Entity.Product prod = prodWorker.Select(p, client.ClientConnections);

                                                            if (prod != null)
                                                            {
                                                                //this is for WebForms and SubGen
                                                                if (prod.AllowDataEntry == true ||
                                                                    (prod.KMImportAllowed == false && prod.ClientImportAllowed == false))
                                                                {
                                                                    elWrk.SaveEngineLog("Product Locked. Skipping - ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                                                                    processFile = false;
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //pubCode does not exist so do not process file - will just mark as done and remove from Que
                                                                //will also log that not done
                                                                FrameworkUAS.BusinessLogic.DQMQue dqmWrk = new FrameworkUAS.BusinessLogic.DQMQue();
                                                                dqmWrk.UpdateComplete(f.ProcessCode, true, "Product code does not exist for " + p, f.SourceFileId);
                                                                processFile = false;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Complimentary)
                                                    {
                                                        foreach (string p in pubs)
                                                        {
                                                            FrameworkUAD.BusinessLogic.Product prodWorker = new FrameworkUAD.BusinessLogic.Product();
                                                            FrameworkUAD.Entity.Product prod = prodWorker.Select(p, client.ClientConnections);

                                                            if (prod != null)
                                                            {
                                                                //this is for WebForms and SubGen
                                                                if (prod.AllowDataEntry == true ||
                                                                    prod.KMImportAllowed == true ||
                                                                    prod.ClientImportAllowed == true ||
                                                                    prod.AddRemoveAllowed == true)
                                                                {
                                                                    elWrk.SaveEngineLog("Product Locked. Skipping - ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                                                                    processFile = false;
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //pubCode does not exist so do not process file - will just mark as done and remove from Que
                                                                //will also log that not done
                                                                FrameworkUAS.BusinessLogic.DQMQue dqmWrk = new FrameworkUAS.BusinessLogic.DQMQue();
                                                                dqmWrk.UpdateComplete(f.ProcessCode, true, "Product code does not exist for " + p, f.SourceFileId);
                                                                processFile = false;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms ||
                                                                dft == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms_Short_Form ||
                                                                dft == FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction)
                                                    {
                                                        foreach (string p in pubs)
                                                        {
                                                            FrameworkUAD.BusinessLogic.Product prodWorker = new FrameworkUAD.BusinessLogic.Product();
                                                            FrameworkUAD.Entity.Product prod = prodWorker.Select(p, client.ClientConnections);

                                                            if (prod != null)
                                                            {
                                                                //this is for WebForms and SubGen
                                                                if (prod.AllowDataEntry == false && prod.KMImportAllowed == false && prod.ClientImportAllowed == false)
                                                                {
                                                                    elWrk.SaveEngineLog("Product Locked. Skipping - ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                                                                    processFile = false;
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //pubCode does not exist so do not process file - will just mark as done and remove from Que
                                                                //will also log that not done
                                                                FrameworkUAS.BusinessLogic.DQMQue dqmWrk = new FrameworkUAS.BusinessLogic.DQMQue();
                                                                dqmWrk.UpdateComplete(f.ProcessCode, true, "Product code does not exist for " + p, f.SourceFileId);
                                                                processFile = false;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (processFile)
                                                    {
                                                        cdf.IsQued = true;
                                                        cdf.DateQued = DateTime.Now;
                                                        dqmQueWorker.Save(cdf);

                                                        ProcessFile(client, cdf, newSourceFile);
                                                        processed++;

                                                        elWrk.SaveEngineLog("Finished DQM for Circ file " + client.FtpFolder + " " + DateTime.Now.TimeOfDay.ToString() + " ProcessCode: " + cdf.ProcessCode, client.ClientID, engineEnum, engineLog);
                                                    }
                                                    else
                                                    {
                                                        elWrk.SaveEngineLog("Skipped DQM for Circ file " + client.FtpFolder + " " + DateTime.Now.TimeOfDay.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                                                    }
                                                    #endregion
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Process Other Files
                                        elWrk.SaveEngineLog("DQM Processing " + processed + " of " + totalFiles, client.ClientID, engineEnum, engineLog);
                                        elWrk.SaveEngineLog("Get source file - ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                                        FrameworkUAS.Entity.SourceFile sourceFile = new FrameworkUAS.Entity.SourceFile();
                                        if (clientAdditionalProperties != null && clientAdditionalProperties.ContainsKey(client.ClientID))
                                        {
                                            sourceFile = sf.SelectSourceFileID(f.SourceFileId, true);
                                        }
                                        else
                                        {
                                            FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                                            FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                                            cap = capWork.SetObjects(client.ClientID, false);
                                            sourceFile = sf.SelectSourceFileID(f.SourceFileId, true);
                                        }

                                        if (sourceFile == null)
                                        {
                                            //Attempt to get new list in case engine was on and didn't update before running a new mapped file
                                            if (clientAdditionalProperties != null && clientAdditionalProperties.ContainsKey(client.ClientID))
                                            {
                                                FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
                                                clientAdditionalProperties[client.ClientID].SourceFilesList = sfData.Select(client.ClientID, true).Where(x => x.IsDeleted == false).ToList();
                                                sourceFile = sf.SelectSourceFileID(f.SourceFileId, true);
                                            }
                                            else
                                            {
                                                FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
                                                sourceFile = sf.SelectSourceFileID(f.SourceFileId, true);
                                            }
                                            if (sourceFile == null)
                                            {
                                                elWrk.SaveEngineLog("Source file was null - ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                                                continue;
                                            }
                                        }

                                        f.IsQued = true;
                                        f.DateQued = DateTime.Now;
                                        dqmQueWorker.Save(f);
                                        ProcessFile(client, f, sourceFile);
                                        processed++;

                                        elWrk.SaveEngineLog("Finished DQM for " + client.FtpFolder + " " + DateTime.Now.TimeOfDay.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                                        #endregion
                                    }
                                    catch (Exception ex)
                                    {
                                        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
                                        if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                                            app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                                        KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                                        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                        alWorker.LogCriticalError(formatException, "ADMS.ADMSProcessingQue.RunDQMForClient_2", app, string.Empty);
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.Service_Que_Monitor;
                        KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                        alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".DQMQueMonitor", app, string.Empty, client.ClientID);
                    }
                    finally
                    {
                        //pause = false;
                    }                    
                    System.Threading.Thread.Sleep(timeout);//time in milliseconds
                    elWrk.UpdateIsRunning(engineLog.EngineLogId, true);
                    elWrk.UpdateRefresh(engineLog.EngineLogId);
                }
                catch (Exception ex)
                {
                    KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.Service_Que_Monitor;
                    KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                    string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".DQMQueMonitor", app, string.Empty, client.ClientID);
                }
            }
        }

        private void ProcessFile(KMPlatform.Entity.Client client, FrameworkUAS.Entity.DQMQue f, FrameworkUAS.Entity.SourceFile sourceFile)
        {
            bool isADMS = false;
            FrameworkUAS.BusinessLogic.Enums.Engine engineEnum = FrameworkUAS.BusinessLogic.Enums.Engine.SQM;
            bool.TryParse(ConfigurationManager.AppSettings["IsADMS"].ToString(), out isADMS);
            if (isADMS)
                engineEnum = FrameworkUAS.BusinessLogic.Enums.Engine.ADMS_DQM;

            engineLog = elWrk.Select(client.ClientID, engineEnum.ToString());
            ADMS.Services.DataCleanser.DQMCleaner dqm = new ADMS.Services.DataCleanser.DQMCleaner();
            elWrk.SaveEngineLog("Starting DQM for " + client.FtpFolder + " " + DateTime.Now.TimeOfDay.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);

            processCode = f.ProcessCode;
            Core.ADMS.Events.FileAddressGeocoded eventMessage = new Core.ADMS.Events.FileAddressGeocoded(new System.IO.FileInfo(sourceFile.FileName), client, true, true,
                                                                true, sourceFile, admsWrk.Select(f.ProcessCode), new FrameworkUAD.Object.ValidationResult());

            //ADMSDQMReadyQue item = new ADMSDQMReadyQue();
            //item = readyForDQMFiles.FirstOrDefault(x => x.EventMessage.AdmsLog.ProcessCode == f.ProcessCode);
            if (eventMessage != null && eventMessage.AdmsLog != null)
                admsLog = eventMessage.AdmsLog;//readyForDQMFiles.FirstOrDefault(x => x.EventMessage.AdmsLog.ProcessCode == f.ProcessCode).EventMessage.AdmsLog;
            if (admsLog == null)
            {
                admsLog = new FrameworkUAS.Entity.AdmsLog();
                admsLog.ProcessCode = f.ProcessCode;
                admsLog.ClientId = client.ClientID;
                admsLog.DateCreated = DateTime.Now;
                admsLog.DQM = f;
                admsLog.SourceFileId = sourceFile.SourceFileID;
                FrameworkUAS.BusinessLogic.AdmsLog aWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
                admsLog.AdmsLogId = aWrk.Save(admsLog);
            }


            try
            {
                if (eventMessage != null)
                {
                    elWrk.SaveEngineLog("Start RunStandardization item != null: " + DateTime.Now.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                    dqm.RunStandardization(client, admsLog, sourceFile);
                    elWrk.SaveEngineLog("End RunStandardization item != null: " + DateTime.Now.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                    try
                    {
                        if (admsLog.RecordSource != "API")
                        {
                            ADMS.Services.Emailer.Emailer email = new ADMS.Services.Emailer.Emailer();

                            System.IO.FileInfo thisFileInfo = eventMessage.ImportFile;
                            FrameworkUAD.Object.ValidationResult vr = eventMessage.ValidationResult;
                            Core.ADMS.Events.FileProcessed thisProcessed = new Core.ADMS.Events.FileProcessed(client, sourceFile.SourceFileID, admsLog, thisFileInfo,
                                                                eventMessage.IsKnownCustomerFileName, eventMessage.IsValidFileType, eventMessage.IsFileSchemaValid, vr);
                            email.SecondaryHandleFileProcessed(thisProcessed);
                        }
                    }
                    catch (Exception ex)
                    {                        
                        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.Service_Que_Monitor;
                        KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                        alWorker.LogCriticalError(formatException, "ServiceQueMonitor.DQMQueMonitor.ProcessFile_1", app, string.Empty, client.ClientID);
                    }
                }
                else
                {
                    elWrk.SaveEngineLog("Start RunStandardization item null: " + DateTime.Now.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                    dqm.RunStandardization(client, admsLog, sourceFile);
                    elWrk.SaveEngineLog("Start RunStandardization item null: " + DateTime.Now.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, engineEnum, engineLog);
                    try
                    {
                        if (admsLog.RecordSource != "API")
                        {
                            ADMS.Services.Emailer.Emailer email = new ADMS.Services.Emailer.Emailer();
                            string filePath = Core.ADMS.BaseDirs.getClientRepoDir() + "//" + client.FtpFolder + "//" + sourceFile.FileName + sourceFile.Extension;
                            System.IO.FileInfo thisFileInfo = new System.IO.FileInfo(filePath);//sourceFile.FileName + sourceFile.Extension
                            FrameworkUAD.Object.ValidationResult vr = new FrameworkUAD.Object.ValidationResult(thisFileInfo, sourceFile.SourceFileID, f.ProcessCode);
                            Core.ADMS.Events.FileProcessed thisProcessed = new Core.ADMS.Events.FileProcessed(client, sourceFile.SourceFileID, admsLog, thisFileInfo,
                                                                true, true, true, vr);
                            email.SecondaryHandleFileProcessed(thisProcessed);
                        }
                    }
                    catch (Exception ex)
                    {
                        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.Service_Que_Monitor;
                        KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                        alWorker.LogCriticalError(formatException, "ServiceQueMonitor.DQMQueMonitor.ProcessFile_2", app, string.Empty, client.ClientID);
                    }
                }
            }
            catch (Exception ex)
            {
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.Service_Que_Monitor;
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, "ServiceQueMonitor.DQMQueMonitor.ProcessFile_3", app, string.Empty, client.ClientID);
            }

            //UPDATE RECORD IN DB TO BE COMPLETED
            f.IsCompleted = true;
            f.DateCompleted = DateTime.Now;
            FrameworkUAS.BusinessLogic.DQMQue dqmWorker = new FrameworkUAS.BusinessLogic.DQMQue();
            dqmWorker.Save(f);

            admsLog = eventMessage.AdmsLog;
            FrameworkUAS.BusinessLogic.AdmsLog alWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
            alWrk.Update(admsLog.ProcessCode,
                         FrameworkUAD_Lookup.Enums.FileStatusType.Completed,
                         FrameworkUAD_Lookup.Enums.ADMS_StepType.Processed,
                         FrameworkUAD_Lookup.Enums.ProcessingStatusType.Completed,
                         FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_DQM, 1, "Remove file from DQM Que", true,
                         admsLog.SourceFileId);
        }        

        void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.Service_Que_Monitor;
            KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
            Exception ex = (Exception)e.ExceptionObject;
            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            Console.Write(formatException);
            alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".DQMQueMonitor", app);

            //RestartEngine(sender, EventArgs.Empty);
        }

        void EmailException(Exception ex, string msg = "")
        {
            MailMessage message = new MailMessage();
            message.Subject = ConfigurationManager.AppSettings["EngineServer"].ToString() + " - EXCEPTION";
            message.To.Add(Core.ADMS.Settings.AdminEmail);
            message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
            message.IsBodyHtml = false;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(msg);
            sb.AppendLine(string.Empty);
            sb.AppendLine(Core_AMS.Utilities.StringFunctions.FormatException(ex));
            message.Body = sb.ToString();
            string userState = "Message";
            SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);

            smtp.SendAsync(message, userState);
        }


        #region depricated restart code - now run as scheduled task
        //void RestartEngine(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        System.Diagnostics.Process.Start("ServiceQueMonitor.exe");
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        Console.Write(msg);
        //    }
        //}
        //void WireUpRestart()
        //{
        //    var domain = AppDomain.CurrentDomain;
        //    domain.ProcessExit += new EventHandler(RestartEngine);
        //    SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
        //}
        //[DllImport("Kernel32")]
        //public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);
        //// A delegate type to be used as the handler routine for SetConsoleCtrlHandler.
        //public delegate bool HandlerRoutine(CtrlTypes CtrlType);
        //// An enumerated type for the control messages sent to the handler routine.
        //public enum CtrlTypes
        //{
        //    CTRL_C_EVENT = 0,
        //    CTRL_BREAK_EVENT,
        //    CTRL_CLOSE_EVENT,
        //    CTRL_LOGOFF_EVENT = 5,
        //    CTRL_SHUTDOWN_EVENT
        //}
        //private bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        //{
        //    try
        //    {
        //        // Put your own handler here
        //        MailMessage message = new MailMessage();
        //        message.To.Add(Core.ADMS.Settings.AdminEmail);
        //        string subject = "ServiceQueMonitor - closed on server: " + ConfigurationManager.AppSettings["EngineServer"].ToString();

        //        message.Subject = subject;
        //        message.From = new MailAddress(Core.ADMS.Settings.EmailFrom);
        //        message.Priority = MailPriority.High;
        //        message.Body = "ServiceQueMonitor - closed unexpectedly on server: " + ConfigurationManager.AppSettings["EngineServer"].ToString() + ". Please check that it has restarted correctly.";

        //        SmtpClient smtp = new SmtpClient(Core.ADMS.Settings.SMTP);

        //        smtp.Send(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        //        Console.Write(msg);
        //    }
        //    return true;
        //}
        #endregion
    }
}
