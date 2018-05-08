using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class SocialShareChart
    {
        public static List<ECN_Framework_Entities.Activity.Report.SocialShareChart> GetChartSharesByBlastID(int blastID, int customerID)
        {
            return ECN_Framework_DataLayer.Activity.Report.SocialShareChart.GetChartSharesByBlastID(blastID, customerID);
        }

        public static List<ECN_Framework_Entities.Activity.Report.SocialShareChart> GetChartSharesByCampaignItemID(int campaignItemID, int customerID)
        {
            return ECN_Framework_DataLayer.Activity.Report.SocialShareChart.GetChartSharesByCampaignItemID(campaignItemID, customerID);
        }

        public static List<ECN_Framework_Entities.Activity.Report.SocialShareChart> GetChartPreviewsByBlastID(int blastID, int customerID)
        {
            return ECN_Framework_DataLayer.Activity.Report.SocialShareChart.GetChartPreviewsByBlastID(blastID, customerID);
        }

        public static List<ECN_Framework_Entities.Activity.Report.SocialShareChart> GetChartPreviewsByCampaignItemID(int campaignItemID, int customerID)
        {
            return ECN_Framework_DataLayer.Activity.Report.SocialShareChart.GetChartPreviewsByCampaignItemID(campaignItemID, customerID);
        }
    }
}
