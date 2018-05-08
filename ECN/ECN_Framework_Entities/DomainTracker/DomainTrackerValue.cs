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
    public class DomainTrackerValue
    {
        public DomainTrackerValue()
        {
            DomainTrackerValueID = -1;
            DomainTrackerFieldsID = -1;
            DomainTrackerActivityID = -1;
            Value = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int DomainTrackerValueID { get; set; }
        [DataMember]
        public int DomainTrackerFieldsID { get; set; }
        [DataMember]
        public int DomainTrackerActivityID { get; set; }
        [DataMember]
        public string Value { get; set; }
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
    }
}
