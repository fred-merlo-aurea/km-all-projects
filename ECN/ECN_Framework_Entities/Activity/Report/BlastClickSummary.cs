using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Activity.Report
{
    [Serializable]
    [DataContract]
    public class BlastClickSummary
    {
        public BlastClickSummary() { }

        public string CampaignItemName { get; set; }

        public string URL { get; set; }

        public int TotalSent { get; set; }

        public int TotalDelivered { get; set; }

        public int Open { get; set; }

        public int TotalCampaignClicks { get; set; }

        public int TotalClicks { get; set; }

        public int UniqueClicks { get; set; }

        public string IssueDate { get; set; }
        

    }
}
