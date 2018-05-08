using System;
using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class BillingProxy : IBillingProxy
    {
        public IList<Billing> Get(int channelId, string customerId, int month, int year)
        {
            return Billing.Get(channelId, customerId, month, year);
        }
    }
}
