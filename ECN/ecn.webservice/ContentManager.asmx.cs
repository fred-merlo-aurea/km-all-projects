using System.Web.Services;
using ecn.webservice.Facades;
using ecn.webservice.Facades.Params;

namespace ecn.webservice
{
    [WebService(Namespace = "http://webservices.ecn5.com/", Description = "The ECN Application Programming Interface (API) is a web service that allows you to control your ECN account programatically via an HTTP POST, an HTTP GET, or an XML-based SOAP call. The following web service methods allow access to managing your Content and Messages in ECN. The supported methods are shown below. <u>IMPORTANT NOTE:</u> All methods need ECN ACCESS KEY to work properly.")]
    public class ContentManager : WebServiceManagerBase
    {
        private IContentFacade _contentFacade;

        public IContentFacade ContentFacade
        {
            get
            {
                if (_contentFacade == null)
                {
                    _contentFacade = new ContentFacade();
                }
                return _contentFacade;
            }
            set
            {
                _contentFacade = value;
            }
        }

        public ContentManager()
        {
        }

        public ContentManager(IWebMethodExecutionWrapper executionWrapper)
            : base(executionWrapper)
        {
        }

        #region Search For Content - SearchForContent()

        [WebMethod(Description = "Search for Content in ECN. ")]
        public string SearchForContent(string ecnAccessKey, string XMLSearch)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.SearchForContentMethodName),
                Consts.SearchForContentMethodName,
                ecnAccessKey,
                string.Format(Consts.SearchLogInput, XMLSearch));

            return executionWrapper.Execute(ContentFacade.SearchForContent, XMLSearch);
        }

        #endregion

        #region Search For Messages - SearchForMessages()

        [WebMethod(Description = "Search for Messages in ECN. ")]
        public string SearchForMessages(string ecnAccessKey, string XMLSearch)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.SearchForMessagesMethodName),
                Consts.SearchForMessagesMethodName,
                ecnAccessKey,
                string.Format(Consts.SearchLogInput, XMLSearch));

            return executionWrapper.Execute(ContentFacade.SearchForMessages, XMLSearch);
        }

        #endregion

        #region Get Content List by Folder ID - GetContentListByFolderID()

        [WebMethod(Description = "Get Content List from ECN. ")]
        public string GetContentListByFolderID(string ecnAccessKey, int FolderID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.GetContentListByFolderIdMethodName),
                Consts.GetContentListByFolderIdMethodName,
                ecnAccessKey,
                string.Format(Consts.FolderIdLogInput, FolderID));

            return executionWrapper.Execute(ContentFacade.GetContentListByFolderId, FolderID);
        }

        #endregion

        #region Get Message List by Folder ID - GetMessageListByFolderID()

        [WebMethod(Description = "Get Message List from ECN. ")]
        public string GetMessageListByFolderID(string ecnAccessKey, int FolderID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.GetMessageListByFolderIdMethodName),
                Consts.GetMessageListByFolderIdMethodName,
                ecnAccessKey,
                string.Format(Consts.FolderIdLogInput, FolderID));

            return executionWrapper.Execute(ContentFacade.GetMessageListByFolderId, FolderID);
        }

        #endregion

        #region Get Content - GetContent()

        [WebMethod(Description = "Get Content from ECN. ")]
        public string GetContent(string ecnAccessKey, int ContentID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.GetContentMethodName),
                Consts.GetContentMethodName,
                ecnAccessKey,
                string.Format(Consts.GetContentLogInput, ContentID));

            return executionWrapper.Execute(ContentFacade.GetContent, ContentID);
        }

        #endregion

        #region Get Message - GetMessage()

        [WebMethod(Description = "Get Message from ECN. ")]
        public string GetMessage(string ecnAccessKey, int layoutID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.GetMessageMethodName),
                Consts.GetMessageMethodName,
                ecnAccessKey,
                string.Format(Consts.GetMessageLogInput, layoutID));

            return executionWrapper.Execute(ContentFacade.GetMessage, layoutID);
        }

        #endregion

        #region Get Message HTML - PreviewMessage()

        [WebMethod(Description = "Get HTML Message from ECN. ")]
        public string PreviewMessage(string ecnAccessKey, int MessageID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.PreviewMessageMethodName),
                Consts.PreviewMessageMethodName,
                ecnAccessKey,
                string.Format(Consts.PreviewMessageLogInput, MessageID));

            return executionWrapper.Execute(ContentFacade.PreviewMessage, MessageID);
        }

        #endregion

        #region Get Content HTML - PreviewContent()

        [WebMethod(Description = "Get HTML Content from ECN. ")]
        public string PreviewContent(string ecnAccessKey, int ContentID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.PreviewContentMethodName),
                Consts.PreviewContentMethodName,
                ecnAccessKey,
                string.Format(Consts.PreviewContentLogInput, ContentID));

            return executionWrapper.Execute(ContentFacade.PreviewContent, ContentID);
        }

        #endregion

        #region Get Content Folders - GetFolders()

        [WebMethod(Description = "GetContent Folders from ECN. ")]
        public string GetFolders(string ecnAccessKey)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.GetFoldersMethodName),
                Consts.GetFoldersMethodName,
                ecnAccessKey,
                Consts.EmptyRootLogInput);

            return executionWrapper.Execute(ContentFacade.GetFolders);
        }

        #endregion

        #region Get Templates - GetTemplates()

        [WebMethod(Description = "Get Templates from ECN. ")]
        public string GetTemplates(string ecnAccessKey)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.GetTemplatesMethodName),
                Consts.GetTemplatesMethodName,
                ecnAccessKey,
                Consts.EmptyRootLogInput);

            return executionWrapper.Execute(ContentFacade.GetTemplates);
        }

        #endregion

        #region Get Message Types - GetMessageTypes()

        [WebMethod(Description = "Get Message Types from ECN. ")]
        public string GetMessageTypes(string ecnAccessKey)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.GetMessageTypesMethodName),
                Consts.GetMessageTypesMethodName,
                ecnAccessKey,
                Consts.EmptyRootLogInput);

            return executionWrapper.Execute(ContentFacade.GetMessageTypes);
        }

        #endregion

        #region Get Customer Departments - GetCustomerDepts()

        [WebMethod(Description = "Get Customer Departments from ECN. ")]
        public string GetCustomerDepts(string ecnAccessKey)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.GetCustomerDeptsMethodName),
                Consts.GetCustomerDeptsMethodName,
                ecnAccessKey,
                Consts.EmptyRootLogInput);

            return executionWrapper.Execute(ContentFacade.GetCustomerDepts);
        }

        #endregion

        #region ADD NEW Content - AddContent()
        [WebMethod(
             Description = "Add new Content to ECN. ", MessageName = "AddContent")
        ]
        public string AddContent(string ecnAccessKey, string Title, string ContentHTML, string ContentText)
        {
            return AddContentMain(ecnAccessKey, Title, ContentHTML, ContentText, 0, true);
        }
        #endregion

        #region ADD NEW Content to Folder - AddContent()
        [WebMethod(
             Description = "Add new Content to a Folder in ECN. ", MessageName = "AddContentToFolder")
        ]
        public string AddContent(string ecnAccessKey, string Title, string ContentHTML, string ContentText, int FolderID)
        {
            return AddContentMain(ecnAccessKey, Title, ContentHTML, ContentText, FolderID, true);
        }
        #endregion

        #region ADD NEW Content to Folder with Preference - AddContentWithPreference()
        [WebMethod(
             Description = "Add new Content to a Folder in ECN with editor preference. ", MessageName = "AddContentWithPreference")
        ]
        public string AddContentWithPreference(string ecnAccessKey, string Title, string ContentHTML, string ContentText, int FolderID, bool UseWYSIWYGeditor)
        {
            return AddContentMain(ecnAccessKey, Title, ContentHTML, ContentText, FolderID, UseWYSIWYGeditor);
        }
        #endregion

        #region ADD NEW Message - AddMessage()
        [WebMethod(
             Description = "Add new Message to ECN. ", MessageName = "AddMessage")
        ]
        public string AddMessage(string ecnAccessKey, string LayoutName, string TableBorder, int TemplateID, string Address, int DeptID, int Content0, int Content1, int Content2, int Content3, int Content4, int Content5, int Content6, int Content7, int Content8)
        {
            return AddMessageMain(ecnAccessKey, LayoutName, null, null, TableBorder, TemplateID, Address, DeptID, Content0, Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8);
        }
        #endregion

        #region ADD NEW Message to Folder - AddMessage()
        [WebMethod(
             Description = "Add new Message to a Folder in ECN. ", MessageName = "AddMessageToFolder")
        ]
        public string AddMessage(string ecnAccessKey, string LayoutName, int FolderID, string TableBorder, int TemplateID, string Address, int DeptID, int Content0, int Content1, int Content2, int Content3, int Content4, int Content5, int Content6, int Content7, int Content8)
        {
            return AddMessageMain(ecnAccessKey, LayoutName, null, FolderID, TableBorder, TemplateID, Address, DeptID, Content0, Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8);
        }
        #endregion

        #region ADD NEW Message to Folder with MessageType - AddMessageWithType()
        [WebMethod(
             Description = "Add new Message to a Folder with Message Type in ECN. ", MessageName = "AddMessageWithType")
        ]
        public string AddMessageWithType(string ecnAccessKey, string LayoutName, int MessageTypeID, int FolderID, string TableBorder, int TemplateID, string Address, int DeptID, int Content0, int Content1, int Content2, int Content3, int Content4, int Content5, int Content6, int Content7, int Content8)
        {
            return AddMessageMain(ecnAccessKey, LayoutName, MessageTypeID, FolderID, TableBorder, TemplateID, Address, DeptID, Content0, Content1, Content2, Content3, Content4, Content5, Content6, Content7, Content8);
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

        #region Delete Folder - DeleteFolder()

        [WebMethod(Description = "Delete a Content Folder from ECN. ")]
        public string DeleteFolder(string ecnAccessKey, int FolderID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.DeleteFolderMethodName),
                Consts.DeleteFolderMethodName,
                ecnAccessKey,
                string.Format(Consts.FolderIdLogInput, FolderID));

            return executionWrapper.Execute(ContentFacade.DeleteFolder, FolderID);
        }

        #endregion

        #region Delete Content - DeleteContent()

        [WebMethod(Description = "Delete Content from ECN. ")]
        public string DeleteContent(string ecnAccessKey, int ContentID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.DeleteContentMethodName),
                Consts.DeleteContentMethodName,
                ecnAccessKey,
                string.Format(Consts.ContentIdLogInput, ContentID));

            return executionWrapper.Execute(ContentFacade.DeleteContent, ContentID);
        }

        #endregion

        #region Delete Message - DeleteMessage()

        [WebMethod(Description = "Delete Message from ECN. ")]
        public string DeleteMessage(string ecnAccessKey, int MessageID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.DeleteMessageMethodName),
                Consts.DeleteMessageMethodName,
                ecnAccessKey,
                string.Format(Consts.MessageIdLogInput, MessageID));

            return executionWrapper.Execute(ContentFacade.DeleteMessage, MessageID);
        }

        #endregion

        #region Update Content - UpdateContent()

        [WebMethod(Description = "Update existing Content in ECN. ")]
        public string UpdateContent(string ecnAccessKey, string Title, string ContentHTML, string ContentText, int ContentID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.UpdateContentMethodName),
                Consts.UpdateContentMethodName,
                ecnAccessKey,
                string.Format(Consts.UpdateContentLogInput, Title, ContentHTML, ContentText, ContentID));

            var parameters = new ContentParams()
            {
                ContentId = ContentID,
                ContentText = ContentText,
                ContentHtml = ContentHTML,
                Title = Title
            };

            return executionWrapper.Execute(ContentFacade.UpdateContent, parameters);
        }

        #endregion

        #region Update Message - UpdateMessage()

        [WebMethod(Description = "Update existing Message in ECN. ")]
        public string UpdateMessage(string ecnAccessKey, string LayoutName, string TableBorder, int TemplateID,
            string Address, int DeptID, int Content0, int Content1, int Content2, int Content3, int Content4,
            int Content5, int Content6, int Content7, int Content8, int MessageID)
        {
            var logInput = string.Format(
                Consts.UpdateMessageLogInput,
                LayoutName,
                TableBorder,
                TemplateID,
                Address,
                DeptID,
                Content0,
                Content1,
                Content2,
                Content3,
                Content4,
                Content5,
                Content6,
                Content7,
                Content8,
                MessageID
            );

            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.UpdateMessageMethodName),
                Consts.UpdateMessageMethodName,
                ecnAccessKey,
                logInput);

            var parameters = new MessageParams
            {
                Address = Address,
                Content0 = Content0,
                Content1 = Content1,
                Content2 = Content2,
                Content3 = Content3,
                Content4 = Content4,
                Content5 = Content5,
                Content6 = Content6,
                Content7 = Content7,
                Content8 = Content8,
                DeptId = DeptID,
                LayoutName = LayoutName,
                MessageId = MessageID,
                TableBorder = TableBorder,
                TemplateId = TemplateID
            };

            return executionWrapper.Execute(ContentFacade.UpdateMessage, parameters);
        }

        #endregion

        private string AddContentMain(string ecnAccessKey, string Title, string ContentHTML, string ContentText,
            int FolderID, bool UseWYSIWYGeditor)
        {
            var logInput = string.Format(
                Consts.AddContentLogInput,
                Title,
                ContentHTML,
                ContentText,
                FolderID,
                UseWYSIWYGeditor
            );

            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.AddContentMethodName),
                Consts.AddContentMethodName,
                ecnAccessKey,
                logInput
            );

            var parameters = new ContentParams
            {
                ContentHtml = ContentHTML,
                ContentText = ContentText,
                FolderId = FolderID,
                Title = Title,
                UseWysiwigEditor = UseWYSIWYGeditor
            };

            return executionWrapper.Execute(ContentFacade.AddContent, parameters);
        }

        private string AddMessageMain(string ecnAccessKey, string LayoutName, int? MessageTypeID, int? FolderID,
            string TableBorder, int TemplateID, string Address, int DeptID, int Content0, int Content1, int Content2,
            int Content3, int Content4, int Content5, int Content6, int Content7, int Content8)
        {
            var logInput = string.Format(
                Consts.AddMessageLogInput,
                LayoutName,
                FolderID.HasValue ? FolderID.Value.ToString() : string.Empty,
                TableBorder,
                TemplateID,
                Address,
                DeptID,
                Content0,
                Content1,
                Content2,
                Content3,
                Content4,
                Content5,
                Content6,
                Content7,
                Content8
            );

            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.AddMessageMainMethodName),
                Consts.AddMessageMainMethodName,
                ecnAccessKey,
                logInput
            );

            var parameters = new MessageParams
            {
                LayoutName = LayoutName,
                MessageTypeId = MessageTypeID,
                FolderId = FolderID,
                TableBorder = TableBorder,
                TemplateId = TemplateID,
                Address = Address,
                DeptId = DeptID,
                Content0 = Content0,
                Content1 = Content1,
                Content2 = Content2,
                Content3 = Content3,
                Content4 = Content4,
                Content5 = Content5,
                Content6 = Content6,
                Content7 = Content7,
                Content8 = Content8
            };

            return executionWrapper.Execute(ContentFacade.AddMessage, parameters);
        }

        private string AddFolderMain(string ecnAccessKey, string folderName, string folderDescription,
            int? parentFolderID)
        {
            var parentFolderId = (parentFolderID.HasValue ? parentFolderID.Value.ToString() : string.Empty);
            var logInput = string.Format(Consts.AddFolderLogInput, folderName, folderDescription, parentFolderId);

            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.ContentManagerServiceMethodName, Consts.AddFolderMainMethodName),
                Consts.AddFolderMainMethodName,
                ecnAccessKey,
                logInput
            );

            var parameters = new FolderParams
            {
                FolderName = folderName,
                FolderDescription = folderDescription,
                ParentFolderId = parentFolderID
            };

            return executionWrapper.Execute(ContentFacade.AddFolder, parameters);
        }
    }
}
