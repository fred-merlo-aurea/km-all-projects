using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAS.Entity
{
    /// <summary>
    /// class for MVC project
    /// </summary>
    [Serializable]
    [DataContract]
    public class DataComparePricingView
    {
        public DataComparePricingView()
        {
            DcViewID = 0;
            SourceFileID = 0;
            ClientId = 0;
            DcTargetCodeId = 0;
            DcTargetIdUad =0;
            DcTypeCodeId = 0;
            CreatedByUserID = 0;
            PaymentStatusId = 0;
            DateCreated = DateTime.Now;
            Cost = 0;
            Notes = "";
            TotalRecordCount = 0;
            UadConsensusCount = 0;
            FileRecordCount = 0;
            MatchedRecordCount = 0;
            TotalDownLoadCost = 0;
            UadNetCount = 0;
        }
        #region Properties
        [DataMember]
        public int DcViewID { get; set; }
        [DataMember]
        public int SourceFileID { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public int DcTargetCodeId { get; set; }
        
        [DataMember]
        public int? DcTargetIdUad { get; set; }

        [DataMember]
        public int DcTypeCodeId { get; set; }

        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int PaymentStatusId { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public decimal Cost { get; set; }
        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public bool IsBillable { get; set; }
        [DataMember]
        public int UadConsensusCount { get; set; }
        [DataMember]
        public int MatchedRecordCount { get; set; }
        [DataMember]
        public int FileRecordCount { get; set; }
        [DataMember]
        public int TotalRecordCount { get; set; }
        [DataMember]
        public decimal TotalDownLoadCost { get; set; }
        [DataMember]
        public int UadNetCount { get; set; }



        #endregion
    }
}
