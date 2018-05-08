using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using System.Xml.Serialization;

namespace ECN_Framework_Entities.Activity.Report
{
    [Serializable]
    [DataContract]
    public class PlatformDetail
    {
        public PlatformDetail() 
        { 
        }

        public string PlatformName { get; set; }
        [XmlElement(ElementName = "Opens")]
        public int Column1 { get; set; }
        public string Usage { get; set; }
        public string EmailClientName { get; set; }
    }
}
