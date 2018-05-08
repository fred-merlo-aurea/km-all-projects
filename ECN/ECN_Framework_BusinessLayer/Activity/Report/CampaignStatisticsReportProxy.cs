using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using ReportEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class CampaignStatisticsReportProxy : ICampaignStatisticsReportProxy
    {
        public IList<ReportEntities.CampaignStatisticsReport> Get(
            int campaignId,
            DateTime startDate,
            DateTime endDate,
            int? groupId = null)
        {
            return CampaignStatisticsReport.Get(campaignId, startDate, endDate, groupId);
        }
    }
}
