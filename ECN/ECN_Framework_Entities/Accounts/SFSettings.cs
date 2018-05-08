using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class SFSettings
    {
        public SFSettings() 
        {
            SFSettingsID = -1;
            CustomerID = null;
            BaseChannelID = null;
            CustomerCanOverride = null;
            CustomerDoesOverride = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            RefreshToken = string.Empty;
            SandboxMode = false;
            PushChannelMasterSuppression = false;
            ConsumerSecret = string.Empty;
            ConsumerKey = string.Empty;
        }

        [DataMember]
        public int SFSettingsID { get; set; }
        [DataMember]
        public int? BaseChannelID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public bool? CustomerCanOverride { get; set; }
        [DataMember]
        public bool? CustomerDoesOverride { get; set; }
        [DataMember]
        public string RefreshToken { get; set; }
        [DataMember]
        public string ConsumerSecret { get; set; }
        [DataMember]
        public string ConsumerKey { get; set; }
        [DataMember]
        public bool? PushChannelMasterSuppression { get; set; }
        [DataMember]
        public bool? SandboxMode { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        
    }
}
