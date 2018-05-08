using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SourceMediaPaidPub.Objects
{

//	[JsonObject]
//	public class Rootobject
//	{
//		
//	}
//
//	[JsonObject]
//	public class Rate
//	{
//		[JsonProperty("zip")]
//		public string Zip { get; set; }
//		public string State { get; set; }
//		[JsonProperty("state_rate")]
//		public string StateRate { get; set; }
//		[JsonProperty("county")]
//		public string County { get; set; }
//		[JsonProperty("county_rate")]
//		public string CountyRate { get; set; }
//		[JsonProperty("city")]
//		public string City { get; set; }
//		[JsonProperty("city_rate")]
//		public string CityRate { get; set; }
//		[JsonProperty("combined_district_rate ")]
//		public string CombinedDistrictRate { get; set; }
//
//		[JsonProperty("combined_rate")]
//		public string CombinedRate { get; set; }
//
//		[JsonProperty("freight_taxable")]
//		public bool FreightTaxable { get; set; }
//		
//		[JsonProperty("country")]
//		public string Country { get; set; }
//	}





	[JsonObject]
	public class Taxjar
	{
		[JsonProperty("rate")]
		public Rate Rate { get; set; }
	}

	[JsonObject]
	public class Rate
	{
		[JsonProperty("zip")]
		public string Zip { get; set; }
		public string State { get; set; }
		[JsonProperty("state_rate")]
		public string StateRate { get; set; }
		[JsonProperty("county")]
		public string County { get; set; }
		[JsonProperty("county_rate")]
		public string CountyRate { get; set; }
		[JsonProperty("city")]
		public string City { get; set; }
		[JsonProperty("city_rate")]
		public string CityRate { get; set; }
		[JsonProperty("combined_district_rate")]
		public string CombinedDistrictRate { get; set; }

		[JsonProperty("combined_rate")]
		public string CombinedRate { get; set; }

		[JsonProperty("freight_taxable")]
		public bool FreightTaxable { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }
	}

}