using System;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class AdHocDimension
    {
        public AdHocDimension() 
        {
            DateCreated = DateTime.Now;
            UADLastUpdatedDate = DateTimeFunctions.GetMinDate();
            DateUpdated = DateTimeFunctions.GetMinDate();
        }
        #region Properties
        [DataMember]
        public int AdHocDimensionID { get; set; }
        [DataMember]
        public int AdHocDimensionGroupId { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        
        [DataMember]
        public string MatchValue { get; set; }
        [DataMember]
        public string Operator { get; set; }
        
        [DataMember]
        public string DimensionValue { get; set; }
        [DataMember]
        public bool UpdateUAD { get; set; }
        [DataMember]
        public DateTime UADLastUpdatedDate { get; set; }
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
