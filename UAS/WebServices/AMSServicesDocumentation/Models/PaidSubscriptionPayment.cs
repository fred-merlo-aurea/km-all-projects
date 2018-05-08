using System;
using System.Linq;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The PaidSubscriptionPayment object.
    /// </summary>
    [Serializable]
    [DataContract]
    public class PaidSubscriptionPayment
    {
        #region Properties
        /// <summary>
        /// Amount for the PaidSubscriptionPayment object.
        /// </summary>
        [DataMember]
        public double Amount { get; set; }
        /// <summary>
        /// Note for the PaidSubscriptionPayment object.
        /// </summary>
        [DataMember]
        public string Note { get; set; }
        /// <summary>
        /// Transaction ID for the PaidSubscriptionPayment object.
        /// </summary>
        [DataMember]
        public string TransactionId { get; set; }
	    /// <summary>
        /// Payment type for the PaidSubscriptionPayment object.
	    /// </summary>
        [DataMember]
        public FrameworkUAD_Lookup.Enums.PaymentType PaymentType { get; set; }
        #endregion
    }
}