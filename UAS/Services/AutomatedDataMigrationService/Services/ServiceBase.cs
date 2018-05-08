using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMS.Services
{
    public class ServiceBase
    {
        #region variables
        public readonly string engine = "ADMS";
        public readonly FrameworkUAS.BusinessLogic.EngineLog elWrk = new FrameworkUAS.BusinessLogic.EngineLog();
        public readonly FrameworkUAS.BusinessLogic.SourceFile sfWrk = new FrameworkUAS.BusinessLogic.SourceFile();
        public readonly KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
        public readonly KMPlatform.BusinessLogic.ApplicationLog appLogWorker = new KMPlatform.BusinessLogic.ApplicationLog();
        public readonly FrameworkUAS.BusinessLogic.AdmsLog admsWrk = new FrameworkUAS.BusinessLogic.AdmsLog();

        //private FrameworkUAS.Entity.AdmsLog _admsLog;
        //public FrameworkUAS.Entity.AdmsLog admsLog
        //{
        //    get
        //    {
        //        if (_admsLog == null)
        //            _admsLog = new FrameworkUAS.Entity.AdmsLog();
        //        return _admsLog;
        //    }
        //    set
        //    {
        //        _admsLog = value;
        //    }
        //}


        /// <summary>
        /// engineLog will set itself
        /// </summary>
        private FrameworkUAS.Entity.EngineLog _engineLog;
        public FrameworkUAS.Entity.EngineLog engineLog
        {
            get
            {
                if (_engineLog == null)
                    if (client != null && client.ClientID > 0)
                        _engineLog = elWrk.Select(client.ClientID, engine);
                return _engineLog;
            }
            set
            {
                _engineLog = value;
            }
        }
        /// <summary>
        /// client will set itself from the app.config
        /// </summary>
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
        //private FrameworkUAS.Entity.SourceFile _sourceFile;
        //public FrameworkUAS.Entity.SourceFile sourceFile
        //{
        //    get
        //    {
        //        if (_sourceFile == null)
        //            if (admsLog != null && admsLog.SourceFileId > 0)
        //                if (BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Exists(x => x.SourceFileID == admsLog.SourceFileId))
        //                    _sourceFile = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.SingleOrDefault(x => x.SourceFileID == admsLog.SourceFileId);
        //                else
        //                    _sourceFile = sfWrk.SelectSourceFileID(admsLog.SourceFileId, true);
        //        return _sourceFile;
        //    }
        //    set
        //    {
        //        _sourceFile = value;
        //    }
        //}
        //private FrameworkUAD_Lookup.Entity.Code _dbFileType;
        //public FrameworkUAD_Lookup.Entity.Code dbFileType
        //{
        //    get
        //    {
        //        if (_dbFileType == null)
        //        {
        //            FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
        //            if (sourceFile != null)
        //            {
        //                _dbFileType = cWrk.SelectCodeId(sourceFile.DatabaseFileTypeId);
        //                _fileType = FrameworkUAS.BusinessLogic.Enums.GetDatabaseFileType(dbFileType.CodeName);
        //            }
        //        }
        //        return _dbFileType;
        //    }
        //    set
        //    {
        //        _dbFileType = value;
        //    }
        //}
        //private FrameworkUAS.BusinessLogic.Enums.FileTypes _fileType;
        //public FrameworkUAS.BusinessLogic.Enums.FileTypes fileType
        //{
        //    get
        //    {
        //        return _fileType;
        //    }
        //    set
        //    {
        //        _fileType = value;
        //    }
        //}
        private Dictionary<int, string> _clientPubCodes;
        public Dictionary<int, string> clientPubCodes
        {
            get
            {
                if(_clientPubCodes ==  null)
                    _clientPubCodes = FrameworkUAS.Object.DBWorker.GetPubIDAndCodesByClient(client);

                return _clientPubCodes;
            }
            set
            {
                _clientPubCodes = FrameworkUAS.Object.DBWorker.GetPubIDAndCodesByClient(client);
            }
        }
        private bool? _logTranDetail;
        public bool logTranDetail
        {
            get
            {
                if (_logTranDetail == null)
                {
                    bool check = false;
                    if (System.Configuration.ConfigurationManager.AppSettings["LogTransformationDetail"] != null)
                        bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["LogTransformationDetail"].ToString(), out check);
                    _logTranDetail = check;
                }
                return _logTranDetail.Value;
            }
            set { _logTranDetail = value; }
        }
        #endregion

        public void SetStaticProperties(Core.ADMS.Events.FileDetected fd)
        {
            client = fd.Client;
            //admsLog = fd.AdmsLog;
            //sourceFile = fd.SourceFile;
            //fileType = FrameworkUAS.BusinessLogic.Enums.GetDatabaseFileType(dbFileType.CodeName);
        }
        public void SetPropertiesNull()
        {
            //admsLog = null;
            //sourceFile = null;
            //dbFileType = null;
        }
        public void ConsoleMessage(FrameworkUAS.Entity.AdmsLog admsLog, string message, string processCode = "", bool createLog = true, int sourceFileId = 0, int updatedByUserId = 0)
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
        public void ConsoleMessage(string message, string processCode = "", bool createLog = true, int sourceFileId = 0, int updatedByUserId = 0)
        {
            if (string.IsNullOrEmpty(processCode))
                return;
            int fileStatusTypeId = 0;
            admsWrk.UpdateStatusMessage(processCode, message, updatedByUserId, createLog, sourceFileId, fileStatusTypeId);
        }
        public void SaveEngineLog(string msg)
        {
            elWrk.SaveEngineLog(msg, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS);
        }
        public void LogError(Exception ex, KMPlatform.Entity.Client client, string msg, FrameworkUAS.Entity.AdmsLog admsLog, bool removeThread = true, bool removeQue = true)
        {
            #region Log Error
            if (admsLog != null)
            {
                if (removeThread)
                    ThreadDictionary.Remove(admsLog.ThreadId);
                //if (removeQue)
                //    ADMSProcessingQue.RemoveClientFile(client, admsLog.ImportFile);
            }
            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            //ConsoleMessage(formatException);

            StringBuilder sbDetail = new StringBuilder();
            sbDetail.AppendLine("Client: " + client.FtpFolder);
            if (admsLog != null)
            {
                if (admsLog.ImportFile != null)
                    sbDetail.AppendLine("File: " + admsLog.ImportFile.FullName);
                sbDetail.AppendLine("SourceFileID: " + admsLog.SourceFileId.ToString());
                sbDetail.AppendLine("Thread: " + admsLog.ThreadId.ToString());
                sbDetail.AppendLine("ProcessCode: " + admsLog.ProcessCode.ToString());
            }
            sbDetail.AppendLine(msg);

            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
            if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
            appLogWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".LogError", app, sbDetail.ToString());
            #endregion
        }
        public void LogError(Exception ex, KMPlatform.Entity.Client client, string msg, bool removeThread = true, bool removeQue = true)
        {
            #region Log Error
            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            //ConsoleMessage(formatException);

            StringBuilder sbDetail = new StringBuilder();
            sbDetail.AppendLine("Client: " + client.FtpFolder);

            sbDetail.AppendLine(msg);

            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
            if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
            appLogWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".LogError", app, sbDetail.ToString());
            #endregion
        }
        public void UnhandledExceptionLog(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception) e.ExceptionObject;
            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
            if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
            appLogWorker.LogCriticalError(formatException, "UnhandledException caught in ADMS.ServiceBase - ", app);
            //restart the application - task scheduler should restart
            //System.Diagnostics.Process.Start("ADMS_Engine.exe");
            //Environment.Exit(1);
        }

        #region RuleSets / Rules
        public bool ExecutionPointExist(FrameworkUAD_Lookup.Enums.ExecutionPointType ept, FrameworkUAS.Entity.SourceFile sourceFile)
        {
            if (sourceFile.RuleSets.ToList().Exists(x => x.ExecutionPointEnum == ept))
                return true;
            else
                return false;
        }
        public bool HasDateSpecificRuleSet(HashSet<FrameworkUAS.Object.RuleSet> ruleSets)
        {
            bool isDateSpecific = false;
            foreach (var rs in ruleSets)
            {
                if (rs.IsDateSpecific == true && rs.StartMonth.HasValue == true && rs.StartDay.HasValue == true)
                {
                    isDateSpecific = true;
                    break;
                }
            }
            return isDateSpecific;
        }
        public HashSet<FrameworkUAS.Object.RuleSet> GetCurrentDateSpecificRuleSet(HashSet<FrameworkUAS.Object.RuleSet> ruleSets)
        {
            var dateRuleSets = new HashSet<FrameworkUAS.Object.RuleSet>(ruleSets.Where(x => x.StartMonth <= DateTime.Now.Month && x.EndMonth >= DateTime.Now.Month
                                                                                                    && x.StartDay <= DateTime.Now.Day && x.EndDay >= DateTime.Now.Day
                                                                                                    && (x.StartYear == 0 || (x.StartYear <= DateTime.Now.Year && x.EndYear >= DateTime.Now.Year))).OrderBy(o => o.ExecutionOrder).ToList());
            return dateRuleSets;
        }
        public bool HasCurrentDateSpecificRuleSet(HashSet<FrameworkUAS.Object.RuleSet> ruleSets)
        {
            var dateRuleSets = GetCurrentDateSpecificRuleSet(ruleSets);
            if (dateRuleSets.Count > 0)
                return true;
            else
                return false;
        }
        public HashSet<FrameworkUAS.Object.RuleSet> GetRuleSetsByExecutionPoint(FrameworkUAD_Lookup.Enums.ExecutionPointType ept, FrameworkUAS.Entity.SourceFile sourceFile)
        {
            return new HashSet<FrameworkUAS.Object.RuleSet>(sourceFile.RuleSets.Where(x => x.ExecutionPointEnum == ept).OrderBy(o => o.ExecutionOrder).ToList());
        }
        public DateTime? GetQDate(FrameworkUAS.Entity.SourceFile sourceFile)
        {
            DateTime? dt = DateTime.Now;
            if (sourceFile.FileTypeEnum == FrameworkUAD_Lookup.Enums.FileTypes.Field_Update)
                dt = null;
            else
            {
                #region RuleSet - QDateDefault
                //set the Qdate default to the RuleValue "option" that client has configured
                //should always check for IsDateSpecific
                //will not support WhereClause so ignore
                //QDate lets you chose 1 of 5 options
                //if is date specific and have multiple configured need to make sure to process what falls in date range and skip rest
                //example:  2 QDate rules - the system one and client has a configured one for 11/1 to 11/30
                //          if todays date is between 11/1 and 11/30 execute that rule else execute other rule
                //          here the order could be both at 1 or maybe 1,2
                var ruleSets = GetRuleSetsByExecutionPoint(FrameworkUAD_Lookup.Enums.ExecutionPointType.QDateDefault, sourceFile);
                var currentDateRS = GetCurrentDateSpecificRuleSet(ruleSets);
                if (currentDateRS.Count > 0)
                {
                    foreach (var rs in currentDateRS)
                    {
                        foreach (var r in rs.Rules.OrderBy(x => x.RuleOrder))
                        {
                            switch (r.RuleName)
                            {
                                case "CurrentDate":
                                    dt = DateTime.Now;
                                    break;
                                case "FirstOfCurrentMonth":
                                    dt = DateTime.Parse(DateTime.Now.Month.ToString() + "/1/" + DateTime.Now.Year.ToString());
                                    //myRow[qDateColumnHeader] = dt.ToShortDateString();
                                    break;
                                case "FirstOfPreviousMonth":
                                    dt = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString() + "/1/" + DateTime.Now.Year.ToString());
                                    //myRow[qDateColumnHeader] = dt.ToShortDateString();
                                    break;
                                case "FirstOfNextMonth":
                                    dt = DateTime.Parse(DateTime.Now.AddMonths(1).ToString() + "/1/" + DateTime.Now.Year.ToString());
                                    //myRow[qDateColumnHeader] = dt.ToShortDateString();
                                    break;
                                case "Blank":
                                    dt = null;
                                    //myRow[qDateColumnHeader] = string.Empty;
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var rs in ruleSets)
                    {
                        foreach (var r in rs.Rules.OrderBy(x => x.RuleOrder))
                        {
                            switch (r.RuleName)
                            {
                                case "CurrentDate":
                                    dt = DateTime.Now;
                                    //myRow[qDateColumnHeader] = dt.ToShortDateString();
                                    break;
                                case "FirstOfCurrentMonth":
                                    dt = DateTime.Parse(DateTime.Now.Month.ToString() + "/1/" + DateTime.Now.Year.ToString());
                                    //myRow[qDateColumnHeader] = dt.ToShortDateString();
                                    break;
                                case "FirstOfPreviousMonth":
                                    dt = DateTime.Parse(DateTime.Now.AddMonths(-1).ToString() + "/1/" + DateTime.Now.Year.ToString());
                                    //myRow[qDateColumnHeader] = dt.ToShortDateString();
                                    break;
                                case "FirstOfNextMonth":
                                    dt = DateTime.Parse(DateTime.Now.AddMonths(1).ToString() + "/1/" + DateTime.Now.Year.ToString());
                                    //myRow[qDateColumnHeader] = dt.ToShortDateString();
                                    break;
                                case "Blank":
                                    dt = null;
                                    //myRow[qDateColumnHeader] = string.Empty;
                                    break;
                            }
                        }
                    }
                }
                //original line: myRow[qDateColumnHeader] = DateTime.Now.ToShortDateString();
                #endregion
            }
            return dt;
        }
        public HashSet<FrameworkUAD.Entity.SubscriberTransformed> GetValidEmail(HashSet<FrameworkUAD.Entity.SubscriberTransformed> listPubCodeValid, FrameworkUAS.Entity.SourceFile sourceFile)
        {
            HashSet<FrameworkUAD.Entity.SubscriberTransformed> listEmailValid = new HashSet<FrameworkUAD.Entity.SubscriberTransformed>();
            var ruleSets = GetRuleSetsByExecutionPoint(FrameworkUAD_Lookup.Enums.ExecutionPointType.RequireValidEmail, sourceFile);
            var currentDateRS = GetCurrentDateSpecificRuleSet(ruleSets);
            if (currentDateRS.Count > 0)
            {
                foreach (var rs in currentDateRS)
                {
                    foreach (var r in rs.Rules.OrderBy(x => x.RuleOrder))
                    {
                        switch (r.RuleName)
                        {
                            case "RequireValidEmail":
                                FrameworkUAS.Entity.RuleValue rv = r.RuleValues.SingleOrDefault(x => x.DisplayValue.Equals("Email RegEx"));
                                if (rv != null)
                                {
                                    if (!string.IsNullOrEmpty(rv.Value))
                                        listPubCodeValid.Where(x => !string.IsNullOrEmpty(x.Email) && Core_AMS.Utilities.StringFunctions.isEmail(x.Email, rv.Value)).ToList().ForEach(x => { listEmailValid.Add(x); });
                                    else
                                        listPubCodeValid.Where(x => !string.IsNullOrEmpty(x.Email) && Core_AMS.Utilities.StringFunctions.isEmail(x.Email)).ToList().ForEach(x => { listEmailValid.Add(x); });
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                foreach (var rs in ruleSets)
                {
                    foreach (var r in rs.Rules.OrderBy(x => x.RuleOrder))
                    {
                        switch (r.RuleName)
                        {
                            case "RequireValidEmail":
                                FrameworkUAS.Entity.RuleValue rv = r.RuleValues.SingleOrDefault(x => x.DisplayValue.Equals("Email RegEx"));
                                if (rv != null)
                                {
                                    if (!string.IsNullOrEmpty(rv.Value))
                                        listPubCodeValid.Where(x => !string.IsNullOrEmpty(x.Email) && Core_AMS.Utilities.StringFunctions.isEmail(x.Email, rv.Value)).ToList().ForEach(x => { listEmailValid.Add(x); });
                                    else
                                        listPubCodeValid.Where(x => !string.IsNullOrEmpty(x.Email) && Core_AMS.Utilities.StringFunctions.isEmail(x.Email)).ToList().ForEach(x => { listEmailValid.Add(x); });
                                }
                                break;
                        }
                    }
                }
            }

            //orginal line:   listPubCodeValid.Where(x => !string.IsNullOrEmpty(x.Email) && Core_AMS.Utilities.StringFunctions.isEmail(x.Email)).ToList().ForEach(x => { listEmailValid.Add(x); });

            return listEmailValid;
        }
        #endregion

    }
}
