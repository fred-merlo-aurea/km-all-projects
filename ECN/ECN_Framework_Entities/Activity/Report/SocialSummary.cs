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
    public class SocialSummary
    {
        public SocialSummary()
        {
            //BlastID = null;
            ReportImagePath = string.Empty;
            Share = null;
            Click = null;
            ID = null;
            IsBlastGroup = null;
            ReportPath = string.Empty;
        }

        [DataMember]
        public int? ID { get; set; }
        //[DataMember]
        //public int? BlastID { get; set; }
        [DataMember]
        public string ReportImagePath { get; set; }
        [DataMember]
        public int? Share { get; set; }
        [DataMember]
        public int? Click { get; set; }
        [DataMember]
        public bool? IsBlastGroup { get; set; }
        [DataMember]
        public string ReportPath { get; set; }

        [DataMember]
        public int SocialMediaID { get; set; }
    }
}
