using System;
using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class DiskMonitorProxy : IDiskMonitorProxy
    {
        public IList<DiskMonitor> Get(int channelId, int month, bool showOverLimit)
        {
            return DiskMonitor.Get(channelId, month, showOverLimit);
        }
    }
}
