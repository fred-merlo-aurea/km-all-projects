using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Activity.View
{
    [Serializable]
    [DataContract]
    public class BlastActivity
    {
        public BlastActivity()
        {
            EmailID = null;
            EmailAddress = string.Empty;
        }

        [DataMember]
        public int? EmailID { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public string ActionTypeCode { get; set; }
        [DataMember]
        public DateTime ActionDate { get; set; }
        [DataMember]
        public string ActionValue { get; set; }
    }
}
