using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Accounts.Interfaces;
using ECN_Framework_Entities.Accounts.Report;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class TotalBlastsForDayProxy : ITotalBlastsForDayProxy
    {
        public IList<TotalBlastsForDay> GetReport(DateTime date)
        {
            return ECN_Framework_BusinessLayer.Accounts.TotalBlastsForDay.GetReport(date);
        }
    }
}
