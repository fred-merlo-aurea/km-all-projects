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
    public class DomainTrackerFields
    {
        public DomainTrackerFields()
        {
            DomainTrackerFieldsID = -1;
            DomainTrackerID = -1;
            FieldName = string.Empty;
            Source = string.Empty;
            SourceID = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int DomainTrackerFieldsID { get; set; }
        [DataMember]
        public int DomainTrackerID { get; set; }
        [DataMember]
        public string FieldName { get; set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public string SourceID { get; set; }
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
