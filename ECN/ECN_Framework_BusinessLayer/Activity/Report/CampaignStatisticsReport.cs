using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;
using System.Transactions;


namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class CampaignStatisticsReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.CampaignStatisticsReport> Get(int CampaignID, DateTime startdate, DateTime enddate, int? GroupID = null)
        {
            return ECN_Framework_DataLayer.Activity.Report.CampaignStatisticsReport.Get(CampaignID, startdate, enddate, GroupID);
        }

        public static DataTable GetDT(int CampaignID, DateTime startdate, DateTime enddate, int? GroupID = null)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.Report.CampaignStatisticsReport.GetDT(CampaignID, startdate, enddate, GroupID);
                scope.Complete();
            }
            return dt;
        }
    }
}
