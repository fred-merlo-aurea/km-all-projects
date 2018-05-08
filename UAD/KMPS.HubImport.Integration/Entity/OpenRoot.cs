using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMPS.HubImport.Integration.Entity
{

	public class OpenRoot
	{
		public bool HasMore { get; set; }
		public string Offset { get; set; }
		public Event[] Events { get; set; }
	}
}
