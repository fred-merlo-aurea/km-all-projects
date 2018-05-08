using System;
using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Activity.Report.Interfaces;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class ChampionAuditReportProxy : IChampionAuditReportProxy
    {
        public  IList<ECN_Framework_Entities.Activity.Report.ChampionAuditReport> Get(
            int customerId,
            DateTime startdate,
            DateTime enddate)
        {
            return ChampionAuditReport.Get(customerId, startdate, enddate);
        }
    }
}