using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;
using ReportEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class AdvertiserClickReportProxy : IAdvertiserClickReportProxy
    {
        public IList<ReportEntities.AdvertiserClickReport> GetList(int groupId, DateTime startDate, DateTime endDate)
        {
            return AdvertiserClickReport.GetList(groupId, startDate, endDate);
        }
    }
}
