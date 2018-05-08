using KMPS.HubImport.Integration.Process;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KMPS.HubImport.Integration.Entity
{
	[JsonObject]
	public class CampaignSubject
	{
		public int Id { get; set; }
		public int AppId { get; set; }
		public string Subject { get; set; }
		public string Name { get; set; }
		[JsonProperty("counters")]
		public CampaignSubjectCounters CampaignSubjectCounters { get; set; }
		public int NumIncluded { get; set; }
		public int NumQueued { get; set; }
		public string Type { get; set; }
		public long ContentId { get; set; }
	}
}