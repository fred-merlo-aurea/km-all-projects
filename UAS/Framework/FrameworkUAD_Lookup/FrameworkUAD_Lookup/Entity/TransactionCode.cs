using System;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Entity
{
    [Serializable]
    [DataContract]
    public class TransactionCode
    {
        public TransactionCode() { }
        #region Properties
        [DataMember]
        public int TransactionCodeID { get; set; }
        [DataMember]
        public int TransactionCodeTypeID { get; set; }
        [DataMember]
        public string TransactionCodeName { get; set; }
        [DataMember]
        public int TransactionCodeValue { get; set; }
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
        [DataMember]
        public bool IsKill { get; set; }
        
        #endregion
    }
}
