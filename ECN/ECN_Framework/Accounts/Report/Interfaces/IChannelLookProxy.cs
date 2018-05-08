using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    public interface IChannelLookProxy
    {
        IList<ChannelLook> Get(int channelId, string customerId, string dateFrom, string dateTo, bool isTestBlast);
    }
}
