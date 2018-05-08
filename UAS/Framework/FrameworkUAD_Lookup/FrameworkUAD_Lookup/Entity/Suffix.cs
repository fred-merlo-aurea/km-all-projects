using System;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Entity
{
    [Serializable]
    [DataContract]
    public class Suffix
    {
        public Suffix() { }

        #region Properties
        [DataMember]
        public int SuffixID { get; set; }
        [DataMember]
        public int SuffixCodeTypeID { get; set; }
        [DataMember]
        public string SuffixName { get; set; }
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
