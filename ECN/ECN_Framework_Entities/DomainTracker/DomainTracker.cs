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
    public class DomainTracker
    {
        public DomainTracker()
        {
            DomainTrackerID = -1;
            BaseChannelID = -1;
            TrackerKey = string.Empty;
            Domain = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            TrackAnonymous = false;
        }

        [DataMember]
        public int DomainTrackerID { get; set; }
        [DataMember]
        public int BaseChannelID { get; set; }
        [DataMember]
        public string TrackerKey { get; set; }
        [DataMember]
        public string Domain { get; set; }     
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool? IsDeleted { get; set; }

        [DataMember]
        public bool TrackAnonymous { get; set; }
    }
}
