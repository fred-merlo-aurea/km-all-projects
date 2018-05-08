using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class AdHocDimensionGroupPubcodeMap
    {
        public AdHocDimensionGroupPubcodeMap() { }

        #region Properties
        [DataMember]
        public int AdHocDimensionGroupId { get; set; }
        [DataMember]
        public string Pubcode { get; set; }
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
