using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading;
using ADMS.ClientMethods;
using Core.ADMS;
using Core.ADMS.Events;
using Core_AMS.Utilities;
using FrameworkUAD.BusinessLogic.Helpers;
using FrameworkUAD.Entity;
using FrameworkUAD.Entity.Helpers;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.Entity;
using KM.Common;
using KM.Common.Functions;
using KM.Common.Import;
using KM.Common.Utilities.Email;
using AdmsLogBusiness = FrameworkUAS.BusinessLogic.AdmsLog;
using Enums = Core_AMS.Utilities.Enums;
using ClientEntity = KMPlatform.Entity.Client;
using CommonEnums = KM.Common.Enums;
using FileFunctions = Core_AMS.Utilities.FileFunctions;
using LookupEnums = FrameworkUAD_Lookup.Enums;
using RuleObject = FrameworkUAS.Object.Rule;
using ServiceSavedSubscriber = FrameworkUAD.ServiceResponse.SavedSubscriber;

namespace ADMS.Services.Validator
{
    public class Validator : ServiceBase, IValidator
    {
        #region Public Event
        public event Action<FileValidated> FileValidated;
        public event Action<CustomFileProcessed> CustomFileProcessed;
        #endregion

        #region class variables
        private int standarTypeID;
        private int demoTypeID;
        private int ignoredTypeID;
        private int demoRespOtherTypeID;
        private int demoDateTypeID;
        private int kmTransformTypeID;
        #endregion

        private const int DefaultExitCode = 101;
        private const char CommaSepator = ',';
        private const string DefaultDateTimeFormat = "yyyyMMdd_HHmmss";
        private const string InvalidPath = "\\Invalid\\";
        private const string Underline = "_";

        private const string CoaKey = "coa";
        private const string StdKey = "std";
        private const string CompanyKey = "Company";
        private const string TitleKey = "Title";

        private const string DemographicInvalidListKey = "DemographicInvalidList";
        private const string ToDataTableSubscriberInvalidKey = "ToDataTable_SubscriberInvalid";
        private const string ValidateDataKey = "ADMS.Services.Validator.Validator.ValidateData";

        private const string DemographicTransformedListKey = "DemographicTransformedList";
        private const string ToDataTableDimensionsErrorKey = "ToDataTable_SubscriberTransformed_DimensionErrors";

        private const string ToDataTableSubscriberTransformedKey = "ToDataTable_SubscriberTransformed";

        //Service Entry Point
        public void HandleFileMoved(FileMoved eventMessage)
        {
            try
            {
                string state = Thread.CurrentThread.ThreadState.ToString();
                ThreadDictionary.Set(eventMessage.ThreadId, state);
                ConsoleMessage("Validator-HandleFileMoved: " + eventMessage.ImportFile.Name, eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);

                #region check if file is encrypted
                if (eventMessage.SourceFile.IsPasswordProtected == true)
                    eventMessage.ImportFile = ExtractFile(eventMessage.SourceFile, eventMessage.ImportFile);
                #endregion
                #region check which Service the file is for
                KMPlatform.BusinessLogic.Service servData = new KMPlatform.BusinessLogic.Service();
                KMPlatform.Entity.Service service = servData.Select(eventMessage.SourceFile.ServiceID);
                #endregion

                #region UAD
                if (service.ServiceCode.Equals(KMPlatform.Enums.Services.UADFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase) || service.ServiceCode.Equals(KMPlatform.Enums.Services.UAD.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if (BillTurner.ClientAdditionalProperties == null || !BillTurner.ClientAdditionalProperties.ContainsKey(eventMessage.Client.ClientID))
                    {
                        FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                        FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                        cap = capWork.SetObjects(eventMessage.Client.ClientID, false);

                        if (BillTurner.ClientAdditionalProperties == null)
                        {
                            BillTurner.ClientAdditionalProperties = new Dictionary<int, FrameworkUAS.Object.ClientAdditionalProperties>();
                            BillTurner.ClientAdditionalProperties.Add(eventMessage.Client.ClientID, cap);
                        }
                        else
                        {
                            BillTurner.ClientAdditionalProperties.Add(eventMessage.Client.ClientID, cap);
                        }
                    }

                    #region UAD Files
                    //get which feature the file is for
                    KMPlatform.BusinessLogic.ServiceFeature sfData = new KMPlatform.BusinessLogic.ServiceFeature();
                    KMPlatform.Entity.ServiceFeature feature = sfData.SelectServiceFeature(eventMessage.SourceFile.ServiceFeatureID);

                    if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Standard_Files)
                    {
                        ProcessFileAsObject(eventMessage, false, feature);
                    }
                    else if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Special_Split)
                    {
                        ProcessFileAsObject(eventMessage, false, feature);
                    }
                    else if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                    {
                        ProcessFileAsObject(eventMessage, false, feature);
                    }
                    else if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Special_Files || KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Custom_Processing || KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Custom_DQM_Procedures)
                    {
                        #region Special Files
                        SourceFile cSpecialFile = null;
                        ClientCustomProcedure ccp = null;
                        FrameworkUAD_Lookup.Entity.Code sfr = null; //SpecialFileResult

                        cSpecialFile = eventMessage.SourceFile;
                        if (cSpecialFile != null && cSpecialFile.ClientCustomProcedureID > 0)
                            ccp = BillTurner.ClientAdditionalProperties[eventMessage.Client.ClientID].ClientCustomProceduresList.SingleOrDefault(x => x.ClientCustomProcedureID == cSpecialFile.ClientCustomProcedureID);

                        FrameworkUAD_Lookup.BusinessLogic.Code sfrData = new FrameworkUAD_Lookup.BusinessLogic.Code();
                        if (cSpecialFile.SpecialFileResultID > 0)
                            sfr = sfrData.SelectCodeId(cSpecialFile.SpecialFileResultID);

                        #region do something besides go through ADMS process
                        if (sfr != null && FrameworkUAD_Lookup.Enums.GetSpecialFileResultType(sfr.CodeName) != FrameworkUAD_Lookup.Enums.GetSpecialFileResultType(FrameworkUAD_Lookup.Enums.SpecialFileResultTypes.Creates_Data_Table.ToString()))
                        {
                            Emailer.Emailer emWorker = new Emailer.Emailer();
                            //RUN A CHECK ON cSpecialFile && ccp
                            if (cSpecialFile == null || !(cSpecialFile.ClientCustomProcedureID > 0))
                                emWorker.EmailError("Custom: Warning Process Missing ClientSpecialFile." +
                                                           "\nClient: " + eventMessage.Client.FtpFolder +
                                                           "\nFile: " + eventMessage.SourceFile.FileName);

                            if (ccp == null)
                                emWorker.EmailError("Custom: Warning Process Missing ClientCustomProcedure." +
                                                           "\nClient: " + eventMessage.Client.FtpFolder +
                                                           "\nFile: " + eventMessage.SourceFile.FileName);

                            //special file but not creating a datatable
                            //doing one of 
                            //Creates_Mapped_File,
                            //Adds_Data,
                            //Updates_Data,
                            //Replaces_Data,
                            //Deletes_Data
                            String path = "ADMS.ClientMethods." + eventMessage.Client.FtpFolder;
                            Type clientClass = Type.GetType(path + ",ADMS");
                            if (FrameworkUAD_Lookup.Enums.GetProcedureType(ccp.ProcedureType) == FrameworkUAD_Lookup.Enums.ProcedureTypes.NET)
                            {
                                if (clientClass != null)
                                {
                                    MethodInfo method = clientClass.GetMethod(ccp.ProcedureName, BindingFlags.Instance | BindingFlags.Public);//TitleAdHocImport(ClientSpecialFile cSpecialFile, FileValidated eventMessage)
                                    var instance = Activator.CreateInstance(clientClass);
                                    if (method != null)
                                    {
                                        //ConsoleMessage("Method Fake Run: " + ccp.ProcedureName, true);
                                        method.Invoke(instance, new object[] { eventMessage.Client, cSpecialFile, ccp, eventMessage });
                                        //In_Cleansing
                                        Core.ADMS.Events.CustomFileProcessed cfpMessage = new Core.ADMS.Events.CustomFileProcessed(eventMessage.ImportFile, eventMessage.Client, eventMessage.SourceFile, eventMessage.AdmsLog, true);
                                        CustomFileProcessed(cfpMessage);
                                    }
                                    else
                                    {
                                        ConsoleMessage("Method not found: " + ccp.ProcedureName, eventMessage.AdmsLog.ProcessCode, true);
                                    }
                                }
                                else
                                    ConsoleMessage("Type not found: " + path, eventMessage.AdmsLog.ProcessCode, true);

                            }
                            else
                            {
                                //execute a sql sproc
                                //ccp.ProcedureName
                                ClientSpecialCommon csc = new ClientSpecialCommon();
                                //ConsoleMessage("SQL Fake Run: " + ccp.ProcedureName, true);
                                csc.ExecuteClientSproc(ccp.ProcedureName);
                                //In_Cleansing
                                Core.ADMS.Events.CustomFileProcessed cfpMessage = new Core.ADMS.Events.CustomFileProcessed(eventMessage.ImportFile, eventMessage.Client, eventMessage.SourceFile, eventMessage.AdmsLog, true);
                                CustomFileProcessed(cfpMessage);
                            }
                            ConsoleMessage("Special File processed...." + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode, true);
                            ConsoleMessage("Awaiting new file...." + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode, true);
                            ThreadDictionary.Remove(eventMessage.ThreadId);
                            //ADMSProcessingQue.RemoveClientFile(eventMessage.Client, eventMessage.ImportFile);
                        }
                        #endregion
                        else
                        {
                            ProcessFileAsObject(eventMessage, true, feature, cSpecialFile, sfr);
                        }
                        #endregion
                    }
                    else if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.AdHoc_Dimensions)
                    {
                        Feature.AdHocDimension ahd = new Feature.AdHocDimension();
                        ahd.ProcessFile(eventMessage);
                        ConsoleMessage("Processed AdHocDimension file for client " + eventMessage.Client.FtpFolder, eventMessage.AdmsLog.ProcessCode, true);
                        ADMS.Services.FileMover.FileMover fMover = new FileMover.FileMover();
                        fMover.CopyFileFromClientRepositoryToArchive(eventMessage.ImportFile.FullName, eventMessage.Client, false, eventMessage.AdmsLog.ProcessCode);
                        if (File.Exists(eventMessage.ImportFile.FullName))
                            fMover.DeleteFileFromClientRepository(eventMessage.ImportFile.FullName);
                        else
                        {
                            string repoFile = Core.ADMS.BaseDirs.getClientRepoDir() + "\\" + eventMessage.Client.FtpFolder + "\\" + eventMessage.ImportFile.Name;
                            fMover.DeleteFileFromClientRepository(repoFile);
                        }
                        ConsoleMessage("Awaiting new file....", eventMessage.AdmsLog.ProcessCode, true);
                    }
                    else if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Aggregate_Dimensions)
                    {
                        Feature.AggregateDimension ad = new Feature.AggregateDimension();
                        ad.ProcessFile(eventMessage.AdmsLog);
                    }
                    else if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Consensus_Dimension)
                    {
                        Feature.ConsensusDimension cd = new Feature.ConsensusDimension();
                        cd.ProcessFile(client, eventMessage.SourceFile, eventMessage.ImportFile);
                    }
                    else if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Topic_Dimension)
                    {
                        Feature.TopicDimension td = new Feature.TopicDimension();
                        td.ProcessFile(client, eventMessage.SourceFile, eventMessage.ImportFile);
                    }
                    else if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Address_Update)
                    {
                        FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                        FrameworkUAD_Lookup.Entity.Code dbft = cWorker.SelectCodeId(eventMessage.SourceFile.DatabaseFileTypeId);
                        FrameworkUAD_Lookup.Enums.FileTypes dft = FrameworkUAD_Lookup.Enums.GetDatabaseFileType(dbft.CodeName);

                        if (dft == FrameworkUAD_Lookup.Enums.FileTypes.ACS)
                        {
                            #region ACS files
                            #region steps
                            /*
                          High level steps
                          DONE - 1. Download customer zip files
                          DONE - 2. Unzip files
                          DONE - 3. reports get printed or saved - we will upload data to a temp table then archive file when done
                          DONE - 4. Get all client data into one "file" - this will be a single temp table
                          5. fill in pubcode, category, transaction, sequence and breakout
                          6. purge out bad data
                          DONE - 7. get billing detail report - save and email
                          8. break out by pubcode, match with Intelligent Mail Barcode - IMB's are matched back to get seq#'s
                          9. match address to USPS - update
                          10. po kills - matching by seq#
                          
                          Login at https://epf.usps.gov/
                                Login:
                                Email = phil.kreun@teamkm.com
                                Password = KMPwat55

                        */
                            #endregion
                            try
                            {
                                ACS_UnZipFile(eventMessage.ImportFile, eventMessage.Client);
                                ACS_ExtractDataFromFiles(eventMessage);

                                List<FrameworkUAD.Entity.AcsFileDetail> acsList = ACS_UAD_SetValues(eventMessage);
                                ACS_UAD_UpdateSubscriber(eventMessage, acsList);

                                //ADMSProcessingQue.RemoveClientFile(eventMessage.Client, eventMessage.ImportFile);

                                ConsoleMessage("ACS File Done", eventMessage.AdmsLog.ProcessCode);
                            }
                            catch (Exception ex)
                            {
                                #region Log Error
                                string msg = "ADMS.Services.Validator.Validator.ACS File Import - Unhandled Exception";
                                LogError(ex, eventMessage.Client, msg);
                                #endregion
                            }
                            #region comments
                            /*so now we have the data from the files into our database
                            What we want to now do is set sequence number, categoryCode and TransactionCode 
                                For each AcsFileDetail record
                                    set the SequenceID
                            match back to Subscription table by SequenceID
                            
                            if PO_OldAddress = KM_Address then Update KM to PO_New
                            if PO_OldAddress != KM_Address then ignore
                            if PO has no address then KM = KILL 32 POKill
                         
                        */

                            /*Update properties on the AcsFileDetail table
                             1. SequenceID
                             2. TransactionCodeId
                             3. CategoryCodeId
                             4. ProductCode (pubcode)
                             5. OldAddress1
                             6. OldAddress2
                             7. OldAddress3
                             8. NewAddress1
                             9. NewAddress2
                             10. NewAddress3
                         
                              */

                            //Once AcsFileDetail table is updated then we match back to our Subscription table on SequenceID  and change the ActionID_Current based on Tran/Cat codes
                            //then from Subscription table match to Subscriber table and update address
                            #endregion
                            #endregion
                        }
                        else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.NCOA)
                        {
                            #region NCOA
                            #region notes
                            /*
                          1.  FTP site link and credentials (username/password) 
                              Ftp site link:   secureftp.peachtreedata.com
                              Username:   kno003
                              Password:  pk22pc
                          2.  Example of what you last sent them
                              We sent them this zip file LS00356.fast.zip
                              Ls00356.fast.zip   has files  Fastinput.txt, fastservices.txt and ls00356.dbf.  
                          3.  Example of what we then got back from submitting #2
                        
                          We received ls00356-output.zip back from Peachtree.
                          Ls00356-output.zip has ncoa reports all of the .pdf files  and ls00356.dbf

                          For your info we have a field in ls00356 called ncoaseq.  This field is really our sequence # from our subfiles.
                        */

                            //will be a csv file
                            //parse file out to datatable
                            //get columns ncoaseq,std_line1,std_line2,std_city,std_state,std_zip,std_zip4
                            //update Subscriber table address matching Subscription.SequenceID to ncoaseq
                            //create xml and send to Database job_NCOA_AddressUpdate
                            #endregion
                            try
                            {
                                DataTable dt = NcoaParseFile(eventMessage);
                                //group out datatable by pubcode - process by pubcode
                                var result = from row in dt.AsEnumerable()
                                             group row by row.Field<string>("pubcode") into grp
                                             select new
                                             {
                                                 PubCode = grp.Key,
                                                 MemberCount = grp.Count()
                                             };
                                foreach (var r in result)
                                {
                                    SendXmlToDatabase(eventMessage, dt, r.PubCode.ToString(), StdKey);
                                }

                                //ADMSProcessingQue.RemoveClientFile(eventMessage.Client, eventMessage.ImportFile);
                                ConsoleMessage("NCOA File Done", eventMessage.AdmsLog.ProcessCode);
                            }
                            catch (Exception ex)
                            {
                                #region Log Error
                                string msg = "ADMS.Services.Validator.Validator.Ncoa_UAD_UpdateAddress - Unhandled Exception";
                                LogError(ex, eventMessage.Client, msg);
                                #endregion
                            }
                            #endregion
                        }
                        else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Long_Form || dft == FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Short_Form)
                        {
                            ProcessFileAsObject(eventMessage, false, feature);
                        }
                    }
                    #endregion
                }
                #endregion
                #region Circulation
                else if (service.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    bool rejectFile = false;
                    //get which feature the file is for
                    KMPlatform.BusinessLogic.ServiceFeature sfData = new KMPlatform.BusinessLogic.ServiceFeature();
                    KMPlatform.Entity.ServiceFeature feature = sfData.SelectServiceFeature(eventMessage.SourceFile.ServiceFeatureID);

                    ConsoleMessage("Checking Circ File Requirements: " + eventMessage.AdmsLog.ProcessCode.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                    #region Process Circ
                    if (!rejectFile)
                    {
                        if (KMPlatform.BusinessLogic.Enums.GetFulfillmentFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.FulfillmentFeatures.Address_Update)
                        {
                            FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                            Code dbft = cWorker.SelectCodeId(eventMessage.SourceFile.DatabaseFileTypeId);
                            FrameworkUAD_Lookup.Enums.FileTypes dft = FrameworkUAD_Lookup.Enums.GetDatabaseFileType(dbft.CodeName);

                            if (dft == FrameworkUAD_Lookup.Enums.FileTypes.ACS)
                            {
                                #region ACS files
                                #region steps
                                /*
                            High level steps
                            DONE - 1. Download customer zip files
                            DONE - 2. Unzip files
                            DONE - 3. reports get printed or saved - we will upload data to a temp table then archive file when done
                            DONE - 4. Get all client data into one "file" - this will be a single temp table
                            5. fill in pubcode, category, transaction, sequence and breakout
                            6. purge out bad data
                            DONE - 7. get billing detail report - save and email
                            8. break out by pubcode, match with Intelligent Mail Barcode - IMB's are matched back to get seq#'s
                            9. match address to USPS - update
                            10. po kills - matching by seq#
                          
                            Login at https://epf.usps.gov/
                                Login:
                                Email = phil.kreun@teamkm.com
                                Password = KMPwat55

                        */
                                #endregion
                                try
                                {
                                    ConsoleMessage("ACS File Start - " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode);
                                    //ADMSProcessingQue.RemoveClientFile(eventMessage.Client, eventMessage.ImportFile);

                                    ACS_UnZipFile(eventMessage.ImportFile, eventMessage.Client);
                                    ACS_ExtractDataFromFiles(eventMessage);

                                    //KMPlatform.BusinessLogic.Client pubWorker = new KMPlatform.BusinessLogic.Client();
                                    //KMPlatform.Entity.Client pubList = pubWorker.Select(eventMessage.Client.ClientID);
                                    FrameworkUAD.BusinessLogic.Product pWorker = new FrameworkUAD.BusinessLogic.Product();
                                    List<FrameworkUAD.Entity.Product> publicationList = new List<FrameworkUAD.Entity.Product>();
                                    //foreach (KMPlatform.Entity.Client p in pubList)
                                    //{
                                    //    publicationList.AddRange(pWorker.Select(p));
                                    //}

                                    publicationList.AddRange(pWorker.Select(eventMessage.Client.ClientConnections));

                                    List<FrameworkUAD.Entity.AcsFileDetail> acsList = ACS_SetValues(eventMessage, publicationList);
                                    ACS_UpdateSubscriber(eventMessage, acsList, publicationList);

                                    //Call E_IMPORTFROMUAS
                                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Start data transfer to Master UAD for client: " + eventMessage.Client.FtpFolder, eventMessage.AdmsLog.ProcessCode, true);
                                    ADMS.Services.UAD.UADProcessor uadProcessor = new UAD.UADProcessor();

                                    uadProcessor.ImportToUAD(eventMessage.Client, eventMessage.AdmsLog, dft, eventMessage.SourceFile);

                                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Data successfully updated in Master UAD for client: " + eventMessage.Client.FtpFolder, eventMessage.AdmsLog.ProcessCode, true);

                                    ADMS.Services.FileMover.FileMover fMover = new FileMover.FileMover();
                                    fMover.CopyFileFromClientRepositoryToArchive(eventMessage.ImportFile.FullName, eventMessage.Client, false, eventMessage.AdmsLog.ProcessCode);
                                    string fileToDelete = eventMessage.ImportFile.FullName;
                                    if (File.Exists(fileToDelete))
                                        fMover.DeleteFileFromClientRepository(eventMessage.ImportFile.FullName);
                                    else
                                    {
                                        string repoFile = Core.ADMS.BaseDirs.getClientRepoDir() + "\\" + eventMessage.Client.FtpFolder + "\\" + eventMessage.ImportFile.Name;
                                        fMover.DeleteFileFromClientRepository(repoFile);
                                    }
                                    ThreadDictionary.Remove(eventMessage.ThreadId);
                                    //ADMSProcessingQue.RemoveClientFile(eventMessage.Client, eventMessage.ImportFile);

                                    ADMS.Services.Emailer.Emailer emailer = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.ACSFileComplete);
                                    emailer.ACSFileComplete(eventMessage.Client, eventMessage.SourceFile.SourceFileID, eventMessage.ImportFile.Name, eventMessage.AdmsLog.ProcessCode);

                                    ConsoleMessage("ACS File Done - " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode);
                                }
                                catch (Exception ex)
                                {
                                    LogError(ex, client, "ADMS.Services.Validator.Validator.ACS File Import - Unhandled Exception");
                                }
                                #region comments
                                /*so now we have the data from the files into our database
                            What we want to now do is set sequence number, categoryCode and TransactionCode 
                                For each AcsFileDetail record
                                    set the SequenceID
                            match back to Subscription table by SequenceID
                            
                            if PO_OldAddress = KM_Address then Update KM to PO_New
                            if PO_OldAddress != KM_Address then ignore
                            if PO has no address then KM = KILL 32 POKill
                         
                        */

                                /*Update properties on the AcsFileDetail table
                                    1. SequenceID
                                    2. TransactionCodeId
                                    3. CategoryCodeId
                                    4. ProductCode (pubcode)
                                    5. OldAddress1
                                    6. OldAddress2
                                    7. OldAddress3
                                    8. NewAddress1
                                    9. NewAddress2
                                    10. NewAddress3
                         
                                    */

                                //Once AcsFileDetail table is updated then we match back to our Subscription table on SequenceID  and change the ActionID_Current based on Tran/Cat codes
                                //then from Subscription table match to Subscriber table and update address
                                #endregion
                                #endregion
                            }
                            else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.NCOA)
                            {
                                ConsoleMessage("NCOA File Detected: " + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode);
                                #region NCOA
                                #region notes
                                /*
                                    1.  FTP site link and credentials (username/password) 
                                        Ftp site link:   secureftp.peachtreedata.com
                                        Username:   kno003
                                        Password:  pk22pc
                                    2.  Example of what you last sent them
                                        We sent them this zip file LS00356.fast.zip
                                        Ls00356.fast.zip   has files  Fastinput.txt, fastservices.txt and ls00356.dbf.  
                                    3.  Example of what we then got back from submitting #2
                        
                                    We received ls00356-output.zip back from Peachtree.
                                    Ls00356-output.zip has ncoa reports all of the .pdf files  and ls00356.dbf

                                    For your info we have a field in ls00356 called ncoaseq.  This field is really our sequence # from our subfiles.
                                */

                                //will be a csv file
                                //parse file out to datatable
                                //get columns ncoaseq,std_line1,std_line2,std_city,std_state,std_zip,std_zip4
                                //update Subscriber table address matching Subscription.SequenceID to ncoaseq
                                //create xml and send to Database job_NCOA_AddressUpdate
                                #endregion
                                try
                                {
                                    DataTable dt = NcoaParseFile(eventMessage);
                                    //group out datatable by pubcode - process by pubcode
                                    #region Check Standard Columns Exist
                                    if ((dt.Columns.Contains("ncoaseq") == false)
                                        || (dt.Columns.Contains("coa_line1") == false)
                                        || (dt.Columns.Contains("coa_line2") == false)
                                        || (dt.Columns.Contains("coa_city") == false)
                                        || (dt.Columns.Contains("coa_state") == false)
                                        || (dt.Columns.Contains("coa_zip") == false)
                                        || (dt.Columns.Contains("coa_zip4") == false)
                                        || (dt.Columns.Contains("pubcode") == false))
                                    {
                                        #region Log and Error Missing Column
                                        ThreadDictionary.Remove(eventMessage.ThreadId);
                                        //ADMSProcessingQue.RemoveClientFile(eventMessage.Client, eventMessage.ImportFile);

                                        StringBuilder sbDetail = new StringBuilder();
                                        sbDetail.AppendLine("Client: " + eventMessage.Client.FtpFolder);
                                        sbDetail.AppendLine("File: " + eventMessage.ImportFile.FullName);
                                        sbDetail.AppendLine("SourceFileID: " + eventMessage.SourceFile.SourceFileID.ToString());
                                        sbDetail.AppendLine("Thread: " + eventMessage.ThreadId.ToString());
                                        sbDetail.AppendLine("ProcessCode: " + eventMessage.AdmsLog.ProcessCode.ToString());
                                        sbDetail.AppendLine("ADMS.Services.Validator.Validator.NcoaUpdateAddress - Missing Column Exception");
                                        if (dt.Columns.Contains("ncoaseq") == false)
                                            sbDetail.AppendLine("Missing Column - ncoaseq");

                                        if (dt.Columns.Contains("coa_line1") == false)
                                            sbDetail.AppendLine("Missing Column - coa_line1");

                                        if (dt.Columns.Contains("coa_line2") == false)
                                            sbDetail.AppendLine("Missing Column - coa_line2");

                                        if (dt.Columns.Contains("coa_city") == false)
                                            sbDetail.AppendLine("Missing Column - coa_city");

                                        if (dt.Columns.Contains("coa_state") == false)
                                            sbDetail.AppendLine("Missing Column - coa_state");

                                        if (dt.Columns.Contains("coa_zip") == false)
                                            sbDetail.AppendLine("Missing Column - coa_zip");

                                        if (dt.Columns.Contains("coa_zip4") == false)
                                            sbDetail.AppendLine("Missing Column - coa_zip4");

                                        if (dt.Columns.Contains("pubcode") == false)
                                            sbDetail.AppendLine("Missing Column - pubcode");


                                        ConsoleMessage(sbDetail.ToString());
                                        LogError(new Exception(), client, sbDetail.ToString());

                                        #endregion
                                    }
                                    #endregion

                                    var result = from row in dt.AsEnumerable()
                                                 group row by row.Field<string>("pubcode") into grp
                                                 select new
                                                 {
                                                     PubCode = grp.Key,
                                                     MemberCount = grp.Count()
                                                 };
                                    foreach (var r in result)
                                    {
                                        SendXmlToDatabase(eventMessage, dt, r.PubCode.ToString(), CoaKey);
                                    }

                                    //Call the DataMatching sproc that matches on Sequence Number
                                    ConsoleMessage("Start NCOA Data Matching: " + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode);
                                    FrameworkUAD.BusinessLogic.SubscriberTransformed stWorker = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
                                    stWorker.SequenceDataMatching(eventMessage.Client.ClientConnections, eventMessage.AdmsLog.ProcessCode);
                                    ConsoleMessage("End NCOA Data Matching: " + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode);

                                    //Call E_IMPORTFROMUAS
                                    ConsoleMessage("Start Update To Master: " + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode);
                                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Start data transfer to Master UAD for client: " + eventMessage.Client.FtpFolder, eventMessage.AdmsLog.ProcessCode, true);
                                    ADMS.Services.UAD.UADProcessor uadProcessor = new UAD.UADProcessor();
                                    uadProcessor.ImportToUAD(eventMessage.Client, eventMessage.AdmsLog, dft, eventMessage.SourceFile);
                                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Data successfully updated in Master UAD for client: " + eventMessage.Client.FtpFolder, eventMessage.AdmsLog.ProcessCode, true);

                                    //Send NCOA complete email
                                    ADMS.Services.Emailer.Emailer emailer = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.NCOAFileComplete);
                                    emailer.NCOAFileComplete(eventMessage.Client, eventMessage.SourceFile.SourceFileID, eventMessage.ImportFile.Name, eventMessage.AdmsLog.ProcessCode);

                                    //Move the file to Archive
                                    ConsoleMessage("Start Archiving NCOA File", eventMessage.AdmsLog.ProcessCode);
                                    ADMS.Services.FileMover.FileMover fileMoverWorker = new FileMover.FileMover();
                                    fileMoverWorker.CopyFileFromClientRepositoryToArchive(eventMessage.ImportFile.FullName, eventMessage.Client, true, eventMessage.AdmsLog.ProcessCode);
                                    if (File.Exists(eventMessage.ImportFile.FullName))
                                        fileMoverWorker.DeleteFileFromClientRepository(eventMessage.ImportFile.FullName);
                                    else
                                    {
                                        string repoFile = Core.ADMS.BaseDirs.getClientRepoDir() + "\\" + eventMessage.Client.FtpFolder + "\\" + eventMessage.ImportFile.Name;
                                        fileMoverWorker.DeleteFileFromClientRepository(repoFile);
                                    }
                                    ConsoleMessage("End Archiving NCOA File", eventMessage.AdmsLog.ProcessCode);
                                    ThreadDictionary.Remove(eventMessage.ThreadId);
                                    //ADMSProcessingQue.RemoveClientFile(eventMessage.Client, eventMessage.ImportFile);
                                    ConsoleMessage("NCOA File Done", eventMessage.AdmsLog.ProcessCode);
                                }
                                catch (Exception ex)
                                {
                                    #region Log Error
                                    string msg = "ADMS.Services.Validator.Validator.NcoaUpdateAddress - Unhandled Exception";
                                    LogError(ex, eventMessage.Client, msg);
                                    #endregion
                                }
                                #endregion
                            }
                        }

                        if (KMPlatform.BusinessLogic.Enums.GetFulfillmentFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.FulfillmentFeatures.File_Import)
                        {
                            //I would guess that first we can go through a common process to get the data out of the file.
                            //then depending what file is do certain things or apply special rules.

                            //now we know this is a Circ file - next find out what type
                            FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                            Code dbft = cWorker.SelectCodeId(eventMessage.SourceFile.DatabaseFileTypeId);
                            FrameworkUAD_Lookup.Enums.FileTypes dft = FrameworkUAD_Lookup.Enums.GetDatabaseFileType(dbft.CodeName);

                            if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Other)
                            {
                                //New Prospect Additions
                                ProcessFileAsObject(eventMessage, false, feature);
                            }
                            else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Long_Form || dft == FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Short_Form)
                            {
                                ProcessFileAsObject(eventMessage, false, feature);
                            }
                            else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_2YR || dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_3YR || dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_Other)
                            {
                                ProcessFileAsObject(eventMessage, false, feature);
                            }
                            else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Field_Update)
                            {
                                ProcessFileAsObject(eventMessage, false, feature, null, null, FrameworkUAD_Lookup.Enums.FileTypes.Field_Update.ToString());
                            }
                            else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms || dft == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms_Short_Form)
                            {
                                ProcessFileAsObject(eventMessage, false, feature);
                            }
                            else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.QuickFill || dft == FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction)
                            {
                                ProcessFileAsObject(eventMessage, false, feature);
                            }
                            else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Audience_Data)
                            {
                                ProcessFileAsObject(eventMessage, false, feature);
                            }
                            else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Complimentary)
                            {
                                ProcessFileAsObject(eventMessage, false, feature);
                            }
                        }
                    }
                    else
                    {
                        ADMS.Services.FileMover.FileMover fMover = new FileMover.FileMover();
                        fMover.CopyFileFromClientRepositoryToArchive(eventMessage.ImportFile.FullName, eventMessage.Client, false, eventMessage.AdmsLog.ProcessCode);
                        if (File.Exists(eventMessage.ImportFile.FullName))
                            fMover.DeleteFileFromClientRepository(eventMessage.ImportFile.FullName);
                        else
                        {
                            string repoFile = Core.ADMS.BaseDirs.getClientRepoDir() + "\\" + eventMessage.Client.FtpFolder + "\\" + eventMessage.ImportFile.Name;
                            fMover.DeleteFileFromClientRepository(repoFile);
                        }
                        ThreadDictionary.Remove(eventMessage.ThreadId);
                        //ADMSProcessingQue.RemoveClientFile(eventMessage.Client, eventMessage.ImportFile);
                    }
                    #endregion
                }
                #endregion
                else
                {
                    #region process file for some other service we offer - Fulfillment,Email_Marketing, Forms
                    //get the method and step into it
                    SourceFile cSpecialFile = eventMessage.SourceFile;

                    Type clientClass = Type.GetType("ADMS.ClientMethods." + eventMessage.Client.FtpFolder);

                    FrameworkUAS.BusinessLogic.ClientCustomProcedure ccpWorker = new FrameworkUAS.BusinessLogic.ClientCustomProcedure();
                    List<ClientCustomProcedure> ccps = ccpWorker.SelectClient(eventMessage.Client.ClientID);
                    ClientCustomProcedure ccp = ccps.Single(x => x.ClientCustomProcedureID == cSpecialFile.ClientCustomProcedureID);

                    if (FrameworkUAD_Lookup.Enums.GetProcedureType(ccp.ProcedureType) == FrameworkUAD_Lookup.Enums.ProcedureTypes.NET)
                    {
                        if (clientClass != null)
                        {
                            MethodInfo method = clientClass.GetMethod(ccp.ProcedureName, BindingFlags.Public);

                            if (method != null)
                                method.Invoke(null, new object[] { eventMessage.Client, eventMessage.SourceFile, cSpecialFile, ccp });
                            else
                            {
                                ConsoleMessage("Method not found: " + ccp.ProcedureName, eventMessage.AdmsLog.ProcessCode, true);
                            }
                        }
                        else
                            ConsoleMessage("Type not found: ADMS.ClientMethods." + eventMessage.Client.FtpFolder, eventMessage.AdmsLog.ProcessCode, true);

                    }
                    else
                    {
                        //execute a sql sproc
                        ClientSpecialCommon csc = new ClientSpecialCommon();
                        csc.ExecuteClientSproc(ccp.ProcedureName);
                    }
                    ThreadDictionary.Remove(eventMessage.ThreadId);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region Log Error
                string msg = "ADMS.Services.Validator.Validator.NcoaUpdateAddress - Unhandled Exceptio";
                LogError(ex, eventMessage.Client, msg);
                #endregion
            }
            finally
            {
                ThreadDictionary.Remove(eventMessage.ThreadId);
                //ADMSProcessingQue.RemoveClientFile(eventMessage.Client, eventMessage.ImportFile);
            }
        }

        private void SendXmlToDatabase(FileMoved eventMessage, DataTable dtNcoa, string pubCode, string type)
        {
            Guard.NotNull(eventMessage, nameof(eventMessage));
            Guard.NotNull(dtNcoa, nameof(dtNcoa));

            var pubList = new List<NCOA>();
            var pubRows = dtNcoa.Select($"pubcode = \'{pubCode}\'").ToList();

            foreach (var row in pubRows)
            {
                int parseSequenceId;
                var resultParse = int.TryParse(row["ncoaseq"].ToString().Trim(), out parseSequenceId);

                pubList.Add(new NCOA
                {
                    SequenceID = parseSequenceId,
                    Address1 = row[$"{type}_line1"].ToString().Trim(),
                    Address2 = row[$"{type}_line2"].ToString().Trim(),
                    City = row[$"{type}_city"].ToString().Trim(),
                    RegionCode = row[$"{type}_state"].ToString().Trim(),
                    ZipCode = row[$"{type}_zip"].ToString().Trim(),
                    Plus4 = row[$"{type}_zip4"].ToString().Trim(),
                    ProductCode = row["pubcode"].ToString().Trim(),
                    PublisherID = 0,
                    PublicationID = 0,
                    ProcessCode = eventMessage.AdmsLog.ProcessCode
                });
            }

            var xml = type.Equals(StdKey) 
                ? Ncoa_UAD_GetXml(pubList) 
                : NcoaGetXml(pubList, eventMessage.Client);

            ConsoleMessage($"Start NCOA Update: {DateTime.Now.ToString()}", eventMessage.AdmsLog.ProcessCode);
            Ncoa_UAD_UpdateAddress(xml, eventMessage.Client, eventMessage.SourceFile.SourceFileID);
            ConsoleMessage($"End NCOA Update: {DateTime.Now.ToString()}", eventMessage.AdmsLog.ProcessCode);
            Ncoa_to_SubGen(pubList, eventMessage.Client, pubCode, eventMessage.AdmsLog.ProcessCode);
        }

        private FileInfo ExtractFile(FrameworkUAS.Entity.SourceFile sf, FileInfo file)
        {
            string dir = file.DirectoryName + "\\";

            Core_AMS.Utilities.FileFunctions ff = new FileFunctions();
            ff.ExtractZipFile(file, dir, sf.ProtectionPassword);
            File.Delete(file.FullName);
            return new FileInfo(dir + sf.FileName + sf.Extension);
        }
        public Dictionary<string, FrameworkUAD.Entity.Product> GetDistinctFilePubCodeList(FileInfo importFile, FileConfiguration fileConfig, KMPlatform.Object.ClientConnections clientConnections, List<FrameworkUAS.Entity.FieldMapping> fieldMappings)
        {
            Dictionary<string, FrameworkUAD.Entity.Product> returnPubCodeAndProduct = new Dictionary<string, FrameworkUAD.Entity.Product>();

            #region Get products for client
            FrameworkUAD.BusinessLogic.Product prodData = new FrameworkUAD.BusinessLogic.Product();
            List<FrameworkUAD.Entity.Product> products = prodData.Select(clientConnections);
            #endregion

            #region Get ImportFile
            FrameworkUAD.BusinessLogic.ImportFile importFileWorker = new FrameworkUAD.BusinessLogic.ImportFile();
            FrameworkUAD.Object.ImportFile dataIV = new FrameworkUAD.Object.ImportFile();
            FileWorker fileWorker = new FileWorker();
            if (fileWorker.AcceptableFileType(importFile) == true)
                dataIV = importFileWorker.GetImportFile(importFile, fileConfig);

            #endregion

            #region Retrieve PubCodes And Append Product
            FrameworkUAS.Entity.FieldMapping fm = fieldMappings.SingleOrDefault(x => x.MAFField.Equals("PubCode", StringComparison.CurrentCultureIgnoreCase));

            if (fm != null)
            {
                foreach (var key in dataIV.DataOriginal.Keys)
                {
                    StringDictionary myRow = dataIV.DataOriginal[key];
                    if (myRow.ContainsKey(fm.IncomingField))
                    {
                        if (!string.IsNullOrEmpty(myRow[fm.IncomingField].ToString()))
                        {
                            string pubCode = myRow[fm.IncomingField].ToString();
                            if (!returnPubCodeAndProduct.ContainsKey(pubCode))
                            {
                                FrameworkUAD.Entity.Product product = products.FirstOrDefault(x => x.PubCode.Equals(pubCode, StringComparison.CurrentCultureIgnoreCase));
                                returnPubCodeAndProduct.Add(pubCode, product);
                            }
                        }
                    }
                }
            }
            #endregion

            return returnPubCodeAndProduct;
        }
        private FrameworkUAD.Object.ImportFile SpecialFileGetImportFile(FrameworkUAS.Entity.SourceFile cSpecialFile, FileMoved eventMessage)
        {
            FrameworkUAD.Object.ImportVessel newIV = new FrameworkUAD.Object.ImportVessel();

            Type clientClass = Type.GetType("ADMS.ClientMethods." + eventMessage.Client.FtpFolder);
            FrameworkUAS.BusinessLogic.ClientCustomProcedure ccpWorker = new FrameworkUAS.BusinessLogic.ClientCustomProcedure();
            List<ClientCustomProcedure> ccps = ccpWorker.SelectClient(eventMessage.Client.ClientID);
            ClientCustomProcedure ccp = ccps.Single(x => x.ClientCustomProcedureID == cSpecialFile.ClientCustomProcedureID);

            if (FrameworkUAD_Lookup.Enums.GetProcedureType(ccp.ProcedureType) == FrameworkUAD_Lookup.Enums.ProcedureTypes.NET)
            {
                if (clientClass != null)
                {
                    MethodInfo method = clientClass.GetMethod(ccp.ProcedureName, BindingFlags.Public);

                    if (method != null)
                        newIV = (FrameworkUAD.Object.ImportVessel) method.Invoke(null, new object[] { eventMessage.Client, eventMessage.SourceFile, cSpecialFile, ccp });
                    else
                    {
                        ConsoleMessage("Method not found: " + ccp.ProcedureName, eventMessage.AdmsLog.ProcessCode, true);
                    }
                }
                else
                    ConsoleMessage("Type not found: ADMS.ClientMethods." + eventMessage.Client.FtpFolder, eventMessage.AdmsLog.ProcessCode, true);
            }
            else
            {
                //execute a sql sproc
                //ccp.ProcedureName
                ClientSpecialCommon csc = new ClientSpecialCommon();
                newIV = csc.ExecuteClientSproc(ccp.ProcedureName);
            }

            FrameworkUAD.BusinessLogic.ImportFile ifWorker = new FrameworkUAD.BusinessLogic.ImportFile();
            FrameworkUAD.Object.ImportFile newIF = ifWorker.ConvertImportVesselToImportFile(newIV);

            return newIF;
        }
        private FrameworkUAD.Object.ImportFile SpecialFileAlterData(FrameworkUAS.Entity.SourceFile cSpecialFile, FileMoved eventMessage, FrameworkUAD.Object.ImportFile dataIV)
        {
            Type clientClass = Type.GetType("ADMS.ClientMethods." + eventMessage.Client.FtpFolder);
            FrameworkUAS.BusinessLogic.ClientCustomProcedure ccpWorker = new FrameworkUAS.BusinessLogic.ClientCustomProcedure();
            List<ClientCustomProcedure> ccps = ccpWorker.SelectClient(eventMessage.Client.ClientID);
            ClientCustomProcedure ccp = ccps.Single(x => x.ClientCustomProcedureID == cSpecialFile.ClientCustomProcedureID);

            if (FrameworkUAD_Lookup.Enums.GetProcedureType(ccp.ProcedureType) == FrameworkUAD_Lookup.Enums.ProcedureTypes.NET)
            {
                if (clientClass != null)
                {
                    MethodInfo method = clientClass.GetMethod(ccp.ProcedureName, BindingFlags.Public);

                    if (method != null)
                        dataIV = (FrameworkUAD.Object.ImportFile) method.Invoke(null, new object[] { eventMessage.Client, eventMessage.SourceFile, cSpecialFile, ccp });
                    else
                    {
                        ConsoleMessage("Method not found: " + ccp.ProcedureName, eventMessage.AdmsLog.ProcessCode, true);
                    }
                }
                else
                    ConsoleMessage("Type not found: ADMS.ClientMethods." + eventMessage.Client.FtpFolder, eventMessage.AdmsLog.ProcessCode, true);
            }
            else
            {
                //execute a sql sproc
                //ccp.ProcedureName
                ClientSpecialCommon csc = new ClientSpecialCommon();
                dataIV = csc.ExecuteClientSproc_ImportFile(ccp.ProcedureName);
            }

            return dataIV;
        }

        #region Fulfillment
        #region ACS
        private void ACS_UnZipFile(System.IO.FileInfo fileInfo, KMPlatform.Entity.Client client)
        {
            Core_AMS.Utilities.FileFunctions ff = new FileFunctions();
            string zipDir = Core.ADMS.BaseDirs.GetFulfillmentZipDir() + "\\" + client.FtpFolder + "\\";// +System.IO.Path.GetFileNameWithoutExtension(fileInfo.Name) + "\\";

            if (!System.IO.Directory.Exists(zipDir))
                System.IO.Directory.CreateDirectory(zipDir);
            //delete any existing files
            if (System.IO.Directory.Exists(zipDir + System.IO.Path.GetFileNameWithoutExtension(fileInfo.Name) + "\\"))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(zipDir + System.IO.Path.GetFileNameWithoutExtension(fileInfo.Name) + "\\");
                FileInfo[] files = dirInfo.GetFiles();

                foreach (FileInfo file in files)
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, client, this.GetType().Name.ToString() + ".ACS_UnZipFile");
                    }
                }
            }
            //so now we have to get the ClientConfiguration ACS Account Password CodeTypeId = 4
            FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            Code acsActPass = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Configuration, FrameworkUAD_Lookup.Enums.ConfigurationTypes.ACS_Account_Password.ToString());
            string password = string.Empty;
            if (client.ClientConfigurations.Exists(x => x.CodeTypeId == acsActPass.CodeTypeId && x.CodeId == acsActPass.CodeId))
                password = client.ClientConfigurations.SingleOrDefault(x => x.CodeTypeId == acsActPass.CodeTypeId && x.CodeId == acsActPass.CodeId).ClientValue;
            bool isDone = ff.ExtractZipFile(fileInfo, zipDir, password);
            if (isDone == false)
                ff.ExtractZipFile(fileInfo, zipDir, "@0&1Y0A2D0S5E4U3t9");//this is the default Everyone password

        }
        private void ACS_ExtractDataFromFiles(FileMoved eventMessage)
        {
            string zipDir = Core.ADMS.BaseDirs.GetFulfillmentZipDir() + "\\" + eventMessage.Client.FtpFolder + "\\" + System.IO.Path.GetFileNameWithoutExtension(eventMessage.ImportFile.Name);
            DirectoryInfo dirInfo = new DirectoryInfo(zipDir);
            FileInfo[] files = dirInfo.GetFiles();
            //rename with .txt file extension
            foreach (FileInfo file in files)
            {
                try
                {
                    int periodIndex = file.Name.IndexOf('.') + 1;
                    string falseExtension = file.Name.Substring(periodIndex, 3);
                    string fullName = file.DirectoryName + "\\" + falseExtension + file.Name.Remove(periodIndex, 3) + "txt";
                    file.CopyTo(fullName);
                    file.Delete();
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ACS_ExtractDataFromFiles");
                }
            }

            files = dirInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                string fileName = file.Name.Replace(".txt", "");
                if (!fileName.Contains("."))
                {
                    if (fileName.EndsWith("SN"))
                    {
                        #region send file as email attachment
                        //SN file just email to Joe and Account Managers
                        KMPlatform.BusinessLogic.User uWorker = new KMPlatform.BusinessLogic.User();
                        List<KMPlatform.Entity.User> users = uWorker.Select(0, KMPlatform.BusinessLogic.Enums.KmSecurityGroups.Account_Managers.ToString(), false);
                        List<string> emailTo = new List<string>();
                        //emailTo.Add("joe.benson@teamkm.com");
                        //foreach (var u in users)
                        //    emailTo.Add(u.EmailAddress);
                        emailTo.Add("pat.connell@teamkm.com");
                        emailTo.Add("justin.wagner@teamkm.com");
                        emailTo.Add("jason.meier@teamkm.com");
                        emailTo.Add("meghan.salim@teamkm.com");
                        List<Attachment> listAtt = new List<Attachment>();
                        listAtt.Add(new Attachment(file.FullName));
                        string subject = "ACS Report for " + eventMessage.Client.FtpFolder;

                        var emailService = new EmailService(new EmailClient(), new ConfigurationProvider());
                        var emailMessage = new EmailMessage
                        {
                            From = Settings.EmailFrom,
                            Subject = subject,
                            Body = $"File: {eventMessage.ImportFile.Name}",
                            IsHtml = false
                        };
                        emailMessage.AddRange(emailMessage.To, emailTo);
                        emailMessage.AddRange(emailMessage.Attachments, listAtt);
                        emailService.SendEmail(emailMessage, Settings.SMTP);
                        #endregion
                    }
                    else if (fileName.EndsWith("SD"))
                    {
                        #region Save Detail
                        try
                        {
                            //SD file save info to AcsShippingDetail table in Circulation
                            //get a dt then save to AcsShippingDetail
                            Core_AMS.Utilities.FileWorker fw = new FileWorker();
                            FileConfiguration fc = new FileConfiguration();
                            fc.FileColumnDelimiter = CommonEnums.ColumnDelimiter.comma.ToString();
                            fc.IsQuoteEncapsulated = false;
                            fc.FileExtension = Core_AMS.Utilities.Enums.FileExtensions.txt.ToString();


                            DataSet ds = fw.GetDataSet(file, fc);
                            DataTable dt = ds.Tables["Errors"];
                            //the header row will be gone, rest of rows will have an item array, 4th item is our values
                            DataTable dtDetail = new DataTable();
                            dtDetail.Columns.Add("Detail");//0
                            dtDetail.Columns.Add("CustomerNumber");//1
                            dtDetail.Columns.Add("AcsDate");//2
                            dtDetail.Columns.Add("ShipmentNumber");//3
                            dtDetail.Columns.Add("MailerId");//4
                            dtDetail.Columns.Add("Title");//5
                            dtDetail.Columns.Add("ProductCode");//6
                            dtDetail.Columns.Add("Description");//7
                            dtDetail.Columns.Add("Quantity");//8
                            dtDetail.Columns.Add("UnitCost");//9
                            dtDetail.Columns.Add("TotalCost");//10
                            dtDetail.AcceptChanges();
                            foreach (DataRow dr in dt.Rows)
                            {
                                string[] items = dr[3].ToString().Split(',');

                                dtDetail.Rows.Add(items);
                            }
                            dtDetail.AcceptChanges();
                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                if (dr["Detail"].ToString().Equals("D"))
                                {
                                    FrameworkUAD.BusinessLogic.AcsShippingDetail worker = new FrameworkUAD.BusinessLogic.AcsShippingDetail();
                                    FrameworkUAD.Entity.AcsShippingDetail acs = new FrameworkUAD.Entity.AcsShippingDetail();
                                    DateTime acsDate = DateTime.Now;
                                    DateTime.TryParse(dr["AcsDate"].ToString(), out acsDate);
                                    acs.AcsDate = acsDate;
                                    acs.AcsName = dr["Title"].ToString();
                                    int acsId = 0;
                                    int.TryParse(dr["MailerId"].ToString(), out acsId);
                                    acs.AcsId = acsId;
                                    acs.AcsTypeId = 0;
                                    int custNo = 0;
                                    int.TryParse(dr["CustomerNumber"].ToString(), out custNo);
                                    acs.CustomerNumber = custNo;
                                    acs.DateCreated = DateTime.Now;
                                    acs.Description = dr["Description"].ToString();
                                    acs.ProductCode = dr["ProductCode"].ToString();
                                    int quant = 0;
                                    int.TryParse(dr["Quantity"].ToString(), out quant);
                                    acs.Quantity = quant;
                                    Int64 shipNo = 0;
                                    Int64.TryParse(dr["ShipmentNumber"].ToString(), out shipNo);
                                    acs.ShipmentNumber = shipNo;
                                    decimal tc = 0;
                                    decimal.TryParse(dr["TotalCost"].ToString(), out tc);
                                    acs.TotalCost = tc;
                                    decimal uc = 0;
                                    decimal.TryParse(dr["UnitCost"].ToString(), out uc);
                                    acs.UnitCost = uc;
                                    acs.ProcessCode = eventMessage.AdmsLog.ProcessCode;

                                    worker.Save(acs, eventMessage.Client.ClientConnections);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, client, this.GetType().Name.ToString() + ".ACS_ExtractDataFromFiles");
                        }
                        #endregion
                    }
                    else if (fileName.EndsWith("CB"))
                    {
                        //do nothing, just delete file
                        file.Delete();
                    }
                    else
                    {
                        #region save info to AcsFileHeader and AcsFileDetail tables in Circulation
                        try
                        {
                            Core_AMS.Utilities.FileWorker fw = new FileWorker();
                            FileConfiguration fc = new FileConfiguration();
                            fc.FileColumnDelimiter = CommonEnums.ColumnDelimiter.tab.ToString();
                            fc.IsQuoteEncapsulated = false;
                            fc.FileExtension = Core_AMS.Utilities.Enums.FileExtensions.txt.ToString();


                            DataSet ds = fw.GetDataSet_NoHeader(file, fc);
                            DataTable dt = ds.Tables["Data"];

                            string header = dt.Rows[0].ItemArray[0].ToString();
                            if (header.Length == 700)
                            {
                                FrameworkUAD.BusinessLogic.AcsFileHeader acsHeadWorker = new FrameworkUAD.BusinessLogic.AcsFileHeader();
                                FrameworkUAD.Entity.AcsFileHeader acsHeader = acsHeadWorker.Parse(header);
                                acsHeader.ProcessCode = eventMessage.AdmsLog.ProcessCode;
                                acsHeadWorker.Save(acsHeader, eventMessage.Client.ClientConnections);

                                List<FrameworkUAD.Entity.AcsFileDetail> acsDetailList = new List<FrameworkUAD.Entity.AcsFileDetail>();
                                FrameworkUAD.BusinessLogic.AcsFileDetail acsDetailWorker = new FrameworkUAD.BusinessLogic.AcsFileDetail();

                                int rowCounter = 0;
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (rowCounter > 0)
                                    {
                                        string detail = dr.ItemArray[0].ToString();
                                        FrameworkUAD.Entity.AcsFileDetail acsDetail = acsDetailWorker.Parse(detail, eventMessage.Client.ClientConnections);
                                        acsDetail.ProcessCode = acsHeader.ProcessCode;
                                        if (!string.IsNullOrEmpty(acsDetail.RecordType))
                                            acsDetailList.Add(acsDetail);
                                    }

                                    rowCounter++;
                                }

                                acsDetailWorker.Insert(acsDetailList, eventMessage.Client.ClientConnections);
                            }
                            else
                                ConsoleMessage("Header file not correct length.", eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, client, "ADMS.Services.Validator.Validator.ACS_ExtractDataFromFiles - Unhandled Exception");
                        }
                        #endregion
                    }
                }
            }

            files = dirInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                try
                {
                    file.Delete();
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ACS_ExtractDataFromFiles");
                }
            }
            //archive the zip file
        }
        private List<FrameworkUAD.Entity.AcsFileDetail> ACS_SetValues(FileMoved eventMessage, List<FrameworkUAD.Entity.Product> publicationList)
        {
            //get AcsFileDetail for current ProcessCode
            FrameworkUAD.BusinessLogic.AcsFileDetail acsWorker = new FrameworkUAD.BusinessLogic.AcsFileDetail();
            List<FrameworkUAD.Entity.AcsFileDetail> acsList = acsWorker.Select(eventMessage.AdmsLog.ProcessCode, eventMessage.Client.ClientConnections);
            FrameworkUAD.BusinessLogic.AcsMailerInfo acsInfoWorker = new FrameworkUAD.BusinessLogic.AcsMailerInfo();

            //FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            //FrameworkUAD_Lookup.Entity.Code killCode = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeTypes.ACS_Action, FrameworkUAD_Lookup.Enums.ACS_Actions.Kill_Address.ToString());
            //FrameworkUAD_Lookup.Entity.Code updateCode = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeTypes.ACS_Action, FrameworkUAD_Lookup.Enums.ACS_Actions.Update_Address.ToString());

            #region special Keyline processes by PubCode
            List<string> eightSevenSequenceProducts = new List<string>();
            eightSevenSequenceProducts.Add("CFDM");//Watt
            eightSevenSequenceProducts.Add("FEMG");//Watt
            eightSevenSequenceProducts.Add("PETI");//Watt
            eightSevenSequenceProducts.Add("WPOU");//Watt
            eightSevenSequenceProducts.Add("EGGI");//Watt
            eightSevenSequenceProducts.Add("FEIN");//Watt
            eightSevenSequenceProducts.Add("INDA");//Watt
            eightSevenSequenceProducts.Add("POUI");//Watt
            eightSevenSequenceProducts.Add("PIGI");//Watt

            List<string> sixSequenceProducts = new List<string>();
            sixSequenceProducts.Add("FNDR");//DeWitt Publishing
            sixSequenceProducts.Add("CLP");//Anthem
            sixSequenceProducts.Add("HEAR");//Anthem
            sixSequenceProducts.Add("IMAG");//Anthem
            sixSequenceProducts.Add("ORPR");//Anthem
            sixSequenceProducts.Add("SLPR");//Anthem
            sixSequenceProducts.Add("X247");//Anthem
            sixSequenceProducts.Add("PSP");//Anthem
            sixSequenceProducts.Add("PTP");//Anthem
            sixSequenceProducts.Add("RHBM");//Anthem
            sixSequenceProducts.Add("RTDM");//Anthem

            List<string> fiveSequenceProducts = new List<string>();
            fiveSequenceProducts.Add("MMA");//MMA

            //List<string> spaceSequenceProducts = new List<string>();
            //spaceSequenceProducts.Add("CONP");//TradePress
            //spaceSequenceProducts.Add("SANM");//TradePress
            //spaceSequenceProducts.Add("PRRD");//TradePress
            //spaceSequenceProducts.Add("BOM");//TradePress
            //spaceSequenceProducts.Add("FMD");//TradePress
            #endregion

            //Get List for comparison in foreach loop
            List<FrameworkUAD.Entity.AcsMailerInfo> acsMailerList = acsInfoWorker.Select(eventMessage.Client.ClientConnections);
            foreach (FrameworkUAD.Entity.AcsFileDetail acs in acsList)
            {
                try
                {
                    #region set PubCode
                    //set PubCode - Query table Publication on AcsMailerId to get PublicationCode - can get all Publications for a client by publisherID - get Publisher object via ClientID
                    //First check acs.AcsMailerId against List of MailerIds for match if that results in a null then check against acs.AcsMailerId against list of AcsCodes


                    //BUG ID: 38271
                    #region Add Back This Version Once MailerID and IMBSEQ are fix
                    //FrameworkUAD.Entity.AcsMailerInfo acsMailerInfo = acsMailerList.FirstOrDefault(x => x.MailerID.ToString().Equals(acs.AcsMailerId, StringComparison.CurrentCultureIgnoreCase));
                    //FrameworkUAD.Entity.Product publication = null;
                    //if (acsMailerInfo != null)
                    //    publication = publicationList.FirstOrDefault(x => x.AcsMailerInfoId == acsMailerInfo.AcsMailerInfoId);
                    //else
                    //{
                    //acsMailerInfo = acsMailerList.FirstOrDefault(x => x.AcsCode.ToString().Equals(acs.AcsMailerId, StringComparison.CurrentCultureIgnoreCase));
                    //    if (acsMailerInfo != null)
                    //        publication = publicationList.FirstOrDefault(x => x.AcsMailerInfoId == acsMailerInfo.AcsMailerInfoId);

                    //}
                    #endregion
                    #region Temp version until MailerID and IMBSEQ are fixed just look at ACSCode for now
                    FrameworkUAD.Entity.AcsMailerInfo acsMailerInfo = acsMailerList.FirstOrDefault(x => x.AcsCode.ToString().Equals(acs.AcsMailerId, StringComparison.CurrentCultureIgnoreCase));
                    FrameworkUAD.Entity.Product publication = null;
                    if (acsMailerInfo != null)
                        publication = publicationList.FirstOrDefault(x => x.AcsMailerInfoId == acsMailerInfo.AcsMailerInfoId);
                    #endregion

                    if (publication != null)
                        acs.ProductCode = publication.PubCode;
                    else if (acs.AcsMailerId.Equals("BXNDYYQ"))
                    {
                        //publication = publicationList.FirstOrDefault(x => x.AcsMailerId.Equals("999944317", StringComparison.CurrentCultureIgnoreCase));
                        if (acsMailerInfo != null)
                        {
                            acsMailerInfo = acsMailerList.FirstOrDefault(x => x.MailerID.ToString().Equals("999944317", StringComparison.CurrentCultureIgnoreCase));
                            publication = publicationList.FirstOrDefault(x => x.AcsMailerInfoId == acsMailerInfo.AcsMailerInfoId);
                            acs.ProductCode = publication.PubCode;
                        }
                    }
                    else if (acs.AcsMailerId.Equals("BXNDYXS"))
                    {
                        //publication = publicationList.FirstOrDefault(x => x.AcsMailerId.Equals("999944318", StringComparison.CurrentCultureIgnoreCase));
                        if (acsMailerInfo != null)
                        {
                            acsMailerInfo = acsMailerList.FirstOrDefault(x => x.MailerID.ToString().Equals("999944318", StringComparison.CurrentCultureIgnoreCase));
                            publication = publicationList.FirstOrDefault(x => x.AcsMailerInfoId == acsMailerInfo.AcsMailerInfoId);
                            acs.ProductCode = publication.PubCode;
                        }
                    }
                    else if (acs.AcsMailerId.Equals("BXNDYWV"))
                    {
                        //publication = publicationList.FirstOrDefault(x => x.AcsMailerId.Equals("999944319", StringComparison.CurrentCultureIgnoreCase));
                        if (acsMailerInfo != null)
                        {
                            acsMailerInfo = acsMailerList.FirstOrDefault(x => x.MailerID.ToString().Equals("999944319", StringComparison.CurrentCultureIgnoreCase));
                            publication = publicationList.FirstOrDefault(x => x.AcsMailerInfoId == acsMailerInfo.AcsMailerInfoId);
                            acs.ProductCode = publication.PubCode;
                        }
                    }
                    else if (acs.AcsMailerId.Equals("BXBFCVR"))
                    {
                        //publication = publicationList.FirstOrDefault(x => x.AcsMailerId.Equals("BXBFCSW", StringComparison.CurrentCultureIgnoreCase));
                        if (acsMailerInfo != null)
                        {
                            acsMailerInfo = acsMailerList.FirstOrDefault(x => x.MailerID.ToString().Equals("BXBFCSW", StringComparison.CurrentCultureIgnoreCase));
                            publication = publicationList.FirstOrDefault(x => x.AcsMailerInfoId == acsMailerInfo.AcsMailerInfoId);
                            acs.ProductCode = publication.PubCode;
                        }
                    }
                    else if (acs.AcsMailerId.Equals("BXNQVJK"))
                    {
                        //publication = publicationList.FirstOrDefault(x => x.AcsMailerId.Equals("900001537", StringComparison.CurrentCultureIgnoreCase));
                        if (acsMailerInfo != null)
                        {
                            acsMailerInfo = acsMailerList.FirstOrDefault(x => x.MailerID.ToString().Equals("900001537", StringComparison.CurrentCultureIgnoreCase));
                            publication = publicationList.FirstOrDefault(x => x.AcsMailerInfoId == acsMailerInfo.AcsMailerInfoId);
                            acs.ProductCode = publication.PubCode;
                        }
                    }
                    else if (acs.AcsMailerId.Equals("BXNPHZV"))
                    {
                        //publication = publicationList.FirstOrDefault(x => x.AcsMailerId.Equals("901456232", StringComparison.CurrentCultureIgnoreCase));
                        if (acsMailerInfo != null)
                        {
                            acsMailerInfo = acsMailerList.FirstOrDefault(x => x.MailerID.ToString().Equals("901456232", StringComparison.CurrentCultureIgnoreCase));
                            publication = publicationList.FirstOrDefault(x => x.AcsMailerInfoId == acsMailerInfo.AcsMailerInfoId);
                            acs.ProductCode = publication.PubCode;
                        }
                    }
                    else if (acs.AcsMailerId.Equals("BXNPHYX"))
                    {
                        //publication = publicationList.FirstOrDefault(x => x.AcsMailerId.Equals("999944320", StringComparison.CurrentCultureIgnoreCase));
                        if (acsMailerInfo != null)
                        {
                            acsMailerInfo = acsMailerList.FirstOrDefault(x => x.MailerID.ToString().Equals("999944320", StringComparison.CurrentCultureIgnoreCase));
                            publication = publicationList.FirstOrDefault(x => x.AcsMailerInfoId == acsMailerInfo.AcsMailerInfoId);
                            acs.ProductCode = publication.PubCode;
                        }
                    }
                    else if (acs.AcsMailerId.Equals("BXNPHXZ"))
                    {
                        //publication = publicationList.FirstOrDefault(x => x.AcsMailerId.Equals("999944321", StringComparison.CurrentCultureIgnoreCase));
                        if (acsMailerInfo != null)
                        {
                            acsMailerInfo = acsMailerList.FirstOrDefault(x => x.MailerID.ToString().Equals("999944321", StringComparison.CurrentCultureIgnoreCase));
                            publication = publicationList.FirstOrDefault(x => x.AcsMailerInfoId == acsMailerInfo.AcsMailerInfoId);
                            acs.ProductCode = publication.PubCode;
                        }
                    }
                    else if (acs.AcsMailerId.Equals("BXNRRWP"))
                    {
                        //publication = publicationList.FirstOrDefault(x => x.AcsMailerId.Equals("999944322", StringComparison.CurrentCultureIgnoreCase));
                        if (acsMailerInfo != null)
                        {
                            acsMailerInfo = acsMailerList.FirstOrDefault(x => x.MailerID.ToString().Equals("999944322", StringComparison.CurrentCultureIgnoreCase));
                            publication = publicationList.FirstOrDefault(x => x.AcsMailerInfoId == acsMailerInfo.AcsMailerInfoId);
                            acs.ProductCode = publication.PubCode;
                        }
                    }
                    else if (acs.AcsMailerId.Equals("BXNQVLF"))
                    {
                        //publication = publicationList.FirstOrDefault(x => x.AcsMailerId.Equals("999944323", StringComparison.CurrentCultureIgnoreCase));
                        if (acsMailerInfo != null)
                        {
                            acsMailerInfo = acsMailerList.FirstOrDefault(x => x.MailerID.ToString().Equals("999944323", StringComparison.CurrentCultureIgnoreCase));
                            publication = publicationList.FirstOrDefault(x => x.AcsMailerInfoId == acsMailerInfo.AcsMailerInfoId);
                            acs.ProductCode = publication.PubCode;
                        }
                    }
                    else if (acs.AcsMailerId.Equals("999944316"))
                    {
                        //publication = publicationList.FirstOrDefault(x => x.AcsMailerId.Equals("999944323", StringComparison.CurrentCultureIgnoreCase));
                        acs.ProductCode = "IMAG";
                    }
                    else
                        acs.ProductCode = string.Empty;
                    #endregion
                    #region set Xact code
                    //set Xact code to either 21 or 31 - Get TranCode/CatCode if trancode = 34 set IsIgnored to true
                    //31 = POKill  21 = Address Change	
                    if ((string.IsNullOrEmpty(acs.DeliverabilityCode) || string.IsNullOrWhiteSpace(acs.DeliverabilityCode)) ||
                            acs.DeliverabilityCode.Equals("K", StringComparison.CurrentCultureIgnoreCase) ||
                            acs.DeliverabilityCode.Equals("G", StringComparison.CurrentCultureIgnoreCase) ||
                            acs.DeliverabilityCode.Equals("W", StringComparison.CurrentCultureIgnoreCase))
                    {
                        acs.TransactionCodeValue = 21;
                        //acs.AcsActionId = updateCode.CodeId;
                    }
                    else if (acs.DeliverabilityCode.Equals("P"))//deceased
                    {
                        acs.TransactionCodeValue = 32;
                        //acs.AcsActionId = killCode.CodeId;
                    }
                    else
                    {
                        acs.TransactionCodeValue = 31;
                        //acs.AcsActionId = killCode.CodeId;
                    }
                    #endregion
                    #region combine address pieces to get Address1 and Address2
                    if ((string.IsNullOrEmpty(acs.DeliverabilityCode) || string.IsNullOrWhiteSpace(acs.DeliverabilityCode)) ||
                            acs.DeliverabilityCode.Equals("K", StringComparison.CurrentCultureIgnoreCase) ||
                            acs.DeliverabilityCode.Equals("G", StringComparison.CurrentCultureIgnoreCase) ||
                            acs.DeliverabilityCode.Equals("W", StringComparison.CurrentCultureIgnoreCase))
                    {
                        acs.OldAddress1 = acs.OldPrimaryNumber.Trim() + " " + acs.OldPreDirectional.Trim() + " " + acs.OldStreetName.Trim() + " " + acs.OldSuffix.Trim() + " " + acs.OldPostDirectional.Trim();
                        acs.OldAddress1 = string.Join(" ", acs.OldAddress1.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                        acs.OldAddress2 = acs.OldUnitDesignator.Trim() + " " + acs.OldSecondaryNumber.Trim();
                        acs.OldAddress2 = string.Join(" ", acs.OldAddress2.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                        acs.OldAddress3 = string.Empty;

                        acs.NewAddress1 = acs.NewPrimaryNumber.Trim() + " " + acs.NewPreDirectional.Trim() + " " + acs.NewStreetName.Trim() + " " + acs.NewSuffix.Trim() + " " + acs.NewPostDirectional.Trim();
                        acs.NewAddress1 = string.Join(" ", acs.NewAddress1.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                        acs.NewAddress2 = acs.NewUnitDesignator.Trim() + " " + acs.NewSecondaryNumber.Trim();
                        acs.NewAddress2 = string.Join(" ", acs.NewAddress2.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                        acs.NewAddress3 = string.Empty;
                    }
                    #endregion
                    #region set SequenceID
                    int seqID = 0;
                    string seqString = string.Empty;

                    if (eightSevenSequenceProducts.Contains(acs.ProductCode))//Watt
                    {
                        #region OLD
                        //if (acs.KeylineSequenceSerialNumber.Length >= 15)
                        //    seqString = acs.KeylineSequenceSerialNumber.Substring(8, 7).Trim();
                        #endregion
                        //Code above commented out 02-08-2017 Keyline will come in 000000007139231 for the listed Watt Pubs With the last number being check digit 
                        //so we just need to convert to number and the next process will remove the check digit providing the correct sequence

                        int s = 0;
                        int.TryParse(acs.KeylineSequenceSerialNumber.Trim().ToString(), out s);
                        if (s > 0)
                            seqString = s.ToString().Trim();
                    }
                    #region Old Code No Longer With
                    //No Longer With
                    //else if (sixSequenceProducts.Contains(acs.ProductCode))//Anthem
                    //{
                    //    //if (acs.KeylineSequenceSerialNumber.Length >= 6)
                    //    //    seqString = acs.KeylineSequenceSerialNumber.Substring(0, 6).Trim();
                    //    int s = 0;
                    //    int.TryParse(acs.KeylineSequenceSerialNumber.Trim().ToString(), out s);
                    //    if (s > 0 && s.ToString().Trim().Length >= 6)
                    //        seqString = s.ToString().Substring(0, 6).Trim();
                    //    else if (s > 0)
                    //        seqString = s.ToString().Trim();
                    //    else
                    //        seqString = acs.KeylineSequenceSerialNumber.Substring(0, 6).Trim();

                    //}
                    //else if (fiveSequenceProducts.Contains(acs.ProductCode))//MMA
                    //{
                    //    //if (acs.KeylineSequenceSerialNumber.Length >= 5)
                    //    //    seqString = acs.KeylineSequenceSerialNumber.Substring(0, 5).Trim();
                    //    int s = 0;
                    //    int.TryParse(acs.KeylineSequenceSerialNumber.Trim().ToString(), out s);
                    //    if (s > 0 && s.ToString().Trim().Length >= 5)
                    //        seqString = s.ToString().Substring(0, 5).Trim();
                    //    else if (s > 0)
                    //        seqString = s.ToString().Trim();
                    //    else
                    //        seqString = acs.KeylineSequenceSerialNumber.Substring(0, 5).Trim();

                    //}
                    #endregion
                    else
                    {
                        #region OLD
                        //if (acs.KeylineSequenceSerialNumber.Length >= 7)
                        //    seqString = acs.KeylineSequenceSerialNumber.Substring(0, 7).Trim();

                        //Remove >= 7 check because it conflicts with SequenceID of 6 digits (11-11-2016) Jason
                        //if (acs.KeylineSequenceSerialNumber.Length >= 7)
                        //{

                        //if (s > 0 && s.ToString().Trim().Length >= 7)
                        //    seqString = s.ToString().Substring(0, 7).Trim();
                        //else if (s > 0)
                        //    seqString = s.ToString().Trim();
                        //else
                        //    seqString = acs.KeylineSequenceSerialNumber.Substring(0, 7).Trim();

                        //}
                        #endregion

                        // Code above commented out 02-08-2017 Keyline will convert to a number and next code will remove the last character which is a check digit.
                        int s = 0;
                        int.TryParse(acs.KeylineSequenceSerialNumber.Trim().ToString(), out s);
                        if (s > 0)
                            seqString = s.ToString().Trim();

                    }

                    //Remove the last number because this is a check digit and askew the sequence id.
                    if (seqString.Length > 0)
                    {
                        seqString = seqString.Substring(0, seqString.Length - 1);
                    }
                    int.TryParse(seqString.Trim(), out seqID);
                    acs.SequenceID = seqID;
                    #endregion
                }
                catch (Exception ex)
                {
                    LogError(ex, client, "ADMS.Services.Validator.Validator.ACS_SetValues - Unhandled Exception");
                }
            }
            acsWorker.Update(acsList, eventMessage.Client.ClientConnections);

            return acsList;
        }
        private void ACS_UpdateSubscriber(FileMoved eventMessage, List<FrameworkUAD.Entity.AcsFileDetail> acsList, List<FrameworkUAD.Entity.Product> publicationList)
        {
            /*
              Match on SequenceID
              if acsOldAddress == KM Address then update KM to acsNewAddress
              if acsOldAddress != KM Address then ignore - per Joe assume PostOffice messed up seq #
              if acs has no address then KM kill - acs.TransactionCodeValue will equal 32 or 31 set KM to same
             */
            //get a distinct list of PubCode

            List<string> distProductCodes = acsList.Select(x => x.ProductCode).Distinct().ToList();
            foreach (string pc in distProductCodes)
            {
                if (!string.IsNullOrEmpty(pc))
                {
                    try
                    {
                        int publicationID = publicationList.SingleOrDefault(x => x.PubCode == pc).PubID;

                        FrameworkUAD.BusinessLogic.AcsFileDetail acsWorker = new FrameworkUAD.BusinessLogic.AcsFileDetail();
                        //split into 2 lists - Kill list and Update Address list
                        List<FrameworkUAD.Entity.AcsFileDetail> killList = acsList.Where(x => (x.TransactionCodeValue == 31 || x.TransactionCodeValue == 32) && x.ProductCode == pc).ToList();
                        //List<FrameworkUAD.Entity.AcsFileDetail> addressUpdateList = acsList.Where(x => x.TransactionCodeValue == 21 && x.ProductCode == pc && 
                        //        (string.IsNullOrEmpty(x.NewAddress1) == false && string.IsNullOrEmpty(x.NewCity) == false && string.IsNullOrEmpty(x.NewStateAbbreviation) == false && string.IsNullOrEmpty(x.NewZipCode) == false)).ToList();
                        List<FrameworkUAD.Entity.AcsFileDetail> addressUpdateList = acsList.Where(x => x.TransactionCodeValue == 21 && x.ProductCode == pc &&
                                (string.IsNullOrEmpty(x.NewAddress1) == false && string.IsNullOrEmpty(x.OldAddress1) == false)
                                && string.IsNullOrEmpty(x.NewCity) == false && string.IsNullOrEmpty(x.NewStateAbbreviation) == false && string.IsNullOrEmpty(x.NewZipCode) == false).ToList();

                        FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                        int userLogTypeId = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.User_Log, FrameworkUAD_Lookup.Enums.UserLogTypes.Edit.ToString()).CodeId;
                        //pull a batch number for each of these - track before and after values - batch is by Publication (PubCode)
                        if (addressUpdateList.Count > 0)
                            acsWorker.UpdateSubscriberAddress(addressUpdateList, publicationID, eventMessage.Client.ClientConnections, userLogTypeId, eventMessage.SourceFile.SourceFileID);//done

                        if (killList.Count > 0)
                            acsWorker.KillSubscriber(killList, publicationID, eventMessage.Client.ClientConnections, userLogTypeId, eventMessage.SourceFile.SourceFileID);

                        #region send to SubGen for address update
                        List<FrameworkUAD.Entity.Product> prodList = new List<Product>();
                        FrameworkUAD.BusinessLogic.Product pWrk = new FrameworkUAD.BusinessLogic.Product();
                        prodList = pWrk.Select(eventMessage.Client.ClientConnections);
                        //if PubCode HasPaidRecords = true and UseSubGen = true then update SubGen addresses
                        if (prodList.Exists(x => x.PubCode.Equals(pc, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            FrameworkUAD.Entity.Product prod = prodList.First(x => x.PubCode.Equals(pc, StringComparison.CurrentCultureIgnoreCase));
                            List<string> where = new List<string>();
                            List<FrameworkUAD.Entity.AcsFileDetail> subGenAcsList = new List<AcsFileDetail>();
                            if (prod.HasPaidRecords == true && prod.UseSubGen == true)
                            {
                                //join PubSubscriptions ps with(nolock) on i.SequenceID = ps.SequenceID
                                foreach (FrameworkUAD.Entity.AcsFileDetail afd in addressUpdateList)
                                {
                                    where.Add(afd.SequenceID.ToString());
                                    subGenAcsList.Add(afd);
                                }

                                FrameworkUAD.BusinessLogic.ProductSubscription psWrk = new FrameworkUAD.BusinessLogic.ProductSubscription();
                                List<FrameworkUAD.Entity.ProductSubscription> psList = psWrk.SelectSequence(where, eventMessage.Client.ClientConnections);
                                FrameworkSubGen.BusinessLogic.Address addWrk = new FrameworkSubGen.BusinessLogic.Address();
                                List<FrameworkUAD.Entity.ProductSubscription> subGenUnableToUpdate = addWrk.UpdateForACS(psList, subGenAcsList, eventMessage.Client.ClientID);
                                Emailer.Emailer em = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.SubGenAddressesNotAbleToUpdate);
                                em.SubGenAddressesNotAbleToUpdate(subGenUnableToUpdate, eventMessage.Client);
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, client, "ADMS.Services.Validator.Validator.ACS_UpdateSubscriber - Unhandled Exception");
                    }
                }
            }

        }
        private List<FrameworkUAD.Entity.AcsFileDetail> ACS_UAD_SetValues(FileMoved eventMessage)
        {
            //get AcsFileDetail for current ProcessCode
            FrameworkUAD.BusinessLogic.AcsFileDetail acsWorker = new FrameworkUAD.BusinessLogic.AcsFileDetail();
            List<FrameworkUAD.Entity.AcsFileDetail> acsList = acsWorker.Select(eventMessage.AdmsLog.ProcessCode, eventMessage.Client.ClientConnections);

            FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            FrameworkUAD_Lookup.Entity.Code killCode = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.ACS_Action, FrameworkUAD_Lookup.Enums.ACS_Actions.Kill_Address.ToString());
            FrameworkUAD_Lookup.Entity.Code updateCode = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.ACS_Action, FrameworkUAD_Lookup.Enums.ACS_Actions.Update_Address.ToString());

            foreach (FrameworkUAD.Entity.AcsFileDetail acs in acsList)
            {
                try
                {
                    #region set Xact code
                    //set Xact code to either 21 or 31 - Get TranCode/CatCode if trancode = 34 set IsIgnored to true
                    //31 = POKill  21 = Address Change	
                    if (string.IsNullOrEmpty(acs.DeliverabilityCode) || string.IsNullOrWhiteSpace(acs.DeliverabilityCode))
                    {
                        acs.TransactionCodeValue = 21;
                        acs.AcsActionId = updateCode.CodeId;
                    }
                    else if (acs.DeliverabilityCode.Equals("P"))//deceased
                    {
                        acs.TransactionCodeValue = 32;
                        acs.AcsActionId = killCode.CodeId;
                    }
                    else
                    {
                        acs.TransactionCodeValue = 31;
                        acs.AcsActionId = killCode.CodeId;
                    }
                    #endregion
                    #region combine address pieces to get Address1 and Address2
                    if (string.IsNullOrEmpty(acs.DeliverabilityCode) || string.IsNullOrWhiteSpace(acs.DeliverabilityCode))
                    {
                        acs.OldAddress1 = acs.OldPrimaryNumber.Trim() + " " + acs.OldPreDirectional.Trim() + " " + acs.OldStreetName.Trim() + " " + acs.OldPostDirectional.Trim();
                        acs.OldAddress1 = acs.OldAddress1.Trim();
                        acs.OldAddress2 = acs.OldUnitDesignator.Trim() + " " + acs.OldSecondaryNumber.Trim();
                        acs.OldAddress2 = acs.OldAddress2.Trim();
                        acs.OldAddress3 = string.Empty;

                        acs.NewAddress1 = acs.NewPrimaryNumber.Trim() + " " + acs.NewPreDirectional.Trim() + " " + acs.NewStreetName.Trim() + " " + acs.NewPostDirectional.Trim();
                        acs.NewAddress1 = acs.NewAddress1.Trim();
                        acs.NewAddress2 = acs.NewUnitDesignator.Trim() + " " + acs.NewSecondaryNumber.Trim();
                        acs.NewAddress2 = acs.NewAddress2.Trim();
                        acs.NewAddress3 = string.Empty;
                    }
                    #endregion
                    #region set SequenceID
                    int seqID = 0;
                    string seqString = string.Empty;

                    if (acs.KeylineSequenceSerialNumber.Length >= 7)
                        seqString = acs.KeylineSequenceSerialNumber.Substring(0, 7).Trim();

                    int.TryParse(seqString.Trim(), out seqID);
                    acs.SequenceID = seqID;
                    #endregion
                }
                catch (Exception ex)
                {
                    LogError(ex, client, "ADMS.Services.Validator.Validator.ACS_UAD_SetValues - Unhandled Exception");
                }
            }
            acsWorker.Update(acsList, eventMessage.Client.ClientConnections);

            return acsList;
        }
        private void ACS_UAD_UpdateSubscriber(FileMoved eventMessage, List<FrameworkUAD.Entity.AcsFileDetail> acsList)
        {
            /*
              Match on SequenceID
              if acsOldAddress == KM Address then update KM to acsNewAddress
              if acsOldAddress != KM Address then ignore - per Joe assume PostOffice messed up seq #
              if acs has no address then KM kill - acs.TransactionCodeValue will equal 32 or 31 set KM to same
             */
            //get a distinct list of PubCode

            List<string> distProductCodes = acsList.Select(x => x.ProductCode).Distinct().ToList();
            List<FrameworkUAD.Entity.Product> prodList = new List<Product>();
            FrameworkUAD.BusinessLogic.Product pWrk = new FrameworkUAD.BusinessLogic.Product();
            prodList = pWrk.Select(eventMessage.Client.ClientConnections);

            foreach (string pc in distProductCodes)
            {
                if (!string.IsNullOrEmpty(pc))
                {
                    try
                    {
                        FrameworkUAD.BusinessLogic.AcsFileDetail acsWorker = new FrameworkUAD.BusinessLogic.AcsFileDetail();
                        //split into 2 lists - Kill list and Update Address list
                        //List<FrameworkUAD.Entity.AcsFileDetail> killList = acsList.Where(x => (x.TransactionCodeValue == 31 || x.TransactionCodeValue == 32) && x.ProductCode == pc).ToList();
                        List<FrameworkUAD.Entity.AcsFileDetail> addressUpdateList = acsList.Where(x => x.TransactionCodeValue == 21 && x.ProductCode == pc).ToList();

                        FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                        int userLogTypeId = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.User_Log, FrameworkUAD_Lookup.Enums.UserLogTypes.Edit.ToString()).CodeId;
                        //no batching needed!
                        acsWorker.UpdateUADSubscriberAddress(addressUpdateList, eventMessage.Client.ClientConnections, userLogTypeId);
                        //acsWorker.KillUADSubscriber(killList, eventMessage.Client);
                        #region send to SubGen for address update
                        //if PubCode HasPaidRecords = true and UseSubGen = true then update SubGen addresses
                        if (prodList.Exists(x => x.PubCode.Equals(pc, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            FrameworkUAD.Entity.Product prod = prodList.First(x => x.PubCode.Equals(pc, StringComparison.CurrentCultureIgnoreCase));
                            List<string> where = new List<string>();
                            List<FrameworkUAD.Entity.AcsFileDetail> subGenAcsList = new List<AcsFileDetail>();
                            if (prod.HasPaidRecords == true && prod.UseSubGen == true)
                            {
                                //join PubSubscriptions ps with(nolock) on i.SequenceID = ps.SequenceID
                                foreach (FrameworkUAD.Entity.AcsFileDetail afd in addressUpdateList)
                                {
                                    where.Add(afd.SequenceID.ToString());
                                    subGenAcsList.Add(afd);
                                }

                                FrameworkUAD.BusinessLogic.ProductSubscription psWrk = new FrameworkUAD.BusinessLogic.ProductSubscription();
                                List<FrameworkUAD.Entity.ProductSubscription> psList = psWrk.SelectSequence(where, eventMessage.Client.ClientConnections);
                                FrameworkSubGen.BusinessLogic.Address addWrk = new FrameworkSubGen.BusinessLogic.Address();
                                List<FrameworkUAD.Entity.ProductSubscription> subGenUnableToUpdate = addWrk.UpdateForACS(psList, subGenAcsList, eventMessage.Client.ClientID);
                                Emailer.Emailer em = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.SubGenAddressesNotAbleToUpdate);
                                em.SubGenAddressesNotAbleToUpdate(subGenUnableToUpdate, eventMessage.Client);
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, client, "ADMS.Services.Validator.Validator.ACS_UAD_UpdateSubscriber - Unhandled Exception");
                    }
                }
            }

        }
        #endregion
        #region NCOA
        private DataTable NcoaParseFile(FileMoved eventMessage)
        {
            FileConfiguration fc = new FileConfiguration();
            fc.FileExtension = ".csv";
            fc.FileColumnDelimiter = CommonEnums.ColumnDelimiter.comma.ToString();
            fc.IsQuoteEncapsulated = true;

            DataTable dt = FileImporter.LoadFile(eventMessage.ImportFile, fc);

            return dt;
        }
        private string NcoaGetXml(List<FrameworkUAD.Object.NCOA> pubList, KMPlatform.Entity.Client client)
        {
            //KMPlatform.BusinessLogic.Client pWorker = new KMPlatform.BusinessLogic.Client();
            //int publisherID = pWorker.SelectForClient(client.ClientID).FirstOrDefault().PublisherID;
            //KMPlatform.Entity.Client publisherID = pWorker.Select(client.ClientID);

            FrameworkUAD.BusinessLogic.Product pubWorker = new FrameworkUAD.BusinessLogic.Product();
            List<FrameworkUAD.Entity.Product> publications = pubWorker.Select(client.ClientConnections);

            //get columns ncoaseq,std_line1,std_line2,std_city,std_state,std_zip,std_zip4,pubcode
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<XML>");
            foreach (FrameworkUAD.Object.NCOA n in pubList)
            {
                int publicationID = 0;
                if (publications.Exists(x => x.PubCode.Equals(n.ProductCode, StringComparison.CurrentCultureIgnoreCase)))
                    publicationID = publications.FirstOrDefault(x => x.PubCode.Equals(n.ProductCode, StringComparison.CurrentCultureIgnoreCase)).PubID;

                sb.AppendLine("<NCOA>");
                sb.AppendLine("<SequenceID>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(n.SequenceID.ToString()) + "</SequenceID>");
                sb.AppendLine("<Address1>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(n.Address1) + "</Address1>");
                sb.AppendLine("<Address2>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(n.Address2) + "</Address2>");
                sb.AppendLine("<City>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(n.City) + "</City>");
                sb.AppendLine("<RegionCode>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(n.RegionCode) + "</RegionCode>");
                sb.AppendLine("<ZipCode>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(n.ZipCode) + "</ZipCode>");
                sb.AppendLine("<Plus4>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(n.Plus4) + "</Plus4>");
                sb.AppendLine("<ProductCode>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(n.ProductCode) + "</ProductCode>");
                sb.AppendLine("<PublisherID>" + client.ClientID.ToString().Trim() + "</PublisherID>");
                sb.AppendLine("<PublicationID>" + publicationID.ToString().Trim() + "</PublicationID>");
                sb.AppendLine("<ProcessCode>" + n.ProcessCode + "</ProcessCode>");
                sb.AppendLine("</NCOA>");
            }
            sb.AppendLine("</XML>");

            return sb.ToString();
        }
        private string Ncoa_UAD_GetXml(List<FrameworkUAD.Object.NCOA> pubList)
        {
            //get columns ncoaseq,std_line1,std_line2,std_city,std_state,std_zip,std_zip4,pubcode
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<XML>");
            foreach (FrameworkUAD.Object.NCOA n in pubList)
            {
                sb.AppendLine("<NCOA>");
                sb.AppendLine("<SequenceID>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(n.SequenceID.ToString()) + "</SequenceID>");
                sb.AppendLine("<Address1>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(n.Address1) + "</Address1>");
                sb.AppendLine("<Address2>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(n.Address2) + "</Address2>");
                sb.AppendLine("<City>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(n.City) + "</City>");
                sb.AppendLine("<RegionCode>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(n.RegionCode) + "</RegionCode>");
                sb.AppendLine("<ZipCode>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(n.ZipCode) + "</ZipCode>");
                sb.AppendLine("<Plus4>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(n.Plus4) + "</Plus4>");
                sb.AppendLine("<ProcessCode>" + n.ProcessCode + "</ProcessCode>");
                sb.AppendLine("</NCOA>");
            }
            sb.AppendLine("</XML>");

            return sb.ToString();
        }
        //private bool NcoaUpdateAddress(string xml)
        //{
        //    FrameworkUAD.BusinessLogic.Subscription sWorker = new FrameworkUAD.BusinessLogic.Subscription();
        //    return sWorker.NcoaUpdateAddress(xml, eventMessage.Client);
        //}
        private bool Ncoa_UAD_UpdateAddress(string xml, KMPlatform.Entity.Client client, int SourceFileID)
        {
            FrameworkUAD.BusinessLogic.Subscription sWorker = new FrameworkUAD.BusinessLogic.Subscription();
            return sWorker.NcoaUpdateAddress(xml, client.ClientConnections, SourceFileID);
        }
        private void Ncoa_to_SubGen(List<FrameworkUAD.Object.NCOA> pubList, KMPlatform.Entity.Client client, string pubCode, string processCode)
        {
            List<FrameworkUAD.Entity.Product> prodList = new List<Product>();
            FrameworkUAD.BusinessLogic.Product pWrk = new FrameworkUAD.BusinessLogic.Product();
            prodList = pWrk.Select(client.ClientConnections);
            //if PubCode HasPaidRecords = true and UseSubGen = true then update SubGen addresses
            List<FrameworkUAD.Object.NCOA> ncoaList = new List<FrameworkUAD.Object.NCOA>();
            List<string> where = new List<string>();

            if (prodList.Exists(x => x.PubCode.Equals(pubCode, StringComparison.CurrentCultureIgnoreCase)))
            {
                FrameworkUAD.Entity.Product prod = prodList.First(x => x.PubCode.Equals(pubCode, StringComparison.CurrentCultureIgnoreCase));
                if (prod.HasPaidRecords == true && prod.UseSubGen == true)
                {
                    pubList.ForEach(x =>
                    {
                        where.Add(x.SequenceID.ToString());
                        ncoaList.Add(x);
                    });
                }
            }

            FrameworkUAD.BusinessLogic.ProductSubscription psWrk = new FrameworkUAD.BusinessLogic.ProductSubscription();
            List<FrameworkUAD.Entity.ProductSubscription> psList = psWrk.SelectSequence(where, client.ClientConnections);
            FrameworkSubGen.BusinessLogic.Address addWrk = new FrameworkSubGen.BusinessLogic.Address();
            List<FrameworkUAD.Entity.ProductSubscription> subGenUnableToUpdate = addWrk.UpdateForNCOA(psList, ncoaList, client.ClientID);
            Emailer.Emailer em = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.SubGenAddressesNotAbleToUpdate);
            em.SubGenAddressesNotAbleToUpdate(subGenUnableToUpdate, client);
        }
        #endregion
        #endregion
        #region UAD file processing - Object based - FrameworkUAD.Object.ImportFile
        public void ProcessFileAsObject(FileMoved eventMessage, bool isSpecialFile, KMPlatform.Entity.ServiceFeature serviceFeature, SourceFile cSpecialFile = null, FrameworkUAD_Lookup.Entity.Code sfr = null, string enumDatabaseFileType = "")
        {
            try
            {
                clientPubCodes = null;
                FrameworkUAD_Lookup.BusinessLogic.Code codeWork = new FrameworkUAD_Lookup.BusinessLogic.Code();
                Code dbft = codeWork.SelectCodeId(eventMessage.SourceFile.DatabaseFileTypeId);
                FrameworkUAD_Lookup.Enums.FileTypes dft = FrameworkUAD_Lookup.Enums.GetDatabaseFileType(dbft.CodeName);

                //FrameworkUAS.Entity.SourceFile sourceFile = new SourceFile();
                //sourceFile = eventMessage.SourceFile.DeepClone();
                FileWorker fileWorker = new FileWorker();
                bool rejectFile = false;
                #region check file TextQualifier matches actual file
                //this will be a simple check - if SourceFile.IsTextQualifier is TRUE then the very first character in the file should be a quotation if not then Reject the file
                //first lets get the very first character in the file
                char first = fileWorker.GetFirstCharacter(eventMessage.ImportFile);
                bool isFirstQuote = false;
                if (first == '"')
                    isFirstQuote = true;
                if (isFirstQuote != eventMessage.SourceFile.IsTextQualifier && (eventMessage.SourceFile.Extension.Equals(".csv") || eventMessage.SourceFile.Extension.Equals(".txt") || eventMessage.SourceFile.Extension.Equals(".dbf")))
                {
                    rejectFile = true;
                    //reject file because setup and file do not match
                    string reason = string.Empty;
                    if (isFirstQuote == false && eventMessage.SourceFile.IsTextQualifier == true)
                    {
                        reason = "Your file is configured to be quote encapsulated but the actual file is not quote encapsulated.  Please either quote encapsulate your file or adjust the setup configuration.";
                    }
                    else if (isFirstQuote == true && eventMessage.SourceFile.IsTextQualifier == false)
                    {
                        reason = "Your file is quote encapsulated but the configuration setup is set to not quote encapsulated.  Please either remove quote encapsulation from your file or adjust the setup configuration.";
                    }
                    Emailer.Emailer em = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.RejectFile);
                    em.RejectFile(eventMessage.Client, eventMessage.ImportFile, reason);
                    ThreadDictionary.Remove(eventMessage.ThreadId);
                    //ADMSProcessingQue.RemoveClientFile(eventMessage.Client, eventMessage.ImportFile);
                    if (File.Exists(eventMessage.ImportFile.FullName))
                        File.Delete(eventMessage.ImportFile.FullName);
                    ConsoleMessage("Awaiting new file....", eventMessage.AdmsLog.ProcessCode, true);
                }
                #endregion

                if (rejectFile == false)
                {
                    #region Get some CodeId's 
                    FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                    List<FrameworkUAD_Lookup.Entity.Code> codes = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping);
                    standarTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString())).CodeId;
                    demoTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString())).CodeId;
                    ignoredTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString())).CodeId;
                    demoRespOtherTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Other.ToString().Replace('_', ' '))).CodeId;
                    demoDateTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Date.ToString().Replace('_', ' '))).CodeId;
                    kmTransformTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.kmTransform.ToString())).CodeId;
                    #endregion


                    foreach (var x in eventMessage.SourceFile.FieldMappings)
                    {
                        if (x.IsNonFileColumn == true)
                            x.IncomingField = x.MAFField;

                        x.IncomingField = x.IncomingField.ToUpper();
                        x.MAFField = x.MAFField.ToUpper();
                    }

                    admsWrk.Update(eventMessage.AdmsLog.ProcessCode,
                          FrameworkUAD_Lookup.Enums.FileStatusType.Processing,
                          FrameworkUAD_Lookup.Enums.ADMS_StepType.Validator_Set_Variables,
                          FrameworkUAD_Lookup.Enums.ProcessingStatusType.In_Validation,
                          FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_Codesheet_Validation, 1,
                          "executing code region Method variables and containers - ValidatorSetVariables", true,
                          eventMessage.AdmsLog.SourceFileId);

                    #region Method variables and containers - ValidatorSetVariables


                    FrameworkUAD.Object.ImportFile dataIV = new FrameworkUAD.Object.ImportFile();
                    dataIV.ThreadId = eventMessage.ThreadId;
                    FrameworkUAD.Object.ValidationResult validationResult = new FrameworkUAD.Object.ValidationResult(eventMessage.ImportFile, eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog.ProcessCode);

                    bool IsPubCodeMissing = false;
                    bool isValidFileType = true;
                    bool isFileSchemaValid = true;
                    int fileTotalRowCount = 0;
                    List<FrameworkUAD.Object.ImportErrorSummary> listIES = new List<FrameworkUAD.Object.ImportErrorSummary>();


                    ConsoleMessage("VALIDATOR: Processing file.", eventMessage.AdmsLog.ProcessCode, true);

                    //Lists for Unexpected and NotFound data
                    HashSet<string> finalUnexpected = new HashSet<string>();
                    List<FrameworkUAS.Entity.FieldMapping> finalNotFound = new List<FrameworkUAS.Entity.FieldMapping>();

                    FileConfiguration fileConfig = new FileConfiguration()
                    {
                        FileColumnDelimiter = eventMessage.SourceFile.Delimiter,
                        IsQuoteEncapsulated = eventMessage.SourceFile.IsTextQualifier,
                    };
                    #endregion

                    #region Process File
                    bool dateParsingFailure = false;
                    bool allCircMappedColumnsExist = true;
                    if (fileWorker.AcceptableFileType(eventMessage.ImportFile) == false)
                    {
                        isValidFileType = false;
                    }
                    else
                    {
                        #region data processing preparation
                        ConsoleMessage("Gettting File Row Count", eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);

                        try
                        {
                            if (isSpecialFile == true && FrameworkUAD_Lookup.Enums.GetSpecialFileResultType(sfr.CodeName) == FrameworkUAD_Lookup.Enums.SpecialFileResultTypes.Creates_Data_Table)
                                fileTotalRowCount = 1;
                            else
                                fileTotalRowCount = fileWorker.GetRowCount(eventMessage.ImportFile);
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, client, "ADMS.Services.Validator.Validator.ProcessFileAsObject - fileTotalRowCount");
                        }

                        try
                        {
                            validationResult.HeadersOriginal = fileWorker.GetFileHeaders(eventMessage.ImportFile, fileConfig);
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, client, "ADMS.Services.Validator.Validator.ProcessFileAsObject - validationResult.HeadersOriginal");
                        }
                        try
                        {
                            validationResult.BadDataOriginalHeaders = validationResult.HeadersOriginal.DeepClone();
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, client, "ADMS.Services.Validator.Validator.ProcessFileAsObject - validationResult.BadDataOriginalHeaders");
                        }
                        ConsoleMessage("Row Count: " + fileTotalRowCount.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                        //fileRowProcessedCount = 0;
                        validationResult.ImportFile = eventMessage.ImportFile;
                        validationResult.TotalRowCount = fileTotalRowCount;
                        validationResult.HasError = false;
                        validationResult.RecordImportErrors = new HashSet<ImportError>();

                        ConsoleMessage("Getting Client PubCodes", eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                        ConsoleMessage("PubCodeCount: " + clientPubCodes.Count.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                        ConsoleMessage("PubCodes retrieved", eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                        #endregion

                        #region if a circ file check that all columns match what is mapped
                        KMPlatform.BusinessLogic.Service servData = new KMPlatform.BusinessLogic.Service();
                        KMPlatform.Entity.Service service = servData.Select(eventMessage.SourceFile.ServiceID);
                        if (service.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase))
                        {
                            //eventMessage.SourceFile.FieldMappings - check that all Not Ignored mappings exist in HeadersOriginal
                            List<FrameworkUAD_Lookup.Entity.Code> fieldMappingTypeCodes = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping);
                            int ignoreId = fieldMappingTypeCodes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString())).CodeId;

                            //validationResult.HeadersOriginal - check that all columns exist in mapping
                            foreach (var col in validationResult.HeadersOriginal.Keys)
                            {
                                if (!eventMessage.SourceFile.FieldMappings.Where(x => x.IsNonFileColumn == false).ToList().Exists(x => x.IncomingField.Equals(col.ToString(), StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    allCircMappedColumnsExist = false;
                                    finalUnexpected.Add(col.ToString());
                                }
                            }

                            foreach (FrameworkUAS.Entity.FieldMapping map in eventMessage.SourceFile.FieldMappings.Where(x => x.FieldMappingTypeID != ignoreId && x.FieldMappingTypeID != kmTransformTypeID && x.IsNonFileColumn == false))
                            {
                                if (!validationResult.HeadersOriginal.ContainsKey(map.IncomingField))
                                {
                                    allCircMappedColumnsExist = false;
                                    finalNotFound.Add(map);
                                }
                            }
                        }
                        #endregion

                        //rename any nonFile columns
                        //once columns are renamed it will change the Hash of the object
                        #region lets drop any "ignored" columns
                        ConsoleMessage("Column count before drop ignored: " + eventMessage.SourceFile.FieldMappings.Count.ToString());
                        List<FrameworkUAS.Entity.FieldMapping> deleteIgnoredFieldMappings = (from i in eventMessage.SourceFile.FieldMappings
                                                                                             where i.FieldMappingTypeID == ignoredTypeID
                                                                                             select i).ToList();
                        var existingMappings = eventMessage.SourceFile.FieldMappings.ToList();

                        foreach (FrameworkUAS.Entity.FieldMapping del in deleteIgnoredFieldMappings)
                        {
                            existingMappings.Remove(del);

                            //if (existingMappings.Count(x => x.FieldMappingID == del.FieldMappingID) > 0)
                            //    existingMappings.RemoveWhere(x => x.FieldMappingID == del.FieldMappingID);
                        }
                        eventMessage.SourceFile.FieldMappings = new HashSet<FrameworkUAS.Entity.FieldMapping>(existingMappings);

                        ConsoleMessage("Column count after drop ignored: " + eventMessage.SourceFile.FieldMappings.Count.ToString());
                        #endregion

                        #region Check: Ensure PUBCODE column is mapped
                        if (KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                        {
                            ConsoleMessage("DataCompare file - no PubCode required", eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                        }
                        else
                        {
                            ConsoleMessage("Check: Ensure PUBCODE column is mapped", eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                            // Verfify file contains PUBCODE column
                            //this was .IncomingField but changing to x.MafField
                            if (eventMessage.SourceFile.FieldMappings.Count(x => x.MAFField.Equals("PubCode", StringComparison.CurrentCultureIgnoreCase)) == 0)
                            {
                                string msg = "File requires a PUBCODE column";
                                IsPubCodeMissing = true;

                                dataIV = ValidationError(dataIV, msg, dft, eventMessage.AdmsLog);
                            }
                            ConsoleMessage("Done: Ensure PUBCODE column is mapped", eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                        }
                        #endregion

                        if (allCircMappedColumnsExist == true)
                        {
                            if (fileTotalRowCount > 0)
                            {
                                #region Process File
                                //disable indexes
                                FrameworkUAD.BusinessLogic.SubscriberTransformed stWorker = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
                                //stWorker.DisableTableIndexes(eventMessage.Client.ClientConnections, eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog.ProcessCode);
                                if (IsPubCodeMissing == false)
                                {
                                    #region Get Data and handle SpecialFile
                                    //loads the data from the file
                                    ConsoleMessage("Begin get ImportVessel", eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                                    FrameworkUAD.BusinessLogic.ImportFile ifWorker = new FrameworkUAD.BusinessLogic.ImportFile();
                                    if (isSpecialFile == false)
                                        dataIV = ifWorker.GetImportFile(eventMessage.ImportFile, fileConfig);
                                    else
                                    {
                                        if (FrameworkUAD_Lookup.Enums.GetSpecialFileResultType(sfr.CodeName) == FrameworkUAD_Lookup.Enums.SpecialFileResultTypes.Creates_Data_Table)
                                        {
                                            dataIV = SpecialFileGetImportFile(cSpecialFile, eventMessage);
                                            fileTotalRowCount = dataIV.OriginalRowCount;
                                        }
                                        else
                                        {
                                            dataIV = ifWorker.GetImportFile(eventMessage.ImportFile, fileConfig);
                                            dataIV = SpecialFileAlterData(cSpecialFile, eventMessage, dataIV);
                                        }
                                    }
                                    ConsoleMessage("Done get ImportFile", eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                                    #endregion
                                    #region set ImportFile properties
                                    dataIV.ClientId = eventMessage.Client.ClientID;
                                    dataIV.SourceFileId = eventMessage.SourceFile.SourceFileID;
                                    dataIV.ProcessCode = eventMessage.AdmsLog.ProcessCode;
                                    #endregion
                                    #region Insert Import Errors to SubscriberInvalid
                                    if (dataIV.ImportErrorCount > 0)
                                    {
                                        FrameworkUAD.BusinessLogic.ImportError ieWorker = new FrameworkUAD.BusinessLogic.ImportError();
                                        ieWorker.SaveBulkSqlInsert(dataIV.ImportErrors.ToList(), eventMessage.Client.ClientConnections);

                                        validationResult.RecordImportErrorCount = dataIV.ImportErrorCount;
                                        validationResult.RecordImportErrors = dataIV.ImportErrors;
                                    }
                                    #endregion
                                    #region Process data
                                    if (dataIV.TotalRowCount > 0)
                                    {
                                        #region Transformations
                                        admsWrk.Update(eventMessage.AdmsLog.ProcessCode,
                                             FrameworkUAD_Lookup.Enums.FileStatusType.Processing,
                                             FrameworkUAD_Lookup.Enums.ADMS_StepType.Validator_Transformations,
                                             FrameworkUAD_Lookup.Enums.ProcessingStatusType.In_Transformations,
                                             FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_Transformations, 1,
                                             "Start: Transform Data " + DateTime.Now.TimeOfDay.ToString(), true,
                                             eventMessage.AdmsLog.SourceFileId);

                                        try
                                        {
                                            dataIV = TransformImportFileData(dataIV, serviceFeature, dft.ToString(), eventMessage.SourceFile, eventMessage.AdmsLog, dft);
                                        }
                                        catch (Exception ex)
                                        {
                                            LogError(ex, client, "ADMS.Services.Validator.Validator.ProcessFile - Transform Data");
                                        }

                                        admsWrk.Update(eventMessage.AdmsLog.ProcessCode,
                                             FrameworkUAD_Lookup.Enums.FileStatusType.Processing,
                                             FrameworkUAD_Lookup.Enums.ADMS_StepType.Executing_Custom_Code,
                                             FrameworkUAD_Lookup.Enums.ProcessingStatusType.Transformed,
                                             FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_Transformations, 1,
                                             "Done: Transform Data " + DateTime.Now.TimeOfDay.ToString(), true,
                                             eventMessage.AdmsLog.SourceFileId);
                                        #endregion
                                        #region lets drop any "kmTransform" columns
                                        ConsoleMessage("Column count before drop kmTransform: " + eventMessage.SourceFile.FieldMappings.Count.ToString());
                                        List<FrameworkUAS.Entity.FieldMapping> deletekmTransformFieldMappings = (from i in eventMessage.SourceFile.FieldMappings
                                                                                                                 where i.FieldMappingTypeID == kmTransformTypeID
                                                                                                                 select i).ToList();
                                        foreach (FrameworkUAS.Entity.FieldMapping del in deletekmTransformFieldMappings)
                                        {
                                            if (eventMessage.SourceFile.FieldMappings.Count(x => x.FieldMappingID == del.FieldMappingID) > 0)
                                                eventMessage.SourceFile.FieldMappings.RemoveWhere(x => x.FieldMappingID == del.FieldMappingID);
                                        }
                                        ConsoleMessage("Column count after drop kmTransform: " + eventMessage.SourceFile.FieldMappings.Count.ToString());
                                        #endregion
                                        #region Execute custom code after transformations
                                        //if (eventMessage.Client.FtpFolder.Equals("Meister"))
                                        //{
                                        //    ClientMethods.Meister meister = new Meister();
                                        //    dataIV = meister.CommaSeperateValues(eventMessage, dataIV);
                                        //}
                                        //else
                                        //{
                                        ClientMethods.ClientSpecialCommon csc = new ClientMethods.ClientSpecialCommon();
                                        FrameworkUAS.BusinessLogic.ClientCustomProcedure ccpWorker = new FrameworkUAS.BusinessLogic.ClientCustomProcedure();
                                        List<ClientCustomProcedure> ccps = ccpWorker.SelectClient(eventMessage.Client.ClientID);
                                        csc.ExecuteClientCustomCode(FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_Transformations, eventMessage.Client, ccps, eventMessage.SourceFile, eventMessage);
                                        //}
                                        #endregion

                                        #region Cat/Tran code value to id conversion if present
                                        //this code is pending Sunils approval - will remain commented out until then - Justin Wagner 9/30/2015

                                        //FrameworkUAD_Lookup.BusinessLogic.TransactionCode tWorker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode();
                                        //FrameworkUAD_Lookup.BusinessLogic.CategoryCode catWorker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode();
                                        //List<FrameworkUAD_Lookup.Entity.TransactionCode> tranFreeCodes = tWorker.SelectActiveIsFree(true).ToList();
                                        //List<FrameworkUAD_Lookup.Entity.CategoryCode> catFreeCodes = catWorker.SelectActiveIsFree(true).ToList();

                                        //foreach (var key in dataIV.DataTransformed.Keys)
                                        //{
                                        //    StringDictionary myRow = dataIV.DataTransformed[key];
                                        //    int catId = 0;
                                        //    int.TryParse(myRow["CategoryID"].ToString(), out catId);
                                        //    int tranId = 0;
                                        //    int.TryParse(myRow["TransactionID"].ToString(), out tranId);

                                        //    //CategoryID, TransactionID
                                        //    if (catId > 0)
                                        //    {
                                        //        FrameworkUAD_Lookup.Entity.CategoryCode cc = catFreeCodes.SingleOrDefault(x => x.CategoryCodeValue == catId);
                                        //        if(cc != null)
                                        //            myRow["CategoryID"] = cc.CategoryCodeID.ToString();
                                        //        else
                                        //        {
                                        //            //try paid codes
                                        //            List<FrameworkUAD_Lookup.Entity.CategoryCode> catPaidCodes = catWorker.SelectActiveIsFree(false).ToList();
                                        //            FrameworkUAD_Lookup.Entity.CategoryCode ccPaid = catPaidCodes.SingleOrDefault(x => x.CategoryCodeValue == catId);
                                        //            myRow["CategoryID"] = ccPaid != null ? ccPaid.CategoryCodeID.ToString() : "0";
                                        //        }
                                        //    }
                                        //    if (tranId > 0)
                                        //    {
                                        //        FrameworkUAD_Lookup.Entity.TransactionCode tc = tranFreeCodes.SingleOrDefault(x => x.TransactionCodeValue == tranId);
                                        //        if(tc != null)
                                        //            myRow["TransactionID"] = tc.TransactionCodeID.ToString();
                                        //        else
                                        //        {
                                        //            //try paid codes
                                        //            List<FrameworkUAD_Lookup.Entity.TransactionCode> tranPaidCodes = tWorker.SelectActiveIsFree(false).ToList();
                                        //            FrameworkUAD_Lookup.Entity.TransactionCode tcPaid = tranPaidCodes.SingleOrDefault(x => x.TransactionCodeValue == tranId);
                                        //            myRow["TransactionID"] = tcPaid != null ? tcPaid.TransactionCodeID.ToString() : "0";
                                        //        }
                                        //    }
                                        //}
                                        #endregion

                                        //not doing column renaming
                                        #region Column Validation
                                        #region need to add any CreatedDimension values from AdHocDimensionGroup to the FieldMapping list if they do not exist
                                        int fieldMappingTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString())).CodeId;

                                        FrameworkUAS.BusinessLogic.AdHocDimensionGroup ahdgWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
                                        List<FrameworkUAS.Entity.AdHocDimensionGroup> ahdGroups = new List<AdHocDimensionGroup>();
                                        if (eventMessage.Client.FtpFolder.Equals("TenMissions"))
                                            ahdGroups = ahdgWorker.Select(eventMessage.Client.ClientID, true).ToList();
                                        else
                                            ahdGroups = ahdgWorker.Select(eventMessage.Client.ClientID, true).Where(x => x.IsActive == true).ToList();

                                        if (ahdGroups != null && ahdGroups.Count > 0)
                                        {
                                            List<string> distCreatedDims = new List<string>();
                                            foreach (FrameworkUAS.Entity.AdHocDimensionGroup adg in ahdGroups)
                                            {
                                                if (!distCreatedDims.Contains(adg.CreatedDimension))
                                                    distCreatedDims.Add(adg.CreatedDimension);
                                            }

                                            foreach (string s in distCreatedDims)
                                            {
                                                if (eventMessage.SourceFile.FieldMappings.Count(x => x.MAFField.Equals(s, StringComparison.CurrentCultureIgnoreCase)) == 0)//(!sourceFile.FieldMappings.Exists(x => x.MAFField.Equals(s, StringComparison.CurrentCultureIgnoreCase)))
                                                {
                                                    FieldMapping dimFM = new FieldMapping();
                                                    dimFM.FieldMappingTypeID = fieldMappingTypeID;
                                                    dimFM.IsNonFileColumn = true;
                                                    dimFM.SourceFileID = eventMessage.SourceFile.SourceFileID;
                                                    dimFM.IncomingField = s;
                                                    dimFM.MAFField = s;
                                                    dimFM.PubNumber = 0;
                                                    dimFM.DataType = "varchar";
                                                    dimFM.PreviewData = string.Empty;
                                                    dimFM.HasMultiMapping = false;
                                                    dimFM.CreatedByUserID = 1;
                                                    dimFM.ColumnOrder = eventMessage.SourceFile.FieldMappings.Count + 1;
                                                    dimFM.DateCreated = DateTime.Now;

                                                    //FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
                                                    //dimFM.FieldMappingID = fmWorker.Save(dimFM);

                                                    eventMessage.SourceFile.FieldMappings.Add(dimFM);
                                                    if (!eventMessage.SourceFile.FieldMappings.Contains(dimFM))
                                                        eventMessage.SourceFile.FieldMappings.Add(dimFM);
                                                    if (!dataIV.HeadersTransformed.ContainsKey(s))
                                                        dataIV.HeadersTransformed.Add(s, (dataIV.HeadersTransformed.Count + 1).ToString());
                                                }
                                            }

                                        }
                                        ahdGroups = null;
                                        ConsoleMessage("try FieldMappings count", eventMessage.AdmsLog.ProcessCode, true);
                                        ConsoleMessage("FieldMappings counts " + eventMessage.SourceFile.FieldMappings.Count.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                                        #endregion
                                        #region Not found check
                                        foreach (FieldMapping map in eventMessage.SourceFile.FieldMappings)
                                        {
                                            if (!dataIV.HeadersTransformed.ContainsKey(map.IncomingField))
                                                finalNotFound.Add(map);
                                        }
                                        //If finalNotFound.Count > 0 log that a mapped column was missing from the file but continue processing
                                        if (finalNotFound.Count > 0)
                                        {
                                            validationResult.HasError = true;
                                            validationResult.RecordImportErrorCount++;
                                            foreach (FrameworkUAS.Entity.FieldMapping nf in finalNotFound)
                                                validationResult.NotFoundColumns.Add(nf.IncomingField);
                                            string notFoundError = "VALIDATOR: Not Found Columns - " + String.Join(", ", validationResult.NotFoundColumns) + "<br/>";
                                            ImportError ie = new ImportError(eventMessage.ThreadId, eventMessage.AdmsLog.ProcessCode, eventMessage.SourceFile.SourceFileID, notFoundError);
                                            validationResult.RecordImportErrors.Add(ie);
                                            ConsoleMessage(notFoundError);

                                            //remove the column from FieldMapping
                                            foreach (FrameworkUAS.Entity.FieldMapping nf in finalNotFound)
                                                eventMessage.SourceFile.FieldMappings.Remove(nf);
                                        }
                                        ConsoleMessage("Done: Check: Missing Columns - NotFound " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                                        #endregion
                                        #region Check: Undefined Columns - Unexpected
                                        foreach (var col in dataIV.HeadersTransformed.Keys)
                                        {
                                            if (!col.ToString().Equals("OriginalImportRow", StringComparison.CurrentCultureIgnoreCase) && !col.ToString().Equals("PubCodeId", StringComparison.CurrentCultureIgnoreCase))
                                            {
                                                if (eventMessage.SourceFile.FieldMappings.Count(x => x.IncomingField.Equals(col.ToString(), StringComparison.CurrentCultureIgnoreCase) || x.FieldMappingTypeID != kmTransformTypeID) == 0)
                                                    finalUnexpected.Add(col.ToString());
                                            }
                                        }

                                        if (finalUnexpected.Count > 0)
                                        {
                                            validationResult.HasError = true;
                                            validationResult.RecordImportErrorCount++;
                                            finalUnexpected.ToList().ForEach(x => { validationResult.UnexpectedColumns.Add(x); });
                                            StringBuilder sb = new StringBuilder();
                                            sb.AppendLine("One or more unmapped columns were found in your import file: " + eventMessage.ImportFile.Name + "<br/>");
                                            sb.AppendLine("The columns have been excluded.<br/>");
                                            sb.AppendLine("If these columns contain data that is required for processing or you intent to have in your UAD,");
                                            sb.Append(" please add these columns to your file mapping and resubmit for processing.<br/>");
                                            sb.AppendLine("Unexpected Columns - " + String.Join(", ", finalUnexpected) + "<br/>");
                                            ImportError ie = new ImportError(eventMessage.ThreadId, eventMessage.AdmsLog.ProcessCode, eventMessage.SourceFile.SourceFileID, sb.ToString());
                                            validationResult.RecordImportErrors.Add(ie);
                                            ConsoleMessage(sb.ToString());

                                            //remove the column from the DataTransformed Headers and Data
                                            foreach (string u in finalUnexpected)
                                            {
                                                dataIV.HeadersTransformed.Remove(u);
                                                foreach (var dt in dataIV.DataTransformed.Keys)
                                                {
                                                    StringDictionary myRow = dataIV.DataTransformed[dt];
                                                    myRow.Remove(u);
                                                }
                                            }
                                        }

                                        ConsoleMessage("Done: Check: Undefined Columns - Unexpected " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                                        #endregion
                                        ConsoleMessage("VALIDATOR: Unexpected count = " + finalUnexpected.Count, eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                                        ConsoleMessage("VALIDATOR: Not Found count = " + finalNotFound.Count, eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                                        #endregion

                                        if (!dft.ToString().Replace("_", " ").Equals("Data Compare") && !dft.ToString().Replace("_", " ").Equals("Field Update"))
                                        {
                                            #region Apply any AdHocDimensions
                                            if (eventMessage.Client.FtpFolder.Equals("TenMissions"))
                                            {
                                                ClientMethods.TenMissions tm = new TenMissions();
                                                dataIV = tm.ApplyConditionalAdHocs(dataIV);
                                            }
                                            else if (eventMessage.Client.FtpFolder.Equals("Advanstar"))
                                            {
                                                ClientMethods.Advanstar adv = new Advanstar();
                                                dataIV = adv.ApplyConditionalAdHocs(dataIV);
                                                dataIV = ApplyAdHocDimensions(dataIV);
                                            }
                                            else
                                                dataIV = ApplyAdHocDimensions(dataIV);
                                            #endregion
                                        }

                                        #region QDate Format
                                        //ToDo:  RuleSet: QDateDefault
                                        //will not support WhereClause

                                        string qDateColumnHeader = string.Empty;
                                        FieldMapping thisFM = eventMessage.SourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals(Core_AMS.Utilities.Enums.MAFFieldStandardFields.QUALIFICATIONDATE.ToString(), StringComparison.CurrentCultureIgnoreCase));
                                        if (thisFM != null && dataIV.HeadersTransformed.ContainsKey(thisFM.IncomingField))
                                            qDateColumnHeader = thisFM.IncomingField;                                        

                                        thisFM = null;
                                        if (!string.IsNullOrEmpty(qDateColumnHeader))
                                        {
                                            string QDatePattern2 = eventMessage.SourceFile.QDateFormat.Replace('D', 'd').Replace('Y', 'y');
                                            foreach (var key in dataIV.DataTransformed.Keys)
                                            {
                                                bool provideDefault = false;
                                                StringDictionary myRow = dataIV.DataTransformed[key];
                                                if (!string.IsNullOrEmpty(myRow[qDateColumnHeader]))
                                                {
                                                    //Get QDateValue and then remove all characters not numeric for comparison.
                                                    string QDateValue = myRow[qDateColumnHeader].ToString();
                                                    QDateValue = new string(QDateValue.Where(c => char.IsDigit(c)).ToArray());
                                                    if (string.IsNullOrEmpty(QDateValue))
                                                    {
                                                        //DateTime? qDate = GetQDate(eventMessage.SourceFile);
                                                        //myRow[qDateColumnHeader] = qDate.Value.ToShortDateString() ?? null;
                                                        provideDefault = true;
                                                    }
                                                    else
                                                    {
                                                        try
                                                        {
                                                            ValidatorMethods.TryParseDateColumnHeader(
                                                                myRow, eventMessage.SourceFile.QDateFormat, qDateColumnHeader, QDatePattern2, QDateValue);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            dateParsingFailure = true;
                                                            provideDefault = true;
                                                            //myRow[qDateColumnHeader] = DateTime.Now.ToShortDateString();
                                                            LogError(ex, client, this.GetType().Name.ToString() + ".ProcessFile - QDate Format");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    //DateTime? qDate = GetQDate(eventMessage.SourceFile);
                                                    //myRow[qDateColumnHeader] = qDate.Value.ToShortDateString() ?? null;
                                                    provideDefault = true;
                                                }                                                
                                                //Provide Default QDATE Later during SubscriberTransformed creation                                               
                                            }
                                        }
                                        #endregion

                                        //if this is a circ file and we had Qdate parsing failure need to reject file and stop processing
                                        servData = new KMPlatform.BusinessLogic.Service();
                                        service = servData.Select(eventMessage.SourceFile.ServiceID);
                                        if (dateParsingFailure == true && service.ServiceCode.Equals(KMPlatform.Enums.Services.CIRCFILEMAPPER.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            string reason = string.Empty;
                                            reason = "Your file is configured to have a QDate format of " + eventMessage.SourceFile.QDateFormat + " but your QDate data does not match that format.  Please either correctly format your QDate file data or adjust the setup configuration.";
                                            Emailer.Emailer em = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.RejectFile);
                                            em.RejectFile(eventMessage.Client, eventMessage.ImportFile, reason);
                                            ThreadDictionary.Remove(eventMessage.ThreadId);
                                            //ADMSProcessingQue.RemoveClientFile(eventMessage.Client, eventMessage.ImportFile);
                                            if (File.Exists(eventMessage.ImportFile.FullName))
                                                File.Delete(eventMessage.ImportFile.FullName);
                                            ConsoleMessage("Awaiting new file....", eventMessage.AdmsLog.ProcessCode, true);
                                        }
                                        else
                                        {
                                            dateParsingFailure = false;//just to be safe reset back to false

                                            dataIV = ValidateData(dataIV, eventMessage, serviceFeature, dft);
                                            if (dataIV == null)
                                                return;

                                            System.Console.ResetColor();

                                            if (dataIV.HasError == true)
                                                validationResult.HasError = true;

                                            validationResult.OriginalRowCount = dataIV.OriginalRowCount;
                                            validationResult.TransformedRowCount = dataIV.TransformedRowCount;
                                            validationResult.RecordImportErrorCount = dataIV.ImportErrorCount;
                                            validationResult.ImportedRowCount = dataIV.ImportedRowCount;

                                            if (dataIV.ImportErrorCount > 0 && dataIV.ImportErrors != null)
                                            {
                                                try
                                                {
                                                    //validationResult.RecordImportErrors.AddRange(dataIV.ImportErrors);
                                                    dataIV.ImportErrors.ToList().ForEach(x => { validationResult.RecordImportErrors.Add(x); });
                                                }
                                                catch (Exception ex)
                                                {
                                                    string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                                    ConsoleMessage(message);
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        ValidationError(dataIV, "No data from file, stopping processing.", dft, eventMessage.AdmsLog);
                                    }

                                    #region Codesheet validation
                                    if (KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeature.SFName) != KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                                    {
                                        ConsoleMessage("Start: Codesheet validation " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                                        try
                                        {
                                            FrameworkUAD.BusinessLogic.CodeSheet csData = new FrameworkUAD.BusinessLogic.CodeSheet();
                                            csData.CodeSheetValidation(eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog.ProcessCode, eventMessage.Client.ClientConnections);
                                        }
                                        catch (Exception ex)
                                        {
                                            LogError(ex, client, this.GetType().Name.ToString() + ".ProcessFile - Codesheet Validation");
                                        }
                                        ConsoleMessage("Done: Codesheet validation " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                                    }
                                    #endregion

                                    #region QSource validation
                                    if (KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeature.SFName) != KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                                    {
                                        ConsoleMessage("Start: QSource validation " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                                        try
                                        {
                                            FrameworkUAD.BusinessLogic.Operations qData = new FrameworkUAD.BusinessLogic.Operations();
                                            qData.QSourceValidation(eventMessage.Client.ClientConnections, eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog.ProcessCode);
                                        }
                                        catch (Exception ex)
                                        {
                                            LogError(ex, client, "ADMS.Services.Validator.Validator.ProcessFile - QSource Validation");
                                        }
                                        ConsoleMessage("Done: QSource validation " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                                    }
                                    #endregion

                                    #region Select Demographic ImportErrors
                                    try
                                    {
                                        //get any error info from CSV - will be written to table ImportError
                                        //add errors to allDataIV and remove bad rows from allDataIV
                                        FrameworkUAD.BusinessLogic.ImportErrorSummary ieData = new FrameworkUAD.BusinessLogic.ImportErrorSummary();
                                        listIES = ieData.Select(eventMessage.SourceFile.SourceFileID, eventMessage.AdmsLog.ProcessCode, eventMessage.Client.ClientConnections);
                                        int errorTotal = listIES.Sum(x => x.ErrorCount);
                                        if (listIES.Count > 0)
                                        {
                                            validationResult.HasError = true;
                                            validationResult.DimensionImportErrorCount += errorTotal;
                                        }
                                        foreach (FrameworkUAD.Object.ImportErrorSummary ies in listIES)
                                            validationResult.DimensionImportErrorSummaries.Add(ies);

                                        //Get List of Split Into Row Transformations, based on the MAFField and Delimiter and PubCode if value exists without being split
                                        //that row is probably garbage and needs to be removed. This is caused by multiple split into row transformations
                                        // Attempt to remove bad "FAKE" dimensions created from Multiple Split into rows
                                        FrameworkUAS.BusinessLogic.TransformSplit tsWorker = new FrameworkUAS.BusinessLogic.TransformSplit();
                                        List<FrameworkUAS.Object.TransformSplitInfo> tsList = new List<FrameworkUAS.Object.TransformSplitInfo>();
                                        tsList = tsWorker.SelectObject(eventMessage.SourceFile.SourceFileID);
                                        foreach (FrameworkUAS.Object.TransformSplitInfo tsi in tsList)
                                        {
                                            string pubcode = "";
                                            if (clientPubCodes.SingleOrDefault(x => x.Key.Equals(tsi.PubID)).Value != null)
                                                pubcode = clientPubCodes.FirstOrDefault(x => x.Key == tsi.PubID).Value;

                                            if (String.IsNullOrEmpty(pubcode))
                                                continue;

                                            char delimiter = CommonEnums.GetDelimiterSymbol(tsi.Delimiter).GetValueOrDefault(',');

                                            validationResult.DimensionImportErrorSummaries.RemoveWhere(x => x.PubCode.Equals(pubcode, StringComparison.CurrentCultureIgnoreCase)
                                                                                                            && x.MAFField.Equals(tsi.MAFField, StringComparison.CurrentCultureIgnoreCase)
                                                                                                            && x.Value.Contains(delimiter));
                                            validationResult.DimensionImportErrorCount = validationResult.DimensionImportErrorSummaries.Count();
                                        }
                                        #endregion

                                        #region OLD NO LONGER USED ADDED TO CREATEDASHBOARDREPORTS -- Create DimensionReport
                                        //try
                                        //{
                                        //    if (validationResult.DimensionImportErrorCount > 0)
                                        //    {
                                        //        #region Grab Fresh Values for Creating Reports
                                        //        //Why grab a fresh FieldMapping? Currently in eventMessage FieldMapping variable there are missing columns (ignore and kmtransformed were removed)
                                        //        FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
                                        //        List<FrameworkUAS.Entity.FieldMapping> fieldMappings = fmWorker.Select(eventMessage.SourceFile.SourceFileID, false);
                                        //        FrameworkUAS.BusinessLogic.TransformSplit newtsWorker = new FrameworkUAS.BusinessLogic.TransformSplit();
                                        //        List<FrameworkUAS.Entity.TransformSplit> transformSplits = newtsWorker.SelectSourceFileID(eventMessage.SourceFile.SourceFileID);
                                        //        FrameworkUAS.BusinessLogic.TransformationFieldMap tfmWorker = new FrameworkUAS.BusinessLogic.TransformationFieldMap();
                                        //        List<FrameworkUAS.Entity.TransformationFieldMap> allTFM = tfmWorker.Select();
                                        //        #endregion

                                        //        DataTable dtDimensionReport = new DataTable();
                                        //        #region DataTable Add Columns
                                        //        foreach (FrameworkUAS.Entity.FieldMapping fm in fieldMappings.OrderBy(x => x.ColumnOrder))
                                        //        {
                                        //            DataColumn dc = new DataColumn(fm.IncomingField);
                                        //            dtDimensionReport.Columns.Add(dc);
                                        //        }
                                        //        #endregion
                                        //        #region Add Rows
                                        //        List<int> origImportRowProcessed = new List<int>();
                                        //        List<FrameworkUAD.Object.ImportRowNumber> transImportRowNumbers = new List<FrameworkUAD.Object.ImportRowNumber>();
                                        //        FrameworkUAD.BusinessLogic.SubscriberTransformed subtransWorker = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
                                        //        transImportRowNumbers = subtransWorker.SelectImportRowNumbers(eventMessage.Client.ClientConnections, eventMessage.AdmsLog.ProcessCode);
                                        //        foreach (var key in dataIV.DataTransformed.Keys)
                                        //        {
                                        //            if (transImportRowNumbers.Count(x => x.SubscriberImportRowNumber == key) == 0)
                                        //                continue;

                                        //            StringDictionary myRow = dataIV.DataTransformed[key];
                                        //            int originalRowNumber = dataIV.TransformedRowToOriginalRowMap[key];

                                        //            string pubCodeColumn = fieldMappings.FirstOrDefault(x => x.MAFField.Equals("pubcode", StringComparison.CurrentCultureIgnoreCase)).IncomingField;
                                        //            //Add only if dimension fell under ValidationResult DimensionImportErrorSummaries
                                        //            if (validationResult.DimensionImportErrorSummaries.Any(x => x.PubCode.Equals(myRow[pubCodeColumn], StringComparison.CurrentCultureIgnoreCase))
                                        //                    && validationResult.DimensionImportErrorSummaries.Any(x => x.Value.Equals(myRow[fieldMappings.FirstOrDefault(y => y.MAFField.Equals(x.MAFField, StringComparison.CurrentCultureIgnoreCase)).IncomingField], StringComparison.CurrentCultureIgnoreCase)))
                                        //            {
                                        //                //Skip if the row was already added otherwise it will add duplicate rows
                                        //                if (origImportRowProcessed.Count(z => z.Equals(originalRowNumber)) > 0)
                                        //                    continue;

                                        //                DataRow dr = dtDimensionReport.NewRow();
                                        //                foreach (FrameworkUAS.Entity.FieldMapping fm in fieldMappings.OrderBy(co => co.ColumnOrder))
                                        //                {
                                        //                    string data = "";
                                        //                    int transformationID = 0;
                                        //                    if (allTFM.Count(z => z.FieldMappingID == fm.FieldMappingID) > 0)
                                        //                    {
                                        //                        transformationID = allTFM.FirstOrDefault(z => z.FieldMappingID == fm.FieldMappingID).TransformationID;
                                        //                        string delimiter = "";
                                        //                        if (transformSplits.FirstOrDefault(z => z.TransformationID == transformationID) != null)
                                        //                            delimiter = transformSplits.FirstOrDefault(z => z.TransformationID == transformationID).Delimiter;

                                        //                        //This will collect all the duplicate rows and combine the column that had a split into row
                                        //                        data = CombineValues(dataIV.DataTransformed, fm.IncomingField, delimiter, originalRowNumber.ToString());
                                        //                    }
                                        //                    else
                                        //                        data = string.IsNullOrEmpty(myRow[fm.IncomingField].ToString()) ? "" : myRow[fm.IncomingField].ToString();

                                        //                    dr[fm.IncomingField] = data;
                                        //                }
                                        //                dtDimensionReport.Rows.Add(dr);
                                        //                origImportRowProcessed.Add(originalRowNumber);
                                        //            }
                                        //        }
                                        //        #endregion
                                        //        #region Save File
                                        //        if (dtDimensionReport.Rows.Count > 0)
                                        //        {
                                        //            string clientArchiveDir = Core.ADMS.BaseDirs.getClientArchiveDir().ToString();
                                        //            string dir = clientArchiveDir + @"\" + eventMessage.Client.FtpFolder + @"\Reports\";
                                        //            System.IO.Directory.CreateDirectory(dir);
                                        //            string cleanProcessCode = Core_AMS.Utilities.StringFunctions.CleanProcessCodeForFileName(eventMessage.AdmsLog.ProcessCode);
                                        //            string transformedReportName = dir + cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._DimensionSubscriber.ToString() + ".xlsx";
                                        //            Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
                                        //            Telerik.Windows.Documents.Spreadsheet.Model.Workbook wb = ef.GetWorkbook(dtDimensionReport, "DimensionSubscriber");
                                        //            Telerik.Windows.Documents.Spreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();
                                        //            using (FileStream output = new FileStream(transformedReportName, FileMode.Create))
                                        //            {
                                        //                formatProvider.Export(wb, output);
                                        //            }
                                        //        }
                                        //        dtDimensionReport = null;
                                        //        #endregion
                                        //    }
                                        //}
                                        //catch (Exception ex)
                                        //{
                                        //    LogError(ex, client, "ADMS.Services.Validator.Validator.ProcessFile - Create DimensionSubscriberReport");
                                        //}
                                        #endregion
                                    }
                                    catch (Exception ex)
                                    {
                                        LogError(ex, client, "ADMS.Services.Validator.Validator.ProcessFile - Select ImportErrors");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region pub code is missing
                                    validationResult.OriginalRowCount = 0;
                                    validationResult.TransformedRowCount = 0;
                                    validationResult.TotalRowCount = 0;

                                    validationResult.HasError = true;
                                    validationResult.RecordImportErrorCount++;

                                    ImportError ie = new ImportError(eventMessage.ThreadId, eventMessage.AdmsLog.ProcessCode, eventMessage.SourceFile.SourceFileID, "File requires a PUBCODE column");
                                    validationResult.RecordImportErrors.Add(ie);

                                    isFileSchemaValid = false;
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                #region File has no rows
                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine("Attention: " + eventMessage.Client.FtpFolder);
                                sb.AppendLine("We were unable to read this file.");
                                sb.AppendLine("File: " + eventMessage.ImportFile.Name);
                                sb.AppendLine("Make sure that the file name does not include invalid characters or punctuation and that it is not too long.");

                                validationResult.OriginalRowCount = 0;
                                validationResult.TransformedRowCount = 0;
                                validationResult.TotalRowCount = 0;

                                validationResult.HasError = true;
                                validationResult.RecordImportErrorCount++;

                                ImportError ie = new ImportError(eventMessage.ThreadId, eventMessage.AdmsLog.ProcessCode, eventMessage.SourceFile.SourceFileID, sb.ToString());
                                validationResult.RecordImportErrors.Add(ie);

                                isFileSchemaValid = false;
                                #endregion
                            }
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder();

                            if (finalNotFound.Count > 0)
                            {
                                foreach (FrameworkUAS.Entity.FieldMapping nf in finalNotFound)
                                    validationResult.NotFoundColumns.Add(nf.IncomingField);
                                sb.AppendLine("VALIDATOR: Not Found Columns - " + String.Join(", ", validationResult.NotFoundColumns) + "<br/>");
                                ConsoleMessage(sb.ToString());
                            }

                            if (finalUnexpected.Count > 0)
                            {
                                sb.AppendLine("One or more unmapped columns were found in your import file: " + eventMessage.ImportFile.Name + "<br/>");
                                sb.Append(" Please add these columns to your file mapping and resubmit for processing.<br/>");
                                sb.AppendLine("Unexpected Columns - " + String.Join(", ", finalUnexpected) + "<br/>");
                                ConsoleMessage(sb.ToString());
                            }

                            Emailer.Emailer em = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.RejectFile);
                            em.RejectFile(eventMessage.Client, eventMessage.ImportFile, sb.ToString());
                            ThreadDictionary.Remove(eventMessage.ThreadId);
                            //ADMSProcessingQue.RemoveClientFile(eventMessage.Client, eventMessage.ImportFile);
                            if (File.Exists(eventMessage.ImportFile.FullName))
                                File.Delete(eventMessage.ImportFile.FullName);
                            ConsoleMessage("Awaiting new file....", eventMessage.AdmsLog.ProcessCode, true);
                        }
                    }
                    #endregion

                    if (dateParsingFailure == false && allCircMappedColumnsExist == true)
                    {
                        #region Done validating - move on to DQM

                        validationResult.OriginalDuplicateRecordCount = dataIV.OriginalDuplicateRecordCount;
                        validationResult.TransformedDuplicateRecordCount = dataIV.TransformedDuplicateRecordCount;

                        FileValidated fileProcessedDetails = new FileValidated(eventMessage.AdmsLog.ImportFile, client, eventMessage.IsKnownCustomerFileName, isValidFileType, isFileSchemaValid, eventMessage.SourceFile, eventMessage.AdmsLog, validationResult, eventMessage.ThreadId);
                        //should now update the FileStatus.FileStatusTypeID to "Validated"
                        admsWrk.Update(eventMessage.AdmsLog.ProcessCode,
                                             FrameworkUAD_Lookup.Enums.FileStatusType.Processing,
                                             FrameworkUAD_Lookup.Enums.ADMS_StepType.Validator_End,
                                             FrameworkUAD_Lookup.Enums.ProcessingStatusType.Validated,
                                             FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_Validation_Process, 1,
                                             "Done: data validation " + DateTime.Now.TimeOfDay.ToString(), true,
                                             eventMessage.AdmsLog.SourceFileId);

                        //Add the ValidationResult.DimensionImportErrorCount to AdmsLog and get distinct count (distinct STRecordIdentifier from Demo)
                        FrameworkUAD.BusinessLogic.SubscriberTransformed sourceFileWrk = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
                        FrameworkUAD.Object.DimensionErrorCount counts = sourceFileWrk.SelectDimensionCount(eventMessage.AdmsLog.ProcessCode, eventMessage.Client.ClientConnections);
                        admsWrk.UpdateDimension(eventMessage.AdmsLog.ProcessCode, counts.DimensionErrorTotal, counts.DimensionDistinctSubscriberCount, 1, true, eventMessage.AdmsLog.SourceFileId);

                        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " Done data validation for file: " + eventMessage.ImportFile.Name + " client: " + eventMessage.Client.FtpFolder, eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                        //file will be Archived --> Email sent to Customer giving details of what was done --> DQM executes

                        //reset variables
                        dataIV = null;
                        validationResult = null;
                        listIES = null;

                        if (FileValidated != null)
                            FileValidated(fileProcessedDetails);
                        else
                        {
                            FileAddressGeocoded geocoded = new FileAddressGeocoded(fileProcessedDetails.ImportFile, fileProcessedDetails.Client, fileProcessedDetails.IsKnownCustomerFileName, fileProcessedDetails.IsValidFileType,
                                                                                fileProcessedDetails.IsFileSchemaValid, fileProcessedDetails.SourceFile, fileProcessedDetails.AdmsLog, fileProcessedDetails.ValidationResult);
                            DataCleanser.AddressClean ac = new DataCleanser.AddressClean();
                            ac.ExecuteAddressCleanse(fileProcessedDetails.AdmsLog, fileProcessedDetails.Client);

                            #region Add to DQM Batch
                            ADMS.Services.Emailer.Emailer e = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.Null);
                            FileProcessed processed = new FileProcessed(fileProcessedDetails.Client, fileProcessedDetails.SourceFile.SourceFileID, eventMessage.AdmsLog, eventMessage.ImportFile,
                                                                    fileProcessedDetails.IsKnownCustomerFileName, fileProcessedDetails.IsValidFileType, fileProcessedDetails.IsFileSchemaValid, fileProcessedDetails.ValidationResult);
                            e.BackupReportBuilder(processed);
                            e.HandleFileProcessed(processed);

                            AddDQMReadyFile(geocoded);
                            //ADMSProcessingQue.AddDQMReadyFile(geocoded);
                            //ADMSProcessingQue.RemoveClientFile(geocoded.Client, geocoded.ImportFile);
                            #endregion

                            ConsoleMessage("Awaiting new file....", eventMessage.AdmsLog.ProcessCode, true);
                            ThreadDictionary.Remove(eventMessage.ThreadId);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, "ADMS.Services.Validator.Validator.ProcessFileAsObject - Unhandled Exception");
            }
        }
        private FrameworkUAD.Object.ImportFile ValidateData(FrameworkUAD.Object.ImportFile dataIV, FileMoved eventMessage, KMPlatform.Entity.ServiceFeature feature, FrameworkUAD_Lookup.Enums.FileTypes dft)
        {
            FrameworkUAD_Lookup.BusinessLogic.Code codeWork = new FrameworkUAD_Lookup.BusinessLogic.Code();
            Code dbft = codeWork.SelectCodeId(eventMessage.SourceFile.DatabaseFileTypeId);
            // HashSet<FieldMapping> fieldMappings = new HashSet<FieldMapping>(sourceFile.FieldMappings.Where(x => !x.MAFField.Equals("Ignore", StringComparison.CurrentCultureIgnoreCase)).ToList());
            ConsoleMessage("Import Row count: " + dataIV.OriginalRowCount.ToString(), eventMessage.AdmsLog.ProcessCode, true);
            ConsoleMessage("Import Error count: " + dataIV.ImportErrorCount.ToString(), eventMessage.AdmsLog.ProcessCode, true);
            ConsoleMessage("Total Row count: " + dataIV.TotalRowCount.ToString(), eventMessage.AdmsLog.ProcessCode, true);

            bool isDataCompare = false;
            if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                isDataCompare = true;

            #region  create original Subscribers takes 2 minutes for 249k
            ConsoleMessage("Start: InsertOriginalSubscribers " + DateTime.Now.TimeOfDay.ToString());
            HashSet<FrameworkUAD.Entity.SubscriberOriginal> listSubscriberOriginal = new HashSet<SubscriberOriginal>();
            if (eventMessage.SourceFile.IsDQMReady == true)
                listSubscriberOriginal = InsertOriginalSubscribers(dataIV, eventMessage, feature, dft);
            ConsoleMessage("End: InsertOriginalSubscribers " + DateTime.Now.TimeOfDay.ToString());

            Dictionary<int, Guid> SubOriginalDictionary = new Dictionary<int, Guid>();
            foreach (SubscriberOriginal so in listSubscriberOriginal)
            {
                SubOriginalDictionary.Add(so.ImportRowNumber, so.SORecordIdentifier);
            }
            admsWrk.Update(eventMessage.AdmsLog.ProcessCode,
                                             FrameworkUAD_Lookup.Enums.FileStatusType.Processing,
                                             FrameworkUAD_Lookup.Enums.ADMS_StepType.Validator_Data_Validation,
                                             FrameworkUAD_Lookup.Enums.ProcessingStatusType.In_Validation,
                                             FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_Validation_Process, 1,
                                             "Start: data validation " + DateTime.Now.TimeOfDay.ToString(), true,
                                             eventMessage.AdmsLog.SourceFileId);
            #endregion

            HashSet<SubscriberTransformed> allSubscriberTransformed = new HashSet<SubscriberTransformed>();
            HashSet<SubscriberTransformed> listSubscriberTransformed = new HashSet<SubscriberTransformed>();
            HashSet<SubscriberInvalid> listSubscriberInvalid = new HashSet<SubscriberInvalid>();
            HashSet<SubscriberTransformed> listValidST = new HashSet<SubscriberTransformed>();
            HashSet<SubscriberTransformed> listInvalidValidST = new HashSet<SubscriberTransformed>();
            if (listSubscriberOriginal.Count > 0)
            {
                #region DATA ROW Validation
                //int i = 1; // Row indicator skipping header
                ConsoleMessage("Get Transform lists start: " + DateTime.Now.ToString());
                KMPlatform.BusinessLogic.ServiceFeature serviceFeatureWorker = new KMPlatform.BusinessLogic.ServiceFeature();
                KMPlatform.Entity.ServiceFeature serviceFeatureEntity = serviceFeatureWorker.SelectServiceFeature(eventMessage.SourceFile.ServiceFeatureID);

                FrameworkUAS.BusinessLogic.Transformation transformationWorker = new FrameworkUAS.BusinessLogic.Transformation();
                HashSet<Transformation> allTransformations = new HashSet<Transformation>(transformationWorker.Select(eventMessage.Client.ClientID, eventMessage.SourceFile.SourceFileID, true).Where(x => x.IsActive == true).ToList());

                FrameworkUAS.BusinessLogic.TransformSplit transformSplitWorker = new FrameworkUAS.BusinessLogic.TransformSplit();
                HashSet<TransformSplit> allTransformSplit = new HashSet<TransformSplit>(transformSplitWorker.SelectSourceFileID(eventMessage.SourceFile.SourceFileID).Where(x => x.IsActive == true).ToList());

                FrameworkUAS.BusinessLogic.AdHocDimensionGroup ahdgWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
                HashSet<AdHocDimensionGroup> ahdGroups = new HashSet<AdHocDimensionGroup>(ahdgWorker.Select(dataIV.ClientId, false).Where(x => x.IsActive == true).OrderBy(y => y.OrderOfOperation).ToList());

                FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                HashSet<Code> transTypes = new HashSet<Code>(cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Transformation).ToList());
                int splitIntoRowsId = transTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Into_Rows.ToString().Replace("_", " "))).CodeId;
                HashSet<Transformation> splitTrans = new HashSet<Transformation>();

                if (allTransformations.Count(x => x.TransformationTypeID == splitIntoRowsId) > 0)
                    splitTrans = new HashSet<Transformation>(allTransformations.Where(x => x.TransformationTypeID == splitIntoRowsId && x.IsActive == true).ToList());


                HashSet<TransformationFieldMap> splitTranFieldMappings = new HashSet<TransformationFieldMap>();
                if (splitTrans != null)
                {
                    foreach (FrameworkUAS.Entity.Transformation t in splitTrans)
                    {
                        foreach (FrameworkUAS.Entity.TransformationFieldMap tfm in t.FieldMap.Where(x => x.SourceFileID == eventMessage.SourceFile.SourceFileID))
                        {
                            FrameworkUAS.Entity.TransformationFieldMap thisTFM = splitTranFieldMappings.FirstOrDefault(x => x.TransformationFieldMapID == tfm.TransformationFieldMapID);
                            if (thisTFM == null)
                                splitTranFieldMappings.Add(tfm);
                        }
                    }
                }
                ConsoleMessage("Get Transform lists done: " + DateTime.Now.ToString());
                ConsoleMessage("Start: Created Transformed Subscriber " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);

                admsWrk.Update(eventMessage.AdmsLog.ProcessCode,
                                             FrameworkUAD_Lookup.Enums.FileStatusType.Processing,
                                             FrameworkUAD_Lookup.Enums.ADMS_StepType.Validator_Create_Transformed_Subscribers,
                                             FrameworkUAD_Lookup.Enums.ProcessingStatusType.In_CreatingSubscribers,
                                             FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_Validation_Process, 1,
                                             "Start: data validation " + DateTime.Now.TimeOfDay.ToString(), true,
                                             eventMessage.AdmsLog.SourceFileId);


                #region set QSource and EmailStatus
                FrameworkUAS.Entity.FieldMapping fm = eventMessage.SourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals("PubCode", StringComparison.CurrentCultureIgnoreCase));
                FrameworkUAS.Entity.FieldMapping fmQSource = eventMessage.SourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals("PubQSourceID", StringComparison.CurrentCultureIgnoreCase));
                HashSet<FrameworkUAD_Lookup.Entity.Code> allQSources = new HashSet<Code>(cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source).ToList());
                FrameworkUAS.Entity.FieldMapping fmEmailStatus = eventMessage.SourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals("EmailStatusID", StringComparison.CurrentCultureIgnoreCase));
                FrameworkUAS.Entity.FieldMapping fmEmail = eventMessage.SourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals("Email", StringComparison.CurrentCultureIgnoreCase));
                FrameworkUAD.BusinessLogic.EmailStatus emailStatusWorker = new FrameworkUAD.BusinessLogic.EmailStatus();
                HashSet<FrameworkUAD.Entity.EmailStatus> allEmailStatus = new HashSet<EmailStatus>(emailStatusWorker.Select(eventMessage.Client.ClientConnections).ToList());

                #region  only temporary, just trying to get a process time - PubCodeId, QSource, EmailStatus
                //if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) != KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                //{
                //    //only temporary, just trying to get a process time
                //    ConsoleMessage("Start: Set QSource and EmailStatus " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                //    foreach (var key in dataIV.DataTransformed.Keys)
                //    {
                //        StringDictionary myRow = dataIV.DataTransformed[key];
                //        //bool isRowValid = true;
                //        int pubCodeID = -1;
                //        int qSourceID = -1;
                //        int emailStatusID = -1;

                //        #region PubCodeId
                //        if (fm != null && myRow[fm.IncomingField] != null)
                //        {
                //            if (!string.IsNullOrEmpty(myRow[fm.IncomingField].ToString()))
                //            {
                //                string pubCodeValue = myRow[fm.IncomingField].ToString();
                //                int.TryParse(clientPubCodes.SingleOrDefault(x => x.Value.Equals(pubCodeValue.ToUpper())).Key.ToString(), out pubCodeID);
                //            }
                //        }
                //        #endregion
                //        #region QSource
                //        if (fmQSource != null && myRow[fmQSource.IncomingField] != null)
                //        {
                //            if (!string.IsNullOrEmpty(myRow[fmQSource.IncomingField].ToString()))
                //            {
                //                string qSourceValue = myRow[fmQSource.IncomingField].ToString();
                //                if (allQSources.FirstOrDefault(x => x.CodeId.ToString().Equals(qSourceValue, StringComparison.CurrentCultureIgnoreCase)) != null)
                //                    int.TryParse(qSourceValue.ToString(), out qSourceID);
                //                else if (allQSources.FirstOrDefault(x => x.CodeValue.Equals(qSourceValue, StringComparison.CurrentCultureIgnoreCase)) != null)
                //                    int.TryParse(allQSources.FirstOrDefault(x => x.CodeValue.Equals(qSourceValue, StringComparison.CurrentCultureIgnoreCase)).CodeId.ToString(), out qSourceID);
                //                else
                //                    qSourceID = 0;
                //            }
                //        }
                //        #endregion 
                //        #region EmailStatus
                //        string emailValue = "";
                //        if (fmEmail != null && myRow[fmEmail.IncomingField] != null)
                //            emailValue = myRow[fmEmail.IncomingField].ToString();

                //        if (fmEmailStatus != null && myRow[fmEmailStatus.IncomingField] != null)
                //        {
                //            string emailStatusValue = myRow[fmEmailStatus.IncomingField].ToString();
                //            if (!string.IsNullOrEmpty(emailStatusValue))
                //            {
                //                int emailStatusAsInt = -1;
                //                int.TryParse(emailStatusValue, out emailStatusAsInt);

                //                if (allEmailStatus.FirstOrDefault(x => x.EmailStatusID == emailStatusAsInt) != null)
                //                    emailStatusID = emailStatusAsInt;
                //                else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.S.ToString(), StringComparison.CurrentCultureIgnoreCase))
                //                    int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                //                else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.P.ToString(), StringComparison.CurrentCultureIgnoreCase))
                //                    int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Unverified.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                //                else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.U.ToString(), StringComparison.CurrentCultureIgnoreCase))
                //                    int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.UnSubscribe.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                //                else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.M.ToString(), StringComparison.CurrentCultureIgnoreCase))
                //                    int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.MasterSuppressed.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                //                else if (allEmailStatus.FirstOrDefault(x => x.Status.Contains(emailStatusValue)) != null)
                //                    int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Contains(emailStatusValue)).EmailStatusID.ToString(), out emailStatusID);
                //                else
                //                {
                //                    if (!string.IsNullOrEmpty(emailValue))
                //                        int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                //                    else
                //                        emailStatusID = -1;
                //                }
                //            }
                //            else
                //            {
                //                if (!string.IsNullOrEmpty(emailValue))
                //                    int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                //                else
                //                    emailStatusID = -1;
                //            }
                //        }
                //        else
                //        {
                //            if (!string.IsNullOrEmpty(emailValue))
                //                int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                //            else
                //                emailStatusID = -1;
                //        }
                //        #endregion
                //    }
                //    ConsoleMessage("End: Set QSource and EmailStatus " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);

                //}
                #endregion
                #endregion

                #region set DemographicUpdate                
                //FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                List<FrameworkUAD_Lookup.Entity.Code> demoUpdateCodes = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);
                #endregion

                string count = dataIV.DataTransformed.Count.ToString();
                foreach (var key in dataIV.DataTransformed.Keys)
                {
                    StringDictionary myRow = dataIV.DataTransformed[key];
                    //string msg = "Creating Transformed Subscriber: " + key.ToString() + " of " + count;
                    //ConsoleMessage(msg, eventMessage.AdmsLog.ProcessCode, logTranDetail);
                    int originalRowNumber = dataIV.TransformedRowToOriginalRowMap[key];
                    try
                    {
                        int pubCodeID = -1;
                        int qSourceID = -1;
                        int emailStatusID = -1;

                        #region PubCodeId
                        if (fm != null && myRow[fm.IncomingField] != null)
                        {
                            if (!string.IsNullOrEmpty(myRow[fm.IncomingField].ToString()))
                            {
                                string pubCodeValue = myRow[fm.IncomingField].ToString();
                                int.TryParse(clientPubCodes.SingleOrDefault(x => x.Value.Equals(pubCodeValue.ToUpper())).Key.ToString(), out pubCodeID);
                            }
                        }
                        #endregion
                        #region QSource
                        if (fmQSource != null && myRow[fmQSource.IncomingField] != null)
                        {
                            if (!string.IsNullOrEmpty(myRow[fmQSource.IncomingField].ToString()))
                            {
                                string qSourceValue = myRow[fmQSource.IncomingField].ToString();
                                if (allQSources.FirstOrDefault(x => x.CodeId.ToString().Equals(qSourceValue, StringComparison.CurrentCultureIgnoreCase)) != null)
                                    int.TryParse(qSourceValue.ToString(), out qSourceID);
                                else if (allQSources.FirstOrDefault(x => x.CodeValue.Equals(qSourceValue, StringComparison.CurrentCultureIgnoreCase)) != null)
                                    int.TryParse(allQSources.FirstOrDefault(x => x.CodeValue.Equals(qSourceValue, StringComparison.CurrentCultureIgnoreCase)).CodeId.ToString(), out qSourceID);
                                else
                                    qSourceID = 0;
                            }
                        }
                        #endregion 
                        #region EmailStatus
                        string emailValue = "";
                        if (fmEmail != null && myRow[fmEmail.IncomingField] != null)
                            emailValue = myRow[fmEmail.IncomingField].ToString();

                        if (fmEmailStatus != null && myRow[fmEmailStatus.IncomingField] != null)
                        {
                            string emailStatusValue = myRow[fmEmailStatus.IncomingField].ToString();
                            if (!string.IsNullOrEmpty(emailStatusValue))
                            {
                                int emailStatusAsInt = -1;
                                int.TryParse(emailStatusValue, out emailStatusAsInt);

                                if (allEmailStatus.FirstOrDefault(x => x.EmailStatusID == emailStatusAsInt) != null)
                                    emailStatusID = emailStatusAsInt;
                                else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.S.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                    int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                                else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.P.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                    int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Unverified.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                                else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.U.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                    int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.UnSubscribe.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                                else if (emailStatusValue.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatusSingleValue.M.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                    int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.MasterSuppressed.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                                else if (allEmailStatus.FirstOrDefault(x => x.Status.Contains(emailStatusValue)) != null)
                                    int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Contains(emailStatusValue)).EmailStatusID.ToString(), out emailStatusID);
                                else
                                {
                                    if (!string.IsNullOrEmpty(emailValue))
                                        int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                                    else
                                        emailStatusID = -1;
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(emailValue))
                                    int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                                else
                                    emailStatusID = -1;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(emailValue))
                                int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);
                            else
                                emailStatusID = -1;
                        }

                        if (string.IsNullOrEmpty(emailValue) || !Core_AMS.Utilities.StringFunctions.isEmail(emailValue))
                            int.TryParse(allEmailStatus.FirstOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Invalid.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID.ToString(), out emailStatusID);

                        if (eventMessage.SourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals("EMAIL", StringComparison.CurrentCultureIgnoreCase)) == null)
                        {
                            emailStatusID = -1;
                        }

                        #endregion
                        listSubscriberTransformed.Add(CreateTransformedSubscriber(serviceFeatureEntity, allTransformSplit, myRow, originalRowNumber, key, SubOriginalDictionary, ahdGroups, splitTranFieldMappings, splitTrans, qSourceID, emailStatusID, demoUpdateCodes, eventMessage.AdmsLog, eventMessage.SourceFile, dft));
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, client, this.GetType().FullName + ".CreateTransformedSubscriber");
                    }
                }
                //Need all subscribers later for generating numbers for reports.
                allSubscriberTransformed = listSubscriberTransformed;
                //added this dedupe step on 04282017 - JW - same as doing it after pub/profile checks - why check dupe records?
                listSubscriberTransformed = DeDupeTransformed(dataIV, listSubscriberTransformed, eventMessage.AdmsLog.ProcessCode, eventMessage.Client);
                ConsoleMessage("End: Created Transformed Subscriber " + DateTime.Now.TimeOfDay.ToString());

                if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) == KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                {
                    //listValidST.AddRange(listSubscriberTransformed; original
                    listValidST = listSubscriberTransformed;//trying this now
                    //listSubscriberTransformed.ToList().ForEach(x => { listValidST.Add(x); }); tried this after original
                }
                else
                {
                    #region PubCode Check
                    ConsoleMessage("Start: PubCode Check - count: " + listSubscriberTransformed.Count.ToString() + " - " + DateTime.Now.TimeOfDay.ToString());
                    //have no pubCode
                    HashSet<SubscriberTransformed> listNoPubCode = new HashSet<SubscriberTransformed>(listSubscriberTransformed.Where(x => string.IsNullOrEmpty(x.PubCode)).ToList());
                    foreach (var x in listNoPubCode)
                    {
                        StringDictionary myRow = dataIV.DataTransformed[x.ImportRowNumber];
                        string msg = String.Format("Blank PUBCODE @ Row {0}", x.ImportRowNumber);
                        dataIV = ValidationError(dataIV, msg, dft, eventMessage.AdmsLog, x.ImportRowNumber, myRow);
                    };

                    //have a pubCode
                    HashSet<SubscriberTransformed> listPubCodeValid = new HashSet<SubscriberTransformed>(listSubscriberTransformed.Except(listNoPubCode));
                    //listSubscriberTransformed.Where(x => !string.IsNullOrEmpty(x.PubCode)).ToList().ForEach(x => { listPubCodeValid.Add(x); });

                    //not exist PubCode
                    HashSet<SubscriberTransformed> notExistPubCode = new HashSet<SubscriberTransformed>((from t in listSubscriberTransformed
                                                                                                         where !clientPubCodes.Any(x => String.Compare(x.Value, t.PubCode, true) == 0)
                                                                                                         select t).ToList());
                    foreach (var ne in notExistPubCode)
                    {
                        StringDictionary myRow = dataIV.DataTransformed[ne.ImportRowNumber];
                        string msg = String.Format("PUBCODE not found in UAD @ Row {0}", ne.ImportRowNumber);
                        ConsoleMessage(msg);
                        dataIV = ValidationError(dataIV, msg, dft, eventMessage.AdmsLog, ne.ImportRowNumber, myRow);

                        //Remove from listPubCodeValid - This list is used for gathering ValidST later
                        listPubCodeValid.Remove(ne);
                    }

                    ConsoleMessage("End: PubCode Check: " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                    #endregion
                    #region Profile Check
                    //Skip Profile Check if Field Update
                    bool performProfileCheckOnFieldUpdate = true;
                    if (dbft != null && (dft == FrameworkUAD_Lookup.Enums.FileTypes.Field_Update
                        || dft == FrameworkUAD_Lookup.Enums.FileTypes.QuickFill
                        || dft == FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction))
                        performProfileCheckOnFieldUpdate = false;

                    if (performProfileCheckOnFieldUpdate)
                    {
                        ConsoleMessage("Start: Profile Check - count: " + listPubCodeValid.Count.ToString() + " - " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                        HashSet<SubscriberTransformed> listEmailValid = GetValidEmail(listPubCodeValid, eventMessage.SourceFile);
                        //listPubCodeValid.Where(x => !string.IsNullOrEmpty(x.Email) && Core_AMS.Utilities.StringFunctions.isEmail(x.Email)).ToList().ForEach(x => { listEmailValid.Add(x); });

                        HashSet<SubscriberTransformed> remove = new HashSet<SubscriberTransformed>(listEmailValid);
                        listPubCodeValid.RemoveWhere(x => remove.Contains(x));
                        HashSet<SubscriberTransformed> listQualProfileValid = HasQualifiedProfile(listPubCodeValid, dataIV, dft, eventMessage.AdmsLog, eventMessage.SourceFile);

                        //listValidST.AddRange(listEmailValid);
                        listEmailValid.ToList().ForEach(x => { listValidST.Add(x); });
                        //listValidST.AddRange(listQualProfileValid);
                        listQualProfileValid.ToList().ForEach(x => { listValidST.Add(x); });
                        //listValidST = listValidST.Distinct().ToList();
                        ConsoleMessage("Done: Profile Check: " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                    }
                    else
                    {
                        //Field Update skip profile check records may not conform. Not good data will be weeded out in job_FieldUpdate sproc.
                        //listValidST.AddRange(listPubCodeValid.Distinct().ToList());
                        listPubCodeValid.Distinct().ToList().ForEach(x => { listValidST.Add(x); });
                    }
                    #endregion
                    #region Web Forms Blank/Null Codesheet values mark invalid
                    if (dbft != null && (dft == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms
                                || dft == FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Long_Form
                                || dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_2YR
                                || dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_3YR
                                || dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_Other))
                    {
                        ConsoleMessage("Start: Web Forms Required Codesheet Check: " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                        //File Should can contain multiple pubcodes. This is checked before calling the ProcessFileAsObject Method
                        List<string> requiredResponse = new List<string>();
                        //HashSet<SubscriberTransformed> listRequiredCodesheetPubCodeValid = listValidST;
                        //have a pubCode
                        //listRequiredCodesheetPubCodeValid.AddRange(listValidST.Where(x => !string.IsNullOrEmpty(x.PubCode)));

                        if (listValidST.Count > 0)
                        {
                            List<string> distinctPubs = listValidST.Select(x => x.PubCode).Distinct().ToList();

                            //Attempt to Remove bad dimensions that split into row should have split
                            FrameworkUAS.BusinessLogic.TransformSplit tsWorker = new FrameworkUAS.BusinessLogic.TransformSplit();
                            List<FrameworkUAS.Object.TransformSplitInfo> tsList = new List<FrameworkUAS.Object.TransformSplitInfo>();
                            tsList = tsWorker.SelectObject(eventMessage.SourceFile.SourceFileID);

                            foreach (string pub in distinctPubs)
                            {
                                if (clientPubCodes.ContainsValue(pub.ToUpper()))
                                {
                                    int pubID = clientPubCodes.FirstOrDefault(x => x.Value.Equals(pub, StringComparison.CurrentCultureIgnoreCase)).Key;
                                    if (pubID > 0)
                                    {
                                        FrameworkUAD.BusinessLogic.ResponseGroup rgWorker = new FrameworkUAD.BusinessLogic.ResponseGroup();
                                        List<FrameworkUAD.Entity.ResponseGroup> rgList = rgWorker.Select(pubID, eventMessage.Client.ClientConnections);

                                        FrameworkUAD.BusinessLogic.CodeSheet csWorker = new FrameworkUAD.BusinessLogic.CodeSheet();
                                        List<FrameworkUAD.Entity.CodeSheet> csList = csWorker.Select(pubID, eventMessage.Client.ClientConnections);
                                        if (rgList != null && rgList.Count > 0)
                                        {
                                            requiredResponse.AddRange(rgList.Where(x => x.IsRequired == true && x.IsActive == true).Select(x => x.ResponseGroupName.ToLower()));
                                            foreach (FrameworkUAD.Entity.SubscriberTransformed st in listValidST.Where(x => x.PubCode.Equals(pub, StringComparison.CurrentCultureIgnoreCase)))
                                            {
                                                bool isSubTranValid = true;

                                                #region Check for Missing Response in Demo List
                                                List<string> distinctMAFFields = new List<string>();
                                                distinctMAFFields.AddRange(st.DemographicTransformedList.Select(x => x.MAFField.ToLower()).Distinct().ToList());
                                                foreach (string val in requiredResponse)
                                                {
                                                    if (!(distinctMAFFields.Contains(val)))
                                                    {
                                                        isSubTranValid = false;
                                                        StringDictionary myRow = dataIV.DataTransformed[st.ImportRowNumber];
                                                        string msg = String.Format("Missing/Blank Required Codesheet: " + val + " @ Row {0}", st.ImportRowNumber);
                                                        dataIV = ValidationError(dataIV, msg, dft, eventMessage.AdmsLog, st.ImportRowNumber, myRow);
                                                    }
                                                }
                                                #endregion
                                                #region Check Values are Valid
                                                if (isSubTranValid)
                                                {
                                                    foreach (FrameworkUAD.Entity.SubscriberDemographicTransformed sdt in st.DemographicTransformedList)
                                                    {
                                                        // Attempt to remove bad "FAKE" dimensions created from Multiple Split into rows
                                                        bool skip = false;
                                                        if (tsList.Any(x => x.MAFField.Equals(sdt.MAFField, StringComparison.CurrentCultureIgnoreCase)))
                                                        {
                                                            foreach (FrameworkUAS.Object.TransformSplitInfo tsi in tsList.Where(x => x.PubID == pubID && x.MAFField.Equals(sdt.MAFField, StringComparison.CurrentCultureIgnoreCase)))
                                                            {
                                                                var del = CommonEnums.GetDelimiterSymbol(tsi.Delimiter)
                                                                    .GetValueOrDefault(',');
                                                                if (sdt.PubID == pubID &&
                                                                    sdt.MAFField == tsi.MAFField &&
                                                                    sdt.Value.Contains(del))
                                                                {
                                                                    skip = true;
                                                                    break;
                                                                };
                                                            }
                                                        }

                                                        if (skip)
                                                        {
                                                            continue;
                                                        }
                                                        // end rmoving "Fake" dimensions

                                                        string sdtValue = sdt.Value;
                                                        int sdtIntValue = 0;
                                                        int.TryParse(sdtValue, out sdtIntValue);
                                                        if (sdtIntValue > 0 && sdtValue.Length == 1)
                                                            sdtValue = "0" + sdtValue;
                                                        else if (sdtIntValue > 0 && sdtValue.Length == 2 && sdtIntValue.ToString().Length == 1)
                                                            sdtValue = sdtIntValue.ToString();

                                                        //Check Value not missing
                                                        if (requiredResponse.Count(x => x.Equals(sdt.MAFField.ToLower(), StringComparison.CurrentCultureIgnoreCase)) > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(sdtValue.Trim()))
                                                            {
                                                                isSubTranValid = false;
                                                                StringDictionary myRow = dataIV.DataTransformed[st.ImportRowNumber];
                                                                string msg = String.Format("Blank Required Codesheet: " + sdt.MAFField + " value is null/empty @ Row {0}", st.ImportRowNumber);
                                                                dataIV = ValidationError(dataIV, msg, dft, eventMessage.AdmsLog, st.ImportRowNumber, myRow);
                                                            }

                                                            if (isSubTranValid && rgList.FirstOrDefault(x => x.ResponseGroupName.Equals(sdt.MAFField, StringComparison.CurrentCultureIgnoreCase)) != null && csList.FirstOrDefault(x => x.ResponseGroup.Equals(sdt.MAFField, StringComparison.CurrentCultureIgnoreCase) && (x.ResponseValue.Equals(sdt.Value.Trim(), StringComparison.CurrentCultureIgnoreCase) || x.ResponseValue.Equals(sdtValue.Trim(), StringComparison.CurrentCultureIgnoreCase))) == null)
                                                            {
                                                                isSubTranValid = false;
                                                                StringDictionary myRow = dataIV.DataTransformed[st.ImportRowNumber];
                                                                string msg = String.Format("Invalid Required Codesheet Value: " + sdt.MAFField + " value " + sdt.Value + " is invalid @ Row {0}", st.ImportRowNumber);
                                                                dataIV = ValidationError(dataIV, msg, dft, eventMessage.AdmsLog, st.ImportRowNumber, myRow);
                                                            }
                                                        }
                                                        //Change value if incoming is "1" and codesheet is "01" then incoming will be "01" and vice versa
                                                        if (isSubTranValid && sdtIntValue > 0)
                                                        {
                                                            if (csList.Count(x => x.ResponseGroup.Equals(sdt.MAFField, StringComparison.CurrentCultureIgnoreCase) && x.ResponseValue.Equals(sdt.Value.Trim(), StringComparison.CurrentCultureIgnoreCase)) > 0)
                                                                sdt.Value = sdt.Value;
                                                            else if (csList.Count(x => x.ResponseGroup.Equals(sdt.MAFField, StringComparison.CurrentCultureIgnoreCase) && x.ResponseValue.Equals(sdtValue.Trim(), StringComparison.CurrentCultureIgnoreCase)) > 0)
                                                                sdt.Value = sdtValue;
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region Add Invalid
                                                if (isSubTranValid == false)
                                                    listInvalidValidST.Add(st);
                                                #endregion
                                            }
                                            #region Remove Invalid from Valid List
                                            var setToRemove = new HashSet<SubscriberTransformed>(listInvalidValidST);
                                            listValidST.RemoveWhere(x => setToRemove.Contains(x));
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                        ConsoleMessage("End: Web Forms Required Codesheet Check: " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                    }
                    #endregion
                    #region Invalids
                    ConsoleMessage("Start: Find invalids: Total: " + listSubscriberTransformed.Count.ToString() + " Valid: " + listValidST.Count.ToString() + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);

                    HashSet<Guid> hashTran = new HashSet<Guid>(listSubscriberTransformed.Select(x => x.STRecordIdentifier));
                    HashSet<Guid> hashValid = new HashSet<Guid>(listValidST.Select(x => x.STRecordIdentifier));
                    HashSet<Guid> hashInvalid = new HashSet<Guid>(hashTran.Except(hashValid));
                    listSubscriberTransformed.Where(x => hashInvalid.Contains(x.STRecordIdentifier)).ToList().ForEach(x => { listInvalidValidST.Add(x); });

                    ConsoleMessage("End: Find invalids: Invalid: " + listInvalidValidST.Count.ToString() + " - " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                    ConsoleMessage("Start Convert invalids to List: " + listInvalidValidST.Count.ToString() + " - " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                    listSubscriberInvalid = ConvertTransformedToInvalid(listInvalidValidST);
                    ConsoleMessage("End: Convert invalids to List: " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                    #endregion
                }
                #endregion
            }

            #region DeDupe the Transformed and Invalid lists before saving
            HashSet<SubscriberTransformed> dedupedTransList = new HashSet<SubscriberTransformed>();
            HashSet<SubscriberInvalid> dedupedInvalidList = new HashSet<SubscriberInvalid>();

            if (dbft != null && (dft == FrameworkUAD_Lookup.Enums.FileTypes.QuickFill
                                        || dft == FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction))
            {
                dedupedTransList = listValidST;
                dedupedInvalidList = DeDupeInvalid(listSubscriberInvalid, eventMessage.AdmsLog);
            }
            else
            {
                ConsoleMessage("Start: Dedupe Transformed Subscriber - count: " + listValidST.Count.ToString() + " - " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                dedupedTransList = DeDupeTransformed(dataIV, listValidST, eventMessage.AdmsLog.ProcessCode, eventMessage.Client);
                ConsoleMessage("End: Dedupe Transformed Subscriber " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);

                ConsoleMessage("Start: Dedupe Invalid Subscriber - count: " + listSubscriberInvalid.Count.ToString() + " - " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                dedupedInvalidList = DeDupeInvalid(listSubscriberInvalid, eventMessage.AdmsLog);
                ConsoleMessage("End: Dedupe Invalid Subscriber " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
            }

            //save FAILED record counts here INVALID
            int failDemoCount = 0;
            foreach (var s in dedupedInvalidList)
                failDemoCount += s.DemographicInvalidList.Count;
            admsWrk.UpdateFailedCounts(eventMessage.AdmsLog.ProcessCode, dedupedInvalidList.Count, dedupedInvalidList.Count, failDemoCount, 1, true, eventMessage.AdmsLog.SourceFileId);
            eventMessage.AdmsLog.FailedRecordCount += listSubscriberInvalid.Count;
            eventMessage.AdmsLog.FailedProfileCount += listSubscriberInvalid.Count;
            eventMessage.AdmsLog.FailedDemoCount += failDemoCount;
            #endregion

            if (!dft.ToString().Replace("_", " ").Equals("Data Compare") && !dft.ToString().Replace("_", " ").Equals("Field Update"))
            {
                #region Scranton Custom - make this an Execution Point
                if (eventMessage.Client.FtpFolder.Equals("Scranton"))
                {
                    ClientMethods.Scranton scran = new Scranton();
                    dedupedTransList = new HashSet<SubscriberTransformed>(scran.CompanySurvey(dedupedTransList.ToList(), clientPubCodes, eventMessage.Client.ClientID).ToList());
                }
                else if (eventMessage.Client.FtpFolder.Equals("Watt"))
                {
                    ClientMethods.Watt watt = new Watt();
                    dedupedTransList = new HashSet<SubscriberTransformed>(watt.TopCompany(dedupedTransList.ToList(), clientPubCodes, eventMessage.Client.ClientID));
                }
                #endregion
            }

            #region Par3c Convert Value to ID
            ConsoleMessage("Start: EmailStatusID/Par3c/SubSrcID Convert to ID " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
            FrameworkUAD_Lookup.BusinessLogic.Code codeBreaker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            #region Par3c
            List<FrameworkUAD_Lookup.Entity.Code> allPar3c = new List<Code>();
            allPar3c = codeBreaker.Select(FrameworkUAD_Lookup.Enums.CodeType.Par3c);
            #endregion
            foreach (FrameworkUAD.Entity.SubscriberTransformed st in dedupedTransList)
            {
                #region Par3c
                if (allPar3c.FirstOrDefault(x => x.CodeId.ToString().Equals(st.Par3C)) == null)
                {
                    if (string.IsNullOrEmpty(st.Par3C))
                    {
                        st.Par3C = "-1";
                    }
                    else if (allPar3c.FirstOrDefault(x => x.CodeValue.Equals(st.Par3C, StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        int par3cID = 0;
                        int.TryParse(allPar3c.FirstOrDefault(x => x.CodeValue.Equals(st.Par3C, StringComparison.CurrentCultureIgnoreCase)).CodeId.ToString(), out par3cID);
                        st.Par3C = par3cID.ToString();
                    }
                    else if (allPar3c.FirstOrDefault(x => x.DisplayName.Contains(st.Par3C)) != null)
                    {
                        int par3cID = 0;
                        int.TryParse(allPar3c.FirstOrDefault(x => x.DisplayName.Contains(st.Par3C)).CodeId.ToString(), out par3cID);
                        st.Par3C = par3cID.ToString();
                    }
                    else
                        st.Par3C = "-1";
                }
                #endregion                                
            }
            ConsoleMessage("Done: EmailStatusID/Par3c/SubSrcID Convert to ID " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
            #endregion

            #region Grab Fresh Values for Creating Reports
            //Why grab a fresh FieldMapping? Currently in eventMessage FieldMapping variable is missing a few columns (ignore and kmtransformed were removed)
            FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
            List<FrameworkUAS.Entity.FieldMapping> fieldMappings = fmWorker.Select(eventMessage.SourceFile.SourceFileID, false);
            FrameworkUAS.BusinessLogic.TransformSplit newtsWorker = new FrameworkUAS.BusinessLogic.TransformSplit();
            List<FrameworkUAS.Entity.TransformSplit> transformSplits = newtsWorker.SelectSourceFileID(eventMessage.SourceFile.SourceFileID);
            FrameworkUAS.BusinessLogic.TransformationFieldMap tfmWorker = new FrameworkUAS.BusinessLogic.TransformationFieldMap();
            List<FrameworkUAS.Entity.TransformationFieldMap> allTFM = tfmWorker.Select();
            #endregion

            #region Save SubscriberTransformed and SubscriberInvalid
            if (eventMessage.SourceFile.IsDQMReady == true)
            {
                //valid rows will be imported - invalid skipped
                if (dedupedTransList.Count > 0)
                {

                    ConsoleMessage("Start: Transformed Bulk Data Insert " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                    FrameworkUAD.BusinessLogic.SubscriberTransformed st = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
                    bool success = st.SaveBulkSqlInsert(dedupedTransList.ToList(), eventMessage.Client.ClientConnections, isDataCompare);
                    ConsoleMessage("Done: Transformed Bulk Data Insert " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);

                    #region TransformedReport  
                    //moved to seperate thread in CreateDashboardReports()
                    #endregion
                    if (success == false)
                    {
                        #region Error
                        StringBuilder msg = new StringBuilder();
                        msg.AppendLine("ERROR: Insert to Transformed Bulk Data Insert " + DateTime.Now.TimeOfDay.ToString());
                        msg.AppendLine("An unexplained error occurred while inserting records into Transformed or DemoTransformed tables.<br/>");
                        msg.AppendLine("The details of this error have been logged in the FileLog table.<br/>");
                        msg.AppendLine("Please run this query.<br/>");
                        msg.AppendLine("Select * From FileLog With(NoLock) Where SourceFileID = " + eventMessage.SourceFile.SourceFileID.ToString() + " AND ProcessCode = '" + eventMessage.AdmsLog.ProcessCode.ToString() + "' ORDER BY LogDate, LogTime; GO");

                        listSubscriberTransformed.Clear();
                        Emailer.Emailer emWorker = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.EmailError);
                        emWorker.EmailError(msg.ToString(), eventMessage.AdmsLog.ProcessCode, eventMessage.SourceFile.SourceFileID);
                        dataIV = ValidationError(dataIV, msg.ToString(), dft, eventMessage.AdmsLog);
                        #endregion
                    }
                }
                else
                    ConsoleMessage("NO VALID Transformed data to insert: " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);

                if (dedupedInvalidList.Count > 0)
                {
                    ConsoleMessage("Start: Invalid Bulk Data Insert " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                    FrameworkUAD.BusinessLogic.SubscriberInvalid si = new FrameworkUAD.BusinessLogic.SubscriberInvalid();
                    bool success = si.SaveBulkSqlInsert(dedupedInvalidList.ToList(), eventMessage.Client.ClientConnections);
                    ConsoleMessage("Done: Invalid Bulk Data Insert " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                    #region InvalidReport       
                    //moved to seperate thread in CreateDashboardReports()
                    #endregion                    
                    if (success == false)
                    {
                        #region Error
                        StringBuilder msg = new StringBuilder();
                        msg.AppendLine("ERROR: Insert to Invalid Bulk Data Insert " + DateTime.Now.TimeOfDay.ToString());
                        msg.AppendLine("An unexplained error occurred while inserting records into Invalid or DemoInvalid tables.<br/>");
                        msg.AppendLine("The details of this error have been logged in the FileLog table.<br/>");
                        msg.AppendLine("Please run this query.<br/>");
                        msg.AppendLine("Select * From FileLog With(NoLock) Where SourceFileID = " + eventMessage.SourceFile.SourceFileID.ToString() + " AND ProcessCode = '" + eventMessage.AdmsLog.ProcessCode.ToString() + "' ORDER BY LogDate, LogTime; GO");

                        listSubscriberInvalid.Clear();
                        Emailer.Emailer emWorker = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.EmailError);
                        emWorker.EmailError(msg.ToString(), eventMessage.AdmsLog.ProcessCode, eventMessage.SourceFile.SourceFileID);
                        dataIV = ValidationError(dataIV, msg.ToString(), dft, eventMessage.AdmsLog);
                        #endregion
                    }
                }
            }
            #endregion

            //save TRANSFORMED counts here
            //dedupedTransList

            //Get the list of Dupes            
            //This dupesList hashset should not contain any originalImportRow numbers from dedupedTransList. Any originalImportRow number in listSubscriberTransformed 
            //that matches in dedupedTransList are most likely a system duplicate(created during split into row transformation) and these do not need to be reported to the user.
            HashSet<int> origImportRowList = new HashSet<int>(dedupedTransList.Select(x => x.OriginalImportRow).ToList());
            List<int> invalidOriginalRowNumbers = new List<int>();
            List<int> invalidImportRowNumbers = dedupedInvalidList.Select(x => x.ImportRowNumber).ToList();
            //Add invalid original row numbers
            foreach (int irn in invalidImportRowNumbers)
            {
                int originalRowNumber = dataIV.TransformedRowToOriginalRowMap[irn];
                if (invalidOriginalRowNumbers.Count(x => x == originalRowNumber) == 0)
                    origImportRowList.Add(originalRowNumber);

            }
            //This will grab a list of dupes by default if not in transList or invalidList must be a dupe.
            //Need to revise this because if dupe had a split into row then its possible for that dupe to display 2+ times.
            HashSet<SubscriberTransformed> dupesList = new HashSet<SubscriberTransformed>(allSubscriberTransformed.Except(allSubscriberTransformed.Where(x => origImportRowList.Contains(x.OriginalImportRow))));

            int transDemoCount = 0;
            foreach (var s in dedupedTransList)
                transDemoCount += s.DemographicTransformedList.Count;
            admsWrk.UpdateTransformedCounts(eventMessage.AdmsLog.ProcessCode, dedupedTransList.Count, dedupedTransList.Count, transDemoCount, 1, true, eventMessage.AdmsLog.SourceFileId);
            eventMessage.AdmsLog.TransformedRecordCount = dedupedTransList.Count;
            eventMessage.AdmsLog.TransformedProfileCount = dedupedTransList.Count;
            eventMessage.AdmsLog.TransformedDemoCount = transDemoCount;

            //save Duplicate counts here = Original - (transformed + duplicates) = failed
            // listSubscriberTransformed - dedupedTransList
            //already have dup counts updated from SubscriberOriginal so now add the additional dupes from transformations
            int currentDupRecordCount = eventMessage.AdmsLog.DuplicateRecordCount;
            int currentDupProfCount = eventMessage.AdmsLog.DuplicateProfileCount;
            int currentDupDemoCount = eventMessage.AdmsLog.DuplicateDemoCount;

            int dedupeDemos = 0;
            List<int> dedupeOriginalRowNumbers = new List<int>();
            foreach (var d in dupesList)
            {
                if (dedupeOriginalRowNumbers.Count(x => x == d.OriginalImportRow) == 0)
                {
                    dedupeDemos += d.DemographicTransformedList.Count;
                    dedupeOriginalRowNumbers.Add(d.OriginalImportRow);
                }
            }
            int origTransDemoCount = 0;
            foreach (var s in listSubscriberTransformed)
                origTransDemoCount += s.DemographicTransformedList.Count;

            int dupeCount = dupesList.Select(x => x.OriginalImportRow).Distinct().ToList().Count; //listSubscriberTransformed.Count - dedupedTransList.Count;
            int dupeDemoCount = dedupeDemos; //origTransDemoCount - dedupeDemos;
            //int totalDupeRecordCount = currentDupRecordCount + dupeCount;
            //int totalDupeDemoCount = currentDupDemoCount + dupeDemoCount;
            admsWrk.UpdateDuplicateCounts(eventMessage.AdmsLog.ProcessCode, dupeCount, dupeCount, dupeDemoCount, 1, true, eventMessage.AdmsLog.SourceFileId);
            eventMessage.AdmsLog.DuplicateRecordCount += dupeCount;
            eventMessage.AdmsLog.DuplicateProfileCount += dupeCount;
            eventMessage.AdmsLog.DuplicateDemoCount += dupeDemoCount;

            dataIV.ImportedRowCount = dedupedTransList.Count;

            #region DuplicateReport            

            CreateDashboardReports(dataIV, eventMessage, dedupedTransList, dedupedInvalidList, new HashSet<SubscriberTransformed>(dupesList.Distinct().ToList()), fieldMappings, allTFM, transformSplits);


            #endregion

            return dataIV;
        }


        #region Apply Transformations
        private FrameworkUAD.Object.ImportFile TransformImportFileData(FrameworkUAD.Object.ImportFile dataIV, KMPlatform.Entity.ServiceFeature feature, string dbFileType, FrameworkUAS.Entity.SourceFile sourceFile, FrameworkUAS.Entity.AdmsLog admsLog, FrameworkUAD_Lookup.Enums.FileTypes dft)
        {
            try
            {
                #region Transformations
                #region Get all Transformations and break into seperate lists by type
                // Transform data by applying all Assign Value transformations
                FrameworkUAS.BusinessLogic.Transformation tData = new FrameworkUAS.BusinessLogic.Transformation();
                HashSet<Transformation> allTrans = new HashSet<Transformation>(tData.Select(dataIV.ClientId, dataIV.SourceFileId, true).Where(x => x.IsActive == true).ToList());
                foreach (var t in allTrans)
                    t.FieldMap = new HashSet<TransformationFieldMap>(t.FieldMap.Where(x => x.SourceFileID == dataIV.SourceFileId).ToList());

                FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                HashSet<Code> transTypes = new HashSet<Code>(cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Transformation).ToList());
                int dataMappingId = transTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Data_Mapping.ToString().Replace("_", " "))).CodeId;
                int joinColumnsId = transTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Join_Columns.ToString().Replace("_", " "))).CodeId;
                int splitIntoRowsId = transTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Into_Rows.ToString().Replace("_", " "))).CodeId;
                int assignValueId = transTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Assign_Value.ToString().Replace("_", " "))).CodeId;
                int splitTransformId = transTypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Transform.ToString().Replace("_", " "))).CodeId;

                //split into the different types - this can be somewhat hard coded since we are hard coding the methods to execute based on type
                HashSet<Transformation> dataMapTrans = new HashSet<Transformation>(allTrans.Where(x => x.TransformationTypeID == dataMappingId).ToList());
                HashSet<Transformation> joinTrans = new HashSet<Transformation>(allTrans.Where(x => x.TransformationTypeID == joinColumnsId).ToList());
                HashSet<Transformation> splitTrans = new HashSet<Transformation>(allTrans.Where(x => x.TransformationTypeID == splitIntoRowsId).ToList());
                HashSet<Transformation> assignTrans = new HashSet<Transformation>(allTrans.Where(x => x.TransformationTypeID == assignValueId).ToList());
                HashSet<Transformation> transSplit = new HashSet<Transformation>(allTrans.Where(x => x.TransformationTypeID == splitTransformId).ToList());
                #endregion
                #region Get data for each of our transformations
                FrameworkUAS.BusinessLogic.TransformAssign taData = new FrameworkUAS.BusinessLogic.TransformAssign();
                HashSet<TransformAssign> allTransAssign = new HashSet<TransformAssign>(taData.SelectSourceFileID(dataIV.SourceFileId).Where(x => x.IsActive == true).ToList());

                FrameworkUAS.BusinessLogic.TransformDataMap tdmData = new FrameworkUAS.BusinessLogic.TransformDataMap();
                HashSet<TransformDataMap> allTransDataMap = new HashSet<TransformDataMap>(tdmData.SelectSourceFileID(dataIV.SourceFileId).Where(x => x.IsActive == true).ToList());

                FrameworkUAS.BusinessLogic.TransformSplit tsData = new FrameworkUAS.BusinessLogic.TransformSplit();
                HashSet<TransformSplit> allTransSplitMap = new HashSet<TransformSplit>(tsData.SelectSourceFileID(dataIV.SourceFileId).Where(x => x.IsActive == true).ToList());

                FrameworkUAS.BusinessLogic.TransformJoin tjData = new FrameworkUAS.BusinessLogic.TransformJoin();
                HashSet<TransformJoin> allTransJoinMap = new HashSet<TransformJoin>(tjData.SelectSourceFileID(dataIV.SourceFileId).Where(x => x.IsActive == true).ToList());

                FrameworkUAS.BusinessLogic.TransformSplitTrans tstData = new FrameworkUAS.BusinessLogic.TransformSplitTrans();
                HashSet<TransformSplitTrans> allSplitTrans = new HashSet<TransformSplitTrans>(tstData.SelectSourceFileID(dataIV.SourceFileId).Where(x => x.IsActive == true).ToList());

                HashSet<TransformDataMap> everyTransDataMap = new HashSet<TransformDataMap>(tdmData.Select().Where(x => x.IsActive == true).ToList());
                HashSet<TransformSplit> everyTransSplitMap = new HashSet<TransformSplit>(tsData.Select().Where(x => x.IsActive == true).ToList());
                #endregion

                FrameworkUAS.Entity.FieldMapping fmPubCode = sourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals("PubCode", StringComparison.CurrentCultureIgnoreCase));
                if (fmPubCode == null)
                {
                    fmPubCode = new FieldMapping();
                    fmPubCode.IncomingField = string.Empty;
                    fmPubCode.MAFField = string.Empty;
                    fmPubCode.PreviewData = string.Empty;
                    fmPubCode.DataType = string.Empty;
                }
                //get a list of distinct PubCodes - compare see if pubID in dataMapTrans, splitTrans, assignTrans
                Dictionary<int, string> distinctPubCodes = new Dictionary<int, string>();

                ConsoleMessage("Start Transformations", dataIV.ProcessCode, true, dataIV.SourceFileId);

                #region setup Transformation DataTable - Add the columns
                ConsoleMessage("Start setup Transformation DataTable - Add the columns", dataIV.ProcessCode, true, dataIV.SourceFileId);
                StringDictionary multiMapToFieldMap = new StringDictionary();
                HashSet<FieldMultiMap> allMultiMappings = new HashSet<FieldMultiMap>();
                foreach (FieldMapping fm in sourceFile.FieldMappings)
                {
                    if (fm.IsNonFileColumn == true)
                    {
                        string column = string.Empty;
                        if (fm.MAFField.Equals("PubCode", StringComparison.CurrentCultureIgnoreCase))
                            column = fm.MAFField;
                        else
                            column = fm.IncomingField;

                        if (!dataIV.HeadersOriginal.ContainsKey(column))
                        {
                            dataIV.HeadersOriginal.Add(column, (dataIV.HeadersOriginal.Count + 1).ToString());
                            //add to each datarow
                            foreach (var key in dataIV.DataOriginal.Keys)
                            {
                                StringDictionary myRow = dataIV.DataOriginal[key];
                                string pubCode = string.Empty;
                                if (myRow[fmPubCode.IncomingField] != null)
                                {
                                    if (!string.IsNullOrEmpty(myRow[fmPubCode.IncomingField].ToString()) && clientPubCodes.ContainsValue(myRow[fmPubCode.IncomingField].ToString()) == true)
                                        pubCode = clientPubCodes.Single(z => z.Value.Equals(myRow[fmPubCode.IncomingField].ToString())).Value;
                                }
                                else if (myRow[column] != null)
                                {
                                    if (!string.IsNullOrEmpty(myRow[column].ToString()) && clientPubCodes.ContainsValue(myRow[column].ToString()) == true)
                                        pubCode = clientPubCodes.Single(z => z.Value.Equals(myRow[column].ToString())).Value;
                                }

                                if (!myRow.ContainsKey(column))
                                    myRow.Add(column, pubCode);
                            }
                        }
                    }
                }

                //if this is DataCompare we do not care about pubCode
                if (dft != FrameworkUAD_Lookup.Enums.FileTypes.Data_Compare)
                {
                    string pubId = "PubCodeId";
                    if (!dataIV.HeadersOriginal.ContainsKey(pubId))
                    {
                        dataIV.HeadersOriginal.Add(pubId, (dataIV.HeadersOriginal.Count + 1).ToString());
                        //add to each datarow
                        foreach (var key in dataIV.DataOriginal.Keys)
                        {
                            StringDictionary myRow = dataIV.DataOriginal[key];
                            if (myRow[fmPubCode.IncomingField.ToLower()] != null)
                            {
                                int pubCodeId = 0;
                                if (!string.IsNullOrEmpty(myRow[fmPubCode.IncomingField].ToString()) && clientPubCodes.ContainsValue(myRow[fmPubCode.IncomingField].ToString()) == true)
                                {
                                    pubCodeId = clientPubCodes.Single(z => z.Value.Equals(myRow[fmPubCode.IncomingField].ToString())).Key;
                                    KeyValuePair<int, string> pub = new KeyValuePair<int, string>(pubCodeId, myRow[fmPubCode.IncomingField].ToString());
                                    if (!distinctPubCodes.Contains(pub))
                                        distinctPubCodes.Add(pubCodeId, myRow[fmPubCode.IncomingField].ToString());
                                }
                                myRow["PubCodeId"] = pubCodeId.ToString();
                                if (!myRow.ContainsKey(pubId))
                                    myRow.Add(pubId, pubCodeId.ToString());
                            }
                        }
                    }
                }

                //Add to the end of the columns NON FILE COLUMNS (MULTI MAPPINGS)
                foreach (FrameworkUAS.Entity.FieldMapping fm in sourceFile.FieldMappings)
                {
                    //Need to add the multi mappings
                    if (fm.FieldMultiMappings.Count > 0)
                    {
                        foreach (FrameworkUAS.Entity.FieldMultiMap fmm in fm.FieldMultiMappings)
                        {
                            allMultiMappings.Add(fmm);
                            string column = string.Empty;
                            if (fm.MAFField.Equals("PubCode", StringComparison.CurrentCultureIgnoreCase))
                                column = fm.MAFField;
                            else
                                column = fm.IncomingField;

                            if (dataIV.HeadersOriginal.ContainsKey(column) == false)
                            {
                                dataIV.HeadersOriginal.Add(column, (dataIV.HeadersOriginal.Count + 1).ToString());
                                if (!multiMapToFieldMap.ContainsKey(fmm.MAFField))
                                    multiMapToFieldMap.Add(fmm.MAFField, fm.IncomingField);
                                //add to each datarow
                                foreach (var key in dataIV.DataOriginal.Keys)
                                {
                                    StringDictionary myRow = dataIV.DataOriginal[key];
                                    if (!myRow.ContainsKey(column))
                                        myRow.Add(column, string.Empty);
                                }
                            }
                        }
                    }
                    //END ADD
                }

                if (dataIV.HeadersOriginal.ContainsValue("OriginalImportRow") == false)
                    dataIV.HeadersOriginal.Add("OriginalImportRow", (dataIV.HeadersOriginal.Count + 1).ToString());

                int origRowCount = 1;
                foreach (var key in dataIV.DataOriginal.Keys)
                {
                    StringDictionary myRow = dataIV.DataOriginal[key];
                    if (!myRow.ContainsKey("OriginalImportRow"))
                        myRow.Add("OriginalImportRow", origRowCount.ToString());
                    origRowCount++;
                }

                //now we have a transform table with all needed rows to dump data into
                foreach (string column in dataIV.HeadersOriginal.Keys)
                {
                    string incomingCol = string.Empty;
                    if (column.Equals("PubCode", StringComparison.CurrentCultureIgnoreCase))
                    {
                        incomingCol = sourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals(column, StringComparison.CurrentCultureIgnoreCase)).MAFField.ToUpper();
                        if (!string.IsNullOrEmpty(incomingCol) && !dataIV.HeadersTransformed.ContainsKey(incomingCol))
                            dataIV.HeadersTransformed.Add(incomingCol, (dataIV.HeadersTransformed.Count + 1).ToString());
                        else
                        {
                            if (!dataIV.HeadersTransformed.ContainsKey(column))
                                dataIV.HeadersTransformed.Add(column, (dataIV.HeadersTransformed.Count + 1).ToString());
                        }
                    }
                    else if (sourceFile.FieldMappings.Count(x => x.IncomingField.Equals(column, StringComparison.CurrentCultureIgnoreCase)) > 0)
                    {
                        incomingCol = sourceFile.FieldMappings.SingleOrDefault(x => x.IncomingField.Equals(column, StringComparison.CurrentCultureIgnoreCase)).IncomingField.ToUpper();
                        if (!string.IsNullOrEmpty(incomingCol) && !dataIV.HeadersTransformed.ContainsKey(incomingCol))
                            dataIV.HeadersTransformed.Add(incomingCol, (dataIV.HeadersTransformed.Count + 1).ToString());
                        else
                        {
                            if (!dataIV.HeadersTransformed.ContainsKey(column))
                                dataIV.HeadersTransformed.Add(column, (dataIV.HeadersTransformed.Count + 1).ToString());
                        }
                    }
                    else if (allMultiMappings.Count(x => x.MAFField.Equals(column, StringComparison.CurrentCultureIgnoreCase)) > 0)
                    {
                        incomingCol = allMultiMappings.SingleOrDefault(x => x.MAFField.Equals(column, StringComparison.CurrentCultureIgnoreCase)).MAFField.ToUpper();
                        if (!string.IsNullOrEmpty(incomingCol) && !dataIV.HeadersTransformed.ContainsKey(incomingCol))
                            dataIV.HeadersTransformed.Add(incomingCol, (dataIV.HeadersTransformed.Count + 1).ToString());
                        else
                        {
                            if (!dataIV.HeadersTransformed.ContainsKey(column))
                                dataIV.HeadersTransformed.Add(column, (dataIV.HeadersTransformed.Count + 1).ToString());
                        }
                    }
                }
                if (!dataIV.HeadersTransformed.ContainsKey("OriginalImportRow"))
                    dataIV.HeadersTransformed.Add("OriginalImportRow", (dataIV.HeadersTransformed.Count + 1).ToString());
                if (!dataIV.HeadersTransformed.ContainsKey("PubCodeId"))
                    dataIV.HeadersTransformed.Add("PubCodeId", (dataIV.HeadersTransformed.Count + 1).ToString());

                #endregion

                ConsoleMessage("End setup Transformation DataTable - Add the columns", dataIV.ProcessCode, true, dataIV.SourceFileId);

                ConsoleMessage("Start Transformation Step 1", dataIV.ProcessCode, true, dataIV.SourceFileId);

                #region Step 1: TransformAssigns where HasPubId = false - Add rows to Data Table dtTransformed
                //CASE: get ALL Assign List then filter where HasPubID = false

                //check to see what pubcode columns are created
                HashSet<TransformAssign> transAssignList = new HashSet<TransformAssign>();
                foreach (FrameworkUAS.Entity.Transformation t in assignTrans)
                {
                    allTransAssign.Where(x => x.TransformationID == t.TransformationID).ToList().ForEach(x => { transAssignList.Add(x); });
                }

                HashSet<TransformAssign> transAssignNoPubID = new HashSet<TransformAssign>(transAssignList.Where(x => x.HasPubID == false).ToList());
                //try to copy dictionary should be faster
                dataIV.DataTransformed = new Dictionary<int, StringDictionary>(dataIV.DataOriginal);
                //loop for Assigns
                foreach (FrameworkUAS.Entity.TransformAssign ta in transAssignNoPubID)
                {
                    FrameworkUAS.Entity.Transformation tran = allTrans.Single(x => x.TransformationID == ta.TransformationID);
                    foreach (FrameworkUAS.Entity.TransformationFieldMap tfm in tran.FieldMap)
                    {
                        FrameworkUAS.Entity.FieldMapping fm = sourceFile.FieldMappings.SingleOrDefault(x => x.FieldMappingID == tfm.FieldMappingID && tfm.SourceFileID == x.SourceFileID);
                        if (fm != null)
                        {
                            dataIV.DataTransformed.ToList().ForEach(row =>
                            {
                                row.Value[fm.IncomingField] = ta.Value.Trim();
                            });
                        }
                    }
                }

                //now just loop for PubCodeId 
                dataIV.DataTransformed.ToList().ForEach(newRow =>
                {
                    int pubCodeId = 0;
                    if (newRow.Value.ContainsKey(fmPubCode.IncomingField) && clientPubCodes.ContainsValue(newRow.Value[fmPubCode.IncomingField].ToString()) == true)
                    {
                        pubCodeId = clientPubCodes.Single(z => z.Value.Equals(newRow.Value[fmPubCode.IncomingField].ToString())).Key;
                        KeyValuePair<int, string> pub = new KeyValuePair<int, string>(pubCodeId, newRow.Value[fmPubCode.IncomingField].ToString());
                        if (!distinctPubCodes.Contains(pub))
                            distinctPubCodes.Add(pubCodeId, newRow.Value[fmPubCode.IncomingField].ToString());
                    }
                    newRow.Value["PubCodeId"] = pubCodeId.ToString();
                });

                #endregion

                ConsoleMessage("Tran count after Step 1 Transform Assigns: " + dataIV.DataTransformed.Count.ToString(), dataIV.ProcessCode, true, dataIV.SourceFileId);

                //run Split Transform Transformations (Split then DataMap then Split)
                #region Step 2: SplitDataMapSplitTransformation  - Add rows to Data Table dtTransformed
                if (transSplit.Count > 0)
                {
                    HashSet<int> removeKeys = new HashSet<int>();
                    Dictionary<int, StringDictionary> addrows = new Dictionary<int, StringDictionary>();

                    foreach (var key in dataIV.DataTransformed.Keys)
                    {
                        StringDictionary myRow = dataIV.DataTransformed[key];
                        try
                        {
                            foreach (FrameworkUAS.Entity.Transformation t in transSplit)
                            {
                                FrameworkUAS.Entity.TransformSplitTrans tt = allSplitTrans.SingleOrDefault(x => x.TransformationID == t.TransformationID);
                                if (tt != null)
                                {
                                    #region SplitBefore
                                    if (tt.SplitBeforeID != 0)
                                    {
                                        List<string> data = new List<string>();
                                        FrameworkUAS.Entity.TransformSplit ts = everyTransSplitMap.SingleOrDefault(x => x.TransformationID == tt.SplitBeforeID);
                                        if (ts != null)
                                        {
                                            if (myRow.ContainsKey(tt.Column) && !string.IsNullOrEmpty(myRow[tt.Column].ToString()))
                                            {
                                                data = myRow[tt.Column].ToString().Trim()
                                                    .Split(CommonEnums.GetDelimiterSymbol(ts.Delimiter).GetValueOrDefault(','))
                                                    .ToList();
                                            }
                                            if (data.Count > 0)
                                                removeKeys.Add(key);

                                            //this is where the dup records are intially getting created
                                            //should only create a new profile if the split is on or makes PubCode
                                            //want to keep the original record - not put in removeKeys unless making PubCode
                                            //should replace the existing record values with new data

                                            //have to take into account next step of datamap - this is why we currently create the dupes
                                            //datamap  is to a specific value
                                            //should allow dups here - look to eliminate after datamap
                                            foreach (string s in data)
                                            {
                                                var sdNew = new StringDictionary();
                                                foreach (string mk in myRow.Keys)
                                                {
                                                    sdNew.Add(mk, myRow[mk]);
                                                }

                                                sdNew[tt.Column] = s.Trim();
                                                addrows.Add(dataIV.DataTransformed.Count + addrows.Count + 1, sdNew);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException(dataIV, ex, key, myRow, dbFileType, admsLog, dft);
                        }
                    }
                    foreach (var row in addrows)
                    {
                        dataIV.DataTransformed.Add(row.Key, row.Value);
                    }
                    foreach (int key in removeKeys)
                    {
                        dataIV.DataTransformed.Remove(key);
                    }
                }
                
                Dictionary<int, int> keyRest = new Dictionary<int, int>();
                int newKey = 1;
                ValidatorMethods.ResetKeyValuesOnTransformedDictionary(dataIV, out keyRest, out newKey);

                if (transSplit.Count > 0)
                {
                    HashSet<int> removeKeys = new HashSet<int>();
                    Dictionary<int, StringDictionary> addrows = new Dictionary<int, StringDictionary>();
                    foreach (var key in dataIV.DataTransformed.Keys)
                    {
                        StringDictionary myRow = dataIV.DataTransformed[key];
                        try
                        {
                            foreach (FrameworkUAS.Entity.Transformation t in transSplit)
                            {
                                FrameworkUAS.Entity.TransformSplitTrans tt = allSplitTrans.SingleOrDefault(x => x.TransformationID == t.TransformationID);
                                if (tt != null)
                                {
                                    #region DataMap
                                    HashSet<TransformDataMap> tdmList = new HashSet<TransformDataMap>(everyTransDataMap.Where(x => x.TransformationID == tt.DataMapID).ToList());
                                    foreach (TransformDataMap tdm in tdmList)
                                    {
                                        if (myRow.ContainsKey(tt.Column))
                                        {
                                            //now set the value
                                            if (!string.IsNullOrEmpty(myRow[tt.Column].ToString()))
                                            {
                                                string matchType = tdm.MatchType.Replace(" ", "_");
                                                if (matchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Any_Character.ToString()))
                                                {
                                                    if (myRow[tt.Column].ToString().ToLower().Contains(tdm.SourceData.ToLower()))
                                                        myRow[tt.Column] = tdm.DesiredData.Trim();
                                                }
                                                else if (matchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Equals.ToString()))
                                                {
                                                    if (myRow[tt.Column].ToString().Equals(tdm.SourceData.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                                        myRow[tt.Column] = tdm.DesiredData.Trim();
                                                }
                                                else if (matchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Like.ToString()))
                                                {
                                                    if (myRow[tt.Column].ToString().ToLower().Contains(tdm.SourceData.ToLower()))
                                                        myRow[tt.Column] = tdm.DesiredData.Trim();
                                                }
                                                else if (matchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Not_Equals.ToString()))
                                                {
                                                    if (!myRow[tt.Column].ToString().Equals(tdm.SourceData.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                                        myRow[tt.Column] = tdm.DesiredData.Trim();
                                                }
                                                else if (matchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Has_Data.ToString()))
                                                {
                                                    if (!String.IsNullOrEmpty(myRow[tt.Column].ToString()))
                                                        myRow[tt.Column] = tdm.DesiredData.Trim();
                                                }
                                                else if (matchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Is_Null_or_Empty.ToString()))
                                                {
                                                    if (String.IsNullOrEmpty(myRow[tt.Column].ToString()))
                                                        myRow[tt.Column] = tdm.DesiredData.Trim();
                                                }
                                                else if (matchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Find_and_Replace.ToString()))
                                                {
                                                    if (!myRow[tt.Column].ToString().Equals(tdm.SourceData.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                                        myRow[tt.Column] = myRow[tt.Column].ToString().Replace(tdm.SourceData, tdm.DesiredData).Trim();
                                                }
                                                else if (matchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Default.ToString()))
                                                {
                                                    myRow[tt.Column] = tdm.DesiredData.Trim();
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region SplitAfter
                                    if (tt.SplitAfterID != 0)
                                    {
                                        List<string> data = new List<string>();
                                        FrameworkUAS.Entity.TransformSplit ts = everyTransSplitMap.SingleOrDefault(x => x.TransformationID == tt.SplitAfterID);
                                        if (ts != null)
                                        {
                                            if (myRow.ContainsKey(tt.Column) && !string.IsNullOrEmpty(myRow[tt.Column].ToString()))
                                            {
                                                var del = CommonEnums.GetDelimiterSymbol(ts.Delimiter);
                                                if (del.HasValue)
                                                {
                                                    data = myRow[tt.Column]
                                                        .Trim()
                                                        .Split(del.Value)
                                                        .ToList();
                                                }
                                            }
                                        }
                                        if (data.Count > 0)
                                            removeKeys.Add(key);

                                        foreach (string s in data)
                                        {
                                            myRow[tt.Column] = s.Trim();
                                            addrows.Add(dataIV.DataTransformed.Count + addrows.Count + 1, myRow);
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException(dataIV, ex, key, myRow, dbFileType, admsLog, dft);
                        }
                    }
                    foreach (var row in addrows)
                        dataIV.DataTransformed.Add(row.Key, row.Value);
                    foreach (int key in removeKeys)
                        dataIV.DataTransformed.Remove(key);
                }

                ValidatorMethods.ResetKeyValuesOnTransformedDictionary(dataIV, out keyRest, out newKey);
                #endregion

                ConsoleMessage("Tran count after Step 2 Split DataMap Split Transformation: " + dataIV.DataTransformed.Count.ToString(), dataIV.ProcessCode, true, dataIV.SourceFileId);

                #region Step 3: DataMap Transform Where MapsPubCode = True  - Add rows to Data Table dtTransformed
                //Do Data Map where it is assigning the pubId
                //when done remov from dataMapTrans list so it is not done again
                HashSet<Transformation> dmSetPubCodeList = new HashSet<Transformation>(dataMapTrans.Where(x => x.MapsPubCode == true).ToList());
                //now remove from dataMapTrans so wont be used later
                foreach (Transformation t in dmSetPubCodeList)
                    dataMapTrans.Remove(t);
                if (dmSetPubCodeList.Count > 0)
                {
                    foreach (var key in dataIV.DataTransformed.Keys)
                    {
                        StringDictionary myRow = dataIV.DataTransformed[key];
                        try
                        {
                            int pubCodeID = Convert.ToInt32(myRow["PubCodeId"]);
                            //if (pubCodeID == 0)
                            //{
                                #region Transform Data Map
                                //MatchType:  Any_Character, Equals, Not_Equals, Like
                                foreach (Transformation t in dmSetPubCodeList)
                                {
                                    HashSet<TransformDataMap> tdmList = new HashSet<TransformDataMap>(allTransDataMap.Where(x => x.TransformationID == t.TransformationID).ToList());
                                //foreach (TransformDataMap tdmx in tdmList)
                                //{
                                    Transformation tranx = allTrans.SingleOrDefault(x => x.TransformationID == t.TransformationID);
                                    if (tranx != null)
                                    {
                                        HashSet<TransformationFieldMap> TranFieldMap = new HashSet<TransformationFieldMap>(tranx.FieldMap.Where(x => x.SourceFileID == dataIV.SourceFileId).ToList());
                                        if (TranFieldMap.Count > 0)
                                        {
                                            foreach (TransformationFieldMap tfm in tranx.FieldMap)
                                            {
                                                FieldMapping fm = sourceFile.FieldMappings.SingleOrDefault(x => x.FieldMappingID == tfm.FieldMappingID && x.MAFField.Equals("PUBCODE", StringComparison.CurrentCultureIgnoreCase));
                                                //now set the value
                                                if (fm != null) 
                                                {
                                                    if (myRow.ContainsKey(fm.IncomingField))
                                                    {
                                                        if (!string.IsNullOrEmpty(myRow[fm.IncomingField]))
                                                        {
                                                            //Any_Character
                                                            TransformDataMap tdm = tdmList.Where(x => x.MatchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Any_Character.ToString())).ToList().FirstOrDefault(x => myRow[fm.IncomingField].ToString().Trim().ToLower().Contains(x.SourceData.ToLower()));
                                                            if (tdm != null)
                                                                myRow[fm.IncomingField] = tdm.DesiredData;
                                                            //Equals
                                                            tdm = tdmList.Where(x => x.MatchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Equals.ToString())).ToList().FirstOrDefault(x => myRow[fm.IncomingField].ToString().Trim().ToLower().Equals(x.SourceData.ToLower()));
                                                            if (tdm != null)
                                                                myRow[fm.IncomingField] = tdm.DesiredData;
                                                            //like
                                                            tdm = tdmList.Where(x => x.MatchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Like.ToString())).ToList().FirstOrDefault(x => myRow[fm.IncomingField].ToString().Trim().ToLower().Equals(x.SourceData.ToLower()));
                                                            if (tdm != null)
                                                                myRow[fm.IncomingField] = tdm.DesiredData;
                                                            //not equals
                                                            tdm = tdmList.Where(x => x.MatchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Not_Equals.ToString())).ToList().FirstOrDefault(x => !myRow[fm.IncomingField].ToString().Trim().ToLower().Equals(x.SourceData.ToLower()));
                                                            if (tdm != null)
                                                                myRow[fm.IncomingField] = tdm.DesiredData;
                                                            //find and replace
                                                            foreach (TransformDataMap tdmi in tdmList.Where(x => x.MatchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Find_and_Replace.ToString())).ToList())
                                                            {
                                                                if (tdmi != null)
                                                                    myRow[fm.IncomingField] = myRow[fm.IncomingField].ToString().Replace(tdmi.SourceData, tdmi.DesiredData).Trim();
                                                            }

                                                            #region OLD CODE
                                                            //string matchType = tdmx.MatchType.Replace(" ", "_");
                                                            //if (matchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Any_Character.ToString()))
                                                            //{
                                                            //    //if (myRow[fm.IncomingField].ToString().Trim().ToLower().Contains(tdmx.SourceData.ToLower()))
                                                            //    //    myRow[fm.IncomingField] = tdmx.DesiredData;
                                                            //}
                                                            //else if (matchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Equals.ToString()))
                                                            //{
                                                            //    //if (myRow[fm.IncomingField].ToString().Trim().Equals(tdmx.SourceData.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                                            //    //    myRow[fm.IncomingField] = tdmx.DesiredData;
                                                            //}
                                                            //else if (matchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Like.ToString()))
                                                            //{
                                                            //    ////if (myRow[fm.IncomingField].ToString().Trim().ToLower().Contains(tdmx.SourceData.ToLower()))
                                                            //    ////    myRow[fm.IncomingField] = tdmx.DesiredData;
                                                            //}
                                                            //else if (matchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Not_Equals.ToString()))
                                                            //{
                                                            //    //if (!myRow[fm.IncomingField].ToString().Trim().Equals(tdmx.SourceData.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                                            //    //    myRow[fm.IncomingField] = tdmx.DesiredData;
                                                            //}
                                                            //else if (matchType.Equals(FrameworkUAD_Lookup.Enums.MatchTypes.Find_and_Replace.ToString()))
                                                            //{
                                                            //    //if (!myRow[fm.IncomingField].ToString().Trim().Equals(tdmx.SourceData.ToString(), StringComparison.CurrentCultureIgnoreCase))
                                                            //    //    myRow[fm.IncomingField] = myRow[fm.IncomingField].ToString().Replace(tdmx.SourceData, tdmx.DesiredData).Trim();
                                                            //}
                                                            #endregion
                                                        }
                                                        if (!string.IsNullOrEmpty(myRow[fm.IncomingField]))
                                                        {
                                                            //set PubCodeId column
                                                            if (!distinctPubCodes.ContainsValue(myRow[fm.IncomingField].ToString()))
                                                            {
                                                                string test = myRow[fm.IncomingField].ToString();
                                                                int pubCodeId = clientPubCodes.SingleOrDefault(z => z.Value.Equals(myRow[fmPubCode.IncomingField].ToString(), StringComparison.CurrentCultureIgnoreCase)).Key;
                                                                if(pubCodeId != 0) 
                                                                    distinctPubCodes.Add(pubCodeId, myRow[fmPubCode.IncomingField].ToString());
                                                            }
                                                            if (distinctPubCodes.ContainsValue(myRow[fm.IncomingField].ToString()))
                                                            {
                                                                int pubCodeId = distinctPubCodes.Single(x => x.Value.Equals(myRow[fm.IncomingField].ToString())).Key;
                                                                myRow["PubCodeId"] = pubCodeId.ToString();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //}
                                }
                                #endregion
                            //}
                        }
                        catch (Exception ex)
                        {
                            LogException(dataIV, ex, key, myRow, dbFileType, admsLog, dft);
                        }
                    }
                }
                #endregion

                ConsoleMessage("Tran count after Step 3 DataMap Transform where MapsPubCode is true: " + dataIV.DataTransformed.Count.ToString(), dataIV.ProcessCode, true, dataIV.SourceFileId);

                #region Step 4: Transformations by PubCode  - Add rows to Data Table dtTransformed
                KMPlatform.BusinessLogic.ServiceFeature serviceFeatureWorker = new KMPlatform.BusinessLogic.ServiceFeature();
                KMPlatform.Entity.ServiceFeature serviceFeatureEntity = serviceFeatureWorker.SelectServiceFeature(sourceFile.ServiceFeatureID);

                if (dataMapTrans.Count > 0 || assignTrans.Count > 0 || (splitTrans.Count > 0 && KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeatureEntity.SFName) != KMPlatform.BusinessLogic.Enums.UADFeatures.Special_Split))
                {
                    if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) != KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                    {
                        HashSet<int> deleteKeys = new HashSet<int>();
                        Dictionary<int, StringDictionary> addRows = new Dictionary<int, StringDictionary>();
                        foreach (KeyValuePair<int, string> kvp in distinctPubCodes)
                        {
                            HashSet<Transformation> dmList = new HashSet<Transformation>((dataMapTrans.Where(x => x.PubMap.Count(m => m.PubID == kvp.Key || m.PubID == 0) > 0)).ToList());
                            HashSet<Transformation> splitList = new HashSet<Transformation>((splitTrans.Where(t => t.PubMap.Count(x => x.PubID == kvp.Key || x.PubID == 0) > 0)).ToList());
                            HashSet<Transformation> assignList = new HashSet<Transformation>((assignTrans.Where(d => d.PubMap.Count(x => x.PubID == kvp.Key || x.PubID == 0) > 0)).ToList());

                            if (logTranDetail)
                            {
                                ConsoleMessage("dmList count for PubCode: " + kvp.Value + " - " + dmList.Count.ToString(), dataIV.ProcessCode, logTranDetail);
                                ConsoleMessage("splitList count for PubCode: " + kvp.Value + " - " + splitList.Count.ToString(), dataIV.ProcessCode, logTranDetail);
                                ConsoleMessage("assignList count for PubCode: " + kvp.Value + " - " + assignList.Count.ToString(), dataIV.ProcessCode, logTranDetail);
                            }

                            if (dmList.Count > 0 || splitList.Count > 0 || assignList.Count > 0)
                            {
                                HashSet<StringDictionary> sdList = new HashSet<StringDictionary>(dataIV.DataTransformed.Values.Where(x => x[fmPubCode.IncomingField].Equals(kvp.Value, StringComparison.CurrentCultureIgnoreCase)).ToList());
                                int total = 0;
                                int counter = 1;
                                foreach (StringDictionary myRow in sdList)//approx 1-3 hundreths of a second per record
                                {
                                    int key = dataIV.DataTransformed.Single(x => x.Value == myRow).Key;
                                    if (logTranDetail)
                                        ConsoleMessage("PubCode Transformations for " + kvp.Value + " : " + counter.ToString() + " of " + total.ToString(), dataIV.ProcessCode, logTranDetail);
                                    try
                                    {
                                        #region Transform Assign
                                        if (assignList.Count > 0)
                                        {
                                            foreach (Transformation t in assignList)
                                            {
                                                HashSet<TransformAssign> taList = new HashSet<TransformAssign>(allTransAssign.Where(x => x.TransformationID == t.TransformationID).ToList());
                                                foreach (TransformAssign tax in taList)
                                                {
                                                    Transformation tranx = allTrans.FirstOrDefault(x => x.TransformationID == tax.TransformationID);
                                                    if (tranx != null)
                                                    {
                                                        foreach (TransformationFieldMap tfm in tranx.FieldMap.Where(x => x.SourceFileID == dataIV.SourceFileId).ToList())
                                                        {
                                                            FieldMapping fm = sourceFile.FieldMappings.SingleOrDefault(x => x.FieldMappingID == tfm.FieldMappingID);
                                                            if (fm != null)
                                                            {
                                                                if (myRow.ContainsKey(fm.IncomingField))
                                                                {
                                                                    myRow[fm.IncomingField] = tax.Value.Trim();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        #region Transform Data Map
                                        //MatchType:  Any_Character, Equals, Not_Equals, Like
                                        if (dmList.Count > 0)
                                        {
                                            foreach (Transformation t in dmList)
                                            {
                                                if (t.LastStepDataMap == false)
                                                {
                                                    bool dmRan = false;
                                                    HashSet<TransformDataMap> tdmList = new HashSet<TransformDataMap>(allTransDataMap.Where(x => x.TransformationID == t.TransformationID && (x.PubID == kvp.Key || x.PubID == 0)).ToList());
                                                    foreach (TransformDataMap tdmx in tdmList)
                                                    {
                                                        if (!dmRan)
                                                        {
                                                            Transformation tranx = allTrans.FirstOrDefault(x => x.TransformationID == tdmx.TransformationID);
                                                            if (tranx != null)
                                                            {
                                                                HashSet<TransformationFieldMap> TranFieldMap = new HashSet<TransformationFieldMap>(tranx.FieldMap.Where(x => x.SourceFileID == dataIV.SourceFileId).ToList());
                                                                if (TranFieldMap.Count > 0)
                                                                {
                                                                    foreach (TransformationFieldMap tfm in TranFieldMap)
                                                                    {
                                                                        FieldMapping fm = sourceFile.FieldMappings.SingleOrDefault(x => x.FieldMappingID == tfm.FieldMappingID);
                                                                        //now set the value
                                                                        if (fm != null)
                                                                        {
                                                                            if (myRow.ContainsKey(fm.IncomingField))
                                                                            {
                                                                                FrameworkUAD_Lookup.Enums.MatchTypes matchType = FrameworkUAD_Lookup.Enums.GetMatchTypeName(tdmx.MatchType);
                                                                                if (!string.IsNullOrEmpty(myRow[fm.IncomingField]))
                                                                                {
                                                                                    dmRan = SetCurrentRowValues(myRow, tdmx, fm, matchType, dmRan).Value;
                                                                                }
                                                                                else if (matchType == FrameworkUAD_Lookup.Enums.MatchTypes.Is_Null_or_Empty)
                                                                                {
                                                                                    if (String.IsNullOrEmpty(myRow[fm.IncomingField].ToString()))
                                                                                    {
                                                                                        myRow[fm.IncomingField] = tdmx.DesiredData.Trim();
                                                                                        dmRan = true;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        #region Transform Split
                                        //If Service Feature Special Split skip split into row
                                        if (splitList.Count > 0)
                                        {
                                            if (KMPlatform.BusinessLogic.Enums.GetUADFeature(serviceFeatureEntity.SFName) != KMPlatform.BusinessLogic.Enums.UADFeatures.Special_Split)
                                            {
                                                bool performedFirstSplitIntoRow = false;
                                                foreach (Transformation t in splitList)
                                                {
                                                    HashSet<TransformSplit> tsList = new HashSet<TransformSplit>(allTransSplitMap.Where(x => x.TransformationID == t.TransformationID).ToList());
                                                    foreach (TransformSplit tsx in tsList)
                                                    {
                                                        Transformation tranx = allTrans.FirstOrDefault(x => x.TransformationID == tsx.TransformationID);
                                                        if (tranx != null)
                                                        {
                                                            foreach (TransformationFieldMap tfm in tranx.FieldMap.Where(x => x.SourceFileID == dataIV.SourceFileId).ToList())
                                                            {
                                                                FieldMapping fm = sourceFile.FieldMappings.SingleOrDefault(x => x.FieldMappingID == tfm.FieldMappingID);
                                                                if (fm != null)
                                                                {
                                                                    if (performedFirstSplitIntoRow == false)
                                                                    {
                                                                        // First SplitIntoRows Adds TempRows
                                                                        List<string> data = new List<string>();
                                                                        if (myRow.ContainsKey(fm.IncomingField) &&
                                                                            !string.IsNullOrEmpty(myRow[fm.IncomingField]))
                                                                        {
                                                                            var del = CommonEnums.GetDelimiterSymbol(tsx.Delimiter);
                                                                            if (!string.IsNullOrEmpty(myRow[fm.IncomingField]) && del.HasValue)
                                                                            {
                                                                                var token = myRow[fm.IncomingField.ToUpper()].Trim();
                                                                                if (del == ':')
                                                                                {
                                                                                    token = token.Replace("\"", "");
                                                                                }
                                                                                data = token.Split(del.Value).ToList();
                                                                            }
                                                                        }

                                                                        if (data.Count > 0)
                                                                        {
                                                                            deleteKeys.Add(key);
                                                                        }

                                                                        BuildRowFromTokens(dataIV, data, myRow, fm, addRows);
                                                                        performedFirstSplitIntoRow = true;
                                                                        #endregion
                                                                    }
                                                                    else
                                                                    {
                                                                        // Split occurred use previous added rows
                                                                        if (!string.IsNullOrEmpty(myRow[fm.IncomingField]))
                                                                        {
                                                                            var del = CommonEnums.GetDelimiterSymbol(tsx.Delimiter);
                                                                            if (del.HasValue)
                                                                            {
                                                                                var data = myRow[fm.IncomingField.ToUpper()].Trim().Split(del.Value).ToList();
                                                                                BuildRowFromTokens(dataIV, data, myRow, fm, addRows);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    catch (Exception ex)
                                    {
                                        LogException(dataIV, ex, key, myRow, dbFileType, admsLog, dft);
                                    }
                                    counter++;
                                }
                            }
                        }

                        //add rows
                        foreach (var k in addRows)
                            dataIV.DataTransformed.Add(k.Key, k.Value);
                        //delete keys
                        foreach (int k in deleteKeys)
                            dataIV.DataTransformed.Remove(k);

                        ValidatorMethods.ResetKeyValuesOnTransformedDictionary(dataIV, out keyRest, out newKey);

                        //clearing lists after their last use
                        splitTrans.Clear();
                        splitTrans.TrimExcess();

                        assignTrans.Clear();
                        assignTrans.TrimExcess();

                        transSplit.Clear();
                        transSplit.TrimExcess();
                    }
                }
                #endregion

                ConsoleMessage("Tran count after Step 4 Transformations by PubCode: " + dataIV.DataTransformed.Count.ToString(), dataIV.ProcessCode, true, dataIV.SourceFileId);

                #region Step 5: Transform Join  - Add rows to Data Table dtTransformed
                if (joinTrans.Count > 0)
                {
                    if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) != KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                    {
                        foreach (var key in dataIV.DataTransformed.Keys)
                        {
                            StringDictionary myRow = dataIV.DataTransformed[key];
                            try
                            {
                                int pubCodeID = Convert.ToInt32(myRow["PubCodeId"]);
                                if (pubCodeID > 0)
                                {
                                    HashSet<Transformation> joinList = new HashSet<Transformation>(joinTrans.Where(d => d.PubMap.Count(x => (x.PubID == pubCodeID || x.PubID == 0)) > 0).ToList());
                                    foreach (Transformation t in joinList)
                                    {
                                        HashSet<TransformJoin> jList = new HashSet<TransformJoin>(allTransJoinMap.Where(x => x.TransformationID == t.TransformationID).ToList());
                                        foreach (TransformJoin tjx in jList)
                                        {
                                            Transformation tranx = allTrans.FirstOrDefault(x => x.TransformationID == tjx.TransformationID);
                                            if (tranx != null)
                                            {
                                                foreach (TransformationFieldMap tfm in tranx.FieldMap.Where(y => y.SourceFileID == dataIV.SourceFileId).ToList())
                                                {
                                                    FieldMapping fm = sourceFile.FieldMappings.SingleOrDefault(x => x.FieldMappingID == tfm.FieldMappingID && x.SourceFileID == dataIV.SourceFileId);
                                                    char splitter = Convert.ToChar(tjx.Delimiter);
                                                    HashSet<string> columns = new HashSet<string>(tjx.ColumnsToJoin.ToString().Split(splitter).ToList());
                                                    StringBuilder newValue = new StringBuilder();
                                                    foreach (string s in columns)
                                                    {
                                                        string col = s.Trim();
                                                        FieldMapping colConversion = sourceFile.FieldMappings.SingleOrDefault(x => x.SourceFileID == dataIV.SourceFileId && x.IncomingField.Equals(col, StringComparison.CurrentCultureIgnoreCase));
                                                        if (colConversion != null)
                                                        {
                                                            if (myRow.ContainsKey(colConversion.IncomingField) && !string.IsNullOrEmpty(myRow[colConversion.IncomingField]))
                                                                newValue.Append(myRow[colConversion.IncomingField].ToString().Trim() + splitter);
                                                        }
                                                    }
                                                    if (fm != null)
                                                    {
                                                        if (myRow.ContainsKey(fm.IncomingField))
                                                        {
                                                            myRow[fm.IncomingField] = newValue.ToString().TrimEnd(splitter).Trim();
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                LogException(dataIV, ex, key, myRow, dbFileType, admsLog, dft);
                            }
                        }
                        //Clearing lists after their last use
                        joinTrans.Clear();
                        joinTrans.TrimExcess();
                    }
                }
                #endregion

                ConsoleMessage("Tran count after Step 5 Transform Join: " + dataIV.DataTransformed.Count.ToString(), dataIV.ProcessCode, true, dataIV.SourceFileId);

                #region Step 6: Datamap = true
                if (dataMapTrans.Count > 0)
                {
                    if (KMPlatform.BusinessLogic.Enums.GetUADFeature(feature.SFName) != KMPlatform.BusinessLogic.Enums.UADFeatures.Data_Compare)
                    {
                        foreach (var key in dataIV.DataTransformed.Keys)
                        {
                            StringDictionary myRow = dataIV.DataTransformed[key];
                            try
                            {
                                int pubCodeID = Convert.ToInt32(myRow["PubCodeId"]);
                                if (pubCodeID > 0)
                                {
                                    HashSet<Transformation> dmList = new HashSet<Transformation>(dataMapTrans.Where(d => d.PubMap.Count(x => (x.PubID == pubCodeID || x.PubID == 0)) > 0).ToList());
                                    foreach (Transformation t in dmList)
                                    {
                                        if (t.LastStepDataMap == true)
                                        {
                                            HashSet<TransformDataMap> tdmList = new HashSet<TransformDataMap>(allTransDataMap.Where(x => x.TransformationID == t.TransformationID && (x.PubID == pubCodeID || x.PubID == 0)).ToList());
                                            foreach (TransformDataMap tdmx in tdmList)
                                            {
                                                Transformation tranx = allTrans.FirstOrDefault(x => x.TransformationID == tdmx.TransformationID);
                                                if (tranx != null)
                                                {
                                                    HashSet<TransformationFieldMap> TranFieldMap = new HashSet<TransformationFieldMap>(tranx.FieldMap.Where(x => x.SourceFileID == dataIV.SourceFileId).ToList());
                                                    if (TranFieldMap.Count > 0)
                                                    {
                                                        foreach (TransformationFieldMap tfm in TranFieldMap)
                                                        {
                                                            FieldMapping fm = sourceFile.FieldMappings.SingleOrDefault(x => x.FieldMappingID == tfm.FieldMappingID);
                                                            //now set the value
                                                            if (fm != null)
                                                            {
                                                                if (myRow.ContainsKey(fm.IncomingField))
                                                                {
                                                                    var matchType = FrameworkUAD_Lookup.Enums.GetMatchTypeName(tdmx.MatchType);
                                                                    if (!string.IsNullOrEmpty(myRow[fm.IncomingField]))
                                                                    {
                                                                        SetCurrentRowValues(myRow, tdmx, fm, matchType, null);
                                                                    }
                                                                    else if (matchType == FrameworkUAD_Lookup.Enums.MatchTypes.Is_Null_or_Empty)
                                                                    {
                                                                        if (String.IsNullOrEmpty(myRow[fm.IncomingField].ToString()))
                                                                        {
                                                                            myRow[fm.IncomingField] = tdmx.DesiredData.Trim();
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogException(dataIV, ex, key, myRow, dbFileType, admsLog, dft);
                            }
                        }
                        //Clearing lists after their last use
                        dataMapTrans.Clear();
                        dataMapTrans.TrimExcess();
                    }
                }
                #endregion

                ConsoleMessage("Tran count after Step 6 DataMap is true: " + dataIV.DataTransformed.Count.ToString(), dataIV.ProcessCode, true, dataIV.SourceFileId);
                #endregion

                #region Step 7: Multi-Mappings assign FieldMapping
                if (multiMapToFieldMap.Count > 0)
                {
                    foreach (var key in dataIV.DataTransformed.Keys)
                    {
                        StringDictionary myRow = dataIV.DataTransformed[key];
                        foreach (DictionaryEntry kvp in multiMapToFieldMap)
                        {
                            if (myRow.ContainsKey(kvp.Key.ToString()))
                                myRow[kvp.Key.ToString()] = myRow[kvp.Value.ToString()];
                        }
                    }
                }
                #endregion

                ConsoleMessage("Tran count after Step 7 Multi-Mappings assign FieldMapping: " + dataIV.DataTransformed.Count.ToString(), dataIV.ProcessCode, true, dataIV.SourceFileId);

                ConsoleMessage("End Transformations", dataIV.ProcessCode, true, dataIV.SourceFileId);

                //this step is taking 2 min 37 seconds - made some edits 9/23/16 10:38 am
                foreach (var key in dataIV.DataTransformed.Keys)
                {
                    StringDictionary myRow = dataIV.DataTransformed[key];
                    int orig = 0;
                    int.TryParse(myRow["OriginalImportRow"].ToString(), out orig);
                    dataIV.TransformedRowToOriginalRowMap.Add(key, orig);

                    if (myRow["PubCodeId"].ToString() == "0")
                    {
                        if (clientPubCodes.ContainsValue(myRow["PubCode"]))
                            myRow["PubCodeId"] = clientPubCodes.Single(x => x.Value.Equals(myRow["PubCode"])).Key.ToString();
                    }
                    //make sure pubcodeId is set in Original
                    StringDictionary origRow = dataIV.DataOriginal[orig];
                    if (origRow["PubCodeId"] != null)
                        origRow["PubCodeId"] = myRow["PubCodeId"];
                    else
                        origRow.Add("PubCodeId", myRow["PubCodeId"]);
                }

                dataIV.TransformedRowCount = dataIV.DataTransformed.Count;
                ConsoleMessage("Tran count final: " + dataIV.DataTransformed.Count.ToString(), dataIV.ProcessCode, true, dataIV.SourceFileId);
            }
            catch (Exception ex)
            {
                LogError(ex, client, "ADMS.Services.Validator.Validator.TransformData - Unhandled Exception");
            }

            return dataIV;
        }

        private bool? SetCurrentRowValues(
            StringDictionary row, 
            TransformDataMap dataMap, 
            FieldMapping fieldMap, 
            LookupEnums.MatchTypes matchType, 
            bool? dmRan)
        {
            Guard.NotNull(row, nameof(row));
            Guard.NotNull(dataMap, nameof(dataMap));
            Guard.NotNull(fieldMap, nameof(fieldMap));

            switch (matchType)
            {
                case LookupEnums.MatchTypes.Any_Character:
                    if (row[fieldMap.IncomingField].ToString().Trim().IndexOf(dataMap.SourceData, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        row[fieldMap.IncomingField] = dataMap.DesiredData.Trim();
                        dmRan = true;
                    }
                    break;
                case LookupEnums.MatchTypes.Equals:
                    if (row[fieldMap.IncomingField].ToString().Trim().Equals(dataMap.SourceData.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        row[fieldMap.IncomingField] = dataMap.DesiredData.Trim();
                        dmRan = true;
                    }
                    break;
                case LookupEnums.MatchTypes.Like:
                    if (row[fieldMap.IncomingField].ToString().Trim().IndexOf(dataMap.SourceData, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        row[fieldMap.IncomingField] =  dataMap.DesiredData.Trim();
                        dmRan = true;
                    }
                    break;
                case LookupEnums.MatchTypes.Not_Equals:
                    if (!row[fieldMap.IncomingField].ToString().Trim().Equals(dataMap.SourceData.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        row[fieldMap.IncomingField] = dataMap.DesiredData.Trim();
                        dmRan = true;
                    }
                    break;
                case LookupEnums.MatchTypes.Has_Data:
                    if (!string.IsNullOrWhiteSpace(row[fieldMap.IncomingField].ToString()))
                    {
                        row[fieldMap.IncomingField] = dataMap.DesiredData.Trim();
                        dmRan = true;
                    }
                    break;
                case LookupEnums.MatchTypes.Is_Null_or_Empty:
                    if (string.IsNullOrWhiteSpace(row[fieldMap.IncomingField].ToString()))
                    {
                        row[fieldMap.IncomingField] = dataMap.DesiredData.Trim();
                        dmRan = true;
                    }
                    break;
                case LookupEnums.MatchTypes.Find_and_Replace:
                    if (!row[fieldMap.IncomingField].ToString().Trim().Equals(dataMap.SourceData.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        row[fieldMap.IncomingField] = row[fieldMap.IncomingField].ToString().Replace(dataMap.SourceData, dataMap.DesiredData).Trim();
                        dmRan = true;
                    }
                    break;
                case LookupEnums.MatchTypes.Default:
                {
                    row[fieldMap.IncomingField] = dataMap.DesiredData.Trim();
                    dmRan = true;
                    break;
                }
            }
            return dmRan;
        }

        private static void BuildRowFromTokens(
            ImportFile dataIV, 
            List<string> data, 
            StringDictionary myRow, 
            FieldMapping fm,
            Dictionary<int, StringDictionary> addRows)
        {
            foreach (string s in data)
            {
                var sdNew = new StringDictionary();
                foreach (string mk in myRow.Keys)
                {
                    sdNew.Add(mk, myRow[mk]);
                }

                sdNew[fm.IncomingField] = s.Trim();
                addRows.Add(dataIV.DataTransformed.Count + addRows.Count + 1, sdNew);
            }
        }

        #region AdHocDimensions
        private StringDictionary SetAdHoc(FrameworkUAS.Entity.AdHocDimensionGroup adg, StringDictionary dr, List<FrameworkUAS.Entity.AdHocDimension> adList, int sourceFileId, List<FrameworkUAS.Entity.FieldMapping> fmList)
        {
            //FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
            //List<FrameworkUAS.Entity.FieldMapping> fmList = fmWorker.Select(sourceFileId);
            string fieldName = null;
            FrameworkUAS.Entity.FieldMapping mappedField = fmList.FirstOrDefault(x => x.MAFField.ToLower() == adg.StandardField.ToLower());
            if (mappedField != null)
                fieldName = mappedField.IncomingField;

            string sourceString = dr[fieldName].ToString();
            int sourceInt = 0;
            int.TryParse(sourceString, out sourceInt);

            string generatedValue = string.Empty;
            string cd = adg.CreatedDimension.ToUpper();
            if (String.IsNullOrEmpty(dr[cd].ToString()))
                generatedValue = adg.DefaultValue.ToString();

            generatedValue = ValidatorMethods.SetGeneratedValue(adList, sourceInt, sourceString, generatedValue, true);

            if (generatedValue == null)
                generatedValue = string.Empty;

            if (String.IsNullOrEmpty(generatedValue))
                generatedValue = dr[cd].ToString();

            dr[cd] = generatedValue;
            return dr;
        }
        private FrameworkUAD.Object.ImportFile ApplyAdHocDimensions(FrameworkUAD.Object.ImportFile dataIV)
        {
            //first check if client has any AHD's
            FrameworkUAS.BusinessLogic.AdHocDimensionGroup ahdgWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
            List<FrameworkUAS.Entity.AdHocDimensionGroup> ahdGroups = ahdgWorker.Select(dataIV.ClientId, true).Where(x => x.IsActive == true).OrderBy(y => y.OrderOfOperation).ToList();
            FrameworkUAS.BusinessLogic.AdHocDimension adWorker = new FrameworkUAS.BusinessLogic.AdHocDimension();

            if (ahdGroups.Count > 0)
            {
                //Grab FieldMapping Columns
                FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
                List<FrameworkUAS.Entity.FieldMapping> fmList = fmWorker.Select(dataIV.SourceFileId);

                //Grab codesheet values for the dimension group for below. Only want to run on invalid values.
                FrameworkUAD.BusinessLogic.CodeSheet blcs = new FrameworkUAD.BusinessLogic.CodeSheet();
                KMPlatform.BusinessLogic.Client blc = new KMPlatform.BusinessLogic.Client();
                KMPlatform.Entity.Client client = blc.Select(dataIV.ClientId);
                List<FrameworkUAD.Entity.CodeSheet> codeSheet = blcs.Select(client.ClientConnections);
                Dictionary<int, string> clientPubCodes = FrameworkUAS.Object.DBWorker.GetPubIDAndCodesByClient(client);

                foreach (FrameworkUAS.Entity.AdHocDimensionGroup adg in ahdGroups)
                {
                    //so now create the dimension column OR set the column value
                    //first check if column exists
                    string fieldName = null;
                    FrameworkUAS.Entity.FieldMapping mappedField = fmList.FirstOrDefault(x => x.MAFField.ToLower() == adg.StandardField.ToLower());
                    if (mappedField != null)
                        fieldName = mappedField.IncomingField;

                    if (fieldName != null && dataIV.HeadersTransformed.ContainsKey(fieldName))
                    {
                        List<FrameworkUAS.Entity.AdHocDimension> adList = adWorker.Select(adg.AdHocDimensionGroupId);
                        //our compare column is there now check for CreatedDimension column if not there create

                        //do a foreach loop instead of contains.
                        bool colExist = false;
                        if (dataIV.HeadersTransformed.ContainsKey(fieldName))
                            colExist = true;

                        if (colExist == false)
                        {
                            dataIV.HeadersTransformed.Add(adg.CreatedDimension, (dataIV.HeadersTransformed.Count + 1).ToString());
                            //add new column to each row with a blank value
                            foreach (var key in dataIV.DataTransformed.Keys)
                            {
                                StringDictionary myRow = dataIV.DataTransformed[key];
                                myRow.Add(adg.CreatedDimension.ToUpper(), string.Empty);
                            }
                        }
                        else
                        {
                            foreach (var key in dataIV.DataTransformed.Keys)
                            {
                                StringDictionary myRow = dataIV.DataTransformed[key];
                                if (!myRow.ContainsKey(adg.CreatedDimension.ToUpper()))
                                    myRow.Add(adg.CreatedDimension.ToUpper(), string.Empty);
                            }
                        }
                        if (adg.IsPubcodeSpecific == true)
                        {
                            foreach (var key in dataIV.DataTransformed.Keys)
                            {
                                StringDictionary myRow = dataIV.DataTransformed[key];
                                string pubCode = string.Empty;
                                if (myRow.ContainsKey("PubCode"))
                                    pubCode = myRow["PubCode"].ToString();
                                //else if (myRow.ContainsKey("PUBCODE"))
                                //    pubCode = myRow["PUBCODE"].ToString();
                                //else if (myRow.ContainsKey("pubcode"))
                                //    pubCode = myRow["pubcode"].ToString();

                                if (adg.DimensionGroupPubcodeMappings.Exists(x => x.Pubcode.Equals(pubCode, StringComparison.CurrentCultureIgnoreCase) && x.IsActive == true))
                                {
                                    //Get the Pubcode ID and codesheet values for the pubID and ResponseGroup = CreatedDimension
                                    //Check the codesheet values where current value = null meaning value not found/invalid and run the SetAdHoc
                                    var pubIDKey = clientPubCodes.FirstOrDefault(x => x.Value.Equals(pubCode, StringComparison.CurrentCultureIgnoreCase)).Key;
                                    int pubID = 0;
                                    int.TryParse(pubIDKey.ToString(), out pubID);
                                    List<FrameworkUAD.Entity.CodeSheet> pubCodeSheet = codeSheet.Where(x => x.PubID.Equals(pubID) && x.ResponseGroup.Equals(adg.CreatedDimension, StringComparison.CurrentCultureIgnoreCase)).ToList();
                                    //if (string.IsNullOrEmpty(dr[adg.CreatedDimension].ToString()) || dr[adg.CreatedDimension].ToString().Equals(adg.DefaultValue))
                                    int i = 0;
                                    string cd = adg.CreatedDimension.ToUpper();
                                    if (pubCodeSheet != null && pubCodeSheet.Count > 0)
                                    {
                                        if (int.TryParse(myRow[cd].ToString(), out i))
                                        {
                                            if (pubCodeSheet.FirstOrDefault(x => x.ResponseValue.Equals(myRow[cd].ToString().TrimStart('0'), StringComparison.CurrentCultureIgnoreCase)) == null)
                                            {
                                                SetAdHoc(adg, myRow, adList, dataIV.SourceFileId, fmList);
                                            }
                                        }
                                        else
                                        {
                                            if (pubCodeSheet.FirstOrDefault(x => x.ResponseValue.Equals(myRow[adg.CreatedDimension].ToString(), StringComparison.CurrentCultureIgnoreCase)) == null)
                                            {
                                                SetAdHoc(adg, myRow, adList, dataIV.SourceFileId, fmList);

                                                //now can call any client custom methods if needed

                                            }
                                        }
                                    }
                                    else
                                        SetAdHoc(adg, myRow, adList, dataIV.SourceFileId, fmList);
                                }
                            }
                        }
                        else
                        {
                            foreach (var key in dataIV.DataTransformed.Keys)
                            {
                                StringDictionary myRow = dataIV.DataTransformed[key];
                                //if(string.IsNullOrEmpty(dr[adg.CreatedDimension].ToString()) || dr[adg.CreatedDimension].ToString().Equals(adg.DefaultValue))
                                SetAdHoc(adg, myRow, adList, dataIV.SourceFileId, fmList);

                                //now can call any client custom methods if needed
                            }

                        }
                    }
                }
            }
            return dataIV;
        }
        #endregion
        #region Subscriber table Inserts - Original - Invalid - Transformed
        private HashSet<SubscriberOriginal> InsertOriginalSubscribers(FrameworkUAD.Object.ImportFile dataIV, FileMoved eventMessage, KMPlatform.Entity.ServiceFeature feature, FrameworkUAD_Lookup.Enums.FileTypes dft)
        {
            //a HashSet will automatically be DeDuped
            HashSet<FrameworkUAD.Entity.SubscriberOriginal> listSubscriberOriginal = new HashSet<SubscriberOriginal>();
            HashSet<FrameworkUAD.Entity.SubscriberOriginal> excluded = new HashSet<SubscriberOriginal>();
            admsWrk.Update(eventMessage.AdmsLog.ProcessCode,
                                             FrameworkUAD_Lookup.Enums.FileStatusType.Processing,
                                             FrameworkUAD_Lookup.Enums.ADMS_StepType.Validator_Create_Original_Subscribers,
                                             FrameworkUAD_Lookup.Enums.ProcessingStatusType.In_CreatingSubscribers,
                                             FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_Validation_Process, 1,
                                             "Start: Validator_Create_Original_Subscribers " + DateTime.Now.TimeOfDay.ToString(), true,
                                             eventMessage.AdmsLog.SourceFileId);

            FrameworkUAS.BusinessLogic.AdHocDimensionGroup ahdgWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
            List<FrameworkUAS.Entity.AdHocDimensionGroup> tmpAhdGroups = ahdgWorker.Select(dataIV.ClientId, false).Where(x => x.IsActive == true).OrderBy(y => y.OrderOfOperation).ToList();
            HashSet<FrameworkUAS.Entity.AdHocDimensionGroup> ahdGroups = new HashSet<AdHocDimensionGroup>(tmpAhdGroups);
            //tmpAhdGroups.ForEach(x => ahdGroups.Add(x));

            if (dataIV.HeadersOriginal.ContainsKey("OriginalImportRow") == false)
                dataIV.HeadersOriginal.Add("OriginalImportRow", (dataIV.HeadersOriginal.Count + 1).ToString());

            int origRowCount = 1;
            foreach (var key in dataIV.DataOriginal.Keys)
            {
                StringDictionary myRow = dataIV.DataOriginal[key];
                if (!myRow.ContainsKey("OriginalImportRow"))
                    myRow.Add("OriginalImportRow", origRowCount.ToString());
                origRowCount++;
            }

            FrameworkUAS.Entity.FieldMapping fm = eventMessage.SourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals("PubCode", StringComparison.CurrentCultureIgnoreCase));
            int total = dataIV.DataOriginal.Keys.Count;
            //int r = 1;
            foreach (var key in dataIV.DataOriginal.Keys)
            {
                //ConsoleMessage(r.ToString() + " of " + total.ToString());
                //r++;
                StringDictionary myRow = dataIV.DataOriginal[key];
                //lets make 2 lists here with a try/catch - my guess is that if duplicate item it will throw an error on the add
                //try putting those in the dupe list
                FrameworkUAD.Entity.SubscriberOriginal createSO = (CreateSubscriber(myRow, ahdGroups, eventMessage.AdmsLog, eventMessage.SourceFile, dft));
                try { listSubscriberOriginal.Add(createSO); }
                catch
                {
                    ConsoleMessage("SO Add dupe profile via Hash equal overrride SORecordIdentifier: " + createSO.SORecordIdentifier.ToString());
                    excluded.Add(createSO);
                }
            }
            //new dup object check
            int dupObjectCount = 0;
            foreach (var d in listSubscriberOriginal)
                dupObjectCount += d.DuplicateProfiles.Count;

            //these are actually already deduped counts
            int totalDemos = 0;
            foreach (var d in listSubscriberOriginal)
                totalDemos += d.DemographicOriginalList.Count;
            admsWrk.UpdateOriginalCounts(eventMessage.AdmsLog.ProcessCode, dataIV.OriginalRowCount, dataIV.OriginalRowCount, totalDemos, 1, true, eventMessage.AdmsLog.SourceFileId);
            eventMessage.AdmsLog.OriginalRecordCount = dataIV.OriginalRowCount;
            eventMessage.AdmsLog.OriginalProfileCount = dataIV.OriginalRowCount;
            eventMessage.AdmsLog.OriginalDemoCount = totalDemos;
            ConsoleMessage("AdmsLog now has Original counts ");
            #region dedupe profiles
            admsWrk.Update(eventMessage.AdmsLog.ProcessCode,
                                             FrameworkUAD_Lookup.Enums.FileStatusType.Processing,
                                             FrameworkUAD_Lookup.Enums.ADMS_StepType.Validator_Dedupe_Subscribers,
                                             FrameworkUAD_Lookup.Enums.ProcessingStatusType.In_DedupeSubscribers,
                                             FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_Validation_Process, 1,
                                             "Start: Check dupe demos " + DateTime.Now.TimeOfDay.ToString(), true,
                                             eventMessage.AdmsLog.SourceFileId);
            //should not be any dupes cause our Equals override will prevent it
            ConsoleMessage("SO List count : " + listSubscriberOriginal.Count.ToString());
            ConsoleMessage("SO Exclude count : " + excluded.Count.ToString());
            ConsoleMessage("SO Dup object check count: " + dupObjectCount.ToString());
            int totalExcludeCount = excluded.Count + dupObjectCount;

            //now due to transformations we may have duplicate profiles
            //can only do this after creating Subscriber objects - before insert
            //HashSet<FrameworkUAD.Entity.SubscriberOriginal> dedupedSubscriberOriginal = new HashSet<FrameworkUAD.Entity.SubscriberOriginal>();
            //List<FrameworkUAD.Entity.SubscriberOriginal> tmpDedupedSubscriberOriginal = new List<FrameworkUAD.Entity.SubscriberOriginal>();
            //tmpDedupedSubscriberOriginal = listSubscriberOriginal.GroupBy(item => new { item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone }).Select(group => group.First()).ToList();
            //tmpDedupedSubscriberOriginal.ForEach(x => dedupedSubscriberOriginal.Add(x));

            //dedupedSubscriberOriginal = listSubscriberOriginal.GroupBy(item => new { item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone }).Select(group => group.First()).ToList();

            //ConsoleMessage("Start get Excluded: " + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode, true);
            //HashSet<FrameworkUAD.Entity.SubscriberOriginal> excluded = listSubscriberOriginal.Except(dedupedSubscriberOriginal);
            dataIV.OriginalDuplicateRecordCount = totalExcludeCount;
            //ConsoleMessage("Done get Excluded: " + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode, true);

            //ConsoleMessage("SO excluded dupes: " + excluded.Count.ToString(), eventMessage.AdmsLog.ProcessCode, true, dataIV.SourceFileId);

            if (totalExcludeCount > 0)
            {
                ConsoleMessage("Start SO demo check: " + DateTime.Now.ToString());
                int rowCount = 1;
                foreach (FrameworkUAD.Entity.SubscriberOriginal exc in excluded)
                {
                    string msg = "SO Demo check: " + rowCount.ToString() + " of " + excluded.Count.ToString();
                    ConsoleMessage(msg, eventMessage.AdmsLog.ProcessCode, logTranDetail);

                    FrameworkUAD.Entity.SubscriberOriginal deDupeMatch = listSubscriberOriginal.SingleOrDefault(x => x.Address == exc.Address &&
                                                                                                                     x.PubCode == exc.PubCode &&
                                                                                                                     x.FName == exc.FName &&
                                                                                                                     x.LName == exc.LName &&
                                                                                                                     x.Company == exc.Company &&
                                                                                                                     x.Title == exc.Title &&
                                                                                                                     x.Email == exc.Email &&
                                                                                                                     x.Phone == exc.Phone &&
                                                                                                                     x.Sequence == exc.Sequence &&
                                                                                                                     x.PubCode == exc.PubCode &&
                                                                                                                     x.AccountNumber == exc.AccountNumber &&
                                                                                                                     x.SORecordIdentifier != exc.SORecordIdentifier);
                    if (deDupeMatch != null)
                    {
                        //this will always get all as differences cause SORecIdentifiers are always different so need to join on Maff and check for different values
                        //exc.DemographicOriginalList.Except(deDupeMatch.DemographicOriginalList).ToList();
                        HashSet<SubscriberDemographicOriginal> notMatch = new HashSet<SubscriberDemographicOriginal>((from excd in exc.DemographicOriginalList
                                                                                                                      join dd in deDupeMatch.DemographicOriginalList on excd.MAFField equals dd.MAFField
                                                                                                                      where excd.Value != dd.Value
                                                                                                                      select excd).Distinct().ToList());

                        HashSet<SubscriberDemographicOriginal> notExist = new HashSet<SubscriberDemographicOriginal>((from e in exc.DemographicOriginalList
                                                                                                                      where !deDupeMatch.DemographicOriginalList.Any(x => x.MAFField == e.MAFField)
                                                                                                                      select e).ToList());


                        HashSet<SubscriberDemographicOriginal> combined = new HashSet<SubscriberDemographicOriginal>();
                        notMatch.ToList().ForEach(x => { combined.Add(x); });
                        notExist.ToList().ForEach(x => { combined.Add(x); });
                        combined.ToList().ForEach(x => x.SORecordIdentifier = deDupeMatch.SORecordIdentifier);
                        combined.ToList().ForEach(x =>
                        {
                            if (!deDupeMatch.DemographicOriginalList.ToList().Exists(y => y.MAFField == x.MAFField && y.Value == x.Value))
                                deDupeMatch.DemographicOriginalList.Add(x);
                        });
                        notMatch = null;
                        notExist = null;
                        combined = null;
                    }
                    deDupeMatch = null;

                    rowCount++;
                }
                ConsoleMessage("End SO demo check: " + DateTime.Now.ToString());

                //Per Sunil 9/17/15 - do not save dedupe records to Archive
                //ConsoleMessage("Start Archive Duplicate Subscribers: " + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode, false);
                //FrameworkUAD.BusinessLogic.SubscriberArchive saworker = new FrameworkUAD.BusinessLogic.SubscriberArchive();
                //saworker.SaveBulkInsert(excluded, eventMessage.Client.ClientConnections);
                //ConsoleMessage("End Archive Duplicate Subscribers: " + DateTime.Now.ToString(), eventMessage.AdmsLog.ProcessCode, false);
            }
            //else
            //    dedupedSubscriberOriginal = listSubscriberOriginal;

            ConsoleMessage("SO List count after dedupe: " + listSubscriberOriginal.Count.ToString(), eventMessage.AdmsLog.ProcessCode, true, dataIV.SourceFileId);

            #endregion

            ConsoleMessage("Start: Original Bulk Data Insert " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
            admsWrk.Update(eventMessage.AdmsLog.ProcessCode,
                                             FrameworkUAD_Lookup.Enums.FileStatusType.Processing,
                                             FrameworkUAD_Lookup.Enums.ADMS_StepType.Validator_Inserting_Original_Subscribers,
                                             FrameworkUAD_Lookup.Enums.ProcessingStatusType.In_CreatingSubscribers,
                                             FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_Validation_Process, 1,
                                             "Start: Original Bulk Data Insert " + DateTime.Now.TimeOfDay.ToString(), true,
                                             eventMessage.AdmsLog.SourceFileId);

            FrameworkUAD.BusinessLogic.SubscriberOriginal so = new FrameworkUAD.BusinessLogic.SubscriberOriginal();
            bool success = so.SaveBulkSqlInsert(listSubscriberOriginal.ToList(), eventMessage.Client.ClientConnections);
            if (success == false)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("ERROR: Insert to Original Bulk Data Insert " + DateTime.Now.TimeOfDay.ToString());
                msg.AppendLine("An unexplained error occurred while inserting records into Original or DemoOriginal tables.<br/>");
                msg.AppendLine("The details of this error have been logged in the FileLog table.<br/>");
                msg.AppendLine("Please run this query.<br/>");
                msg.AppendLine("Select * From FileLog With(NoLock) Where SourceFileID = " + eventMessage.SourceFile.SourceFileID.ToString() + " AND ProcessCode = '" + eventMessage.AdmsLog.ProcessCode.ToString() + "' ORDER BY LogDate, LogTime; GO");

                ConsoleMessage(msg.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);
                Emailer.Emailer emWorker = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.EmailError);
                emWorker.EmailError(msg.ToString(), eventMessage.AdmsLog.ProcessCode, eventMessage.SourceFile.SourceFileID);

                //dedupedSubscriberOriginal.Clear();
            }

            ConsoleMessage("Done: Original Bulk Data Insert " + DateTime.Now.TimeOfDay.ToString(), eventMessage.AdmsLog.ProcessCode, true, eventMessage.SourceFile.SourceFileID);

            //original dupes 
            int dedupeDemos = 0;
            foreach (var d in excluded)
                dedupeDemos += d.DemographicOriginalList.Count;
            admsWrk.UpdateDuplicateCounts(eventMessage.AdmsLog.ProcessCode, totalExcludeCount, totalExcludeCount, dedupeDemos, 1, true, eventMessage.AdmsLog.SourceFileId);
            eventMessage.AdmsLog.DuplicateRecordCount = totalExcludeCount;
            eventMessage.AdmsLog.DuplicateProfileCount = totalExcludeCount;
            eventMessage.AdmsLog.DuplicateDemoCount = dedupeDemos;

            return listSubscriberOriginal;
        }
        private int GetPubCodeId(StringDictionary myRow, FrameworkUAS.Entity.SourceFile sourceFile)
        {
            int pubCodeId = 0;
            var pub = sourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals("pubcode", StringComparison.CurrentCultureIgnoreCase));
            if (pub != null)
            {
                string pubCodeValue = myRow[pub.IncomingField].ToString();
                if (clientPubCodes.Count(x => x.Value.Equals(pubCodeValue, StringComparison.CurrentCultureIgnoreCase)) > 0)
                    pubCodeId = clientPubCodes.SingleOrDefault(x => x.Value.Equals(pubCodeValue, StringComparison.CurrentCultureIgnoreCase)).Key;
            }
            return pubCodeId;
        }

        private SubscriberOriginal CreateSubscriber(
            StringDictionary myRow,
            HashSet<AdHocDimensionGroup> ahdGroups,
            AdmsLog admsLog,
            SourceFile sourceFile,
            FrameworkUAD_Lookup.Enums.FileTypes dft)
        {
            //ConsoleMessage("Start: Create Original Subscriber " + DateTime.Now.TimeOfDay.ToString(),admsLog.ProcessCode, logTranDetail);
            var rowNumber = 0;
            var pubCodeId = 0;

            pubCodeId = GetPubCodeId(myRow, sourceFile);
            int.TryParse(myRow["OriginalImportRow"].ToString(), out rowNumber);
            var subscriberOriginal = new SubscriberOriginal(admsLog.SourceFileId, rowNumber, admsLog.ProcessCode);

            // Standard/Demographic
            foreach (var map in sourceFile.FieldMappings.Where(m=>m.FieldMappingTypeID != demoDateTypeID))
            {
                try
                {
                    //If FieldMappingTypeId is Demographic Date ignore. This will be used to set the DateCreated to be passed to PubSubscriptionDetail DateCreated
                    if (map.IncomingField == null)
                        continue;
                    else if (map.FieldMappingTypeID == demoDateTypeID)
                        continue;

                    var value = myRow[map.IncomingField] ?? string.Empty;
                    if (map.FieldMappingTypeID == standarTypeID)
                    {
                        ValidatorHelper.SetProfileFieldValueByFieldMapping(map, subscriberOriginal, value);
                    }
                    else if ((map.FieldMappingTypeID == demoTypeID) && ((value != null && !string.IsNullOrEmpty(value.ToString())) || dft == FrameworkUAD_Lookup.Enums.FileTypes.QuickFill || dft == FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction))
                    {
                        ValidatorHelper.ExtendWithDemographicsItem(ahdGroups, subscriberOriginal, map.DemographicUpdateCodeId, map.MAFField, pubCodeId, value);
                    }  
                    if (map.FieldMultiMappings.Count > 0)
                    {
                        foreach (var mmap in map.FieldMultiMappings.OfType<IFieldMapping>())
                        {
                            if (mmap.FieldMappingTypeID == standarTypeID)
                            {
                                ValidatorHelper.SetProfileFieldValueByFieldMapping(mmap, subscriberOriginal, value);
                            }
                            else if ((mmap.FieldMappingTypeID == demoTypeID) &&
                                      ((value != null && !string.IsNullOrEmpty(value.ToString())) ||
                                        dft == FrameworkUAD_Lookup.Enums.FileTypes.QuickFill ||
                                        dft == FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction))
                            {
                                ValidatorHelper.ExtendWithDemographicsItem(ahdGroups, subscriberOriginal, map.DemographicUpdateCodeId, mmap.MAFField, pubCodeId, value);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".CreateSubscriber");
                }
            }

            // Demographics Response Other
            foreach (var map in sourceFile.FieldMappings.Where(x => x.FieldMappingTypeID.Equals(demoRespOtherTypeID)).ToList())
            {
                try
                {
                    if (map.IncomingField == null)
                        continue;
                    var value = myRow[map.IncomingField] == null ? string.Empty : myRow[map.IncomingField];
                    if ((map.FieldMappingTypeID == demoRespOtherTypeID) && value != null && !string.IsNullOrEmpty(value.ToString()))
                    {
                        // Demographics
                        string demoName = map.MAFField.ToUpper().Replace("_RESPONSEOTHER", "").ToString();
                        List<SubscriberDemographicOriginal> sdoList = new List<SubscriberDemographicOriginal>();
                        sdoList.AddRange(subscriberOriginal.DemographicOriginalList.Where(x => x.MAFField.Equals(demoName, StringComparison.CurrentCultureIgnoreCase)));
                        sdoList.ForEach(x => x.ResponseOther = value);
                    }
                    if (map.FieldMultiMappings.Count > 0)
                    {
                        foreach (var mmap in map.FieldMultiMappings)
                        {
                            if ((mmap.FieldMappingTypeID == demoRespOtherTypeID) && value != null && !string.IsNullOrEmpty(value.ToString()))
                            {
                                // Demographics
                                string demoName = map.MAFField.ToUpper().Replace("_RESPONSEOTHER", "").ToString();
                                List<SubscriberDemographicOriginal> sdoList = new List<SubscriberDemographicOriginal>();
                                sdoList.AddRange(subscriberOriginal.DemographicOriginalList.Where(x => x.MAFField.Equals(demoName, StringComparison.CurrentCultureIgnoreCase)));
                                sdoList.ForEach(x => x.ResponseOther = value);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".CreateSubscriber");
                }
            }

            //ConsoleMessage("End: Create Original Subscriber " + DateTime.Now.TimeOfDay.ToString(), admsLog.ProcessCode, logTranDetail);
            return subscriberOriginal;
        }

        private SubscriberTransformed CreateTransformedSubscriber(
            KMPlatform.Entity.ServiceFeature myServFeat,
            HashSet<TransformSplit> allTransformSplit,
            StringDictionary myRow,
            int originalRowNumber,
            int transformedRowNumber,
            Dictionary<int, Guid> SubOriginalDictionary,
            HashSet<AdHocDimensionGroup> ahdGroups,
            HashSet<TransformationFieldMap> splitTranFieldMappings,
            HashSet<Transformation> splitTrans,
            int qSourceId,
            int emailStatusId,
            List<Code> demoUpdateCodes,
            AdmsLog admsLog,
            SourceFile sourceFile,
            FrameworkUAD_Lookup.Enums.FileTypes dft)
        {
            Guid soRecordIdentifier = Guid.Empty;
            if (SubOriginalDictionary.ContainsKey(originalRowNumber))
                soRecordIdentifier = SubOriginalDictionary[originalRowNumber];

            var subscriberTransformed =
                new SubscriberTransformed(admsLog.SourceFileId, soRecordIdentifier, admsLog.ProcessCode)
                {
                    ImportRowNumber = transformedRowNumber,
                    OriginalImportRow = originalRowNumber
                };

            //Apply EmailStatusID and QSourceID in case the user didn't map them. This way they get set.
            subscriberTransformed.EmailStatusID = emailStatusId != -1 ? (int?)emailStatusId : null;
            subscriberTransformed.QSourceID = qSourceId;
            var pubCodeId = GetPubCodeId(myRow, sourceFile);
            var demoAppendId = 0;
            var demoReplaceId = 0;
            var demoOverwriteId = 0;
            int.TryParse(demoUpdateCodes.FirstOrDefault(x => x.CodeName.Equals("Append")).CodeId.ToString(), out demoAppendId);
            int.TryParse(demoUpdateCodes.FirstOrDefault(x => x.CodeName.Equals("Replace")).CodeId.ToString(), out demoReplaceId);
            int.TryParse(demoUpdateCodes.FirstOrDefault(x => x.CodeName.Equals("Overwrite")).CodeId.ToString(), out demoOverwriteId);

            // Standard/Demographic
            foreach (var map in sourceFile.FieldMappings)
            {
                try
                {
                    //If FieldMappingTypeId is Demographic Date ignore. This will be used to set the DateCreated to be passed to PubSubscriptionDetail DateCreated
                    if (map.FieldMappingTypeID == demoDateTypeID)
                    {
                        var demoValueCol = map.MAFField.ToUpper().Replace("_DEMODATE", "");
                        if (sourceFile.FieldMappings.Any(x =>
                            x.MAFField.Equals(demoValueCol, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            //Demo value found (isDemoDate = false)
                            continue;
                        }

                        //Demo value not found (isDemoDate = true)
                        string dateValue = myRow[map.IncomingField] ?? string.Empty;
                        if (string.IsNullOrEmpty(dateValue))
                        {
                            dateValue = GetDateValueFromQualificationDate(myRow, sourceFile);
                        }

                        var demoDate = DemoDateFormat(sourceFile.QDateFormat, dateValue);

                        ValidatorHelper.ExtendWithDemographicsItem(
                            ahdGroups,
                            subscriberTransformed,
                            map.DemographicUpdateCodeId,
                            demoValueCol, 
                            pubCodeId,
                            demoDate,
                            true,
                            string.Empty);
                    }
                    else if (map.MAFField.Equals(
                        Enums.MAFFieldStandardFields.QUALIFICATIONDATE.ToString(),
                        StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }

                    var value = myRow[map.IncomingField] ?? string.Empty;
                    if (map.FieldMappingTypeID == standarTypeID)
                    {
                        ValidatorHelper.SetProfileFieldValueByFieldMapping(map, subscriberTransformed, value);
                    }
                    else if ((map.FieldMappingTypeID == demoTypeID) &&
                             (!string.IsNullOrWhiteSpace(value) ||
                              (string.IsNullOrEmpty(value.ToString()) &&
                               map.DemographicUpdateCodeId == demoOverwriteId) ||
                              dft == FrameworkUAD_Lookup.Enums.FileTypes.QuickFill ||
                              dft == FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction)
                    )
                    {
                        string dateValue = GetDemoDate(myRow, sourceFile, map) ??
                                           GetDateValueFromQualificationDate(myRow, sourceFile);
                        DateTime? demoDate = DemoDateFormat(sourceFile.QDateFormat, dateValue);

                        // Demographics
                        if (KMPlatform.BusinessLogic.Enums.GetUADFeature(myServFeat.SFName) ==
                            KMPlatform.BusinessLogic.Enums.UADFeatures.Special_Split &&
                            splitTranFieldMappings.Any(x => x.FieldMappingID == map.FieldMappingID))
                        {
                            var splitData = ValidatorHelper.SplitDataStringByMapping(
                                allTransformSplit,
                                splitTranFieldMappings,
                                splitTrans,
                                map,
                                value);

                            foreach (string s in splitData.Where(s => !string.IsNullOrEmpty(s)).Distinct())
                            {
                                ValidatorHelper.ExtendWithDemographicsItem(
                                    ahdGroups, 
                                    subscriberTransformed,
                                    map.DemographicUpdateCodeId,
                                    map.MAFField,
                                    pubCodeId,
                                    demoDate,
                                    true,
                                    s);
                            }
                        }
                        else
                        {
                            ValidatorHelper.ExtendWithDemographicsItem(
                                ahdGroups, 
                                subscriberTransformed,
                                map.DemographicUpdateCodeId, 
                                map.MAFField, 
                                pubCodeId, 
                                demoDate,
                                true,
                                value);
                        }
                    }
                    else if (map.FieldMappingTypeID == demoRespOtherTypeID)
                    {
                        if (string.IsNullOrWhiteSpace(value) ||
                            string.IsNullOrWhiteSpace(map.IncomingField))
                        {
                            continue;
                        }

                        if (KMPlatform.BusinessLogic.Enums.GetUADFeature(myServFeat.SFName) ==
                            KMPlatform.BusinessLogic.Enums.UADFeatures.Special_Split &&
                            splitTranFieldMappings.Any(x => x.FieldMappingID == map.FieldMappingID))
                        {
                            var splitData = ValidatorHelper.SplitDataStringByMapping(
                                allTransformSplit,
                                splitTranFieldMappings,
                                splitTrans,
                                map,
                                value);

                            foreach (string s in splitData.Where(s => !string.IsNullOrEmpty(s)).Distinct())
                            {
                                ValidatorHelper.SetDemographicsResponseFields(map.MAFField, subscriberTransformed, s);
                            }
                        }
                        else
                        {
                            ValidatorHelper.SetDemographicsResponseFields(map.MAFField, subscriberTransformed, value);
                        }
                    }

                    foreach (var mmap in map.FieldMultiMappings)
                    {
                        if (mmap.FieldMappingTypeID == standarTypeID)
                        {
                            ValidatorHelper.SetProfileFieldValueByFieldMapping(mmap, subscriberTransformed, value);
                        }
                        else if ((mmap.FieldMappingTypeID == demoTypeID) && 
                                 (!string.IsNullOrEmpty(value.ToString()) ||
                                     (string.IsNullOrEmpty(value.ToString()) &&
                                      map.DemographicUpdateCodeId == demoOverwriteId) ||
                                      dft == FrameworkUAD_Lookup.Enums.FileTypes.QuickFill ||
                                      dft == FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction))
                        {
                            string dateValue = GetDemoDate(myRow, sourceFile, map) ??
                                               GetDemoDate(myRow, sourceFile, mmap) ??
                                               GetDateValueFromQualificationDate(myRow, sourceFile);

                            DateTime? demoDate = DemoDateFormat(sourceFile.QDateFormat, dateValue);
                            ValidatorHelper.ExtendWithDemographicsItem(
                                ahdGroups,
                                subscriberTransformed,
                                map.DemographicUpdateCodeId,
                                mmap.MAFField,
                                pubCodeId,
                                demoDate, 
                                true,
                                value);
                        }
                        else if (mmap.FieldMappingTypeID == demoRespOtherTypeID && !string.IsNullOrEmpty(value))
                        {
                            ValidatorHelper.SetDemographicsResponseFields(map.MAFField, subscriberTransformed, value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".CreateTransformedSubscriber");
                }
            }

            //Apply Default for Blank QDATE if mapped in file
            string qDateColumnHeader = string.Empty;
            FieldMapping thisFM = sourceFile.FieldMappings.SingleOrDefault(x =>
                x.MAFField.Equals(Enums.MAFFieldStandardFields.QUALIFICATIONDATE.ToString(),
                    StringComparison.CurrentCultureIgnoreCase));
            if (thisFM != null)
            {
                qDateColumnHeader = thisFM.IncomingField;
            }

            if (!string.IsNullOrWhiteSpace(qDateColumnHeader))
            {
                if (!string.IsNullOrWhiteSpace(myRow[qDateColumnHeader]))
                {
                    var QDateValue = myRow[qDateColumnHeader];
                    QDateValue = new string(QDateValue.Where(c => char.IsDigit(c)).ToArray());
                    if (string.IsNullOrEmpty(QDateValue))
                    {
                        DateTime? qDate = GetQDate(sourceFile);
                        myRow[qDateColumnHeader] = qDate.Value.ToShortDateString() ?? null;
                    }
                }
                else
                {
                    DateTime? qDate = GetQDate(sourceFile);
                    myRow[qDateColumnHeader] = qDate.Value.ToShortDateString() ?? null;
                }

                DateTime minDate = DateTime.Parse("1/1/1900");
                DateTime maxDate = DateTime.Parse("1/1/2079");
                var value = myRow[qDateColumnHeader] ?? string.Empty;
                bool dateDone = false;
                DateTime myDateValue = minDate;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    dateDone = DateTime.TryParseExact(value.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out myDateValue);
                    if (!dateDone)
                    {
                        DateTime.TryParse(value.ToString(), out myDateValue);
                    }
                }

                if (myDateValue == DateTime.MinValue ||
                    myDateValue > maxDate ||
                    myDateValue < minDate)
                {
                    myDateValue = minDate;
                }

                subscriberTransformed.QDate = myDateValue;
            }

            return subscriberTransformed;
        }

        private static string GetDemoDate(StringDictionary myRow, SourceFile sourceFile, IFieldMapping map)
        {
            var demoMap = sourceFile.FieldMappings.FirstOrDefault(x =>
                x.MAFField.Equals(map.MAFField + "_DemoDate", StringComparison.CurrentCultureIgnoreCase));
            if (demoMap != null)
            {
                if (string.IsNullOrWhiteSpace(myRow[demoMap.IncomingField]))
                {
                    return null;
                }

                return myRow[demoMap.IncomingField];
            }

            return null;
        }

        private static string GetDateValueFromQualificationDate(StringDictionary myRow, SourceFile sourceFile)
        {
            string dateValue = string.Empty;
            var qdateMap = sourceFile.FieldMappings.FirstOrDefault(x =>
                x.MAFField.Equals(
                    Enums.MAFFieldStandardFields.QUALIFICATIONDATE.ToString(),
                    StringComparison.CurrentCultureIgnoreCase));
            if (qdateMap != null)
            {
                dateValue = myRow[qdateMap.IncomingField] ?? string.Empty;
            }

            return dateValue;
        }

        private DateTime? DemoDateFormat(string QDateFormat, string dateValue)
        {
            #region DemoDate Format
            DateTime? returnValue = null;
            string QDatePattern2 = QDateFormat.Replace('D', 'd').Replace('Y', 'y');

            if (!string.IsNullOrEmpty(dateValue))
            {
                //Get QDateValue and then remove all characters not numeric for comparison.
                string QDateValue = dateValue.ToString();
                QDateValue = new string(QDateValue.Where(c => char.IsDigit(c)).ToArray());
                if (string.IsNullOrEmpty(QDateValue))
                    returnValue = null;
                else
                {
                    try
                    {
                        DateTime defaultDateTime = new DateTime();
                        DateTime QDateConvertValue = defaultDateTime;
                        DateTime.TryParseExact(QDateValue, QDatePattern2, null, DateTimeStyles.None, out QDateConvertValue);
                        if (QDateConvertValue == defaultDateTime)
                        {
                            //Reset to new and TryParse the original string
                            QDateConvertValue = defaultDateTime;
                            DateTime.TryParse(dateValue.ToString(), out QDateConvertValue);
                            if (QDateConvertValue == defaultDateTime)
                            {
                                var df = (DateFormat) Enum.Parse(typeof(DateFormat), QDateFormat);
                                QDateConvertValue = DateTimeFunctions.ParseDate(df, QDateValue);

                                if (QDateConvertValue >= defaultDateTime)
                                    returnValue = null;
                                else
                                    returnValue = QDateConvertValue;

                            }
                            else
                                returnValue = QDateConvertValue;

                        }
                        else
                            returnValue = QDateConvertValue;

                    }
                    catch (Exception ex)
                    {
                        returnValue = null;
                        LogError(ex, client, this.GetType().Name.ToString() + ".DemoDateFormat - Date Format");
                    }
                }
            }
            else
                returnValue = null;

            #endregion            
            return returnValue;
        }
        private HashSet<SubscriberInvalid> ConvertTransformedToInvalid(HashSet<SubscriberTransformed> listST)
        {
            HashSet<SubscriberInvalid> listSI = new HashSet<SubscriberInvalid>();
            foreach (SubscriberTransformed st in listST)
            {
                SubscriberInvalid si = new SubscriberInvalid(st);
                listSI.Add(si);
            }
            return listSI;
        }
        private HashSet<SubscriberTransformed> DeDupeTransformed(FrameworkUAD.Object.ImportFile dataIV, HashSet<SubscriberTransformed> listOrigST, string processCode, KMPlatform.Entity.Client client)
        {
            #region dedupe profiles
            List<FrameworkUAD.Entity.Product> products = new List<Product>();
            FrameworkUAD.BusinessLogic.Product pWorker = new FrameworkUAD.BusinessLogic.Product();
            products = pWorker.Select(client.ClientConnections);
            listOrigST.OrderBy(x => x.FName).ThenBy(y => y.LName).ToList();
            ConsoleMessage("ST List count before dedupe: " + listOrigST.Count.ToString(), processCode, true, dataIV.SourceFileId);

            //now due to transformations we may have duplicate profiles
            //can only do this after creating Subscriber objects - before insert
            HashSet<SubscriberTransformed> dedupedST = new HashSet<SubscriberTransformed>();
            //hashSet includes Demos tho so could have a "dupe"
            dedupedST = new HashSet<SubscriberTransformed>(listOrigST.GroupBy(item => new { item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone, item.Sequence, item.AccountNumber }).Select(group => group.First()).ToList());
            ConsoleMessage("dedupedST count: " + dedupedST.Count.ToString());
            ConsoleMessage("Start get Excluded: " + DateTime.Now.ToString());
            HashSet<SubscriberTransformed> excluded = new HashSet<SubscriberTransformed>(listOrigST.Except(dedupedST).ToList());
            dataIV.TransformedDuplicateRecordCount = excluded.Count;
            ConsoleMessage("Done get Excluded: " + DateTime.Now.ToString());
            ConsoleMessage("ST excluded dupes: " + excluded.Count.ToString(), processCode, true, dataIV.SourceFileId, 1);
            if (excluded.Count > 0)
            {
                ConsoleMessage("Start demo check: " + DateTime.Now.ToString(), processCode, logTranDetail);
                int rowCount = 1;
                foreach (FrameworkUAD.Entity.SubscriberTransformed exc in excluded)
                {
                    string msg = "Demo check: " + rowCount.ToString() + " of " + excluded.Count.ToString();
                    ConsoleMessage(msg, processCode, logTranDetail);

                    FrameworkUAD.Entity.SubscriberTransformed deDupeMatch = dedupedST.FirstOrDefault(x => (x.Address == exc.Address &&
                                                                                            x.PubCode == exc.PubCode &&
                                                                                            x.FName == exc.FName &&
                                                                                            x.LName == exc.LName &&
                                                                                            x.Company == exc.Company &&
                                                                                            x.Title == exc.Title &&
                                                                                            x.Email == exc.Email &&
                                                                                            x.Phone == exc.Phone &&
                                                                                            x.Sequence == exc.Sequence &&
                                                                                            x.AccountNumber == exc.AccountNumber) && x.STRecordIdentifier != exc.STRecordIdentifier);
                    if (deDupeMatch != null
                        && (products.FirstOrDefault(x => x.PubCode.Equals(deDupeMatch.PubCode, StringComparison.CurrentCultureIgnoreCase) && x.IsCirc == false) != null//not circ
                            ||
                            (products.FirstOrDefault(x => x.PubCode.Equals(deDupeMatch.PubCode, StringComparison.CurrentCultureIgnoreCase) && x.IsCirc == true) != null && deDupeMatch.SORecordIdentifier == exc.SORecordIdentifier)
                            )
                        )
                    {
                        HashSet<SubscriberDemographicTransformed> notMatch = new HashSet<SubscriberDemographicTransformed>((from excd in exc.DemographicTransformedList
                                                                                                                            join dd in deDupeMatch.DemographicTransformedList on excd.MAFField equals dd.MAFField
                                                                                                                            where excd.Value != dd.Value
                                                                                                                            select excd).Distinct().ToList());

                        HashSet<SubscriberDemographicTransformed> notExist = new HashSet<SubscriberDemographicTransformed>((from e in exc.DemographicTransformedList
                                                                                                                            where !deDupeMatch.DemographicTransformedList.Any(x => x.MAFField == e.MAFField)
                                                                                                                            select e).ToList());


                        HashSet<SubscriberDemographicTransformed> combined = new HashSet<SubscriberDemographicTransformed>();
                        notMatch.ToList().ForEach(x => { combined.Add(x); });
                        notExist.ToList().ForEach(x => { combined.Add(x); });
                        combined.ToList().ForEach(x => x.STRecordIdentifier = deDupeMatch.STRecordIdentifier);
                        combined.ToList().ForEach(x => x.SORecordIdentifier = deDupeMatch.SORecordIdentifier);
                        combined.ToList().ForEach(x =>
                        {
                            if (!deDupeMatch.DemographicTransformedList.ToList().Exists(y => y.MAFField == x.MAFField && y.Value == x.Value))
                                deDupeMatch.DemographicTransformedList.Add(x);
                        });
                        notMatch = null;
                        notExist = null;
                        combined = null;
                    }
                    deDupeMatch = null;
                    rowCount++;
                }
                ConsoleMessage("End demo check: " + DateTime.Now.ToString(), processCode, true);

                //Per Sunil 9/17/15 - do not save dedupe records to Archive
                //ConsoleMessage("Start Archive Duplicate Subscribers: " + DateTime.Now.ToString(), processCode, false);
                //FrameworkUAD.BusinessLogic.SubscriberArchive saworker = new FrameworkUAD.BusinessLogic.SubscriberArchive();
                //saworker.SaveBulkInsert(excluded, client.ClientConnections);
                //ConsoleMessage("End Archive Duplicate Subscribers: " + DateTime.Now.ToString(), processCode, false);
            }
            else
                dedupedST = listOrigST;

            ConsoleMessage("ST List count after dedupe: " + dedupedST.Count.ToString(), processCode, true, dataIV.SourceFileId);

            #endregion

            return dedupedST;
        }
        private HashSet<SubscriberInvalid> DeDupeInvalid(HashSet<SubscriberInvalid> listOrig, FrameworkUAS.Entity.AdmsLog admsLog)
        {
            #region dedupe profiles
            ConsoleMessage("SubscriberInvalid List count before dedupe: " + listOrig.Count.ToString(), admsLog.ProcessCode, true);

            //now due to transformations we may have duplicate profiles
            //can only do this after creating Subscriber objects - before insert
            //group on PubCode, FirstName, LastName, Company, Title, Address, Email, Phone
            HashSet<SubscriberInvalid> deduped = new HashSet<SubscriberInvalid>(listOrig.GroupBy(item => new { item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone }).Select(group => group.First()).ToList());

            ConsoleMessage("Start get Excluded: " + DateTime.Now.ToString(), admsLog.ProcessCode, true);
            List<FrameworkUAD.Entity.SubscriberInvalid> excluded = listOrig.Except(deduped).ToList();
            ConsoleMessage("Done get Excluded: " + DateTime.Now.ToString(), admsLog.ProcessCode, true);

            ConsoleMessage("SI excluded dupes: " + excluded.Count.ToString(), admsLog.ProcessCode, true, -2);


            if (excluded.Count > 0)
            {
                ConsoleMessage("Start demo check: " + DateTime.Now.ToString(), admsLog.ProcessCode, true);
                int rowCount = 1;
                foreach (FrameworkUAD.Entity.SubscriberInvalid exc in excluded)
                {
                    string msg = "Demo check: " + rowCount.ToString() + " of " + excluded.Count.ToString();
                    ConsoleMessage(msg, admsLog.ProcessCode, logTranDetail);

                    FrameworkUAD.Entity.SubscriberInvalid deDupeMatch = deduped.FirstOrDefault(x => (x.Address == exc.Address &&
                                                                                            x.PubCode == exc.PubCode &&
                                                                                            x.FName == exc.FName &&
                                                                                            x.LName == exc.LName &&
                                                                                            x.Company == exc.Company &&
                                                                                            x.Title == exc.Title &&
                                                                                            x.Email == exc.Email &&
                                                                                            x.Phone == exc.Phone) && x.SIRecordIdentifier != exc.SIRecordIdentifier);
                    if (deDupeMatch != null)
                    {
                        List<FrameworkUAD.Entity.SubscriberDemographicInvalid> notMatch = (from excd in exc.DemographicInvalidList
                                                                                           join dd in deDupeMatch.DemographicInvalidList on excd.MAFField equals dd.MAFField
                                                                                           where excd.Value != dd.Value
                                                                                           select excd).Distinct().ToList();

                        List<FrameworkUAD.Entity.SubscriberDemographicInvalid> notExist = (from e in exc.DemographicInvalidList
                                                                                           where !deDupeMatch.DemographicInvalidList.Any(x => x.MAFField == e.MAFField)
                                                                                           select e).ToList();


                        List<FrameworkUAD.Entity.SubscriberDemographicInvalid> combined = new List<SubscriberDemographicInvalid>();
                        combined.AddRange(notMatch);
                        combined.AddRange(notExist);
                        combined.ForEach(x => x.SIRecordIdentifier = deDupeMatch.SIRecordIdentifier);
                        combined.ForEach(x => x.SORecordIdentifier = deDupeMatch.SORecordIdentifier);
                        combined.ToList().ForEach(x =>
                        {
                            if (!deDupeMatch.DemographicInvalidList.ToList().Exists(y => y.MAFField == x.MAFField && y.Value == x.Value))
                                deDupeMatch.DemographicInvalidList.Add(x);
                        });
                        notMatch = null;
                        notExist = null;
                        combined = null;
                    }
                    deDupeMatch = null;
                    rowCount++;
                }
                ConsoleMessage("End demo check: " + DateTime.Now.ToString(), admsLog.ProcessCode, true);
            }
            else
                deduped = listOrig;

            ConsoleMessage("SubscriberInvalid List count after dedupe: " + deduped.Count.ToString(), admsLog.ProcessCode, true);

            #endregion

            return deduped;
        }
        #endregion


        private HashSet<SubscriberTransformed> HasQualifiedProfile(HashSet<SubscriberTransformed> checkList, FrameworkUAD.Object.ImportFile dataIV, FrameworkUAD_Lookup.Enums.FileTypes dft, FrameworkUAS.Entity.AdmsLog admsLog, FrameworkUAS.Entity.SourceFile sourceFile)
        {
            var ruleSets = GetRuleSetsByExecutionPoint(FrameworkUAD_Lookup.Enums.ExecutionPointType.QualifiedProfile, sourceFile);
            var currentDateRS = GetCurrentDateSpecificRuleSet(ruleSets);

            HashSet<SubscriberTransformed> qualified = new HashSet<SubscriberTransformed>();

            //HasQualifiedProfile is a set of one or more Rules used to determine if a valid profile
            //Each Rule can have one or more values to evaluate for
            //Rules can be chained together for AND/OR execution / Can also break execution based on condition being met.

            FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> ruleChainCodes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Rule_Chain);

            if (currentDateRS.Count > 0)
            {
                #region Date Range Rules
                foreach (var t in checkList)
                {
                    bool isQualified = true;
                    bool previousCheckResult = true;
                    StringBuilder sb = new StringBuilder();
                    foreach (var rs in currentDateRS)
                    {
                        foreach (var r in rs.Rules.OrderBy(x => x.RuleOrder))
                        {
                            FrameworkUAD_Lookup.Entity.Code rcCodeId = ruleChainCodes.Single(x => x.CodeId == r.RuleChainId);
                            FrameworkUAD_Lookup.Enums.RuleChain currentRC = FrameworkUAD_Lookup.Enums.GetRuleChain(rcCodeId.CodeName);
                            FrameworkUAD_Lookup.Enums.RuleChain? previousRC = null;
                            if (r.RuleOrder > 1)
                            {
                                int? prevRcId = rs.Rules.SingleOrDefault(x => x.RuleOrder == (r.RuleOrder - 1)).RuleChainId;
                                if (prevRcId.HasValue)
                                {
                                    FrameworkUAD_Lookup.Entity.Code rcPrev = ruleChainCodes.Single(x => x.CodeId == prevRcId);
                                    previousRC = FrameworkUAD_Lookup.Enums.GetRuleChain(rcPrev.CodeName);
                                }
                            }

                            switch (r.RuleName)
                            {
                                case "QualifiedName":
                                    //when we hit the QName check have 3 things to test for
                                    //--2 7   First and Last Name FName_LName if first or last name null or empty then false else true
                                    //--3 7   First Name  FName   if first name null or empty then false else true
                                    //--4 7   Last Name   LName   if last name null or empty then false else true
                                    //--5 8   Address Address if address name null or empty then false else true
                                    foreach (var rv in r.RuleValues)
                                    {
                                        if (rv.Value == "FName_LName")
                                        {
                                            if (string.IsNullOrEmpty(t.FName) && string.IsNullOrEmpty(t.LName))
                                            {
                                                isQualified = false;
                                                sb.Append("First name and last name are missing. ");
                                            }
                                            else
                                                isQualified = true;
                                        }
                                        if (rv.Value == "FName")
                                        {
                                            if (string.IsNullOrEmpty(t.FName))
                                            {
                                                isQualified = false;
                                                sb.Append("First name is missing. ");
                                            }
                                            else
                                                isQualified = true;
                                        }
                                        if (rv.Value == "LName")
                                        {
                                            if (string.IsNullOrEmpty(t.LName))
                                            {
                                                isQualified = false;
                                                sb.Append("Last name is missing. ");
                                            }
                                            else
                                                isQualified = true;
                                        }
                                    }

                                    //FrameworkUAS.Entity.RuleValue rv = r.RuleValues.SingleOrDefault(x => x.DisplayValue.Equals("Email RegEx"));
                                    //if (rv != null)
                                    //{
                                    //    if (!string.IsNullOrEmpty(rv.Value))
                                    //        checkList.Where(x => !string.IsNullOrEmpty(x.Email) && Core_AMS.Utilities.StringFunctions.isEmail(x.Email, rv.Value)).ToList().ForEach(x => { listEmailValid.Add(x); });
                                    //    else
                                    //        checkList.Where(x => !string.IsNullOrEmpty(x.Email) && Core_AMS.Utilities.StringFunctions.isEmail(x.Email)).ToList().ForEach(x => { listEmailValid.Add(x); });
                                    //}
                                    break;
                                case "QualifiedAddress":
                                    foreach (var rv in r.RuleValues)
                                    {
                                        if (rv.Value == "Address")
                                        {
                                            if (string.IsNullOrEmpty(t.Address))
                                            {
                                                isQualified = false;
                                                sb.Append("Address is missing. ");
                                            }
                                            else
                                                isQualified = true;
                                        }
                                        if (rv.Value == "Zip_City")
                                        {
                                            if (string.IsNullOrEmpty(t.Zip) && string.IsNullOrEmpty(t.City))
                                            {
                                                isQualified = false;
                                                sb.Append("Zip code and City are missing. ");
                                            }
                                            else
                                                isQualified = true;
                                        }
                                        if (rv.Value == "State_Country")
                                        {
                                            if (string.IsNullOrEmpty(t.RegCode) && string.IsNullOrEmpty(t.Country))
                                            {
                                                isQualified = false;
                                                sb.Append("Country and state are missing. ");
                                            }
                                            else
                                                isQualified = true;
                                        }
                                    }
                                    break;
                                case "QualifiedPhone":
                                    foreach (var rv in r.RuleValues)
                                    {
                                        if (rv.Value == "Phone_Mobile_Fax")
                                        {
                                            if (string.IsNullOrEmpty(t.Phone) && string.IsNullOrEmpty(t.Fax) && string.IsNullOrEmpty(t.Mobile))
                                            {
                                                isQualified = false;
                                                sb.Append("Phone, fax or mobile is missing. ");
                                            }
                                            else
                                                isQualified = true;
                                        }
                                        if (rv.Value == "Phone")
                                        {
                                            if (string.IsNullOrEmpty(t.Phone))
                                            {
                                                isQualified = false;
                                                sb.Append("Phone number is missing. ");
                                            }
                                            else
                                                isQualified = true;
                                        }
                                        if (rv.Value == "Mobile")
                                        {
                                            if (string.IsNullOrEmpty(t.Mobile))
                                            {
                                                isQualified = false;
                                                sb.Append("Mobile number is missing. ");
                                            }
                                            else
                                                isQualified = true;
                                        }
                                        if (rv.Value == "Fax")
                                        {
                                            if (string.IsNullOrEmpty(t.Fax))
                                            {
                                                isQualified = false;
                                                sb.Append("Fax number is missing. ");
                                            }
                                            else
                                                isQualified = true;
                                        }
                                    }
                                    break;
                                case "QualifiedCompany":
                                    isQualified = SetRuleIsQualified(t, sb, r, CompanyKey, isQualified);
                                    break;
                                case "QualifiedTitle":
                                    isQualified = SetRuleIsQualified(t, sb, r, TitleKey, isQualified);
                                    break;
                            }
                            previousCheckResult = isQualified;

                            if (previousRC.HasValue)
                            {
                                if (previousRC == FrameworkUAD_Lookup.Enums.RuleChain.And)
                                {
                                    if (previousCheckResult == true && isQualified == true)
                                        isQualified = true;
                                    else
                                        isQualified = false;
                                }
                                else if (previousRC == FrameworkUAD_Lookup.Enums.RuleChain.Or)
                                {
                                    if (previousCheckResult == true || isQualified == true)
                                        isQualified = true;
                                    else
                                        isQualified = false;
                                }
                            }


                            if (currentRC == FrameworkUAD_Lookup.Enums.RuleChain.BreakAlways)
                                break;
                            else if (currentRC == FrameworkUAD_Lookup.Enums.RuleChain.BreakTrue)
                                if (isQualified == true)
                                    break;
                                else if (currentRC == FrameworkUAD_Lookup.Enums.RuleChain.BreakFalse)
                                    if (isQualified == false)
                                        break;
                        }
                    }

                    if (isQualified)
                        qualified.Add(t);
                    else
                    {
                        String msg = String.Format(sb.ToString() + "@ Row {0}", t.OriginalImportRow);
                        dataIV = ValidationErrorHashSet(dataIV, msg, dft, admsLog, t.OriginalImportRow, t);
                    }
                }
                #endregion
            }
            else
            {
                #region Non date range rules
                foreach (var t in checkList)
                {
                    #region record vars
                    //bool isFirstSet = true;
                    #endregion

                    #region RuleSet iteration
                    foreach (var rs in ruleSets)//there will only ever be one RuleSet for Qualified Profile
                    {
                        #region ConditionGroup iteration
                        StringBuilder sb = new StringBuilder();
                        bool breakConditionProcessing = false;
                        List<int> conditionGroupsProcessed = new List<int>();//var for holding condtion groups processed so those rules can be skipped in outer most loop
                        int currentConditionGroup = 0;
                        int previousConditionGroup = 0;
                        Dictionary<int, bool> conditionGroupResults = new Dictionary<int, bool>();
                        List<int> conditionGroups = new List<int>();
                        rs.Rules.ToList().ForEach(x =>
                        {
                            if (!conditionGroups.Contains(x.ConditionGroup))
                                conditionGroups.Add(x.ConditionGroup);

                            if (!conditionGroupResults.ContainsKey(x.ConditionGroup))
                                conditionGroupResults.Add(x.ConditionGroup, false);
                        });
                        bool currentCondGroupIsQualified = false;
                        bool previousCondGroupIsQualified = false;
                        FrameworkUAD_Lookup.Enums.RuleChain? previousConditionGroupRC = null;

                        foreach (int cg in conditionGroups)//will always have at least 1 condition group - KM default is 2 condition groups
                        {
                            if (breakConditionProcessing == false)
                            {
                                if (!conditionGroupsProcessed.Contains(cg))
                                {
                                    #region Condition Group
                                    List<int> ruleGroups = new List<int>();
                                    Dictionary<int, bool> ruleGroupResults = new Dictionary<int, bool>();
                                    List<FrameworkUAS.Object.Rule> cgRuleList = rs.Rules.Where(x => x.ConditionGroup == cg).OrderBy(x => x.RuleOrder).ToList();
                                    cgRuleList.ForEach(x =>
                                    {
                                        if (!ruleGroups.Contains(x.RuleGroup))
                                            ruleGroups.Add(x.RuleGroup);
                                        if (!ruleGroupResults.ContainsKey(x.RuleGroup))
                                            ruleGroupResults.Add(x.RuleGroup, false);
                                    });

                                    foreach (var cgRule in cgRuleList)//outer most rule loop
                                    {
                                        if (breakConditionProcessing == false)
                                        {
                                            if (!conditionGroupsProcessed.Contains(cgRule.ConditionGroup))
                                            {
                                                FrameworkUAD_Lookup.Entity.Code condtionGroupCode = ruleChainCodes.Single(x => x.CodeId == cgRule.ConditionChainId);
                                                FrameworkUAD_Lookup.Enums.RuleChain currentConditionGroupRC = FrameworkUAD_Lookup.Enums.GetRuleChain(condtionGroupCode.CodeName);
                                                currentConditionGroup = cgRule.ConditionGroup;

                                                bool breakRuleGroupProcessing = false;
                                                List<int> ruleGroupsProcessed = new List<int>();//var for holding rule groups processed so those rules can be skipped in outer most loop
                                                FrameworkUAD_Lookup.Enums.RuleChain currentRuleGroupRC;
                                                FrameworkUAD_Lookup.Enums.RuleChain? previousRuleGroupRC = null;
                                                int currentRuleGroup = 0;
                                                int previousRuleGroup = 0;
                                                bool currentRuleGroupIsQualified = false;
                                                bool previousRuleGroupIsQualified = false;
                                                //if there is a ruleGroup do that in a loop else just check the single rule
                                                //need to be able to skip any rules in r outer loop we process as a RuleGroup below
                                                #region RuleGroups in Condition Group
                                                foreach (var rg in ruleGroups)
                                                {
                                                    if (breakRuleGroupProcessing == false)
                                                    {
                                                        Dictionary<int, bool> ruleResults = new Dictionary<int, bool>();
                                                        List<int> ruleIds = new List<int>();
                                                        List<FrameworkUAS.Object.Rule> rgRuleList = cgRuleList.Where(x => x.RuleGroup == rg && !ruleGroupsProcessed.Contains(rg)).OrderBy(x => x.RuleOrder).ToList();

                                                        rgRuleList.ForEach(x =>
                                                        {
                                                            if (!ruleIds.Contains(x.RuleId))
                                                                ruleIds.Add(x.RuleId);
                                                            if (!ruleResults.ContainsKey(x.RuleId))
                                                                ruleResults.Add(x.RuleId, false);
                                                        });
                                                        foreach (var rgRule in rgRuleList)
                                                        {
                                                            if (breakRuleGroupProcessing == false)
                                                            {
                                                                if (!ruleGroupsProcessed.Contains(rgRule.RuleGroup))
                                                                {
                                                                    FrameworkUAD_Lookup.Entity.Code ruleGroupCode = ruleChainCodes.Single(x => x.CodeId == rgRule.RuleGroupChainId);
                                                                    currentRuleGroupRC = FrameworkUAD_Lookup.Enums.GetRuleChain(ruleGroupCode.CodeName);
                                                                    currentRuleGroup = rgRule.RuleGroup;

                                                                    #region Rule Loop
                                                                    List<int> ruleIdsProcessed = new List<int>();
                                                                    FrameworkUAD_Lookup.Enums.RuleChain currentRuleRC;
                                                                    FrameworkUAD_Lookup.Enums.RuleChain? previousRuleRC = null;
                                                                    int currentRuleId = 0;
                                                                    int previousRuleId = 0;
                                                                    bool currentRuleIsQualified = false;
                                                                    bool previousRuleIsQualified = false;
                                                                    foreach (var ruleId in ruleIds)
                                                                    {
                                                                        List<FrameworkUAS.Object.Rule> ruleList = rgRuleList.Where(x => x.RuleId == ruleId && !ruleIdsProcessed.Contains(rg)).OrderBy(x => x.RuleOrder).ToList();
                                                                        foreach (var rule in ruleList)
                                                                        {
                                                                            FrameworkUAD_Lookup.Entity.Code ruleCode = ruleChainCodes.Single(x => x.CodeId == rule.RuleChainId);
                                                                            currentRuleRC = FrameworkUAD_Lookup.Enums.GetRuleChain(ruleCode.CodeName);
                                                                            currentRuleId = rule.RuleId;

                                                                            #region rule switch loop - phase 2 make this dynamic
                                                                            switch (rule.RuleName)
                                                                            {
                                                                                #region evaluate rules
                                                                                case "QualifiedName":
                                                                                    #region QualifiedName
                                                                                    //when we hit the QName check have 3 things to test for
                                                                                    //--2 7   First and Last Name FName_LName if first or last name null or empty then false else true
                                                                                    //--3 7   First Name  FName   if first name null or empty then false else true
                                                                                    //--4 7   Last Name   LName   if last name null or empty then false else true
                                                                                    //--5 8   Address Address if address name null or empty then false else true
                                                                                    foreach (var rv in rule.RuleValues)
                                                                                    {
                                                                                        if (rv.Value == "FName_LName")
                                                                                        {
                                                                                            if (string.IsNullOrEmpty(t.FName) && string.IsNullOrEmpty(t.LName))
                                                                                            {
                                                                                                currentRuleIsQualified = false;
                                                                                                sb.Append("First name and last name are missing. ");
                                                                                            }
                                                                                            else
                                                                                                currentRuleIsQualified = true;
                                                                                        }
                                                                                        if (rv.Value == "FName")
                                                                                        {
                                                                                            if (string.IsNullOrEmpty(t.FName))
                                                                                            {
                                                                                                currentRuleIsQualified = false;
                                                                                                sb.Append("First name is missing. ");
                                                                                            }
                                                                                            else
                                                                                                currentRuleIsQualified = true;
                                                                                        }
                                                                                        if (rv.Value == "LName")
                                                                                        {
                                                                                            if (string.IsNullOrEmpty(t.LName))
                                                                                            {
                                                                                                currentRuleIsQualified = false;
                                                                                                sb.Append("Last name is missing. ");
                                                                                            }
                                                                                            else
                                                                                                currentRuleIsQualified = true;
                                                                                        }
                                                                                        if (currentRuleIsQualified)
                                                                                            break;
                                                                                    }
                                                                                    break;
                                                                                #endregion
                                                                                case "QualifiedAddress":
                                                                                    #region QualifiedAddress
                                                                                    foreach (var rv in rule.RuleValues)
                                                                                    {
                                                                                        if (rv.Value == "Address")
                                                                                        {
                                                                                            if (!string.IsNullOrEmpty(t.Address))
                                                                                                currentRuleIsQualified = true;
                                                                                            else
                                                                                            {
                                                                                                currentRuleIsQualified = false;
                                                                                                sb.Append("Address is missing. ");
                                                                                            }
                                                                                        }
                                                                                        if (rv.Value == "Zip_City")
                                                                                        {
                                                                                            if (!string.IsNullOrEmpty(t.Zip) || !string.IsNullOrEmpty(t.City))//change from && 04282017 JW bug for condtion 2 not working
                                                                                                currentRuleIsQualified = true;
                                                                                            else
                                                                                            {
                                                                                                currentRuleIsQualified = false;
                                                                                                sb.Append("Zipcode or city is missing. ");
                                                                                            }
                                                                                        }
                                                                                        if (rv.Value == "State_Country")
                                                                                        {
                                                                                            if (!string.IsNullOrEmpty(t.State) || !string.IsNullOrEmpty(t.Country))//change from && 04282017 JW bug for condtion 2 not working
                                                                                                currentRuleIsQualified = true;
                                                                                            else
                                                                                            {
                                                                                                currentRuleIsQualified = false;
                                                                                                sb.Append("State or country is missing. ");
                                                                                            }
                                                                                        }
                                                                                        if (currentRuleIsQualified)
                                                                                            break;
                                                                                    }
                                                                                    break;
                                                                                #endregion
                                                                                case "QualifiedPhone":
                                                                                    #region QualifiedPhone
                                                                                    foreach (var rv in rule.RuleValues)
                                                                                    {
                                                                                        if (rv.Value == "Phone_Mobile_Fax")
                                                                                        {
                                                                                            if (string.IsNullOrEmpty(t.Phone) && string.IsNullOrEmpty(t.Fax) && string.IsNullOrEmpty(t.Mobile))
                                                                                            {
                                                                                                currentRuleIsQualified = false;
                                                                                                sb.Append("Phone, mobile, or fax is missing. ");
                                                                                            }
                                                                                            else
                                                                                                currentRuleIsQualified = true;
                                                                                        }
                                                                                        if (rv.Value == "Phone")
                                                                                        {
                                                                                            if (string.IsNullOrEmpty(t.Phone))
                                                                                            {
                                                                                                currentRuleIsQualified = false;
                                                                                                sb.Append("Phone is missing. ");
                                                                                            }
                                                                                            else
                                                                                                currentRuleIsQualified = true;
                                                                                        }
                                                                                        if (rv.Value == "Mobile")
                                                                                        {
                                                                                            if (string.IsNullOrEmpty(t.Mobile))
                                                                                            {
                                                                                                currentRuleIsQualified = false;
                                                                                                sb.Append("Mobile is missing. ");
                                                                                            }
                                                                                            else
                                                                                                currentRuleIsQualified = true;
                                                                                        }
                                                                                        if (rv.Value == "Fax")
                                                                                        {
                                                                                            if (string.IsNullOrEmpty(t.Fax))
                                                                                            {
                                                                                                currentRuleIsQualified = false;
                                                                                                sb.Append("Fax is missing. ");
                                                                                            }
                                                                                            else
                                                                                                currentRuleIsQualified = true;
                                                                                        }
                                                                                        if (currentRuleIsQualified)
                                                                                            break;
                                                                                    }
                                                                                    break;
                                                                                #endregion
                                                                                case "QualifiedCompany":
                                                                                    currentRuleIsQualified = SetRuleIsQualified(t, sb, rule, CompanyKey, currentRuleIsQualified);
                                                                                    break;
                                                                                case "QualifiedTitle":
                                                                                    currentRuleIsQualified = SetRuleIsQualified(t, sb, rule, TitleKey, currentRuleIsQualified);
                                                                                    break;
                                                                                    #endregion
                                                                            }
                                                                            #endregion

                                                                            #region set current IsQualified based on previous RuleChain - RuleGroup 1 this is skipped
                                                                            //at the rule level need to be careful of the chain between 2 different rules - mainly a user setup issue
                                                                            if (previousRuleRC.HasValue && previousRuleId > 0)
                                                                            {
                                                                                if (previousRuleId == currentRuleId)
                                                                                {
                                                                                    if (previousRuleRC == FrameworkUAD_Lookup.Enums.RuleChain.And)
                                                                                    {
                                                                                        if (previousRuleIsQualified == true && currentRuleIsQualified == true)
                                                                                            currentRuleIsQualified = true;
                                                                                        else
                                                                                            currentRuleIsQualified = false;
                                                                                    }
                                                                                    else if (previousRuleRC == FrameworkUAD_Lookup.Enums.RuleChain.Or)
                                                                                    {
                                                                                        if (previousRuleIsQualified == true || currentRuleIsQualified == true)
                                                                                            currentRuleIsQualified = true;
                                                                                        else
                                                                                            currentRuleIsQualified = false;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {

                                                                                    if (previousRuleRC == FrameworkUAD_Lookup.Enums.RuleChain.And)
                                                                                    {
                                                                                        if (previousRuleIsQualified == true && currentRuleIsQualified == true)
                                                                                            currentRuleIsQualified = true;
                                                                                        else
                                                                                            currentRuleIsQualified = false;
                                                                                    }
                                                                                    else if (previousRuleRC == FrameworkUAD_Lookup.Enums.RuleChain.Or)
                                                                                    {
                                                                                        if (previousRuleIsQualified == true || currentRuleIsQualified == true)
                                                                                            currentRuleIsQualified = true;
                                                                                        else
                                                                                            currentRuleIsQualified = false;
                                                                                    }
                                                                                }
                                                                            }
                                                                            #endregion

                                                                            ruleResults[rule.RuleId] = currentRuleIsQualified;
                                                                            if (!ruleIdsProcessed.Contains(currentRuleId))
                                                                                ruleIdsProcessed.Add(currentRuleId);

                                                                            #region is a break RuleChain set??
                                                                            if (currentRuleRC == FrameworkUAD_Lookup.Enums.RuleChain.BreakAlways)
                                                                            {
                                                                                previousRuleIsQualified = currentRuleIsQualified;
                                                                                previousRuleRC = currentRuleRC;
                                                                                previousRuleId = currentRuleId;
                                                                                break;
                                                                            }
                                                                            else if (currentRuleRC == FrameworkUAD_Lookup.Enums.RuleChain.BreakTrue)
                                                                            {
                                                                                if (currentRuleIsQualified == true)
                                                                                {
                                                                                    previousRuleIsQualified = currentRuleIsQualified;
                                                                                    previousRuleRC = currentRuleRC;
                                                                                    previousRuleId = currentRuleId;
                                                                                    break;
                                                                                }
                                                                            }
                                                                            else if (currentRuleRC == FrameworkUAD_Lookup.Enums.RuleChain.BreakFalse)
                                                                            {
                                                                                if (currentRuleIsQualified == false)
                                                                                {
                                                                                    previousRuleIsQualified = currentRuleIsQualified;
                                                                                    previousRuleRC = currentRuleRC;
                                                                                    previousRuleId = currentRuleId;
                                                                                    break;
                                                                                }
                                                                            }
                                                                            //else if (currentRuleRC == FrameworkUAD_Lookup.Enums.RuleChain.Or)
                                                                            //{
                                                                            //    if (previousConditionGroupRC.HasValue && previousConditionGroupRC.Value == FrameworkUAD_Lookup.Enums.RuleChain.And)
                                                                            //    {
                                                                            //        if (previousRuleIsQualified == rgRule.ConditionBreakResult && previousRuleIsQualified == currentRuleIsQualified)
                                                                            //            break;
                                                                            //        else
                                                                            //        {
                                                                            //            if (currentRuleIsQualified == rgRule.ConditionBreakResult)
                                                                            //                break;
                                                                            //        }
                                                                            //    }
                                                                            //}
                                                                            #endregion

                                                                            previousRuleIsQualified = currentRuleIsQualified;
                                                                            previousRuleRC = currentRuleRC;
                                                                            previousRuleId = currentRuleId;
                                                                        }//rule object loop
                                                                    }//ruleId int Loop
                                                                    #endregion
                                                                    //now set currentRuleGroupIsQualified based on last in ruleResults
                                                                    currentRuleGroupIsQualified = ruleResults.Last().Value;

                                                                    //last steps
                                                                    #region set current IsQualified based on previous RuleChain - RuleGroup 1 this is skipped
                                                                    if (previousRuleGroupRC.HasValue && previousRuleGroup > 0)
                                                                    {
                                                                        if (previousRuleGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.And)
                                                                        {
                                                                            if (previousRuleGroupIsQualified == true && currentRuleGroupIsQualified == true)
                                                                                currentRuleGroupIsQualified = true;
                                                                            else
                                                                                currentRuleGroupIsQualified = false;
                                                                        }
                                                                        else if (previousRuleGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.Or)
                                                                        {
                                                                            if (previousRuleGroupIsQualified == true || currentRuleGroupIsQualified == true)
                                                                                currentRuleGroupIsQualified = true;
                                                                            else
                                                                                currentRuleGroupIsQualified = false;
                                                                        }
                                                                    }
                                                                    #endregion

                                                                    ruleGroupResults[rgRule.RuleGroup] = currentRuleGroupIsQualified;
                                                                    if (!ruleGroupsProcessed.Contains(currentRuleGroup))
                                                                        ruleGroupsProcessed.Add(currentRuleGroup);

                                                                    #region is a break RuleChain set??
                                                                    if (currentRuleGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.BreakAlways)
                                                                    {
                                                                        previousRuleGroupIsQualified = currentRuleGroupIsQualified;
                                                                        previousRuleGroupRC = currentRuleGroupRC;
                                                                        previousRuleGroup = currentRuleGroup;
                                                                        breakRuleGroupProcessing = true;
                                                                        break;
                                                                    }
                                                                    else if (currentRuleGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.BreakTrue)
                                                                    {
                                                                        if (currentRuleGroupIsQualified == true)
                                                                        {
                                                                            previousRuleGroupIsQualified = currentRuleGroupIsQualified;
                                                                            previousRuleGroupRC = currentRuleGroupRC;
                                                                            previousRuleGroup = currentRuleGroup;
                                                                            breakRuleGroupProcessing = true;
                                                                            break;
                                                                        }
                                                                    }
                                                                    else if (currentRuleGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.BreakFalse)
                                                                    {
                                                                        if (currentRuleGroupIsQualified == false)
                                                                        {
                                                                            previousRuleGroupIsQualified = currentRuleGroupIsQualified;
                                                                            previousRuleGroupRC = currentRuleGroupRC;
                                                                            previousRuleGroup = currentRuleGroup;
                                                                            breakRuleGroupProcessing = true;
                                                                            break;
                                                                        }
                                                                    }
                                                                    else if (currentRuleGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.And && currentRuleGroupIsQualified == false)//i can do this for Qualified profile becase on our AND predefined rules the right side always must be true
                                                                    {
                                                                        previousRuleGroupIsQualified = currentRuleGroupIsQualified;
                                                                        previousRuleGroupRC = currentRuleGroupRC;
                                                                        previousRuleGroup = currentRuleGroup;
                                                                        breakRuleGroupProcessing = true;
                                                                        break;
                                                                    }
                                                                    //else if (currentRuleGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.Or)
                                                                    //{
                                                                    //    if (previousRuleGroupIsQualified == rgRule.ConditionBreakResult)
                                                                    //    {
                                                                    //        //if previous was an AND make sure previousIsQualifed matches
                                                                    //        if (previousConditionGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.And)
                                                                    //        {
                                                                    //            if (previousRuleGroupIsQualified == rgRule.ConditionBreakResult)
                                                                    //                break;
                                                                    //        }
                                                                    //        else
                                                                    //            break;
                                                                    //    }
                                                                    //}
                                                                    #endregion

                                                                    previousRuleGroupIsQualified = currentRuleGroupIsQualified;
                                                                    previousRuleGroupRC = currentRuleGroupRC;
                                                                    previousRuleGroup = currentRuleGroup;
                                                                }
                                                            }
                                                        }//rg object loop
                                                    }
                                                }//rgIntLoop
                                                #endregion

                                                //now set currentRuleGroupIsQualified based on last in ruleResults
                                                currentCondGroupIsQualified = ruleGroupResults.Last().Value;

                                                #region set current IsQualified based on previous RuleChain - RuleGroup 1 this is skipped
                                                if (previousConditionGroupRC.HasValue && previousConditionGroup > 0)
                                                {
                                                    if (previousConditionGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.And)
                                                    {
                                                        if (previousCondGroupIsQualified == true && currentCondGroupIsQualified == true)
                                                            currentCondGroupIsQualified = true;
                                                        else
                                                            currentCondGroupIsQualified = false;
                                                    }
                                                    else if (previousConditionGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.Or)
                                                    {
                                                        if (previousCondGroupIsQualified == true || currentCondGroupIsQualified == true)
                                                            currentCondGroupIsQualified = true;
                                                        else
                                                            currentCondGroupIsQualified = false;
                                                    }
                                                }
                                                #endregion

                                                conditionGroupResults[cgRule.ConditionGroup] = currentCondGroupIsQualified;
                                                if (!conditionGroupsProcessed.Contains(cgRule.ConditionGroup))
                                                    conditionGroupsProcessed.Add(cgRule.ConditionGroup);

                                                #region is a break RuleChain set??
                                                if (currentConditionGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.BreakAlways)
                                                {
                                                    previousCondGroupIsQualified = currentCondGroupIsQualified;
                                                    previousConditionGroupRC = currentConditionGroupRC;
                                                    previousConditionGroup = currentConditionGroup;
                                                    breakConditionProcessing = true;
                                                    break;
                                                }
                                                else if (currentConditionGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.BreakTrue)
                                                {
                                                    if (currentCondGroupIsQualified == true)
                                                    {
                                                        previousCondGroupIsQualified = currentCondGroupIsQualified;
                                                        previousConditionGroupRC = currentConditionGroupRC;
                                                        previousConditionGroup = currentConditionGroup;
                                                        breakConditionProcessing = true;
                                                        break;
                                                    }
                                                }
                                                else if (currentConditionGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.BreakFalse)//if qualname is false should break and skip address 
                                                {
                                                    if (currentCondGroupIsQualified == false)
                                                    {
                                                        previousCondGroupIsQualified = currentCondGroupIsQualified;
                                                        previousConditionGroupRC = currentConditionGroupRC;
                                                        previousConditionGroup = currentConditionGroup;
                                                        breakConditionProcessing = true;
                                                        break;
                                                    }
                                                }
                                                else if (currentConditionGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.And && currentCondGroupIsQualified == true)//i can do this for Qualified profile because if any predefined condition is true then can stop
                                                {
                                                    previousCondGroupIsQualified = currentCondGroupIsQualified;
                                                    previousConditionGroupRC = currentConditionGroupRC;
                                                    previousConditionGroup = currentConditionGroup;
                                                    breakConditionProcessing = true;
                                                    break;
                                                }
                                                //else if (currentConditionGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.Or)
                                                //{
                                                //    if (previousCondGroupIsQualified == cgRule.ConditionBreakResult)
                                                //    {
                                                //        //if previous was an AND make sure previousIsQualifed matches
                                                //        if (previousConditionGroupRC == FrameworkUAD_Lookup.Enums.RuleChain.And)
                                                //        {
                                                //            if (previousCondGroupIsQualified == cgRule.ConditionBreakResult)
                                                //                break;
                                                //        }
                                                //        else
                                                //            break;
                                                //    }
                                                //}
                                                #endregion

                                                previousCondGroupIsQualified = currentCondGroupIsQualified;
                                                previousConditionGroupRC = currentConditionGroupRC;
                                                previousConditionGroup = currentConditionGroup;
                                            }
                                        }
                                    }//cg object loop
                                    #endregion
                                }
                            }
                        }//cgIntLoop
                        #endregion

                        //check each conditionGroup - if any resulted in true then profile is good
                        if (conditionGroupResults.ContainsValue(true)) qualified.Add(t);
                        else
                        {
                            String msg = String.Format(sb.ToString() + "@ Row {0}", t.OriginalImportRow);
                            dataIV = ValidationErrorHashSet(dataIV, msg, dft, admsLog, t.OriginalImportRow, t);
                        }
                    }
                    #endregion
                }
                #endregion
            }

            return qualified;

            #region old
            // HashSet<SubscriberTransformed> qualified = new HashSet<SubscriberTransformed>();
            //foreach (SubscriberTransformed st in checkList)
            //{
            //    bool isQualified = true;
            //    bool hasName = HasQualifiedName(st);
            //    bool hasAddress = HasQualifiedAddress(st);
            //    bool hasPhone = HasQualifiedPhone(st);
            //    bool hasCompany = HasQualifiedCompany(st);
            //    bool hasTitle = HasQualifiedTitle(st);
            //    StringDictionary row = dataIV.DataTransformed[st.ImportRowNumber];
            //    int rowIndex = st.ImportRowNumber;

            //    if (hasName == true)
            //    {
            //        if (hasAddress == true || hasPhone == true || hasCompany == true)
            //            isQualified = true;
            //        else
            //        {
            //            isQualified = false;
            //            StringBuilder sb = new StringBuilder();
            //            if (hasAddress == false) sb.Append("Address is not present ");
            //            if (hasPhone == false) sb.Append("Phone is not present ");
            //            if (hasCompany == false) sb.Append("Company is not present ");
            //            String msg = String.Format(sb.ToString() + "@ Row {0}", rowIndex);
            //            isQualified = false;
            //            dataIV = ValidationError(dataIV, msg, dft, admsLog, rowIndex, row);
            //        }
            //    }
            //    else if (hasTitle == true && hasCompany == true)
            //    {
            //        if (hasAddress == true)
            //            isQualified = true;
            //        else
            //        {
            //            isQualified = false;
            //            StringBuilder sb = new StringBuilder();
            //            if (hasAddress == false) sb.Append("Address is not present ");
            //            if (hasCompany == false) sb.Append("Company is not present ");
            //            if (hasTitle == false) sb.Append("Title is not present ");
            //            String msg = String.Format(sb.ToString() + "@ Row {0}", rowIndex);
            //            isQualified = false;
            //            dataIV = ValidationError(dataIV, msg, dft, admsLog, rowIndex, row);
            //        }
            //    }
            //    else
            //    {
            //        isQualified = false;
            //        String msg = String.Format("Name or Title and Company is not present for the profile @ Row {0}", rowIndex);
            //        isQualified = false;
            //        dataIV = ValidationError(dataIV, msg, dft, admsLog, rowIndex, row);
            //    }

            //    if (isQualified)
            //        qualified.Add(st);
            //    else
            //    {
            //        string msg = String.Format("Row missing Subscriber Profile and Email @ Row {0}, you must have one or the other", rowIndex);
            //        ConsoleMessage(msg);
            //    }

            //}
            //return qualified;
            #endregion
        }

        private bool SetRuleIsQualified(
            SubscriberTransformed subscriber, 
            StringBuilder sbRules,
            RuleObject rule,
            string ruleName,
            bool isQualified)
        {
            Guard.NotNull(subscriber, nameof(subscriber));
            Guard.NotNull(sbRules, nameof(sbRules));
            Guard.NotNull(rule, nameof(rule));

            var subscriberField = ruleName.Equals(CompanyKey) 
                ? subscriber.Company 
                : subscriber.Title;

            foreach (var ruleValue in rule.RuleValues)
            {
                if (ruleValue.Value == ruleName)
                {
                    if (string.IsNullOrWhiteSpace(subscriberField))
                    {
                        isQualified = false;
                        sbRules.Append($"{ruleName} is missing. ");
                    }
                    else
                    {
                        isQualified = true;
                    }
                }
            }

            return isQualified;
        }

        private void ProcessRuleObject(FrameworkUAS.Object.Rule r)
        {

        }
       
        #region Exception Logging
        private FrameworkUAD.Object.ImportFile LogException(FrameworkUAD.Object.ImportFile dataIV, Exception ex, int rowNumber, StringDictionary rowData, string dbFileType, FrameworkUAS.Entity.AdmsLog admsLog, FrameworkUAD_Lookup.Enums.FileTypes dft, bool sendEmail = false)
        {
            string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
            fl.Save(new FrameworkUAS.Entity.FileLog(admsLog.SourceFileId, -99, message, admsLog.ProcessCode));

            StringBuilder sbDetail = new StringBuilder();
            sbDetail.AppendLine("ClientID: " + client.ClientID.ToString());
            sbDetail.AppendLine("SourceFileID: " + admsLog.SourceFileId.ToString());
            sbDetail.AppendLine("ProcessCode: " + admsLog.ProcessCode.ToString());
            sbDetail.AppendLine("Row Number: " + rowNumber.ToString());
            sbDetail.AppendLine(System.Environment.NewLine);

            if (sendEmail == true)
            {
                Emailer.Emailer emWorker = new Emailer.Emailer();
                emWorker.EmailException(ex, sbDetail.ToString());
            }

            sbDetail.AppendLine(message);
            dataIV = ValidationError(dataIV, sbDetail.ToString(), dft, admsLog, rowNumber, rowData);
            return dataIV;
        }
        private FrameworkUAD.Object.ImportFile ValidationError(FrameworkUAD.Object.ImportFile dataIV, string errorMsg, FrameworkUAD_Lookup.Enums.FileTypes dft, FrameworkUAS.Entity.AdmsLog admsLog, int rowNumber = 0, StringDictionary rowData = null)
        {
            var importErrorEntity = ValidatorMethods.CreateImportErrorEntity(dataIV, rowData, rowNumber, errorMsg, false);

            ConsoleMessage(errorMsg);

            var check = ValidatorMethods.SetImportErrorsValue(dataIV, importErrorEntity);

            if ((dataIV.ImportErrorCount >= 100000 || check >= 25) &&
                (dft != FrameworkUAD_Lookup.Enums.FileTypes.Field_Update
                    && dft != FrameworkUAD_Lookup.Enums.FileTypes.QuickFill
                    && dft != FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction
                    && dft != FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms
                    && dft != FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms_Short_Form))
            {
                //send some email that the processing of this file has stopped due to number of error encountered. 
                //100k errors or 25% of records in error state.
                Emailer.Emailer em = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.RejectFileErrorLimitExceeded);
                em.RejectFileErrorLimitExceeded(dataIV);

                ConsoleMessage("Rejecting file due to error threshold - will delete file from repository then shut down current engine and start new instance - " + DateTime.Now.TimeOfDay.ToString(), dataIV.ProcessCode, true, dataIV.SourceFileId);

                KMPlatform.BusinessLogic.Client cworker = new KMPlatform.BusinessLogic.Client();
                KMPlatform.Entity.Client client = cworker.Select(dataIV.ClientId);

                ThreadDictionary.Remove(dataIV.ThreadId);
                RemoveFileFromRepository(dataIV, admsLog, client);
                Environment.Exit(DefaultExitCode);
                return null;
            }
            else
                return dataIV;
        }
        private FrameworkUAD.Object.ImportFile ValidationErrorHashSet(FrameworkUAD.Object.ImportFile dataIV, string errorMsg, FrameworkUAD_Lookup.Enums.FileTypes dft, FrameworkUAS.Entity.AdmsLog admsLog, int rowNumber = 0, SubscriberTransformed rowData = null)
        {
            FrameworkUAD.Entity.ImportError ie = new ImportError();
            ie.RowNumber = rowNumber;

            if (dataIV.ImportErrors == null)
                dataIV.ImportErrors = new HashSet<ImportError>();
            dataIV.HasError = true;
            if (dataIV.ImportErrors.FirstOrDefault(x => x.RowNumber == rowData.OriginalImportRow) == null)
            {
                dataIV.ImportErrorCount++;
                dataIV.ImportErrors.Add(ie);
                StringBuilder sb = new StringBuilder();

                sb.Append(rowData.FName + ", ");
                sb.Append(rowData.LName + ", ");
                sb.Append(rowData.Title + ", ");
                sb.Append(rowData.Company + ", ");
                sb.Append(rowData.Address + ", ");
                sb.Append(rowData.MailStop + ", ");
                sb.Append(rowData.Address3 + ", ");
                sb.Append(rowData.Email + ", ");
                sb.Append(rowData.Phone + ", ");

                ie.BadDataRow = sb.ToString().Trim();//.TrimEnd(',');
                sb = sb.Clear();

                errorMsg = " " + errorMsg;
                string[] errors = errorMsg.Split('.');
                errors = errors.Distinct().ToArray();

                foreach (string i in errors)
                {
                    sb.Append(i + ". ");
                }

                ie.ClientMessage = sb.ToString().Trim();


                ConsoleMessage(sb.ToString().Trim());
            }
            int check = 0;
            if (dataIV.DataTransformed.Count > 0)
                check = (int) (((decimal) dataIV.ImportErrorCount / (decimal) dataIV.DataTransformed.Count) * 100);
            if ((dataIV.ImportErrorCount >= 100000 || check >= 25) &&
                (dft != FrameworkUAD_Lookup.Enums.FileTypes.Field_Update
                    && dft != FrameworkUAD_Lookup.Enums.FileTypes.QuickFill
                    && dft != FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction
                    && dft != FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms
                    && dft != FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms_Short_Form))
            {
                //send some email that the processing of this file has stopped due to number of error encountered. 
                //100k errors or 25% of records in error state.
                Emailer.Emailer em = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.RejectFileErrorLimitExceeded);
                em.RejectFileErrorLimitExceeded(dataIV);

                ConsoleMessage("Rejecting file due to error threshold - will delete file from repository then shut down current engine and start new instance - " + DateTime.Now.TimeOfDay.ToString(), dataIV.ProcessCode, true, dataIV.SourceFileId);

                KMPlatform.BusinessLogic.Client cworker = new KMPlatform.BusinessLogic.Client();
                KMPlatform.Entity.Client client = cworker.Select(dataIV.ClientId);

                ThreadDictionary.Remove(dataIV.ThreadId);
                RemoveFileFromRepository(dataIV, admsLog, client);
                Environment.Exit(DefaultExitCode);
                return null;
            }
            else
                return dataIV;
        }

        private void RemoveFileFromRepository(ImportFile dataIV, AdmsLog admsLog, ClientEntity client)
        {
            Guard.NotNull(dataIV, nameof(dataIV));
            Guard.NotNull(admsLog, nameof(admsLog));
            Guard.NotNull(client, nameof(client));

            var clientRepo = $"{BaseDirs.createDirectory(BaseDirs.getClientArchiveDir(), client.FtpFolder)}{InvalidPath}";
            var formatForFile = DefaultDateTimeFormat;
            var dateForFile = DateTime.Now.ToString(formatForFile);

            if (File.Exists(dataIV.ProcessFile.FullName))
            {
                var replacedName = dataIV.ProcessFile.Name.Replace(dataIV.ProcessFile.Extension, $"{Underline}{dateForFile}");
                File.Move(dataIV.ProcessFile.FullName, $"{clientRepo}{replacedName}{dataIV.ProcessFile.Extension}");
            }

            admsWrk.UpdateFileEnd(admsLog.ProcessCode, DateTime.Now, 1, admsLog.SourceFileId);
        }

        #endregion

        #region Service
        public FrameworkUAD.ServiceResponse.SavedSubscriber SavePaidSubscriber(KMPlatform.Entity.Client client, FrameworkUAD.Object.PaidSubscription subscription)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            FrameworkUAD.ServiceResponse.SavedSubscriber savedSubscriber = new FrameworkUAD.ServiceResponse.SavedSubscriber();
            if (subscription != null)
            {
                savedSubscriber.ProcessCode = Core_AMS.Utilities.StringFunctions.GenerateProcessCode();
                string msg = DateTime.Now.TimeOfDay.ToString() + " Entered SavePaidSubscriber service method" + " client: " + client.FtpFolder;
                ConsoleMessage(msg, savedSubscriber.ProcessCode, true);

                //get products for clients
                FrameworkUAD.BusinessLogic.Product prodData = new FrameworkUAD.BusinessLogic.Product();
                List<FrameworkUAD.Entity.Product> products = prodData.Select(client.ClientConnections);

                List<FrameworkUAD.Entity.EmailStatus> statuses = new List<EmailStatus>();
                FrameworkUAD.BusinessLogic.EmailStatus workerES = new FrameworkUAD.BusinessLogic.EmailStatus();
                statuses = workerES.Select(client.ClientConnections);
            }

            return savedSubscriber;
        }
        public FrameworkUAD.ServiceResponse.SavedSubscriber SaveSubscriber(KMPlatform.Entity.Client client, FrameworkUAD.Object.SaveSubscriber subscription)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            FrameworkUAD.ServiceResponse.SavedSubscriber savedSubscriber = new FrameworkUAD.ServiceResponse.SavedSubscriber();
            if (subscription != null)
            {
                savedSubscriber.ProcessCode = Core_AMS.Utilities.StringFunctions.GenerateProcessCode();
                string msg = DateTime.Now.TimeOfDay.ToString() + " Entered SaveSubscriber service method" + " client: " + client.FtpFolder;
                ConsoleMessage(msg, savedSubscriber.ProcessCode, true);

                //get products for clients
                FrameworkUAD.BusinessLogic.Product prodData = new FrameworkUAD.BusinessLogic.Product();
                List<FrameworkUAD.Entity.Product> products = prodData.Select(client.ClientConnections);

                List<FrameworkUAD.Entity.EmailStatus> statuses = new List<EmailStatus>();
                FrameworkUAD.BusinessLogic.EmailStatus workerES = new FrameworkUAD.BusinessLogic.EmailStatus();
                statuses = workerES.Select(client.ClientConnections);

                int sfID = 0;
                try
                {
                    bool isDemo = true;
                    bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IsDemo"].ToString(), out isDemo);

                    //null checks
                    if (subscription.Products == null)
                        subscription.Products = new List<FrameworkUAD.Object.ProductSubscription>();

                    foreach (var p in subscription.Products)
                    {
                        if (p.SubscriberProductDemographics == null)
                            p.SubscriberProductDemographics = new List<FrameworkUAD.Object.SubscriberProductDemographic>();
                    }

                    //make sure client has Custom Props
                    KMPlatform.BusinessLogic.Client cData = new KMPlatform.BusinessLogic.Client();
                    client = cData.Select(client.ClientID, true);

                    //SourceFileID from SourceFile table by clientID where FileName = UAD_WS_AddSubscriber
                    FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
                    FrameworkUAS.Entity.SourceFile sourceFile = sfData.Select(client.ClientID, "UAD_WS_AddSubscriber");
                    if (sourceFile == null)
                    {
                        FrameworkUAD_Lookup.BusinessLogic.Code cworker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                        List<FrameworkUAD_Lookup.Entity.Code> codes = cworker.Select(FrameworkUAD_Lookup.Enums.CodeType.File_Recurrence);
                        List<FrameworkUAD_Lookup.Entity.Code> dbFiletypes = cworker.Select(FrameworkUAD_Lookup.Enums.CodeType.Database_File);

                        KMPlatform.BusinessLogic.Service sworker = new KMPlatform.BusinessLogic.Service();
                        KMPlatform.Entity.Service s = sworker.Select(KMPlatform.Enums.Services.UADFILEMAPPER, true);

                        //create the UAD_WS_AddSubscriber file for the client
                        sourceFile = new SourceFile();
                        sourceFile.FileRecurrenceTypeId = 0;
                        if (codes.Exists(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FileRecurrenceTypes.Recurring.ToString())))
                            sourceFile.FileRecurrenceTypeId = codes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FileRecurrenceTypes.Recurring.ToString())).CodeId;
                        sourceFile.DatabaseFileTypeId = dbFiletypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.GetDatabaseFileType(FrameworkUAD_Lookup.Enums.FileTypes.Audience_Data.ToString()))).CodeId;
                        sourceFile.FileName = "UAD_WS_AddSubscriber";
                        sourceFile.ClientID = client.ClientID;
                        sourceFile.IsDQMReady = true;
                        sourceFile.ServiceID = s.ServiceID;
                        sourceFile.ServiceFeatureID = 0;
                        if (s.ServiceFeatures.Exists(x => x.SFName.Equals(KMPlatform.BusinessLogic.Enums.UADFeatures.UAD_Api.ToString().Replace("_", " "))))
                            sourceFile.ServiceFeatureID = s.ServiceFeatures.Single(x => x.SFName.Equals(KMPlatform.BusinessLogic.Enums.UADFeatures.UAD_Api.ToString().Replace("_", " "))).ServiceFeatureID;
                        sourceFile.CreatedByUserID = 1;
                        sourceFile.QDateFormat = "MMDDYYYY";
                        sourceFile.BatchSize = 2500;
                        sourceFile.SourceFileID = sfData.Save(sourceFile);
                    }
                    sfID = sourceFile.SourceFileID;

                    var admsLog = SaveAdmsLog(client, savedSubscriber, sourceFile);

                    admsWrk.Update(admsLog.ProcessCode,
                                             FrameworkUAD_Lookup.Enums.FileStatusType.ApiProcessing,
                                             FrameworkUAD_Lookup.Enums.ADMS_StepType.Validator_Api_Validation,
                                             FrameworkUAD_Lookup.Enums.ProcessingStatusType.In_ApiValidation,
                                             FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_ApiValidation, 1,
                                             "Start: api data validation " + DateTime.Now.TimeOfDay.ToString(), true,
                                             admsLog.SourceFileId);


                    //subscription object coming in will have the PubCode set but will not have the PubID set - lets set that
                    savedSubscriber = SetProductID(savedSubscriber, ref subscription, products, threadId);
                    //now our PubID is set and if Pubcode does not exist in the database we removed those records
                    if (savedSubscriber.IsPubCodeValid == true)
                    {
                        //validate subscriber - ProductDetail to codesheet
                        //here we will match product value to codesheet values if not match will remove record
                        savedSubscriber = ValidateProductSubscription(savedSubscriber, ref subscription, client, statuses, threadId);

                        //validate that any incoming SubscriberConsensusDemographic items exist - remove those that do not exist
                        ValidateConsensusDemographics(ref subscription, client, savedSubscriber.ProcessCode);

                        //insert to SubscriberOriginal & Demo --> SubscriberTransformed and STDemographics
                        savedSubscriber = SaveSubscriberTransformed(savedSubscriber, client, subscription, sourceFile, savedSubscriber.ProcessCode);
                        //result.AppendLine("Save complete at " + DateTime.Now.ToString());
                        if (savedSubscriber.IsProductSubscriberCreated == true)
                        {
                            //call DQMCleanse which then runs DQM and calls UAD to move new subscriber to UAD 
                            admsWrk.Update(admsLog.ProcessCode,
                                             FrameworkUAD_Lookup.Enums.FileStatusType.ApiProcessing,
                                             FrameworkUAD_Lookup.Enums.ADMS_StepType.Validator_Api_End,
                                             FrameworkUAD_Lookup.Enums.ProcessingStatusType.ApiValidated,
                                             FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_ApiSaveSubscriber, 1,
                                             "End: api data validation " + DateTime.Now.TimeOfDay.ToString(), true,
                                             admsLog.SourceFileId);

                            //call Address Validation first
                            DataCleanser.AddressClean ac = new DataCleanser.AddressClean();
                            ac.ExecuteAddressCleanse(admsLog, client);

                            //Add to DQMQue 
                            DQMQue q = new DQMQue(savedSubscriber.ProcessCode, client.ClientID, isDemo, false, sfID);
                            FrameworkUAS.BusinessLogic.DQMQue dqmWorker = new FrameworkUAS.BusinessLogic.DQMQue();
                            dqmWorker.Save(q);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".SaveSubscriber");
                }
            }
            return savedSubscriber;
        }

        private AdmsLog SaveAdmsLog(ClientEntity clientEntity, ServiceSavedSubscriber savedSubscriber, SourceFile sourceFile)
        {
            Guard.NotNull(clientEntity, nameof(clientEntity));
            Guard.NotNull(savedSubscriber, nameof(savedSubscriber));
            Guard.NotNull(sourceFile, nameof(sourceFile));

            var admsLog = new AdmsLog
            {
                ClientId = clientEntity.ClientID,
                StatusMessage = LookupEnums.FileStatusType.Detected.ToString(),
                AdmsStepId = 0,
                DateCreated = DateTime.Now,
                FileLogDetails = new List<FileLog>(),
                FileNameExact = "UAD_WS_AddSubscriber",
                FileStart = DateTime.Now,
                FileStatusId = 0,
                ImportFile = null,
                ProcessCode = savedSubscriber.ProcessCode,
                ProcessingStatusId = 0,
                ExecutionPointId = 0,
                RecordSource = "API",
                SourceFileId = sourceFile.SourceFileID,
                ThreadId = Thread.CurrentThread.ManagedThreadId
            };

            var alWrk = new AdmsLogBusiness();
            admsLog.AdmsLogId = alWrk.Save(admsLog);
            return admsLog;
        }

        public FrameworkUAD.ServiceResponse.SavedSubscriber SaveSubscribers(KMPlatform.Entity.Client client, List<FrameworkUAD.Object.SaveSubscriber> subscriptions, bool isCirculation = false)
        {
            FrameworkUAD.ServiceResponse.SavedSubscriber savedSubscriber = new FrameworkUAD.ServiceResponse.SavedSubscriber();
            savedSubscriber.ProcessCode = Core_AMS.Utilities.StringFunctions.GenerateProcessCode();
            string msg = DateTime.Now.TimeOfDay.ToString() + " Entered SaveSubscribers service method" + " client: " + client.FtpFolder;
            ConsoleMessage(msg, savedSubscriber.ProcessCode, true);
            int threadID = Thread.CurrentThread.ManagedThreadId;

            //make sure client has Custom Props
            KMPlatform.BusinessLogic.Client cData = new KMPlatform.BusinessLogic.Client();
            client = cData.Select(client.ClientID, true);

            //SourceFileID from SourceFile table by clientID where FileName = UAD_WS_AddSubscriber
            FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
            FrameworkUAS.Entity.SourceFile sourceFile = new SourceFile();
            if (isCirculation == false)
                sourceFile = sfData.Select(client.ClientID, "UAD_WS_AddSubscriber");
            else
                sourceFile = sfData.Select(client.ClientID, "CIRC_WS_AddSubscriber");

            if (sourceFile == null)
            {
                string fileName = "UAD_WS_AddSubscriber";
                if (isCirculation == true)
                    fileName = "CIRC_WS_AddSubscriber";

                FrameworkUAD_Lookup.BusinessLogic.Code cworker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                List<FrameworkUAD_Lookup.Entity.Code> codes = cworker.Select(FrameworkUAD_Lookup.Enums.CodeType.File_Recurrence);
                List<FrameworkUAD_Lookup.Entity.Code> dbFiletypes = cworker.Select(FrameworkUAD_Lookup.Enums.CodeType.Database_File);

                KMPlatform.BusinessLogic.Service sworker = new KMPlatform.BusinessLogic.Service();
                KMPlatform.Entity.Service s = sworker.Select(KMPlatform.Enums.Services.UADFILEMAPPER, true);
                if (isCirculation == true)
                    s = sworker.Select(KMPlatform.Enums.Services.CIRCFILEMAPPER, true);

                int sfId = s.ServiceFeatures.Single(x => x.SFName.Equals(KMPlatform.BusinessLogic.Enums.UADFeatures.UAD_Api.ToString().Replace("_", " "))).ServiceFeatureID;
                if (isCirculation == true)
                    sfId = s.ServiceFeatures.Single(x => x.SFName.Equals(KMPlatform.BusinessLogic.Enums.FulfillmentFeatures.File_Import.ToString().Replace("_", " "))).ServiceFeatureID;

                //create the UAD_WS_AddSubscriber file for the client
                sourceFile = new SourceFile();
                sourceFile.FileRecurrenceTypeId = codes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FileRecurrenceTypes.Recurring.ToString())).CodeId;
                sourceFile.DatabaseFileTypeId = dbFiletypes.Single(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.GetDatabaseFileType(FrameworkUAD_Lookup.Enums.FileTypes.Audience_Data.ToString()))).CodeId;
                sourceFile.FileName = fileName;
                sourceFile.ClientID = client.ClientID;
                sourceFile.IsDQMReady = true;
                sourceFile.ServiceID = s.ServiceID;
                sourceFile.ServiceFeatureID = sfId;
                sourceFile.CreatedByUserID = 1;
                sourceFile.QDateFormat = "MMDDYYYY";
                sourceFile.BatchSize = 2500;
                sourceFile.SourceFileID = sfData.Save(sourceFile);
            }

            var admsLog = SaveAdmsLog(client, savedSubscriber, sourceFile);

            //get products for clients
            FrameworkUAD.BusinessLogic.Product prodData = new FrameworkUAD.BusinessLogic.Product();
            List<FrameworkUAD.Entity.Product> products = prodData.Select(client.ClientConnections);

            List<FrameworkUAD.Entity.EmailStatus> statuses = new List<EmailStatus>();
            FrameworkUAD.BusinessLogic.EmailStatus workerES = new FrameworkUAD.BusinessLogic.EmailStatus();
            statuses = workerES.Select(client.ClientConnections);

            foreach (FrameworkUAD.Object.SaveSubscriber subscription in subscriptions)
            {
                try
                {
                    bool isDemo = true;
                    bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IsDemo"].ToString(), out isDemo);

                    //Add to DQMQue
                    DQMQue q = new DQMQue(savedSubscriber.ProcessCode, client.ClientID, isDemo, false, sourceFile.SourceFileID);
                    FrameworkUAS.BusinessLogic.DQMQue dqmWorker = new FrameworkUAS.BusinessLogic.DQMQue();
                    dqmWorker.Save(q);
                    admsLog.DQM = q;

                    admsWrk.Update(admsLog.ProcessCode,
                                             FrameworkUAD_Lookup.Enums.FileStatusType.ApiProcessing,
                                             FrameworkUAD_Lookup.Enums.ADMS_StepType.Validator_Api_End,
                                             FrameworkUAD_Lookup.Enums.ProcessingStatusType.ApiValidated,
                                             FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_ApiSaveSubscriber, 1,
                                             "End: api data validation " + DateTime.Now.TimeOfDay.ToString(), true,
                                             admsLog.SourceFileId);

                    //null checks
                    if (subscription.Products == null)
                        subscription.Products = new List<FrameworkUAD.Object.ProductSubscription>();

                    foreach (var p in subscription.Products)
                    {
                        if (p.SubscriberProductDemographics == null)
                            p.SubscriberProductDemographics = new List<FrameworkUAD.Object.SubscriberProductDemographic>();
                    }

                    //subscription object coming in will have the PubCode set but will not have the PubID set - lets set that
                    FrameworkUAD.Object.SaveSubscriber mySub = subscription;

                    savedSubscriber = SetProductID(savedSubscriber, ref mySub, products, threadID);
                    //now our PubID is set and if Pubcode does not exist in the database we removed those records
                    if (savedSubscriber.IsPubCodeValid == true)
                    {
                        //validate subscriber - ProductDetail to codesheet
                        //here we will match product value to codesheet values if not match will remove record
                        savedSubscriber = ValidateProductSubscription(savedSubscriber, ref mySub, client, statuses, threadID);

                        //insert to SubscriberOriginal & Demo --> SubscriberTransformed and STDemographics
                        savedSubscriber = SaveSubscriberTransformed(savedSubscriber, client, mySub, sourceFile, savedSubscriber.ProcessCode);
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".SaveSubscriber");
                }
            }

            admsLog = new AdmsLog();
            admsLog.ProcessCode = savedSubscriber.ProcessCode;
            admsLog.SourceFileId = sourceFile.SourceFileID;
            admsLog.ClientId = sourceFile.ClientID;

            //call Address Validation first
            DataCleanser.AddressClean ac = new DataCleanser.AddressClean();
            ac.ExecuteAddressCleanse(admsLog, client);

            return savedSubscriber;
        }
        private FrameworkUAD.ServiceResponse.SavedSubscriber SetProductID(FrameworkUAD.ServiceResponse.SavedSubscriber response, ref FrameworkUAD.Object.SaveSubscriber subscription, List<FrameworkUAD.Entity.Product> products, int threadID)
        {
            try
            {
                var notExistPubCode = new List<FrameworkUAD.Object.ProductSubscription>();
                var badPubCode = new List<string>();
                foreach (FrameworkUAD.Object.ProductSubscription ps in subscription.Products)
                {
                    if (!string.IsNullOrEmpty(ps.PubCode))
                    {
                        if (products.Exists(x => x.PubCode.Equals(ps.PubCode, StringComparison.CurrentCultureIgnoreCase)))
                            ps.PubID = products.Single(x => x.PubCode.Equals(ps.PubCode, StringComparison.CurrentCultureIgnoreCase)).PubID;
                        else
                        {
                            notExistPubCode.Add(ps);
                            if (!badPubCode.Contains(ps.PubCode))
                                badPubCode.Add(ps.PubCode);
                        }
                    }
                    else
                    {
                        notExistPubCode.Add(ps);
                        if (!badPubCode.Contains("xx--BLANK--xx"))
                            badPubCode.Add("xx--BLANK--xx");
                    }
                }

                if (badPubCode.Count > 0)
                {
                    response.IsPubCodeValid = false;
                    if (badPubCode.Count == 1)
                    {
                        response.PubCodeMessage += "An invalid PubCode was detected." + System.Environment.NewLine;
                        response.PubCodeMessage += "Invalid PubCode: " + badPubCode.First().ToString() + System.Environment.NewLine;
                    }
                    else
                    {
                        response.PubCodeMessage += "Invalid PubCodes were detected as follows." + System.Environment.NewLine;
                        foreach (string pc in badPubCode)
                        {
                            response.PubCodeMessage += "PubCode: " + pc + System.Environment.NewLine;
                        }
                    }

                    foreach (FrameworkUAD.Object.ProductSubscription ps in notExistPubCode)
                        subscription.Products.Remove(ps);
                }
                else
                {
                    response.IsPubCodeValid = true;
                    response.PubCodeMessage += "All PubCodes are valid." + System.Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".SetProductID");
            }
            return response;
        }
        private FrameworkUAD.ServiceResponse.SavedSubscriber ValidateProductSubscription(FrameworkUAD.ServiceResponse.SavedSubscriber response, ref FrameworkUAD.Object.SaveSubscriber subscription, KMPlatform.Entity.Client client, List<FrameworkUAD.Entity.EmailStatus> statuses, int threadID)
        {
            try
            {
                //validate subscriber - ProductDetail to codesheet
                //get distinct ps.PubID from subscription.Products
                //get CodeSheet from UAD for each PubID
                List<int> distinctPubIDs = new List<int>();
                if (subscription.Products != null)
                {
                    foreach (FrameworkUAD.Object.ProductSubscription ps in subscription.Products)
                    {
                        //if (ps.Status != null)// && !string.IsNullOrEmpty(ps.Status))
                        //{
                        //    if (statuses.Exists(x => x.Status.Equals(ps.Status.ToString(), StringComparison.CurrentCultureIgnoreCase)))
                        //        ps.EmailStatusID = statuses.SingleOrDefault(x => x.Status.Equals(ps.Status.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID;
                        //}
                        //else
                        //{
                        //    if (statuses.Exists(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)))
                        //        ps.EmailStatusID = statuses.SingleOrDefault(x => x.Status.Equals(FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString(), StringComparison.CurrentCultureIgnoreCase)).EmailStatusID;
                        //}
                        if (ps != null)
                        {
                            if (!distinctPubIDs.Contains(ps.PubID))
                                distinctPubIDs.Add(ps.PubID);
                        }
                    }
                }

                List<FrameworkUAD.Entity.CodeSheet> allCS = new List<FrameworkUAD.Entity.CodeSheet>();
                FrameworkUAD.BusinessLogic.CodeSheet csData = new FrameworkUAD.BusinessLogic.CodeSheet();
                foreach (int pubID in distinctPubIDs)
                {
                    allCS.AddRange(csData.Select(pubID, client.ClientConnections));
                }

                FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper psemWorker = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper();
                List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> psemList = psemWorker.SelectAll(client.ClientConnections);

                List<FrameworkUAD.Object.SubscriberProductDemographic> badDemos = new List<FrameworkUAD.Object.SubscriberProductDemographic>();
                if (subscription.Products != null)
                {
                    foreach (FrameworkUAD.Object.ProductSubscription ps in subscription.Products)
                    {
                        if (ps.SubscriberProductDemographics != null)
                        {
                            foreach (FrameworkUAD.Object.SubscriberProductDemographic d in ps.SubscriberProductDemographics)
                            {
                                //need to handle leading 0 on single digit numbers
                                if (!string.IsNullOrEmpty(d.Name) && !string.IsNullOrEmpty(d.Value)
                                    && !psemList.Exists(x => x.CustomField.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    if (d.Value == "0"
                                        || d.Value == "1"
                                        || d.Value == "2"
                                        || d.Value == "3"
                                        || d.Value == "4"
                                        || d.Value == "5"
                                        || d.Value == "6"
                                        || d.Value == "7"
                                        || d.Value == "8"
                                        || d.Value == "9")
                                    {
                                        if ((!allCS.Exists(x => x.PubID == ps.PubID &&
                                                          x.ResponseGroup.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                          x.ResponseValue.Equals(d.Value, StringComparison.CurrentCultureIgnoreCase)))
                                            &&
                                            (!allCS.Exists(x => x.PubID == ps.PubID &&
                                                          x.ResponseGroup.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                          x.ResponseValue.Equals("0" + d.Value, StringComparison.CurrentCultureIgnoreCase)))

                                            )
                                        {
                                            //bad
                                            badDemos.Add(d);
                                        }
                                    }
                                    else if (d.Value == "00"
                                        || d.Value == "01"
                                        || d.Value == "02"
                                        || d.Value == "03"
                                        || d.Value == "04"
                                        || d.Value == "05"
                                        || d.Value == "06"
                                        || d.Value == "07"
                                        || d.Value == "08"
                                        || d.Value == "09")
                                    {
                                        if ((!allCS.Exists(x => x.PubID == ps.PubID &&
                                                          x.ResponseGroup.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                          x.ResponseValue.Equals(d.Value.Replace("0", "").ToString(), StringComparison.CurrentCultureIgnoreCase)))
                                            &&
                                            (!allCS.Exists(x => x.PubID == ps.PubID &&
                                                          x.ResponseGroup.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                          x.ResponseValue.Equals(d.Value, StringComparison.CurrentCultureIgnoreCase)))

                                            )
                                        {
                                            //bad
                                            badDemos.Add(d);
                                        }
                                    }
                                    else if (!allCS.Exists(x => x.PubID == ps.PubID &&
                                                          x.ResponseGroup.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                                          x.ResponseValue.Equals(d.Value, StringComparison.CurrentCultureIgnoreCase)))
                                    {
                                        //bad
                                        badDemos.Add(d);
                                    }
                                }
                            }
                        }
                    }
                }

                if (badDemos.Count > 0)
                {
                    response.IsCodeSheetValid = false;
                    if (badDemos.Count == 1)
                    {
                        response.CodeSheetMessage += "An invalid Response Group or Value was detected, this will be removed and not inserted to your UAD." + System.Environment.NewLine;
                        response.CodeSheetMessage += "Invalid Response Group:Value - " + badDemos.First().Name.ToString() + ":" + badDemos.First().Value.ToString() + System.Environment.NewLine;
                    }
                    else
                    {
                        response.CodeSheetMessage += "Invalid Response Groups or Values were detected as follows, these will be removed and not inserted to your UAD." + System.Environment.NewLine;
                        foreach (FrameworkUAD.Object.SubscriberProductDemographic d in badDemos)
                        {
                            string badName = d.Name != null ? d.Name.ToString() : string.Empty;
                            string badValue = d.Value != null ? d.Value.ToString() : string.Empty;
                            response.CodeSheetMessage += "Response Group:Value - " + badName + ":" + badValue + System.Environment.NewLine;
                        }
                    }
                    if (subscription.Products != null)
                    {
                        foreach (FrameworkUAD.Object.ProductSubscription ps in subscription.Products)
                        {
                            foreach (FrameworkUAD.Object.SubscriberProductDemographic d in badDemos)
                            {
                                if (ps.SubscriberProductDemographics.Contains(d))
                                    ps.SubscriberProductDemographics.Remove(d);
                            }
                        }
                    }
                }
                else
                {
                    response.IsCodeSheetValid = true;
                    response.CodeSheetMessage += "All Response Groups and Values are valid." + System.Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".ValidateProductSubscription");
            }
            return response;
        }
        private void ValidateConsensusDemographics(ref FrameworkUAD.Object.SaveSubscriber subscription, KMPlatform.Entity.Client client, string processCode)
        {
            //make sure that each SubscriberConsensusDemographic name exists in SubscriptionsExtensionMapper
            //select CustomField from SubscriptionsExtensionMapper where Active = 1

            try
            {
                FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper semWorker = new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper();
                List<FrameworkUAD.Entity.SubscriptionsExtensionMapper> semList = semWorker.SelectAll(client.ClientConnections);

                List<FrameworkUAD.Object.SubscriberConsensusDemographic> badConDemos = new List<FrameworkUAD.Object.SubscriberConsensusDemographic>();
                if (subscription != null && subscription.ConsensusDemographics != null && semList != null)
                {
                    foreach (var check in subscription.ConsensusDemographics)
                    {
                        if (semList.Exists(x => x.CustomField.Equals(check.Name, StringComparison.CurrentCultureIgnoreCase)) == false)
                        {
                            if (!badConDemos.Contains(check))
                                badConDemos.Add(check);
                        }
                    }
                }
                foreach (var rem in badConDemos)
                    subscription.ConsensusDemographics.Remove(rem);
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".ValidateConsensusDemographics");
            }
        }
        private FrameworkUAD.ServiceResponse.SavedSubscriber SaveSubscriberTransformed(FrameworkUAD.ServiceResponse.SavedSubscriber response, KMPlatform.Entity.Client client, FrameworkUAD.Object.SaveSubscriber subscription, FrameworkUAS.Entity.SourceFile sf, string processCode)
        {
            try
            {
                int demoAppendId = 0;
                int demoReplaceId = 0;
                int demoOverwriteId = 0;
                FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                List<FrameworkUAD_Lookup.Entity.Code> demoUpdateCodes = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);

                int.TryParse(demoUpdateCodes.FirstOrDefault(x => x.CodeName.Equals("Append")).CodeId.ToString(), out demoAppendId);
                int.TryParse(demoUpdateCodes.FirstOrDefault(x => x.CodeName.Equals("Replace")).CodeId.ToString(), out demoReplaceId);
                int.TryParse(demoUpdateCodes.FirstOrDefault(x => x.CodeName.Equals("Overwrite")).CodeId.ToString(), out demoOverwriteId);

                FrameworkUAD_Lookup.BusinessLogic.TransactionCode tWorker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode();
                FrameworkUAD_Lookup.BusinessLogic.CategoryCode catWorker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode();
                List<FrameworkUAD_Lookup.Entity.TransactionCode> tranCodes = tWorker.SelectActiveIsFree(true).ToList();
                List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodes = catWorker.SelectActiveIsFree(true).ToList();

                #region Subscription Null Value Checks
                if (subscription.Address == null)
                    subscription.Address = string.Empty;
                if (subscription.Address3 == null)
                    subscription.Address3 = string.Empty;

                if (subscription.City == null)
                    subscription.City = string.Empty;
                if (subscription.Company == null)
                    subscription.Company = string.Empty;
                if (subscription.Country == null)
                    subscription.Country = string.Empty;

                if (subscription.County == null)
                    subscription.County = string.Empty;
                if (!(subscription.Demo31 == true || subscription.Demo31 == false))
                    subscription.Demo31 = false;
                if (!(subscription.Demo32 == true || subscription.Demo32 == false))
                    subscription.Demo32 = false;
                if (!(subscription.Demo33 == true || subscription.Demo33 == false))
                    subscription.Demo33 = false;
                if (!(subscription.Demo34 == true || subscription.Demo34 == false))
                    subscription.Demo34 = false;
                if (!(subscription.Demo35 == true || subscription.Demo35 == false))
                    subscription.Demo35 = false;
                if (!(subscription.Demo36 == true || subscription.Demo36 == false))
                    subscription.Demo36 = false;

                if (subscription.Fax == null)
                    subscription.Fax = string.Empty;

                if (subscription.FName == null)
                    subscription.FName = string.Empty;
                if (subscription.ForZip == null)
                    subscription.ForZip = string.Empty;
                if (subscription.Gender == null)
                    subscription.Gender = string.Empty;
                if (subscription.Home_Work_Address == null)
                    subscription.Home_Work_Address = string.Empty;

                if (subscription.LName == null)
                    subscription.LName = string.Empty;

                if (subscription.MailStop == null)
                    subscription.MailStop = string.Empty;
                if (subscription.Mobile == null)
                    subscription.Mobile = string.Empty;

                if (subscription.Phone == null)
                    subscription.Phone = string.Empty;

                if (subscription.Plus4 == null)
                    subscription.Plus4 = string.Empty;

                if (subscription.State == null)
                    subscription.State = string.Empty;

                if (subscription.Title == null)
                    subscription.Title = string.Empty;

                if (subscription.Zip == null)
                    subscription.Zip = string.Empty;

                #endregion
                foreach (FrameworkUAD.Object.ProductSubscription ps in subscription.Products)
                {
                    if (ps.SubscriberProductDemographics == null)
                        ps.SubscriberProductDemographics = new List<FrameworkUAD.Object.SubscriberProductDemographic>();

                    List<FrameworkUAD.Entity.SubscriberOriginal> addSO = new List<SubscriberOriginal>();
                    List<FrameworkUAD.Entity.SubscriberTransformed> addST = new List<SubscriberTransformed>();
                    FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper psemWorker = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper();
                    List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> psemList = psemWorker.SelectAll(client.ClientConnections);

                    FrameworkUAD.Entity.SubscriberOriginal newSO = new SubscriberOriginal();
                    #region ProductSubscription Null Value Checks
                    if (ps.Demo7 == null)
                        ps.Demo7 = string.Empty;
                    if (ps.Email == null)
                        ps.Email = string.Empty;
                    //if (ps.EmailStatusID == null)
                    //    ps.EmailStatusID = 1;
                    if (ps.PubCode == null)
                        ps.PubCode = string.Empty;
                    if (ps.PubName == null)
                        ps.PubName = string.Empty;
                    if (ps.PubTypeDisplayName == null)
                        ps.PubTypeDisplayName = string.Empty;
                    if (ps.QualificationDate == null)
                        ps.QualificationDate = DateTime.Now;
                    if (ps.Status == null)
                        ps.Status = FrameworkUAD.BusinessLogic.Enums.EmailStatus.Active.ToString();
                    if (ps.StatusUpdatedDate == null)
                        ps.StatusUpdatedDate = DateTime.Now;
                    if (ps.StatusUpdatedReason == null)
                        ps.StatusUpdatedReason = "Subscribed";
                    //if (ps.SubscriptionID == null)
                    //    ps.SubscriptionID = 0;
                    if (ps.QualificationDate == null)
                        ps.QualificationDate = DateTimeFunctions.GetMinDate();
                    #endregion
                    #region SO
                    newSO.ProcessCode = processCode;
                    newSO.Address = !string.IsNullOrEmpty(ps.Address1) ? ps.Address1 : subscription.Address;
                    newSO.Address3 = !string.IsNullOrEmpty(ps.Address3) ? ps.Address3 : subscription.Address3;
                    FrameworkUAD_Lookup.Entity.CategoryCode cc = null;
                    if (catCodes.Exists(x => x.CategoryCodeValue == ps.PubCategoryID))
                        cc = catCodes.SingleOrDefault(x => x.CategoryCodeValue == ps.PubCategoryID);
                    newSO.CategoryID = cc != null ? cc.CategoryCodeID : 0;
                    newSO.City = !string.IsNullOrEmpty(ps.City) ? ps.City : subscription.City;
                    newSO.Company = !string.IsNullOrEmpty(ps.Company) ? ps.Company : subscription.Company;
                    newSO.Country = !string.IsNullOrEmpty(ps.Country) ? ps.Country : subscription.Country;
                    newSO.County = !string.IsNullOrEmpty(ps.County) ? ps.County : subscription.County;
                    newSO.CreatedByUserID = 1;
                    newSO.DateCreated = DateTime.Now;
                    newSO.MailPermission = ps.MailPermission.HasValue == true ? ps.MailPermission.Value : subscription.Demo31;
                    newSO.FaxPermission = ps.FaxPermission.HasValue == true ? ps.FaxPermission : subscription.Demo32;
                    newSO.PhonePermission = ps.PhonePermission.HasValue == true ? ps.PhonePermission : subscription.Demo33;
                    newSO.OtherProductsPermission = ps.OtherProductsPermission.HasValue == true ? ps.OtherProductsPermission : subscription.Demo34;
                    newSO.ThirdPartyPermission = ps.ThirdPartyPermission.HasValue == true ? ps.ThirdPartyPermission : subscription.Demo35;
                    newSO.EmailRenewPermission = ps.EmailRenewPermission.HasValue == true ? ps.EmailRenewPermission : subscription.Demo36;
                    //newSO.TextPermission = subscription.TextPermission;
                    newSO.Demo7 = ps.Demo7.Length > 0 ? ps.Demo7 : string.Empty;
                    newSO.Email = ps.Email.Length > 0 ? ps.Email : string.Empty;
                    //newSO.EmailExists = newSO.Email.Length > 0 ? true : false;
                    //newSO.EmailStatusID = ps.EmailStatusID;
                    newSO.Fax = !string.IsNullOrEmpty(ps.Fax) ? ps.Fax : subscription.Fax;
                    newSO.FName = !string.IsNullOrEmpty(ps.FirstName) ? ps.FirstName : subscription.FName;
                    newSO.ForZip = subscription.ForZip;
                    newSO.Gender = !string.IsNullOrEmpty(ps.Gender) ? ps.Gender : subscription.Gender;
                    newSO.Home_Work_Address = subscription.Home_Work_Address;
                    //newSO.IsDQMProcessFinished = false;
                    //newSO.IsMailable = !string.IsNullOrEmpty(subscription.Address) ? true : false;
                    //newSO.IsUpdatedInLive = false;
                    newSO.LName = !string.IsNullOrEmpty(ps.LastName) ? ps.LastName : subscription.LName;
                    newSO.MailStop = !string.IsNullOrEmpty(ps.Address2) ? ps.Address2 : subscription.MailStop;
                    newSO.Mobile = !string.IsNullOrEmpty(ps.Mobile) ? ps.Mobile : subscription.Mobile;
                    newSO.Phone = !string.IsNullOrEmpty(ps.Phone) ? ps.Phone : subscription.Phone;
                    newSO.Plus4 = !string.IsNullOrEmpty(ps.Plus4) ? ps.Plus4 : subscription.Plus4;
                    newSO.PubCode = ps.PubCode;
                    //newSO.PubIDs = ps.PubID.ToString();
                    newSO.ProcessCode = processCode;
                    newSO.QDate = ps.QualificationDate.ToString().Length > 0 ? ps.QualificationDate : DateTime.Now;
                    newSO.QSourceID = ps.PubQSourceID > 0 ? ps.PubQSourceID : 0;
                    newSO.Sequence = ps.SequenceID > 0 ? ps.SequenceID : subscription.Sequence;
                    newSO.SourceFileID = sf.SourceFileID;
                    newSO.State = !string.IsNullOrEmpty(ps.RegionCode) ? ps.RegionCode : subscription.State;
                    //newSO.StatusUpdatedDate = ps.StatusUpdatedDate.ToString().Length > 0 ? ps.StatusUpdatedDate : DateTime.Now;
                    //newSO.StatusUpdatedReason = ps.StatusUpdatedReason;
                    newSO.Title = !string.IsNullOrEmpty(ps.Title) ? ps.Title : subscription.Title;
                    FrameworkUAD_Lookup.Entity.TransactionCode tc = null;
                    if (tranCodes.Exists(x => x.TransactionCodeValue == ps.PubTransactionID))
                        tc = tranCodes.SingleOrDefault(x => x.TransactionCodeValue == ps.PubTransactionID);
                    newSO.TransactionID = tc != null ? tc.TransactionCodeID : 0;
                    newSO.Zip = !string.IsNullOrEmpty(ps.ZipCode) ? ps.ZipCode : subscription.Zip;
                    newSO.IsActive = true;// ps.IsActive != null ? ps.IsActive : subscription.IsActive;
                    #endregion
                    #region SODemographic
                    if (ps.SubscriberProductDemographics != null)
                    {
                        foreach (FrameworkUAD.Object.SubscriberProductDemographic d in ps.SubscriberProductDemographics)
                        {
                            //d.Name = MAFField
                            //d.Value = Value
                            if (!string.IsNullOrEmpty(d.Value))
                            {
                                FrameworkUAD.Entity.SubscriberDemographicOriginal newSDO = new SubscriberDemographicOriginal();
                                newSDO.CreatedByUserID = 1;
                                newSDO.DateCreated = DateTime.Now;
                                newSDO.MAFField = d.Name;
                                newSDO.NotExists = false;
                                newSDO.PubID = ps.PubID;
                                newSDO.SORecordIdentifier = newSO.SORecordIdentifier;
                                newSDO.Value = d.Value;
                                if (psemList.Exists(x => x.CustomField.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase)))
                                    newSDO.IsAdhoc = true;
                                else
                                    newSDO.IsAdhoc = false;
                                if (d.DemoUpdateAction == FrameworkUAD_Lookup.Enums.DemographicUpdate.Append)
                                    newSDO.DemographicUpdateCodeId = demoAppendId;
                                else if (d.DemoUpdateAction == FrameworkUAD_Lookup.Enums.DemographicUpdate.Overwrite)
                                    newSDO.DemographicUpdateCodeId = demoOverwriteId;
                                else
                                    newSDO.DemographicUpdateCodeId = demoReplaceId;


                                //if (d.map.FieldMappingTypeID == demoRespOtherTypeID)
                                //    newSDO.IsResponseOther = true;

                                newSO.DemographicOriginalList.Add(newSDO);
                            }
                        }
                    }
                    else
                        ps.SubscriberProductDemographics = new List<FrameworkUAD.Object.SubscriberProductDemographic>();

                    if (subscription.ConsensusDemographics != null)
                    {
                        foreach (var cd in subscription.ConsensusDemographics)
                        {
                            if (!string.IsNullOrEmpty(cd.Value))
                            {
                                FrameworkUAD.Entity.SubscriberDemographicOriginal newSDO = new SubscriberDemographicOriginal();
                                newSDO.CreatedByUserID = 1;
                                newSDO.DateCreated = DateTime.Now;
                                newSDO.MAFField = cd.Name;
                                newSDO.NotExists = false;
                                newSDO.PubID = ps.PubID;
                                newSDO.SORecordIdentifier = newSO.SORecordIdentifier;
                                newSDO.Value = cd.Value;
                                newSDO.DemographicUpdateCodeId = demoAppendId;
                                newSDO.IsAdhoc = true;

                                //if (cd.FieldMappingTypeID == demoRespOtherTypeID)
                                //    newSDO.IsResponseOther = true;

                                newSO.DemographicOriginalList.Add(newSDO);
                            }
                        }
                    }

                    #endregion
                    addSO.Add(newSO);
                    FrameworkUAD.Entity.SubscriberTransformed newST = new SubscriberTransformed();
                    #region ST
                    newST.ProcessCode = processCode;
                    newST.Address = !string.IsNullOrEmpty(ps.Address1) ? ps.Address1 : subscription.Address;
                    newST.Address3 = !string.IsNullOrEmpty(ps.Address3) ? ps.Address3 : subscription.Address3;
                    newST.CategoryID = newSO.CategoryID;
                    newST.City = !string.IsNullOrEmpty(ps.City) ? ps.City : subscription.City;
                    newST.Company = !string.IsNullOrEmpty(ps.Company) ? ps.Company : subscription.Company;
                    newST.Country = !string.IsNullOrEmpty(ps.Country) ? ps.Country : subscription.Country;
                    newST.County = !string.IsNullOrEmpty(ps.County) ? ps.County : subscription.County;
                    newST.CreatedByUserID = 1;
                    newST.DateCreated = DateTime.Now;
                    newST.MailPermission = ps.MailPermission.HasValue == true ? ps.MailPermission.Value : subscription.Demo31;
                    newST.FaxPermission = ps.FaxPermission.HasValue == true ? ps.FaxPermission : subscription.Demo32;
                    newST.PhonePermission = ps.PhonePermission.HasValue == true ? ps.PhonePermission : subscription.Demo33;
                    newST.OtherProductsPermission = ps.OtherProductsPermission.HasValue == true ? ps.OtherProductsPermission : subscription.Demo34;
                    newST.ThirdPartyPermission = ps.ThirdPartyPermission.HasValue == true ? ps.ThirdPartyPermission : subscription.Demo35;
                    newST.EmailRenewPermission = ps.EmailRenewPermission.HasValue == true ? ps.EmailRenewPermission : subscription.Demo36;
                    //newST.TextPermission = subscription.TextPermission
                    newST.Demo7 = ps.Demo7.Length > 0 ? ps.Demo7 : string.Empty;
                    newST.Email = ps.Email.Length > 0 ? ps.Email : string.Empty;
                    //newST.EmailExists = newST.Email.Length > 0 ? true : false;
                    //newST.EmailStatusID = ps.EmailStatusID;
                    newST.Fax = !string.IsNullOrEmpty(ps.Fax) ? ps.Fax : subscription.Fax;
                    newST.FName = !string.IsNullOrEmpty(ps.FirstName) ? ps.FirstName : subscription.FName;
                    newST.ForZip = subscription.ForZip;
                    newST.Gender = !string.IsNullOrEmpty(ps.Gender) ? ps.Gender : subscription.Gender;
                    newST.Home_Work_Address = subscription.Home_Work_Address;
                    //newST.IsDQMProcessFinished = false;
                    //newST.IsMailable = false;
                    //newST.IsMailable = !string.IsNullOrEmpty(subscription.Address) ? true : false;
                    //newST.IsUpdatedInLive = false;
                    newST.LName = !string.IsNullOrEmpty(ps.LastName) ? ps.LastName : subscription.LName;
                    newST.MailStop = !string.IsNullOrEmpty(ps.Address2) ? ps.Address2 : subscription.MailStop;
                    newST.Mobile = !string.IsNullOrEmpty(ps.Mobile) ? ps.Mobile : subscription.Mobile;
                    newST.Phone = !string.IsNullOrEmpty(ps.Phone) ? ps.Phone : subscription.Phone;
                    newST.Plus4 = !string.IsNullOrEmpty(ps.Plus4) ? ps.Plus4 : subscription.Plus4;
                    newST.PubCode = ps.PubCode;
                    //newST.PubIDs = ps.PubID.ToString();
                    newST.QDate = ps.QualificationDate.ToString().Length > 0 ? ps.QualificationDate : DateTime.Now;
                    newST.QSourceID = ps.PubQSourceID > 0 ? ps.PubQSourceID : 0;
                    newST.Sequence = ps.SequenceID > 0 ? ps.SequenceID : subscription.Sequence;
                    newST.SORecordIdentifier = newSO.SORecordIdentifier;
                    newST.SourceFileID = sf.SourceFileID;
                    newST.State = !string.IsNullOrEmpty(ps.RegionCode) ? ps.RegionCode : subscription.State;
                    //newST.StatusUpdatedDate = ps.StatusUpdatedDate.ToString().Length > 0 ? ps.StatusUpdatedDate : DateTime.Now;
                    //newST.StatusUpdatedReason = ps.StatusUpdatedReason;
                    newST.Title = !string.IsNullOrEmpty(ps.Title) ? ps.Title : subscription.Title;
                    newST.TransactionID = newSO.TransactionID;
                    newST.Zip = !string.IsNullOrEmpty(ps.ZipCode) ? ps.ZipCode : subscription.Zip;
                    newST.IsActive = true;// ps.IsActive != null ? ps.IsActive : subscription.IsActive;
                    #endregion
                    #region STDemographic
                    if (ps.SubscriberProductDemographics != null)
                    {
                        foreach (FrameworkUAD.Object.SubscriberProductDemographic d in ps.SubscriberProductDemographics)
                        {
                            //d.Name = MAFField
                            //d.Value = Value
                            if (!string.IsNullOrEmpty(d.Value))
                            {
                                FrameworkUAD.Entity.SubscriberDemographicTransformed newSDT = new SubscriberDemographicTransformed();
                                newSDT.CreatedByUserID = 1;
                                newSDT.DateCreated = DateTime.Now;
                                newSDT.MAFField = d.Name;
                                newSDT.NotExists = false;
                                newSDT.PubID = ps.PubID;
                                newSDT.SORecordIdentifier = newSO.SORecordIdentifier;
                                newSDT.STRecordIdentifier = newST.STRecordIdentifier;
                                newSDT.Value = d.Value;
                                if (psemList.Exists(x => x.CustomField.Equals(d.Name, StringComparison.CurrentCultureIgnoreCase)))
                                    newSDT.IsAdhoc = true;
                                else
                                    newSDT.IsAdhoc = false;
                                if (d.DemoUpdateAction == FrameworkUAD_Lookup.Enums.DemographicUpdate.Append)
                                    newSDT.DemographicUpdateCodeId = demoAppendId;
                                else if (d.DemoUpdateAction == FrameworkUAD_Lookup.Enums.DemographicUpdate.Overwrite)
                                    newSDT.DemographicUpdateCodeId = demoOverwriteId;
                                else
                                    newSDT.DemographicUpdateCodeId = demoReplaceId;

                                //if (cd.FieldMappingTypeID == demoRespOtherTypeID)
                                //    newSDO.IsResponseOther = true;

                                newST.DemographicTransformedList.Add(newSDT);
                            }
                        }
                    }
                    else
                        ps.SubscriberProductDemographics = new List<FrameworkUAD.Object.SubscriberProductDemographic>();

                    if (subscription.ConsensusDemographics != null)
                    {
                        foreach (var cd in subscription.ConsensusDemographics)
                        {
                            if (!string.IsNullOrEmpty(cd.Value))
                            {
                                FrameworkUAD.Entity.SubscriberDemographicTransformed newSDT = new SubscriberDemographicTransformed();
                                newSDT.CreatedByUserID = 1;
                                newSDT.DateCreated = DateTime.Now;
                                newSDT.MAFField = cd.Name;
                                newSDT.NotExists = false;
                                newSDT.PubID = ps.PubID;
                                newSDT.SORecordIdentifier = newSO.SORecordIdentifier;
                                newSDT.STRecordIdentifier = newST.STRecordIdentifier;
                                newSDT.Value = cd.Value;
                                newSDT.DemographicUpdateCodeId = demoAppendId;
                                newSDT.IsAdhoc = true;

                                //if (cd.FieldMappingTypeID == demoRespOtherTypeID)
                                //    newSDO.IsResponseOther = true;

                                newST.DemographicTransformedList.Add(newSDT);
                            }
                        }
                    }
                    #endregion
                    addST.Add(newST);

                    #region do inserts
                    FrameworkUAD.BusinessLogic.SubscriberOriginal soData = new FrameworkUAD.BusinessLogic.SubscriberOriginal();
                    FrameworkUAD.BusinessLogic.SubscriberTransformed stData = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
                    if (soData.SaveBulkSqlInsert(addSO, client.ClientConnections) == true && stData.SaveBulkSqlInsert(addST, client.ClientConnections, false) == true)
                    {
                        response.SubscriberProductMessage += "Saved Subscriber: " + newSO.SORecordIdentifier.ToString() + " for Product: " + newSO.PubCode + System.Environment.NewLine;
                        response.IsProductSubscriberCreated = true;
                        response.SubscriberProductIdentifiers.Add(newSO.SORecordIdentifier, newSO.PubCode);
                    }
                    else
                    {

                        response.IsProductSubscriberCreated = false;
                        response.SubscriberProductMessage += "FAILED to create a Subscriber for Product: " + ps.PubCode + System.Environment.NewLine;
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".SaveSubscriberTransformed");
            }

            return response;
        }
        #endregion

        public static void AddDQMReadyFile(Core.ADMS.Events.FileAddressGeocoded eventMessage)
        {
            FrameworkUAS.Entity.AdmsLog admsLog = eventMessage.AdmsLog;
            string processCode = admsLog.ProcessCode;

            //CALL AND SAVE TO DB NOW
            bool isDemo = true;
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IsDemo"].ToString(), out isDemo);

            FrameworkUAS.Entity.DQMQue q = new FrameworkUAS.Entity.DQMQue(eventMessage.AdmsLog.ProcessCode, eventMessage.Client.ClientID, isDemo, true, eventMessage.SourceFile.SourceFileID);
            FrameworkUAS.BusinessLogic.DQMQue dqmWorker = new FrameworkUAS.BusinessLogic.DQMQue();
            dqmWorker.Save(q);
            eventMessage.AdmsLog.DQM = q;

            FrameworkUAS.BusinessLogic.AdmsLog admsWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
            admsWrk.Update(eventMessage.AdmsLog.ProcessCode,
                         FrameworkUAD_Lookup.Enums.FileStatusType.Qued,
                         FrameworkUAD_Lookup.Enums.ADMS_StepType.Qued,
                         FrameworkUAD_Lookup.Enums.ProcessingStatusType.Qued,
                         FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_DQM, 1, "Added file to DQM Que", true,
                         eventMessage.AdmsLog.SourceFileId);

        }

        private void CreateDashboardReports(FrameworkUAD.Object.ImportFile dataIV, FileMoved eventMessage, HashSet<SubscriberTransformed> dedupedTransList, HashSet<SubscriberInvalid> dedupedInvalidList, HashSet<SubscriberTransformed> dupeList, List<FieldMapping> fieldMappings, List<FrameworkUAS.Entity.TransformationFieldMap> allTFM, List<FrameworkUAS.Entity.TransformSplit> transformSplits)
        {
            BackgroundWorker bw = new BackgroundWorker();
            // this allows our worker to report progress during work
            bw.WorkerReportsProgress = true;

            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                BackgroundWorker b = o as BackgroundWorker;
                //need column lists
                List<string> incomingColumns = new List<string>();
                fieldMappings.OrderBy(x => x.ColumnOrder).ToList().ForEach(fm => incomingColumns.Add(fm.IncomingField));
                List<string> tmpDemoCols = new List<string>();
                fieldMappings.OrderBy(x => x.ColumnOrder).Where(t => t.FieldMappingTypeID == 37).ToList().ForEach(fm => tmpDemoCols.Add(fm.IncomingField));

                #region Rollup Dupes
                HashSet<SubscriberTransformed> newDupesList = new HashSet<SubscriberTransformed>();
                try
                {
                    foreach (SubscriberTransformed st in dupeList)
                    {
                        if (newDupesList.Count(x => x.OriginalImportRow == st.OriginalImportRow) > 0)
                        {
                            //Rollup
                            SubscriberTransformed newDupeListST = newDupesList.FirstOrDefault(x => x.OriginalImportRow == st.OriginalImportRow);
                            if (newDupeListST != null)
                            {
                                //newDupeListST.DemographicTransformedList
                                HashSet<SubscriberDemographicTransformed> notMatch = new HashSet<SubscriberDemographicTransformed>((from excd in st.DemographicTransformedList
                                                                                                                                    join dd in newDupeListST.DemographicTransformedList on excd.MAFField equals dd.MAFField
                                                                                                                                    where excd.Value != dd.Value
                                                                                                                                    select excd).Distinct().ToList());

                                HashSet<SubscriberDemographicTransformed> notExist = new HashSet<SubscriberDemographicTransformed>((from e in st.DemographicTransformedList
                                                                                                                                    where !newDupeListST.DemographicTransformedList.Any(x => x.MAFField == e.MAFField)
                                                                                                                                    select e).ToList());


                                HashSet<SubscriberDemographicTransformed> combined = new HashSet<SubscriberDemographicTransformed>();
                                notMatch.ToList().ForEach(x => { combined.Add(x); });
                                notExist.ToList().ForEach(x => { combined.Add(x); });
                                combined.ToList().ForEach(x =>
                                {
                                    if (!newDupeListST.DemographicTransformedList.ToList().Exists(y => y.MAFField == x.MAFField && y.Value == x.Value))
                                        newDupeListST.DemographicTransformedList.Add(x);
                                });
                                notMatch = null;
                                notExist = null;
                                combined = null;
                            }
                        }
                        else
                            newDupesList.Add(st);

                    }
                }
                catch (Exception ex)
                {
                    LogError(ex, client, "ADMS.Services.Validator.Validator.ValidateData - Rollup Dupes");
                }
                #endregion

                //our deDuped lists already have combined values from dedupe process so should just be able to dump to a file
                ConsoleMessage(eventMessage.AdmsLog, "Start 1: " + DateTime.Now);
                DataTable dt = ToDataTable_SubscriberTransformed(dedupedTransList, incomingColumns, tmpDemoCols, fieldMappings);//Core_AMS.Utilities.DataTableFunctions.ToDataTable<SubscriberTransformed, SubscriberDemographicTransformed>(dedupedTransList.ToList(),col1, tmpDemoCols);
                ConsoleMessage(eventMessage.AdmsLog, "1 DataTable created: " + DateTime.Now);
                WriteReport(eventMessage, dt, Core_AMS.Utilities.Enums.DashboardReportName._TransformedReport.ToString());
                ConsoleMessage(eventMessage.AdmsLog, "End 1: " + DateTime.Now);
                ConsoleMessage(eventMessage.AdmsLog, "Start 2: " + DateTime.Now);
                dt = ToDataTable_SubscriberInvalid(dedupedInvalidList.ToList(), incomingColumns, tmpDemoCols, fieldMappings);
                ConsoleMessage(eventMessage.AdmsLog, "2 DataTable created: " + DateTime.Now);
                WriteReport(eventMessage, dt, Core_AMS.Utilities.Enums.DashboardReportName._InvalidReport.ToString());
                ConsoleMessage(eventMessage.AdmsLog, "End 2: " + DateTime.Now);
                ConsoleMessage(eventMessage.AdmsLog, "Start 3: " + DateTime.Now);
                dt = ToDataTable_SubscriberTransformed(newDupesList, incomingColumns, tmpDemoCols, fieldMappings);
                ConsoleMessage(eventMessage.AdmsLog, "3 DataTable created: " + DateTime.Now);
                WriteReport(eventMessage, dt, Core_AMS.Utilities.Enums.DashboardReportName._DuplicatesReport.ToString());
                ConsoleMessage(eventMessage.AdmsLog, "End 3: " + DateTime.Now);

                #region Create DimensionReport
                FrameworkUAD.BusinessLogic.ImportErrorSummary rptWorker = new FrameworkUAD.BusinessLogic.ImportErrorSummary();
                List<FrameworkUAD.Object.ImportErrorSummary> summary = rptWorker.Select(eventMessage.AdmsLog.SourceFileId, eventMessage.AdmsLog.ProcessCode, eventMessage.Client.ClientConnections);                

                if (summary.Count() > 0)
                {
                    foreach (var s in summary)
                    {
                        s.ClientMessage = Core_AMS.Utilities.XmlFunctions.CleanInvalidXmlChars(s.ClientMessage.Trim());
                        s.MAFField = Core_AMS.Utilities.XmlFunctions.CleanInvalidXmlChars(s.MAFField.Trim());
                        s.PubCode = Core_AMS.Utilities.XmlFunctions.CleanInvalidXmlChars(s.PubCode.Trim());
                        s.Value = Core_AMS.Utilities.XmlFunctions.CleanInvalidXmlChars(s.Value.Trim());
                    }

                    ConsoleMessage(eventMessage.AdmsLog, "Start 4: " + DateTime.Now);
                    dt = ToDataTable_SubscriberTransformed_DimensionErrors(dedupedTransList, incomingColumns, tmpDemoCols, fieldMappings, summary);
                    ConsoleMessage(eventMessage.AdmsLog, "4 DataTable created: " + DateTime.Now);
                    WriteReport(eventMessage, dt, Core_AMS.Utilities.Enums.DashboardReportName._DimensionSubscriber.ToString());
                    ConsoleMessage(eventMessage.AdmsLog, "End 4: " + DateTime.Now);

                    //Update Counts in case count has changed (Bug ID: 48267)
                    FrameworkUAD.BusinessLogic.SubscriberTransformed sourceFileWrk = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
                    FrameworkUAD.Object.DimensionErrorCount counts = sourceFileWrk.SelectDimensionCount(eventMessage.AdmsLog.ProcessCode, eventMessage.Client.ClientConnections);
                    int distinctSubscriberCount = counts.DimensionDistinctSubscriberCount;
                    if (dt.Rows.Count > 0)
                        distinctSubscriberCount = dt.Rows.Count;

                    admsWrk.UpdateDimension(eventMessage.AdmsLog.ProcessCode, counts.DimensionErrorTotal, dt.Rows.Count, 1, true, eventMessage.AdmsLog.SourceFileId);
                }
                #endregion
            });

            // what to do when progress changed (update the progress bar for example)
            bw.ProgressChanged += new ProgressChangedEventHandler(
            delegate (object o, ProgressChangedEventArgs args)
            {

            });

            
            // what to do when worker completes its task (notify the user)
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
            delegate (object o, RunWorkerCompletedEventArgs args)
            {
                //removing per Bobby
                //Emailer.Emailer e = new Emailer.Emailer();
                //e.NotifyDashboardReportsGenerated(eventMessage.Client, eventMessage.ImportFile);
            });

            bw.RunWorkerAsync();
        }
        private void WriteReport(FileMoved eventMessage, DataTable dt, string reportName)//FrameworkUAD.Object.ImportFile dataIV, FileMoved eventMessage, HashSet<SubscriberTransformed> dedupedTransList, List<FieldMapping> fieldMappings, List<FrameworkUAS.Entity.TransformationFieldMap> allTFM, List<FrameworkUAS.Entity.TransformSplit> transformSplits)
        {
            try
            {
                //Create report in datatable and Save File
                if (dt.Rows.Count > 0)
                {
                    string clientArchiveDir = Core.ADMS.BaseDirs.getClientArchiveDir().ToString();
                    string dir = clientArchiveDir + @"\" + eventMessage.Client.FtpFolder + @"\Reports\";
                    System.IO.Directory.CreateDirectory(dir);
                    string cleanProcessCode = Core_AMS.Utilities.StringFunctions.CleanProcessCodeForFileName(eventMessage.AdmsLog.ProcessCode);
                    string transformedReportName = dir + cleanProcessCode + reportName + ".xlsx";
                    Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
                    Telerik.Windows.Documents.Spreadsheet.Model.Workbook wb = ef.GetWorkbook(dt, reportName.Replace("_", ""));
                    Telerik.Windows.Documents.Spreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();
                    using (FileStream output = new FileStream(transformedReportName, FileMode.Create))
                    {
                        formatProvider.Export(wb, output);
                    }
                }
                dt = null;
            }
            catch (Exception ex)
            {
                LogError(ex, client, "ADMS.Services.Validator.Validator.ValidateData - Create " + reportName.Replace("_", ""));
            }
        }

        private DataTable ToDataTable_SubscriberTransformed(
            HashSet<SubscriberTransformed> subList, 
            IList<string> incomingColumns, 
            IList<string> tmpDemoColumns, 
            IList<FieldMapping> fieldMappings)
        {
            Guard.NotNull(subList, nameof(subList));
            Guard.NotNull(incomingColumns, nameof(incomingColumns));
            Guard.NotNull(tmpDemoColumns, nameof(tmpDemoColumns));
            Guard.NotNull(fieldMappings, nameof(fieldMappings));

            var returnDT = new DataTable();
            try
            { 
                var stCols = incomingColumns.DeepClone();
                var totalRows = subList.Count.ToString();
                var properties = TypeDescriptor.GetProperties(typeof(SubscriberTransformed));
                var rowCounter = 1;
                
                foreach (var item in tmpDemoColumns)
                {
                    stCols.Remove(item);
                }
                foreach (var item in incomingColumns)
                {
                    returnDT.Columns.Add(item);
                }
                
                foreach (var item in subList)
                {
                    Console.WriteLine($"Row {rowCounter} of {totalRows}");
                    var row = returnDT.NewRow();

                    foreach (PropertyDescriptor prop in properties)
                    {
                        var tempField = SetTempField(prop);

                        if (fieldMappings.Any(x => x.MAFField.Equals(tempField, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            var fieldMap = fieldMappings.Single(map => 
                                map.MAFField.Equals(tempField, StringComparison.CurrentCultureIgnoreCase));
                            SetIncomeField(incomingColumns, item, row, prop, fieldMap);
                        }
                        if (prop.Name == DemographicTransformedListKey)
                        {
                            var demographicTransformedList = prop.GetValue(item) as HashSet<SubscriberDemographicTransformed>;
                            SetMafFieldRowsForTransformedList(row, demographicTransformedList);
                        }
                    }
                    returnDT.Rows.Add(row);
                    rowCounter++;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, $"{ValidateDataKey} - {ToDataTableSubscriberTransformedKey}");
            }
            return returnDT;
        }

        private DataTable ToDataTable_SubscriberInvalid(
            IList<SubscriberInvalid> dedupedInvalidList, 
            IList<string> incomingColumns, 
            IList<string> tmpDemoColumns, 
            IList<FieldMapping> fieldMappings)
        {
            Guard.NotNull(dedupedInvalidList, nameof(dedupedInvalidList));
            Guard.NotNull(incomingColumns, nameof(incomingColumns));
            Guard.NotNull(tmpDemoColumns, nameof(tmpDemoColumns));
            Guard.NotNull(fieldMappings, nameof(fieldMappings));

            var returnDT = new DataTable();
            try
            {
                var stCols = incomingColumns.DeepClone();
                var totalRows = dedupedInvalidList.Count.ToString();
                var properties = TypeDescriptor.GetProperties(typeof(SubscriberInvalid));
                var rowCounter = 1;
                
                foreach (var item in tmpDemoColumns)
                {
                    stCols.Remove(item);
                }
                foreach (var item in incomingColumns)
                {
                    returnDT.Columns.Add(item);
                }

                foreach (var item in dedupedInvalidList)
                {
                    Console.WriteLine($"Row {rowCounter} of {totalRows}");
                    var row = returnDT.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                    {
                        var tempField = SetTempField(prop);

                        if (fieldMappings.Any(map => map.MAFField.Equals(tempField, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            var fieldMap = fieldMappings.Single(map => 
                                map.MAFField.Equals(tempField, StringComparison.CurrentCultureIgnoreCase));
                            SetIncomeField(incomingColumns, item, row, prop, fieldMap);
                        }

                        if (prop.Name == DemographicInvalidListKey)
                        {
                            var DemographicTransformedList = (HashSet<SubscriberDemographicInvalid>)prop.GetValue(item);
                            SetMafFieldRowsForInvalidList(row, DemographicTransformedList);
                        }
                    }
                    returnDT.Rows.Add(row);
                    rowCounter++;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, $"{ValidateDataKey} - {ToDataTableSubscriberInvalidKey}");
            }
            return returnDT;
        }

        private static string SetTempField(PropertyDescriptor prop)
        {
            string tempField;
            switch (prop.DisplayName.ToUpper())
            {
                case "SEQUENCE":
                    tempField = "SequenceID";
                    break;
                case "TRANSACTIONID":
                    tempField = "PubTransactionID";
                    break;
                case "CATEGORYID":
                    tempField = "PubCategoryID";
                    break;
                case "QSOURCEID":
                    tempField = "PubQSourceID";
                    break;
                case "TRANSACTIONDATE":
                    tempField = "PubTransactionDate";
                    break;
                case "PAR3C":
                    tempField = "Par3cID";
                    break;
                case "DEMO31":
                    tempField = "MailPermission";
                    break;
                case "DEMO32":
                    tempField = "FaxPermission";
                    break;
                case "DEMO33":
                    tempField = "PhonePermission";
                    break;
                case "DEMO34":
                    tempField = "OtherProductsPermission";
                    break;
                case "DEMO35":
                    tempField = "ThirdPartyPermission";
                    break;
                case "DEMO36":
                    tempField = "EmailRenewPermission";
                    break;
                default:
                    tempField = prop.DisplayName;
                    break;
            }
            return tempField;
        }

        private DataTable ToDataTable_SubscriberTransformed_DimensionErrors(
            HashSet<SubscriberTransformed> subList, 
            IList<string> incomingColumns, 
            IList<string> tmpDemoColumns, 
            IList<FieldMapping> fieldMappings, 
            IList<ImportErrorSummary> summary)
        {
            Guard.NotNull(subList, nameof(subList));
            Guard.NotNull(incomingColumns, nameof(incomingColumns));
            Guard.NotNull(tmpDemoColumns, nameof(tmpDemoColumns));
            Guard.NotNull(fieldMappings, nameof(fieldMappings));
            Guard.NotNull(summary, nameof(summary));

            var returnDT = new DataTable();
            try
            { 
                var stCols = incomingColumns.DeepClone();
                var totalRows = subList.Count.ToString();
                var rowCounter = 1;
                var properties = TypeDescriptor.GetProperties(typeof(SubscriberTransformed));

                foreach (var item in tmpDemoColumns)
                {
                    stCols.Remove(item);
                }
                foreach (var item in incomingColumns)
                {
                    returnDT.Columns.Add(item);
                }
                
                foreach (var item in subList)
                {                
                    Console.WriteLine($"Row {rowCounter} of {totalRows}");

                    var hasBadDimensionValue = false;
                    var pubSpecificSummary = summary.Where(ies =>
                            ies.PubCode.Equals(item.PubCode, StringComparison.CurrentCultureIgnoreCase))
                        .ToList();

                    foreach (var sdtItem in item.DemographicTransformedList)
                    {
                        if (pubSpecificSummary.Count(ies => ies.MAFField.Equals(sdtItem.MAFField, StringComparison.CurrentCultureIgnoreCase) 
                                                            && ies.Value.Equals(sdtItem.Value, StringComparison.CurrentCultureIgnoreCase)) > 0)
                        {
                            hasBadDimensionValue = true;
                            break;
                        }
                    }

                    if (hasBadDimensionValue)
                    {
                        AddReturnDataTableRows(incomingColumns, fieldMappings, returnDT, properties, item);
                    }
                    rowCounter++;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, $"{ValidateDataKey} - {ToDataTableDimensionsErrorKey}");
            }
            return returnDT;
        }

        private void AddReturnDataTableRows(
            IList<string> incomingColumns, 
            IList<FieldMapping> fieldMappings, 
            DataTable returnDT, 
            PropertyDescriptorCollection properties, 
            SubscriberTransformed subscriberTransformed)
        {
            Guard.NotNull(incomingColumns, nameof(incomingColumns));
            Guard.NotNull(fieldMappings, nameof(fieldMappings));
            Guard.NotNull(returnDT, nameof(returnDT));
            Guard.NotNull(properties, nameof(properties));
            Guard.NotNull(subscriberTransformed, nameof(subscriberTransformed));

            var row = returnDT.NewRow();
            foreach (PropertyDescriptor prop in properties)
            {
                var tempField = SetTempField(prop);

                if (fieldMappings.Any(x => x.MAFField.Equals(tempField, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var fm = fieldMappings.Single(x => x.MAFField.Equals(tempField, StringComparison.CurrentCultureIgnoreCase));
                    SetIncomeField(incomingColumns, subscriberTransformed, row, prop, fm);
                }
                if (prop.Name == DemographicTransformedListKey)
                {
                    var DemographicTransformedList = (HashSet<SubscriberDemographicTransformed>)prop.GetValue(subscriberTransformed);
                    SetMafFieldRowsForTransformedList(row, DemographicTransformedList);
                }
            }
            returnDT.Rows.Add(row);
        }

        private void SetMafFieldRowsForTransformedList(DataRow row, HashSet<SubscriberDemographicTransformed> demographicTransformedList)
        {
            Guard.NotNull(row, nameof(row));
            Guard.NotNull(demographicTransformedList, nameof(demographicTransformedList));

            foreach (var item in demographicTransformedList.OrderBy(x => x.MAFField))
            {
                if (demographicTransformedList.Count(x => x.MAFField.Equals(item.MAFField, StringComparison.CurrentCultureIgnoreCase)) > 1)
                {
                    row[item.MAFField] += $",{item.Value}";

                    if (demographicTransformedList.Last(x => x.MAFField.Equals(item.MAFField, StringComparison.CurrentCultureIgnoreCase)).Equals(item))
                    {
                        row[item.MAFField] = row[item.MAFField].ToString().Trim().TrimEnd(CommaSepator);
                    }
                    row[item.MAFField] = row[item.MAFField].ToString().Trim().TrimStart(CommaSepator);
                }
                else
                {
                    row[item.MAFField] = item.Value;
                }
            }
        }

        private void SetMafFieldRowsForInvalidList(DataRow row, HashSet<SubscriberDemographicInvalid> demographicInvalidList)
        {
            Guard.NotNull(row, nameof(row));
            Guard.NotNull(demographicInvalidList, nameof(demographicInvalidList));

            foreach (var item in demographicInvalidList.OrderBy(x => x.MAFField))
            {
                if (demographicInvalidList.Count(x => x.MAFField.Equals(item.MAFField, StringComparison.CurrentCultureIgnoreCase)) > 1)
                {
                    row[item.MAFField] += $",{item.Value}";

                    if (demographicInvalidList.Last(x => x.MAFField.Equals(item.MAFField, StringComparison.CurrentCultureIgnoreCase)).Equals(item))
                    {
                        row[item.MAFField] = row[item.MAFField].ToString().Trim().TrimEnd(CommaSepator);
                    }
                    row[item.MAFField] = row[item.MAFField].ToString().Trim().TrimStart(CommaSepator);
                }
                else
                {
                    row[item.MAFField] = item.Value;
                }
            }
        }

        private void SetIncomeField(IList<string> incomingColumns, object subscriberItem, DataRow row, PropertyDescriptor property, FieldMapping fieldMapping)
        {
            Guard.NotNull(incomingColumns, nameof(incomingColumns));
            Guard.NotNull(subscriberItem, nameof(subscriberItem));
            Guard.NotNull(row, nameof(row));
            Guard.NotNull(property, nameof(property));
            Guard.NotNull(fieldMapping, nameof(fieldMapping));

            if (incomingColumns.Any(x => x.Equals(fieldMapping.IncomingField, StringComparison.CurrentCultureIgnoreCase)))
            {
                try
                {
                    row[fieldMapping.IncomingField] = property.GetValue(subscriberItem) ?? string.Empty;

                    if (Type.GetType(property.PropertyType.FullName).ToString().Contains(typeof(DateTime).ToString()))
                    {
                        var data = row[fieldMapping.IncomingField].ToString();
                        if (!string.IsNullOrWhiteSpace(data))
                        {
                            var date = Convert.ToDateTime(data);
                            row[fieldMapping.IncomingField] = date.ToShortDateString();
                        }
                    }
                }
                catch (Exception ex) { }
            }
        }

        #region old report gen code - no longer used - replaced with ToDataTable_xxxxxx methods
        private void CreateTransformedReport(FrameworkUAD.Object.ImportFile dataIV, FileMoved eventMessage, HashSet<SubscriberTransformed> dedupedTransList, List<FieldMapping> fieldMappings, List<FrameworkUAS.Entity.TransformationFieldMap> allTFM, List<FrameworkUAS.Entity.TransformSplit> transformSplits)
        {
            try
            {
                //Create report in datatable and Save File
                DataTable dtTransformedReport = CreateReport(dedupedTransList.Select(x => x.OriginalImportRow).ToList(), dataIV, fieldMappings, allTFM, transformSplits);
                if (dtTransformedReport.Rows.Count > 0)
                {
                    string clientArchiveDir = Core.ADMS.BaseDirs.getClientArchiveDir().ToString();
                    string dir = clientArchiveDir + @"\" + eventMessage.Client.FtpFolder + @"\Reports\";
                    System.IO.Directory.CreateDirectory(dir);
                    string cleanProcessCode = Core_AMS.Utilities.StringFunctions.CleanProcessCodeForFileName(eventMessage.AdmsLog.ProcessCode);
                    string transformedReportName = dir + cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._TransformedReport.ToString() + ".xlsx";
                    Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
                    Telerik.Windows.Documents.Spreadsheet.Model.Workbook wb = ef.GetWorkbook(dtTransformedReport, "Transformed");
                    Telerik.Windows.Documents.Spreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();
                    using (FileStream output = new FileStream(transformedReportName, FileMode.Create))
                    {
                        formatProvider.Export(wb, output);
                    }
                }
                dtTransformedReport = null;
            }
            catch (Exception ex)
            {
                LogError(ex, client, "ADMS.Services.Validator.Validator.ValidateData - Create TransformedReport");
            }
        }
        private void CreateInvalidReport(FrameworkUAD.Object.ImportFile dataIV, FileMoved eventMessage, HashSet<SubscriberInvalid> dedupedInvalidList, List<FieldMapping> fieldMappings, List<FrameworkUAS.Entity.TransformationFieldMap> allTFM, List<FrameworkUAS.Entity.TransformSplit> transformSplits)
        {
            try
            {
                //Create Report and Save File If Row Count > 0
                //Need to collect originalRowNumbers, Sends to function and prevents adding duplicate rows
                List<int> originalRowNumbers = new List<int>();
                List<int> importRowNumbers = dedupedInvalidList.Select(x => x.ImportRowNumber).ToList();
                foreach (int irn in importRowNumbers)
                {
                    int originalRowNumber = dataIV.TransformedRowToOriginalRowMap[irn];
                    if (originalRowNumbers.Count(x => x == originalRowNumber) == 0)
                        originalRowNumbers.Add(originalRowNumber);

                }
                DataTable dtInvalidReport = CreateReport(originalRowNumbers, dataIV, fieldMappings, allTFM, transformSplits);
                if (dtInvalidReport.Rows.Count > 0)
                {
                    string clientArchiveDir = Core.ADMS.BaseDirs.getClientArchiveDir().ToString();
                    string dir = clientArchiveDir + @"\" + eventMessage.Client.FtpFolder + @"\Reports\";
                    System.IO.Directory.CreateDirectory(dir);
                    string cleanProcessCode = Core_AMS.Utilities.StringFunctions.CleanProcessCodeForFileName(eventMessage.AdmsLog.ProcessCode);
                    string transformedReportName = dir + cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._InvalidReport.ToString() + ".xlsx";
                    Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
                    Telerik.Windows.Documents.Spreadsheet.Model.Workbook wb = ef.GetWorkbook(dtInvalidReport, "Invalid");
                    Telerik.Windows.Documents.Spreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();
                    using (FileStream output = new FileStream(transformedReportName, FileMode.Create))
                    {
                        formatProvider.Export(wb, output);
                    }
                }
                dtInvalidReport = null;
            }
            catch (Exception ex)
            {
                LogError(ex, client, "ADMS.Services.Validator.Validator.ValidateData - Create InvalidReport");
            }
        }
        private void CreateDupesReport(FrameworkUAD.Object.ImportFile dataIV, FileMoved eventMessage, HashSet<SubscriberTransformed> dupesList, List<FieldMapping> fieldMappings, List<FrameworkUAS.Entity.TransformationFieldMap> allTFM, List<FrameworkUAS.Entity.TransformSplit> transformSplits)
        {
            try
            {
                //Save File
                DataTable dtDupesReport = CreateReport(dupesList.Select(x => x.OriginalImportRow).Distinct().ToList(), dataIV, fieldMappings, allTFM, transformSplits);
                if (dtDupesReport.Rows.Count > 0)
                {
                    string clientArchiveDir = Core.ADMS.BaseDirs.getClientArchiveDir().ToString();
                    string dir = clientArchiveDir + @"\" + eventMessage.Client.FtpFolder + @"\Reports\";
                    System.IO.Directory.CreateDirectory(dir);
                    string cleanProcessCode = Core_AMS.Utilities.StringFunctions.CleanProcessCodeForFileName(eventMessage.AdmsLog.ProcessCode);
                    string transformedReportName = dir + cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._DuplicatesReport.ToString() + ".xlsx";
                    Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
                    Telerik.Windows.Documents.Spreadsheet.Model.Workbook wb = ef.GetWorkbook(dtDupesReport, "Duplicates");
                    Telerik.Windows.Documents.Spreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();
                    using (FileStream output = new FileStream(transformedReportName, FileMode.Create))
                    {
                        formatProvider.Export(wb, output);
                    }
                }
                dtDupesReport = null;
            }
            catch (Exception ex)
            {
                LogError(ex, client, "ADMS.Services.Validator.Validator.ValidateData - Create DuplicatesReport");
            }
        }
        //This will create the DataTable used to create the Transformed, Invalid, and Duplicate Dashboard Report.
        private DataTable CreateReport(List<int> originalImportRowList, FrameworkUAD.Object.ImportFile dataIV, List<FrameworkUAS.Entity.FieldMapping> fieldMappings, List<FrameworkUAS.Entity.TransformationFieldMap> allTFM, List<FrameworkUAS.Entity.TransformSplit> transformSplits)
        {
            //return DT because that is what Telerik needs to create excel file
            DataTable dt = new DataTable();

            #region  use HashSets as they are faster than Lists
            HashSet<FrameworkUAS.Entity.FieldMapping> hsFieldMappings = new HashSet<FieldMapping>(fieldMappings.OrderBy(co => co.ColumnOrder).ToList());
            HashSet<int> origImportRows = new HashSet<int>(originalImportRowList);
            HashSet<FrameworkUAS.Entity.TransformationFieldMap> hsAllTFM = new HashSet<TransformationFieldMap>(allTFM);
            HashSet<FrameworkUAS.Entity.TransformSplit> hsTransformSplits = new HashSet<TransformSplit>(transformSplits);


            #endregion

            #region DataTable Add Columns
            fieldMappings.OrderBy(x => x.ColumnOrder);
            fieldMappings.ForEach(x =>
            {
                DataColumn dc = new DataColumn(x.IncomingField);
                dt.Columns.Add(dc);
            });
            //foreach (FrameworkUAS.Entity.FieldMapping fm in fieldMappings.OrderBy(x => x.ColumnOrder))
            //{
            //    DataColumn dc = new DataColumn(fm.IncomingField);
            //    dt.Columns.Add(dc);
            //}
            #endregion
            #region Add Rows
            //origImportRowProcessed - this is just a memory hog object, has no purpose other than ensure we don't double process a row number
            //this handle this better so we dont chew memory

            //lets first break dataIV.DataTransformed into 2 groups - Splits/NonSplits   - Splits we need to combine values

            var countCheck = dataIV.DataTransformed.Count;
            //originalImportRowList should be a 1 to xxx number of records so if List.Count == countCheck 

            //no splits to deal with
            if (origImportRows.Count == dataIV.DataTransformed.Count)
            {
                //just create DT
                foreach (var key in dataIV.DataTransformed.Keys)
                {
                    StringDictionary myRow = dataIV.DataTransformed[key];
                    if (myRow["originalimportrow"] != null)
                    {
                        DataRow dr = dt.NewRow();
                        foreach (var fm in hsFieldMappings)
                        {
                            dr[fm.IncomingField] = myRow[fm.IncomingField] == null ? "" : myRow[fm.IncomingField].ToString();
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            else
            {
                int rowCount = 0;
                Dictionary<int, int> splitRowCheck = new Dictionary<int, int>();
                Console.WriteLine(DateTime.Now);
                foreach (var key in dataIV.DataTransformed.Keys)
                {
                    StringDictionary myRow = dataIV.DataTransformed[key];
                    splitRowCheck.Add(rowCount, Convert.ToInt32(myRow["originalimportrow"]));
                }
                Console.WriteLine(DateTime.Now);
            }







            foreach (int x in originalImportRowList)
            {
                List<string> origImportRowProcessed = new List<string>();
                foreach (var key in dataIV.DataTransformed.Keys)
                {
                    StringDictionary myRow = dataIV.DataTransformed[key];
                    //Prevent duplicate rows from being added
                    if (!origImportRowProcessed.Any(z => z.Equals(myRow["originalimportrow"])))
                    {
                        if (myRow["originalimportrow"] != null && myRow["originalimportrow"].ToString() == x.ToString())
                        {
                            DataRow dr = dt.NewRow();
                            foreach (FrameworkUAS.Entity.FieldMapping fm in fieldMappings.OrderBy(co => co.ColumnOrder))
                            {
                                string data = "";
                                int transformationID = 0;
                                if (allTFM.Count(z => z.FieldMappingID == fm.FieldMappingID) > 0)
                                {
                                    transformationID = allTFM.FirstOrDefault(z => z.FieldMappingID == fm.FieldMappingID).TransformationID;
                                    string delimiter = "";
                                    if (transformSplits.FirstOrDefault(z => z.TransformationID == transformationID) != null)
                                        delimiter = transformSplits.FirstOrDefault(z => z.TransformationID == transformationID).Delimiter;

                                    //This will combine all the values separated by split into row transformation
                                    data = CombineValues(dataIV.DataTransformed, fm.IncomingField, delimiter, myRow["originalimportrow"]);
                                }
                                else
                                    data = string.IsNullOrEmpty(myRow[fm.IncomingField].ToString()) ? "" : myRow[fm.IncomingField].ToString();

                                dr[fm.IncomingField] = data;
                            }
                            dt.Rows.Add(dr);
                        }
                        origImportRowProcessed.Add(myRow["originalimportrow"]);
                    }
                }
            }
            #endregion

            return dt;
        }
        private string CombineValues(Dictionary<int, StringDictionary> data, string column, string delimiter, string originalImportRow)
        {
            //the intent here is to get rows of data that are the same original import record - meaning they were a split transform
            //we want to combine the column answers to one "value" seperated by the correct delimeter as originally used

            List<string> values = new List<string>();

            foreach (var key in data.Keys)
            {
                StringDictionary myRow = data[key];
                if (myRow["originalimportrow"] == originalImportRow)
                {
                    if (values.Count(x => x.Equals(myRow[column].ToString(), StringComparison.CurrentCultureIgnoreCase)) == 0)
                        values.Add(myRow[column]);
                }
            }

            var delimiterValue = CommonEnums.GetDelimiterSymbol(delimiter).GetValueOrDefault(',').ToString();
            return String.Join(delimiterValue, values);
        }
        #endregion



       
        //private string PubToSubColumnDif(string subProp)
        //{
        //    #region Mapping Differences
        //    switch (subProp.ToUpper())
        //    {
        //        case "ADDRESS1":
        //            subProp = "Address";
        //            break;
        //        case "ADDRESS2":
        //            subProp = "MailStop";
        //            break;
        //        case "FIRSTNAME":
        //            subProp = "FName";
        //            break;
        //        case "LASTNAME":
        //            subProp = "LName";
        //            break;
        //        case "PAR3CID":
        //            subProp = "Par3c";
        //            break;
        //        case "PUBCATEGORYID":
        //            subProp = "CategoryID";
        //            break;
        //        case "PUBQSOURCEID":
        //            subProp = "QSourceID";
        //            break;
        //        case "PUBTRANSACTIONDATE":
        //            subProp = "TransactionDate";
        //            break;
        //        case "PUBTRANSACTIONID":
        //            subProp = "TransactionID";
        //            break;
        //        case "QUALIFICATIONDATE":
        //            subProp = "QDate";
        //            break;
        //        case "REGIONCODE":
        //            subProp = "State";
        //            break;
        //        case "SEQUENCEID":
        //            subProp = "Sequence";
        //            break;
        //        case "SUBSCRIBERSOURCECODE":
        //            subProp = "Subsrc";
        //            break;
        //        case "VERIFY":
        //            subProp = "Verified";
        //            break;
        //        case "ZIPCODE":
        //            subProp = "Zip";
        //            break;
        //        case "DEMO31":
        //            subProp = "MailPermission";
        //            break;
        //        case "DEMO32":
        //            subProp = "FaxPermission";
        //            break;
        //        case "DEMO33":
        //            subProp = "PhonePermission";
        //            break;
        //        case "DEMO34":
        //            subProp = "OtherProductsPermission";
        //            break;
        //        case "DEMO35":
        //            subProp = "ThirdPartyPermission";
        //            break;
        //        case "DEMO36":
        //            subProp = "EmailRenewPermission";
        //            break;
        //    }
        //    #endregion
        //    return subProp;
        //}
        //private string SubToPubColumnDif(string subProp)
        //{
        //    #region Mapping Differences
        //    switch (subProp.ToUpper())
        //    {
        //        case "Address":
        //            subProp = "ADDRESS1";
        //            break;
        //        case "MailStop" :
        //            subProp = "ADDRESS2";
        //            break;
        //        case "FName":
        //            subProp = "FIRSTNAME";
        //            break;
        //        case "LName":
        //            subProp = "LASTNAME";
        //            break;
        //        case "Par3c":
        //            subProp = "PAR3CID";
        //            break;
        //        case "PUBCATEGORYID":
        //            subProp = "CategoryID";
        //            break;
        //        case "PUBQSOURCEID":
        //            subProp = "QSourceID";
        //            break;
        //        case "PUBTRANSACTIONDATE":
        //            subProp = "TransactionDate";
        //            break;
        //        case "PUBTRANSACTIONID":
        //            subProp = "TransactionID";
        //            break;
        //        case "QUALIFICATIONDATE":
        //            subProp = "QDate";
        //            break;
        //        case "REGIONCODE":
        //            subProp = "State";
        //            break;
        //        case "SEQUENCEID":
        //            subProp = "Sequence";
        //            break;
        //        case "SUBSCRIBERSOURCECODE":
        //            subProp = "Subsrc";
        //            break;
        //        case "VERIFY":
        //            subProp = "Verified";
        //            break;
        //        case "ZIPCODE":
        //            subProp = "Zip";
        //            break;
        //        case "DEMO31":
        //            subProp = "MailPermission";
        //            break;
        //        case "DEMO32":
        //            subProp = "FaxPermission";
        //            break;
        //        case "DEMO33":
        //            subProp = "PhonePermission";
        //            break;
        //        case "DEMO34":
        //            subProp = "OtherProductsPermission";
        //            break;
        //        case "DEMO35":
        //            subProp = "ThirdPartyPermission";
        //            break;
        //        case "DEMO36":
        //            subProp = "EmailRenewPermission";
        //            break;
        //    }
        //    #endregion
        //    return subProp;
        //}
    }
}
