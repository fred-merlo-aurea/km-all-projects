using System;
using System.Linq;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The SubscriberProductDemographic object.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SubscriberProductDemographic
    {
        /// <summary>
        /// Name of the SubscriberProductDemographic object.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Value for the SubscriberProductDemographic object.
        /// </summary>
        [DataMember]
        public string Value { get; set; }
        /// <summary>
        /// DemoUpdateAction for the SubscriberProductDemographic object. 
        /// </summary>
        [IgnoreDataMember]
        public FrameworkUAD_Lookup.Enums.DemographicUpdate DemoUpdateAction { get; set; }
    }
}