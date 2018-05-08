using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class CampaignItemTemplateSuppressionGroup
    {
        public CampaignItemTemplateSuppressionGroup()
        {
            CampaignItemTemplateSuppressionGroupID = -1;
            CampaignItemTemplateID = null;
            GroupID = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
        }

        [DataMember]
        public int CampaignItemTemplateSuppressionGroupID { get; set; }
        [DataMember]
        public int? CampaignItemTemplateID { get; set; }
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
        [DataMember]
        public List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> Filters { get; set; }
    }
}
