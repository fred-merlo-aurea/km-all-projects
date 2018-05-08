using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KM.Integration.Marketo
{
	[JsonObject]

	public class MarketoListResponse
	{
		public string requestId { get; set; }
		public bool success { get; set; }

		[JsonProperty("result")]
		public SearchResult[] result { get; set; }

		public string nextPageToken { get; set; }
	}

	public class SearchResult
	{
		public int id { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public string programName { get; set; }
		public DateTime createdAt { get; set; }
		public DateTime updatedAt { get; set; }

		public string email { get; set; }

		public string firstName { get; set; }

		public string lastName { get; set; }
	}

}
