namespace ecn.webservice
{
    public class Consts
    {
        public static readonly string GetBlastMethodName = "GetBlast";
        public static readonly string GetSubscriberCountMethodName = "GetSubscriberCount";
        public static readonly string AddBlastMainMethodName = "AddBlastMain";
        public static readonly string AddScheduledBlastMethodName = "AddScheduledBlastMain";
        public static readonly string UpdateBlastMethodName = "UpdateBlast";
        public static readonly string SearchForBlastsMethodName = "SearchForBlasts";
        public static readonly string SearchForContentMethodName = "SearchForContent";
        public static readonly string SearchForMessagesMethodName = "SearchForMessages";
        public static readonly string DeleteBlastMethodName = "DeleteBlast";
        public static readonly string GetBlastReportMethodName = "GetBlastReport";
        public static readonly string GetBlastReportByISPMethodName = "GetBlastReportByISP";
        public static readonly string GetBlastOpensReportMethodName = "GetBlastOpensReport";
        public static readonly string GetBlastClicksReportMethodName = "GetBlastClicksReport";
        public static readonly string GetBlastBounceReportMethodName = "GetBlastBounceReport";
        public static readonly string GetBlastUnsubscribeReportMethodName = "GetBlastUnsubscribeReport";
        public static readonly string GetBlastDeliveryReportMethodName = "GetBlastDeliveryReport";
        public static readonly string GetListEmailProfilesByEmailAddressMethodName = "GetListEmailProfilesByEmailAddress";
        public static readonly string GetCustomFieldsMethodName = "GetCustomFields";
        public static readonly string GetFiltersMethodName = "GetFilters";
        public static readonly string GetContentListByFolderIdMethodName = "GetContentListByFolderID";
        public static readonly string GetMessageListByFolderIdMethodName = "GetMessageListByFolderID";
        public static readonly string GetListsMethodName = "GetLists";
        public static readonly string GetListByNameMethodName = "GetListByName";
        public static readonly string GetListsByFolderIdMethodName = "GetListsByFolderID";
        public static readonly string DeleteFolderMethodName = "DeleteFolder";
        public static readonly string DeleteListMethodName = "DeleteList";
        public static readonly string DeleteSubscriberMethodName = "DeleteSubscriber";
        public static readonly string DeleteCustomFieldMethodName = "DeleteCustomField";
        public static readonly string UnsubscribeSubscriberMethodName = "UnsubscribeSubscriber";
        public static readonly string UpdateCustomFieldMethodName = "UpdateCustomField";
        public static readonly string AddCustomFieldMethodName = "AddCustomField";
        public static readonly string AddSubscribersMethodName = "AddSubscribers";
        public static readonly string AddSubscriberUsingSmartFormMethodName = "AddSubscriberUsingSmartForm";
        public static readonly string AddSubscribersWithDupesMethodName = "AddSubscribersWithDupes";
        public static readonly string AddSubscribersWithDupesUsingSmartFormMethodName = "AddSubscribersWithDupesUsingSmartForm";
        public static readonly string GetContentMethodName = "GetContent";
        public static readonly string GetMessageMethodName = "GetMessage";
        public static readonly string PreviewMessageMethodName = "PreviewMessage";
        public static readonly string PreviewContentMethodName = "PreviewContent";
        public static readonly string GetFoldersMethodName = "GetFolders";
        public static readonly string GetTemplatesMethodName = "GetTemplates";
        public static readonly string GetMessageTypesMethodName = "GetMessageTypes";
        public static readonly string GetCustomerDeptsMethodName = "GetCustomerDepts";
        public static readonly string DeleteContentMethodName = "DeleteContent";
        public static readonly string DeleteMessageMethodName = "DeleteMessage";
        public static readonly string UpdateContentMethodName = "UpdateContent";
        public static readonly string UpdateMessageMethodName = "UpdateMessage";
        public static readonly string AddContentMethodName = "AddContentMain";
        public static readonly string AddMessageMainMethodName = "AddMessageMain";
        public static readonly string AddFolderMainMethodName = "AddFolderMain";

