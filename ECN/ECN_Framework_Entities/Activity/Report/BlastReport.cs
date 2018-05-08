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
    public class BlastReport
    {
        public BlastReport() 
        { 
        }
        
        public int BlastID { get; set; }
        public string EmailSubject { get; set; }
        public string EmailFromName { get; set; }
        public string EmailFrom { get; set; }
        public string GroupName { get; set; }
        public string FilterName { get; set; }
        public string LayoutName { get; set; }
        public DateTime SendTime { get; set; }
        public DateTime FinishTime { get; set; }
        public int SuccessTotal { get; set; }
        public int SendTotal { get; set; }
        public string SetupCost { get; set; }
        public string OutboundCost { get; set; }
        public string InboundCost { get; set; }
        public string DesignCost { get; set; }
        public string OtherCost { get; set; }

        public string SuppressionGroups { get; set; }

        public string SuppressionGroupFilters { get; set; }
    }
}
