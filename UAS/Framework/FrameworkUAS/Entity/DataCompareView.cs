using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class DataCompareView
    {
        public DataCompareView() 
        {
            DcViewId = 0;
            DcRunId = 0;
            DcTargetCodeId = 0;
            DcTargetIdUad = null;
            UadNetCount = 0;
            MatchedCount = 0;
            NoDataCount = 0;
            Cost = 0;
            DateCreated = DateTime.Now;
            DateUpdated = null; 
            CreatedByUserID = 0;
            UpdatedByUserID = null;
            IsBillable = true;
            Notes = string.Empty;
            DcTypeCodeId = 0;
            Downloads = new List<DataCompareDownload>();
        }
        #region Properties
        [DataMember]
        public int DcViewId { get; set; }
        [DataMember]
        public int DcRunId { get; set; }
        [DataMember]
        public int DcTargetCodeId { get; set; }
        [DataMember]
        public int? DcTargetIdUad { get; set; } 
        [DataMember]
        public int UadNetCount { get; set; }
        [DataMember]
        public int MatchedCount { get; set; }
        [DataMember]
        public int NoDataCount { get; set; }
        [DataMember]
        public decimal Cost { get; set; }
        [DataMember] public DateTime DateCreated { get; set; }
        [DataMember] public DateTime? DateUpdated { get; set; }
        [DataMember] public int CreatedByUserID { get; set; }
        [DataMember] public int? UpdatedByUserID { get; set; }
        [DataMember] public bool IsBillable { get; set; }
        [DataMember] public string Notes { get; set; }
        [DataMember] public int PaymentStatusId { get; set; }
        [DataMember] public DateTime? PaidDate { get; set; }
        [DataMember]
        public int DcTypeCodeId { get; set; }//match like
        #endregion

        public List<DataCompareDownload> Downloads { get; set; }
    }
}
