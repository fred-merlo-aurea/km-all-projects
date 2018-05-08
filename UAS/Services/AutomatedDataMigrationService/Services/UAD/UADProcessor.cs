using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Core.ADMS.Events;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup;
using FrameworkUAS.Entity;
using FrameworkUAS.Object;
using KMPlatform.Entity;

namespace ADMS.Services.UAD
{
    public class UADProcessor : ServiceBase, IUADProcessor
    {
        public event Action<FileProcessed> FileProcessed;

        public void ImportToUAD(Client client, AdmsLog admsLog, Enums.FileTypes fileType, SourceFile sourceFile)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (admsLog == null)
            {
                throw new ArgumentNullException(nameof(admsLog));
            }

            if (sourceFile == null)
            {
                throw new ArgumentNullException(nameof(sourceFile));
            }

            SetClientAdditionalProperties(client);

            var threadId = Thread.CurrentThread.ManagedThreadId;
            try
            {
                var shouldProcessFurther = ProcessFileTypes(client, admsLog, fileType, sourceFile, sourceFile.FileName);
                if (!shouldProcessFurther)
                {
                    return;
                }

                // Pre UAD Import
                try
                {
                    ClientExecuteCustomCode(client, admsLog, sourceFile, Enums.ExecutionPointType.Pre_UAD_Import);
                }
                catch (Exception ex)
                {
                    LogError(ex, client, $"{GetType().Name}.ImportToUAD");
                }                

                TransferDataToUAD(client, admsLog, fileType, sourceFile);
                DedupeDataToUAD(client, admsLog);

                // Post UAD Import
                try
                {
                    ClientExecuteCustomCode(client, admsLog, sourceFile, Enums.ExecutionPointType.Post_UAD_Import);
                }
                catch (Exception ex)
                {
                    LogError(ex, client, $"{GetType().Name}.ImportToUAD");
                }                

                CalculateAdmsLogFinalCounts(client, admsLog, sourceFile, true);
            }
            catch (Exception ex)
            {
                LogError(ex, client, $"{GetType().Name}.ImportToUAD");
            }
            finally
            {
                admsWrk.Update(admsLog.ProcessCode, Enums.FileStatusType.Completed, Enums.ADMS_StepType.Watching_for_File, Enums.ProcessingStatusType.Watching_for_File, Enums.ExecutionPointType.Post_UAD_Import, 1, "Waiting for File", true, admsLog.SourceFileId, true);
                ThreadDictionary.Remove(threadId);
                SetPropertiesNull();
            }
        }

