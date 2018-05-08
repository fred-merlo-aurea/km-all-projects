using System;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Entity
{
    [Serializable]
    [DataContract]
    public class Action
    {
        public Action() { }
        #region Properties
        [DataMember]
        public int ActionID { get; set; }
        [DataMember]
        public int ActionTypeID { get; set; }
        [DataMember]
        public int CategoryCodeID { get; set; }
        [DataMember]
        public int TransactionCodeID { get; set; }
        [DataMember]
        public string Note { get; set; }
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
