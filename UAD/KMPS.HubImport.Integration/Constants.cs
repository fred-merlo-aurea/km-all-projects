namespace KMPS.Hubspot.Integration
{
	public class Constants
	{
		public static string RecentCampaignURL = "https://api.hubapi.com/email/public/v1/campaigns?hapikey=";
		public const string RecentContactURL = "https://api.hubapi.com/contacts/v1/lists/recently_updated/contacts/recent?hapikey=";
		public const string PROPERTIES_KEY = "properties";
		public const string IDENTITY_PROFILES_KEY = "identity-profiles";
		public const string IDENTITIES_IDENTITY_PROFILES_KEY = "identities";
		public const string VALUE_KEY = "value";
		public const string FIRSTNAME_KEY = "firstname";
		public const string LASTNAME_KEY = "lastname";
		public const string CONTACTS_KEY = "contacts";
		public const string VID_OFFSET_KEY = "vid-offset";
		public const string TIME_OFFSET_KEY = "time-offset";
		public const string HUBSPOTINTEGRATIONJSON = @"hubspot_integration.json";
		public const string CAMPAIGNS = "campaigns";
		public const string CAMPAIGN_OFF_SET = "offset";
		public const string SubjectUrl = "https://api.hubapi.com/email/public/v1/campaigns/";
		public const string OpenUrl = "https://api.hubapi.com/email/public/v1/events?hapikey=";
		public static string EVENTS_KEY = "events";
		public static string OPEN = "OPEN";
		public static string CLICK = "CLICK";
		public const string ClickUrl = "https://api.hubapi.com/email/public/v1/events?hapikey=";

		public const string KmpsHubimportintegrationAppName = "KMPS.HubImportIntegration";
	}
}