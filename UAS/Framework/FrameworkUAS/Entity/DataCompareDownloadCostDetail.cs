using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class DataCompareDownloadCostDetail
    {
        public DataCompareDownloadCostDetail()
        {
            DcDownloadId = 0;
            CodeTypeId = 0;
            CostPerItemClient = 0;
            CostPerItemDetailClient = string.Empty;
            CostPerItemThirdParty = 0;
            CostPerItemDetailThirdParty = string.Empty;
            ItemCount = 0;
            ItemTotalCostClient = 0;
            ItemTotalCostThirdParty = 0;
        }
        #region Properties

        [DataMember]
        public int DcDownloadId { get; set; }
        [DataMember]
        public int CodeTypeId { get; set; }
        [DataMember]
        public decimal CostPerItemClient { get; set; }
        [DataMember]
        public string CostPerItemDetailClient { get; set; }
        [DataMember]
        public decimal CostPerItemThirdParty { get; set; }
        [DataMember]
        public string CostPerItemDetailThirdParty { get; set; }
        [DataMember]
        public int ItemCount { get; set; }
        [DataMember]
        public decimal ItemTotalCostClient { get; set; }
        [DataMember]
        public decimal ItemTotalCostThirdParty { get; set; }
        #endregion
    }
}
