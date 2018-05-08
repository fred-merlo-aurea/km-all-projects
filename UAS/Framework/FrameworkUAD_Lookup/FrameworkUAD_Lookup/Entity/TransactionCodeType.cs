using System;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Entity
{
    [Serializable]
    [DataContract]
    public class TransactionCodeType
    {
        public TransactionCodeType() { }
        #region Properties
        [DataMember]
        public int TransactionCodeTypeID { get; set; }
        [DataMember]
        public string TransactionCodeTypeName { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsFree { get; set; }
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
