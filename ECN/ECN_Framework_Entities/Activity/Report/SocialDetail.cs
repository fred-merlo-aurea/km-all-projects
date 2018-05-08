using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Activity.Report
{
    [Serializable]
    [DataContract]
    public class SocialDetail
    {
        public SocialDetail()
        {
            BlastID = null;
            EmailAddress = string.Empty;
            Click = null;
            DisplayName = string.Empty;
        }

        [DataMember]
        public int? BlastID { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public int? Click { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public int? SocialMediaID { get; set; }
    }
}
