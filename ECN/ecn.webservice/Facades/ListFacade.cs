using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;
using System.Xml;
using ecn.common.classes;
using ecn.webservice.Facades.Params;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Objects;
using KM.Common.Extensions;
using AccountsBusinessLayer = ECN_Framework_BusinessLayer.Accounts;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;
using CommunicatorObjects = ECN_Framework_Common.Objects.Communicator;
using StringFunctions = ECN_Framework_Common.Functions.StringFunctions;
using User = KMPlatform.Entity.User;

namespace ecn.webservice.Facades
{
    public class ListFacade : FacadeBase, IListFacade
    {
        private const string ApplicationIdAppSetting = "KMCommon_Application";
        private const string NotifyAdminSubject = "Error in ECN WebServices";
        private const string TotalRecordsType = "T";
        private const string InsertRecordType = "I";
        private const string UpdateRecordType = "U";
        private const string DuplicateRecordType = "D";
        private const string SkippedRecordType = "S";
        private const string MSkippedRecordType = "M";
        private const string LowerCaseUserPrefix = "user_";
        private const string UpperCaseUserPrefix = "USER_";

        private static ArrayList _emailsColumnHeadings;
        private Hashtable hUpdatedRecords = new Hashtable();
        private string _importResults = string.Empty;
        private string _importError = string.Empty;
        private int? LogID = null;

        private string ImportResults
        {
            set { _importResults = value; }
            get { return _importResults; }
        }

        private string ImportError
        {
            set { _importError = value; }
            get { return _importError; }
        }

        public string GetFolders(WebMethodExecutionContext context)
        {
            var folderList = Folder.GetByType(context.User.CustomerID,
                CommunicatorObjects.Enums.FolderTypes.GRP.ToString(),
                context.User);

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, BuildFolderReturnXML(folderList));
        }