        public static readonly string EmptyRootLogInput = "<ROOT></ROOT>";
        public static readonly string GetBlastLogInput = "<ROOT><BlastID>{0}</BlastID></ROOT>";
        public static readonly string DeleteBlastLogInput = "<ROOT><BlastID>{0}</BlastID></ROOT>";
        public static readonly string GetSubscriberCountLogInput = "<ROOT><GroupID>{0}</GroupID></ROOT>";
        public static readonly string AddBlastLogInput =
            "<ROOT><MessageID>{0}</MessageID><ListID>{1}</ListID><DeptID>{2}</DeptID><FilterID>{3}</FilterID><Subject>{4}</Subject>" +
            "<FromEmail>{5}</FromEmail><FromName>{6}</FromName><ReplyEmail>{7}</ReplyEmail><IsTest>{8}</IsTest><RefBlasts>{9}</RefBlasts>" +
            "<OverRideAmount>{10}</OverRideAmount><IsOverRideAmount>{11}</IsOverRideAmount></ROOT>";
        public static readonly string UpdateBlastLogInput =
            "<ROOT><MessageID>{0}</MessageID><ListID>{1}</ListID><BlastID>{2}</BlastID><FilterID>{3}</FilterID><Subject>{4}</Subject>" +
            "<FromEmail>{5}</FromEmail><FromName>{6}</FromName><ReplyEmail>{7}</ReplyEmail></ROOT>";
        public static readonly string SearchLogInput = "<ROOT><XMLSearch><![CDATA[{0}]]></XMLSearch></ROOT>";
        public static readonly string GetBlastReportLogInput = "<ROOT><BlastID>{0}</BlastID></ROOT>";
        public static readonly string GetBlastReportByISPLogInput = "<ROOT><BlastID>{0}</BlastID><XMLSearch><![CDATA[{1}]]></XMLSearch></ROOT>";
        public static readonly string GetBlastOpensReportLogInput = "<ROOT><BlastID>{0}</BlastID><FilterType>{1}</FilterType><WithDetail>{2}</WithDetail></ROOT>";
        public static readonly string GetBlastClicksReportLogInput = "<ROOT><BlastID>{0}</BlastID><FilterType>{1}</FilterType><WithDetail>{2}</WithDetail></ROOT>";
        public static readonly string GetBlastBounceReportLogInput = "<ROOT><BlastID>{0}</BlastID><WithDetail>{1}</WithDetail></ROOT>";
        public static readonly string GetBlastUnsubscribeReportLogInput = "<ROOT><BlastID>{0}</BlastID><WithDetail>{1}</WithDetail></ROOT>";
        public static readonly string GetBlastDeliveryReportLogInput = "<ROOT><FromDate>{0}</FromDate><ToDate>{1}</ToDate></ROOT>";
        public static readonly string GetListEmailProfilesByEmailAddressLogInput = "<ROOT><ListID>{0}</ListID><EmailAddress>{1}</EmailAddress></ROOT>";
        public static readonly string ListIdLogInput = "<ROOT><ListID>{0}</ListID></ROOT>";
        public static readonly string FolderIdLogInput = "<ROOT><FolderID>{0}</FolderID></ROOT>";
        public static readonly string GetListByNameLogInput = "<ROOT><Name>{0}</Name></ROOT>";
        public static readonly string DeleteSubscriberLogInput = "<ROOT><ListID>{0}</ListID><EmailAddress>{1}</EmailAddress></ROOT>";
        public static readonly string DeleteCustomFieldLogInput = "<ROOT><ListID>{0}</ListID><UDFID>{1}</UDFID></ROOT>";
        public static readonly string UnsubscribeSubscriberLogInput = "<ROOT><ListID>{0}</ListID><XMLEmails>{1}</XMLEmails></ROOT>";
        public static readonly string UpdateCustomFieldLogInput = "<ROOT><ListID>{0}</ListID><UDFID>{1}</UDFID><CustomFieldName>{2}</CustomFieldName><CustomFieldDescription>{3}</CustomFieldDescription><IsPublic>{4}</IsPublic></ROOT>";
        public static readonly string AddCustomFieldLogInput = "<ROOT><ListID>{0}</ListID><CustomFieldName>{1}</CustomFieldName><CustomFieldDescription>{2}</CustomFieldDescription><IsPublic>{3}</IsPublic></ROOT>";
        public static readonly string GetCustomFieldsLogInput = "<ROOT><ListID>{0}</ListID></ROOT>";
        public static readonly string GetFiltersLogInput = "<ROOT><ListID>{0}</ListID></ROOT>";
        public static readonly string GetByFolderIdLogInput = "<ROOT><FolderID>{0}</FolderID></ROOT>";
        public static readonly string GetContentLogInput = "<ROOT><ContentID>{0}</ContentID></ROOT>";
        public static readonly string GetMessageLogInput = "<ROOT><LayoutID>{0}</LayoutID></ROOT>";
        public static readonly string PreviewMessageLogInput = "<ROOT><MessageID>{0}</MessageID></ROOT>";
        public static readonly string PreviewContentLogInput = "<ROOT><ContentID>{0}</ContentID></ROOT>";
        public static readonly string ContentIdLogInput = "<ROOT><ContentID>{0}</ContentID></ROOT>";
        public static readonly string MessageIdLogInput = "<ROOT><MessageID>{0}</MessageID></ROOT>";
        public static readonly string AddMessageLogInput =
            "<ROOT><LayoutName>{0}</LayoutName><FolderID>{1}</FolderID><TableBorder>{2}</TableBorder><TemplateID>{3}</TemplateID>" +
            "<Address>{4}</Address><DeptID>{5}</DeptID><Content0>{6}</Content0><Content1>{7}</Content1><Content2>{8}</Content2><Content3>{9}</Content3>" +
            "<Content4>{10}</Content4><Content5>{11}</Content5><Content6>{12}</Content6><Content7>{13}</Content7><Content8>{14}</Content8></ROOT>";
        public static readonly string UpdateMessageLogInput =
            "<ROOT><LayoutName>{0}</LayoutName><TableBorder>{1}</TableBorder><TemplateID>{2}</TemplateID><Address>{3}</Address><DeptID>{4}</DeptID><Content0>{5}</Content0>" +
            "<Content1>{6}</Content1><Content2>{7}</Content2><Content3>{8}</Content3><Content4>{9}</Content4><Content5>{10}<Content5>" +
            "<Content6>{11}</Content6><Content7>{12}</Content7><Content8>{13}</Content8><MessageID>{14}</MessageID></ROOT>";
        public static readonly string AddContentLogInput = "<ROOT><Title>{0}</Title><ContentHTML><![CDATA[{1}]]></ContentHTML><ContentText><![CDATA[{2}]]></ContentText><FolderID>{3}</FolderID><UseWYSIWYGeditor>{4}</UseWYSIWYGeditor></ROOT>";
        public static readonly string UpdateContentLogInput = "<ROOT><Title>{0}</Title><ContentHTML><![CDATA[{1}]]></ContentHTML><ContentText>{2}</ContentText><ContentID>{3}</ContentID></ROOT>";
        public static readonly string AddFolderLogInput = "<ROOT><FolderName>{0}</FolderName><FolderDescription>{1}</FolderDescription><ParentFolderID>{2}</ParentFolderID></ROOT>";
        public static readonly string AddSubscribersLogInput = "<ROOT><ListID>{0}</ListID><SubscriptionType>{1}</SubscriptionType><FormatType>{2}</FormatType><XMLString><![CDATA[{3}]]></XMLString></ROOT>";
        public static readonly string AddSubscribersWithDupesLogInput = "<ROOT><ListID>{0}</ListID><SubscriptionType>{1}</SubscriptionType><FormatType>{2}</FormatType><CompositeKey>{3}</CompositeKey><OverwriteWithNull>{4}</OverwriteWithNull><XMLString><![CDATA[{5}]]></XMLString></ROOT>";
        public static readonly string AddSubscribersWithDupesUsingSmartFormLogInput = "<ROOT><ListID>{0}</ListID><SubscriptionType>{1}</SubscriptionType><FormatType>{2}</FormatType><CompositeKey>{3}</CompositeKey><OverwriteWithNull>{4}</OverwriteWithNull><XMLString><![CDATA[{5}]]></XMLString><SFID>{6}</SFID></ROOT>";
        public static readonly string AddSubscriberUsingSmartFormLogInput = "<ROOT><ListID>{0}</ListID><SubscriptionType>{1}</SubscriptionType><FormatType>{2}</FormatType><XMLString><![CDATA[{3}]]></XMLString><SFID>{4}</SFID></ROOT>";

