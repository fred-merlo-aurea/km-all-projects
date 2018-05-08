using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Publisher
{
    [Serializable]
    [DataContract]
    public class EditionHistory
    {
        public EditionHistory()
        {
            EditionHistoryID = -1;
            EditionID = null;
            ActivatedDate = null;
            ArchievedDate = null;
            DeActivatedDate = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
        }

        [DataMember]
        public int EditionHistoryID { get; set; }
        [DataMember]
        public int? EditionID { get; set; }
        [DataMember]
        public DateTime? ActivatedDate { get; set; }
        [DataMember]
        public DateTime? ArchievedDate { get; set; }
        [DataMember]
        public DateTime? DeActivatedDate { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
    }
}
