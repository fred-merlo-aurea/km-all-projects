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
    public class LandingPageAssign
    {
        public LandingPageAssign()
        {
            LPAID = -1;
            LPID = null;
            IsDefault = null;
            BaseChannelID = null;
            CustomerCanOverride = null;
            CustomerID = null;
            CustomerDoesOverride = null;
            Label = string.Empty;
            Header = string.Empty;
            Footer = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            BaseChannelDoesOverride = null;
        }

        [DataMember]
        public int LPAID { get; set; }
        [DataMember]
        public int? LPID { get; set; }
        [DataMember]
        public bool? IsDefault { get; set; }
        [DataMember]
        public int? BaseChannelID { get; set; }
        [DataMember]
        public bool? CustomerCanOverride { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public bool? CustomerDoesOverride { get; set; }
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public string Header { get; set; }
        [DataMember]
        public string Footer { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public bool? BaseChannelDoesOverride { get; set; }
        //optional
        public List<LandingPageAssignContent> AssignContentList { get; set; }
    }
}
