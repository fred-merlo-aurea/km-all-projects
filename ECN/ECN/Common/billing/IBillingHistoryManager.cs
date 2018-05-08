using System;
using ecn.common.classes;
using ecn.common.classes.billing;

namespace ecn.common.classes.billing
{
	public interface IBillingHistoryManager
	{
		BillItem GetLatestBillItem(QuoteItem quoteItem);	
	}
}
