using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class DataCompareDownloadFilterGroup
    {
        public DataCompareDownloadFilterGroup()
        {
            DcFilterGroupId = 0;
            DcDownloadId = 0;
            DcFilterDetails = new List<DataCompareDownloadFilterDetail>();
        }
        #region Properties
        [DataMember]
        public int DcFilterGroupId { get; set; }
        [DataMember]
        public int DcDownloadId { get; set; }
        [DataMember]
        public List<DataCompareDownloadFilterDetail> DcFilterDetails { get; set; }
        #endregion
    }
}
