using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class RelationalPubCode
    {
        public RelationalPubCode() { }
        #region Properties
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public string SpecialFileName { get; set; }
        [DataMember]
        public string RelationalFileName { get; set; }
        [DataMember]
        public string PubCode { get; set; }
        [DataMember]
        public string CodeID { get; set; }
        [DataMember]
        public string CodeDateType { get; set; }
        [DataMember]
        public string FieldName { get; set; }
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
