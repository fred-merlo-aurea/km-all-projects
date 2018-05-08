using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models.EmailGroup
{
    [Serializable]
    [DataContract(Namespace = "")]
    [XmlRoot("ManageProfileAndTrigger")]
    public class ManageProfileAndTrigger : ManageProfile
    {
        /// <summary>
        /// The TriggerID
        /// </summary>
        [DataMember]
        public int TriggerID { get; set; }
    }
}