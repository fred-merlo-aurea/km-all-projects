using System;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Entity
{
    [Serializable]
    [DataContract]
    public class CategoryCodeType
    {
        public CategoryCodeType() { }
        #region Properties
        [DataMember]
        public int CategoryCodeTypeID { get; set; }
        [DataMember]
        public string CategoryCodeTypeName { get; set; }
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