        public string GetListEmailProfilesByEmailAddress(WebMethodExecutionContext context,
            GetListEmailProfilesParams parameters)
        {
            var swEmails = new StringWriter();
            var dataTable = EmailGroup.GetGroupEmailProfilesWithUDF(
                parameters.ListId,
                context.User.CustomerID,
                " and emails.emailaddress = '" + parameters.EmailAddress + "' ", "'S','P','U'");

            dataTable.TableName = "EmailProfiles";
            dataTable.WriteXml(swEmails);
            var xmlEmails = swEmails.ToString();

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, xmlEmails);
        }

        public string GetCustomFields(WebMethodExecutionContext context, int listId)
        {
            if (Group.Exists(listId, context.User.CustomerID))
            {
                var gdfList = GroupDataFields.GetByGroupID(listId, context.User);
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, BuildGDFReturnXML(gdfList));
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "LIST DOESN'T EXIST");
        }

        public string GetFilters(WebMethodExecutionContext context, int listId)
        {
            if (Group.Exists(listId, context.User.CustomerID))
            {
                var filterList = Filter.GetByGroupID(listId, true, context.User);
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, BuildFilterReturnXML(filterList));
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "LIST DOESN'T EXIST");
        }

        public string GetLists(WebMethodExecutionContext context)
        {
            var groupsStringWriter = new StringWriter();

            context.User.CustomerID = AccountsBusinessLayer.Customer.GetByClientID(
                    context.User.DefaultClientID,
                    false)
                .CustomerID;
            var groupsDataTable = Group.GetGroupDR(
                context.User.CustomerID,
                context.User.UserID,
                context.User);
            groupsDataTable.TableName = "Group";
            groupsDataTable.WriteXml(groupsStringWriter);
            var xmlGroup = groupsStringWriter.ToString();

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, xmlGroup);
        }

        public string GetListByName(WebMethodExecutionContext context, string name)
        {
            var dtGroups = Group.GetByGroupName(name, "equals", context.User);

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, BuildGroupReturnXML(dtGroups));
        }

        public string GetListsByFolderId(WebMethodExecutionContext context, int folderId)
        {
            var groupList = Group.GetByFolderIDCustomerID(folderId, context.User);

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, BuildGroupReturnXML(groupList));
        }

        public string GetSubscriberCount(WebMethodExecutionContext context, int groupId)
        {
            int blastCount = 0;
            try
            {
                blastCount = EmailGroup.GetSubscriberCount(
                    groupId,
                    Convert.ToInt32(context.User.CustomerID),
                    context.User);
            }
            catch (Exception)
            {
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, blastCount.ToString());
        }

        public string DeleteFolder(WebMethodExecutionContext context, int folderId)
        {
            if (Folder.Exists(folderId, context.User.CustomerID))
            {
                Folder.Delete(folderId, context.User);
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, "FOLDER DELETED", folderId);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "FOLDER DOESN'T EXIST");
        }

        public string DeleteList(WebMethodExecutionContext context, int listId)
        {
            if (Group.Exists(listId, context.User.CustomerID))
            {
                Group.Delete(listId, context.User);
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, "LIST DELETED", listId);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "LIST DOESN'T EXIST");
        }

        public string DeleteSubscriber(WebMethodExecutionContext context, DeleteSubscriberParams parameters)
        {
            var emailGroup = EmailGroup.GetByEmailAddressGroupID(
                parameters.EmailAddress,
                parameters.ListId,
                context.User);

            if (emailGroup != null && emailGroup.EmailID > 0)
            {
                EmailGroup.Delete(parameters.ListId, emailGroup.EmailID, context.User);
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, "SUBSCRIBER DELETED", parameters.ListId);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "SUBSCRIBER DOESN'T EXIST");
        }

        public string DeleteCustomField(WebMethodExecutionContext context, DeleteCustomFieldParams parameters)
        {
            if (GroupDataFields.Exists(parameters.UdfId, parameters.ListId, context.User.CustomerID))
            {
                GroupDataFields.Delete(parameters.UdfId, parameters.ListId, context.User);
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);

                return GetSuccessResponse(context, "CUSTOM FIELD DELETED", parameters.ListId);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "CUSTOM FIELD DOESN'T EXIST");
        }

        public string UnsubscribeSubscriber(WebMethodExecutionContext context, UnsubscribeSubscriberParams parameters)
        {
            if (Group.Exists(parameters.ListId, context.User.CustomerID))
            {
                var emails = GetXMLEmails(parameters.XmlEmails);
                if (emails.Length > 0)
                {
                    EmailGroup.UnsubscribeSubscribers(parameters.ListId, emails, context.User);
                    context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);

                    return GetSuccessResponse(context, "SUBSCRIBER UPDATED", parameters.ListId);
                }

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetFailResponse(context, "NO EMAILS FOUND");
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "GROUP DOESN'T EXIST");
        }

        public string UpdateCustomField(WebMethodExecutionContext context, CustomFieldParams parameters)
        {
            var groupField = GroupDataFields.GetByID(parameters.UdfId, parameters.ListId, context.User);

            if (groupField != null)
            {
                groupField.ShortName = parameters.CustomFieldName;
                groupField.LongName = parameters.CustomFieldDescription;
                groupField.IsPublic = parameters.IsPublic;
                groupField.UpdatedUserID = context.User.UserID;
                GroupDataFields.Save(groupField, context.User);

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, "CUSTOM FIELD UPDATED", groupField.GroupDataFieldsID);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "CUSTOM FIELD DOESN'T EXIST FOR CUSTOMER");
        }

        public string AddCustomField(WebMethodExecutionContext context, CustomFieldParams parameters)
        {
            if (!GroupDataFields.Exists(parameters.CustomFieldName, null, parameters.ListId, context.User.CustomerID))
            {
                var groupDataFields = new CommunicatorEntities.GroupDataFields
                {
                    CustomerID = context.User.CustomerID,
                    CreatedUserID = context.User.UserID,
                    GroupID = parameters.ListId,
                    ShortName = parameters.CustomFieldName,
                    LongName = parameters.CustomFieldDescription,
                    IsPublic = parameters.IsPublic
                };

                GroupDataFields.Save(groupDataFields, context.User);
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, "CUSTOM FIELD ADDED", groupDataFields.GroupDataFieldsID);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "CUSTOM FIELD ALREADY EXISTS FOR GROUP");
        }

        public string AddSubscribers(WebMethodExecutionContext context, AddSubscribersParams parameters)
        {
            ValidateCustomerIds(parameters.ListId, context.User, context.User.CustomerID);
            ValidateListId(context, parameters.ListId);
            CheckAndFixSubscriptionType(parameters);
            CheckAndFixFormatType(parameters);

            if (parameters.XmlString.Length > 0 && parameters.ListId > 0)
            {
                ExtractCoumnNamesFromEmailsTable();
                var xmlDataTable = ExtractColumnNamesFromXmlString(parameters, context.User.CustomerID);

                if (parameters.AutoGenerateUdfs)
                {
                    GenerateUdfs(context.User, parameters.ListId, xmlDataTable);
                }

                if (xmlDataTable != null)
                {
                    bool importSuccess;
                    try
                    {
                        importSuccess = ImportData(
                            context.User,
                            parameters.ListId,
                            parameters.SubscriptionType,
                            parameters.FormatType,
                            xmlDataTable,
                            parameters.Source);
                    }
                    catch (NullReferenceException)
                    {
                        context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                        return GetFailResponse(context, "GROUP DOES NOT EXIST");
                    }

                    if (importSuccess)
                    {
                        if (xmlDataTable.Columns.Contains("Emailaddress"))
                        {
                            var groupPlansList = LayoutPlans.GetByGroupID_NoAccessCheck(
                                parameters.ListId,
                                context.User.CustomerID);

                            if (groupPlansList.Count > 0)
                            {
                                CreateEvents(context.User, parameters.ListId, xmlDataTable, false, false, parameters.SmartFormId);
                            }
                        }

                        context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                        return GetSuccessResponse(context, ImportResults);
                    }

                    context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                    return GetFailResponse(context, WebUtility.HtmlEncode(ImportError));
                }

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetFailResponse(context, "INVALID XML STRING");
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "INVALID LIST ID / XML STRING");
        }

        public string AddSubscribersWithDupes(WebMethodExecutionContext context, AddSubscribersParams parameters)
        {
            ValidateListId(context, parameters.ListId);
            ValidateCustomerIds(parameters.ListId, context.User, context.User.CustomerID);
            CheckAndFixSubscriptionType(parameters);
            CheckAndFixFormatType(parameters);
            if (!IsCompositeKeyValid(parameters))
            {
                return GetFailResponse(context, "INVALID COMPOSITE KEY");
            }

            if (parameters.XmlString.Length > 0 && parameters.ListId > 0)
            {
                ExtractCoumnNamesFromEmailsTable();
                var xmlDataTable = ExtractColumnNamesFromXmlString(parameters, context.User.CustomerID);

                if (xmlDataTable != null)
                {
                    if (IsAddDupeSubscribersFeatureAvailable(context.User.DefaultClientID))
                    {
                        var importSuccess = ImportDataWithDupes(
                            context.User,
                            parameters.ListId,
                            parameters.SubscriptionType,
                            parameters.FormatType,
                            parameters.CompositeKey,
                            parameters.OverwriteWithNull,
                            xmlDataTable,
                            context.ServiceMethodName);

                        if (importSuccess)
                        {
                            if (parameters.CreateEmailActivityLog)
                            {
                                var emailId = EmailGroup.GetEmailIDFromComposite(
                                    parameters.ListId,
                                    context.User.CustomerID,
                                    xmlDataTable.Rows[0]["Emailaddress"].ToString().Replace("'", "''"),
                                    parameters.CompositeKey,
                                    xmlDataTable.Rows[0][parameters.CompositeKey].ToString().Replace("'", "''"),
                                    context.User);

                                CreateEmailActivityLog(context.User, emailId);
                            }

                            CreateEvents(context.User, parameters.ListId, xmlDataTable, true, parameters.EscapeApostrophesInEmailAddress, parameters.SmartFormId);

                            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                            return GetSuccessResponse(context, ImportResults);
                        }

                        var logId = -1;
                        if (!string.IsNullOrWhiteSpace(ImportError))
                        {
                            logId = LogCriticalError(new Exception(ImportError), context.ServiceMethodName);
                        }

                        context.ApiLoggingManager.UpdateLog(context.ApiLogId, logId);
                        return GetFailResponse(context, System.Net.WebUtility.HtmlEncode(ImportError));
                    }

                    throw new SecurityException();
                }

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetFailResponse(context, "INVALID XML STRING");
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "INVALID LIST ID / XML STRING");
        }

        public string AddSubscriberUsingSmartForm(WebMethodExecutionContext context, AddSubscribersParams parameters)
        {
            ValidateListId(context, parameters.ListId);
            ValidateCustomerIds(parameters.ListId, context.User, context.User.CustomerID);
            CheckAndFixSubscriptionType(parameters);
            CheckAndFixFormatType(parameters);

            if (parameters.XmlString.Length > 0 && parameters.ListId > 0)
            {
                ExtractCoumnNamesFromEmailsTable();
                var xmlDataTable = ExtractColumnNamesFromXmlString(parameters, context.User.CustomerID);

                if (xmlDataTable != null)
                {
                    bool importSuccess;
                    var emailId = -1;

                    var smartFormTracking = new CommunicatorEntities.SmartFormTracking
                    {
                        GroupID = parameters.ListId,
                        SFID = parameters.SmartFormId,
                        CustomerID = context.User.CustomerID
                    };
                    SmartFormTracking.Insert(smartFormTracking);

                    using (var transactionScope = new TransactionScope())
                    {
                        importSuccess = ImportData(
                            context.User,
                            parameters.ListId,
                            parameters.SubscriptionType,
                            parameters.FormatType,
                            xmlDataTable);

                        if (importSuccess)
                        {
                            emailId = EmailGroup.GetEmailIDFromWhatEmail(
                                parameters.ListId,
                                context.User.CustomerID,
                                xmlDataTable.Rows[0]["Emailaddress"].ToString().Replace("'", "''"),
                                context.User);

                            CreateEmailActivityLog(context.User, emailId);

                            EventOrganizer.Event(
                                context.User.CustomerID,
                                parameters.ListId,
                                emailId,
                                context.User,
                                parameters.SmartFormId);

                            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                        }
                        else
                        {
                            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                        }

                        transactionScope.Complete();
                    }

                    if (importSuccess && emailId > 0)
                    {
                        SendEmailFromSmartForm(
                            parameters.SmartFormId.Value,
                            xmlDataTable.Rows[0],
                            parameters.ListId,
                            context.User.CustomerID,
                            emailId,
                            xmlDataTable.Rows[0]["Emailaddress"].ToString().Replace("'", "''"),
                            context.User);

                        return GetSuccessResponse(context, ImportResults);
                    }

                    return GetFailResponse(context, WebUtility.HtmlEncode(ImportError));
                }

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetFailResponse(context, "INVALID XML STRING");
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "INVALID LIST ID / XML STRING");
        }

        public DataTable ExtractColumnNamesFromXmlString(string xmlString, int listId, int customerId)
        {
            var fixedXml = xmlString.Replace("&", "&amp;");
            DataTable resultDataTable = null;
            var xmlFilePath = string.Format(
                "{0}_{1}_{2}_{3:MMddyyyy_hhmmss_ffff}_{4}.xml",
                ConfigurationManager.AppSettings["XMLPath"],
                customerId,
                listId,
                DateTime.Now,
                Guid.NewGuid().ToString().Substring(1, 5));

            StreamWriter file = null;
            try
            {
                file = new StreamWriter(xmlFilePath);
                file.Write(fixedXml);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error: {0}", ex);
                var body = $" extractColumnNamesFromXMLString :  {xmlFilePath}  / {ex.Message}";
                NotifyAdmin(NotifyAdminSubject, body);
            }
            finally
            {
                file.Flush();
                file.Close();
            }

            try
            {
                var dataset = new DataSet();
                dataset.ReadXml(xmlFilePath);
                resultDataTable = dataset.Tables[0];
                File.Delete(xmlFilePath);
            }
            catch (Exception ex)
            {
                var body = $" extractColumnNamesFromXMLString READXML :  {xmlFilePath}  / {ex.Message}";
                NotifyAdmin(NotifyAdminSubject, body);
            }

            return resultDataTable;
        }

        private void ValidateListId(WebMethodExecutionContext context, int listId)
        {
            if (listId <= 0)
            {
                LogNonCriticalError($"Unknown GroupID: {listId}", context.ServiceMethodName);
            }
        }

        private void CreateEmailActivityLog(User user, int emailId)
        {
            var emailActivityLog = new CommunicatorEntities.EmailActivityLog
            {
                EmailID = emailId,
                ActionTypeCode = CommunicatorObjects.Enums.ActionTypeCode.Subscribe.ToString(),
                ActionValue = CommunicatorObjects.Enums.ActionValue.S.ToString(),
                BlastID = 0
            };
            EmailActivityLog.Insert(emailActivityLog, user);
        }

        private int LogCriticalError(Exception exception, string source)
        {
            var appId = Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationIdAppSetting]);

            return KM.Common.Entity.ApplicationLog.LogCriticalError(
                exception,
                source,
                appId);
        }

        private int LogNonCriticalError(string errorMessage, string source)
        {
            var appId = Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationIdAppSetting]);

            return KM.Common.Entity.ApplicationLog.LogNonCriticalError(
                errorMessage,
                source,
                appId);
        }

        private bool IsAddDupeSubscribersFeatureAvailable(int clientId)
        {
            return KMPlatform.BusinessLogic.Client.HasServiceFeature(
                clientId,
                KMPlatform.Enums.Services.EMAILMARKETING,
                KMPlatform.Enums.ServiceFeatures.AddDupeSubscribers);
        }

        private bool IsCompositeKeyValid(AddSubscribersParams parameters)
        {
            var fixedCompositeKey = parameters.CompositeKey.ToLower();

            return fixedCompositeKey == "user1" ||
                   fixedCompositeKey == "user2" ||
                   fixedCompositeKey == "user3" ||
                   fixedCompositeKey == "user4" ||
                   fixedCompositeKey == "user5" ||
                   fixedCompositeKey == "user6";
        }

        private void CreateEvents(User user, int listId, DataTable xmlDataTable, bool accessCheck, bool escapeApostrophes, int? smartFormId)
        {
            CommunicatorEntities.EmailGroup emailGroup;

            foreach (DataRow xmlDataRow in xmlDataTable.Rows)
            {
                var emailAddress = xmlDataRow["Emailaddress"].ToString();

                if (escapeApostrophes)
                {
                    emailAddress = emailAddress.Replace("'", "''");
                }

                if (accessCheck)
                {
                    emailGroup = EmailGroup.GetByEmailAddressGroupID(
                        emailAddress,
                        listId,
                        user);
                }
                else
                {
                    emailGroup = EmailGroup.GetByEmailAddressGroupID_NoAccessCheck(
                        emailAddress,
                        listId);

                }

                if (emailGroup != null)
                {
                    EventOrganizer.Event(
                        user.CustomerID,
                        listId,
                        emailGroup.EmailID,
                        user,
                        smartFormId);
                }
            }
        }

        private void GenerateUdfs(User user, int listId, DataTable xmlDataTable)
        {
            var hgdfFields = GetUdfsForList(listId, user);
            var udfNames = (from DataColumn col in xmlDataTable.Columns
                where col.ColumnName.ToUpper().Contains(UpperCaseUserPrefix)
                select col.ColumnName).ToList();
            var arrayList = new ArrayList(hgdfFields.Keys);
            var existingUdfs = arrayList.Cast<string>().ToList();
            var intersectList = udfNames.Intersect(existingUdfs, StringComparer.OrdinalIgnoreCase);
            udfNames = RemoveFromList(udfNames, intersectList);

            foreach (var udfName in udfNames)
            {
                var groupDataFields = new CommunicatorEntities.GroupDataFields
                {
                    GroupID = listId,
                    ShortName = udfName.Replace(LowerCaseUserPrefix, string.Empty),
                    LongName = udfName.Replace(LowerCaseUserPrefix, string.Empty),
                    IsPublic = "Y",
                    CreatedUserID = user.UserID,
                    CustomerID = user.CustomerID
                };
                GroupDataFields.Save(groupDataFields, user);
            }
        }

        private void CheckAndFixFormatType(AddSubscribersParams parameters)
        {
            parameters.FormatType = parameters.FormatType.ToLower();
            if (parameters.FormatType != "html" && parameters.FormatType != "text")
            {
                parameters.FormatType = "html";
            }
        }

        private void CheckAndFixSubscriptionType(AddSubscribersParams parameters)
        {
            if (parameters.SubscriptionType != "S" &&
                parameters.SubscriptionType != "P" &&
                parameters.SubscriptionType != "U")
            {
                parameters.SubscriptionType = "S";
            }
        }

        private bool ImportDataWithDupes(
            User user,
            int listId,
            string subscriptionType,
            string formatType,
            string compositeKey,
            bool overwriteWithNull,
            DataTable fileDataTable,
            string source = "ecn.webservice.listmanager.importDataWithDupes")
        {
            var startDateTime = DateTime.Now;

            var xmlUdfStringBuilder = new StringBuilder(string.Empty);
            var xmlProfileStringBuilder = new StringBuilder(string.Empty);

            try
            {
                var hgdfFields = GetUdfsForList(listId, user);

                for (int cnt = 0; cnt < fileDataTable.Rows.Count; cnt++)
                {

                    DataRow drFile = fileDataTable.Rows[cnt];

                    xmlProfileStringBuilder.Append(GetProfilesXml(fileDataTable, drFile));
                    xmlUdfStringBuilder.Append(GetUdfXml(compositeKey, fileDataTable, drFile, hgdfFields));

                    if (cnt != 0 && cnt % 10000 == 0 || cnt == fileDataTable.Rows.Count - 1)
                    {
                        var recordsDataTable = EmailGroup.ImportEmailsWithDupes_NoAccessCheck(
                            user,
                            listId,
                            $"<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{xmlProfileStringBuilder}</XML>",
                            $"<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{xmlUdfStringBuilder}</XML>",
                            formatType,
                            subscriptionType,
                            false,
                            compositeKey,
                            overwriteWithNull,
                            source);

                        if (recordsDataTable.Rows.Count > 0)
                        {
                            foreach (DataRow dr in recordsDataTable.Rows)
                            {
                                if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                                    hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(),
                                        Convert.ToInt32(dr["Counts"]));
                                else
                                {
                                    int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                                    hUpdatedRecords[dr["Action"].ToString().ToUpper()] =
                                        eTotal + Convert.ToInt32(dr["Counts"]);
                                }
                            }
                        }

                        xmlProfileStringBuilder = new StringBuilder(string.Empty);
                        xmlUdfStringBuilder = new StringBuilder(string.Empty);
                    }
                }

                hgdfFields.Clear();

                if (hUpdatedRecords.Count > 0)
                {
                    ImportResults = GenerateImportResultsString(startDateTime);
                }

                return true;
            }
            catch (Exception ex)
            {
                ImportError = ex.ToString();
                return false;
            }
        }

        private string GetProfilesXml(DataTable fileDataTable, DataRow drFile)
        {
            var profilesXmlStringBuilder = new StringBuilder(string.Empty);
            profilesXmlStringBuilder.Append("<Emails>");

            foreach (DataColumn dcFile in fileDataTable.Columns)
            {
                if (dcFile.ColumnName.ToLower().IndexOf(LowerCaseUserPrefix) == -1)
                {
                    var fileDataColumnTag = string.Format("<{0}>{1}</{2}>",
                        dcFile.ColumnName.ToLower(),
                        cleanXMLString(drFile[dcFile.ColumnName].ToString()),
                        dcFile.ColumnName.ToLower());
                    profilesXmlStringBuilder.Append(fileDataColumnTag);
                }

            }

            profilesXmlStringBuilder.Append("</Emails>");

            return profilesXmlStringBuilder.ToString();
        }

        private string GetUdfXml(
            string compositeKey,
            DataTable fileDataTable,
            DataRow fileDataRow,
            Hashtable hgdfFields)
        {
            bool bRowCreated;
            bRowCreated = false;
            var xmlUdfStringBuilder = new StringBuilder(string.Empty);

            foreach (DataColumn fileDataColumn in fileDataTable.Columns)
            {
                if (hgdfFields.Count > 0)
                {
                    if (fileDataColumn.ColumnName.ToLower().IndexOf(LowerCaseUserPrefix) > -1)
                    {
                        if (!bRowCreated)
                        {
                            var emailAddress = cleanXMLString(fileDataRow["emailaddress"].ToString());
                            xmlUdfStringBuilder.Append("<row>");

                            if (compositeKey.IsNullOrWhiteSpace())
                            {
                                xmlUdfStringBuilder.Append("<ea>" + emailAddress + "</ea>");
                            }
                            else
                            {
                                xmlUdfStringBuilder.Append("<ea ");

                                if (cleanXMLString(fileDataRow[compositeKey].ToString()).Trim().Length == 0)
                                {
                                    throw new Exception("Cannot have empty CompositeKey");
                                }

                                xmlUdfStringBuilder.Append(
                                    "kv=\"" + cleanXMLString(fileDataRow[compositeKey].ToString()).Trim() +
                                    "\">");

                                xmlUdfStringBuilder.Append(emailAddress + "</ea>");
                            }

                            bRowCreated = true;
                        }

                        var fieldId = hgdfFields[fileDataColumn.ColumnName.ToLower()];
                        var fieldData =
                            cleanXMLString(fileDataRow[fileDataColumn.ColumnName.ToLower()].ToString())
                                .Replace("&amp;", "&");

                        xmlUdfStringBuilder
                            .Append("<udf id=\"" + fieldId + "\">")
                            .Append("<v><![CDATA[" + fieldData + "]]></v>")
                            .Append("</udf>");
                    }
                }
            }

            if (bRowCreated)
            {
                xmlUdfStringBuilder.Append("</row>");
            }

            return xmlUdfStringBuilder.ToString();
        }

        private bool ImportData(
            User user,
            int listId,
            string subscriptionType,
            string formatType,
            DataTable fileDataTable,
            string source = "ecn.webservice.listmanager.importData")
        {
            var xmlUdfStringBuilder = new StringBuilder(string.Empty);
            var xmlProfilesStringBuilder = new StringBuilder(string.Empty);

            var startDateTime = DateTime.Now;

            try
            {
                for (int rowIndex = 0; rowIndex < fileDataTable.Rows.Count; rowIndex++)
                {
                    var hgdfFields = GetUdfsForList(listId, user);
                    var fileDataRow = fileDataTable.Rows[rowIndex];
                    xmlProfilesStringBuilder.Append(GetProfilesXml(fileDataTable, fileDataRow));
                    xmlUdfStringBuilder.Append(GetUdfXml(string.Empty, fileDataTable, fileDataRow, hgdfFields));

                    if (rowIndex != 0 && rowIndex % 5000 == 0 || rowIndex == fileDataTable.Rows.Count - 1)
                    {
                        var recordsDataTable = EmailGroup.ImportEmails(user, user.CustomerID, listId,
                            $"<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{xmlProfilesStringBuilder}</XML>",
                            $"<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{xmlUdfStringBuilder}</XML>",
                            formatType, subscriptionType, false, string.Empty, source);

                        if (recordsDataTable.Rows.Count > 0)
                        {
                            foreach (DataRow dr in recordsDataTable.Rows)
                            {
                                if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                                {
                                    hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(),
                                        Convert.ToInt32(dr["Counts"]));
                                }
                                else
                                {
                                    int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                                    hUpdatedRecords[dr["Action"].ToString().ToUpper()] =
                                        eTotal + Convert.ToInt32(dr["Counts"]);
                                }
                            }
                        }

                        xmlProfilesStringBuilder = new StringBuilder(string.Empty);
                        xmlUdfStringBuilder = new StringBuilder(string.Empty);
                    }
                }

                if (hUpdatedRecords.Count > 0)
                {
                    ImportResults = GenerateImportResultsString(startDateTime);
                }

                return true;
            }
            catch (NullReferenceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                ImportError = ex.ToString();
                return false;
            }
        }

        private string GenerateImportResultsString(DateTime startDateTime)
        {
            var result = string.Empty;

            foreach (DictionaryEntry updatedRecord in hUpdatedRecords)
            {
                if (updatedRecord.Key.ToString() == TotalRecordsType)
                {
                    result += $"<TotalRecords>{updatedRecord.Value}</TotalRecords>";
                }
                else if (updatedRecord.Key.ToString() == InsertRecordType)
                {
                    result += $"<New>{updatedRecord.Value}</New>";
                }
                else if (updatedRecord.Key.ToString() == UpdateRecordType)
                {
                    result += $"<Changed>{updatedRecord.Value}</Changed>";
                }
                else if (updatedRecord.Key.ToString() == DuplicateRecordType)
                {
                    result += $"<Duplicates>{updatedRecord.Value}</Duplicates>";
                }
                else if (updatedRecord.Key.ToString() == SkippedRecordType)
                {
                    result += $"<Skipped>{updatedRecord.Value}</Skipped>";
                }
                else if (updatedRecord.Key.ToString() == MSkippedRecordType)
                {
                    result += $"<MSSkipped>{updatedRecord.Value}</MSSkipped>";
                }
            }

            var duration = DateTime.Now - startDateTime;
            result += string.Format("<ImportTime>{0}:{1}:{2}:{3}</ImportTime>",
                duration.Hours,
                duration.Minutes,
                duration.Seconds,
                duration.Milliseconds);

            return result;
        }

        private string cleanXMLString(string inputXml)
        {
            return inputXml
                .Replace("&", "&amp;")
                .Replace("\"", "&quot;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("á", "a");
        }

        private List<T> RemoveFromList<T>(IList<T> inputList, IEnumerable<T> itemsToRemove)
        {
            var itemsToRemoveHashSet = new HashSet<T>(itemsToRemove);

            var list = inputList as List<T>;
            if (list == null)
            {
                int listItemIndex = 0;
                while (listItemIndex < inputList.Count)
                {
                    if (itemsToRemoveHashSet.Contains(inputList[listItemIndex]))
                    {
                        inputList.RemoveAt(listItemIndex);
                    }
                    else
                    {
                        listItemIndex++;
                    }
                }
            }
            else
            {
                list.RemoveAll(itemsToRemoveHashSet.Contains);
            }

            return list;
        }

        private Hashtable GetUdfsForList(int groupId, User user)
        {
            var fieldsHashTable = new Hashtable();
            var groupFieldsList = GroupDataFields.GetByGroupID(groupId, user);
            if (groupFieldsList.Count > 0)
            {
                foreach (var groupField in groupFieldsList)
                {
                    fieldsHashTable.Add($"user_{groupField.ShortName.ToLower()}", groupField.GroupDataFieldsID);
                }
            }

            return fieldsHashTable;
        }

        private DataTable ExtractColumnNamesFromXmlString(AddSubscribersParams parameters, int customerId)
        {
            return ExtractColumnNamesFromXmlString(parameters.XmlString, parameters.ListId, customerId);
        }
        
        private void NotifyAdmin(string subject, string body)
        {
            if (ConfigurationManager.AppSettings["Admin_Notify"] == "true")
            {
                var adminEmailBody = new StringBuilder(body);
                adminEmailBody.AppendLine();

                var emailFunctions = new communicator.classes.EmailFunctions();
                emailFunctions.SimpleSend(
                    ConfigurationManager.AppSettings["Admin_ToEmail"],
                    ConfigurationManager.AppSettings["Admin_FromEmail"],
                    subject,
                    adminEmailBody.ToString());
            }
        }

        private void ExtractCoumnNamesFromEmailsTable()
        {
            DataTable email = Email.GetColumnNames();
            _emailsColumnHeadings = DataFunctions.GetDataTableColumns(email);

            for (int i = 0; i < _emailsColumnHeadings.Count; i++)
            {
                _emailsColumnHeadings[i] = _emailsColumnHeadings[i].ToString().ToLower();
            }
        }

        private void ValidateCustomerIds(int groupId, User user, int customerId)
        {
            var group = Group.GetByGroupID(groupId, user);
            if (group != null)
            {
                if (group.CustomerID != customerId)
                {
                    var exceptionMessage = $"ACCESS TO GROUP {groupId} IS NOT ALLOWED";
                    throw new SecurityException(exceptionMessage);
                }
            }
            else
            {
                var errorList = new List<ECNError>
                {
                    new ECNError(Enums.Entity.Group, Enums.Method.Validate, "GROUP DOES NOT EXIST")
                };

                throw new ECNException("GROUP DOES NOT EXIST", errorList);
            }
        }

        private string BuildFolderReturnXML(List<CommunicatorEntities.Folder> folderList)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<DocumentElement xmlns=\"\">");

            foreach (var folder in folderList)
            {
                stringBuilder.Append("<Folder><FolderID>");
                stringBuilder.Append(folder.FolderID);
                stringBuilder.Append("</FolderID><FolderName><![CDATA[");
                stringBuilder.Append(folder.FolderName);
                stringBuilder.Append("]]></FolderName></Folder>");
            }

            stringBuilder.Append("</DocumentElement>");
            return stringBuilder.ToString();
        }

        private string BuildGDFReturnXML(List<CommunicatorEntities.GroupDataFields> groupDataFields)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<DocumentElement xmlns=\"\">");
            foreach (var groupDataField in groupDataFields)
            {
                stringBuilder.Append("<CustomFields><GroupDataFieldsID>");
                stringBuilder.Append(groupDataField.GroupDataFieldsID);
                stringBuilder.Append("</GroupDataFieldsID><ShortName>");
                stringBuilder.Append(groupDataField.ShortName);
                stringBuilder.Append("</ShortName><LongName>");
                stringBuilder.Append(groupDataField.LongName);
                stringBuilder.Append("</LongName><IsPublic>");
                stringBuilder.Append(groupDataField.IsPublic);
                stringBuilder.Append("</IsPublic></CustomFields>");
            }

            stringBuilder.Append("</DocumentElement>");
            return stringBuilder.ToString();
        }

        private string BuildFilterReturnXML(List<CommunicatorEntities.Filter> filters)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<DocumentElement xmlns=\"\">");
            foreach (var filter in filters)
            {
                stringBuilder.Append("<Filter><FilterID>");
                stringBuilder.Append(filter.FilterID);
                stringBuilder.Append("</FilterID><FilterName><![CDATA[");
                stringBuilder.Append(filter.FilterName);
                stringBuilder.Append("]]></FilterName></Filter>");
            }

            stringBuilder.Append("</DocumentElement>");
            return stringBuilder.ToString();
        }

        private string BuildGroupReturnXML(DataTable groupDataTable)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<DocumentElement xmlns=\"\">");
            if (groupDataTable != null && groupDataTable.Rows.Count > 0)
            {
                foreach (DataRow groupDataRow in groupDataTable.Rows)
                {
                    stringBuilder.Append("<Lists><ListID>");
                    stringBuilder.Append(groupDataRow["GroupID"]);
                    stringBuilder.Append("</ListID><ListName><![CDATA[");
                    stringBuilder.Append(groupDataRow["GroupName"]);
                    stringBuilder.Append("]]></ListName></Lists>");
                }
            }
            stringBuilder.Append("</DocumentElement>");
            return stringBuilder.ToString();
        }

        private string BuildGroupReturnXML(List<CommunicatorEntities.Group> groupList)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<DocumentElement xmlns=\"\">");
            foreach (var groupItem in groupList)
            {
                stringBuilder.Append("<Group><GroupID>");
                stringBuilder.Append(groupItem.GroupID);
                stringBuilder.Append("</GroupID><GroupName><![CDATA[");
                stringBuilder.Append(groupItem.GroupName);
                stringBuilder.Append("]]></GroupName></Group>");
            }
            stringBuilder.Append("</DocumentElement>");
            return stringBuilder.ToString();
        }

        private string GetXMLEmails(string xmlEmails)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlEmails);

            var emailAddresses = string.Empty;
            var xmlNode = xmlDoc.SelectSingleNode("//EmailAddresses");
            if (xmlNode != null)
            {
                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    if (childNode.Name == "EmailAddress")
                    {
                        emailAddresses += "'" + childNode.InnerText + "',";
                    }
                }
                emailAddresses = emailAddresses.Remove(emailAddresses.Length - 1, 1);
            }
            return emailAddresses;
        }

        private void SendEmailFromSmartForm(
            int smartFormId,
            DataRow xmlDataRow,
            int groupId,
            int customerId,
            int emailId,
            string emailAddress,
            User user)
        {
            if (smartFormId > 0)
            {
                var licenseCheck = new LicenseCheck();
                CommunicatorEntities.SmartFormActivityLog log;

                if (emailId > 0)
                {
                    CommunicatorEntities.Email emailObj = Email.GetByEmailID(emailId, user);
                    emailObj.EmailAddress = emailAddress;
                    var group = Group.GetByGroupID(groupId, user);
                    var emailDirect = new CommunicatorEntities.EmailDirect();
                    var smartFormHistory = SmartFormsHistory.GetBySmartFormID(smartFormId, groupId, user);

                    emailDirect.CustomerID = customerId;
                    emailDirect.EmailAddress = emailObj.EmailAddress;
                    emailDirect.EmailSubject = "Knowledge Marketing Gateway - Reset Password";
                    emailDirect.FromName = "Webservice";
                    emailDirect.Process = "Webservice - ListManager.SendEmailFromSF";
                    emailDirect.Source = "Webservice";
                    emailDirect.ReplyEmailAddress = "info@knowledgemarketing.com";
                    emailDirect.SendTime = DateTime.Now;
                    emailDirect.CreatedUserID = user.UserID;
                    emailDirect.Content = smartFormHistory.Response_UserMsgBody;

                    if (smartFormHistory != null)
                    {
                        string from = smartFormHistory.Response_FromEmail;
                        string adminEmail = smartFormHistory.Response_AdminEmail;

                        /* Send Response Email to user*/

                        if (from.Length > 5 && emailAddress.Length > 5)
                        {
                            if (emailDirect.Content.ToLower().IndexOf("%%unsubscribelink%%") > 0)
                            {
                                emailDirect.Content = StringFunctions.Replace(emailDirect.Content, "http://%%unsubscribelink%%", "%%unsubscribelink%%");
                                emailDirect.Content = StringFunctions.Replace(emailDirect.Content, "%%unsubscribelink%%", ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/unsubscribe.aspx?e=" + emailAddress + "&g=" + groupId + "&b=0&c=" + customerId + "&s=U");
                            }
                            else
                            {
                                //Add Unsubscribe Link at the bottom of the email as per CAN-SPAM & USI requested it to be done. 
                                string unsubscribeText = "<p style=\"padding-TOP:5px\"><div style=\"font-size:8.0pt;font-family:'Arial,sans-serif'; color:#666666\"><IMG style=\"POSITION:relative; TOP:5px\" src='" + ConfigurationManager.AppSettings["Image_DomainPath"] + "/images/Sure-Unsubscribe.gif'/>&nbsp;If you feel you have received this message in error, or wish to be removed, please <a href='" + ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/unsubscribe.aspx?e=" + emailAddress + "&g=" + groupId + "&b=0&c=" + customerId + "&s=U'>Unsubscribe</a>.</div></p>";
                                emailDirect.Content += unsubscribeText;
                            }

                            try
                            {
                                emailDirect.EmailSubject = smartFormHistory.Response_UserMsgSubject;
                                emailDirect.Content = smartFormHistory.Response_UserMsgBody;
                                emailDirect.Content = ReplaceCodeSnippets(group, emailObj, emailDirect.Content, xmlDataRow, user);
                                EmailDirect.Save(emailDirect); //who does this send to? should send to ed.EmailAddress.

                                log = new CommunicatorEntities.SmartFormActivityLog();
                                log.SFID = smartFormId;
                                log.CustomerID = customerId;
                                log.GroupID = groupId;
                                log.EmailID = emailId;
                                log.EmailType = "user";
                                log.EmailTo = emailAddress;
                                log.EmailFrom = from;
                                log.EmailSubject = emailDirect.EmailSubject;
                                log.SendTime = DateTime.Now;
                                log.CreatedUserID = user.UserID;
                                SmartFormActivityLog.Insert(log, user);
                                licenseCheck.UpdateUsed(customerId, "emailblock10k", 1);

                            }
                            catch (ECNException ecn)
                            {
                                string message = "";
                                foreach (ECNError e in ecn.ErrorList)
                                {
                                    message += "<br />" + e.Entity + ": " + e.ErrorMessage;
                                }
                                LogID = LogUnspecifiedException(ecn, emailDirect.Process, message);
                            }
                            catch (Exception ex)
                            {
                                string note = "Sending email from SF, sfID: " + smartFormId.ToString() + " groupID: " + groupId.ToString() + " customerID: " + customerId.ToString() + " emailID: " + emailId.ToString() + " emailAddress: " + emailAddress + " userID: " + user.UserID.ToString();
                                LogID = LogUnspecifiedException(ex, "ecn.webservice.ListManager.SendEmailFromSF", note);

                            }
                        }

                        /* Send Response Email to Admin*/

                        if (from.Length > 5 && adminEmail.Length > 5)
                        {
                            try
                            {
                                emailDirect.EmailAddress = adminEmail; //change email to admin's. Resave.
                                emailDirect.EmailSubject = smartFormHistory.Response_AdminMsgSubject;
                                emailDirect.Content = smartFormHistory.Response_AdminMsgBody;
                                emailDirect.Content = ReplaceCodeSnippets(group, emailObj, emailDirect.Content, xmlDataRow, user);
                                EmailDirect.Save(emailDirect);

                                log = new CommunicatorEntities.SmartFormActivityLog();
                                log.SFID = smartFormId;
                                log.CustomerID = customerId;
                                log.GroupID = groupId;
                                log.EmailID = emailId;
                                log.EmailType = "admin";
                                log.EmailTo = emailDirect.EmailAddress;
                                log.EmailFrom = from;
                                log.EmailSubject = emailDirect.EmailSubject;
                                log.SendTime = DateTime.Now;
                                log.CreatedUserID = user.UserID;
                                SmartFormActivityLog.Insert(log, user);
                                licenseCheck.UpdateUsed(customerId, "emailblock10k", 1);
                            }
                            catch (ECNException ecn)
                            {
                                string message = "";
                                foreach (ECNError e in ecn.ErrorList)
                                {
                                    message += "<br />" + e.Entity + ": " + e.ErrorMessage;
                                }
                                LogID = LogUnspecifiedException(ecn, emailDirect.Process, message);
                            }
                            catch (Exception ex)
                            {
                                string note = "Sending response email to admin, sfID: " + smartFormId + " groupID: " + groupId + " customerID: " + customerId + " emailID: " + emailId + " emailAddress: " + emailAddress + " userID: " + user.UserID;
                                LogID = LogUnspecifiedException(ex, "ecn.webservice.ListManager.SendEmailFromSF", note);
                            }
                        }
                    }
                }
            }
        }

        private string ReplaceCodeSnippets(
            CommunicatorEntities.Group group,
            CommunicatorEntities.Email emailObj,
            string emailbody,
            DataRow ecnPostParamsDataRow,
            User user)
        {
            emailbody = StringFunctions.Replace(emailbody, "%%GroupID%%", group.GroupID.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%GroupName%%", group.GroupName);
            emailbody = StringFunctions.Replace(emailbody, "%%EmailID%%", emailObj.EmailID.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%EmailAddress%%", emailObj.EmailAddress);
            emailbody = StringFunctions.Replace(emailbody, "%%Title%%", emailObj.Title);
            emailbody = StringFunctions.Replace(emailbody, "%%FirstName%%", emailObj.FirstName);
            emailbody = StringFunctions.Replace(emailbody, "%%LastName%%", emailObj.LastName);
            emailbody = StringFunctions.Replace(emailbody, "%%FullName%%", emailObj.FullName);
            emailbody = StringFunctions.Replace(emailbody, "%%Company%%", emailObj.Company);
            emailbody = StringFunctions.Replace(emailbody, "%%Occupation%%", emailObj.Occupation);
            emailbody = StringFunctions.Replace(emailbody, "%%Address%%", emailObj.Address);
            emailbody = StringFunctions.Replace(emailbody, "%%Address2%%", emailObj.Address2);
            emailbody = StringFunctions.Replace(emailbody, "%%City%%", emailObj.City);
            emailbody = StringFunctions.Replace(emailbody, "%%State%%", emailObj.State);
            emailbody = StringFunctions.Replace(emailbody, "%%Zip%%", emailObj.Zip);
            emailbody = StringFunctions.Replace(emailbody, "%%Country%%", emailObj.Country);
            emailbody = StringFunctions.Replace(emailbody, "%%Voice%%", emailObj.Voice);
            emailbody = StringFunctions.Replace(emailbody, "%%Mobile%%", emailObj.Mobile);
            emailbody = StringFunctions.Replace(emailbody, "%%Fax%%", emailObj.Fax);
            emailbody = StringFunctions.Replace(emailbody, "%%Website%%", emailObj.Website);
            emailbody = StringFunctions.Replace(emailbody, "%%Age%%", emailObj.Age);
            emailbody = StringFunctions.Replace(emailbody, "%%Income%%", emailObj.Income);
            emailbody = StringFunctions.Replace(emailbody, "%%Gender%%", emailObj.Gender);
            emailbody = StringFunctions.Replace(emailbody, "%%Notes%%", emailObj.Notes);
            emailbody = StringFunctions.Replace(emailbody, "%%BirthDate%%", emailObj.Birthdate.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%User1%%", emailObj.User1);
            emailbody = StringFunctions.Replace(emailbody, "%%User2%%", emailObj.User2);
            emailbody = StringFunctions.Replace(emailbody, "%%User3%%", emailObj.User3);
            emailbody = StringFunctions.Replace(emailbody, "%%User4%%", emailObj.User4);
            emailbody = StringFunctions.Replace(emailbody, "%%User5%%", emailObj.User5);
            emailbody = StringFunctions.Replace(emailbody, "%%User6%%", emailObj.User6);
            emailbody = StringFunctions.Replace(emailbody, "%%UserEvent1%%", emailObj.UserEvent1);
            emailbody = StringFunctions.Replace(emailbody, "%%UserEvent1Date%%", emailObj.UserEvent1Date.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%UserEvent2%%", emailObj.UserEvent2);
            emailbody = StringFunctions.Replace(emailbody, "%%UserEvent2Date%%", emailObj.UserEvent2Date.ToString());

            var udfDataArrayList = new ArrayList();
            var groupDataFields = GroupDataFields.GetByGroupID(group.GroupID, user);

            if (groupDataFields.Count > 0)
            {
                foreach (var groupDataField in groupDataFields)
                {
                    var fieldValue = "user_" + groupDataField.ShortName;
                    try
                    {
                        var UDFData = Convert.ToString(ecnPostParamsDataRow[fieldValue]);
                        udfDataArrayList.Add(UDFData);
                        emailbody = StringFunctions.Replace(emailbody, "%%" + fieldValue + "%%", UDFData);
                    }
                    catch
                    {
                        emailbody = StringFunctions.Replace(emailbody, "%%" + fieldValue + "%%", "");
                    }
                }
            }

            return emailbody;
        }
    }
}