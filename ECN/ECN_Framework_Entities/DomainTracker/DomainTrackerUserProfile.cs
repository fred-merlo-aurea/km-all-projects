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
    public class DomainTrackerUserProfile
    {
        public DomainTrackerUserProfile()
        {
            ProfileID = -1;
            BaseChannelID = -1;
            EmailAddress = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            ProfileCount = 0;
        }

        [DataMember]
        public int ProfileID { get; set; }
        [DataMember]
        public int BaseChannelID { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
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
        public int ProfileCount { get; set; }

        [DataMember]
        public DateTime? ConvertedDate { get; set; }

        [DataMember]
        public bool? IsKnown { get; set; }
    }
}
