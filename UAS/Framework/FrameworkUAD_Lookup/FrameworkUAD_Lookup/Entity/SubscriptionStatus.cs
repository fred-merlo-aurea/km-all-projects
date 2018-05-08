using System;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriptionStatus
    {
        public SubscriptionStatus() { }
        #region Properties
        [DataMember]
        public int SubscriptionStatusID { get; set; }
        [DataMember]
        public string StatusName { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public string Icon { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
    }
}
