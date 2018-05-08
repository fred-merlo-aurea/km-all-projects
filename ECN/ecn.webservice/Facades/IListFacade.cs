using System.Data;
using ecn.webservice.Facades.Params;

namespace ecn.webservice.Facades
{
    public interface IListFacade
    {
        string GetFolders(WebMethodExecutionContext context);

        string GetListEmailProfilesByEmailAddress(
            WebMethodExecutionContext context,
            GetListEmailProfilesParams parameters);

        string GetCustomFields(WebMethodExecutionContext context, int listId);

        string GetFilters(WebMethodExecutionContext context, int listId);

        string GetLists(WebMethodExecutionContext context);

        string GetListByName(WebMethodExecutionContext context, string name);

        string GetListsByFolderId(WebMethodExecutionContext context, int folderId);

        string GetSubscriberCount(WebMethodExecutionContext context, int groupId);

        string DeleteFolder(WebMethodExecutionContext context, int folderId);

        string DeleteList(WebMethodExecutionContext context, int listId);

        string DeleteSubscriber(WebMethodExecutionContext context, DeleteSubscriberParams parameters);

        string DeleteCustomField(WebMethodExecutionContext context, DeleteCustomFieldParams parameters);

        string UnsubscribeSubscriber(WebMethodExecutionContext context, UnsubscribeSubscriberParams parameters);

        string UpdateCustomField(WebMethodExecutionContext context, CustomFieldParams parameters);

        string AddCustomField(WebMethodExecutionContext context, CustomFieldParams parameters);

        string AddSubscribers(WebMethodExecutionContext context, AddSubscribersParams parameters);

        string AddSubscribersWithDupes(WebMethodExecutionContext context, AddSubscribersParams parameters);

        string AddSubscriberUsingSmartForm(WebMethodExecutionContext context, AddSubscribersParams parameters);

        DataTable ExtractColumnNamesFromXmlString(string xmlString, int listId, int customerId);
    }
}