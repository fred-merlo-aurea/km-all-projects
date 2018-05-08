using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class DataCompareCostBase
    {
        public DataCompareCostBase() 
        {
            CodeTypeId = 0;
            CostPerItem = 0;
            DateUpdated = DateTime.Now;
            UpdatedByUserId = 0;
        }
        #region Properties
        [DataMember]
        public int CodeTypeId { get; set; }
        [DataMember]
        public decimal CostPerItem { get; set; }
        [DataMember]
        public DateTime DateUpdated { get; set; }
        [DataMember]
        public int UpdatedByUserId { get; set; }
        #endregion
    }
}
