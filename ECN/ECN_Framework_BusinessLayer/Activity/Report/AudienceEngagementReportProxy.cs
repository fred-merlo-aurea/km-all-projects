using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using ReportEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class AudienceEngagementReportProxy : IAudienceEngagementReportProxy
    {
        public IList<ReportEntities.AudienceEngagementReport> GetList(
            int groupId,
            int clickPercentage,
            int days,
            string download,
            string downloadType)
        {
            return AudienceEngagementReport.GetList(groupId, clickPercentage, days, download, downloadType);
        }
    }
}
