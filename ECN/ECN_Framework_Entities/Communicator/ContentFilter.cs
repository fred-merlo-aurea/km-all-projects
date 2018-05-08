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
    public class ContentFilter
    {
        public ContentFilter()
        {
            FilterID = -1;
            LayoutID = null;
            SlotNumber = null;
            ContentID = null;
            GroupID = null;
            FilterName = string.Empty;
            WhereClause = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            ContentTitle = string.Empty;
            DetailList = null;
            CustomerID = null;
        }

        [DataMember]
        public int FilterID { get; set; }
        [DataMember]
        public int? LayoutID { get; set; }
        [DataMember]
        public int? SlotNumber { get; set; }
        [DataMember]
        public int? ContentID { get; set; }
        [DataMember]
        public int? GroupID { get; set; }
        [DataMember]
        public string FilterName { get; set; }
        [DataMember]
        public string WhereClause { get; set; }
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
        //optional
        [DataMember]
        public int? CustomerID { get; set; }
        public string ContentTitle { get; set; }
        public List<ContentFilterDetail> DetailList { get; set; }
    }
}
