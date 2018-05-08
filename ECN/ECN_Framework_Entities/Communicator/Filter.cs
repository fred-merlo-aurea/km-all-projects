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
    public class Filter
    {
        public Filter()
        {
            FilterID = -1;
            CustomerID = null;
            GroupID = null;
            FilterName = string.Empty;
            WhereClause = string.Empty;
            DynamicWhere = string.Empty;
            GroupCompareType = string.Empty;
            CreatedDate = null;
            UpdatedDate = null;
            CreatedUserID = null;
            UpdatedUserID = null;
            IsDeleted = null;
            FilterGroupList = null;
            Archived = false;
        }

        [DataMember]
        public int FilterID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public int? GroupID { get; set; }
        [DataMember]
        public string FilterName { get; set; }
        [DataMember]
        public string WhereClause { get; set; }
        [DataMember]
        public string DynamicWhere { get; set; }
        [DataMember]
        public string GroupCompareType { get; set; }
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
        public bool? Archived { get; set; }

        public List<FilterGroup> FilterGroupList { get; set; }
    }
}
