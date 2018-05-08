using System;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The MarketingMap object.
    /// </summary>
    [Serializable]
    [DataContract]
    public class MarketingMap
    {
        #region Properties
        /// <summary>
        /// Marketing ID for the MarketingMap object.
        /// </summary>
        [DataMember]
        public int MarketingID { get; set; }
        /// <summary>
        /// PubSubscription ID for the MarketingMap object.
        /// </summary>
        [DataMember]
        public int PubSubscriptionID { get; set; }
        /// <summary>
        /// Publication ID for the MarketingMap object.
        /// </summary>
        [DataMember]
        public int PublicationID { get; set; }
        /// <summary>
        /// If the MarketingMap object is active.
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }
        /// <summary>
        /// Date the MarketingMap object was created.
        /// </summary>
        [DataMember]
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Date when the MarketingMap object was last updated.
        /// </summary>
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        /// <summary>
        /// User ID for the user that created the MarketingMap object.
        /// </summary>
        [DataMember]
        public int CreatedByUserID { get; set; }
        /// <summary>
        /// User ID for the user that updated the MarketingMap object.
        /// </summary>
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
    }
}