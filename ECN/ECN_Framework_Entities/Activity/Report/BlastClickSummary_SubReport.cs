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
    public class BlastClickSummary_SubReport
    {
       
        public BlastClickSummary_SubReport() { }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime IssueDate { get; set; }

        [DataMember]
        public int TotalSent { get; set; }

        [DataMember]
        public int TotalDelivered { get; set; }

        [DataMember]
        public int Open { get; set; }

        [DataMember]
        public int TotalClicks { get; set; }

    }
}
