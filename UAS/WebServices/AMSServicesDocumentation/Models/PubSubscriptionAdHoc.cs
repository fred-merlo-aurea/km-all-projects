using System;
using System.Linq;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The PubSubscriptionAdhoc object.
    /// </summary>
    [Serializable]
    [DataContract]
    public class PubSubscriptionAdHoc
    {
        /// <summary>
        /// AdHocField for the PubSubscriptionAdhoc object.
        /// </summary>
        [DataMember]
        public string AdHocField { get; set; }
        /// <summary>
        /// AdhocValue for the PubSubscriptionAdhoc object.
        /// </summary>
        [DataMember]
        public string Value { get; set; }
    }
}