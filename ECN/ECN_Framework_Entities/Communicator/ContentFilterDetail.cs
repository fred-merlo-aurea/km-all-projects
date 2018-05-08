using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class ContentFilterDetail
    {
        public ContentFilterDetail()
        {
            FDID = -1;
            FilterID = null;
            FieldType = string.Empty;
            CompareType = string.Empty;
            FieldName = string.Empty;
            Comparator = string.Empty;
            CompareValue = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
        }

        [DataMember]
        public int FDID { get; set; }
        [DataMember]
        public int? FilterID { get; set; }
        [DataMember]
        public string FieldType { get; set; }
        [DataMember]
        public string CompareType { get; set; }
        [DataMember]
        public string FieldName { get; set; }
        [DataMember]
        public string Comparator { get; set; }
        [DataMember]
        public string CompareValue { get; set; }
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
