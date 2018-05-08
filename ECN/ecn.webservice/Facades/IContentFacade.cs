using ecn.webservice.Facades.Params;

namespace ecn.webservice.Facades
{
    public interface IContentFacade
    {
        string SearchForContent(WebMethodExecutionContext context, string XMLSearch);

        string SearchForMessages(WebMethodExecutionContext context, string XMLSearch);

        string GetContentListByFolderId(WebMethodExecutionContext context, int folderId);

        string GetMessageListByFolderId(WebMethodExecutionContext context, int folderId);

        string GetContent(WebMethodExecutionContext context, int contentId);

        string GetMessage(WebMethodExecutionContext context, int layoutId);

        string PreviewMessage(WebMethodExecutionContext context, int messageId);

        string PreviewContent(WebMethodExecutionContext context, int contentId);

        string GetFolders(WebMethodExecutionContext context);

        string GetTemplates(WebMethodExecutionContext context);

        string GetMessageTypes(WebMethodExecutionContext context);

        string GetCustomerDepts(WebMethodExecutionContext context);

        string DeleteFolder(WebMethodExecutionContext context, int folderId);

        string AddContent(WebMethodExecutionContext context, ContentParams parameters);

        string UpdateContent(WebMethodExecutionContext context, ContentParams parameters);

        string DeleteContent(WebMethodExecutionContext context, int contentId);

        string AddMessage(WebMethodExecutionContext context, MessageParams parameters);

        string UpdateMessage(WebMethodExecutionContext context, MessageParams parameters);

        string DeleteMessage(WebMethodExecutionContext context, int messageId);

        string AddFolder(WebMethodExecutionContext context, FolderParams parameters);
    }
}