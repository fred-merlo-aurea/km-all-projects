using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The PaidSubscriptionProductDemographic object
    /// </summary>
    [Serializable]
    [DataContract]
    public class PaidSubscriptionProductDemographic
    {
        #region Properties
        /// <summary>
        /// Product code for the PaidSubscriptionProductDemographic object.
        /// </summary>
        [DataMember]
        public string ProductCode { get; set; }
        /// <summary>
        /// Demographic name of the PaidSubscriptionProductDemographic object.
        /// </summary>
        [DataMember]
        public string DemographicName { get; set; }
        /// <summary>
        /// OtherTextValue for the PaidSubscriptionProductDemographic object.
        /// </summary>
        [DataMember]
        public string OtherTextValue { get; set; }
        /// <summary>
        /// List of values for the PaidSubscriptionProductDemographic object.
        /// </summary>
        [DataMember]
        public List<string> Values { get; set; }
        #endregion
    }
}