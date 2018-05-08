using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KMPS.HubImport.Integration.Entity
{
	[JsonObject]
	public class FirstName : ValueKey
	{
	}

	[JsonObject]
	public class LastName : ValueKey
	{
	}

	[JsonObject]
	public class Company : ValueKey
	{
	}

	[JsonObject]
	public class LastModifiedDate : ValueKey
	{
	}
	public class Hs_email_optout_868681:ValueKey
	{
	}

	public class Hs_email_optout_868676:ValueKey
	{
	}

	public class Hs_email_optout_868678:ValueKey
	{
	}

	public class Hs_email_optout_868677:ValueKey
	{
	}

	public class Hs_email_optout_868679:ValueKey
	{
	}

	public class Hs_email_optout_868680:ValueKey
	{
	}

	public class Hs_email_optout_868671:ValueKey
	{
	}

	public class Hs_email_optout_868669:ValueKey
	{
	}

	public class Hs_email_optout_409865:ValueKey
	{
	}

	public class Hs_email_optout:ValueKey
	{
	}

	[JsonObject]
	public abstract class ValueKey
	{
		[JsonProperty("value")]
		public virtual string Value { get; set; }
	}

}
