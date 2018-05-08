using System;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Services.Description;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ecn.common.classes;
using ecn.webservice.classes;
using ecn.webservice.Facades;
using ecn.webservice.Facades.Params;
using ECN_Framework_Common.Objects;
using StringFunctions = ECN_Framework_Common.Functions.StringFunctions;

namespace ecn.webservice
{
    [WebService(
         Namespace = "http://webservices.ecn5.com/",
         Description = "The ECN Application Programming Interface (API) is a web service that allows you to control your ECN account programatically via an HTTP POST, an HTTP GET, or an XML-based SOAP call. The following web service methods allow access to managing your LISTS in ECN. The supported methods are shown below. <u>IMPORTANT NOTE:</u> All methods need ECN ACCESS KEY to work properly.")
    ]
    public class ListManager : WebServiceManagerBase
    {

        public string connStr { get { return ConfigurationSettings.AppSettings["connString"]; } }
        public string accountsDB { get { return ConfigurationSettings.AppSettings["accountsdb"]; } }
        public string commDB { get { return ConfigurationSettings.AppSettings["communicatordb"]; } }

        public string _emailProfilesXMLString = "";
        public string emailProfilesXMLString { set { _emailProfilesXMLString = value; } get { return _emailProfilesXMLString; } }

        public string _accessKey = "";
        public string accessKey { set { _accessKey = value; } get { return _accessKey; } }
        public int _baseChannelID = 0;
        public int baseChannelID { set { _baseChannelID = value; } get { return _baseChannelID; } }
        public int _customerID = 0;
        public int customerID { set { _customerID = value; } get { return _customerID; } }
        public int _userID = 0;
        public int userID { set { _userID = value; } get { return _userID; } }
        public string _customerName = "";
        public string customerName { set { _customerName = value; } get { return _customerName; } }

        private static ArrayList emailsColumnHeadings;

        private IListFacade _listFacade;

        public IListFacade ListFacade
        {
            get
            {
                if (_listFacade == null)
                {
                    _listFacade = new ListFacade();
                }
                return _listFacade;
            }
            set
            {
                _listFacade = value;
            }
        }

        public ListManager()
        {
            InitializeComponent();
        }

        public ListManager(IWebMethodExecutionWrapper executionWrapper)
            : base(executionWrapper)
        {
        }

        private int? LogID = null;
        private ECN_Framework_Entities.Communicator.APILogging Log = null;
        private string ServiceName = "ecn.webservice.ListManager.";
        private string MethodName = string.Empty;

        #region Get List Folders - GetFolders()

