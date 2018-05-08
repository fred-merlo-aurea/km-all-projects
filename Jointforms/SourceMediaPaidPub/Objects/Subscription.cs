using System;
using System.Collections.Generic;
using SourceMediaPaidPub.Process;

namespace SourceMediaPaidPub.Objects
{
	public class Subscription
	{
		public string PubCode { get; set; }
		public double TaxAmount { get; set; }
		public double TotalAmount { get; set; }
		public Dictionary<int, PremiumPubCodeDTO> PremiumPubCodeDTOs { get; set; }

		public DateTime CurrentSubscriptionEndDate { get; set; }
		public DateTime TermStartDate { get; set; }
		public DateTime TermEndDate { get; set; }

		public double ItemPrice { get; set; }
	}
}
