using System;
using System.Collections.Generic;

namespace KMPS.HubImport.Integration.Entity
{

	public class CampaignJsonObject
	{
		public bool HasMore { get; set; }
		public string Offset { get; set; }
		public List<Campaign> Campaigns { get; set; }
	}
	public class Campaign
	{

		public int Id { get; set; }
		public long LastUpdatedTime { get; set; }
		public int AppId { get; set; }
		public string AppName { get; set; }

		public DateTime LastUpdatedDateTime { get; set; }

		public DateTime GetDateTimeFromTimeStamp(DateTime epoch , long lastUpdatedTime)
		{
			return epoch.AddMilliseconds(lastUpdatedTime);

		}
	}
}