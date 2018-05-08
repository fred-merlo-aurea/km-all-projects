using System;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriptionStatusMatrix
    {
        public SubscriptionStatusMatrix() { }
        #region Properties
        [DataMember]
        public int StatusMatrixID { get; set; }
        [DataMember]
        public int SubscriptionStatusID { get; set; }
        [DataMember]
        public int CategoryCodeID { get; set; }
        [DataMember]
        public int TransactionCodeID { get; set; }
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
