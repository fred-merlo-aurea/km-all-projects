using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class FileValidator_DemographicTransformed
    {
        public FileValidator_DemographicTransformed() { }
        #region Properties
        [DataMember]
        public int FV_DemographicTransformedID { get; set; }
        [DataMember]
        public int PubID { get; set; }
        [DataMember]
        public Guid STRecordIdentifier { get; set; }
        [DataMember]
        public string MAFField { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public bool NotExists { get; set; }
        [DataMember]
        public string NotExistReason { get; set; }
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
