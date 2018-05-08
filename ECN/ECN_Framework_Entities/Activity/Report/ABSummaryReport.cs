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
    public class ABSummaryReport
    {
        public ABSummaryReport() 
        { 
        }
        
        public DateTime SendTime { get; set; }
        public int BlastID { get; set; }
        public int SampleID { get; set; }
        public string CampaignItemName { get; set; }
        public string EmailSubject { get; set; }
        public int SendTotal { get; set; }
        public int Opens { get; set; }
        public int Clicks { get; set; }
        public int Bounce { get; set; }
        public int Unsubscribe { get; set; }
        public int Forward { get; set; }
        public int Delivered { get; set; }
        public string CampaignItemType { get; set; }
        public bool Winner { get; set; }
        public string LayoutName { get; set; }
        public string ABWinnerType { get; set; }
    }
}
