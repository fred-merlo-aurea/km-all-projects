using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace EmailMarketing.API.Models.EmailGroup
{
    [DataContract]
    [XmlRoot("SubscribeTypes")]
    public enum SubscribeTypes
    {

        /// <summary>
        /// Short hand for "Subscribe"
        /// </summary>
        [EnumMember]
        [XmlEnum]
        S = 1,
        /// <summary>For requests, directs the API to add the subscriber to the Group.  For result, 
        /// indicates the subscriber is subscribed to the group</summary>
        [EnumMember]
        [XmlEnum]
        Subscribe = 1,

        /// <summary>For requests, directs the API to unsubscribe the subscriber from the Group.  For result, 
        /// indicates the subscriber is subscribed to the group</summary>
        [EnumMember]
        [XmlEnum]
        Unsubscribe = 2,
        /// <summary>Shorthand for "Unsubscribe"</summary>
        [EnumMember]
        [XmlEnum]
        U = 2,

        /// <summary>Shorthand for "Pending"</summary>
        [EnumMember]
        [XmlEnum]
        P = 3,
        /// <summary>Legacy, do not use.  Previously used to indicate a subscription event was pending.</summary>
        [EnumMember]
        [XmlEnum]
        Pending = 3,

        /// <summary>Shorthand for "MasterSupress"</summary>
        [EnumMember]
        [XmlEnum]
        M = 4,
        /// <summary>For requests, directs the API to add the subscriber to the Master Suppression group.  
        /// For result, indicates the subscriber is a member of the Master Suppression group.</summary>
        [EnumMember]
        [XmlEnum]
        MasterSuppress = 4
    }
}