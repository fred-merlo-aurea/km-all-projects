using System;
using System.Data;

namespace ECN_Framework_BusinessLayer.Accounts.Report
{
    [Serializable]
    public class EmailDirectReportProxy : IEmailDirectReportProxy
    {
        public DataTable Get(int baseChannelId, string customerIds, DateTime startDate, DateTime endDate)
        {
            return EmailDirectReport.Get(baseChannelId, customerIds, startDate, endDate);
        }
    }
}
