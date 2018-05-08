using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization;


namespace EmailMarketing.API.Models.Reports.Blast
{
    [Serializable]
    [XmlRoot("BlastReport")]
    [DataContract]
    public class BlastReport
    {
        public BlastReport()
        {
            Sends = null;
            Clicks = null;
            Bounces = null;
            Opens = null;
            Resends = null;
        }

        [XmlElement]
        [DataMember]
        public ReportRow Sends;

        [XmlElement]
        [DataMember]
        public ReportRow Clicks;

        [XmlElement]
        [DataMember]
        public ReportRow Bounces;

        [XmlElement]
        [DataMember]
        public ReportRow Opens;

        [XmlElement]
        [DataMember]
        public ReportRow Resends;


        public bool HasSends { get { return Sends != null; } }
    }
}