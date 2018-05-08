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
    public class FilterGroup
    {
        public FilterGroup()
        {
            FilterGroupID = -1;
            FilterID = -1;
            SortOrder = -1;
            Name = string.Empty;
            ConditionCompareType = string.Empty;
            CreatedDate = null;
            UpdatedDate = null;
            CreatedUserID = null;
            UpdatedUserID = null;
            IsDeleted = null;
            CustomerID = null;
            FilterConditionList = null;
        }

        [DataMember]
        public int FilterGroupID { get; set; }
        [DataMember]
        public int FilterID { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ConditionCompareType { get; set; }
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

        public List<FilterCondition> FilterConditionList { get; set; }
    }
}
