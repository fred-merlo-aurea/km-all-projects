using System;
using System.Linq;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The ProductSubscriptionDetail object.
    /// </summary>
    [Serializable]
    [DataContract]
    public class ProductSubscriptionDetail
    {
        public ProductSubscriptionDetail()
        {
            PubSubscriptionDetailID = 0;
            PubSubscriptionID = 0;
            SubscriptionID = 0;
            CodeSheetID = 0;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            ResponseOther = "";
        }

        #region Properties
        /// <summary>
        /// PubSubscriptionDetail ID for the ProductSubscriptionDetail object.
        /// </summary>
        [DataMember]
        public int PubSubscriptionDetailID { get; set; }
        /// <summary>
        /// PubSubscription ID for the ProductSubscriptionDetail object.
        /// </summary>
        [DataMember]
        public int PubSubscriptionID { get; set; }
        /// <summary>
        /// Subscription ID for the ProductSubscriptionDetail object.
        /// </summary>
        [DataMember]
        public int SubscriptionID { get; set; }
        /// <summary>
        /// CodeSheet ID for the ProductSubscriptionDetail object.
        /// </summary>
        [DataMember]
        public int CodeSheetID { get; set; }
        /// <summary>
        /// Date that the ProductSubscriptionDetail object was created.
        /// </summary>
        [DataMember]
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Date that the ProductSubscriptionDetail object was updated.
        /// </summary>
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        /// <summary>
        /// User ID for the user that created the ProductSubscriptionDetail object.
        /// </summary>
        [DataMember]
        public int CreatedByUserID { get; set; }
        /// <summary>
        /// User ID for the user that updated the ProductSubscriptionDetail object.
        /// </summary>
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        /// <summary>
        /// ResponseOther for the ProductSubscriptionDetail object.
        /// </summary>
        [DataMember]
        public string ResponseOther { get; set; }
        #endregion
    }
}