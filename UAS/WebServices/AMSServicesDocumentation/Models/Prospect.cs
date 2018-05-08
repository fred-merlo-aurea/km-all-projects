using System;
using System.Runtime.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The Prospect object.
    /// </summary>
    [Serializable]
    [DataContract]
    public class Prospect
    {
        public Prospect() { }
        #region Properties
        /// <summary>
        /// Prospect ID for the Prospect object.
        /// </summary>
        [DataMember]
        public int ProspectID { get; set; }
        /// <summary>
        /// Publication ID for the Prospect object.
        /// </summary>
        [DataMember]
        public int PublicationID { get; set; }
        /// <summary>
        /// Subscriber ID for the Prospect object.
        /// </summary>
        [DataMember]
        public int SubscriberID { get; set; }
        /// <summary>
        /// If the Prospect object is a prospect.
        /// </summary>
        [DataMember]
        public bool IsProspect { get; set; }
        /// <summary>
        /// If the Prospect object is a verified prospect.
        /// </summary>
        [DataMember]
        public bool IsVerifiedProspect { get; set; }
        /// <summary>
        /// Date the Prospect object was created.
        /// </summary>
        [DataMember]
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Date the Prospect object was updated.
        /// </summary>
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        /// <summary>
        /// User ID for the user that created the Prospect object.
        /// </summary>
        [DataMember]
        public int CreatedByUserID { get; set; }
        /// <summary>
        /// User ID for the user that updated the Prospect object.
        /// </summary>
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
    }
}