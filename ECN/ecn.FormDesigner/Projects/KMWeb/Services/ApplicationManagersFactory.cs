using KMManagers;

namespace KMWeb.Services
{
    public class ApplicationManagersFactory : IApplicationManagersFactory
    {
        public FormManager CreateFormManager()
        {
            return new FormManager();
        }

        public ControlManager CreateControlManager()
        {
            return new ControlManager();
        }

        public NotificationManager CreateNotificationManager()
        {
            return new NotificationManager();
        }

        public CssFileManager CreateCssFileManager()
        {
            return new CssFileManager();
        }

        public RuleManager CreateRuleManager()
        {
            return new RuleManager();
        }

        public ControlTypeManager CreateControlTypeManager()
        {
            return new ControlTypeManager();
        }

        public SubscriberLoginManager CreateSubscriberLoginManager()
        {
            return new SubscriberLoginManager();
        }
    }
}