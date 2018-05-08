using System;
using System.Collections.Generic;

namespace SourceMediaPaidPub.Objects
{
	public class ECNLastYearAmountsTermDatesPremiumChecks
	{
		public List<Double> LastYearAmounts { get; set; }
		public List<DateTime> TermEndDates { get; set; }
		public bool IsPremium { get; set; }
	}
}