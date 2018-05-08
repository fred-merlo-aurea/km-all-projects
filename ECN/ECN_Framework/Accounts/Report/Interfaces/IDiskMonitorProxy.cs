using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    public interface IDiskMonitorProxy
    {
        IList<DiskMonitor> Get(int channelId, int month, bool showOverLimit);
    }
}
