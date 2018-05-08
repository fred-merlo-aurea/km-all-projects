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
    public class UnsubscribeReason
    {
        public UnsubscribeReason() { }

        [DataMember]
        public string SelectedReason { get; set; }
        [DataMember]
        public int UniqueCount { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
    }
}
