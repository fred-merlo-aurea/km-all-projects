using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Activity
{
    [Serializable]
    [DataContract]
    public class BlastActivitySocial
    {
        public BlastActivitySocial()
        {
            SocialID = null;
            BlastID = null;
            EmailID = null;
            RefEmailID = null;
            SocialActivityCodeID = null;
            SocialActivityCode = string.Empty;
            ActionDate = null;
            URL = string.Empty;
            SocialMediaID = null;
            SocialMedia = string.Empty;
        }

        [DataMember]
        public int? SocialID { get; set; }
        [DataMember]
        public int? BlastID { get; set; }
        [DataMember]
        public int? EmailID { get; set; }
        [DataMember]
        public int? RefEmailID { get; set; }
        [DataMember]
        public int? SocialActivityCodeID { get; set; }
        [DataMember]
        public string SocialActivityCode { get; set; }
        [DataMember]
        public DateTime? ActionDate { get; set; }
        [DataMember]
        public string URL { get; set; }
        [DataMember]
        public int? SocialMediaID { get; set; }
        [DataMember]
        public string SocialMedia { get; set; }
    }
}
