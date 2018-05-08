using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models.Reports.Blast
{
    [Serializable]
    [DataContract]
    [XmlRoot("ISPReport")]
    public class ISPReport
    {
        public ISPReport()
        {
            ISPs = string.Empty;
            Sends = string.Empty;
            Opens = string.Empty;
            Clicks = string.Empty;
            Bounces = string.Empty;
            Unsubscribes = string.Empty;
            Resends = string.Empty;
            Forwards = string.Empty;
            FeedbackUnsubs = string.Empty;
        }

        [XmlElement]
        [DataMember]
        public string ISPs;

        [XmlElement]
        [DataMember]
        public string Sends;

        [XmlElement]
        [DataMember]
        public string Opens;

        [XmlElement]
        [DataMember]
        public string Clicks;

        [XmlElement]
        [DataMember]
        public string Bounces;

        [XmlElement]
        [DataMember]
        public string Unsubscribes;

        [XmlElement]
        [DataMember]
        public string Resends;

        [XmlElement]
        [DataMember]
        public string Forwards;

        [XmlElement]
        [DataMember]
        public string FeedbackUnsubs;
    }
}