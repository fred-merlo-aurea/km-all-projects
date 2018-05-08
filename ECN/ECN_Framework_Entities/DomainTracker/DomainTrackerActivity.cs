using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.DomainTracker
{
    [Serializable]
    [DataContract]
    public class DomainTrackerActivity
    {
        public DomainTrackerActivity()
        {
            DomainTrackerActivityID = -1;
            DomainTrackerID = -1;
            ProfileID = null;
            PageURL = string.Empty;
            TimeStamp = null;
            IPAddress = null;
            UserAgent = null;
            OS = null;
            Browser = null;
            ReferralURL = null;
            SourceBlastID = null;
        }

        [DataMember]
        public int DomainTrackerActivityID { get; set; }
        [DataMember]
        public int DomainTrackerID { get; set; }
        [DataMember]
        public int? ProfileID { get; set; }
        [DataMember]
        public string PageURL { get; set; }
        [DataMember]
        public DateTime? TimeStamp { get; set; }
        [DataMember]
        public string IPAddress { get; set; }
        [DataMember]
        public string UserAgent { get; set; }
        [DataMember]
        public string OS { get;  set; }
        [DataMember]
        public string Browser { get; set; }
        [DataMember]
        public string ReferralURL { get;  set; }
        [DataMember]
        public int? SourceBlastID { get; set; }
        [DataMember]
        public List<FieldsValuePair> FieldsValuePairsList { get; set; }
    }

  
}