        [WebMethod(Description = "Get Group Folders from ECN. ")]
        public string GetFolders(string ecnAccessKey)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.GetFoldersMethodName),
                Consts.GetFoldersMethodName,
                ecnAccessKey,
                Consts.EmptyRootLogInput);

            return executionWrapper.Execute(ListFacade.GetFolders);
        }

        #endregion

        #region Get Email Profiles By Email Address - GetListEmailProfilesByEmailAddress()

        [WebMethod(Description = "Get Email Profiles from ECN. ")]
        public string GetListEmailProfilesByEmailAddress(string ecnAccessKey, int listID, string emailAddress)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.GetListEmailProfilesByEmailAddressMethodName),
                Consts.GetListEmailProfilesByEmailAddressMethodName,
                ecnAccessKey,
                string.Format(Consts.GetListEmailProfilesByEmailAddressLogInput, listID, emailAddress));

            var parameters = new GetListEmailProfilesParams
            {
                ListId = listID,
                EmailAddress = emailAddress
            };

            return executionWrapper.Execute(ListFacade.GetListEmailProfilesByEmailAddress, parameters);
        }

        #endregion

        #region Get UDFs - GetCustomFields()

        [WebMethod(Description = "Get Group Custom Fields from ECN. ")]
        public string GetCustomFields(string ecnAccessKey, int listID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.GetCustomFieldsMethodName),
                Consts.GetCustomFieldsMethodName,
                ecnAccessKey,
                string.Format(Consts.ListIdLogInput, listID));

            return executionWrapper.Execute(ListFacade.GetCustomFields, listID);
        }

        #endregion

        #region Get Filters - GetFilters()

        [WebMethod(Description = "Get Filters from ECN. ")]
        public string GetFilters(string ecnAccessKey, int listID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.GetFiltersMethodName),
                Consts.GetFiltersMethodName,
                ecnAccessKey,
                string.Format(Consts.ListIdLogInput, listID));

            return executionWrapper.Execute(ListFacade.GetFilters, listID);
        }

        #endregion

        #region ADD NEW List - AddList()
        [WebMethod(
             Description = "Add a new List in ECN. ", MessageName = "AddList")
        ]
        public string AddList(string ecnAccessKey, string listName, string listDescription)
        {
            return AddListMain(ecnAccessKey, listName, listDescription, null);
        }
        #endregion

        #region ADD NEW List To Folder - AddList()
        [WebMethod(
             Description = "Add a new List to a Folder in ECN. ", MessageName = "AddListToFolder")
        ]
        public string AddList(string ecnAccessKey, string listName, string listDescription, int FolderID)
        {
            return AddListMain(ecnAccessKey, listName, listDescription, FolderID);
        }
        #endregion

        #region ADD NEW Folder - AddFolder()
        [WebMethod(
             Description = "Add a new Folder in ECN. ", MessageName = "AddFolder")
        ]
        public string AddFolder(string ecnAccessKey, string folderName, string folderDescription)
        {
            return AddFolderMain(ecnAccessKey, folderName, folderDescription, null);
        }
        #endregion

        #region ADD NEW Folder To Parent - AddFolder()
        [WebMethod(
             Description = "Add a new Folder to a Parent Folder in ECN. ", MessageName = "AddFolderToParent")
        ]
        public string AddFolder(string ecnAccessKey, string folderName, string folderDescription, int parentFolderID)
        {
            return AddFolderMain(ecnAccessKey, folderName, folderDescription, parentFolderID);
        }
        #endregion

        #region Get Lists - GetLists()

        [WebMethod(Description = "Get Email Lists from ECN. ")]
        public string GetLists(string ecnAccessKey)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.GetListsMethodName),
                Consts.GetListsMethodName,
                ecnAccessKey,
                Consts.EmptyRootLogInput);

            return executionWrapper.Execute(ListFacade.GetLists);
        }

        #endregion

        #region Get List By Name - GetListByName()

        [WebMethod(Description = "Get Email List from ECN.")]
        public string GetListByName(string ecnAccessKey, string Name)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.GetListByNameMethodName),
                Consts.GetListByNameMethodName,
                ecnAccessKey,
                string.Format(Consts.GetListByNameLogInput, Name));

            return executionWrapper.Execute(ListFacade.GetListByName, Name);
        }

        #endregion

        #region Get Lists by Folder ID - GetListsByFolderID()

        [WebMethod(Description = "Get Lists from ECN. ")]
        public string GetListsByFolderID(string ecnAccessKey, int FolderID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.GetListsByFolderIdMethodName),
                Consts.GetListsByFolderIdMethodName,
                ecnAccessKey,
                string.Format(Consts.FolderIdLogInput, FolderID));

            return executionWrapper.Execute(ListFacade.GetListsByFolderId, FolderID);
        }

        #endregion

        #region Get Subscriber Count By Group ID - GetSubscriberCount()

        [WebMethod(Description = "Get the subscriber count from a List from ECN.")]
        public string GetSubscriberCount(string ecnAccessKey, int GroupID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.GetSubscriberCountMethodName),
                Consts.GetSubscriberCountMethodName,
                ecnAccessKey,
                string.Format(Consts.GetSubscriberCountLogInput, GroupID));

            return executionWrapper.Execute(ListFacade.GetListsByFolderId, GroupID);
        }

        #endregion

        #region Delete Folder - DeleteFolder()

        [WebMethod(Description = "Delete a List Folder from ECN. ")]
        public string DeleteFolder(string ecnAccessKey, int FolderID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.DeleteFolderMethodName),
                Consts.DeleteFolderMethodName,
                ecnAccessKey,
                string.Format(Consts.FolderIdLogInput, FolderID));

            return executionWrapper.Execute(ListFacade.DeleteFolder, FolderID);
        }

        #endregion

        #region DeleteList - DeleteList()

        [WebMethod(Description = "Delete a List from ECN. ")]
        public string DeleteList(string ecnAccessKey, int ListID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.DeleteListMethodName),
                Consts.DeleteListMethodName,
                ecnAccessKey,
                string.Format(Consts.ListIdLogInput, ListID));

            return executionWrapper.Execute(ListFacade.DeleteList, ListID);
        }

        #endregion

        #region Delete Subscriber - DeleteSubscriber()

        [WebMethod(Description = "Delete a Subscriber from a List in ECN. ")]
        public string DeleteSubscriber(string ecnAccessKey, int ListID, string EmailAddress)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.DeleteSubscriberMethodName),
                Consts.DeleteSubscriberMethodName,
                ecnAccessKey,
                string.Format(Consts.DeleteSubscriberLogInput, ListID, EmailAddress));

            var parameters = new DeleteSubscriberParams
            {
                ListId = ListID,
                EmailAddress = EmailAddress
            };

            return executionWrapper.Execute(ListFacade.DeleteSubscriber, parameters);
        }

        #endregion

        #region Delete UDF - DeleteCustomField()

        [WebMethod(Description = "Delete a Custom Field from a List in ECN. ")]
        public string DeleteCustomField(string ecnAccessKey, int ListID, int UDFID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.DeleteCustomFieldMethodName),
                Consts.DeleteCustomFieldMethodName,
                ecnAccessKey,
                string.Format(Consts.DeleteCustomFieldLogInput, ListID, UDFID));

            var parameters = new DeleteCustomFieldParams
            {
                ListId = ListID,
                UdfId = UDFID
            };

            return executionWrapper.Execute(ListFacade.DeleteCustomField, parameters);
        }

        #endregion

        #region Unsubscribe Subscriber - UnsubscribeSubscriber()

        [WebMethod(Description = "Unsubscribe a Subscriber in a List in ECN")]
        public string UnsubscribeSubscriber(string ecnAccessKey, int listID, string XMLEmails)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.UnsubscribeSubscriberMethodName),
                Consts.UnsubscribeSubscriberMethodName,
                ecnAccessKey,
                string.Format(Consts.UnsubscribeSubscriberLogInput, listID, XMLEmails));

            var parameters = new UnsubscribeSubscriberParams
            {
                ListId = listID,
                XmlEmails = XMLEmails
            };

            return executionWrapper.Execute(ListFacade.UnsubscribeSubscriber, parameters);
        }
        
        #endregion

        #region UPDATE List - UpdateList()
        [WebMethod(
             Description = "Update a List in ECN. ", MessageName = "UpdateList")
        ]
        public string UpdateList(string ecnAccessKey, int ListID, string NewListName, string NewListDescription)
        {
            return UpdateListMain(ecnAccessKey, ListID, NewListName, NewListDescription, null);
        }
        #endregion

        #region UPDATE List With Folder - UpdateList()
        [WebMethod(
             Description = "Update a List with Folder in ECN. ", MessageName = "UpdateListWithFolder")
        ]
        public string UpdateList(string ecnAccessKey, int ListID, string NewListName, string NewListDescription, int NewFolderID)
        {
            return UpdateListMain(ecnAccessKey, ListID, NewListName, NewListDescription, NewFolderID);
        }
        #endregion

        #region Update UDF - UpdateCustomField()

        [WebMethod(Description = "Update a Custom Field in ECN")]
        public string UpdateCustomField(string ecnAccessKey, int listID, int udfID, string customFieldName,
            string customFieldDescription, string isPublic)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.UpdateCustomFieldMethodName),
                Consts.UpdateCustomFieldMethodName,
                ecnAccessKey,
                string.Format(
                    Consts.UpdateCustomFieldLogInput,
                    listID,
                    udfID,
                    customFieldName,
                    customFieldDescription,
                    isPublic));

            var parameters = new CustomFieldParams
            {
                ListId = listID,
                UdfId = udfID,
                CustomFieldName = customFieldName,
                CustomFieldDescription = customFieldDescription,
                IsPublic = isPublic
            };

            return executionWrapper.Execute(ListFacade.UpdateCustomField, parameters);
        }

        #endregion

        #region ADD UDF TO List - addCustomField()

        [WebMethod(Description = "Add a new Custom Field to a List in ECN")]
        public string AddCustomField(string ecnAccessKey, int listID, string customFieldName, string customFieldDescription, string isPublic)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.AddCustomFieldMethodName),
                Consts.AddCustomFieldMethodName,
                ecnAccessKey,
                string.Format(
                    "<ROOT><ListID>{0}</ListID><CustomFieldName>{1}</CustomFieldName><CustomFieldDescription>{2}</CustomFieldDescription><IsPublic>{3}</IsPublic></ROOT>",
                    listID,
                    customFieldName,
                    customFieldDescription,
                    isPublic));

            var parameters = new CustomFieldParams
            {
                ListId = listID,
                CustomFieldName = customFieldName,
                CustomFieldDescription = customFieldDescription,
                IsPublic = isPublic
            };

            return executionWrapper.Execute(ListFacade.AddCustomField, parameters);
        }

        #endregion

        #region ADD EMAIL Profiles to List - AddSubscribers()
        #region Getters & Setters
        public string _importError = "";
        public string importError { set { _importError = value; } get { return _importError; } }

        public string _importResults = "";
        public string importResults { set { _importResults = value; } get { return _importResults; } }

        public Hashtable hUpdatedRecords = new Hashtable();
        
        #endregion

        [WebMethod(Description = "Import email profiles & its associated profile data to an Existing List thru XML file. <br>* XMLstring that has the profile data to be imported. NOTE: the XML should NOT have line breaks OR special characters (Sample: <a href='http://www.ecn5.com/documents/emails.xml'>XML</a>, <a href='http://www.ecn5.com/documents/emails.xsd'>Schema</a>)")]
        public string AddSubscribers(string ecnAccessKey, int listID, string subscriptionType, string formatType, string xmlString)
        {
            return AddSubscribersCommon(ecnAccessKey, listID, subscriptionType, formatType, xmlString, false, "ecn.webservice.listmanager.AddSubscribers");
        }

        [WebMethod(Description = "Import email profiles & its associated profile data to an Existing List thru XML file. <br>* XMLstring that has the profile data to be imported. NOTE: the XML should NOT have line breaks OR special characters (Sample: <a href='http://www.ecn5.com/documents/emails.xml'>XML</a>, <a href='http://www.ecn5.com/documents/emails.xsd'>Schema</a>)")]
        public string AddSubscribersGenerateUDF(string ecnAccessKey, int listID, string subscriptionType, string formatType, string xmlString, bool AutoGenerateUDFs = true)
        {
            return AddSubscribersCommon(ecnAccessKey, listID, subscriptionType, formatType, xmlString, AutoGenerateUDFs, "ecn.webservice.listmanager.AddSubscribersGenerateUDF");
        }

        private string AddSubscribersCommon(
            string ecnAccessKey,
            int listID,
            string subscriptionType,
            string formatType,
            string xmlString,
            bool AutoGenerateUDFs,
            string source = "ecn.webservice.listmanager.AddSubscribersCommon")
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.AddSubscribersMethodName),
                Consts.AddSubscribersMethodName,
                ecnAccessKey,
                string.Format(
                    Consts.AddSubscribersLogInput,
                    listID,
                    subscriptionType,
                    formatType,
                    xmlString));

            var parameters = new AddSubscribersParams
            {
                ListId = listID,
                SubscriptionType = subscriptionType,
                FormatType = formatType,
                XmlString = xmlString,
                AutoGenerateUdfs = AutoGenerateUDFs,
                Source = source
            };

            return executionWrapper.Execute(ListFacade.AddSubscribers, parameters);
        }

        [WebMethod(Description = "Import email profiles & its associated profile data to an Existing List thru XML file. <br>* XMLstring that has the profile data to be imported. NOTE: the XML should NOT have line breaks OR special characters (Sample: <a href='http://www.ecn5.com/documents/emails.xml'>XML</a>, <a href='http://www.ecn5.com/documents/emails.xsd'>Schema</a>)")]
        public string AddSubscribersWithDupes(string ecnAccessKey, int listID, string subscriptionType, string formatType, string compositeKey, bool overwriteWithNULL, string xmlString)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.AddSubscribersWithDupesMethodName),
                Consts.AddSubscribersWithDupesMethodName,
                ecnAccessKey,
                string.Format(
                    Consts.AddSubscribersWithDupesLogInput,
                    listID,
                    subscriptionType,
                    formatType,
                    compositeKey,
                    overwriteWithNULL,
                    xmlString));

            var parameters = new AddSubscribersParams
            {
                ListId = listID,
                SubscriptionType = subscriptionType,
                FormatType = formatType,
                CompositeKey = compositeKey,
                OverwriteWithNull = overwriteWithNULL,
                XmlString = xmlString,
                CreateEmailActivityLog = false,
                EscapeApostrophesInEmailAddress = false
            };

            return executionWrapper.Execute(ListFacade.AddSubscribersWithDupes, parameters);
        }

        [WebMethod(Description = "Import email profiles & its associated profile data to an Existing List thru XML file. <br>* XMLstring that has the profile data to be imported. NOTE: the XML should NOT have line breaks OR special characters (Sample: <a href='http://www.ecn5.com/documents/emails.xml'>XML</a>, <a href='http://www.ecn5.com/documents/emails.xsd'>Schema</a>)")]
        public string AddSubscribersWithDupesUsingSmartForm(string ecnAccessKey, int listID, string subscriptionType, string formatType, string compositeKey, bool overwriteWithNULL, string xmlString, int SFID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.AddSubscribersWithDupesUsingSmartFormMethodName),
                Consts.AddSubscribersWithDupesUsingSmartFormMethodName,
                ecnAccessKey,
                string.Format(
                    Consts.AddSubscribersWithDupesUsingSmartFormLogInput,
                    listID,
                    subscriptionType,
                    formatType,
                    compositeKey,
                    overwriteWithNULL,
                    xmlString,
                    SFID));

            var parameters = new AddSubscribersParams
            {
                ListId = listID,
                SubscriptionType = subscriptionType,
                FormatType = formatType,
                CompositeKey = compositeKey,
                OverwriteWithNull = overwriteWithNULL,
                XmlString = xmlString,
                SmartFormId = SFID,
                CreateEmailActivityLog = true,
                EscapeApostrophesInEmailAddress = true
            };

            return executionWrapper.Execute(ListFacade.AddSubscribersWithDupes, parameters);
        }

        [WebMethod(Description = "Import email profiles & its associated profile data to an Existing List thru XML file. <br>* XMLstring that has the profile data to be imported. NOTE: the XML should NOT have line breaks OR special characters (Sample: <a href='http://www.ecn5.com/documents/emails.xml'>XML</a>, <a href='http://www.ecn5.com/documents/emails.xsd'>Schema</a>)")]
        public string AddSubscriberUsingSmartForm(string ecnAccessKey, int listID, string subscriptionType, string formatType, string xmlString, int sfID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ListManagerServiceMethodName, Consts.AddSubscriberUsingSmartFormMethodName),
                Consts.AddSubscriberUsingSmartFormMethodName,
                ecnAccessKey,
                string.Format(
                    Consts.AddSubscriberUsingSmartFormLogInput,
                    listID,
                    subscriptionType,
                    formatType,
                    xmlString,
                    sfID));

            var parameters = new AddSubscribersParams
            {
                ListId = listID,
                SubscriptionType = subscriptionType,
                FormatType = formatType,
                XmlString = xmlString,
                SmartFormId = sfID,
            };

            return executionWrapper.Execute(ListFacade.AddSubscriberUsingSmartForm, parameters);
        }

        #endregion

        #region ADD Email Addresses to the Master Suppression List - AddToMasterSuppressionList()
        [WebMethod(
             Description = "Add Email Addresses to the Master Suppression List in ECN")
        ]
        public string AddToMasterSuppressionList(string ecnAccessKey, string xmlString)
        {
            try
            {
                MethodName = "AddToMasterSuppressionList";
                Guid localGUID;
                if (!Guid.TryParse(ecnAccessKey, out localGUID))
                    return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "INVALID ECN ACCESS KEY FORMAT");
                else
                {
                    Log = new ECN_Framework_Entities.Communicator.APILogging();
                    Log.AccessKey = ecnAccessKey;
                    Log.APIMethod = ServiceName + MethodName;
                    Log.Input = "<ROOT><XMLString><![CDATA[" + xmlString + "]]></XMLString></ROOT>";
                    Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);

                    KMPlatform.Entity.User user = new KMPlatform.BusinessLogic.User().LogIn(localGUID, true);
                    user.CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(user.DefaultClientID, false).CustomerID;
                    if (user != null)
                    {
                        ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(user.CustomerID, user);
                        if (group != null)
                        {
                            ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                            return AddSubscribers(ecnAccessKey, group.GroupID, ECN_Framework_Common.Objects.Communicator.Enums.ActionValue.S.ToString(), "html", xmlString);
                        }
                        else
                        {
                            ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                            return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "NO MASTER SUPPRESSION LIST FOUND");
                        }
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                        return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                    }
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnEX)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ECN_Framework_Common.Objects.ECNException.CreateErrorMessage(ecnEX));
            }
            catch (ECN_Framework_Common.Objects.SecurityException)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "SECURITY VIOLATION");
            }
            catch (KMPlatform.Object.UserLoginException ule)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);

                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ule.UserStatus.ToString());
            }
            catch (Exception ex)
            {
                LogID = LogUnspecifiedException(ex, ServiceName + MethodName);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, LogID);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ex.ToString());
            }
        }
        #endregion

        [WebMethod(Description = "This is used by internal KM Applications")]
        public string GetLists_Internal(string ecnAccessKey, int CustomerID)
        {
            AuthenticationHandler authHandler = new AuthenticationHandler(ecnAccessKey);
            if (authHandler.authenticateUser())
            {
                if (authHandler.isSysAdmin())
                {
                    if (ecnAccessKey.ToUpper().Equals(ConfigurationManager.AppSettings["UASMasterAccessKey"].ToString()))
                    {
                        KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ecnAccessKey, false);
                        DataTable dtGroups = ECN_Framework_BusinessLayer.Communicator.Group.GetGroupDR(CustomerID, user.UserID, user);
                        StringBuilder sbGroups = new StringBuilder();
                        sbGroups.Append("<Groups>");

                        foreach (DataRow dr in dtGroups.Rows)
                        {
                            sbGroups.Append("<Group>");
                            sbGroups.Append("<ID>" + dr["GroupID"].ToString() + "</ID>");
                            sbGroups.Append("<Name><![CDATA[" + dr["GroupName"].ToString() + "]]></Name>");
                            sbGroups.Append("</Group>");
                        }
                        sbGroups.Append("</Groups>");

                        return SendResponse.response("GetLists_Internal", SendResponse.ResponseCode.Success, 0, sbGroups.ToString());
                    }
                    else
                    {
                        return SendResponse.response("GetLists_Internal", SendResponse.ResponseCode.Fail, 0, "USER DOES NOT HAVE PERMISSIONS");
                    }
                }
                else
                {
                    return SendResponse.response("GetLists_Internal", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                }
            }
            else
            {
                return SendResponse.response("GetLists_Internal", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
            }
        }


        #region GET SUBSCRIBER STATUS

        [WebMethod(Description = "Gets the status of the subscriber", MessageName = "GetSubscriberStatus")]
        public string GetSubscriberStatus(string ecnAccessKey, string emailAddress)
        {
            try
            {
                MethodName = "GetSubscriberStatus";
                Guid localGUID;
                if (!Guid.TryParse(ecnAccessKey, out localGUID))
                    return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "INVALID ECN ACCESS KEY FORMAT");
                else
                {
                    Log = new ECN_Framework_Entities.Communicator.APILogging();
                    Log.AccessKey = ecnAccessKey;
                    Log.APIMethod = ServiceName + MethodName;
                    Log.Input = "<ROOT><EmailAddress>" + emailAddress + "</EmailAddress></ROOT>";
                    Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);

                    KMPlatform.Entity.User user = new KMPlatform.BusinessLogic.User().LogIn(localGUID, true);
                    user.CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(user.DefaultClientID, false).CustomerID;
                    if (user != null)
                    {
                        MemoryStream stream = new MemoryStream();
                        XmlTextWriter xmlWriter = new XmlTextWriter(stream, Encoding.UTF8);
                        DataTable dtemails = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetSubscriberStatus(emailAddress, user);
                        if (dtemails.Rows.Count > 0)
                        {
                            try
                            {
                                xmlWriter.WriteStartElement("XML");
                                xmlWriter.WriteElementString("EmailAddress", emailAddress);
                                xmlWriter.WriteStartElement("Groups");

                                foreach (DataRow dr in dtemails.Rows)
                                {
                                    xmlWriter.WriteStartElement("Group");
                                    xmlWriter.WriteAttributeString("ID", dr["GroupID"].ToString());
                                    xmlWriter.WriteAttributeString("Name", dr["GroupName"].ToString());
                                    xmlWriter.WriteAttributeString("SubscriptionTypeCode", dr["SubscribeTypeCode"].ToString());
                                    xmlWriter.WriteEndElement();
                                }

                                xmlWriter.WriteEndElement();
                                xmlWriter.WriteEndElement();
                                xmlWriter.Flush();
                            }
                            catch { }

                            StreamReader reader = new StreamReader(stream);
                            stream.Position = 0;
                            ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                            return SendResponse.response(MethodName, SendResponse.ResponseCode.Success, 0, reader.ReadToEnd());
                        }
                        else
                        {
                            ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                            return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "Records not found");
                        }
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                        return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                    }
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnEX)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ECN_Framework_Common.Objects.ECNException.CreateErrorMessage(ecnEX));
            }
            catch (ECN_Framework_Common.Objects.SecurityException)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "SECURITY VIOLATION");
            }
            catch (KMPlatform.Object.UserLoginException ule)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);

                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ule.UserStatus.ToString());
            }
            catch (Exception ex)
            {
                LogID = LogUnspecifiedException(ex, ServiceName + MethodName);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, LogID);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ex.ToString());
            }
        }

        #endregion


        [WebMethod(Description = "This method call will import the email profiles in the XML string and alos updates the emailaddress.<br>Parameters to this method<br>* ecn_listID - is the ID of the list to import the email profiles.<br>* ecn_emailProfilesXMLString - is XML string that has the profile data to be imported.<br>* NOTE: the XML should have <u>no line breaks</u> OR <u>special characters</u>", EnableSession = false)]
        [SoapDocumentMethod(Action = "http://webservices.ecn5.com/ListManager.asmx?op=UpdateEmailAddress",
            RequestNamespace = "ecn.webServices.communicator", RequestElementName = "ImportEmailProfilesRequestSFUpdate",
            ResponseNamespace = "ecn.webServices.communicator", ResponseElementName = "ResponseSFUpdate",
            Use = SoapBindingUse.Default)]
        public string UpdateEmailAddress(string ecn_accessKey, int ecn_listID, string ecn_emailProfilesXMLString, string oldEmailAddress, string newEmailAddress, int sfID)
        {
            try
            {
                MethodName = "UpdateEmailAddress";
                Guid localGUID;
                if (!Guid.TryParse(ecn_accessKey, out localGUID))
                    return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "INVALID ECN ACCESS KEY FORMAT");
                else
                {
                    Log = new ECN_Framework_Entities.Communicator.APILogging();
                    Log.AccessKey = ecn_accessKey;
                    Log.APIMethod = ServiceName + MethodName;
                    Log.Input = "<ROOT><ListID>" + ecn_listID.ToString() + "</ListID><XMLString><![CDATA[" + ecn_emailProfilesXMLString + "]]></XMLString><OldEmailAddress>" + oldEmailAddress + "</OldEmailAddress><NewEmailAddress>" + newEmailAddress + "</NewEmailAddress><SFID>" + sfID.ToString() + "</SFID></ROOT>";
                    Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);

                    KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ecn_accessKey, true);
                    user.CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(user.DefaultClientID, false).CustomerID;

                    if (user != null && user.CustomerID != null)
                    {
                        if (ecn_listID <= 0)
                            KM.Common.Entity.ApplicationLog.LogNonCriticalError("Unknown GroupID: " + ecn_listID, ServiceName + MethodName, Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                        //for missing groupids with sfid, go get the groupid from the smart form...fix for bad group ids from GD
                        //if (ecn_listID <= 0 && sfID > 0 && user.CustomerID > 0)
                        //{
                        //    int ecn_listID_Original = ecn_listID;
                        //    ecn_listID = ECN_Framework_BusinessLayer.Communicator.SmartFormsHistory.GetGroupID(user.CustomerID, sfID);
                        //    if (ecn_listID > 0)
                        //        KM.Common.Entity.ApplicationLog.LogNonCriticalError("Unknown GroupID: " + ecn_listID_Original, "ecn.webservice.ListManager.UpdateEmailAddress", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                        //    else
                        //        KM.Common.Entity.ApplicationLog.LogNonCriticalError("Unknown GroupID: " + ecn_listID_Original + ", found and using GroupID: " + ecn_listID + " from SFID: " + sfID, "ecn.webservice.ListManager.UpdateEmailAddress", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]));
                        //}

                        if (ECN_Framework_BusinessLayer.Communicator.Group.Exists(ecn_listID, user.CustomerID))
                        {
                            if (ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(oldEmailAddress) && ECN_Framework_BusinessLayer.Communicator.Email.IsValidEmailAddress(newEmailAddress))
                            {
                                if (ecn_emailProfilesXMLString.Length > 0)
                                {
                                    extractCoumnNamesFromEmailsTable();
                                    DataTable xmlDT = ListFacade.ExtractColumnNamesFromXmlString(ecn_emailProfilesXMLString, ecn_listID, customerID);
                                    if (!(xmlDT == null))
                                    {
                                        bool importSuccess = importDataWithUpdate(user, xmlDT, oldEmailAddress, newEmailAddress, ecn_listID, "ecn.webservice.listmanager." + MethodName);

                                        if (importSuccess)
                                        {
                                            string emailAddress = xmlDT.Rows[0]["EmailAddress"].ToString();
                                            int emailID = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetEmailIDFromWhatEmail(ecn_listID, user.CustomerID, xmlDT.Rows[0]["Emailaddress"].ToString().Replace("'", "''"), user);
                                            SendEmailFromSF(sfID, xmlDT.Rows[0], ecn_listID, user.CustomerID, emailID, emailAddress, user);
                                            ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                                            return SendResponse.response(MethodName, "0", 0, importResults);
                                        }
                                        else
                                        {
                                            ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                                            return SendResponse.response(MethodName, "1", 0, System.Net.WebUtility.HtmlEncode(importError));
                                        }
                                    }
                                    else
                                    {
                                        ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                                        return SendResponse.response(MethodName, "1", 0, "INVALID XML STRING");
                                    }
                                }
                                else
                                {
                                    ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                                    return SendResponse.response(MethodName, "1", 0, "INVALID XML STRING");
                                }
                            }
                            else
                            {
                                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                                return SendResponse.response(MethodName, "1", 0, "INVALID NEW OR OLD EMAIL ADDRESS");
                            }
                        }
                        else
                        {
                            ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                            return SendResponse.response(MethodName, "1", 0, "UNAUTHORIZED ACCESS TO LIST");
                        }
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                        return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                    }
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnEX)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ECN_Framework_Common.Objects.ECNException.CreateErrorMessage(ecnEX));
            }
            catch (ECN_Framework_Common.Objects.SecurityException)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "SECURITY VIOLATION");
            }
            catch (KMPlatform.Object.UserLoginException ule)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);

                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ule.UserStatus.ToString());
            }
            catch (Exception ex)
            {
                string note = "ecn_accessKey: " + ecn_accessKey + " ecn_accessKey: " + ecn_accessKey.ToString() + " ecn_emailProfilesXMLString: " + ecn_emailProfilesXMLString + " oldEmailAddress: " + oldEmailAddress + " newEmailAddress: " + newEmailAddress + " sfID: " + sfID.ToString();
                LogID = LogUnspecifiedException(ex, ServiceName + MethodName, note);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, LogID);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ex.ToString());
            }
        }

        
        private int LogUnspecifiedException(Exception ex, string sourceMethod, string note = "")
        {
            return KM.Common.Entity.ApplicationLog.LogCriticalError(ex, sourceMethod, Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
            //KM.Common.Entity.ApplicationLog log = new KM.Common.Entity.ApplicationLog();
            //log.ApplicationID = Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]);
            //log.SeverityID = 1;
            //log.Exception = KM.Common.Entity.ApplicationLog.FormatException(ex);
            //log.NotificationSent = false;
            //log.SourceMethod = sourceMethod;
            //KM.Common.Entity.ApplicationLog.Save(ref log);

        }

        private string AddListMain(string ecnAccessKey, string listName, string listDescription, int? FolderID)
        {
            try
            {
                MethodName = "AddListMain";
                Guid localGUID;
                if (!Guid.TryParse(ecnAccessKey, out localGUID))
                    return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "INVALID ECN ACCESS KEY FORMAT");
                else
                {
                    Log = new ECN_Framework_Entities.Communicator.APILogging();
                    Log.AccessKey = ecnAccessKey;
                    Log.APIMethod = ServiceName + MethodName;
                    Log.Input = "<ROOT><ListName>" + listName + "</ListName><ListDescription>" + listDescription + "</ListDescription><FolderID>" + (FolderID.HasValue ? FolderID.Value.ToString() : string.Empty) + "</FolderID></ROOT>";
                    Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);

                    KMPlatform.Entity.User user = new KMPlatform.BusinessLogic.User().LogIn(localGUID, true);
                    user.CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(user.DefaultClientID, false).CustomerID;

                    if (user != null)
                    {
                        if (FolderID == null || FolderID.Value == 0 || ECN_Framework_BusinessLayer.Communicator.Folder.Exists(FolderID.Value, user.CustomerID))
                        {
                            if (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(-1, listName, FolderID == null ? 0 : FolderID.Value, user.CustomerID))
                            {
                                ECN_Framework_Entities.Communicator.Group group = new ECN_Framework_Entities.Communicator.Group();
                                group.CreatedUserID = user.UserID;
                                group.OwnerTypeCode = "customer";
                                group.PublicFolder = 0;
                                group.GroupName = listName;
                                group.GroupDescription = listDescription;
                                group.CustomerID = user.CustomerID;
                                group.AllowUDFHistory = "N";
                                group.IsSeedList = false;
                                group.FolderID = FolderID == null ? 0 : FolderID.Value;
                                ECN_Framework_BusinessLayer.Communicator.Group.Save(group, user);
                                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                                return SendResponse.response(MethodName, SendResponse.ResponseCode.Success, group.GroupID, "LIST CREATED");
                            }
                            else
                            {
                                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, listName + " ALREADY EXISTS FOR CUSTOMER");
                            }
                        }
                        else
                        {
                            ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                            return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "FOLDER DOES NOT EXIST FOR CUSTOMER");
                        }
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                        return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                    }
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnEX)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ECN_Framework_Common.Objects.ECNException.CreateErrorMessage(ecnEX));
            }
            catch (ECN_Framework_Common.Objects.SecurityException)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "SECURITY VIOLATION");
            }
            catch (KMPlatform.Object.UserLoginException ule)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);

                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ule.UserStatus.ToString());
            }
            catch (Exception ex)
            {
                LogID = LogUnspecifiedException(ex, ServiceName + MethodName);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, LogID);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ex.ToString());
            }
        }

        private string AddFolderMain(string ecnAccessKey, string folderName, string folderDescription, int? parentFolderID)
        {
            try
            {
                MethodName = "AddFolderMain";
                Guid localGUID;
                if (!Guid.TryParse(ecnAccessKey, out localGUID))
                    return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "INVALID ECN ACCESS KEY FORMAT");
                else
                {
                    Log = new ECN_Framework_Entities.Communicator.APILogging();
                    Log.AccessKey = ecnAccessKey;
                    Log.APIMethod = ServiceName + MethodName;
                    Log.Input = "<ROOT><FolderName>" + folderName + "</FolderName><FolderDescription>" + folderDescription + "</FolderDescription><ParentFolderID>" + (parentFolderID.HasValue ? parentFolderID.Value.ToString() : string.Empty) + "</ParentFolderID></ROOT>";
                    Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);

                    KMPlatform.Entity.User user = new KMPlatform.BusinessLogic.User().LogIn(localGUID, true);
                    user.CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(user.DefaultClientID, false).CustomerID;
                    if (user != null)
                    {
                        if (parentFolderID == null || parentFolderID.Value == 0 || ECN_Framework_BusinessLayer.Communicator.Folder.Exists(parentFolderID.Value, user.CustomerID))
                        {
                            if (!ECN_Framework_BusinessLayer.Communicator.Folder.Exists(-1, folderName, parentFolderID == null ? 0 : parentFolderID.Value, user.CustomerID, "GRP"))
                            {
                                ECN_Framework_Entities.Communicator.Folder folder = new ECN_Framework_Entities.Communicator.Folder();
                                folder.FolderName = folderName;
                                folder.FolderDescription = folderDescription;
                                folder.CustomerID = user.CustomerID;
                                folder.CreatedUserID = user.UserID;
                                folder.FolderType = ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString();
                                folder.IsSystem = false;
                                folder.ParentID = parentFolderID == null ? 0 : parentFolderID.Value;
                                ECN_Framework_BusinessLayer.Communicator.Folder.Save(folder, user);
                                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                                return SendResponse.response("AddFolder", SendResponse.ResponseCode.Success, Convert.ToInt32(folder.FolderID), "FOLDER CREATED");
                            }
                            else
                            {
                                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                                return SendResponse.response("AddFolder", SendResponse.ResponseCode.Fail, 0, folderName + " ALREADY EXISTS IN PARENT FOR CUSTOMER");
                            }
                        }
                        else
                        {
                            ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                            return SendResponse.response("AddFolder", SendResponse.ResponseCode.Fail, 0, "PARENT FOLDER DOES NOT EXIST FOR CUSTOMER");
                        }
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                        return SendResponse.response("AddFolder", SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                    }
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnEX)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response("AddMessage", SendResponse.ResponseCode.Fail, 0, ECN_Framework_Common.Objects.ECNException.CreateErrorMessage(ecnEX));
            }
            catch (ECN_Framework_Common.Objects.SecurityException)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response("AddFolder", SendResponse.ResponseCode.Fail, 0, "SECURITY VIOLATION");
            }
            catch (KMPlatform.Object.UserLoginException ule)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);

                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ule.UserStatus.ToString());
            }
            catch (Exception ex)
            {
                LogID = LogUnspecifiedException(ex, ServiceName + MethodName);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, LogID);
                return SendResponse.response("AddFolder", SendResponse.ResponseCode.Fail, 0, ex.ToString());
            }
        }

        

        private string UpdateListMain(string ecnAccessKey, int ListID, string NewListName, string NewListDescription, int? NewFolderID)
        {
            try
            {
                MethodName = "UpdateListMain";
                Guid localGUID;
                if (!Guid.TryParse(ecnAccessKey, out localGUID))
                    return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "INVALID ECN ACCESS KEY FORMAT");
                else
                {
                    Log = new ECN_Framework_Entities.Communicator.APILogging();
                    Log.AccessKey = ecnAccessKey;
                    Log.APIMethod = ServiceName + MethodName;
                    Log.Input = "<ROOT><ListID>" + ListID.ToString() + "</ListID><NewListName>" + NewListName + "</NewListName><NewListDescription>" + NewListDescription + "</NewListDescription><NewFolderID>" + (NewFolderID.HasValue ? NewFolderID.Value.ToString() : string.Empty) + "</NewFolderID></ROOT>";
                    Log.APILogID = ECN_Framework_BusinessLayer.Communicator.APILogging.Insert(Log);

                    KMPlatform.Entity.User user = new KMPlatform.BusinessLogic.User().LogIn(localGUID, true);
                    user.CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(user.DefaultClientID, false).CustomerID;
                    if (user != null)
                    {
                        ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(ListID, user);
                        if (group != null)
                        {
                            group.GroupName = NewListName;
                            group.GroupDescription = NewListDescription;
                            group.UpdatedUserID = user.UserID;
                            if (NewFolderID != null)
                                group.FolderID = NewFolderID;
                            if (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(ListID, group.GroupName, group.FolderID == null ? 0 : group.FolderID.Value, user.CustomerID))
                            {
                                ECN_Framework_BusinessLayer.Communicator.Group.Save(group, user);
                                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                                return SendResponse.response(MethodName, SendResponse.ResponseCode.Success, group.GroupID, "LIST UPDATED");
                            }
                            else
                            {
                                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, NewListName + " ALREADY EXISTS FOR CUSTOMER");
                            }
                        }
                        else
                        {
                            ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                            return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "LIST DOESN'T EXIST FOR CUSTOMER");
                        }
                    }
                    else
                    {
                        ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                        return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "LOGIN AUTHENTICATION FAILED");
                    }
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnEX)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ECN_Framework_Common.Objects.ECNException.CreateErrorMessage(ecnEX));
            }
            catch (ECN_Framework_Common.Objects.SecurityException)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, "SECURITY VIOLATION");
            }
            catch (KMPlatform.Object.UserLoginException ule)
            {
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, null);

                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ule.UserStatus.ToString());
            }
            catch (Exception ex)
            {
                LogID = LogUnspecifiedException(ex, ServiceName + MethodName);
                ECN_Framework_BusinessLayer.Communicator.APILogging.UpdateLog(Log.APILogID, LogID);
                return SendResponse.response(MethodName, SendResponse.ResponseCode.Fail, 0, ex.ToString());
            }
        }

        private bool importDataWithUpdate(KMPlatform.Entity.User user, DataTable dtFile, string oldEmailAddres, string newEmailAddress, int ecn_listID, string source = "ecn.webservice.listmanager.importDataWithUpdate")
        {
            StringBuilder xmlUDF = new StringBuilder("");
            StringBuilder xmlProfile = new StringBuilder("");

            DateTime startDateTime = DateTime.Now;
            try
            {
                Hashtable hGDFFields = getUDFsForList(ecn_listID, user);

                bool bRowCreated = false;
                for (int cnt = 0; cnt < dtFile.Rows.Count; cnt++)
                {

                    DataRow drFile = dtFile.Rows[cnt];
                    bRowCreated = false;
                    xmlProfile.Append("<Emails>");

                    foreach (DataColumn dcFile in dtFile.Columns)
                    {
                        if (dcFile.ColumnName.ToLower().IndexOf("user_") == -1)
                        {
                            xmlProfile.Append("<" + dcFile.ColumnName.ToLower() + ">" + cleanXMLString(drFile[dcFile.ColumnName].ToString()) + "</" + dcFile.ColumnName.ToLower() + ">");
                        }

                        if (hGDFFields.Count > 0)
                        {
                            if (dcFile.ColumnName.ToLower().IndexOf("user_") > -1)
                            {
                                if (!bRowCreated)
                                {
                                    xmlUDF.Append("<row>");
                                    xmlUDF.Append("<ea>" + cleanXMLString(drFile["emailaddress"].ToString()) + "</ea>");
                                    bRowCreated = true;
                                }

                                try
                                {
                                    xmlUDF.Append("<udf id=\"" + hGDFFields[dcFile.ColumnName.ToLower()].ToString() + "\">");
                                    xmlUDF.Append("<v><![CDATA[" + cleanXMLString(drFile[dcFile.ColumnName.ToLower()].ToString()).Replace("&amp;", "&") + "]]></v>");
                                    xmlUDF.Append("</udf>");
                                }
                                catch
                                {
                                    importError = dcFile.ColumnName.ToString() + " is not a valid UDF name";
                                    return false;
                                }
                            }
                        }
                    }

                    xmlProfile.Append("</Emails>");

                    if (bRowCreated)
                        xmlUDF.Append("</row>");

                    if ((cnt != 0) && (cnt % 10000 == 0) || (cnt == dtFile.Rows.Count - 1))
                    {
                        ECN_Framework_BusinessLayer.Communicator.Email.UpdateEmailAddress(ecn_listID, user.CustomerID, newEmailAddress, oldEmailAddres, user, source);
                        DataTable dtRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(user, user.CustomerID, ecn_listID, "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>", "html", "S", false, "", source);


                        if (dtRecords.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtRecords.Rows)
                            {
                                if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                                    hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(), Convert.ToInt32(dr["Counts"]));
                                else
                                {
                                    int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                                    hUpdatedRecords[dr["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr["Counts"]);
                                }
                            }
                        }

                        xmlProfile = new StringBuilder("");
                        xmlUDF = new StringBuilder("");
                    }
                }
                hGDFFields.Clear();

                if (hUpdatedRecords.Count > 0)
                {
                    importResults = "";
                    foreach (DictionaryEntry de in hUpdatedRecords)
                    {
                        if (de.Key.ToString() == "T")
                            importResults += "<TotalRecords>" + de.Value.ToString() + "</TotalRecords>";
                        else if (de.Key.ToString() == "I")
                            importResults += "<New>" + de.Value.ToString() + "</New>";
                        else if (de.Key.ToString() == "U")
                            importResults += "<Changed>" + de.Value.ToString() + "</Changed>";
                        else if (de.Key.ToString() == "D")
                            importResults += "<Duplicates>" + de.Value.ToString() + "</Duplicates>";
                        else if (de.Key.ToString() == "S")
                            importResults += "<Skipped>" + de.Value.ToString() + "</Skipped>";
                        else if (de.Key.ToString() == "M")
                        {
                            importResults += "<MSSkipped>" + de.Value.ToString() + "</MSSkipped>";
                        }
                    }

                    TimeSpan duration = DateTime.Now - startDateTime;
                    importResults += "<ImportTime>" + duration.Hours + ":" + duration.Minutes + ":" + duration.Seconds + "</ImportTime>";
                }

                return true;
            }
            catch (Exception ex)
            {
                importError = ex.ToString();
                return false;
            }
        }

        

        private string ReplaceCodeSnippets(ECN_Framework_Entities.Communicator.Group group, ECN_Framework_Entities.Communicator.Email emailObj, string emailbody, DataRow ECNPostParams, KMPlatform.Entity.User user)
        {
            emailbody = StringFunctions.Replace(emailbody, "%%GroupID%%", group.GroupID.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%GroupName%%", group.GroupName.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%EmailID%%", emailObj.EmailID.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%EmailAddress%%", emailObj.EmailAddress.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Title%%", emailObj.Title.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%FirstName%%", emailObj.FirstName.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%LastName%%", emailObj.LastName.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%FullName%%", emailObj.FullName.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Company%%", emailObj.Company.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Occupation%%", emailObj.Occupation.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Address%%", emailObj.Address.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Address2%%", emailObj.Address2.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%City%%", emailObj.City.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%State%%", emailObj.State.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Zip%%", emailObj.Zip.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Country%%", emailObj.Country.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Voice%%", emailObj.Voice.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Mobile%%", emailObj.Mobile.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Fax%%", emailObj.Fax.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Website%%", emailObj.Website.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Age%%", emailObj.Age.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Income%%", emailObj.Income.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Gender%%", emailObj.Gender.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%Notes%%", emailObj.Notes.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%BirthDate%%", emailObj.Birthdate.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%User1%%", emailObj.User1.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%User2%%", emailObj.User2.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%User3%%", emailObj.User3.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%User4%%", emailObj.User4.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%User5%%", emailObj.User5.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%User6%%", emailObj.User6.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%UserEvent1%%", emailObj.UserEvent1.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%UserEvent1Date%%", emailObj.UserEvent1Date.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%UserEvent2%%", emailObj.UserEvent2.ToString());
            emailbody = StringFunctions.Replace(emailbody, "%%UserEvent2Date%%", emailObj.UserEvent2Date.ToString());

            //UDF Data 
            //SortedList UDFHash = group.UDFHash;
            ArrayList _keyArrayList = new ArrayList();
            ArrayList _UDFData = new ArrayList();

            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(group.GroupID, user);
            if (gdfList.Count > 0)
            {
                foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in gdfList)
                {
                    string UDFData = "";
                    string _value = "user_" + gdf.ShortName;
                    string _key = gdf.GroupDataFieldsID.ToString();
                    try
                    {
                        UDFData = Convert.ToString(ECNPostParams[_value]);
                        _keyArrayList.Add(_key);
                        _UDFData.Add(UDFData);
                        emailbody = StringFunctions.Replace(emailbody, "%%" + _value + "%%", UDFData);
                    }
                    catch
                    {
                        emailbody = StringFunctions.Replace(emailbody, "%%" + _value + "%%", "");
                    }
                }
            }

            //End UDF Data

            return emailbody;
        }

        private void SendEmailFromSF(int sfID, DataRow xmlDR, int groupID, int customerID, int emailID, string emailAddress, KMPlatform.Entity.User user)
        {
            if (sfID > 0)
            {
                ecn.common.classes.LicenseCheck lc = new ecn.common.classes.LicenseCheck();
                ECN_Framework_Entities.Communicator.SmartFormActivityLog log;

                if (emailID > 0)
                {
                    ECN_Framework_Entities.Communicator.Email emailObj = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(emailID, user);
                    emailObj.EmailAddress = emailAddress;
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(groupID, user);
                    //ecn.communicator.classes.EmailFunctions emailSend = new ecn.communicator.classes.EmailFunctions();
                    ECN_Framework_Entities.Communicator.EmailDirect ed = new ECN_Framework_Entities.Communicator.EmailDirect();

                    ECN_Framework_Entities.Communicator.SmartFormsHistory sfHistory = ECN_Framework_BusinessLayer.Communicator.SmartFormsHistory.GetBySmartFormID(sfID, groupID, user);

                    ed.CustomerID = customerID;
                    ed.EmailAddress = emailObj.EmailAddress;
                    ed.EmailSubject = "Knowledge Marketing Gateway - Reset Password";

                    ed.FromName = "Webservice";
                    ed.Process = "Webservice - ListManager.SendEmailFromSF";
                    ed.Source = "Webservice";
                    ed.ReplyEmailAddress = "info@knowledgemarketing.com";
                    ed.SendTime = DateTime.Now;
                    ed.CreatedUserID = user.UserID;
                    ed.Content = sfHistory.Response_UserMsgBody;


                    if (sfHistory != null)
                    {
                        string from = sfHistory.Response_FromEmail;
                        string adminEmail = sfHistory.Response_AdminEmail;


                        /* Send Response Email to user*/

                        if (from.Length > 5 && emailAddress.Length > 5)
                        {
                            if (ed.Content.ToLower().IndexOf("%%unsubscribelink%%") > 0)
                            {
                                ed.Content = StringFunctions.Replace(ed.Content, "http://%%unsubscribelink%%", "%%unsubscribelink%%");
                                ed.Content = StringFunctions.Replace(ed.Content, "%%unsubscribelink%%", ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/unsubscribe.aspx?e=" + emailAddress + "&g=" + groupID + "&b=0&c=" + customerID + "&s=U");
                            }
                            else
                            {
                                //Add Unsubscribe Link at the bottom of the email as per CAN-SPAM & USI requested it to be done. 
                                string unsubscribeText = "<p style=\"padding-TOP:5px\"><div style=\"font-size:8.0pt;font-family:'Arial,sans-serif'; color:#666666\"><IMG style=\"POSITION:relative; TOP:5px\" src='" + ConfigurationManager.AppSettings["Image_DomainPath"] + "/images/Sure-Unsubscribe.gif'/>&nbsp;If you feel you have received this message in error, or wish to be removed, please <a href='" + ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/unsubscribe.aspx?e=" + emailAddress + "&g=" + groupID + "&b=0&c=" + customerID + "&s=U'>Unsubscribe</a>.</div></p>";
                                ed.Content += unsubscribeText;
                            }

                            try
                            {
                                ed.EmailSubject = sfHistory.Response_UserMsgSubject;
                                ed.Content = sfHistory.Response_UserMsgBody;
                                ed.Content = ReplaceCodeSnippets(group, emailObj, ed.Content, xmlDR, user);
                                //emailSend.SimpleSend(emailAddress, from, subjectUser, bodyUser);
                                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed); //who does this send to? should send to ed.EmailAddress.
                                //log email
                                log = new ECN_Framework_Entities.Communicator.SmartFormActivityLog();
                                log.SFID = sfID;
                                log.CustomerID = customerID;
                                log.GroupID = groupID;
                                log.EmailID = emailID;
                                log.EmailType = "user";
                                log.EmailTo = emailAddress;
                                log.EmailFrom = from;
                                log.EmailSubject = ed.EmailSubject;
                                log.SendTime = DateTime.Now;
                                log.CreatedUserID = user.UserID;
                                ECN_Framework_BusinessLayer.Communicator.SmartFormActivityLog.Insert(log, user);
                                lc.UpdateUsed(customerID, "emailblock10k", 1);

                            }
                            catch (ECN_Framework_Common.Objects.ECNException ecn)
                            {
                                string message = "";
                                foreach (ECN_Framework_Common.Objects.ECNError e in ecn.ErrorList)
                                {
                                    message += "<br />" + e.Entity + ": " + e.ErrorMessage;
                                }
                                LogID = LogUnspecifiedException(ecn, ed.Process, message);
                            }
                            catch (Exception ex)
                            {
                                string note = "Sending email from SF, sfID: " + sfID.ToString() + " groupID: " + groupID.ToString() + " customerID: " + customerID.ToString() + " emailID: " + emailID.ToString() + " emailAddress: " + emailAddress + " userID: " + user.UserID.ToString();
                                LogID = LogUnspecifiedException(ex, "ecn.webservice.ListManager.SendEmailFromSF", note);

                            }
                        }

                        /* Send Response Email to Admin*/

                        if (from.Length > 5 && adminEmail.Length > 5)
                        {
                            try
                            {
                                ed.EmailAddress = adminEmail; //change email to admin's. Resave.
                                ed.EmailSubject = sfHistory.Response_AdminMsgSubject;
                                ed.Content = sfHistory.Response_AdminMsgBody;
                                ed.Content = ReplaceCodeSnippets(group, emailObj, ed.Content, xmlDR, user);
                                //ed.SimpleSend(respAdmin, from, subjectAdmin, bodyAdmin);
                                ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(ed);

                                //log email
                                log = new ECN_Framework_Entities.Communicator.SmartFormActivityLog();
                                log.SFID = sfID;
                                log.CustomerID = customerID;
                                log.GroupID = groupID;
                                log.EmailID = emailID;
                                log.EmailType = "admin";
                                log.EmailTo = ed.EmailAddress;
                                log.EmailFrom = from;
                                log.EmailSubject = ed.EmailSubject;
                                log.SendTime = DateTime.Now;
                                log.CreatedUserID = user.UserID;
                                ECN_Framework_BusinessLayer.Communicator.SmartFormActivityLog.Insert(log, user);
                                lc.UpdateUsed(customerID, "emailblock10k", 1);
                            }
                            catch (ECN_Framework_Common.Objects.ECNException ecn)
                            {
                                string message = "";
                                foreach (ECN_Framework_Common.Objects.ECNError e in ecn.ErrorList)
                                {
                                    message += "<br />" + e.Entity + ": " + e.ErrorMessage;
                                }
                                LogID = LogUnspecifiedException(ecn, ed.Process, message);
                            }
                            catch (Exception ex)
                            {
                                string note = "Sending response email to admin, sfID: " + sfID.ToString() + " groupID: " + groupID.ToString() + " customerID: " + customerID.ToString() + " emailID: " + emailID.ToString() + " emailAddress: " + emailAddress + " userID: " + user.UserID.ToString();
                                LogID = LogUnspecifiedException(ex, "ecn.webservice.ListManager.SendEmailFromSF", note);

                            }
                        }
                    }
                }
            }
        }

        #region extract Column Names from From EmailsTable
        //extract Column Names from From XLSFile / Table
        private void extractCoumnNamesFromEmailsTable()
        {
            DataTable email = ECN_Framework_BusinessLayer.Communicator.Email.GetColumnNames();
            emailsColumnHeadings = DataFunctions.GetDataTableColumns(email);

            for (int i = 0; i < emailsColumnHeadings.Count; i++)
            {
                emailsColumnHeadings[i] = emailsColumnHeadings[i].ToString().ToLower();
            }
        }
        #endregion

        private string cleanXMLString(string text)
        {

            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            text = text.Replace("á", "a");
            return text;
        }

        private Hashtable getUDFsForList(int groupID, KMPlatform.Entity.User user)
        {
            Hashtable fields = new Hashtable();
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, user);
            if (gdfList.Count > 0)
            {
                foreach (ECN_Framework_Entities.Communicator.GroupDataFields gdf in gdfList)
                {
                    fields.Add("user_" + gdf.ShortName.ToLower(), gdf.GroupDataFieldsID);
                }
            }

            return fields;
        }

        #region Component Designer generated code
        //Required by the Web Services Designer 
        private IContainer components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion


    }
}
