using System;
using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class ECNTodayProxy : IECNTodayProxy
    {
        public IList<ECNToday> Get(int month, int year, bool isTestBlast)
        {
            return ECNToday.Get(month, year, isTestBlast);
        }
    }
}
