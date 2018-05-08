using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class DataCompareDownload
    {
        public DataCompareDownload() 
        {
            DcDownloadId = 0;
            DcViewId = 0;
            WhereClause = string.Empty;
            //ProfileColumns = string.Empty;
            //DimensionColumns = string.Empty;
            DcTypeCodeId = 0;
            ProfileCount = 0;
            TotalItemCount = 0;
            TotalBilledCost = 0;
            TotalThirdPartyCost = 0;
            IsPurchased = false;
            PurchasedByUserId = 0;
            PurchasedDate = null;
            PurchasedCaptcha = string.Empty;
            IsBilled = false;
            BilledDate = null;
            DownloadFileName = null;
            CreatedByUserId = 0;
            DateCreated = DateTime.Now;
            CostDetail = new List<DataCompareDownloadCostDetail>();
        }
        #region Properties
        [DataMember]
        public int DcDownloadId { get; set; }
        [DataMember]
        public int DcViewId { get; set; }
        [DataMember]
        public string WhereClause { get; set; }
        //[DataMember]
        //public string ProfileColumns { get; set; }
        //[DataMember]
        //public string DimensionColumns { get; set; }
        [DataMember]
        public int DcTypeCodeId { get; set; }//match like
        [DataMember]
        public int ProfileCount { get; set; }//recordCount
        [DataMember]
        public int TotalItemCount { get; set; }
        [DataMember]
        public decimal TotalBilledCost { get; set; }//cost to client
        [DataMember]
        public decimal TotalThirdPartyCost { get; set; }//cost that client can charge a third party - thier customer
        [DataMember]
        public bool IsPurchased { get; set; }
        [DataMember]
        public int PurchasedByUserId { get; set; }
        [DataMember]
        public DateTime? PurchasedDate { get; set; }
        [DataMember]
        public string PurchasedCaptcha { get; set; }
        [DataMember]
        public bool IsBilled { get; set; }
        [DataMember]
        public DateTime? BilledDate { get; set; }
        [DataMember]
        public string DownloadFileName { get; set; }
        [DataMember]
        public int CreatedByUserId { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        #endregion

        [DataMember]
        public List<FrameworkUAS.Entity.DataCompareDownloadCostDetail> CostDetail { get; set; }
    }
}
