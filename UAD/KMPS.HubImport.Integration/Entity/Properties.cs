using Newtonsoft.Json;

namespace KMPS.HubImport.Integration.Entity
{
	[JsonObject]
	public class Properties
	{
		[JsonProperty("firstname")]	
		public FirstName FirstName { get; set; }

		[JsonProperty("lastname")]
		public LastName LastName { get; set; }

		[JsonProperty("company")]
		public Company Company { get; set; }

		[JsonProperty("lastmodifieddate")]
		public LastModifiedDate LastModifiedDate { get; set; }

		[JsonProperty("hs_email_optout")]
		public  Hs_email_optout HsEmailOptout { get; set; }
		[JsonProperty("hs_email_optout_409865")]
		public  Hs_email_optout_409865 HsEmailOptout409865 { get; set; }
		[JsonProperty("hs_email_optout_868669")]
		public  Hs_email_optout_868669 HsEmailOptout868669 { get; set; }
		[JsonProperty("hs_email_optout_868671")]
		public  Hs_email_optout_868671 HsEmailOptout868671 { get; set; }
		[JsonProperty("hs_email_optout_868680 ")]
		public  Hs_email_optout_868680 HsEmailOptout868680 { get; set; }
		[JsonProperty("hs_email_optout_868679")]
		public  Hs_email_optout_868679 HsEmailOptout868679 { get; set; }
		[JsonProperty("hs_email_optout_868677")]
		public  Hs_email_optout_868677 HsEmailOptout868677 { get; set; }
		[JsonProperty("hs_email_optout_868678")]
		public  Hs_email_optout_868678 HsEmailOptout868678 { get; set; }
		[JsonProperty("hs_email_optout_868676")]
		public  Hs_email_optout_868676 HsEmailOptout868676 { get; set; }
		[JsonProperty("hs_email_optout_868681")]
		public  Hs_email_optout_868681 HsEmailOptout868681 { get; set; }



	}

	
}