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
    [XmlRoot("ManageProfileWithDupeAndTrigger")]
    public class ManageProfileWithDupeAndTrigger : ManageProfileAndTrigger
    {
        /// <summary>
        /// The Composite Key
        /// </summary>
        [DataMember]
        public string CompositeKey { get; set; }
    }
}