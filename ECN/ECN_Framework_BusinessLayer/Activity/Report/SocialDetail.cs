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
    public class SocialDetail
    {
        public static List<ECN_Framework_Entities.Activity.Report.SocialDetail> GetSocialDetailByBlastID(int blastID, int customerID)
        {
            return ECN_Framework_DataLayer.Activity.Report.SocialDetail.GetSocialDetailByBlastID(blastID, customerID);
        }

        public static List<ECN_Framework_Entities.Activity.Report.SocialDetail> GetSocialDetailByCampaignItemID(int campaignItemID, int customerID)
        {
            return ECN_Framework_DataLayer.Activity.Report.SocialDetail.GetSocialDetailByCampaignItemID(campaignItemID, customerID);
        }
    }
}
