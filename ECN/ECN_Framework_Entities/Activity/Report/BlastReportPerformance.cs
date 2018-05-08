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
    public class BlastReportPerformance
    {
        public BlastReportPerformance() 
        { 
        }
        
        public int BlastID { get; set; }
        public int Total { get; set; }
        public string ActionTypeCode { get; set; }
    }
}
