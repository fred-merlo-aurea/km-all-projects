using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMPS.HubImport.Integration.Entity
{
	public class Click
	{
		public string Pubcode { get; set; }
		public string EmailAddress { get; set; }
		public DateTime ClickTime { get; set; }
		public string ClickURL { get; set; }
		public string Alias { get; set; }
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
