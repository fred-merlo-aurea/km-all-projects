using Newtonsoft.Json;

namespace KMPS.HubImport.Integration.Entity
{
	[JsonObject]
	public class CampaignSubjectCounters
	{
		[JsonProperty("processed")]
		public int Processed { get; set; }

		[JsonProperty("deferred")]
		public int Deferred { get; set; }

		[JsonProperty("forward")]
		public int Forward { get; set; }

		[JsonProperty("delivered")]
		public int Delivered { get; set; }

		[JsonProperty("sent")]
		public int Sent { get; set; }

		[JsonProperty("click")]
		public int Click { get; set; }

		[JsonProperty("open")]
		public int Open { get; set; }
	}
}