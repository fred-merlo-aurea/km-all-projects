using System;

namespace KMDbManagers
{
    public class DbResolver
    {
        public ConditionDbManager ConditionDbManager = new ConditionDbManager();
        public ConditionGroupDbManager ConditionGroupDbManager = new ConditionGroupDbManager();
        public ControlDbManager ControlDbManager = new ControlDbManager();
        public ControlPropertyDbManager ControlPropertyDbManager = new ControlPropertyDbManager();
        public CssClassDbManager CssClassDbManager = new CssClassDbManager();
        public CssFileDbManager CssFileDbManager = new CssFileDbManager();
        public DataTypePatternDbManager DataTypePatternDbManager = new DataTypePatternDbManager();
        public FormControlPropertyDbManager FormControlPropertyDbManager = new FormControlPropertyDbManager();
        public FormControlPropertyGridDbManager FormControlPropertyGridDbManager = new FormControlPropertyGridDbManager();
        public FormDbManager FormDbManager = new FormDbManager();
        public FormResultDbManager FormResultDbManager = new FormResultDbManager();
        public FormStatisticDbManager FormStaticsticDbManager = new FormStatisticDbManager();
        public NotificationDbManager NotificationDbManager = new NotificationDbManager();
        public RuleDbManager RuleDbManager = new RuleDbManager();
        public SubmitHistoryDbManager SubmitHistoryDbManager = new SubmitHistoryDbManager();
        public ThirdPartyQueryValueDbManager ThirdPartyQueryValueDbManager = new ThirdPartyQueryValueDbManager();
        public NewsletterGroupDbManager NewsletterGroupDbManager = new NewsletterGroupDbManager();
        public NewsletterGroupUDFDbManager NewsletterGroupUDFDbManager = new NewsletterGroupUDFDbManager();
        public ControlCategoryDbManager ControlCategoryDbManager = new ControlCategoryDbManager();
        public OverwriteDataPostDbManager OverWriteDataPostDbManager = new OverwriteDataPostDbManager();
        public RequestQueryUrlDbManager RequestQueryUrlDbManager = new RequestQueryUrlDbManager();
        public ControlTypeDbManager ControlTypeDbManager = new ControlTypeDbManager();
        public SubscriberLoginDbManager SubscriberLoginDbManager = new SubscriberLoginDbManager();
    }
}