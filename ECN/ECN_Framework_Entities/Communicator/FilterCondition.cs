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
    public class FilterCondition
    {
        public FilterCondition()
        {
            FilterConditionID = -1;
            FilterGroupID = -1;
            SortOrder = -1;
            Field = string.Empty;
            FieldType = string.Empty;
            Comparator = string.Empty;
            CompareValue = string.Empty;
            NotComparator = null;
            DatePart = string.Empty;
            CreatedDate = null;
            UpdatedDate = null;
            CreatedUserID = null;
            UpdatedUserID = null;
            IsDeleted = null;
            CustomerID = null;
        }

        [DataMember]
        public int FilterConditionID { get; set; }
        [DataMember]
        public int FilterGroupID { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        [DataMember]
        public string Field { get; set; }
        [DataMember]
        public string FieldType { get; set; }
        [DataMember]
        public string Comparator { get; set; }
        [DataMember]
        public string CompareValue { get; set; }
        [DataMember]
        public int? NotComparator { get; set; }
        [DataMember]
        public string DatePart { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
    }
}
