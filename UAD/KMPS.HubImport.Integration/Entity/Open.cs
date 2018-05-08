using System;

namespace KMPS.HubImport.Integration.Entity
{
	public class Open
	{
		public string Pubcode { get; set; }
		public string EmailAddress { get; set; }
		public DateTime OpenTime { get; set; }
		public int BlastId { get; set; }
		public string Subject { get; set; }
		public DateTime SendTime { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }

	}
}
