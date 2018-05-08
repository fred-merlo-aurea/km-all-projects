using System.Collections.Generic;
using Newtonsoft.Json;

namespace KMPS.HubImport.Integration.Entity
{
	[JsonObject]
	public class Hubspot
	{
		public string ClientName { get; set; }
		public bool IsEnabled { get; set; }
		public string API { get; set; }

		public string FtpSite { get; set; }

		public string FtpUser { get; set; }
		public string FtpPassword { get; set; }
		public string Pubcode { get; set; }
		public string LogPath { get; set; }
		public string MissFilePath { get; set; }
		public string BadFilePath { get; set; }

		[JsonProperty(PropertyName = "HubspotProcess")]
		public IList<HubspotProcess> Process { get; set; }

	}

	[JsonObject]
	public class HubspotProcess
	{
		public string ProcessType { get; set; }
		public string ProcessDuration { get; set; }
		public string ProcessDurationUnit { get; set; }
		public string ColumnHeaders { get; set; }

		public string FtpFolderLocation { get; set; }
	}
}
