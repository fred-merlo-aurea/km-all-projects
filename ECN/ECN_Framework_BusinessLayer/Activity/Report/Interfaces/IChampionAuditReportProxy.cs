using System;
using System.Collections.Generic;

namespace ECN_Framework_BusinessLayer.Activity.Report.Interfaces
{
    public interface IChampionAuditReportProxy
    {
        IList<ECN_Framework_Entities.Activity.Report.ChampionAuditReport> Get(
            int customerId,
            DateTime startdate,
            DateTime enddate);
    }
}
