using System;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class CampaignItemSuppression
    {
        public CampaignItemSuppression()
        {
            CampaignItemSuppressionID = -1;
            CampaignItemID = null;
            GroupID = null; 
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
        }

        [DataMember]
        public int CampaignItemSuppressionID { get; set; }
        [DataMember]
        public int? CampaignItemID { get; set; }
        [DataMember]
        public int? GroupID { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }

        public List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> Filters { get; set; }
    }
}
