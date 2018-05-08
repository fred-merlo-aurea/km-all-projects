using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    public interface INoUsageProxy
    {
        IList<NoUsage> Get(int channelId, string customerId, string dateFrom, string dateTo);
    }
}