        public static readonly string BlastManagerServiceMethodName = "ecn.webservice.BlastManager.{0}";
        public static readonly string ListManagerServiceMethodName = "ecn.webservice.ListManager.{0}";
        public static readonly string ContentManagerServiceMethodName = "ecn.webservice.ContentManager.{0}";

        public static readonly string BlastIdParameter = "blastId";
        public static readonly string GroupIdParameter = "groupId";
        public static readonly string MessageIdParameter = "messageId";
        public static readonly string ListIdParameter = "listId";
        public static readonly string FilterIdParameter = "filterId";
        public static readonly string SubjectParameter = "subject";
        public static readonly string FromEmailParameter = "fromEmail";
        public static readonly string FromNameParameter = "fromName";
        public static readonly string ReplyEmailParameter = "replyEmail";
        public static readonly string IsTestParameter = "isTest";
        public static readonly string RefBlastsParameter = "refBlasts";
        public static readonly string XmlScheduleParameter = "xmlSchedule";

        public static readonly string InvalidEcnAccessKeyResponseOutput = "INVALID ECN ACCESS KEY FORMAT";
        public static readonly string LoginFailedResponseOutput = "LOGIN AUTHENTICATION FAILED";
        public static readonly string SecurityViolationResponseOutput = "SECURITY VIOLATION";
        public static readonly string BlastNotFoundResponseOutput = "BLAST NOT FOUND: {0}";
        public static readonly string BlastDoesntExistResponseOutput = "BLAST DOESN'T EXIST";
        public static readonly string DeleteBlastResposneOutput = "BLAST DELETED";
    }
}