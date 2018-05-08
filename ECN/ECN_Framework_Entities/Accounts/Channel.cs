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
    public class Channel
    {
        public Channel() 
        {
            ChannelID = -1;
            BaseChannelID = null;
            ChannelName = string.Empty;
            AssetsPath = string.Empty;
            VirtualPath = string.Empty;
            PickupPath = string.Empty;
            HeaderSource = string.Empty;
            FooterSource = string.Empty;
            ChannelTypeCodeID = ECN_Framework_Common.Objects.Accounts.Enums.ChannelTypeCode.accounts;
            ChannelTypeCode = string.Empty;
            Active = string.Empty;
            MailingIP = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int ChannelID { get; set; }
        [DataMember]
        public int? BaseChannelID { get; set; }
        [DataMember]
        public string ChannelName { get; set; }
        [DataMember]
        public string AssetsPath { get; set; }
        [DataMember]
        public string VirtualPath { get; set; }
        [DataMember]
        public string PickupPath { get; set; }
        [DataMember]
        public string HeaderSource { get; set; }
        [DataMember]
        public string FooterSource { get; set; }
        [DataMember]
        public ECN_Framework_Common.Objects.Accounts.Enums.ChannelTypeCode ChannelTypeCodeID { get; set; }
        [DataMember]
        public string ChannelTypeCode { get; set; }
        [DataMember]
        public string Active { get; set; }
        [DataMember]
        public string MailingIP { get; set; }
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
    }
}
