using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class ChampionAuditReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.ChampionAuditReport> Get(int customerID, DateTime startdate, DateTime enddate)
        {
            return ECN_Framework_DataLayer.Activity.Report.ChampionAuditReport.Get(customerID, startdate, enddate);
        }
    }
}