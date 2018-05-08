using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace EmailMarketing.API.Models.EmailGroup
{
    [Serializable]
    [DataContract]
    [XmlRoot("SubscriptionResult")]
    public class SubscriptionResult
    {
        // NOTE: no default constructor, all properties have inferable default values
        public SubscriptionResult() {
        
        }
        #region properties
        [XmlElement]
        [DataMember]
        public int? GroupID { get; set; }
        [XmlElement]
        [DataMember]
        public int? EmailID { get; set; }
        [XmlElement]
        [DataMember]
        public string EmailAddress { get; set; }
        [XmlElement]
        [DataMember]
        public SubscribeTypes Status { get; set; }
        [XmlElement]
        [DataMember]
        public Statuses Result { get; set; }

        #endregion properties
    }
}