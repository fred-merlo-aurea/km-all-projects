using System;
using System.Collections.Generic;

namespace ECN_Framework_BusinessLayer.Accounts.Interfaces
{
    public interface ITotalBlastsForDayProxy
    {
        IList<ECN_Framework_Entities.Accounts.Report.TotalBlastsForDay> GetReport(DateTime date);
    }
}
