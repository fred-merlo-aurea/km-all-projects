using Core.ADMS.Events;
using Core_AMS.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Threading;
using System.Text;
using System.Runtime.CompilerServices;
using ADMS.Services.Emailer;
using System.Reflection;
using ADMS.Services;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.BusinessLogic;
using FrameworkUAS.DataAccess;
using FrameworkUAS.Object;
using KM.Common.Data;
using KMPlatform.Entity;
using ApplicationLog = KMPlatform.BusinessLogic.ApplicationLog;
using ClientCustomProcedure = FrameworkUAS.Entity.ClientCustomProcedure;
using ClientFTP = FrameworkUAS.Entity.ClientFTP;
using DataFunctions = KM.Common.DataFunctions;
using Enums = FrameworkUAD_Lookup.Enums;
using SourceFile = FrameworkUAS.Entity.SourceFile;

namespace ADMS.ClientMethods
{
    public class ClientSpecialCommon : ServiceBase
    {
        // Assume that names are case sensetive
        public static readonly string EqualOperation = "equal";

        private static readonly string EventSwipeDataDimGroup = "Event Swipe Data";
        private static readonly string TopCompanyDim = "TOP_COMPANY";
        private static readonly string PubCodeStandardField = "PubCode";
        private static readonly string MarketFieldName = "Market";
        protected static readonly string PubCodeFieldName = "Pubcode";
        protected static readonly string StandardFieldCompany = "Company";
        protected static readonly string ContainsOperation = "contains";
        protected static readonly string EndsWithOperation = "ends_with";
        protected static readonly string StartsWithOperation = "starts_with";
        protected static readonly string PhoneStandardField = "Phone";
        protected static readonly string FaxStandardField = "Fax";
        protected static readonly string MobileStandardField = "Mobile";
        protected static readonly string EmailStandardField = "EMAIL";
        protected static readonly string StandardFieldEmail = "Email";
        protected static readonly string CompanyStandardField = "COMPANY";
        protected static readonly string TitleFieldName = "TITLE";
        protected static readonly string TitleCodeFieldName = "TITLE_CODE";
        protected static readonly string TitleStandardField = "TITLE";

        public ImportVessel ExecuteClientSproc(string sproc)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sproc;
            DataTable dt = KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
            ImportVessel iv = new ImportVessel();
            iv.DataOriginal = dt;
            iv.DataTransformed = dt;
            iv.HasError = false;

            return iv;
        }
        public ImportFile ExecuteClientSproc_ImportFile(string sproc)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sproc;
            DataTable dt = DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
            ImportFile iv = new ImportFile();
            FrameworkUAD.BusinessLogic.ImportFile ifWorker = new FrameworkUAD.BusinessLogic.ImportFile();
            iv.DataOriginal = ifWorker.ConvertToDictionary(dt);
            iv.DataTransformed = ifWorker.ConvertToDictionary(dt);
            iv.HasError = false;

