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
    public class SocialSummary
    {
        public static List<ECN_Framework_Entities.Activity.Report.SocialSummary> GetSocialSummaryByBlastID(int blastID, int customerID)
        {
            return ECN_Framework_DataLayer.Activity.Report.SocialSummary.GetSocialSummaryByBlastID(blastID, customerID);
        }

        public static List<ECN_Framework_Entities.Activity.Report.SocialSummary> GetSocialSummaryByCampaignItemID(int campaignItemID, int customerID)
        {
            return ECN_Framework_DataLayer.Activity.Report.SocialSummary.GetSocialSummaryByCampaignItemID(campaignItemID, customerID);
        }
    }
}
