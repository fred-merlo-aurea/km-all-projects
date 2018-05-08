using System;
using System.Collections.Generic;
using ReportEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_BusinessLayer.Activity.Report.Interfaces
{
    public interface ICampaignStatisticsReportProxy
    {
        IList<ReportEntities.CampaignStatisticsReport> Get(
            int campaignId,
            DateTime startDate,
            DateTime endDate,
            int? groupId = null);
    }
}