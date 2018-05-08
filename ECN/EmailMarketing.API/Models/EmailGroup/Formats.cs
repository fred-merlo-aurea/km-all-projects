using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models.EmailGroup
{
    [DataContract]
    [XmlRoot("Formats")]
    public enum Formats
    {
        [EnumMember]
        HTML,
        [EnumMember]
        Text
    }
}