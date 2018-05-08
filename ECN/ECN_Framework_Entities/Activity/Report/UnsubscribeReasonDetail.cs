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
    public class UnsubscribeReasonDetail
    {
        public UnsubscribeReasonDetail() { }

        [DataMember]
        public string CampaignItemName { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public DateTime UnsubscribeTime { get; set; }
        [DataMember]
        public string SelectedReason { get; set; }
    }
}
