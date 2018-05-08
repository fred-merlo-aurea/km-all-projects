using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models
{
    [Serializable]
    [DataContract]
    [XmlRoot("Int")]
    public class IntResult
    {
        public IntResult() { }
        [DataMember]
        [XmlElement]
        public int Result { get; set; }
    }
}