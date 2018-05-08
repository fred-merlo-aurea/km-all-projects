using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KMPS.HubImport.Integration.Entity
{
	[JsonObject]
	public class Contact
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public string Email { get; set; }

		public string PubCode { get; set; }

	}

	[JsonObject]
	public class IdentityProfile
	{
		[JsonProperty("identities")]
		public List<Identiy> Identities { get; set; }
	}

	[JsonObject]
	public class Identiy
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }
	}
}
