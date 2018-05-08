using System;
using System.Linq;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The SubscriberConsensusDemographic object.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SubscriberConsensusDemographic
    {
        /// <summary>
        /// Name of the SubscriberConsensusDemographic object.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Display name of the SubscriberConsensusDemographic object.
        /// </summary>
        [DataMember]
        public string DisplayName { get; set; }
        /// <summary>
        /// Value for the SubscriberConsensusDemographic object.
        /// </summary>
        [DataMember]
        public string Value { get; set; }
    }
}