        private bool ProcessFileTypes(Client client, AdmsLog admsLog, Enums.FileTypes fileType, SourceFile sourceFile, string emailFileName)
        {
            switch (fileType)
            {
                case Enums.FileTypes.Complimentary:
                    RunAction("complimentary add", client, admsLog, sourceFile, ApplyComplimenaryLogic);
                    return false;

                case Enums.FileTypes.Other:
                    RunAction("ApplyCircOtherFileLogic", client, admsLog, sourceFile, ApplyCircOtherFileLogic);
                    return false;

                case Enums.FileTypes.Data_Compare:
                    RunAction("Data Compare", client, admsLog, sourceFile, ExecuteDataCompare);
                    return false;

                case Enums.FileTypes.Telemarketing_Long_Form:
                case Enums.FileTypes.Telemarketing_Short_Form:
                    return RunOperation("ApplyTelemarketingLogic", client, admsLog, sourceFile, fileType, ApplyTelemarketingLogic);

                case Enums.FileTypes.Web_Forms:
                case Enums.FileTypes.Web_Forms_Short_Form:
                    return RunOperation("ApplyWebFormLogic", client, admsLog, sourceFile, ApplyWebFormLogic);

                case Enums.FileTypes.List_Source_2YR:
                    return RunOperation("ApplyListSource2YRLogic", client, admsLog, sourceFile, ApplyListSource2YRLogic);

                case Enums.FileTypes.List_Source_3YR:
                    return RunOperation("ApplyListSource3YRLogic", client, admsLog, sourceFile, ApplyListSource3YRLogic);

                case Enums.FileTypes.List_Source_Other:
                    return RunOperation("ApplyListSourceOtherLogic", client, admsLog, sourceFile, ApplyListSourceOtherLogic);

                case Enums.FileTypes.Field_Update:
                {
                    var noErrors = RunOperation("ApplyFieldUpdateLogic", client, admsLog, sourceFile, ApplyFieldUpdateLogic);

                    try
                    {
                        EmailBadData(client, admsLog, sourceFile, emailFileName);
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, client, $"{GetType().Name}.ImportToUAD");
                    }

                    return noErrors;
                }

                case Enums.FileTypes.QuickFill:
                    return RunOperation("ApplyQuickFillLogic", client, admsLog, sourceFile, fileType, ApplyQuickFillLogic);

                case Enums.FileTypes.Paid_Transaction:
                    return RunOperation("Paid_Transaction", client, admsLog, sourceFile, fileType, ApplyPaidTransactionLogic);

                default:
                    return true;
            }
        }

        private void DedupeDataToUAD(Client client, AdmsLog admsLog)
        {
            ConsoleMessage($"{DateTime.Now.TimeOfDay} - Start data dedupe in Master UAD for client: {client.FtpFolder}", admsLog.ProcessCode);

            var dedupeResult = DedupeMasterDB(client, admsLog.ProcessCode);

            ConsoleMessage(
                dedupeResult
                    ? $"{DateTime.Now.TimeOfDay} - Data succesfully deduped in Master UAD for client: {client.FtpFolder}"
                    : $"{DateTime.Now.TimeOfDay} - An exception occurred while updating Master UAD for client: {client.FtpFolder}. Please check log.",
                admsLog.ProcessCode);
        }

        private void TransferDataToUAD(Client client, AdmsLog admsLog, Enums.FileTypes fileType, SourceFile sourceFile)
        {
            ConsoleMessage($"{DateTime.Now.TimeOfDay} - Start data transer to Master UAD for client: {client.FtpFolder}", admsLog.ProcessCode);

            var transferResult = UpdateMasterDB(client, admsLog.ProcessCode, sourceFile.SourceFileID, sourceFile.FileName, fileType);

            if (transferResult)
            {
                ConsoleMessage($"{DateTime.Now.TimeOfDay} - Data succesfully updated in Master UAD for client: {client.FtpFolder}", admsLog.ProcessCode);
                SetSubscriptionPaid(client, admsLog.ProcessCode);
            }
            else
            {
                ConsoleMessage($"{DateTime.Now.TimeOfDay} - An exception occurred while updating Master UAD for client: {client.FtpFolder}. Please check log.", admsLog.ProcessCode);
            }
        }

        private void CalculateAdmsLogFinalCounts(Client client, AdmsLog admsLog, SourceFile sourceFile, bool checkRecordSource)
        {
            ConsoleMessage($"{client.FtpFolder} - Update Finished SubscriberFinal counts {DateTime.Now.TimeOfDay}", admsLog.ProcessCode);
            var sourceFileWrk = new FrameworkUAD.BusinessLogic.SubscriberFinal();
            var counts = sourceFileWrk.SelectResultCountAfterProcessToLive(admsLog.ProcessCode, client.ClientConnections);

            var ignoredRecordCount = admsLog.FinalRecordCount - counts.FinalProfileCount;
            var ignoredProfileCount = admsLog.FinalProfileCount - counts.FinalProfileCount;
            var ignoredDemoCount = admsLog.FinalDemoCount - counts.FinalDemoCount;

            admsWrk.UpdateFinalCountsAfterProcessToLive(
                admsLog.ProcessCode, 
                counts.FinalProfileCount,
                counts.FinalProfileCount, 
                counts.FinalDemoCount, 
                ignoredRecordCount, 
                ignoredProfileCount,
                ignoredDemoCount, 
                counts.MatchedRecordCount, 
                counts.UadConsensusCount, 
                1, 
                true,
                admsLog.SourceFileId);

            admsLog.FinalRecordCount = counts.FinalProfileCount;
            admsLog.FinalProfileCount = counts.FinalProfileCount;
            admsLog.FinalDemoCount = counts.FinalDemoCount;
            admsLog.MatchedRecordCount = counts.MatchedRecordCount;
            admsLog.UadConsensusCount = counts.UadConsensusCount;

            var sourceRestrictCreateReports = new[]
            {
                "API",
                "ACS",
                "Field Update",
                "NCOA"
            };
            
            if (!checkRecordSource || !sourceRestrictCreateReports.Contains(admsLog.RecordSource))
            {
                CreateDashboardFileHistoryReports_Threaded(sourceFile.SourceFileID, admsLog.ProcessCode, client);
            }
        }

        private void ClientExecuteCustomCode(Client client, AdmsLog admsLog, SourceFile sourceFile, Enums.ExecutionPointType executionPointType)
        {
            var csc = new ClientMethods.ClientSpecialCommon();

            List<ClientCustomProcedure> customProceduresList;

            if (BillTurner.ClientAdditionalProperties != null &&
                BillTurner.ClientAdditionalProperties.ContainsKey(client.ClientID))
            {
                customProceduresList = BillTurner.ClientAdditionalProperties[client.ClientID].ClientCustomProceduresList;
            }
            else
            {
                var capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                var cap = capWork.SetObjects(client.ClientID, false);
                customProceduresList = cap.ClientCustomProceduresList;
            }

            csc.ExecuteClientCustomCode(executionPointType, client, customProceduresList, sourceFile, admsLog.ProcessCode);
        }

        private void EmailBadData(Client client, AdmsLog admsLog, SourceFile sourceFile, string emailFileName)
        {
            var fieldMappings = GetFieldMappings(sourceFile);

            var nonProcessedRecords = GetNonProcessedRecords(client, admsLog, fieldMappings);

            if (nonProcessedRecords.Length > 0)
            {
                var e = new Emailer.Emailer(Enums.MailMessageType.EmailFieldUpdateIssues);
                e.EmailFieldUpdateIssues(client, emailFileName, admsLog.ProcessCode, nonProcessedRecords);
            }
        }

        private string GetNonProcessedRecords(Client client, AdmsLog admsLog, IReadOnlyCollection<FieldMapping> fieldMappings)
        {
            var subFinalWorker = new FrameworkUAD.BusinessLogic.SubscriberFinal();
            var listSubFinal = subFinalWorker.SelectForFieldUpdate(admsLog.ProcessCode, client.ClientConnections);
            
            if (!listSubFinal.Any())
            {
                return string.Empty;
            }

            const string startChar = "\"";
            const string betweenChar = "\",\"";
            const string endChar = "\"\n";

            var nonProcessedRecords = startChar;
            var firstRun = true;
            var forPropList = new FrameworkUAD.Entity.SubscriberFinal();
            var subFinalProperties = forPropList.GetType().GetProperties();
            foreach (var lsourceFile in listSubFinal)
            {
                if (firstRun)
                {
                    firstRun = false;

                    var header = string.Join(betweenChar, fieldMappings.Select(m => m.MAFField));
                    nonProcessedRecords += header + endChar;
                }
                
                var lstMapping = true;
                foreach (var fieldMapping in fieldMappings)
                {
                    var propName = subFinalProperties.FirstOrDefault(x => x.Name.Equals(fieldMapping.MAFField, StringComparison.CurrentCultureIgnoreCase));

                    string valueSeparator;
                    if (lstMapping)
                    {
                        lstMapping = false;
                        valueSeparator = startChar;
                    }
                    else
                    {
                        valueSeparator = betweenChar;
                    }

                    if (propName != null)
                    {
                        var propValue = lsourceFile.GetType().GetProperty(propName.Name)?.GetValue(lsourceFile, null).ToString();
                        var value = propValue ?? string.Empty;

                        nonProcessedRecords += valueSeparator + value;
                    }
                }

                nonProcessedRecords += endChar;
            }

            return nonProcessedRecords;
        }

        private IReadOnlyCollection<FieldMapping> GetFieldMappings(SourceFile sourceFile)
        {
            var cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            var codes = cWorker.Select(Enums.CodeType.Field_Mapping);
            var standardTypeId = codes.Single(x => x.CodeName.Equals(Enums.FieldMappingTypes.Standard.ToString())).CodeId;
            var fieldMappings = sourceFile.FieldMappings.Where(x => x.FieldMappingTypeID == standardTypeId).ToList();
            if (!fieldMappings.Any())
            {
                var fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
                fieldMappings = fmWorker.Select(sourceFile.SourceFileID);
            }

            return fieldMappings;
        }

        private void RunAction(string description, Client client, AdmsLog admsLog, SourceFile sourceFile, Action<Client, SourceFile, string> operation)
        {
            ConsoleMessage($"{DateTime.Now.TimeOfDay} - Start {description} for client: {client.FtpFolder}", admsLog.ProcessCode);
            operation(client, sourceFile, admsLog.ProcessCode);
            ConsoleMessage($"{DateTime.Now.TimeOfDay} - Finished {description} for client: {client.FtpFolder}", admsLog.ProcessCode);
        }

        private bool RunOperation(string description, Client client, AdmsLog admsLog, SourceFile sourceFile, Enums.FileTypes fileType, Func<Client, SourceFile, string, Enums.FileTypes, bool> operation)
        {
            ConsoleMessage($"{DateTime.Now.TimeOfDay} - Start {description} for client: {client.FtpFolder}", admsLog.ProcessCode);
            var noErrors = operation(client, sourceFile, admsLog.ProcessCode, fileType);
            ConsoleMessage($"{DateTime.Now.TimeOfDay} - End {description} for client: {client.FtpFolder}", admsLog.ProcessCode);

            return noErrors;
        }

        private bool RunOperation(string description, Client client, AdmsLog admsLog, SourceFile sourceFile, Func<Client, SourceFile, string, bool> operation)
        {
            ConsoleMessage($"{DateTime.Now.TimeOfDay} - Start {description} for client: {client.FtpFolder}", admsLog.ProcessCode);
            var noErrors = operation(client, sourceFile, admsLog.ProcessCode);
            ConsoleMessage($"{DateTime.Now.TimeOfDay} - End {description} for client: {client.FtpFolder}", admsLog.ProcessCode);

            return noErrors;
        }

        private void SetClientAdditionalProperties(Client client)
        {
            if (BillTurner.ClientAdditionalProperties == null || !BillTurner.ClientAdditionalProperties.ContainsKey(client.ClientID))
            {
                var capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                var cap = capWork.SetObjects(client.ClientID, false);

                BillTurner.ClientAdditionalProperties = BillTurner.ClientAdditionalProperties ?? new Dictionary<int, ClientAdditionalProperties>();
                

                BillTurner.ClientAdditionalProperties.Add(client.ClientID, cap);
            }
        }

        public void HandleFileCleansed(FileCleansed eventMessage)
        {
            if (eventMessage == null)
            {
                throw new ArgumentNullException(nameof(eventMessage));
            }

            var client = eventMessage.Client;
            var sourceFile = eventMessage.SourceFile;
            var admsLog = eventMessage.AdmsLog;

            var fileType = Enums.FileTypes.Audience_Data;
            if (sourceFile.DatabaseFileTypeId > 0)
            {
                var cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                var code = cWorker.SelectCodeId(sourceFile.DatabaseFileTypeId);
                fileType = Enums.GetDatabaseFileType(code.CodeName);
            }

            SetClientAdditionalProperties(client);

            var threadId = Thread.CurrentThread.ManagedThreadId;
            try
            {
                var shouldProcessFurther = ProcessFileTypes(client, admsLog, fileType, sourceFile, eventMessage.ImportFile.Name);
                if (!shouldProcessFurther)
                {
                    return;
                }

                // Pre UAD Import
                ClientExecuteCustomCode(client, admsLog, sourceFile, Enums.ExecutionPointType.Pre_UAD_Import);

                TransferDataToUAD(client, admsLog, fileType, sourceFile);
                DedupeDataToUAD(client, admsLog);

                // Post UAD Import
                ClientExecuteCustomCode(client, admsLog, sourceFile, Enums.ExecutionPointType.Post_UAD_Import);

                CalculateAdmsLogFinalCounts(client, admsLog, sourceFile, false);
            }
            catch (Exception ex)
            {
                LogError(ex, client, $"{GetType().Name}.ImportToUAD");
            }
            finally
            {
                admsWrk.Update(eventMessage.AdmsLog.ProcessCode, Enums.FileStatusType.Completed, Enums.ADMS_StepType.Watching_for_File, FrameworkUAD_Lookup.Enums.ProcessingStatusType.Watching_for_File, FrameworkUAD_Lookup.Enums.ExecutionPointType.Post_UAD_Import, 1, "Waiting for File", true, eventMessage.AdmsLog.SourceFileId, true);
                ThreadDictionary.Remove(threadId);
                SetPropertiesNull();
            }
        }

        private SqlConnection GetSqlConnection(KMPlatform.Entity.Client client)
        {
            string connectionString = client.ClientTestDBConnectionString;
            bool isDemo = false;
            bool isNetworkDeployed = false;

            try
            {
                bool.TryParse(ConfigurationManager.AppSettings["IsDemo"].ToString(), out isDemo);
                bool.TryParse(ConfigurationManager.AppSettings["IsNetworkDeployed"].ToString(), out isNetworkDeployed);

                if (isDemo == false)
                    connectionString = client.ClientLiveDBConnectionString;

                if (isNetworkDeployed == true)
                    connectionString = connectionString.Replace("216.17", "10.10");
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".GetSqlConnection");
            }

            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
        public void SetSubscriptionPaid(KMPlatform.Entity.Client client, string processCode)
        {
            try
            {
                if (client.HasPaid == true && client.UseSubGen == true)
                {
                    FrameworkSubGen.BusinessLogic.Payment pWrk = new FrameworkSubGen.BusinessLogic.Payment();
                    pWrk.Save(processCode, client.ClientConnections);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".SetSubscriptionPaid");
            }
        }
        private bool UpdateMasterDB(KMPlatform.Entity.Client client, string processCode, int sourceFileID, string fileName, FrameworkUAD_Lookup.Enums.FileTypes fileType)
        {
            //run any custom import rules except deletes BEFORE import sproc
            RunPreImportRules();

            #region Import sproc
            bool done = true;
            try
            {
                bool MailPermissionOverRide = false;
                bool FaxPermissionOverRide = false;
                bool PhonePermissionOverRide = false;
                bool OtherProductsPermissionOverRide = false;
                bool ThirdPartyPermissionOverRide = false;
                bool EmailRenewPermissionOverRide = false;
                bool TextPermissionOverRide = false;


                FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
                List<FrameworkUAS.Entity.FieldMapping> list = fmWorker.Select(sourceFileID);

                if (list.Exists(x => x.MAFField.Equals("MailPermission", StringComparison.CurrentCultureIgnoreCase)))
                    MailPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("FaxPermission", StringComparison.CurrentCultureIgnoreCase)))
                    FaxPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("PhonePermission", StringComparison.CurrentCultureIgnoreCase)))
                    PhonePermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("OtherProductsPermission", StringComparison.CurrentCultureIgnoreCase)))
                    OtherProductsPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("ThirdPartyPermission", StringComparison.CurrentCultureIgnoreCase)))
                    ThirdPartyPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("EmailRenewPermission", StringComparison.CurrentCultureIgnoreCase)))
                    EmailRenewPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("TextPermission", StringComparison.CurrentCultureIgnoreCase)))
                    TextPermissionOverRide = true;

                FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                List<FrameworkUAD_Lookup.Entity.Code> codes = new List<FrameworkUAD_Lookup.Entity.Code>();
                codes = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Demographic_Update);
                FrameworkUAD_Lookup.Entity.Code replace = codes.FirstOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.DemographicUpdate.Replace.ToString(), StringComparison.CurrentCultureIgnoreCase));
                FrameworkUAD_Lookup.Entity.Code overwrite = codes.FirstOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.DemographicUpdate.Overwrite.ToString(), StringComparison.CurrentCultureIgnoreCase));
                int replaceID = 0;
                int overwriteID = 0;
                if (replace != null)
                    replaceID = replace.CodeId;
                if (overwrite != null)
                    overwriteID = overwrite.CodeId;

                string replaceMAFFields = string.Join(",", list.Where(x => x.DemographicUpdateCodeId == replaceID).Select(x => x.MAFField));
                string overwriteMAFFields = string.Join(",", list.Where(x => x.DemographicUpdateCodeId == overwriteID).Select(x => x.MAFField));
                //if this is an api SaveSubscriber then there is not a Mapped file so our replace/overwrite fields are always blank
                //check if SourceFileName = UAD_WS_AddSubscriber
                if (fileName == "UAD_WS_AddSubscriber")
                {
                    //get a distinct list of MAFField by ProcessCode where demo update type is Overwrite or Replace
                    //return a dataTable of MafField | UpdateAction
                    //updateAction = 'Replace' or 'Overwrite'
                    System.Text.StringBuilder sbReplace = new System.Text.StringBuilder();
                    System.Text.StringBuilder sbOver = new System.Text.StringBuilder();
                    FrameworkUAD.BusinessLogic.SubscriberDemographicTransformed dtWrk = new FrameworkUAD.BusinessLogic.SubscriberDemographicTransformed();
                    DataTable dt = dtWrk.MafFieldUpdateAction(processCode, client.ClientConnections);
                    foreach(DataRow dr in dt.Rows)
                    {
                        if (dr["UpdateAction"].ToString().Equals("Replace", StringComparison.CurrentCultureIgnoreCase))
                            sbReplace.Append(dr["MafField"].ToString() + ",");
                        else if (dr["UpdateAction"].ToString().Equals("Overwrite", StringComparison.CurrentCultureIgnoreCase))
                            sbOver.Append(dr["MafField"].ToString() + ",");
                    }
                    replaceMAFFields = sbReplace.ToString().TrimEnd(',');
                    overwriteMAFFields = sbOver.ToString().TrimEnd(',');
                }

                SqlConnection sqlConn = GetSqlConnection(client);
                SqlCommand cmd = new SqlCommand("e_ImportFromUAS", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProcessCode", processCode);
                cmd.Parameters.AddWithValue("@MailPermissionOverRide", MailPermissionOverRide);
                cmd.Parameters.AddWithValue("@FaxPermissionOverRide", FaxPermissionOverRide);
                cmd.Parameters.AddWithValue("@PhonePermissionOverRide", PhonePermissionOverRide);
                cmd.Parameters.AddWithValue("@OtherProductsPermissionOverRide", OtherProductsPermissionOverRide);
                cmd.Parameters.AddWithValue("@ThirdPartyPermissionOverRide", ThirdPartyPermissionOverRide);
                cmd.Parameters.AddWithValue("@EmailRenewPermissionOverRide", EmailRenewPermissionOverRide);
                cmd.Parameters.AddWithValue("@TextPermissionOverRide", TextPermissionOverRide);
                cmd.Parameters.AddWithValue("@FileType", fileType.ToString());
                cmd.Parameters.AddWithValue("@OverwriteValues", overwriteMAFFields);
                cmd.Parameters.AddWithValue("@ReplaceValues", replaceMAFFields);
                cmd.CommandTimeout = 0;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

                ConsoleMessage(client.FtpFolder + " - finished executiing sproc e_ImportFromUAS " + DateTime.Now.TimeOfDay.ToString(), processCode, true);
            }
            catch (Exception ex)
            {
                done = false;
                LogError(ex, client, this.GetType().Name.ToString() + ".UpdateMasterDB");
            }

            #endregion
            //run any DELETE custom import rules
            RunPostImportRules();

            ImportPaidData(processCode, client);
            return done;
        }
        private void RunPreImportRules()
        {
            //apply rules to SubscriberFinal table
        }
        private void RunPostImportRules()
        {
            //only deletes are run post import

        }
        private void ImportPaidData(string processCode, KMPlatform.Entity.Client client)
        {
            try
            {
                if (client.HasPaid == true && client.UseSubGen == true)
                {
                    ConsoleMessage(client.FtpFolder + " - begin Import Paid Data " + DateTime.Now.TimeOfDay.ToString(), processCode, true);
                    FrameworkUAD.BusinessLogic.SubscriptionPaid sp = new FrameworkUAD.BusinessLogic.SubscriptionPaid();
                    sp.ImportFromSubGen(processCode, client.ClientConnections);
                    ConsoleMessage(client.FtpFolder + " - end Import Paid Data " + DateTime.Now.TimeOfDay.ToString(), processCode, true);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".ImportPaidData");
            }
        }
        private void ApplyComplimenaryLogic(KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, string processCode)
        {
            try
            {
                FrameworkUAD.BusinessLogic.IssueComp ic = new FrameworkUAD.BusinessLogic.IssueComp();
                int pubID = 0;
                int.TryParse(sourceFile.PublicationID.ToString(), out pubID);
                ic.JobSaveComplimentary(client.ClientConnections, processCode, pubID, sourceFile.SourceFileID);
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".ApplyComplimenaryLogic");
            }
        }
        private bool DedupeMasterDB(KMPlatform.Entity.Client client, string processCode)
        {
            bool done = true;
            try
            {
                SqlConnection sqlConn = GetSqlConnection(client);
                SqlCommand cmd = new SqlCommand("e_DupeCleanUp", sqlConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                done = false;
                LogError(ex, client, this.GetType().Name.ToString() + ".DedupeMasterDB");
            }

            return done;
        }
        private bool ApplyTelemarketingLogic(KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, string processCode, FrameworkUAD_Lookup.Enums.FileTypes fileType)
        {
            //so at this point will have records in SubscriberFinal - DQM all done

            //could likely just call a sproc and pass ProcessCode
            //apply rules to TransourceFileormed
            //create Circ batch
            //update Circ
            //this is only to UPDATE existing data
            //AFTER UAD processing will then call Circ Sync which will send the new records to Circ DB
            //OR should the Telemarketing sproc after doing updates then insert new records to Circ??? probably can't at this point because havent gone through DQM
            bool noErrors = true;
            try
            {
                FrameworkUAD.BusinessLogic.CircIntegration ciWorker = new FrameworkUAD.BusinessLogic.CircIntegration();
                ciWorker.ApplyTelemarketingRules(processCode, fileType, client.ClientConnections, client.ClientID);
            }
            catch (Exception ex)
            {
                noErrors = false;
                LogError(ex, client, this.GetType().Name.ToString() + ".ApplyTelemarketingLogic");
            }

            //Each Publication has a list of Audited fields (Demographics)  Need to store these somewhere. ProductAudit table

            //2 code paths - Long Form or Short Form - Short Form can have audit questions
            //much like NCOA - match on Seq# then update data
            #region Shared rules
            //- Updates existing active qualified record with new information from a pre-approved telemarketing script.
            //- Cat 10  Xact 27
            //- use qdate on incoming record 
            //- replace all par3c with '1' for ((function#' ' and function#'zz') or Title#' ') and (Fname#' ' or Lname#' ') 
            //  replace all par3c with '2' for ((function=' ' and function='zz') and Title=' ') and (Fname#' ' or Lname#' ') 
            //  replace all par3c with '3' for ((function#' ' and function#'zz') or Title=' ') and (Fname=' ' or Lname=' ') 
            //  replace all par3c with '4' for ((function=' ' and function='zz') or Title=' ') and (Fname=' ' or Lname=' ' and company#' ') 
            //  replace all par3c with '5' for copies > '1'
            //- keep existing SequenceNumber
            //- Assign a new batch. No limit to batch size.
            //- Update any record within the existing file as long as the incoming qdate is greater than the current date on the record or the qsource is a (H,I,J,K,L,M,N).
            //- if seqNumber is blank send through DQM Matching process to determin if subscriber already exist for this product or to create a new record
            #endregion
            #region Long Form rules
            //- Overwrite existing demos from incoming  telemarketing file. 
            //- If the incoming list does not have a demo7(media) field replace demo7 with "A" for print. If demo7 is on incoming file replace existing demo7 with demo7 incoming data.
            #endregion

            #region Short Form rules
            //- Keep Demos from existing record when not provided on short form.
            //- If the incoming list does not have a demo7(media) field  or the field is there but is blank replace demo7 with "A" for print.
            #endregion
            return noErrors;
        }
        private bool ApplyWebFormLogic(KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, string processCode)
        {
            bool noErrors = true;
            try
            {
                FrameworkUAD.BusinessLogic.CircIntegration ciWorker = new FrameworkUAD.BusinessLogic.CircIntegration();
                ciWorker.ApplyWebFormRules(processCode, client.ClientConnections, client.ClientID);
            }
            catch (Exception ex)
            {
                noErrors = false;
                LogError(ex, client, this.GetType().Name.ToString() + ".ApplyWebFormLogic");
            }
            return noErrors;
        }
        #region ListSource
        private bool ApplyListSource2YRLogic(KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, string processCode)
        {
            bool noErrors = true;
            try
            {
                FrameworkUAD.BusinessLogic.CircIntegration ciWorker = new FrameworkUAD.BusinessLogic.CircIntegration();
                ciWorker.ApplyListSource2YRRules(processCode, client.ClientConnections, client.ClientID);
            }
            catch (Exception ex)
            {
                noErrors = false;
                LogError(ex, client, this.GetType().Name.ToString() + ".ApplyListSource2YRLogic");
            }
            return noErrors;
        }
        private bool ApplyListSource3YRLogic(KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, string processCode)
        {
            bool noErrors = true;
            try
            {
                FrameworkUAD.BusinessLogic.CircIntegration ciWorker = new FrameworkUAD.BusinessLogic.CircIntegration();
                ciWorker.ApplyListSource3YRRules(processCode, client.ClientConnections, client.ClientID);
            }
            catch (Exception ex)
            {
                noErrors = false;
                LogError(ex, client, this.GetType().Name.ToString() + ".ApplyListSource3YRLogic");
            }
            return noErrors;
        }
        private bool ApplyListSourceOtherLogic(KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, string processCode)
        {
            bool noErrors = true;
            try
            {
                FrameworkUAD.BusinessLogic.CircIntegration ciWorker = new FrameworkUAD.BusinessLogic.CircIntegration();
                ciWorker.ApplyListSourceOtherRules(processCode, client.ClientConnections, client.ClientID);
            }
            catch (Exception ex)
            {
                noErrors = false;
                LogError(ex, client, this.GetType().Name.ToString() + ".ApplyListSourceOtherLogic");
            }
            return noErrors;
        }
        #endregion

        private bool ApplyFieldUpdateLogic(KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, string processCode)
        {
            bool noErrors = true;
            try
            {
                bool qdateOverRide = false;
                bool mailPermissionOverRide = false;
                bool faxPermissionOverRide = false;
                bool phonePermissionOverRide = false;
                bool otherProductsPermissionOverRide = false;
                bool thirdPartyPermissionOverRide = false;
                bool emailRenewPermissionOverRide = false;
                bool textPermissionOverRide = false;

                FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
                List<FrameworkUAS.Entity.FieldMapping> list = fmWorker.Select(sourceFile.SourceFileID);

                if (list.Exists(x => x.MAFField.Equals("QUALIFICATIONDATE", StringComparison.CurrentCultureIgnoreCase)))
                    qdateOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("MailPermission", StringComparison.CurrentCultureIgnoreCase)))
                    mailPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("FaxPermission", StringComparison.CurrentCultureIgnoreCase)))
                    faxPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("PhonePermission", StringComparison.CurrentCultureIgnoreCase)))
                    phonePermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("OtherProductsPermission", StringComparison.CurrentCultureIgnoreCase)))
                    otherProductsPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("ThirdPartyPermission", StringComparison.CurrentCultureIgnoreCase)))
                    thirdPartyPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("EmailRenewPermission", StringComparison.CurrentCultureIgnoreCase)))
                    emailRenewPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("TextPermission", StringComparison.CurrentCultureIgnoreCase)))
                    textPermissionOverRide = true;

                FrameworkUAD.BusinessLogic.CircIntegration ciWorker = new FrameworkUAD.BusinessLogic.CircIntegration();
                ciWorker.ApplyFieldUpdateRules(processCode, client.ClientConnections, client.ClientID, qdateOverRide, mailPermissionOverRide, faxPermissionOverRide, phonePermissionOverRide, otherProductsPermissionOverRide, thirdPartyPermissionOverRide, emailRenewPermissionOverRide, textPermissionOverRide);
            }
            catch (Exception ex)
            {
                noErrors = false;
                LogError(ex, client, this.GetType().Name.ToString() + ".ApplyFieldUpdateLogic");
            }
            return noErrors;
        }
        private bool ApplyQuickFillLogic(KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, string processCode, FrameworkUAD_Lookup.Enums.FileTypes fileType)
        {
            bool noErrors = true;
            try
            {
                bool qdateOverRide = false;
                bool mailPermissionOverRide = false;
                bool faxPermissionOverRide = false;
                bool phonePermissionOverRide = false;
                bool otherProductsPermissionOverRide = false;
                bool thirdPartyPermissionOverRide = false;
                bool emailRenewPermissionOverRide = false;
                bool textPermissionOverRide = false;

                FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
                List<FrameworkUAS.Entity.FieldMapping> list = fmWorker.Select(sourceFile.SourceFileID);

                if (list.Exists(x => x.MAFField.Equals("QUALIFICATIONDATE", StringComparison.CurrentCultureIgnoreCase)))
                    qdateOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("MailPermission", StringComparison.CurrentCultureIgnoreCase)))
                    mailPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("FaxPermission", StringComparison.CurrentCultureIgnoreCase)))
                    faxPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("PhonePermission", StringComparison.CurrentCultureIgnoreCase)))
                    phonePermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("OtherProductsPermission", StringComparison.CurrentCultureIgnoreCase)))
                    otherProductsPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("ThirdPartyPermission", StringComparison.CurrentCultureIgnoreCase)))
                    thirdPartyPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("EmailRenewPermission", StringComparison.CurrentCultureIgnoreCase)))
                    emailRenewPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("TextPermission", StringComparison.CurrentCultureIgnoreCase)))
                    textPermissionOverRide = true;

                FrameworkUAD.BusinessLogic.CircIntegration ciWorker = new FrameworkUAD.BusinessLogic.CircIntegration();
                ciWorker.ApplyQuickFillRules(processCode, client.ClientConnections, client.ClientID, qdateOverRide, mailPermissionOverRide, faxPermissionOverRide, phonePermissionOverRide, otherProductsPermissionOverRide, thirdPartyPermissionOverRide, emailRenewPermissionOverRide, textPermissionOverRide);
            }
            catch (Exception ex)
            {
                noErrors = false;
                LogError(ex, client, this.GetType().Name.ToString() + ".ApplyQuickFillLogic");
            }
            return noErrors;
        }
        private bool ApplyPaidTransactionLogic(KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, string processCode, FrameworkUAD_Lookup.Enums.FileTypes fileType)
        {
            bool noErrors = true;
            try
            {
                bool qdateOverRide = false;
                bool mailPermissionOverRide = false;
                bool faxPermissionOverRide = false;
                bool phonePermissionOverRide = false;
                bool otherProductsPermissionOverRide = false;
                bool thirdPartyPermissionOverRide = false;
                bool emailRenewPermissionOverRide = false;
                bool textPermissionOverRide = false;

                FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
                List<FrameworkUAS.Entity.FieldMapping> list = fmWorker.Select(sourceFile.SourceFileID);

                if (list.Exists(x => x.MAFField.Equals("QUALIFICATIONDATE", StringComparison.CurrentCultureIgnoreCase)))
                    qdateOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("MailPermission", StringComparison.CurrentCultureIgnoreCase)))
                    mailPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("FaxPermission", StringComparison.CurrentCultureIgnoreCase)))
                    faxPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("PhonePermission", StringComparison.CurrentCultureIgnoreCase)))
                    phonePermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("OtherProductsPermission", StringComparison.CurrentCultureIgnoreCase)))
                    otherProductsPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("ThirdPartyPermission", StringComparison.CurrentCultureIgnoreCase)))
                    thirdPartyPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("EmailRenewPermission", StringComparison.CurrentCultureIgnoreCase)))
                    emailRenewPermissionOverRide = true;
                if (list.Exists(x => x.MAFField.Equals("TextPermission", StringComparison.CurrentCultureIgnoreCase)))
                    textPermissionOverRide = true;

                FrameworkUAD.BusinessLogic.CircIntegration ciWorker = new FrameworkUAD.BusinessLogic.CircIntegration();
                ciWorker.ApplyPaidTransactionLogic(processCode, client.ClientConnections, client.ClientID, qdateOverRide, mailPermissionOverRide, faxPermissionOverRide, phonePermissionOverRide, otherProductsPermissionOverRide, thirdPartyPermissionOverRide, emailRenewPermissionOverRide, textPermissionOverRide);
            }
            catch (Exception ex)
            {
                noErrors = false;
                LogError(ex, client, this.GetType().Name.ToString() + ".ApplyPaidTransactionLogic");
            }
            return noErrors;
        }
        private void ApplyCircOtherFileLogic(KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, string processCode)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".ApplyCircOtherFileLogic");
            }
        }
        private void ExecuteDataCompare(KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, string processCode)
        {
            try
            {
                //insert to -  DataCompareRun
                //copy profile data from SubscriberFinal to DataCompareProfile - can use SubscriberFinal.IsNewRecord flag set in job_DataMatching
                FrameworkUAD.BusinessLogic.DataCompareProfile dcpWrk = new FrameworkUAD.BusinessLogic.DataCompareProfile();
                dcpWrk.InsertFromSubscriberFinal(client.ClientConnections, processCode);

                FrameworkUAS.BusinessLogic.AdmsLog aWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
                FrameworkUAS.Entity.AdmsLog aLog = aWrk.Select(processCode);

                FrameworkUAS.BusinessLogic.DataCompareRun dcrWrk = new FrameworkUAS.BusinessLogic.DataCompareRun();
                FrameworkUAS.Entity.DataCompareRun run = new FrameworkUAS.Entity.DataCompareRun();
                run.ClientId = client.ClientID;
                run.FileRecordCount = aLog.OriginalRecordCount;//can get OriginalRecordCount from AdmsLog table in UAS.
                run.MatchedRecordCount = aLog.MatchedRecordCount;//must call DataCompareProfile table ClientUAD - lets add this as a count to AdmsLog table
                run.ProcessCode = processCode;
                run.SourceFileId = sourceFile.SourceFileID;
                run.UadConsensusCount = aLog.UadConsensusCount;//count of Subsciptions table ClientUAD - lets add this as a count to AdmsLog table
                run.IsBillable = sourceFile.IsBillable;

                run.DcRunId = dcrWrk.Save(run);

                //On Initial Import when a Billable record is added to Data Compare Run we need to add a record to DataCompareView, 
                //these then will be billed at the Consensus Rate if not used within 2 weeks.
                //Billable Import: PaymentStatusID(Pending), IsBillable = 1
                //Nonbillable Import: PaymentStatusID(Nonbillable), IsBillable = 0
                FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
                FrameworkUAD_Lookup.Entity.Code c = cWrk.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Data_Compare_Target, FrameworkUAD_Lookup.Enums.DataCompareTargetTypes.Consensus.ToString());
                FrameworkUAD_Lookup.Entity.Code dcTargetCodeId = cWrk.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Data_Compare_Type, FrameworkUAD_Lookup.Enums.DataCompareType.Match.ToString());

                FrameworkUAS.BusinessLogic.DataCompareView dcvWrk = new FrameworkUAS.BusinessLogic.DataCompareView();
                FrameworkUAS.Entity.DataCompareView dcv = new FrameworkUAS.Entity.DataCompareView();
                dcv.DcTypeCodeId = dcTargetCodeId.CodeId;//this will be MATCH
                dcv.DcRunId = run.DcRunId;
                dcv.DcTargetCodeId = c.CodeId;//this is CONSENSUS
                dcv.DcTargetIdUad = 0;// used when created via UI - will have an id that relates back to BRAND or PRODUCT
                dcv.UadNetCount = run.UadConsensusCount;
                dcv.MatchedCount = run.MatchedRecordCount;
                dcv.NoDataCount = run.FileRecordCount - run.MatchedRecordCount;
                dcv.IsBillable = run.IsBillable;
                if (dcv.IsBillable)
                {
                    FrameworkUAD_Lookup.Entity.Code cPayStatus = cWrk.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Pending.ToString());
                    dcv.PaymentStatusId = cPayStatus.CodeId;
                }
                else
                {
                    FrameworkUAD_Lookup.Entity.Code cPayStatus = cWrk.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Payment_Status, FrameworkUAD_Lookup.Enums.PaymentStatus.Non_Billed.ToString());
                    dcv.PaymentStatusId = cPayStatus.CodeId;
                }
                dcv.CreatedByUserID = sourceFile.CreatedByUserID;
                //get the cost object - use pricing in this order - User, Client, Base
                //here I have to get the MergePurge cost
                //mp cost = # of records in UAD consensus + # of records in file
                int mpCount = run.UadConsensusCount + run.FileRecordCount;
                dcv.Cost = dcvWrk.GetDataCompareCost(0, client.ClientID, mpCount, FrameworkUAD_Lookup.Enums.DataCompareType.Match, FrameworkUAD_Lookup.Enums.DataCompareCost.MergePurge);

                //orginal
                //int likeCount = dcv.UadNetCount - dcv.MatchedCount;
                //FrameworkUAS.Object.DataCompareViewCost dcvCost = dcvWrk.GetDataCompareViewCost(dcv.MatchedCount, likeCount, client.ClientID, sourceFile.CreatedByUserID, dcv.DcRunId);
                //dcv.Cost = dcvCost.MatchCostTotal;

                dcv.DcViewId = dcvWrk.Save(dcv);
             }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".ExecuteDataCompare");
            }
    #region old code - now just dump from SubFinal to DataCompareProfile
    ////1. execute a datacompare sproc which will save results in some tables
    ////2. pull all the different result sets
    ////3. create data files / charts for each result set
    ////4. create and send email with result files
    ////5. set flag somewhere that data compare is done so UI can "see" this and have result files available via UI for download and viewing in grid/chart

    ////get a list of all DataCompareResultQue for SourceFileId wehre DateQued is null
    //FrameworkUAS.BusinessLogic.DataCompareResultQue rqWorker = new FrameworkUAS.BusinessLogic.DataCompareResultQue();
    //List<FrameworkUAS.Entity.DataCompareQue> listRQ = rqWorker.SelectNotQuedForSourceFile(sourceFile.SourceFileID);
    //foreach (FrameworkUAS.Entity.DataCompareQue rq in listRQ)
    //{
    //    //first DateQued
    //    rqWorker.SetQued(rq.DataCompareQueId);
    //}

    ////Core_AMS.Utilities.FileFunctions fw = new Core_AMS.Utilities.FileFunctions();
    //Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
    //string dcFTP = Core.ADMS.BaseDirs.getFtpDir() + "\\" + client.FtpFolder + "\\ADMS\\DataCompare\\";

    //FrameworkUAS.BusinessLogic.DataCompareDownload rWorker = new FrameworkUAS.BusinessLogic.DataCompareDownload();
    ////now lets do some processing
    //foreach (FrameworkUAS.Entity.DataCompareQue rq in listRQ)
    //{
    //    try
    //    {
    //        int productId = 0;
    //        int brandId = 0;
    //        int marketId = 0;
    //        if (rq.BrandId.HasValue)
    //            brandId = rq.BrandId.Value;
    //        if (rq.MarketId.HasValue)
    //            marketId = rq.MarketId.Value;
    //        if (rq.ProductId.HasValue)
    //            productId = rq.ProductId.Value;

    //        #region Setup
    //        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - DataCompare steps 1 and 2");
    //        //steps 1 and 2
    //        FrameworkUAS.Entity.DataCompareDownload dcr = rWorker.CreateResult(rq.DataCompareQueId, processCode, client.FtpFolder);
    //        #endregion

    //        #region Get DataTables
    //        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Get Matching Profile data");
    //        string mpFileName = dcFTP + "MatchingProfile_" + rq.FileName + ".csv";
    //        System.Data.DataTable dtMP = rWorker.SelectMatchingProfileData(client.ClientConnections, rq.DataCompareQueId, dcr.DataCompareResultId, rq.MatchClause, rqWorker.SelectTarget(rq), mpFileName, client.FtpFolder + "_MatchingProfile_" + rq.FileName, processCode, productId, brandId, marketId, client.ClientID, client.ParentClientId);

    //        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Get Matching Demographic data");
    //        string mdFileName = dcFTP + "MatchingDemo_" + rq.FileName + ".csv";
    //        System.Data.DataTable dtMD = rWorker.SelectMatchingDemoData(client.ClientConnections, rq.DataCompareQueId, dcr.DataCompareResultId, rq.MatchClause, rqWorker.SelectTarget(rq), mdFileName, client.FtpFolder + "_MatchingDemo_" + rq.FileName, processCode, productId, brandId, marketId, client.ClientID, client.ParentClientId);

    //        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Get Like Profile data");
    //        string lpFileName = dcFTP + "LikeProfile_" + rq.FileName + ".csv";
    //        System.Data.DataTable dtLP = rWorker.SelectLikeProfileData(client.ClientConnections, rq.DataCompareQueId, dcr.DataCompareResultId, rq.LikeClause, rqWorker.SelectTarget(rq), lpFileName, client.FtpFolder + "_LikeProfile_" + rq.FileName, processCode, productId, brandId, marketId, client.ClientID, client.ParentClientId);

    //        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Get Like Demographic data");
    //        string ldFileName = dcFTP + "LikeDemo_" + rq.FileName + ".csv";
    //        System.Data.DataTable dtLD = rWorker.SelectLikeDemoData(client.ClientConnections, rq.DataCompareQueId, dcr.DataCompareResultId, rq.LikeClause, rqWorker.SelectTarget(rq), ldFileName, client.FtpFolder + "_LikeDemo_" + rq.FileName, processCode, productId, brandId, marketId, client.ClientID, client.ParentClientId);

    //        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Get No Data");
    //        string noDataFileName = dcFTP + "NoData_" + rq.FileName + ".csv";
    //        System.Data.DataTable dtNoData = rWorker.SelectNoDataData(client.ClientConnections, rq.DataCompareQueId, dcr.DataCompareResultId, noDataFileName, client.FtpFolder + "_NoData_" + rq.FileName, processCode);

    //        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Get Summary Report");
    //        string sourceFileileName = dcFTP + "SummaryReport_" + rq.FileName + ".csv";
    //        System.Data.DataTable dtS = rWorker.CreateSummaryReportFile(dcr.DataCompareResultId, sourceFileileName, processCode);
    //        #endregion
    //        #region Send estimated completion time email
    //        Emailer.Emailer emEstCompletion = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.DataCompareEstimatedCompletionTime);
    //        //compute completion times by 84,000 records written to file per minute
    //        int matchProfile = 0;
    //        int matchDemo = 0;
    //        int likeProfile = 0;
    //        int likeDemo = 0;
    //        int noData = 0;
    //        int summary = 0;
    //        if (dtMP != null)
    //            matchProfile = dtMP.Rows.Count;
    //        if (dtMD != null)
    //            matchDemo = dtMD.Rows.Count;
    //        if (dtLP != null)
    //            likeProfile = dtLP.Rows.Count;
    //        if (dtLD != null)
    //            likeDemo = dtLD.Rows.Count;
    //        if (dtNoData != null)
    //            noData = dtNoData.Rows.Count;
    //        if (dtS != null)
    //            summary = dtS.Rows.Count;

    //        emEstCompletion.DataCompareEstimatedCompletionTime(client, rq, matchProfile, matchDemo, likeProfile, likeDemo, noData, summary);
    //        #endregion
    //        #region Write DataTables to files
    //        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Write Matching Profile data to file");
    //        if (dtMP != null)
    //        {
    //            if (dtMP.Columns.Contains("SubscriptionID1"))
    //                dtMP.Columns.Remove("SubscriptionID1");

    //            List<DataRow> del = new List<DataRow>();
    //            foreach (DataRow dr in dtMP.Rows)
    //            {
    //                if (dr["SubscriptionID"] != null && dr["SubscriptionID"].ToString() == "0")
    //                    del.Add(dr);
    //            }
    //            foreach (DataRow dr in del)
    //                dtMP.Rows.Remove(dr);

    //            //fw.CreateCSVFromDataTable(dtMP, mpFileName);
    //            Workbook wb = ef.GetWorkbook(dtMP, "Matching Profile");
    //            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
    //            using (FileStream output = new FileStream(mpFileName, FileMode.Create))
    //            {
    //                formatProvider.Export(wb, output);
    //            }
    //        }

    //        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Write Matching Demographic data to file");
    //        if (dtMD != null)
    //        {
    //            List<DataRow> del = new List<DataRow>();
    //            foreach (DataRow dr in dtMD.Rows)
    //            {
    //                if (dr["SubscriptionID"] != null && dr["SubscriptionID"].ToString() == "0")
    //                    del.Add(dr);
    //            }
    //            foreach (DataRow dr in del)
    //                dtMD.Rows.Remove(dr);

    //            System.Data.DataTable dtMatchAll = dtMP.Copy();
    //            //now add the demo columns except SubscriptionID
    //            //Demo tables = SubscriptionID, Demographic, Value
    //            //need a distinct list of Demographic
    //            var demoColumns = new List<string>();
    //            foreach (DataRow dr in dtMD.Rows)
    //            {
    //                if (!demoColumns.Contains(dr["Demographic"].ToString()))
    //                    demoColumns.Add(dr["Demographic"].ToString());
    //            }
    //            foreach (var c in demoColumns)
    //                if (!dtMatchAll.Columns.Contains(c))
    //                    dtMatchAll.Columns.Add(c);

    //            foreach (DataRow dr in dtMD.Rows)
    //            {
    //                DataRow drMP = dtMatchAll.Select("SubscriptionID = " + dr["SubscriptionID"].ToString()).FirstOrDefault();
    //                if (drMP != null)
    //                    drMP[dr["Demographic"].ToString()] = dr["Value"];
    //            }
    //            dtMatchAll.AcceptChanges();
    //            //fw.CreateCSVFromDataTable(dtMatchAll, mdFileName);
    //            Workbook wb = ef.GetWorkbook(dtMatchAll, "Matching Demos");
    //            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
    //            using (FileStream output = new FileStream(mpFileName, FileMode.Create))
    //            {
    //                formatProvider.Export(wb, output);
    //            }
    //            dtMD.Dispose();
    //            dtMP.Dispose();
    //            dtMatchAll.Dispose();
    //        }

    //        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Write Like Profile data to file");
    //        if (dtLP != null)
    //        {
    //            if (dtLP.Columns.Contains("SubscriptionID1"))
    //                dtLP.Columns.Remove("SubscriptionID1");

    //            List<DataRow> del = new List<DataRow>();
    //            foreach (DataRow dr in dtLP.Rows)
    //            {
    //                if (dr["SubscriptionID"] != null && dr["SubscriptionID"].ToString() == "0")
    //                    del.Add(dr);
    //            }
    //            foreach (DataRow dr in del)
    //                dtLP.Rows.Remove(dr);

    //            //fw.CreateCSVFromDataTable(dtLP, lpFileName);
    //            Workbook wb = ef.GetWorkbook(dtLP, "Like Profiles");
    //            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
    //            using (FileStream output = new FileStream(lpFileName, FileMode.Create))
    //            {
    //                formatProvider.Export(wb, output);
    //            }
    //        }

    //        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Write Like Demographic data to file");
    //        if (dtLD != null)
    //        {
    //            List<DataRow> del = new List<DataRow>();
    //            foreach (DataRow dr in dtLD.Rows)
    //            {
    //                if (dr["SubscriptionID"] != null && dr["SubscriptionID"].ToString() == "0")
    //                    del.Add(dr);
    //            }
    //            foreach (DataRow dr in del)
    //                dtLD.Rows.Remove(dr);


    //            System.Data.DataTable dtLikeAll = dtLP.Copy();
    //            //now add the demo columns except SubscriptionID
    //            var demoColumns = new List<string>();
    //            foreach (DataRow dr in dtLD.Rows)
    //            {
    //                if (!demoColumns.Contains(dr["Demographic"].ToString()))
    //                    demoColumns.Add(dr["Demographic"].ToString());
    //            }
    //            foreach (var c in demoColumns)
    //                dtLikeAll.Columns.Add(c);

    //            foreach (DataRow dr in dtLD.Rows)
    //            {
    //                DataRow drLP = dtLikeAll.Select("SubscriptionID = " + dr["SubscriptionID"].ToString()).FirstOrDefault();
    //                if (drLP != null)
    //                    drLP[dr["Demographic"].ToString()] = dr["Value"];
    //            }
    //            dtLikeAll.AcceptChanges();
    //            //fw.CreateCSVFromDataTable(dtLikeAll, ldFileName);
    //            Workbook wb = ef.GetWorkbook(dtLikeAll, "Like Demos");
    //            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
    //            using (FileStream output = new FileStream(ldFileName, FileMode.Create))
    //            {
    //                formatProvider.Export(wb, output);
    //            }
    //            dtLD.Dispose();
    //            dtLP.Dispose();
    //            dtLikeAll.Dispose();
    //        }

    //        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Write No Data to file");
    //        if (dtNoData != null)
    //        {
    //            //fw.CreateCSVFromDataTable(dtNoData, noDataFileName);
    //            Workbook wb = ef.GetWorkbook(dtNoData, "No Data");
    //            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
    //            using (FileStream output = new FileStream(ldFileName, FileMode.Create))
    //            {
    //                formatProvider.Export(wb, output);
    //            }
    //            dtNoData.Dispose();
    //        }

    //        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Write Summary Report to file");
    //        if (dtS != null)
    //        {
    //            //fw.CreateCSVFromDataTable(dtS, sourceFileileName);Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
    //            Workbook wb = ef.GetWorkbook(dtS, "Summary Report");
    //            IWorkbookFormatProvider formatProvider = new XlsxFormatProvider();
    //            using (FileStream output = new FileStream(sourceFileileName, FileMode.Create))
    //            {
    //                formatProvider.Export(wb, output);
    //            }
    //            dtS.Dispose();
    //        }
    //        #endregion
    //        #region OTHER - currently not defined
    //        //ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - DataCompare step 7", processCode, true);
    //        ////--step 7 - call a sproc which creates OtherProfileFile on server and updates OtherProfileFile with filename - going to save to a KM ftp folder DataCompare
    //        ////  -- update OtherProfileRecordCount
    //        ////  -- update OtherProfileCost
    //        //string opFileName = dcFTP + "OtherProfile_" + rq.FileName + ".csv";
    //        //System.Data.DataTable dtOP = rWorker.CreateOtherProfileFile(client, dcr.DataCompareResultId, opFileName, processCode);
    //        //fw.CreateCSVFromDataTable(dtOP, opFileName);

    //        //ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - DataCompare step 8", processCode, true);
    //        ////--step 8 - call a sproc which creates OtherDemoFile on server and updates OtherDemoFile with filename - going to save to a KM ftp folder DataCompare
    //        ////  -- update OtherDemoRecordCount
    //        ////  -- update OtherDemoCost
    //        //string odFileName = dcFTP + "OtherDemo_" + rq.FileName + ".csv";
    //        //System.Data.DataTable dtOD = rWorker.CreateOtherDemoFile(client, dcr.DataCompareResultId, odFileName, processCode);
    //        //fw.CreateCSVFromDataTable(dtOD, odFileName);
    //        #endregion

    //        ConsoleMessage(DateTime.Now.TimeOfDay.ToString() + " - Send Data Compare completion email");
    //        //send email that DC is done
    //        Emailer.Emailer em = new Emailer.Emailer(FrameworkUAD_Lookup.Enums.MailMessageType.DataCompareComplete);
    //        em.DataCompareComplete(client, rq, sourceFileileName);
    //    }
    //    catch (Exception ex)
    //    {
    //        LogError(ex, client, this.GetType().Name.ToString() + ".ExecuteDataCompare");
    //    }
    //}
    #endregion
}


        public void CreateDashboardFileHistoryReports_Threaded(int sourceFileID, string processCode, KMPlatform.Entity.Client client)
        {
            BackgroundWorker bw = new BackgroundWorker();
            // this allows our worker to report progress during work
            bw.WorkerReportsProgress = true;

            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                BackgroundWorker b = o as BackgroundWorker;
                CreateDashboardFileHistoryReports(sourceFileID, processCode, client);
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
               
            });

            bw.RunWorkerAsync();
        }
        public void CreateDashboardFileHistoryReports(int sourceFileID, string processCode, KMPlatform.Entity.Client client)
        {
            try
            {
                //Grab a fresh FieldMapping. One currently in eventMessage is missing a few columns (ignore and kmtransformed)
                FrameworkUAS.BusinessLogic.FieldMapping fmWorker = new FrameworkUAS.BusinessLogic.FieldMapping();
                List<FrameworkUAS.Entity.FieldMapping> fieldMappings = fmWorker.Select(sourceFileID, false);

                FrameworkUAS.BusinessLogic.SourceFile sfWorker = new FrameworkUAS.BusinessLogic.SourceFile();
                FrameworkUAS.Entity.SourceFile sf = sfWorker.SelectSourceFileID(sourceFileID, false);

                int standardTypeID;
                int demoTypeID;
                int demoRespOtherTypeID;

                FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                List<FrameworkUAD_Lookup.Entity.Code> codes = cWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Field_Mapping);
                standardTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString())).CodeId;
                demoTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString())).CodeId;
                demoRespOtherTypeID = codes.SingleOrDefault(x => x.CodeName.Equals(FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Other.ToString().Replace('_', ' '))).CodeId;

                List<string> incomingColumns = new List<string>();
                fieldMappings.OrderBy(x => x.ColumnOrder).ToList().ForEach(fm => incomingColumns.Add(fm.IncomingField));
                List<string> tmpDemoCols = new List<string>();
                fieldMappings.OrderBy(x => x.ColumnOrder).Where(t => t.FieldMappingTypeID == 37).ToList().ForEach(fm => tmpDemoCols.Add(fm.IncomingField));

                FrameworkUAD.BusinessLogic.SubscriberFinal subFinalWorker = new FrameworkUAD.BusinessLogic.SubscriberFinal();

                #region IgnoredReport                            
                List<FrameworkUAD.Entity.SubscriberFinal> ignoredList = new List<FrameworkUAD.Entity.SubscriberFinal>();
                ignoredList = subFinalWorker.SelectForIgnoredReport(processCode, true, client.ClientConnections, true);

                if (ignoredList.Count > 0)
                {
                    HashSet<FrameworkUAD.Entity.SubscriberFinal> hashList = new HashSet<FrameworkUAD.Entity.SubscriberFinal>(ignoredList);
                    #region Save File                
                    DataTable dtIgnoreReport = ToDataTableSubscriberFinal(hashList, incomingColumns, tmpDemoCols, fieldMappings);
                    if (dtIgnoreReport.Rows.Count > 0)
                    {
                        string clientArchiveDir = Core.ADMS.BaseDirs.getClientArchiveDir().ToString();
                        string dir = clientArchiveDir + @"\" + client.FtpFolder + @"\Reports\";
                        System.IO.Directory.CreateDirectory(dir);
                        string cleanProcessCode = Core_AMS.Utilities.StringFunctions.CleanProcessCodeForFileName(processCode);
                        string transformedReportName = dir + cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._IgnoredReport.ToString() + ".xlsx";
                        Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
                        Telerik.Windows.Documents.Spreadsheet.Model.Workbook wb = ef.GetWorkbook(dtIgnoreReport, "Ignore");
                        Telerik.Windows.Documents.Spreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();
                        ef.SaveWorkbook(wb, transformedReportName);
                        //using (FileStream output = new FileStream(transformedReportName, FileMode.Create))
                        //{
                        //    formatProvider.Export(wb, output);
                        //}
                    }
                    dtIgnoreReport = null;
                    #endregion
                }
                #endregion

                #region Processed            
                List<FrameworkUAD.Entity.SubscriberFinal> processedList = new List<FrameworkUAD.Entity.SubscriberFinal>();
                processedList = subFinalWorker.SelectForProcessedReport(processCode, false, client.ClientConnections, true);

                if (processedList.Count > 0)
                {
                    HashSet<FrameworkUAD.Entity.SubscriberFinal> hashList = new HashSet<FrameworkUAD.Entity.SubscriberFinal>(processedList);
                    #region Save File
                    DataTable dtProcessedReport = ToDataTableSubscriberFinal(hashList, incomingColumns, tmpDemoCols, fieldMappings);
                    if (dtProcessedReport.Rows.Count > 0)
                    {
                        string clientArchiveDir = Core.ADMS.BaseDirs.getClientArchiveDir().ToString();
                        string dir = clientArchiveDir + @"\" + client.FtpFolder + @"\Reports\";
                        System.IO.Directory.CreateDirectory(dir);
                        string cleanProcessCode = Core_AMS.Utilities.StringFunctions.CleanProcessCodeForFileName(processCode);
                        string transformedReportName = dir + cleanProcessCode + Core_AMS.Utilities.Enums.DashboardReportName._ProcessedReport.ToString() + ".xlsx";
                        Core_AMS.Utilities.ExcelFunctions ef = new Core_AMS.Utilities.ExcelFunctions();
                        Telerik.Windows.Documents.Spreadsheet.Model.Workbook wb = ef.GetWorkbook(dtProcessedReport, "Processed");
                        Telerik.Windows.Documents.Spreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();
                        ef.SaveWorkbook(wb, transformedReportName);
                        //using (FileStream output = new FileStream(transformedReportName, FileMode.Create))
                        //{
                        //    formatProvider.Export(wb, output);
                        //}
                    }
                    dtProcessedReport = null;
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".CreateDashboardFileHistoryReports");
            }
        }

        private DataTable ToDataTableSubscriberFinal(
            IReadOnlyCollection<SubscriberFinal> subList, 
            IReadOnlyCollection<string> incomingColumns, 
            IReadOnlyCollection<string> tmpDemoColumns, 
            IReadOnlyCollection<FieldMapping> fieldMappings)
        {
            if (subList == null)
            {
                throw new ArgumentNullException(nameof(subList));
            }

            if (incomingColumns == null)
            {
                throw new ArgumentNullException(nameof(incomingColumns));
            }

            if (fieldMappings == null)
            {
                throw new ArgumentNullException(nameof(fieldMappings));
            }

            var result = new DataTable();
            foreach (var incomingColumn in incomingColumns)
            {
                result.Columns.Add(incomingColumn);
            }

            try
            {
                var totalRows = subList.Count.ToString();
                var rowCounter = 1;

                var properties = TypeDescriptor.GetProperties(typeof(SubscriberFinal));
                foreach (var item in subList)
                {
                    Console.WriteLine($"Row {rowCounter} of {totalRows}");
                    var row = result.NewRow();

                    foreach (PropertyDescriptor prop in properties)
                    {
                        if (prop.DisplayName.Equals("ignore", StringComparison.CurrentCultureIgnoreCase))
                        {
                            continue;
                        }

                        var mappingFieldName = GetMappingFieldName(prop);
                        ProcessMappingField(incomingColumns, fieldMappings, item, mappingFieldName, row, prop);
                        ProcessDemographicFinalList(item, prop, row);
                    }

                    result.Rows.Add(row);
                    rowCounter++;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, client, $"{GetType().Name}.ToDataTableSubscriberFinal");
            }

            return result;
        }
 
        private void ProcessMappingField(
            IEnumerable<string> incomingColumns, 
            IReadOnlyCollection<FieldMapping> fieldMappings, 
            SubscriberFinal item,
            string mappingFieldName, 
            DataRow row, 
            PropertyDescriptor prop)
        {
            if (!fieldMappings.Any(x => x.MAFField.Equals(mappingFieldName, StringComparison.CurrentCultureIgnoreCase)))
            {
                return;
            }

            // Try using DisplayName which will match
            var fieldMapping = fieldMappings.Single(x => x.MAFField.Equals(mappingFieldName, StringComparison.CurrentCultureIgnoreCase));
            if (!incomingColumns.Any(x => x.Equals(fieldMapping.IncomingField, StringComparison.CurrentCultureIgnoreCase)))
            {
                return;
            }

            row[fieldMapping.IncomingField] = prop.GetValue(item) ?? string.Empty;
            
            // Attempt to convert DateTime to Date (example: QDate)
            if (prop.PropertyType.ToString().Contains(typeof(DateTime).ToString()))
            {
                var data = row[fieldMapping.IncomingField].ToString();
                if (!string.IsNullOrWhiteSpace(data))
                {
                    var date = Convert.ToDateTime(data);
                    row[fieldMapping.IncomingField] = date.ToShortDateString();
                }
            }            
        }

        private void ProcessDemographicFinalList(SubscriberFinal item, PropertyDescriptor prop, DataRow row)
        {
            const string demographicPropertyName = "DemographicFinalList";
            if (prop.Name != demographicPropertyName)
            {
                return;
            }

            var demographicFinalList = prop.GetValue(item) as HashSet<SubscriberDemographicFinal>;
            if (demographicFinalList == null)
            {
                return;
            }

            foreach (var demographicGroup in demographicFinalList.GroupBy(x => x.MAFField).OrderBy(g => g.Key))
            {
                const string demographicValueSeparator = ",";
                row[demographicGroup.Key] = string.Join(demographicValueSeparator, demographicGroup.Select(i => i.Value));
            }
        }

        private string GetMappingFieldName(MemberDescriptor prop)
        {
            string tempField;

            switch (prop.DisplayName.ToUpper())
            {
                case "ADDRESS":
                    tempField = "Address1";
                    break;
                case "MAILSTOP":
                    tempField = "Address2";
                    break;
                case "FNAME":
                    tempField = "FirstName";
                    break;
                case "LNAME":
                    tempField = "LastName";
                    break;
                case "PAR3C":
                    tempField = "Par3cID";
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
                case "TRANSACTIONID":
                    tempField = "PubTransactionID";
                    break;
                case "QDATE":
                    tempField = "QualificationDate";
                    break;
                case "STATE":
                    tempField = "RegionCode";
                    break;
                case "SEQUENCE":
                    tempField = "SequenceID";
                    break;
                case "SUBSRC":
                    tempField = "SubscriberSourceCode";
                    break;
                case "VERIFIED":
                    tempField = "Verify";
                    break;
                case "ZIP":
                    tempField = "ZipCode";
                    break;
                default:
                    tempField = prop.DisplayName;
                    break;
            }

            return tempField;
        }
    }
}
