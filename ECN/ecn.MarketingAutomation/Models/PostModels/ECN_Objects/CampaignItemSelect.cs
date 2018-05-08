using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ecn.MarketingAutomation.Models.PostModels.ECN_Objects
{
    public class CampaignItemSelect
    {
        public int CustomerID { get; set; }
        public int CampaignItemID { get; set; }
        public string CampaignItemName { get; set; }

        public string Group { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string EmailSubject { get; set; }

        public string CampaignItemType { get; set; }

        public DateTime SendTime { get; set; }

        public string SendTimeUTC { get; set; }

        public int LayoutID { get; set; }

        public string LayoutName { get; set; }
    }
}