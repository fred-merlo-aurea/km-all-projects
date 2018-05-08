using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models.EmailGroup
{
    [Serializable]
    [DataContract(Namespace = "")]
    [XmlRoot("ProfileCustomField")]
    public class ProfileCustomField
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
}