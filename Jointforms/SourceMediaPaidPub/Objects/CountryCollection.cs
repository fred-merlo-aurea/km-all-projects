using Newtonsoft.Json;

namespace SourceMediaPaidPub.Objects
{
	[JsonObject]
	public class CountryCollection
	{
		public Country[] Countries { get; set; }
	}

	public class Country
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("alpha-2")]
		public string ISOCode { get; set; }

		[JsonProperty("country-code")]
		public string Countrycode { get; set; }
	}

}