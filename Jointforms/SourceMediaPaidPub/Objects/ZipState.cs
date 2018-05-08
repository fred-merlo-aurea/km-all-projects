using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SourceMediaPaidPub.Objects
{
	[JsonObject]
	public class GoogleAPI
	{
		[JsonProperty("results")]
		public List<ZipState> ZipStates { get; set; }
		public string Status { get; set; }
	}

	[JsonObject]
	public class ZipState
	{
		[JsonProperty("address_components")]
		public List<AddressComponent> AddressComponents { get; set; }
		[JsonProperty("formatted_address")]
		public string FormattedAddress { get; set; }
	}

	
	[JsonObject]
	
	public class AddressComponent
	{
		[JsonProperty("long_name")]
		public string LongName { get; set; }

		[JsonProperty("short_name")]
		public string ShortName { get; set; }

		[JsonProperty("types")]
		public List<string> types { get; set; }
	}

}