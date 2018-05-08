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
    public class BlastComparision
    {
        public BlastComparision() 
        { 
        }

        
        [DataMember]
        public string ActionTypeCode { get; set; }
        [DataMember]
        public string BlastID { get; set; }
        [DataMember]
        public int DistinctCount { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public DateTime SendTime { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public int TotalSent { get; set; }
        [DataMember]
        public string Month { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public float Perc { get; set; }
    }
}
