using System;
using System.Collections.Generic;
using System.Web;

namespace ecn.communicator.classes
{
    public class SF_CampaignMember
    {
        public SF_CampaignMember()
        {
            CampaignMemberId = string.Empty;
            CampaignId = string.Empty;
            ContactId = string.Empty;
            FirstRespondedDate = DateTime.MinValue;
            Status = string.Empty;
        }
        public string CampaignMemberId { get; set; }
        public string CampaignId { get; set; }
        public string ContactId { get; set; }
        public DateTime FirstRespondedDate { get; set; }
        public string Status { get; set; }
    }
}
