using System;
using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class NoUsageProxy : INoUsageProxy
    {
        public IList<NoUsage> Get(int channelId, string customerId, string dateFrom, string dateTo)
        {
            return NoUsage.Get(channelId, customerId, dateFrom, dateTo);
        }
    }
}
