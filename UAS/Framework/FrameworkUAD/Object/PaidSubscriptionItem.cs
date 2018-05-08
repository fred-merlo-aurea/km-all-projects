using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class PaidSubscriptionItem
    {
        public PaidSubscriptionItem() 
        {
            PublicationName = string.Empty;
            Price = 0;
            Issues = 0;
            Copies = 0;
            PromoCode = string.Empty;
            SubscriptionType = FrameworkUAD_Lookup.Enums.DeliverTypes.Both;
            GrandTotal = 0;
        }
        #region Properties
        [DataMember]
        public string PublicationName { get; set; }

        /// <summary>
        /// Price per issue per copy
        /// </summary>
        [DataMember]
        public double Price { get; set; }

        /// <summary>
        /// Issues to be received to subscriber.
        /// </summary>
        [DataMember]
        public int Issues { get; set; }

        /// <summary>
        /// Number of copies received per issue.
        /// </summary>
        [DataMember]
        public int Copies { get; set; }

        /// <summary>
        /// Promotion tracking code
        /// </summary>
        [DataMember]
        public string PromoCode { get; set; }

        /// <summary>
        /// Subscription Type: Print, Digital, Both
        /// </summary>
        [DataMember]
        public FrameworkUAD_Lookup.Enums.DeliverTypes SubscriptionType { get; set; }
        
        /// <summary>
        /// Price * Issues * Copies
        /// </summary>
        [DataMember]
        public double GrandTotal { get; set; }
        #endregion
    }
}
