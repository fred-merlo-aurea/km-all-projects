using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ADMS
{
    //public static class ADMSProcessingQue
    //{
    //    public static List<ADMSCurrentProcessQue> processingFiles = new List<ADMSCurrentProcessQue>();
    //    public static List<ADMSDQMReadyQue> readyForDQMFiles = new List<ADMSDQMReadyQue>();
    //    public static List<KMPlatform.Entity.Client> clientsRunningDQM = new List<KMPlatform.Entity.Client>();
    //    public static List<KMPlatform.Entity.Client> DQMClients = new List<KMPlatform.Entity.Client>();
    //    public static List<KMPlatform.Entity.Service> services;
    //    readonly static FrameworkUAS.BusinessLogic.AdmsLog admsWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
    //    readonly static FrameworkUAS.BusinessLogic.EngineLog elWrk = new FrameworkUAS.BusinessLogic.EngineLog();
    //    readonly static FrameworkUAS.BusinessLogic.SourceFile sfWrk = new FrameworkUAS.BusinessLogic.SourceFile();
    //    private static FrameworkUAS.Entity.EngineLog EngineLog { get; set; }
    //    public static string processCode { get; set; }

    //    private static FrameworkUAS.Entity.AdmsLog _admsLog;
    //    public static FrameworkUAS.Entity.AdmsLog admsLog
    //    {
    //        get
    //        {
    //            if (_admsLog == null)
    //            {
    //                _admsLog = admsWrk.Select(processCode);
    //            }

    //            return _admsLog;
    //        }
    //        set
    //        {
    //            _admsLog = value;
    //        }
    //    }

    //    #region ServiceBase
    //    private static void ConsoleMessage(string message, string processCode, bool createLog = true, int sourceFileId = 0, int updatedByUserId = 0)
    //    {
    //        ADMSProcessingQue.processCode = processCode;
    //        if (string.IsNullOrEmpty(processCode) && admsLog == null)
    //            return;
    //        int fileStatusTypeId = 0;
    //        if (admsLog != null)
    //        {
    //            if (string.IsNullOrEmpty(processCode))
    //                processCode = admsLog.ProcessCode;
    //            if (sourceFileId == 0)
    //                sourceFileId = admsLog.SourceFileId;
    //            fileStatusTypeId = admsLog.FileStatusId;
    //        }
    //        admsWrk.UpdateStatusMessage(processCode, message, updatedByUserId, createLog, sourceFileId, fileStatusTypeId);
    //    }
    //    private static void SaveEngineLog(string msg, int clientId)
    //    {
    //        elWrk.SaveEngineLog(msg, clientId, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS);
    //    }
    //    private static void LogError(Exception ex, KMPlatform.Entity.Client client, string msg, string processCode, bool removeThread = true, bool removeQue = true)
    //    {
    //        ADMSProcessingQue.processCode = processCode;
    //        #region Log Error
    //        if (removeThread)
    //            ThreadDictionary.Remove(admsLog.ThreadId);
    //        if (removeQue)
    //            ADMSProcessingQue.RemoveClientFile(client, admsLog.ImportFile);
    //        string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
    //        ConsoleMessage(formatException, processCode);

    //        System.Text.StringBuilder sbDetail = new System.Text.StringBuilder();
    //        sbDetail.AppendLine("Client: " + client.FtpFolder);
    //        sbDetail.AppendLine("File: " + admsLog.ImportFile.FullName);
    //        sbDetail.AppendLine("SourceFileID: " + admsLog.SourceFileId.ToString());
    //        sbDetail.AppendLine("Thread: " + admsLog.ThreadId.ToString());
    //        sbDetail.AppendLine("ProcessCode: " + admsLog.ProcessCode.ToString());
    //        sbDetail.AppendLine(msg);

    //        KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
    //        if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
    //            app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
    //        KMPlatform.BusinessLogic.ApplicationLog appLogWorker = new KMPlatform.BusinessLogic.ApplicationLog();
    //        appLogWorker.LogCriticalError(formatException, "ADMSProcessingQue.LogError", app, sbDetail.ToString());
    //        #endregion
    //    }
    //    #endregion

    //    #region Processing Files Methods
    //    public static bool AddClientFile(KMPlatform.Entity.Client client, FileInfo file)
    //    {
    //        ADMSCurrentProcessQue newItem = new ADMSCurrentProcessQue() { Client = client, File = file };
    //        bool returnValue = false;
    //        if (CanAddClientFile(client, file) == true && CanAddClientDQM(client) == true)
    //        {
    //            if (processingFiles.ToList().Count(x => x.Client.ClientID == client.ClientID && x.File.Name == file.Name) == 0)
    //            {
    //                processingFiles.Add(newItem);
    //                returnValue = true;
    //            }
    //            else
    //                returnValue = false;
    //        }
    //        else
    //        {
    //            returnValue = false;
    //        }
    //        return returnValue;
    //    }
    //    public static bool CanAddClientFile(KMPlatform.Entity.Client client, FileInfo file)
    //    {
    //        var clientsFiles = processingFiles.ToList().Select(x => x.Client == client).ToList();
    //        if (clientsFiles.Count < 10)
    //            return true;
    //        else
    //            return false;
    //    }
    //    public static List<ADMSCurrentProcessQue> ReturnAllProcessingFiles()
    //    {
    //        return processingFiles.ToList();
    //    }
    //    public static int ClientProcessFileCount(KMPlatform.Entity.Client client)
    //    {
    //        List<ADMSCurrentProcessQue> clientsFiles = new List<ADMSCurrentProcessQue>();
    //        foreach (ADMSCurrentProcessQue a in processingFiles)
    //        {
    //            if (a.Client == client)
    //            {
    //                clientsFiles.Add(a);
    //            }
    //        }

    //        return clientsFiles.Count;
    //    }
    //    public static void RemoveClientFile(KMPlatform.Entity.Client client, FileInfo file)
    //    {
    //        try
    //        {
    //            processingFiles.RemoveAll(x => x.Client.ClientID == client.ClientID && x.File.Name.Equals(file.Name, StringComparison.CurrentCultureIgnoreCase));
    //            //ADMSCurrentProcessQue item = processingFiles.FirstOrDefault(x => x.Client == client && x.File.Name == file.Name);
    //            //if (item != null && processingFiles.Contains(item))
    //            //    processingFiles.Remove(item);
    //        }
    //        catch { }
    //    }
    //    public static void RemoveClientFileByNames(string client, string file)
    //    {
    //        //ADMSCurrentProcessQue newItem = new ADMSCurrentProcessQue() { Client = client, File = file };
    //        ADMSCurrentProcessQue newItem = processingFiles.FirstOrDefault(x => x.Client.FtpFolder == client && x.File.FullName == file);
    //        if (newItem != null)
    //            if (processingFiles.Contains(newItem))
    //                processingFiles.Remove(newItem);

    //    }
    //    #endregion

    //    #region Running DQM Methods
    //    public static bool AddClientDQM(KMPlatform.Entity.Client client)
    //    {
    //        bool returnValue = false;
    //        if (CanAddClientDQM(client))
    //        {
    //            if (!clientsRunningDQM.Contains(client))
    //                clientsRunningDQM.Add(client);
    //            returnValue = true;
    //        }
    //        else
    //            returnValue = false;

    //        return returnValue;
    //    }
    //    public static bool CanAddClientDQM(KMPlatform.Entity.Client client)
    //    {
    //        bool returnValue = true;
    //        if (clientsRunningDQM.Contains(client))
    //            returnValue = false;
    //        else
    //            returnValue = true;

    //        return returnValue;
    //    }
    //    public static List<KMPlatform.Entity.Client> ReturnAllInDQM()
    //    {
    //        return clientsRunningDQM.ToList();
    //    }
    //    public static void RemoveClientDQM(KMPlatform.Entity.Client client)
    //    {
    //        if (clientsRunningDQM.Contains(client))
    //            clientsRunningDQM.Remove(client);

    //    }
    //    #endregion

    //    #region DQM Ready Files
    //    public static void AddDQMReadyFile(Core.ADMS.Events.FileAddressGeocoded eventMessage)
    //    {
    //        admsLog = eventMessage.AdmsLog;
    //        processCode = admsLog.ProcessCode;

    //        ADMSDQMReadyQue newItem = new ADMSDQMReadyQue() { Client = eventMessage.Client, EventMessage = eventMessage };
    //        if (!readyForDQMFiles.Contains(newItem))
    //        {
    //            readyForDQMFiles.Add(newItem);
    //        }

    //        //CALL AND SAVE TO DB NOW
    //        bool isDemo = true;
    //        bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IsDemo"].ToString(), out isDemo);

    //        FrameworkUAS.Entity.DQMQue q = new FrameworkUAS.Entity.DQMQue(eventMessage.AdmsLog.ProcessCode, eventMessage.Client.ClientID, isDemo, true, eventMessage.SourceFile.SourceFileID);
    //        FrameworkUAS.BusinessLogic.DQMQue dqmWorker = new FrameworkUAS.BusinessLogic.DQMQue();
    //        dqmWorker.Save(q);
    //        eventMessage.AdmsLog.DQM = q;

    //        admsWrk.Update(eventMessage.AdmsLog.ProcessCode,
    //                     FrameworkUAD_Lookup.Enums.FileStatusType.Qued,
    //                     FrameworkUAD_Lookup.Enums.ADMS_StepType.Qued,
    //                     FrameworkUAD_Lookup.Enums.ProcessingStatusType.Qued,
    //                     FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_DQM, 1, "Added file to DQM Que", true,
    //                     eventMessage.AdmsLog.SourceFileId);

    //    }
    //    public static List<FrameworkUAS.Entity.DQMQue> ReturnClientReadyFiles(KMPlatform.Entity.Client client)
    //    {
    //        //List<ADMSDQMReadyQue> fileList = new List<ADMSDQMReadyQue>();
    //        //foreach (ADMSDQMReadyQue a in readyForDQMFiles)
    //        //{
    //        //    if (a.Client == client)
    //        //    {
    //        //        fileList.Add(a);
    //        //    }
    //        //}
    //        List<FrameworkUAS.Entity.DQMQue> fileList = new List<FrameworkUAS.Entity.DQMQue>();
    //        FrameworkUAS.BusinessLogic.DQMQue dqmQueWorker = new FrameworkUAS.BusinessLogic.DQMQue();
    //        fileList = dqmQueWorker.Select(client.ClientID, true, false);
    //        return fileList;
    //    }
    //    public static void RunDQMForClient(KMPlatform.Entity.Client client, bool isActived = true)
    //    {
    //        try
    //        {
    //            EngineLog = elWrk.Select(client.ClientID, "ADMS");

    //            //List<ADMSDQMReadyQue> files = ReturnClientReadyFiles(client);
    //            List<FrameworkUAS.Entity.DQMQue> files = new List<FrameworkUAS.Entity.DQMQue>();
    //            List<FrameworkUAS.Entity.SourceFile> sourceFiles = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList;// new List<FrameworkUAS.Entity.SourceFile>();
    //            FrameworkUAS.BusinessLogic.DQMQue dqmQueWorker = new FrameworkUAS.BusinessLogic.DQMQue();

    //            bool isDemo = true;
    //            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IsDemo"].ToString(), out isDemo);

    //            files = dqmQueWorker.Select(client.ClientID, isDemo, true).Where(x => x.IsCompleted == false && x.IsQued == false).ToList();
    //            //sourceFiles = sfWrk.Select(client.ClientID,true,false);
    //            if (files.Count > 0)
    //            {
    //                #region Process files
    //                if (!isActived)
    //                {
    //                    foreach (FrameworkUAS.Entity.DQMQue f in files)
    //                    {
    //                        FrameworkUAS.Entity.SourceFile mySf = sourceFiles.FirstOrDefault(x => x.SourceFileID == f.SourceFileId);
    //                        processCode = f.ProcessCode;
    //                        Core.ADMS.Events.FileAddressGeocoded eventMessage = new Core.ADMS.Events.FileAddressGeocoded(new FileInfo(mySf.FileName), client, true, true,
    //                                                                            true, mySf, admsWrk.Select(f.ProcessCode), new FrameworkUAD.Object.ValidationResult());

    //                        ADMSDQMReadyQue newItem = new ADMSDQMReadyQue() { Client = eventMessage.Client, EventMessage = eventMessage };
    //                        if (!readyForDQMFiles.Contains(newItem))
    //                            readyForDQMFiles.Add(newItem);
    //                    }
    //                    //readyForDQMFiles
    //                }

    //                List<FrameworkUAD_Lookup.Entity.Code> databaseFileTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
    //                FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
    //                databaseFileTypes = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Database_File).ToList();

    //                #region split files into Circ / Other lists
    //                //order files so that Circ files always process first
    //                if (services == null || services.Count == 0)
    //                {
    //                    KMPlatform.BusinessLogic.Service sworker = new KMPlatform.BusinessLogic.Service();
    //                    services = sworker.Select(false).ToList();
    //                }
    //                int circ = services.SingleOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase)).ServiceID;
    //                List<FrameworkUAS.Entity.SourceFile> circSourceFiles = new List<FrameworkUAS.Entity.SourceFile>();
    //                if (BillTurner.ClientAdditionalProperties != null && BillTurner.ClientAdditionalProperties.ContainsKey(client.ClientID))
    //                {
    //                    circSourceFiles = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Where(x => x.ServiceID == circ).ToList();
    //                }
    //                else
    //                {
    //                    FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
    //                    FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
    //                    cap = capWork.SetObjects(client.ClientID, false);
    //                    circSourceFiles = cap.SourceFilesList.Where(x => x.ServiceID == circ).ToList();
    //                }

    //                List<FrameworkUAS.Entity.DQMQue> circDQMFiles = (from cf in circSourceFiles
    //                                                                 join d in files on cf.SourceFileID equals d.SourceFileId
    //                                                                 select d).ToList();

    //                List<FrameworkUAS.Entity.DQMQue> otherDQMFiles = (from f in files
    //                                                                  where !(from c in circSourceFiles
    //                                                                          select c.SourceFileID).Contains(f.SourceFileId)
    //                                                                  select f).ToList();
    //                #endregion

    //                int processed = 1;
    //                int totalFiles = files.Count;
    //                if (totalFiles > 0)
    //                {
    //                    #region Circ
    //                    foreach (FrameworkUAS.Entity.DQMQue f in circDQMFiles)
    //                    {
    //                        try
    //                        {
    //                            #region Process Circ Files
    //                            elWrk.SaveEngineLog("DQM Processing Circ file " + processed + " of " + totalFiles, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                            bool processFile = true;
    //                            FrameworkUAS.Entity.SourceFile sourceFile = new FrameworkUAS.Entity.SourceFile();
    //                            if (BillTurner.ClientAdditionalProperties != null && BillTurner.ClientAdditionalProperties.ContainsKey(client.ClientID))
    //                            {
    //                                sourceFile = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.SingleOrDefault(x => x.SourceFileID == f.SourceFileId);
    //                            }
    //                            else
    //                            {
    //                                FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
    //                                FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
    //                                cap = capWork.SetObjects(client.ClientID, false);
    //                                sourceFile = cap.SourceFilesList.SingleOrDefault(x => x.SourceFileID == f.SourceFileId);
    //                            }

    //                            if (sourceFile == null)
    //                            {
    //                                //Attempt to get new list in case engine was on and didn't update before running a new mapped file
    //                                FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
    //                                if (BillTurner.ClientAdditionalProperties != null && BillTurner.ClientAdditionalProperties.ContainsKey(client.ClientID))
    //                                {
    //                                    BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList = sfData.Select(client.ClientID, true).Where(x => x.IsDeleted == false).ToList();
    //                                    sourceFile = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.SingleOrDefault(x => x.SourceFileID == f.SourceFileId);
    //                                }
    //                                else
    //                                {
    //                                    FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
    //                                    FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
    //                                    cap = capWork.SetObjects(client.ClientID, false);
    //                                    sourceFile = cap.SourceFilesList.SingleOrDefault(x => x.SourceFileID == f.SourceFileId);
    //                                }
    //                                if (sourceFile == null)
    //                                {
    //                                    elWrk.SaveEngineLog("Source file was null - ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                                    continue;
    //                                }
    //                            }

    //                            FrameworkUAD_Lookup.Entity.Code dbft = databaseFileTypes.SingleOrDefault(x => x.CodeId == sourceFile.DatabaseFileTypeId);
    //                            FrameworkUAD_Lookup.Enums.FileTypes dft = FrameworkUAD_Lookup.Enums.GetDatabaseFileType(dbft.CodeName);
    //                            //get PubCodes
    //                            FrameworkUAD.BusinessLogic.SubscriberTransformed stWorker = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
    //                            List<string> pubs = stWorker.GetDistinctPubCodes(client.ClientConnections, f.ProcessCode);

    //                            if (dft == FrameworkUAD_Lookup.Enums.FileTypes.ACS ||
    //                                dft == FrameworkUAD_Lookup.Enums.FileTypes.Field_Update ||
    //                                dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_2YR ||
    //                                dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_3YR ||
    //                                dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_Other ||
    //                                dft == FrameworkUAD_Lookup.Enums.FileTypes.NCOA ||
    //                                dft == FrameworkUAD_Lookup.Enums.FileTypes.QuickFill ||
    //                                dft == FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Long_Form ||
    //                                dft == FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Short_Form)
    //                            {
    //                                foreach (string p in pubs)
    //                                {
    //                                    FrameworkUAD.BusinessLogic.Product prodWorker = new FrameworkUAD.BusinessLogic.Product();
    //                                    FrameworkUAD.Entity.Product prod = prodWorker.Select(p, client.ClientConnections);

    //                                    if (prod != null)
    //                                    {
    //                                        //this is for WebForms and SubGen
    //                                        if (prod.AllowDataEntry == true ||
    //                                            (prod.KMImportAllowed == false && prod.ClientImportAllowed == false))
    //                                        {
    //                                            elWrk.SaveEngineLog("Product Locked. Skipping - ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                                            processFile = false;
    //                                            break;
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        //pubCode does not exist so do not process file - will just mark as done and remove from Que
    //                                        //will also log that not done
    //                                        FrameworkUAS.BusinessLogic.DQMQue dqmWrk = new FrameworkUAS.BusinessLogic.DQMQue();
    //                                        dqmWrk.UpdateComplete(f.ProcessCode, true, "Product code does not exist for " + p, f.SourceFileId);
    //                                        processFile = false;
    //                                        break;
    //                                    }
    //                                }
    //                            }
    //                            else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Complimentary)
    //                            {
    //                                foreach (string p in pubs)
    //                                {
    //                                    FrameworkUAD.BusinessLogic.Product prodWorker = new FrameworkUAD.BusinessLogic.Product();
    //                                    FrameworkUAD.Entity.Product prod = prodWorker.Select(p, client.ClientConnections);

    //                                    if (prod != null)
    //                                    {
    //                                        //this is for WebForms and SubGen
    //                                        if (prod.AllowDataEntry == true ||
    //                                            prod.KMImportAllowed == true ||
    //                                            prod.ClientImportAllowed == true ||
    //                                            prod.AddRemoveAllowed == true)
    //                                        {
    //                                            elWrk.SaveEngineLog("Product Locked. Skipping - ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                                            processFile = false;
    //                                            break;
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        //pubCode does not exist so do not process file - will just mark as done and remove from Que
    //                                        //will also log that not done
    //                                        FrameworkUAS.BusinessLogic.DQMQue dqmWrk = new FrameworkUAS.BusinessLogic.DQMQue();
    //                                        dqmWrk.UpdateComplete(f.ProcessCode, true, "Product code does not exist for " + p, f.SourceFileId);
    //                                        processFile = false;
    //                                        break;
    //                                    }
    //                                }
    //                            }
    //                            else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms ||
    //                                     dft == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms_Short_Form ||
    //                                     dft == FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction)
    //                            {
    //                                foreach (string p in pubs)
    //                                {
    //                                    FrameworkUAD.BusinessLogic.Product prodWorker = new FrameworkUAD.BusinessLogic.Product();
    //                                    FrameworkUAD.Entity.Product prod = prodWorker.Select(p, client.ClientConnections);

    //                                    if (prod != null)
    //                                    {
    //                                        //this is for WebForms and SubGen
    //                                        if (prod.AllowDataEntry == false && prod.KMImportAllowed == false && prod.ClientImportAllowed == false)
    //                                        {
    //                                            elWrk.SaveEngineLog("Product Locked. Skipping - ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                                            processFile = false;
    //                                            break;
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        //pubCode does not exist so do not process file - will just mark as done and remove from Que
    //                                        //will also log that not done
    //                                        FrameworkUAS.BusinessLogic.DQMQue dqmWrk = new FrameworkUAS.BusinessLogic.DQMQue();
    //                                        dqmWrk.UpdateComplete(f.ProcessCode, true, "Product code does not exist for " + p, f.SourceFileId);
    //                                        processFile = false;
    //                                        break;
    //                                    }
    //                                }
    //                            }

    //                            if (processFile)
    //                            {
    //                                f.IsQued = true;
    //                                f.DateQued = DateTime.Now;
    //                                dqmQueWorker.Save(f);

    //                                ProcessFile(client, f, sourceFile);
    //                                processed++;

    //                                elWrk.SaveEngineLog("Finished DQM for Circ file " + client.FtpFolder + " " + DateTime.Now.TimeOfDay.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                            }
    //                            else
    //                            {
    //                                elWrk.SaveEngineLog("Skipped DQM for Circ file " + client.FtpFolder + " " + DateTime.Now.TimeOfDay.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                            }
    //                            #endregion
    //                        }
    //                        catch (Exception ex)
    //                        {
    //                            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
    //                            if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
    //                                app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
    //                            KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
    //                            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
    //                            alWorker.LogCriticalError(formatException, "ADMS.ADMSProcessingQue.RunDQMForClient_1", app, string.Empty);
    //                        }
    //                    }
    //                    #endregion
    //                    #region Other
    //                    foreach (FrameworkUAS.Entity.DQMQue f in otherDQMFiles)
    //                    {
    //                        try
    //                        {
    //                            #region Check for any new circ files
    //                            List<FrameworkUAS.Entity.DQMQue> newFiles = new List<FrameworkUAS.Entity.DQMQue>();
    //                            var dqmQueList = dqmQueWorker.Select(client.ClientID, isDemo, true);
    //                            if (dqmQueList != null && dqmQueList.Count > 0)
    //                            {
    //                                newFiles = dqmQueList.Where(x => x.IsCompleted == false && x.IsQued == false).ToList();
    //                                List<FrameworkUAS.Entity.SourceFile> newCircSourceFiles = new List<FrameworkUAS.Entity.SourceFile>();
    //                                try
    //                                {
    //                                    if (BillTurner.ClientAdditionalProperties != null && BillTurner.ClientAdditionalProperties.ContainsKey(client.ClientID))
    //                                    {
    //                                        newCircSourceFiles = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.Where(x => x.ServiceID == circ).ToList();
    //                                    }
    //                                    else
    //                                    {
    //                                        FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
    //                                        FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
    //                                        cap = capWork.SetObjects(client.ClientID, false);
    //                                        newCircSourceFiles = cap.SourceFilesList.Where(x => x.ServiceID == circ).ToList();
    //                                    }
    //                                }
    //                                catch (Exception ex)
    //                                {
    //                                    KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
    //                                    if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
    //                                        app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
    //                                    KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
    //                                    string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
    //                                    alWorker.LogCriticalError(formatException, "ADMS.ADMSProcessingQue.OtherCircCheck", app, string.Empty);
    //                                }

    //                                List<FrameworkUAS.Entity.DQMQue> newCircDQMFiles = (from cf in newCircSourceFiles
    //                                                                                    join d in newFiles on cf.SourceFileID equals d.SourceFileId
    //                                                                                    select d).ToList();

    //                                if (newCircDQMFiles.Count > 0)
    //                                {
    //                                    totalFiles += newCircDQMFiles.Count;

    //                                    foreach (FrameworkUAS.Entity.DQMQue cdf in newCircDQMFiles)
    //                                    {
    //                                        #region Process Circ Files
    //                                        elWrk.SaveEngineLog("DQM Processing Circ file " + processed + " of " + totalFiles, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                                        bool processFile = true;
    //                                        FrameworkUAS.Entity.SourceFile newSourceFile = new FrameworkUAS.Entity.SourceFile();
    //                                        if (BillTurner.ClientAdditionalProperties != null && BillTurner.ClientAdditionalProperties.ContainsKey(client.ClientID))
    //                                        {
    //                                            newSourceFile = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.SingleOrDefault(x => x.SourceFileID == cdf.SourceFileId);
    //                                        }
    //                                        else
    //                                        {
    //                                            FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
    //                                            FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
    //                                            cap = capWork.SetObjects(client.ClientID, false);
    //                                            newSourceFile = cap.SourceFilesList.SingleOrDefault(x => x.SourceFileID == cdf.SourceFileId);
    //                                        }

    //                                        if (newSourceFile == null)
    //                                        {
    //                                            elWrk.SaveEngineLog("Source file was null - ProcessCode: " + cdf.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                                            continue;
    //                                        }

    //                                        FrameworkUAD_Lookup.Entity.Code dbft = databaseFileTypes.SingleOrDefault(x => x.CodeId == newSourceFile.DatabaseFileTypeId);
    //                                        FrameworkUAD_Lookup.Enums.FileTypes dft = FrameworkUAD_Lookup.Enums.GetDatabaseFileType(dbft.CodeName);
    //                                        //get PubCodes
    //                                        FrameworkUAD.BusinessLogic.SubscriberTransformed stWorker = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
    //                                        List<string> pubs = stWorker.GetDistinctPubCodes(client.ClientConnections, f.ProcessCode);

    //                                        if (dft == FrameworkUAD_Lookup.Enums.FileTypes.ACS ||
    //                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.Field_Update ||
    //                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_2YR ||
    //                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_3YR ||
    //                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_Other ||
    //                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.NCOA ||
    //                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.QuickFill ||
    //                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Long_Form ||
    //                                            dft == FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Short_Form)
    //                                        {
    //                                            foreach (string p in pubs)
    //                                            {
    //                                                FrameworkUAD.BusinessLogic.Product prodWorker = new FrameworkUAD.BusinessLogic.Product();
    //                                                FrameworkUAD.Entity.Product prod = prodWorker.Select(p, client.ClientConnections);

    //                                                if (prod != null)
    //                                                {
    //                                                    //this is for WebForms and SubGen
    //                                                    if (prod.AllowDataEntry == true ||
    //                                                        (prod.KMImportAllowed == false && prod.ClientImportAllowed == false))
    //                                                    {
    //                                                        elWrk.SaveEngineLog("Product Locked. Skipping - ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                                                        processFile = false;
    //                                                        break;
    //                                                    }
    //                                                }
    //                                                else
    //                                                {
    //                                                    //pubCode does not exist so do not process file - will just mark as done and remove from Que
    //                                                    //will also log that not done
    //                                                    FrameworkUAS.BusinessLogic.DQMQue dqmWrk = new FrameworkUAS.BusinessLogic.DQMQue();
    //                                                    dqmWrk.UpdateComplete(f.ProcessCode, true, "Product code does not exist for " + p, f.SourceFileId);
    //                                                    processFile = false;
    //                                                    break;
    //                                                }
    //                                            }
    //                                        }
    //                                        else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Complimentary)
    //                                        {
    //                                            foreach (string p in pubs)
    //                                            {
    //                                                FrameworkUAD.BusinessLogic.Product prodWorker = new FrameworkUAD.BusinessLogic.Product();
    //                                                FrameworkUAD.Entity.Product prod = prodWorker.Select(p, client.ClientConnections);

    //                                                if (prod != null)
    //                                                {
    //                                                    //this is for WebForms and SubGen
    //                                                    if (prod.AllowDataEntry == true ||
    //                                                        prod.KMImportAllowed == true ||
    //                                                        prod.ClientImportAllowed == true ||
    //                                                        prod.AddRemoveAllowed == true)
    //                                                    {
    //                                                        elWrk.SaveEngineLog("Product Locked. Skipping - ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                                                        processFile = false;
    //                                                        break;
    //                                                    }
    //                                                }
    //                                                else
    //                                                {
    //                                                    //pubCode does not exist so do not process file - will just mark as done and remove from Que
    //                                                    //will also log that not done
    //                                                    FrameworkUAS.BusinessLogic.DQMQue dqmWrk = new FrameworkUAS.BusinessLogic.DQMQue();
    //                                                    dqmWrk.UpdateComplete(f.ProcessCode, true, "Product code does not exist for " + p, f.SourceFileId);
    //                                                    processFile = false;
    //                                                    break;
    //                                                }
    //                                            }
    //                                        }
    //                                        else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms ||
    //                                                 dft == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms_Short_Form ||
    //                                                 dft == FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction)
    //                                        {
    //                                            foreach (string p in pubs)
    //                                            {
    //                                                FrameworkUAD.BusinessLogic.Product prodWorker = new FrameworkUAD.BusinessLogic.Product();
    //                                                FrameworkUAD.Entity.Product prod = prodWorker.Select(p, client.ClientConnections);

    //                                                if (prod != null)
    //                                                {
    //                                                    //this is for WebForms and SubGen
    //                                                    if (prod.AllowDataEntry == false && prod.KMImportAllowed == false && prod.ClientImportAllowed == false)
    //                                                    {
    //                                                        elWrk.SaveEngineLog("Product Locked. Skipping - ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                                                        processFile = false;
    //                                                        break;
    //                                                    }
    //                                                }
    //                                                else
    //                                                {
    //                                                    //pubCode does not exist so do not process file - will just mark as done and remove from Que
    //                                                    //will also log that not done
    //                                                    FrameworkUAS.BusinessLogic.DQMQue dqmWrk = new FrameworkUAS.BusinessLogic.DQMQue();
    //                                                    dqmWrk.UpdateComplete(f.ProcessCode, true, "Product code does not exist for " + p, f.SourceFileId);
    //                                                    processFile = false;
    //                                                    break;
    //                                                }
    //                                            }
    //                                        }

    //                                        if (processFile)
    //                                        {
    //                                            cdf.IsQued = true;
    //                                            cdf.DateQued = DateTime.Now;
    //                                            dqmQueWorker.Save(cdf);

    //                                            ProcessFile(client, cdf, newSourceFile);
    //                                            processed++;

    //                                            elWrk.SaveEngineLog("Finished DQM for Circ file " + client.FtpFolder + " " + DateTime.Now.TimeOfDay.ToString() + " ProcessCode: " + cdf.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                                        }
    //                                        else
    //                                        {
    //                                            elWrk.SaveEngineLog("Skipped DQM for Circ file " + client.FtpFolder + " " + DateTime.Now.TimeOfDay.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                                        }
    //                                        #endregion
    //                                    }
    //                                }
    //                            }
    //                            #endregion
    //                            #region Process Other Files
    //                            elWrk.SaveEngineLog("DQM Processing " + processed + " of " + totalFiles, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                            elWrk.SaveEngineLog("Get source file - ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                            FrameworkUAS.Entity.SourceFile sourceFile = new FrameworkUAS.Entity.SourceFile();
    //                            if (BillTurner.ClientAdditionalProperties != null && BillTurner.ClientAdditionalProperties.ContainsKey(client.ClientID))
    //                            {
    //                                sourceFile = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.SingleOrDefault(x => x.SourceFileID == f.SourceFileId);
    //                            }
    //                            else
    //                            {
    //                                FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
    //                                FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
    //                                cap = capWork.SetObjects(client.ClientID, false);
    //                                sourceFile = cap.SourceFilesList.SingleOrDefault(x => x.SourceFileID == f.SourceFileId);
    //                            }

    //                            if (sourceFile == null)
    //                            {
    //                                //Attempt to get new list in case engine was on and didn't update before running a new mapped file
    //                                if (BillTurner.ClientAdditionalProperties != null && BillTurner.ClientAdditionalProperties.ContainsKey(client.ClientID))
    //                                {
    //                                    FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
    //                                    BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList = sfData.Select(client.ClientID, true).Where(x => x.IsDeleted == false).ToList();
    //                                    sourceFile = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.SingleOrDefault(x => x.SourceFileID == f.SourceFileId);
    //                                }
    //                                else
    //                                {
    //                                    FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
    //                                    sourceFile = sfData.Select(client.ClientID, true).Where(x => x.IsDeleted == false).ToList().SingleOrDefault(x => x.SourceFileID == f.SourceFileId);
    //                                }
    //                                if (sourceFile == null)
    //                                {
    //                                    elWrk.SaveEngineLog("Source file was null - ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                                    continue;
    //                                }
    //                            }

    //                            f.IsQued = true;
    //                            f.DateQued = DateTime.Now;
    //                            dqmQueWorker.Save(f);
    //                            ProcessFile(client, f, sourceFile);
    //                            processed++;

    //                            elWrk.SaveEngineLog("Finished DQM for " + client.FtpFolder + " " + DateTime.Now.TimeOfDay.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                            #endregion
    //                        }
    //                        catch (Exception ex)
    //                        {
    //                            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
    //                            if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
    //                                app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
    //                            KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
    //                            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
    //                            alWorker.LogCriticalError(formatException, "ADMS.ADMSProcessingQue.RunDQMForClient_2", app, string.Empty);
    //                        }
    //                    }
    //                    #endregion
    //                }
    //                #endregion
    //            }
    //            RemoveClientDQM(client);
    //        }
    //        catch { }
    //    }
    //    private static void ProcessFile(KMPlatform.Entity.Client client, FrameworkUAS.Entity.DQMQue f, FrameworkUAS.Entity.SourceFile sourceFile)
    //    {
    //        EngineLog = elWrk.Select(client.ClientID, "ADMS");
    //        ADMS.Services.DataCleanser.DQMCleaner dqm = new Services.DataCleanser.DQMCleaner();
    //        elWrk.SaveEngineLog("Starting DQM for " + client.FtpFolder + " " + DateTime.Now.TimeOfDay.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);

    //        //if for some reason readyForDQMFiles is 0 or null lets reset
    //        if (readyForDQMFiles == null || readyForDQMFiles.Count == 0)
    //        {
    //            readyForDQMFiles = new List<ADMSDQMReadyQue>();
    //            //FrameworkUAS.Entity.SourceFile mySf = BillTurner.ClientAdditionalProperties[client.ClientID].SourceFilesList.SingleOrDefault(x => x.SourceFileID == f.SourceFileId);
    //            processCode = f.ProcessCode;
    //            Core.ADMS.Events.FileAddressGeocoded eventMessage = new Core.ADMS.Events.FileAddressGeocoded(new FileInfo(sourceFile.FileName), client, true, true,
    //                                                                true, sourceFile, admsWrk.Select(f.ProcessCode), new FrameworkUAD.Object.ValidationResult());


    //            ADMSDQMReadyQue newItem = new ADMSDQMReadyQue() { Client = eventMessage.Client, EventMessage = eventMessage };
    //            readyForDQMFiles.Add(newItem);
    //        }

    //        ADMSDQMReadyQue item = new ADMSDQMReadyQue();
    //        item = readyForDQMFiles.FirstOrDefault(x => x.EventMessage.AdmsLog.ProcessCode == f.ProcessCode);
    //        if(item != null && item.EventMessage != null && item.EventMessage.AdmsLog != null)
    //            admsLog = item.EventMessage.AdmsLog;//readyForDQMFiles.FirstOrDefault(x => x.EventMessage.AdmsLog.ProcessCode == f.ProcessCode).EventMessage.AdmsLog;
    //        if (admsLog == null)
    //        {
    //            admsLog = new FrameworkUAS.Entity.AdmsLog();
    //            admsLog.ProcessCode = f.ProcessCode;
    //            admsLog.ClientId = client.ClientID;
    //            admsLog.DateCreated = DateTime.Now;
    //            admsLog.DQM = f;
    //            admsLog.SourceFileId = sourceFile.SourceFileID;
    //            FrameworkUAS.BusinessLogic.AdmsLog aWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
    //            admsLog.AdmsLogId = aWrk.Save(admsLog);
    //        }
 

    //        try
    //        {
    //            if (item != null && item.EventMessage != null)
    //            {
    //                elWrk.SaveEngineLog("Start RunStandardization item != null: " + DateTime.Now.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                dqm.RunStandardization(client, admsLog, sourceFile);
    //                elWrk.SaveEngineLog("End RunStandardization item != null: " + DateTime.Now.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                try
    //                {
    //                    ADMS.Services.Emailer.Emailer email = new Services.Emailer.Emailer();

    //                    FileInfo thisFileInfo = item.EventMessage.ImportFile;
    //                    FrameworkUAD.Object.ValidationResult vr = item.EventMessage.ValidationResult;
    //                    Core.ADMS.Events.FileProcessed thisProcessed = new Core.ADMS.Events.FileProcessed(client, sourceFile.SourceFileID, admsLog, thisFileInfo,
    //                                                        item.EventMessage.IsKnownCustomerFileName, item.EventMessage.IsValidFileType, item.EventMessage.IsFileSchemaValid, vr);
    //                    email.SecondaryHandleFileProcessed(thisProcessed);
    //                }
    //                catch (Exception ex)
    //                {
    //                    LogError(ex, client, "ADMS.ADMSProcessingQue.ProcessFile_1", admsLog.ProcessCode);
    //                }
    //            }
    //            else
    //            {
    //                elWrk.SaveEngineLog("Start RunStandardization item null: " + DateTime.Now.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                dqm.RunStandardization(client, admsLog, sourceFile);
    //                elWrk.SaveEngineLog("Start RunStandardization item null: " + DateTime.Now.ToString() + " ProcessCode: " + f.ProcessCode, client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS, EngineLog);
    //                try
    //                {
    //                    ADMS.Services.Emailer.Emailer email = new Services.Emailer.Emailer();
    //                    string filePath = Core.ADMS.BaseDirs.getClientRepoDir() + "//" + client.FtpFolder + "//" + sourceFile.FileName + sourceFile.Extension;
    //                    FileInfo thisFileInfo = new FileInfo(filePath);//sourceFile.FileName + sourceFile.Extension
    //                    FrameworkUAD.Object.ValidationResult vr = new FrameworkUAD.Object.ValidationResult(thisFileInfo, sourceFile.SourceFileID, f.ProcessCode);
    //                    Core.ADMS.Events.FileProcessed thisProcessed = new Core.ADMS.Events.FileProcessed(client, sourceFile.SourceFileID, admsLog, thisFileInfo,
    //                                                        true, true, true, vr);
    //                    email.SecondaryHandleFileProcessed(thisProcessed);
    //                }
    //                catch (Exception ex)
    //                {
    //                    LogError(ex, client, "ADMS.ADMSProcessingQue.ProcessFile_2", admsLog.ProcessCode);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogError(ex, client, "ADMS.ADMSProcessingQue.ProcessFile_3", admsLog.ProcessCode);
    //        }

    //        RemoveDQMReadyFile(f);
    //    }
    //    public static void RemoveDQMReadyFile(FrameworkUAS.Entity.DQMQue dqmQue)
    //    {
    //        //ADMSDQMReadyQue newItem = new ADMSDQMReadyQue() { Client = eventMessage.Client, EventMessage = eventMessage };
    //        ADMSDQMReadyQue item = readyForDQMFiles.FirstOrDefault(x => x.EventMessage.AdmsLog.ProcessCode == dqmQue.ProcessCode);
    //        if (readyForDQMFiles.Contains(item))
    //        {
    //            readyForDQMFiles.Remove(item);
    //        }

    //        //UPDATE RECORD IN DB TO BE COMPLETED
    //        dqmQue.IsCompleted = true;
    //        dqmQue.DateCompleted = DateTime.Now;
    //        FrameworkUAS.BusinessLogic.DQMQue dqmWorker = new FrameworkUAS.BusinessLogic.DQMQue();
    //        dqmWorker.Save(dqmQue);

    //        FrameworkUAS.Entity.AdmsLog admsLog = item.EventMessage.AdmsLog;
    //        FrameworkUAS.BusinessLogic.AdmsLog alWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
    //        alWrk.Update(admsLog.ProcessCode,
    //                     FrameworkUAD_Lookup.Enums.FileStatusType.Completed,
    //                     FrameworkUAD_Lookup.Enums.ADMS_StepType.Processed,
    //                     FrameworkUAD_Lookup.Enums.ProcessingStatusType.Completed,
    //                     FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_DQM, 1, "Remove file from DQM Que", true,
    //                     admsLog.SourceFileId);
    //    }
    //    #endregion

    //    #region Clients Running In DQM
    //    public static void AddToDQMClients(KMPlatform.Entity.Client client)
    //    {
    //        if (!DQMClients.Contains(client))
    //        {
    //            DQMClients.Add(client);
    //        }
    //    }
    //    public static void RemoveToDQMClients(KMPlatform.Entity.Client client)
    //    {
    //        if (DQMClients.Contains(client))
    //        {
    //            DQMClients.Remove(client);
    //        }
    //    }
    //    public static List<KMPlatform.Entity.Client> ReturnDQMClients()
    //    {
    //        return DQMClients;
    //    }
    //    public static bool IsInDQMClients(KMPlatform.Entity.Client client)
    //    {
    //        bool returnValue = false;
    //        if (DQMClients.Exists(x => x.ClientID == client.ClientID))
    //        {
    //            returnValue = true;
    //        }
    //        return returnValue;
    //    }
    //    #endregion
    //}

    //public class ADMSCurrentProcessQue
    //{
    //    public KMPlatform.Entity.Client Client { get; set; }
    //    public FileInfo File { get; set; }
    //}
    //public class ADMSDQMReadyQue
    //{
    //    public KMPlatform.Entity.Client Client { get; set; }
    //    public Core.ADMS.Events.FileAddressGeocoded EventMessage { get; set; }
    //}
}
