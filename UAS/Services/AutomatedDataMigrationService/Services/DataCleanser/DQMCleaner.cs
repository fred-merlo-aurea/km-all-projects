using System;
using System.Collections.Generic;
using System.Linq;
using Core.ADMS.Events;
using System.IO;
using FrameworkUAS.BusinessLogic;
using Core_AMS.Utilities;
using System.Threading;
using System.Text;
using System.Collections.Specialized;
using KM.Common.Import;

namespace ADMS.Services.DataCleanser
{
    /// <summary>
    /// 1. Client Custom Procedures
    /// 2. Country Region Cleanse
    /// 3. DQM Cleanse
    /// </summary>
    public class DQMCleaner : ServiceBase, IDQMCleaner
    {
        public event Action<FileCleansed> FileCleansed;
        public event Action<FileProcessed> FileProcessed;

        public void HandleFileAddressGeocoded(FileAddressGeocoded eventMessage)
        {
            bool isKnownCustomerFileName = eventMessage.IsKnownCustomerFileName;
            bool isValidFileType = eventMessage.IsValidFileType;
            bool isFileSchemaValid = eventMessage.IsFileSchemaValid;
            FrameworkUAD.Object.ValidationResult validationResult = eventMessage.ValidationResult;

            if (eventMessage.IsFileSchemaValid == true)
                RunStandardization(eventMessage.Client, eventMessage.AdmsLog, eventMessage.SourceFile, eventMessage.ImportFile, isKnownCustomerFileName, isValidFileType, isFileSchemaValid, validationResult);
            else
            {
                //send not valid email and exit
                Emailer.Emailer emailer = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.SendFileNotValidMessage);
                emailer.SendFileNotValidMessage(eventMessage.Client, eventMessage.SourceFile, eventMessage.ValidationResult.UnexpectedColumns.ToList(), eventMessage.ValidationResult.NotFoundColumns.ToList(), eventMessage.ValidationResult.DuplicateColumns.ToList(), eventMessage.ImportFile, eventMessage.IsKnownCustomerFileName, eventMessage.IsValidFileType, eventMessage.IsFileSchemaValid, eventMessage.ValidationResult, eventMessage.AdmsLog.ProcessCode);
                ConsoleMessage("File not valid, processing stopped, invalid file notification sent...." + DateTime.Now.ToString());

                ThreadDictionary.Remove(Thread.CurrentThread.ManagedThreadId);
                //ADMSProcessingQue.RemoveClientFile(eventMessage.Client, eventMessage.ImportFile);
                ConsoleMessage("Awaiting new file....");
            }
        }
        public string RunStandardization(KMPlatform.Entity.Client _client, FrameworkUAS.Entity.AdmsLog _admsLog, FrameworkUAS.Entity.SourceFile _sourceFile, FileInfo fileInfo = null, bool isKnownCustomerFileName = true, bool isValidFileType = true, bool isFileSchemaValid = true, FrameworkUAD.Object.ValidationResult validationResult = null)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                if (_sourceFile.IsDQMReady == true)
                {
                    HashSet<FrameworkUAS.Object.RuleSet> ruleSet = new HashSet<FrameworkUAS.Object.RuleSet>();
                    FrameworkUAD.BusinessLogic.SubscriberTransformed stWorker = new FrameworkUAD.BusinessLogic.SubscriberTransformed();
                    //stWorker.EnableTableIndexes(_client.ClientConnections, _sourceFile.SourceFileID, _admsLog.ProcessCode);

                    AdmsLog admsLogWrk = new AdmsLog();
                    try
                    {
                        // Standard Country and Region(State) cleanse
                        AddressClean ac = new AddressClean();
                        ac.CountryRegionCleanse(_sourceFile.SourceFileID, _admsLog.ProcessCode, _client);
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization_AddressClean");
                    }

                    #region Get FileType for later use
                    FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                    FrameworkUAD_Lookup.Entity.Code dbft = cWorker.SelectCodeId(_sourceFile.DatabaseFileTypeId);
                    FrameworkUAD_Lookup.Enums.FileTypes dft = FrameworkUAD_Lookup.Enums.GetDatabaseFileType(dbft.CodeName);
                    #endregion

                    #region DQM Stored Procedures
                    #region Pre DQM Scripts
                    try
                    {
                        ClientMethods.ClientSpecialCommon csc = new ClientMethods.ClientSpecialCommon();
                        if (BillTurner.ClientAdditionalProperties != null && BillTurner.ClientAdditionalProperties.ContainsKey(_client.ClientID))
                        {
                            csc.ExecuteClientCustomCode(FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_DQM, _client, BillTurner.ClientAdditionalProperties[_client.ClientID].ClientCustomProceduresList, _sourceFile, _admsLog.ProcessCode);
                        }
                        else
                        {
                            FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                            FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                            cap = capWork.SetObjects(_client.ClientID, false);
                            csc.ExecuteClientCustomCode(FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_DQM, _client, cap.ClientCustomProceduresList, _sourceFile, _admsLog.ProcessCode);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization_Pre_DQM");
                    }
                    #endregion

                    #region Save data from Subscriber Transformed to Subscriber Final
                    result.AppendLine("Process Code: " + _admsLog.ProcessCode);
                    //Move records to subscriberFinal
                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Start Insert to SubscriberFinal - ProcessCode: " + _admsLog.ProcessCode, _admsLog.ProcessCode, true, _admsLog.SourceFileId);
                    FrameworkUAD.BusinessLogic.SubscriberFinal sf = new FrameworkUAD.BusinessLogic.SubscriberFinal();
                    sf.SaveDQMClean(_client.ClientConnections, _admsLog.ProcessCode, dft.ToString());
                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Done  Insert to SubscriberFinal", _admsLog.ProcessCode, true, _admsLog.SourceFileId);
                    #endregion

                    #region Pre DataMatch Scripts
                    try
                    {
                        ClientMethods.ClientSpecialCommon csc = new ClientMethods.ClientSpecialCommon();
                        if (BillTurner.ClientAdditionalProperties != null && BillTurner.ClientAdditionalProperties.ContainsKey(_client.ClientID))
                        {
                            csc.ExecuteClientCustomCode(FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_DataMatch, _client, BillTurner.ClientAdditionalProperties[_client.ClientID].ClientCustomProceduresList, _sourceFile, _admsLog.ProcessCode);
                        }
                        else
                        {
                            FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                            FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                            cap = capWork.SetObjects(_client.ClientID, false);
                            csc.ExecuteClientCustomCode(FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_DataMatch, _client, cap.ClientCustomProceduresList, _sourceFile, _admsLog.ProcessCode);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization_Pre_DataMatch");
                    }
                    #endregion

                    #region Nullify KMPSGroup email addresses for specific clients
                    if (_client.FtpFolder.Equals(FrameworkUAS.BusinessLogic.Enums.Clients.Meister.ToString()))
                    {
                        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Start Nullify KMPSGroup Emails - ProcessCode: " + _admsLog.ProcessCode, _admsLog.ProcessCode, true);
                        sf = new FrameworkUAD.BusinessLogic.SubscriberFinal();
                        sf.NullifyKMPSGroupEmails(_client.ClientConnections, _admsLog.ProcessCode);
                        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Done  Nullify KMPSGroup Emails", _admsLog.ProcessCode, true);
                    }
                    #endregion

                    #region Call Pre-DataMatching for Circ FileTypes                    
                    if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Long_Form || dft == FrameworkUAD_Lookup.Enums.FileTypes.Telemarketing_Short_Form
                        || dft == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms || dft == FrameworkUAD_Lookup.Enums.FileTypes.Web_Forms_Short_Form
                        || dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_2YR || dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_3YR
                        || dft == FrameworkUAD_Lookup.Enums.FileTypes.List_Source_Other)
                    {
                        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Start Pre-DataMatching for Telemarketing/Web Forms/List Source - ProcessCode: " + _admsLog.ProcessCode, _admsLog.ProcessCode, true);
                        FrameworkUAD.BusinessLogic.CircIntegration ciWorker = new FrameworkUAD.BusinessLogic.CircIntegration();
                        ciWorker.CircFileTypeUpdateIGrpBySequence(_admsLog.ProcessCode, dft, _client.ClientConnections);
                        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Done Pre-DataMatching for Telemarketing/Web Forms/List Source - ProcessCode: " + _admsLog.ProcessCode, _admsLog.ProcessCode, true);
                    }
                    #endregion

                    #region DataMatching - need to create rules for existing fileTypes before going live
                    #region old data matching
                    //#region FieldUpdateDataMatching
                    ////dft defined above
                    //if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Field_Update)
                    //{
                    //    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Begin Field Update Data Matching process - ProcessCode: " + _admsLog.ProcessCode, _admsLog.ProcessCode, true);
                    //    try
                    //    {
                    //        FrameworkUAD.BusinessLogic.CircIntegration ciWorker = new FrameworkUAD.BusinessLogic.CircIntegration();
                    //        ciWorker.FieldUpdateDataMatching(_client.ClientConnections, _admsLog.ProcessCode);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization");
                    //    }
                    //    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - End Field Update Data Matching process", _admsLog.ProcessCode, true);
                    //}
                    //#endregion
                    //#region QuickFill DataMatching
                    //else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.QuickFill || dft == FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction)
                    //{
                    //    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Begin Quick Fill Data Matching process - ProcessCode: " + _admsLog.ProcessCode, _admsLog.ProcessCode, true);
                    //    try
                    //    {
                    //        FrameworkUAD.BusinessLogic.CircIntegration ciWorker = new FrameworkUAD.BusinessLogic.CircIntegration();
                    //        ciWorker.QuickFillDataMatching(_client.ClientConnections, _admsLog.ProcessCode);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization");
                    //    }
                    //    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - End Quick Fill Data Matching process", _admsLog.ProcessCode, true);
                    //}
                    //#endregion
                    //#region Normal DataMatching
                    //else
                    //{
                    //    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Begin Data Matching process - ProcessCode: " + _admsLog.ProcessCode, _admsLog.ProcessCode, true);
                    //    try
                    //    {
                    //        stWorker.DataMatching(_client.ClientConnections, _sourceFile.SourceFileID, _admsLog.ProcessCode);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization.DataMatch_old");
                    //    }
                    //    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - End Data Matching process", _admsLog.ProcessCode, true);
                    //}
                    //#endregion
                    #endregion

                    #region Rule based DataMatching
                    //will need a business rule that only One Datamatching RuleSet can be set per SourceFile
                    ruleSet = GetRuleSetsByExecutionPoint(FrameworkUAD_Lookup.Enums.ExecutionPointType.DataMatch, _sourceFile);
                    #region FieldUpdateDataMatching
                    //dft defined above
                    if (dft == FrameworkUAD_Lookup.Enums.FileTypes.Field_Update)
                    {
                        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Begin Field Update Data Matching process - ProcessCode: " + _admsLog.ProcessCode, _admsLog.ProcessCode, true);
                        try
                        {
                            FrameworkUAD.BusinessLogic.CircIntegration ciWorker = new FrameworkUAD.BusinessLogic.CircIntegration();
                            ciWorker.FieldUpdateDataMatching(_client.ClientConnections, _admsLog.ProcessCode);
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization");
                        }
                        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - End Field Update Data Matching process", _admsLog.ProcessCode, true);
                    }
                    #endregion
                    #region QuickFill DataMatching
                    else if (dft == FrameworkUAD_Lookup.Enums.FileTypes.QuickFill || dft == FrameworkUAD_Lookup.Enums.FileTypes.Paid_Transaction)
                    {
                        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Begin Quick Fill Data Matching process - ProcessCode: " + _admsLog.ProcessCode, _admsLog.ProcessCode, true);
                        try
                        {
                            FrameworkUAD.BusinessLogic.CircIntegration ciWorker = new FrameworkUAD.BusinessLogic.CircIntegration();
                            ciWorker.QuickFillDataMatching(_client.ClientConnections, _admsLog.ProcessCode);
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization");
                        }
                        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - End Quick Fill Data Matching process", _admsLog.ProcessCode, true);
                    }
                    #endregion
                    #region Normal DataMatching
                    else
                    {
                        if (ruleSet.First().Rules.Count > 1)
                        {
                            StringBuilder sbMatch = new StringBuilder();
                            foreach (var r in ruleSet.First().Rules)
                            {
                                sbMatch.Append(r.RuleName.ToString() + ",");
                            }

                            ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Begin Data Matching multiple - ProcessCode: " + _admsLog.ProcessCode, _admsLog.ProcessCode, true);
                            try
                            {
                                stWorker.DataMatching_multiple(_client.ClientConnections, _sourceFile.SourceFileID, _admsLog.ProcessCode, sbMatch.ToString().Trim().TrimEnd(','));
                            }
                            catch (Exception ex)
                            {
                                LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization.DataMatchMultiple");
                            }
                            ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - End Data Matching single", _admsLog.ProcessCode, true);
                        }
                        else if (ruleSet.First().Rules.Count == 1)
                        {
                            ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Begin Data Matching single - ProcessCode: " + _admsLog.ProcessCode, _admsLog.ProcessCode, true);
                            try
                            {
                                stWorker.DataMatching_single(_client.ClientConnections, _admsLog.ProcessCode, ruleSet.First().Rules.First().RuleName);
                            }
                            catch (Exception ex)
                            {
                                LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization.DataMatchSingle");
                            }
                            ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - End Data Matching single", _admsLog.ProcessCode, true);
                        }

                        //ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Begin Data Matching process - ProcessCode: " + _admsLog.ProcessCode, _admsLog.ProcessCode, true);
                        //try
                        //{
                        //    stWorker.DataMatching(_client.ClientConnections, _sourceFile.SourceFileID, _admsLog.ProcessCode);
                        //}
                        //catch (Exception ex)
                        //{
                        //    LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization.DataMatch_old");
                        //}
                        //ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - End Data Matching process", _admsLog.ProcessCode, true);
                    }
                    #endregion



                    
                    #endregion
                    #endregion

                    #region Post DataMatch Scripts
                    try
                    {
                        ClientMethods.ClientSpecialCommon csc = new ClientMethods.ClientSpecialCommon();
                        if (BillTurner.ClientAdditionalProperties != null && BillTurner.ClientAdditionalProperties.ContainsKey(_client.ClientID))
                        {
                            csc.ExecuteClientCustomCode(FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_DataMatch, _client, BillTurner.ClientAdditionalProperties[_client.ClientID].ClientCustomProceduresList, _sourceFile, _admsLog.ProcessCode);
                        }
                        else
                        {
                            FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                            FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                            cap = capWork.SetObjects(_client.ClientID, false);
                            csc.ExecuteClientCustomCode(FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_DataMatch, _client, cap.ClientCustomProceduresList, _sourceFile, _admsLog.ProcessCode);

                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization_Post_DataMatch");
                    }
                    #endregion

                    //update FileStatusID to 'Cleansed'
                    admsLogWrk.UpdateProcessingStatus(_admsLog.ProcessCode, FrameworkUAD_Lookup.Enums.ProcessingStatusType.Cleansed, 1, "Cleansed", true, _admsLog.SourceFileId);

                    #region Pre Suppression Scripts
                    try
                    {
                        ClientMethods.ClientSpecialCommon csc = new ClientMethods.ClientSpecialCommon();
                        if (BillTurner.ClientAdditionalProperties != null && BillTurner.ClientAdditionalProperties.ContainsKey(_client.ClientID))
                        {
                            csc.ExecuteClientCustomCode(FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_Suppression, _client, BillTurner.ClientAdditionalProperties[_client.ClientID].ClientCustomProceduresList, _sourceFile, _admsLog.ProcessCode);
                        }
                        else
                        {
                            FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                            FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                            cap = capWork.SetObjects(_client.ClientID, false);
                            csc.ExecuteClientCustomCode(FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_Suppression, _client, cap.ClientCustomProceduresList, _sourceFile, _admsLog.ProcessCode);

                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization_Pre_Suppression");
                    }
                    #endregion

                    #region Suppression
                    KMPlatform.BusinessLogic.Client cData = new KMPlatform.BusinessLogic.Client();
                    if (cData.UseUADSuppressionFeature(_client.ClientID) == true)
                        PerformSuppression(_admsLog);
                    #endregion

                    #region Post Suppression Scripts
                    try
                    {
                        ClientMethods.ClientSpecialCommon csc = new ClientMethods.ClientSpecialCommon();
                        if (BillTurner.ClientAdditionalProperties != null && BillTurner.ClientAdditionalProperties.ContainsKey(_client.ClientID))
                        {
                            csc.ExecuteClientCustomCode(FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_Suppression, _client, BillTurner.ClientAdditionalProperties[_client.ClientID].ClientCustomProceduresList, _sourceFile, _admsLog.ProcessCode);
                        }
                        else
                        {
                            FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                            FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                            cap = capWork.SetObjects(_client.ClientID, false);
                            csc.ExecuteClientCustomCode(FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_Suppression, _client, cap.ClientCustomProceduresList, _sourceFile, _admsLog.ProcessCode);

                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization_Post_Suppression");
                    }
                    #endregion

                    #region Perform standard roll up to master records
                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Start roll up to master - ProcessCode: " + _admsLog.ProcessCode, _admsLog.ProcessCode, true);
                    //stWorker.StandardRollUpToMaster(_client.ClientConnections, _sourceFile.SourceFileID, _admsLog.ProcessCode);

                    //will want a method to get a list of rules by sourceFileId by ExecutionPointId - order by ExecutionOrder
                    //looking for FrameworkUAD_Lookup.Enums.ExecutionPointType.ConsensusDataRollup   RULE - system rule
                    ruleSet = GetRuleSetsByExecutionPoint(FrameworkUAD_Lookup.Enums.ExecutionPointType.ConsensusDataRollup, _sourceFile);

                    //get each parameter value and pass to a new method
                    #region set parameters - defaults match what KM has historically done 
                    bool mailPermissionOverRide = false;
                    bool faxPermissionOverRide = false;
                    bool phonePermissionOverRide = false;
                    bool otherProductsPermissionOverRide = false;
                    bool thirdPartyPermissionOverRide = false;
                    bool emailRenewPermissionOverRide = false;
                    bool textPermissionOverRide = false;
                    bool updateEmail = true;
                    bool updatePhone = true;
                    bool updateFax = true;
                    bool updateMobile = true;

                    bool parseBool = false;
                    foreach(var rs in ruleSet)
                    {
                        foreach(var r in rs.Rules)
                        {
                            if (r.RuleValues.Count > 0)
                            {
                                switch (r.RuleName)
                                {
                                    case "MailPermissionOverRide":
                                        bool.TryParse(r.RuleValues.First().Value.ToString(), out parseBool);
                                        mailPermissionOverRide = parseBool;
                                        break;
                                    case "FaxPermissionOverRide":
                                        bool.TryParse(r.RuleValues.First().Value.ToString(), out parseBool);
                                        faxPermissionOverRide = parseBool;
                                        break;
                                    case "PhonePermissionOverRide":
                                        bool.TryParse(r.RuleValues.First().Value.ToString(), out parseBool);
                                        phonePermissionOverRide = parseBool;
                                        break;
                                    case "OtherProductsPermissionOverRide":
                                        bool.TryParse(r.RuleValues.First().Value.ToString(), out parseBool);
                                        otherProductsPermissionOverRide = parseBool;
                                        break;
                                    case "ThirdPartyPermissionOverRide":
                                        bool.TryParse(r.RuleValues.First().Value.ToString(), out parseBool);
                                        thirdPartyPermissionOverRide = parseBool;
                                        break;
                                    case "EmailRenewPermissionOverRide":
                                        bool.TryParse(r.RuleValues.First().Value.ToString(), out parseBool);
                                        emailRenewPermissionOverRide = parseBool;
                                        break;
                                    case "TextPermissionOverRide":
                                        bool.TryParse(r.RuleValues.First().Value.ToString(), out parseBool);
                                        textPermissionOverRide = parseBool;
                                        break;
                                    case "UpdateEmail":
                                        bool.TryParse(r.RuleValues.First().Value.ToString(), out parseBool);
                                        updateEmail = parseBool;
                                        break;
                                    case "UpdatePhone":
                                        bool.TryParse(r.RuleValues.First().Value.ToString(), out parseBool);
                                        updatePhone = parseBool;
                                        break;
                                    case "UpdateFax":
                                        bool.TryParse(r.RuleValues.First().Value.ToString(), out parseBool);
                                        updateFax = parseBool;
                                        break;
                                    case "UpdateMobile":
                                        bool.TryParse(r.RuleValues.First().Value.ToString(), out parseBool);
                                        updateMobile = parseBool;
                                        break;
                                }
                            }
                        }
                    }
                    #endregion

                    stWorker.StandardRollUpToMaster(_client.ClientConnections, _sourceFile.SourceFileID, _admsLog.ProcessCode, mailPermissionOverRide,
                        faxPermissionOverRide,phonePermissionOverRide,otherProductsPermissionOverRide,thirdPartyPermissionOverRide,emailRenewPermissionOverRide,
                        textPermissionOverRide,updateEmail,updatePhone,updateFax,updateMobile);


                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Done roll up to master", _admsLog.ProcessCode, true);
                    #endregion
                    #region third party suppression
                    if ((_client.FtpFolder == "SAETB" || _client.FtpFolder == "Meister") && !_sourceFile.FileName.Equals("UAD_WS_AddSubscriber", StringComparison.CurrentCultureIgnoreCase))
                    {
                        try
                        {
                            FrameworkUAD.BusinessLogic.SubscriberFinal sfWorker = new FrameworkUAD.BusinessLogic.SubscriberFinal();
                            List<int> groupids = new List<int>();
                            if (_client.FtpFolder == "SAETB")
                            {
                                groupids.Add(333250);
                            }
                            else if (_client.FtpFolder == "Meister")
                            {
                                groupids.Add(181967);
                                groupids.Add(193397);
                            }
                            sfWorker.ECN_ThirdPartySuppresion(_client.ClientConnections, _admsLog.ProcessCode, groupids);
                        }
                        catch(Exception ex)
                        {
                            result.AppendLine("An unexpected exception occured during data cleansing. Customer support has been notified of this issue.");
                            LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization_ThirdPartySuppression");
                            return result.ToString();
                        }
                    }
                    #endregion

                    #region Post DQM Scripts
                    try
                    {
                        ClientMethods.ClientSpecialCommon csc = new ClientMethods.ClientSpecialCommon();
                        if (BillTurner.ClientAdditionalProperties != null && BillTurner.ClientAdditionalProperties.ContainsKey(_client.ClientID))
                        {
                            csc.ExecuteClientCustomCode(FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_DQM, _client, BillTurner.ClientAdditionalProperties[_client.ClientID].ClientCustomProceduresList, _sourceFile, _admsLog.ProcessCode);
                        }
                        else
                        {
                            FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                            FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                            cap = capWork.SetObjects(_client.ClientID, false);
                            csc.ExecuteClientCustomCode(FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_DQM, _client, cap.ClientCustomProceduresList, _sourceFile, _admsLog.ProcessCode);

                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization_PostDQMScripts");
                    }
                    #endregion

                    #region Execute Custom Rules
                    //FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_DQM
                    #endregion

                    #region update ADMS Log counts
                    //update AdmsLog counts MatchedRecordCount  UadConsensusCount
                    //lets set the AdmsLog.Final counts
                    //need to query SubscriberFinal by ProcessCode
                    ConsoleMessage(_client.FtpFolder + " - Update SubscriberFinal counts " + DateTime.Now.TimeOfDay.ToString());
                    FrameworkUAD.BusinessLogic.SubscriberFinal sourceFileWrk = new FrameworkUAD.BusinessLogic.SubscriberFinal();
                    FrameworkUAD.Object.AdmsResultCount counts = sourceFileWrk.SelectResultCount(_admsLog.ProcessCode, _client.ClientConnections);
                    admsWrk.UpdateFinalCounts(_admsLog.ProcessCode, counts.FinalProfileCount, counts.FinalProfileCount, counts.FinalDemoCount, counts.MatchedRecordCount, counts.UadConsensusCount, 1, true, _admsLog.SourceFileId, counts.ArchiveProfileCount);
                    _admsLog.FinalRecordCount = counts.FinalProfileCount;
                    _admsLog.FinalProfileCount = counts.FinalProfileCount;
                    _admsLog.FinalDemoCount = counts.FinalDemoCount;
                    _admsLog.MatchedRecordCount = counts.MatchedRecordCount;
                    _admsLog.UadConsensusCount = counts.UadConsensusCount;
                    #endregion
                    #endregion

                    //raise FileProcessed
                    if (fileInfo != null)
                    {
                        FileCleansed fileCleansedDetails = new FileCleansed(fileInfo, _client, _sourceFile, _admsLog, isFileSchemaValid, isKnownCustomerFileName, isValidFileType, validationResult);
                        FileCleansed(fileCleansedDetails);
                    }
                    else
                    {
                        //Call UAD
                        UAD.UADProcessor uad = new UAD.UADProcessor();
                        uad.ImportToUAD(_client, _admsLog, dft, _sourceFile);
                    }

                    return result.ToString();
                }
                else
                {
                    //FrameworkUAS.BusinessLogic.FileStatus fsData = new FrameworkUAS.BusinessLogic.FileStatus();
                    //fsData.SetStatus(fileStatus.FileStatusID, FrameworkUAS.BusinessLogic.Enums.FileStatusTypes.Processed, DateTime.Now, 1);
                    ConsoleMessage("Not ready for DQM....", _admsLog.ProcessCode, false);
                    ConsoleMessage("Awaiting new file....", _admsLog.ProcessCode, false);
                    ThreadDictionary.Remove(Thread.CurrentThread.ManagedThreadId);
                    //ADMSProcessingQue.RemoveClientFile(_client, fileInfo);
                    result.AppendLine("Not ready for DQM....Please ensure the Add Subscriber feature is enabled for your account.");
                    FileProcessed processed = new FileProcessed(_client, _sourceFile.SourceFileID, _admsLog, fileInfo, isKnownCustomerFileName, isValidFileType, isFileSchemaValid, validationResult);
                    FileProcessed(processed);
                    return result.ToString();
                }
            }
            catch (Exception ex)
            {
                #region handle exception
                result.AppendLine("An unexpected exception occured during data cleansing. Customer support has been notified of this issue.");
                LogError(ex, _client, this.GetType().Name.ToString() + ".RunStandardization_OuterHandler");
                return result.ToString();
                #endregion
            }
        }

        public void PerformSuppression(KMPlatform.Entity.Client client, FrameworkUAS.Entity.AdmsLog admsLog)
        {
            try
            {
                ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Begin Suppression process", admsLog.ProcessCode, true);
                if (client.FtpFolder.Equals("TenMissions", StringComparison.CurrentCultureIgnoreCase))
                {
                    ClientMethods.TenMissions tm = new ClientMethods.TenMissions();
                    tm.DonoSuppression(client, admsLog.SourceFileId, admsLog.ProcessCode);
                }

                #region Standard Suppression Process
                FrameworkUAS.BusinessLogic.ClientFTP cftpData = new FrameworkUAS.BusinessLogic.ClientFTP();
                FrameworkUAS.Entity.ClientFTP cFTP = cftpData.SelectClient(client.ClientID).FirstOrDefault();

                if (cFTP == null)
                {
                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - No client ftp", admsLog.ProcessCode, true);
                    StringBuilder sbDetail = new StringBuilder();
                    sbDetail.AppendLine("Client: " + client.FtpFolder);
                    sbDetail.AppendLine("Thread: " + Thread.CurrentThread.ManagedThreadId.ToString());
                    sbDetail.AppendLine("ProcessCode: " + admsLog.ProcessCode.ToString());
                    sbDetail.AppendLine("ADMS.Services.DataCleanser.DQMCleaner - RunStandardization");
                    sbDetail.AppendLine("No client FTP setup for client");
                    sbDetail.AppendLine(System.Environment.NewLine);
                    Emailer.Emailer emWorker = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.EmailError);
                    emWorker.EmailError(sbDetail.ToString(), admsLog.ProcessCode.ToString(), admsLog.SourceFileId);
                }
                else
                {
                    string host = cFTP.Server + "/ADMS/Suppression";
                    string localDir = @"C:\ADMS\Suppression\" + client.ClientName + @"\";
                    if (!System.IO.Directory.Exists(localDir))
                        System.IO.Directory.CreateDirectory(localDir);

                    Core_AMS.Utilities.FtpFunctions ftp = new Core_AMS.Utilities.FtpFunctions(host, cFTP.UserName, cFTP.Password);

                    // Check for any new files on FTP site
                    string[] ftpDirfileNames = ftp.DirectoryListSimple("");

                    FileWorker fileWorker = new FileWorker();
                    int fileTotalRowCount = 0;
                    //int fileRowProcessedCount = 0;
                    FrameworkUAD.Object.ImportFile dataIV = new FrameworkUAD.Object.ImportFile();

                    FileConfiguration fileConfig = new FileConfiguration()
                    {
                        FileColumnDelimiter = "tab",
                        IsQuoteEncapsulated = true,
                    };

                    if (ftpDirfileNames == null || ftpDirfileNames.Length == 0 || ftpDirfileNames[0] == "")
                    {
                        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - No new suppression file(s) detected", admsLog.ProcessCode, true);
                    }
                    else
                    {
                        foreach (var needToDL in ftpDirfileNames)
                        {
                            if (!string.IsNullOrEmpty(needToDL))
                            {
                                try
                                {
                                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Downloading suppression file: " + needToDL.ToString(), admsLog.ProcessCode, true);
                                    ftp.Download(needToDL, localDir + needToDL);
                                    //ftp.Delete(needToDL);
                                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Download complete", admsLog.ProcessCode, true);
                                }
                                catch (Exception ex) { LogError(ex, client, this.GetType().Name.ToString() + ".PerformSuppression - Download files"); }
                            }
                        }
                    }

                    List<string> suppDirectory = new List<string>(Directory.EnumerateFiles(localDir));
                    List<FrameworkUAD.Entity.SubscriberFinal> sfList = new List<FrameworkUAD.Entity.SubscriberFinal>();
                    FrameworkUAD.BusinessLogic.Suppressed callSuppSF = new FrameworkUAD.BusinessLogic.Suppressed();

                    foreach (var localSuppFile in suppDirectory)
                    {
                        try
                        {
                            if (File.Exists(localSuppFile))
                            {
                                FileInfo suppfileName = new FileInfo(localSuppFile);

                                ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Loading file " + suppfileName.Name + " to DataTable", admsLog.ProcessCode, true);

                                #region Load Data to DataTable
                                fileTotalRowCount = fileWorker.GetRowCount(suppfileName);
                                dataIV.ProcessFile = suppfileName;
                                dataIV.TotalRowCount = fileTotalRowCount;
                                dataIV.HasError = false;
                                dataIV.ImportErrors = new HashSet<FrameworkUAD.Entity.ImportError>();
                                FrameworkUAD.BusinessLogic.ImportFile ifWorker = new FrameworkUAD.BusinessLogic.ImportFile();

                                #region get DatTable
                                //loads the data from the file
                                dataIV = ifWorker.GetImportFile(suppfileName, fileConfig);
                                #endregion

                                #endregion

                                // Load data to st list, if multiple files, all data will load to same list until done
                                FrameworkUAS.BusinessLogic.SourceFile sfWorker = new SourceFile();
                                FrameworkUAS.Entity.SourceFile sf = sfWorker.Select(client.ClientID, suppfileName.Name, false);//.SelectSourceFileID(sourceFileId);
                                if (sf != null && sf.SourceFileID > 0)
                                {
                                    foreach (var key in dataIV.DataOriginal.Keys)
                                    {
                                        StringDictionary myRow = dataIV.DataOriginal[key];
                                        FrameworkUAD.Entity.SubscriberFinal newSF = new FrameworkUAD.Entity.SubscriberFinal();
                                        #region get row values
                                        foreach (var fm in sf.FieldMappings)
                                        {
                                            switch (fm.MAFField)
                                            {
                                                case "LName":
                                                    newSF.LName = myRow[fm.IncomingField].ToString();
                                                    break;
                                                case "FName":
                                                    newSF.FName = myRow[fm.IncomingField].ToString();
                                                    break;
                                                case "Company":
                                                    newSF.Company = myRow[fm.IncomingField].ToString();
                                                    break;
                                                case "Address":
                                                    newSF.Address = myRow[fm.IncomingField].ToString();
                                                    break;
                                                case "City":
                                                    newSF.City = myRow[fm.IncomingField].ToString();
                                                    break;
                                                case "State":
                                                    newSF.State = myRow[fm.IncomingField].ToString();
                                                    break;
                                                case "Zip":
                                                    newSF.Zip = myRow[fm.IncomingField].ToString();
                                                    break;
                                                case "Plus4":
                                                    newSF.Plus4 = myRow[fm.IncomingField].ToString();
                                                    break;
                                                case "Phone":
                                                    newSF.Phone = myRow[fm.IncomingField].ToString();
                                                    break;
                                                case "Fax":
                                                    newSF.Fax = myRow[fm.IncomingField].ToString();
                                                    break;
                                                case "Email":
                                                    newSF.Email = myRow[fm.IncomingField].ToString();
                                                    break;
                                            }
                                        }
                                        #endregion
                                        sfList.Add(newSF);
                                    }
                                }
                                foreach (var x in sfList)
                                {
                                    // Clean up phone numbers
                                    x.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Phone);
                                    x.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Fax);
                                    if (!string.IsNullOrEmpty(x.Plus4))
                                    {
                                        x.Zip = x.Zip + " " + x.Plus4;
                                    }
                                    // Try to reformat
                                    x.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(x.Phone);
                                    x.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(x.Fax);
                                }
                                ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Loading Complete", admsLog.ProcessCode, true);
                                if (sfList.Count > 0)
                                {
                                    // Once all data loaded to list, we can send list to suppression proc
                                    int suppCount;
                                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Perform data suppression", admsLog.ProcessCode, true);
                                    AdmsLog alWrk = new AdmsLog();
                                    //update the AdmsLog items
                                    alWrk.Update(admsLog.ProcessCode, FrameworkUAD_Lookup.Enums.FileStatusType.Processing, FrameworkUAD_Lookup.Enums.ADMS_StepType.Suppression, FrameworkUAD_Lookup.Enums.ProcessingStatusType.Suppression, FrameworkUAD_Lookup.Enums.ExecutionPointType.Pre_Suppression, 1);

                                    suppCount = callSuppSF.PerformSuppression(sfList, client.ClientConnections, admsLog.SourceFileId, admsLog.ProcessCode, suppfileName.Name);
                                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Suppression Count: " + suppCount, admsLog.ProcessCode, true);

                                    // Clean up data table,list and delete supp file
                                    dataIV.DataOriginal.Clear();
                                    sfList.Clear();
                                    File.Delete(suppfileName.FullName);
                                }
                                else
                                {
                                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Suppression file not parsed properly or no data", admsLog.ProcessCode, true);
                                    // Clean up data table,list and delete supp file
                                    dataIV.DataOriginal.Clear();
                                    sfList.Clear();
                                    File.Delete(suppfileName.FullName);
                                }
                            }
                        }
                        catch (Exception ex) { LogError(ex, client, this.GetType().Name.ToString() + ".PerformSuppression - process local files"); }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".PerformSuppression");
            }
        }
        public void PerformSuppression(FrameworkUAS.Entity.AdmsLog admsLog)
        {
            try
            {
                if (client.FtpFolder.Equals("TenMissions", StringComparison.CurrentCultureIgnoreCase))
                {
                    ClientMethods.TenMissions tm = new ClientMethods.TenMissions();
                    tm.DonoSuppression(client, admsLog.SourceFileId, admsLog.ProcessCode);
                }

                //1. check for Files in FTP Site
                //2. bring down locally
                //3. check if file name in SupressionFile table
                //4. if not insert file to SuppressionFile table then bring file contents into SuppressionFileDetail
                //5. if file name exists compare tables FileDateModified vs files DateModified attribute
                //      a. if both are same do nothing
                //      b. if files DateModified is newer than table entry then DELETE SupressionFileDetail for SuppressionFile then reload new data
                //6. Call existing job_Suppression stored procedure but change sproc to use SuppressionFileDetail vs temp table created from xml

                #region ListsAndWorkers
                FrameworkUAS.BusinessLogic.ClientFTP cftpData = new FrameworkUAS.BusinessLogic.ClientFTP();
                FrameworkUAS.Entity.ClientFTP cFTP = cftpData.SelectClient(client.ClientID).FirstOrDefault();
                FrameworkUAD.BusinessLogic.SuppressionFile SupWorker = new FrameworkUAD.BusinessLogic.SuppressionFile();
                FrameworkUAD.BusinessLogic.SuppressionFileDetail fileDetailWorker = new FrameworkUAD.BusinessLogic.SuppressionFileDetail();
                List<FrameworkUAD.Entity.SuppressionFile> SupFileList = new List<FrameworkUAD.Entity.SuppressionFile>();
                List<FrameworkUAD.Entity.SuppressionFileDetail> spdList = new List<FrameworkUAD.Entity.SuppressionFileDetail>();
                #endregion

                if (cFTP != null)
                {
                    #region ftpInteraction
                    string host = cFTP.Server + "/ADMS/Suppression";
                    string localDir = @"C:\ADMS\Suppression\" + client.FtpFolder + @"\";
                    if (!System.IO.Directory.Exists(localDir))
                        System.IO.Directory.CreateDirectory(localDir);

                    Core_AMS.Utilities.FtpFunctions ftp = new Core_AMS.Utilities.FtpFunctions(host, cFTP.UserName, cFTP.Password);

                    string[] ftpDirfileNames = ftp.DirectoryListSimple("");

                    FileWorker fileWorker = new FileWorker();
                    int fileTotalRowCount = 0;
                    //int fileRowProcessedCount = 0;
                    FrameworkUAD.Object.ImportFile dataIV = new FrameworkUAD.Object.ImportFile();

                    FileConfiguration fileConfig = new FileConfiguration()
                    {
                        FileColumnDelimiter = "tab",
                        IsQuoteEncapsulated = true,
                    };

                    if (ftpDirfileNames == null || ftpDirfileNames.Length == 0 || ftpDirfileNames[0] == "")
                    {
                        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - No new suppression file(s) detected", admsLog.ProcessCode, true);
                    }
                    else
                    {
                        foreach (var needToDL in ftpDirfileNames)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(needToDL))
                                {
                                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Downloading suppression file: " + needToDL.ToString(), admsLog.ProcessCode, true);
                                    ftp.Download(needToDL, localDir + needToDL);
                                    //ftp.Delete(needToDL);
                                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Download complete", admsLog.ProcessCode, true);
                                }
                            }
                            catch (Exception ex) { LogError(ex, client, this.GetType().Name.ToString() + ".PerformSuppression - Download files"); }
                        }
                    }
                    #endregion
                    List<string> suppDirectory = new List<string>(Directory.EnumerateFiles(localDir));

                    foreach (var localSuppFile in suppDirectory)
                    {
                        try
                        {
                            int suppressionFileId = 0;
                            bool AddFileDetails = false;

                            if (File.Exists(localSuppFile) && localSuppFile.EndsWith(".txt"))
                            {
                                #region CheckingLocalFiles
                                SupFileList = SupWorker.Select(client.ClientConnections);
                                FileInfo suppfileName = new FileInfo(localSuppFile);


                                if (!SupFileList.Exists(x => string.Equals(x.FileName, suppfileName.Name, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    AddFileDetails = true;
                                    FrameworkUAD.Entity.SuppressionFile suppFile = new FrameworkUAD.Entity.SuppressionFile();
                                    suppFile.FileName = suppfileName.Name.ToString();
                                    suppFile.FileDateModified = suppfileName.LastWriteTime;

                                    suppressionFileId = SupWorker.Save(suppFile, client.ClientConnections);
                                }
                                else
                                {
                                    FrameworkUAD.Entity.SuppressionFile matchingFile = SupFileList.SingleOrDefault(x => string.Equals(x.FileName, suppfileName.Name, StringComparison.CurrentCultureIgnoreCase));
                                    if (matchingFile.FileDateModified < suppfileName.LastWriteTime)
                                    {
                                        fileDetailWorker.deleteBySourceFileId(matchingFile, client.ClientConnections);
                                        suppressionFileId = matchingFile.SuppressionFileId;
                                        SupWorker.Save(matchingFile, client.ClientConnections);
                                        AddFileDetails = true;
                                    }
                                }
                                #endregion
                                if (AddFileDetails)
                                {
                                    #region Load Data to DataTable
                                    fileTotalRowCount = fileWorker.GetRowCount(suppfileName);
                                    dataIV.ProcessFile = suppfileName;
                                    dataIV.TotalRowCount = fileTotalRowCount;
                                    dataIV.HasError = false;
                                    dataIV.ImportErrors = new HashSet<FrameworkUAD.Entity.ImportError>();
                                    FrameworkUAD.BusinessLogic.ImportFile ifWorker = new FrameworkUAD.BusinessLogic.ImportFile();

                                    #region get DatTable
                                    //loads the data from the file
                                    dataIV = ifWorker.GetImportFile(suppfileName, fileConfig);
                                    #endregion

                                    #endregion
                                    #region MapData
                                    // Load data to st list, if multiple files, all data will load to same list until done
                                    FrameworkUAS.BusinessLogic.SourceFile sfWorker = new SourceFile();
                                    FrameworkUAS.Entity.SourceFile sf = sfWorker.Select(client.ClientID, suppfileName.Name, false);//.SelectSourceFileID(sourceFileId);
                                    if (sf != null && sf.SourceFileID > 0 && sf.IsDeleted == false)
                                    {
                                        foreach (var key in dataIV.DataOriginal.Keys)
                                        {
                                            StringDictionary myRow = dataIV.DataOriginal[key];
                                            FrameworkUAD.Entity.SuppressionFileDetail newSPD = new FrameworkUAD.Entity.SuppressionFileDetail();
                                            #region get row values
                                            foreach (var fm in sf.FieldMappings)
                                            {
                                                switch (fm.MAFField)
                                                {
                                                    case "LName":
                                                        newSPD.LastName = myRow[fm.IncomingField].ToString();
                                                        break;
                                                    case "FName":
                                                        newSPD.FirstName = myRow[fm.IncomingField].ToString();
                                                        break;
                                                    case "Company":
                                                        newSPD.Company = myRow[fm.IncomingField].ToString();
                                                        break;
                                                    case "Address":
                                                        newSPD.Address1 = myRow[fm.IncomingField].ToString();
                                                        break;
                                                    case "City":
                                                        newSPD.City = myRow[fm.IncomingField].ToString();
                                                        break;
                                                    case "State":
                                                        newSPD.RegionCode = myRow[fm.IncomingField].ToString();
                                                        break;
                                                    case "Zip":
                                                        newSPD.ZipCode = myRow[fm.IncomingField].ToString();
                                                        break;
                                                    case "Phone":
                                                        newSPD.Phone = myRow[fm.IncomingField].ToString();
                                                        break;
                                                    case "Fax":
                                                        newSPD.Fax = myRow[fm.IncomingField].ToString();
                                                        break;
                                                    case "Email":
                                                        newSPD.Email = myRow[fm.IncomingField].ToString();
                                                        break;
                                                }
                                            }
                                            #endregion
                                            spdList.Add(newSPD);
                                        }
                                    }
                                    else
                                    {
                                        foreach (var key in dataIV.DataOriginal.Keys)
                                        {
                                            StringDictionary myRow = dataIV.DataOriginal[key];
                                            spdList.Add(new FrameworkUAD.Entity.SuppressionFileDetail
                                            {
                                                FirstName = myRow["FName"].ToString(),
                                                LastName = myRow["LName"].ToString(),
                                                Company = myRow["Company"].ToString(),
                                                Address1 = myRow["Address"].ToString(),
                                                City = myRow["City"].ToString(),
                                                RegionCode = myRow["State"].ToString(),
                                                ZipCode = myRow["Zip"].ToString(),
                                                Phone = myRow["Phone"].ToString(),
                                                Fax = myRow["Fax"].ToString(),
                                                Email = myRow["Email"].ToString()

                                            });
                                        }
                                    }
                                    foreach (var x in spdList)
                                    {
                                        // Clean up phone numbers
                                        x.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Phone);
                                        x.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Fax);
                                        // Try to reformat
                                        x.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(x.Phone);
                                        x.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneWithDashes(x.Fax);
                                    }
                                    #endregion
                                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Loading Complete", admsLog.ProcessCode, true);
                                    #region InsertOrDisplayErrorMessage
                                    if (spdList.Count > 0)
                                    {
                                        //Replace with save
                                        FrameworkUAD.BusinessLogic.SuppressionFileDetail suppDetailWorker = new FrameworkUAD.BusinessLogic.SuppressionFileDetail();
                                        suppDetailWorker.SaveBulkInsert(spdList, client.ClientConnections, suppressionFileId);

                                        // Clean up data table,list and delete supp file
                                        dataIV.DataOriginal.Clear();
                                        spdList.Clear();
                                        File.Delete(suppfileName.FullName);
                                    }
                                    else
                                    {
                                        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Suppression file not parsed properly or no data", admsLog.ProcessCode, true);
                                        // Clean up data table,list and delete supp file
                                        dataIV.DataOriginal.Clear();
                                        spdList.Clear();
                                        File.Delete(suppfileName.FullName);
                                    }
                                    #endregion
                                }
                            }
                        }
                        catch (Exception ex) { LogError(ex, client, this.GetType().Name.ToString() + ".PerformSuppression - process local files"); }
                    }
                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Perform data suppression", admsLog.ProcessCode, true);
                    SupWorker.RunSuppression(client.ClientConnections, admsLog.ProcessCode);
                    ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Data suppression complete", admsLog.ProcessCode, true);
                }
            }
            catch (Exception ex) { LogError(ex, client, this.GetType().Name.ToString() + ".PerformSuppression - outer exception handler"); }
        }
    }
}
