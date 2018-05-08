using System;
using System.Linq;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The PaidBundleItem object.
    /// </summary>
    [Serializable]
    [DataContract]
    public class PaidBundleItem
    {
        #region Properties
        /// <summary>
        /// Name of the PaidBundleItem object.
        /// </summary>
        [DataMember]
        public string BundleName { get; set; }

        /// <summary>
        /// Price for the PaidBundleItem object.
        /// </summary>
        [DataMember]
        public double Price { get; set; }
        #endregion
    }
}