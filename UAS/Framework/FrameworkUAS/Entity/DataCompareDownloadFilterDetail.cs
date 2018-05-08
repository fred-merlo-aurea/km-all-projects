using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class DataCompareDownloadFilterDetail
    {
        public DataCompareDownloadFilterDetail()
        {
            DcFilterDetailID = 0;
            DcFilterGroupId = 0;
            FilterType = 0;
            Group = string.Empty;
            Name = string.Empty;
            Values = string.Empty;
            SearchCondition = string.Empty;
        }
        #region Properties
        [DataMember]
        public int DcFilterDetailID { get; set; }
        [DataMember]
        public int DcFilterGroupId { get; set; }
        [DataMember]
        public int FilterType { get; set; }
        [DataMember]
        public string Group { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Values { get; set; }
        [DataMember]
        public string SearchCondition { get; set; }
        #endregion
    }
}