            return iv;
        }
        public ImportFile ExecuteClientMethod(FileMoved eventMessage, ImportFile data, ClientCustomProcedure ccp)
        {
            String path = "ADMS.ClientMethods." + eventMessage.Client.FtpFolder;
            Type clientClass = Type.GetType(path + ",ADMS");
            if (Enums.GetProcedureType(ccp.ProcedureType) == Enums.ProcedureTypes.NET)
            {
                if (clientClass != null)
                {
                    MethodInfo method = clientClass.GetMethod(ccp.ProcedureName, BindingFlags.Instance | BindingFlags.Public);
                    var instance = Activator.CreateInstance(clientClass);
                    if (method != null)
                    {
                        data = (ImportFile)method.Invoke(instance, new object[] { eventMessage, data });
                    }
                    else
                    {
                        ConsoleMessage("Method not found: " + ccp.ProcedureName);
                    }
                }
                else
                    ConsoleMessage("Type not found: " + path);
            }

            return data;
        }
        public void ExecuteClientCustomCode(Enums.ExecutionPointType executionPoint, Client client, List<ClientCustomProcedure> clientCustomProcList, SourceFile sourceFile, FileMoved eventMessage)
        {
            try
            {
                Code cWorker = new Code();
                List<FrameworkUAD_Lookup.Entity.Code> executionPoints = cWorker.Select(Enums.CodeType.Execution_Points);

                FrameworkUAD_Lookup.Entity.Code ep = new FrameworkUAD_Lookup.Entity.Code();
                List<ClientCustomProcedure> afterList = new List<ClientCustomProcedure>();
                ep = executionPoints.SingleOrDefault(x => x.CodeName.Equals(executionPoint.ToString().Replace("_", " ")));
                if (ep != null)
                {
                    afterList = clientCustomProcList.Where(x => x.IsActive == true && x.ExecutionPointID == ep.CodeId && x.IsForSpecialFile == false && x.ProcedureType == Enums.ProcedureTypes.SQL.ToString()).OrderBy(x => x.ExecutionOrder).ToList();
                    if (afterList.Count > 0)
                        ExecuteClientCustomStoredProcedures(afterList, sourceFile.SourceFileID, client, eventMessage);
                    //need to execute .NET custom code
                    afterList = null;
                    afterList = clientCustomProcList.Where(x => x.IsActive == true && x.ExecutionPointID == ep.CodeId && x.IsForSpecialFile == false && x.ProcedureType == Enums.ProcedureTypes.NET.ToString()).OrderBy(x => x.ExecutionOrder).ToList();
                    if (afterList.Count > 0)
                        ExecuteClientCustomMethods(afterList, client, sourceFile, eventMessage);
                }
                ep = null;
                afterList = null;
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".ExecuteClientCustomCode - ExecutionPoint: " + executionPoint.ToString());
            }
        }
        public void ExecuteClientCustomCode(Enums.ExecutionPointType executionPoint, Client client, List<ClientCustomProcedure> clientCustomProcList, SourceFile sourceFile, string processCode)
        {
            try
            {
                Code cWorker = new Code();
                List<FrameworkUAD_Lookup.Entity.Code> executionPoints = cWorker.Select(Enums.CodeType.Execution_Points);

                FrameworkUAD_Lookup.Entity.Code ep = new FrameworkUAD_Lookup.Entity.Code();
                List<ClientCustomProcedure> afterList = new List<ClientCustomProcedure>();
                ep = executionPoints.SingleOrDefault(x => x.CodeName.Equals(executionPoint.ToString().Replace("_", " ")));
                if (ep != null)
                {
                    afterList = clientCustomProcList.Where(x => x.IsActive == true && x.ExecutionPointID == ep.CodeId && x.IsForSpecialFile == false && x.ProcedureType == Enums.ProcedureTypes.SQL.ToString()).OrderBy(x => x.ExecutionOrder).ToList();
                    if (afterList.Count > 0)
                        ExecuteClientCustomStoredProcedures(afterList, sourceFile.SourceFileID, sourceFile.FileName, client, processCode);
                    //need to execute .NET custom code
                    afterList = null;
                    afterList = clientCustomProcList.Where(x => x.IsActive == true && x.ExecutionPointID == ep.CodeId && x.IsForSpecialFile == false && x.ProcedureType == Enums.ProcedureTypes.NET.ToString()).OrderBy(x => x.ExecutionOrder).ToList();
                    if (afterList.Count > 0)
                        ExecuteClientCustomMethods(afterList, client, sourceFile, processCode);
                }
                ep = null;
                afterList = null;
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".ExecuteClientCustomCode - ExecutionPoint: " + executionPoint.ToString());
            }
        }
        public void ExecuteClientCustomMethods(List<ClientCustomProcedure> list, Client client, SourceFile sourceFile, FileMoved eventMessage)
        {
            ClientCustomProcedure ccpRun = null;
            try
            {
                foreach (var p in list)
                {
                    ccpRun = p;
                    if (p.ProcedureType.Equals(Enums.ProcedureTypes.NET.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        String path = "ADMS.ClientMethods." + client.FtpFolder;
                        Type clientClass = Type.GetType(path + ",ADMS");
                        if (clientClass != null)
                        {
                            MethodInfo method = clientClass.GetMethod(p.ProcedureName, BindingFlags.Instance | BindingFlags.Public);
                            var instance = Activator.CreateInstance(clientClass);
                            if (method != null)
                            {
                                //ConsoleMessage("Method Fake Run: " + ccp.ProcedureName, true);
                                method.Invoke(instance, new object[] { client, sourceFile, p, eventMessage });
                            }
                            else
                            {
                                ConsoleMessage("Method not found: " + p.ProcedureName);
                            }
                        }
                        else
                            ConsoleMessage("Type not found: " + path);
                    }
                }
            }
            catch (Exception ex)
            {
                string proc = String.Empty;
                if (ccpRun != null)
                    proc = ccpRun.ProcedureName;
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
                if (AppData.myAppData != null && AppData.myAppData.CurrentApp != null)
                    app = KMPlatform.BusinessLogic.Enums.GetApplication(AppData.myAppData.CurrentApp.ApplicationName);
                ApplicationLog alWorker = new ApplicationLog();
                string formatException = StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".ExecuteClientCustomCode", app, "ClientCustomProcedure: " + proc, client.ClientID);
            }
        }
        public void ExecuteClientCustomStoredProcedures(List<ClientCustomProcedure> list, int sourceFileID, Client client, FileMoved eventMessage)
        {
            ClientCustomProcedure ccpRun = null;
            try
            {
                ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Start execute client custom procs");
                FrameworkUAS.BusinessLogic.ClientCustomProcedure ccpData = new FrameworkUAS.BusinessLogic.ClientCustomProcedure();
                foreach (ClientCustomProcedure ccp in list)
                {
                    ccpRun = ccp;
                    if (ccp.ProcedureType.Equals(Enums.ProcedureTypes.SQL.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        try
                        {
                            ccpData.ExecuteSproc(ccp.ProcedureName, sourceFileID, eventMessage.SourceFile.FileName, client, eventMessage.AdmsLog.ProcessCode);
                        }
                        catch (Exception ex)
                        {
                            string proc = String.Empty;
                            if (ccpRun != null)
                                proc = ccpRun.ProcedureName;
                            LogError(ex, client, this.GetType().Name.ToString() + ".ExecuteClientCustomProcs - ClientCustomProcedure: " + proc);
                        }
                    }
                }
                ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Done execute client custom procs");
            }
            catch (Exception ex)
            {
                string proc = String.Empty;
                if (ccpRun != null)
                    proc = ccpRun.ProcedureName;
                LogError(ex, client, this.GetType().Name.ToString() + ".ExecuteClientCustomProcs - ClientCustomProcedure: " + proc);
            }
        }
        public void ExecuteClientCustomMethods(List<ClientCustomProcedure> list, Client client, SourceFile sourceFile, string processCode)
        {
            ClientCustomProcedure ccpRun = null;
            try
            {
                foreach (var p in list)
                {
                    ccpRun = p;
                    if (p.ProcedureType.Equals(Enums.ProcedureTypes.NET.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        String path = "ADMS.ClientMethods." + client.FtpFolder;
                        Type clientClass = Type.GetType(path + ",ADMS");
                        if (clientClass != null)
                        {
                            MethodInfo method = clientClass.GetMethod(p.ProcedureName, BindingFlags.Instance | BindingFlags.Public);
                            var instance = Activator.CreateInstance(clientClass);
                            if (method != null)
                            {
                                method.Invoke(instance, new object[] { client, sourceFile, p, processCode });
                            }
                            else
                            {
                                ConsoleMessage("Method not found: " + p.ProcedureName, processCode, true);
                            }
                        }
                        else
                            ConsoleMessage("Type not found: " + path, processCode, true);
                    }
                }
            }
            catch (Exception ex)
            {
                string proc = String.Empty;
                if (ccpRun != null)
                    proc = ccpRun.ProcedureName;
                LogError(ex, client, this.GetType().Name.ToString() + ".ExecuteClientCustomCode - ClientCustomProcedure: " + proc);
            }
        }
        public void ExecuteClientCustomStoredProcedures(List<ClientCustomProcedure> list, int sourceFileID, string fileName, Client client, string processCode)
        {
            ClientCustomProcedure ccpRun = null;
            try
            {
                ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Start execute client custom procs", processCode, true);
                FrameworkUAS.BusinessLogic.ClientCustomProcedure ccpData = new FrameworkUAS.BusinessLogic.ClientCustomProcedure();
                foreach (ClientCustomProcedure ccp in list)
                {
                    ccpRun = ccp;
                    if (ccp.ProcedureType.Equals(Enums.ProcedureTypes.SQL.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        try
                        {
                            ccpData.ExecuteSproc(ccp.ProcedureName, sourceFileID, fileName, client, processCode);
                        }
                        catch (Exception ex)
                        {
                            string proc = String.Empty;
                            if (ccpRun != null)
                                proc = ccpRun.ProcedureName;
                            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
                            if (AppData.myAppData != null && AppData.myAppData.CurrentApp != null)
                                app = KMPlatform.BusinessLogic.Enums.GetApplication(AppData.myAppData.CurrentApp.ApplicationName);
                            ApplicationLog alWorker = new ApplicationLog();
                            string formatException = StringFunctions.FormatException(ex);
                            alWorker.LogCriticalError(formatException, this.GetType().Name.ToString() + ".ExecuteClientCustomProcs", app, "ClientCustomProcedure: " + proc, client.ClientID);
                        }
                    }
                }
                ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Done execute client custom procs", processCode, true);
            }
            catch (Exception ex)
            {
                string proc = String.Empty;
                if (ccpRun != null)
                    proc = ccpRun.ProcedureName;
                LogError(ex, client, this.GetType().Name.ToString() + ".ExecuteClientCustomCode - ClientCustomProcedure: " + proc);
            }
        }
        private bool UploadFileToADMS(FileInfo file, ClientFTP clientFTP)
        {
            bool uploadDone = true;
            FtpFunctions ftp = new FtpFunctions(clientFTP.Server, clientFTP.UserName, clientFTP.Password);
            uploadDone = ftp.Upload(clientFTP.Folder + "\\" + file.Name, file.FullName);

            return uploadDone;
        }

        public void EventSwipeDataDefault(Client client, FileMoved eventMessage)
        {
            var fillAgGroupAndTableArgs = new FillAgGroupAndTableArgs()
            {
                Client = client,
                EventMessage = eventMessage,
                SourceFileId = eventMessage.SourceFile.SourceFileID,
                AdHocDimensionGroupName = EventSwipeDataDimGroup,
                CreatedDimension = TopCompanyDim,
                StandardField = PubCodeStandardField,
                DimensionValueField = MarketFieldName,
                MatchValueField = PubCodeFieldName,
                DimensionOperator = EqualOperation,
                UpdateUAD = true

            };
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(fillAgGroupAndTableArgs);
        }

        protected static void AdHockDimensionsImport(
            FileMoved eventMessage,
            string adHocDimensionGroupName,
            string createdDimension,
            string dimensionValue,
            string matchFieldName)
        {
            var fillAgGroupAndTableArgs = new FillAgGroupAndTableArgs()
            {
                AdHocDimensionGroupName = adHocDimensionGroupName,
                Client = eventMessage.Client,
                SourceFileId = eventMessage.SourceFile.SourceFileID,
                CreatedDimension = createdDimension,
                StandardField = matchFieldName,
                DimensionValue = dimensionValue,
                DimensionOperator = ContainsOperation,
                EventMessage = eventMessage,
                MatchValueField = matchFieldName
            };
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(fillAgGroupAndTableArgs);
        }
    }
}
