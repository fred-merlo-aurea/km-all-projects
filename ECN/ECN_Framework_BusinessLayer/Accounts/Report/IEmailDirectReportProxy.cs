using System;
using System.Data;

namespace ECN_Framework_BusinessLayer.Accounts.Report
{
    public interface IEmailDirectReportProxy
    {
        DataTable Get(int baseChannelId, string customerIds, DateTime startDate, DateTime endDate);
    }
}