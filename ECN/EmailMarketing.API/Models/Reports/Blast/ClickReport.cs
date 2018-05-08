using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace EmailMarketing.API.Models.Reports.Blast
{
    [Serializable]
    [DataContract]
    [XmlRoot("ClickReport")]
    public class ClickReport
    {
        public ClickReport()
        {
            ClickTime = string.Empty;
            EmailAddress = string.Empty;
            Link = string.Empty;
        }

        [DataMember]
        [XmlElement]
        public string ClickTime;

        [DataMember]
        [XmlElement]
        public string EmailAddress;

        [DataMember]
        [XmlElement]
        public string Link;
    }
}