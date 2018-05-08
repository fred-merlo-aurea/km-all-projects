using Newtonsoft.Json;


namespace KM.Integration.Marketo
{
	[JsonObject]
	public class IdentityResponse
	{
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }
		[JsonProperty("token_type")]
		public string Tokentype { get; set; }
		[JsonProperty("expires_in")]
		public int Expiresin { get; set; }
		[JsonProperty("scope")]
		public string Scope { get; set; }
	}

}
