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
    public class SocialMedia
    {
        public SocialMedia()
        {
            SocialMediaID = -1;
            DisplayName = string.Empty;
            IsActive = null;
            MatchString = string.Empty;
            ImagePath = string.Empty;
            ShareLink = string.Empty;
            CanShare = null;
            CanPublish = null;
            DateAdded = null;
            ReportImagePath = string.Empty;
        }

        [DataMember]
        public int SocialMediaID { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public string MatchString { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
        [DataMember]
        public string ShareLink { get; set; }
        [DataMember]
        public bool? CanShare { get; set; }
        [DataMember]
        public bool? CanPublish { get; set; }
        [DataMember]
        public DateTime? DateAdded { get; set; }
        [DataMember]
        public string ReportImagePath { get; set; }
    }
}
