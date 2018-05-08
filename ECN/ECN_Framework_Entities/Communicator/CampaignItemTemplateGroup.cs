using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class CampaignItemTemplateGroup : CampaignItemTemplateGroupBase
    {
        public CampaignItemTemplateGroup()
        {
            CampaignItemTemplateGroupID = -1;
            CampaignItemTemplateID = -1;
            GroupID = -1;
            IsDeleted = false;
            CreatedDate = null;
            CreatedUserID = null;
            UpdatedDate = null;
            UpdatedUserID = null;
            Filters = new List<CampaignItemBlastFilter>();
        }

        [DataMember]
        public int CampaignItemTemplateGroupID { get; set; }

        [DataMember]
        public List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> Filters { get; set; }
    }
}
