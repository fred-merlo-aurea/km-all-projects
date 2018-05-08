using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class DataCompareRecordPriceRange
    {
        public DataCompareRecordPriceRange()
        {
            DcRecordPriceRangeId = 0;
            MatchMergePurgeCost = 0;
            MinCount = 0;
            MaxCount = 0;
            MatchPricePerRecord = 0;
            LikeMergePurgeCost = 0;
            LikePricePerRecord = 0;
            IsMergePurgePerRecordPricing = false;
            IsActive = true;
            DateCreated = DateTime.Now;
            CreatedByUserId = 0;
            DateUpdated = null;
            UpdatedByUserId = null;
        }
        #region Properties
        [DataMember]
        public int DcRecordPriceRangeId { get; set; }
        [DataMember]
        public int MinCount { get; set; }
        [DataMember]
        public int MaxCount { get; set; }
        [DataMember]
        public decimal MatchMergePurgeCost { get; set; }
        [DataMember]
        public decimal MatchPricePerRecord { get; set; }
        [DataMember]
        public decimal LikeMergePurgeCost { get; set; }
        [DataMember]
        public decimal LikePricePerRecord { get; set; }
        [DataMember]
        public bool IsMergePurgePerRecordPricing { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public int CreatedByUserId { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int? UpdatedByUserId { get; set; }
        #endregion
    }
}

