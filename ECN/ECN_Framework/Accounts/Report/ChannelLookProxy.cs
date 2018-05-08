using System;
using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class ChannelLookProxy : IChannelLookProxy
    {
        public IList<ChannelLook> Get(int channelId, string customerId, string dateFrom, string dateTo, bool isTestBlast)
        {
            return ChannelLook.Get(channelId, customerId, dateFrom, dateTo, isTestBlast);
        }
    }
}
