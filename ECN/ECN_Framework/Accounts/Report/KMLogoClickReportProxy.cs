using System;
using System.Collections.Generic;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class KMLogoClickReportProxy : IKMLogoClickReportProxy
    {
        public IList<KMLogoClickReport> Get(DateTime dateFrom, DateTime dateTo)
        {
            return KMLogoClickReport.Get(dateFrom, dateTo);
        }
    }
}
