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
    [XmlRoot("OpensReport")]
    public class OpensReport
    {
        public OpensReport()
        {
            OpenTime = string.Empty;
            EmailAddress = string.Empty;
            Info = string.Empty;
        }

        [XmlElement]
        [DataMember]
        public string OpenTime;

        [XmlElement]
        [DataMember]
        public string EmailAddress;

        [XmlElement]
        [DataMember]
        public string Info;
    }
}