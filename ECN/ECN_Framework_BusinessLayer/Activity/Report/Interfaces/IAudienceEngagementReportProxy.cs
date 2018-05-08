using System.Collections.Generic;
using ReportEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_BusinessLayer.Activity.Report.Interfaces
{
    public interface IAudienceEngagementReportProxy
    {
        IList<ReportEntities.AudienceEngagementReport> GetList(
            int groupId,
            int clickPercentage,
            int days,
            string download,
            string downloadType);
    }
}
