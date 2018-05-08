using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class ProductSubscriptionDetail
    {
        public ProductSubscriptionDetail()
        {
            PubSubscriptionDetailID = 0;
            PubSubscriptionID = 0;
            SubscriptionID = 0;
            CodeSheetID = 0;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            ResponseOther = "";
        }

        #region Properties
        [DataMember]
        public int PubSubscriptionDetailID { get; set; }
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
