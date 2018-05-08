using System;
using Newtonsoft.Json;

namespace SourceMediaPaidPub.Objects
{
	[JsonObject]
	public class Price
	{
		public string Term { get; set; }
		[JsonProperty("type")]
		public PriceType Type { get; set; }
		public string Country { get; set; }

		public float Value { get; set; }
		public enum PriceType
		{
			Standard, Premium

		}
	}
}

