using Newtonsoft.Json;

namespace KMPS.HubImport.Integration.Entity
{
	[JsonObject]
	public class Event
	{
		public string AppName { get; set; }
		public string UserAgent { get; set; }
		public int? Duration { get; set; }
		public Location Location { get; set; }
		public string Id { get; set; }
		public long Created { get; set; }
		public string IpAddress { get; set; }
		public Browser Browser { get; set; }
		public string Type { get; set; }
		public string DeviceType { get; set; }
		public int AppId { get; set; }
		public int PortalId { get; set; }
		[JsonProperty("recipient")]
		public string Recipient { get; set; }
		public object SmtpId { get; set; }
		public Sentby SentBy { get; set; }
		public string Hmid { get; set; }
		public int EmailCampaignId { get; set; }
		public Causedby CausedBy { get; set; }
		[JsonProperty("url")]
		public string Url { get; set; }
	}
}