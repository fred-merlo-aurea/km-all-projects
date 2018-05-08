using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    public interface IECNTodayProxy
    {
        IList<ECNToday> Get(int month, int year, bool isTestBlast);
    }
}
