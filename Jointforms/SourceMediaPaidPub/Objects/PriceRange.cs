using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SourceMediaPaidPub.Objects
{
	[JsonObject]
	public class PriceRange
	{
		[JsonProperty("PaidFrom")]
		public double PaidFrom { get; set; }

		[JsonProperty("PaidTo")]
		public double PaidTo { get; set; }

		[JsonProperty("Price")]
		public IEnumerable<Price> PriceCollection { get; set; }
		
	}
}