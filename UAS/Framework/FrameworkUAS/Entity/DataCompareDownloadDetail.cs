using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class DataCompareDownloadDetail
    {
        public DataCompareDownloadDetail()
        {
            DcDownloadDetailId = 0;
            DcDownloadId = 0;
            SubscriptionID = 0;
        }
        #region Properties
        [DataMember]
        public int DcDownloadDetailId { get; set; }
        [DataMember]
        public int DcDownloadId { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        #endregion
    }
}
