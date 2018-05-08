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
    public class QuickTestBlastConfig
    {
        public QuickTestBlastConfig()
        {
            QTBCID = -1;
            IsDefault = null;
            BaseChannelID = null;
            BaseChannelDoesOverride = null;
            CustomerCanOverride = null;
            CustomerID = null;
            CustomerDoesOverride = null;
            AllowAdhocEmails = false;
            AutoCreateGroup = false;
            AutoArchiveGroup = false;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;            
        }

        [DataMember]
        public int QTBCID { get; set; }
        [DataMember]
        public bool? IsDefault { get; set; }
        [DataMember]
        public int? BaseChannelID { get; set; }
        [DataMember]
        public bool? BaseChannelDoesOverride { get; set; }
        [DataMember]
        public bool? CustomerCanOverride { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public bool? CustomerDoesOverride { get; set; }
        [DataMember]
        public bool? AllowAdhocEmails { get; set; }
        [DataMember]
        public bool? AutoCreateGroup { get; set; }
        [DataMember]
        public bool? AutoArchiveGroup { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
    }
}
