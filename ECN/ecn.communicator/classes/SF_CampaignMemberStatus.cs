using System;
using System.Collections.Generic;
using System.Web;

namespace ecn.communicator.classes
{
    public class SF_CampaignMemberStatus
    {
        public SF_CampaignMemberStatus()
        {
            CampaignMemberStatusId = string.Empty;
            CampaignId = string.Empty;
            HasResponded = false;
            IsDefault = false;
            Label = string.Empty;
            SortOrder = -1;
        }
        public string CampaignMemberStatusId { get; set; }
        public string CampaignId { get; set; }
        public bool HasResponded { get; set; }
        public bool IsDefault { get; set; }
        public string Label { get; set; }
        public int SortOrder { get; set; }

       
       
       
    }
}
