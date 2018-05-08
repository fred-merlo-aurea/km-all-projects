using System;
using System.Collections.Generic;
using ReportEntities = ECN_Framework_Entities.Activity.Report;

namespace ECN_Framework_BusinessLayer.Activity.Report.Interfaces
{
    public interface IAdvertiserClickReportProxy
    {
        IList<ReportEntities.AdvertiserClickReport> GetList(int groupId, DateTime startDate, DateTime endDate);
    }
}