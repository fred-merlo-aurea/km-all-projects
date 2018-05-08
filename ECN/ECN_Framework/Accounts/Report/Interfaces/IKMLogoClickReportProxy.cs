using System;
using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    public interface IKMLogoClickReportProxy
    {
        IList<KMLogoClickReport> Get(DateTime dateFrom, DateTime dateTo);
    }
}
