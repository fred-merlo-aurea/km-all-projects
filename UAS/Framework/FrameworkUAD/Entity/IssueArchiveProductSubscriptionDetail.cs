using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class IssueArchiveProductSubscriptionDetail
    {
        public IssueArchiveProductSubscriptionDetail() 
        {
            IAProductSubscriptionDetailID = 0;
            IssueArchiveSubscriptionId = 0;
            PubSubscriptionID = 0;
            SubscriptionID = 0;
            CodeSheetID = 0;
            //IsActive = false;
            ResponseOther = string.Empty;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 0;
            UpdatedByUserID = 0;
        }
        public IssueArchiveProductSubscriptionDetail(FrameworkUAD.Entity.ProductSubscriptionDetail responseMap)
        {
            IAProductSubscriptionDetailID = 0;
            IssueArchiveSubscriptionId = 0;
            PubSubscriptionID = responseMap.PubSubscriptionID;
            SubscriptionID = responseMap.SubscriptionID;
            CodeSheetID = responseMap.CodeSheetID;
            //IsActive = responseMap.IsActive;
            ResponseOther = responseMap.ResponseOther;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 0;
            UpdatedByUserID = 0;
        }
        #region Properties
        [DataMember]
        public int IAProductSubscriptionDetailID { get; set; }
        [DataMember]
        public int IssueArchiveSubscriptionId { get; set; }
        [DataMember]
        public int PubSubscriptionID { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public int CodeSheetID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public string ResponseOther { get; set; }
        #endregion
    }
}
