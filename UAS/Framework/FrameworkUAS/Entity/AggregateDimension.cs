using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class AggregateDimension
    {
        public AggregateDimension() { }
        #region Properties
        [DataMember]
        public int AggregateDimensionID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string StandardFields { get; set; }
        [DataMember]
        public string DimensionFields { get; set; }
        [DataMember]
        public string Formula { get; set; }
        [DataMember]
        public string CreatedDimension { get; set; }
        [DataMember]
        public int? ClientCustomProcedureID { get; set; }
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
