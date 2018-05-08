using KMManagers;

namespace KMWeb.Services
{
    public interface IApplicationManagersFactory
    {
        FormManager CreateFormManager();
        ControlManager CreateControlManager();
        NotificationManager CreateNotificationManager();
        CssFileManager CreateCssFileManager();
        RuleManager CreateRuleManager();
        ControlTypeManager CreateControlTypeManager();
        SubscriberLoginManager CreateSubscriberLoginManager();
    }
}