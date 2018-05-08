using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    public interface IBillingProxy
    {
        IList<Billing> Get(int channelId, string customerId, int month, int year);
    }
}
