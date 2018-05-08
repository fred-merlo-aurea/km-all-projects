using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class LandingPageAssignContent
    {
        public LandingPageAssignContent()
        {
            LPACID = -1;
            LPAID = null;
            LPOID = null;
            Display = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int LPACID { get; set; }
        [DataMember]
        public int? LPAID { get; set; }
        [DataMember]
        public int? LPOID { get; set; }
        [DataMember]
        public string Display { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public int? SortOrder { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
    }
}
