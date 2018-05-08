using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class Suppressed
    {
        public Suppressed() { }
        #region Properties
        [DataMember]
        public Guid STRecordIdentifier { get; set; }
        [DataMember]
        public Guid SFRecordIdentifier { get; set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public bool IsSuppressed { get; set; }
        [DataMember]
        public bool IsEmailMatch { get; set; }
        [DataMember]
        public bool IsPhoneMatch { get; set; }
        [DataMember]
        public bool IsAddressMatch { get; set; }
        [DataMember]
        public bool IsCompanyMatch { get; set; }
        [DataMember]
        public string ProcessCode { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int UpdatedByUserID { get; set; }
        #endregion
    }
}
