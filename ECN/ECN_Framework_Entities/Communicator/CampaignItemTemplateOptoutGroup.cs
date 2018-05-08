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
    public class CampaignItemTemplateOptoutGroup : CampaignItemTemplateGroupBase
    {
        public CampaignItemTemplateOptoutGroup()
        {
            CampaignItemTemplateOptOutGroupID = -1;
            CampaignItemTemplateID = -1;
            GroupID = -1;
            IsDeleted = false;
            CreatedDate = null;
            CreatedUserID = null;
            UpdatedDate = null;
            UpdatedUserID = null;
        }

        [DataMember]
        public int CampaignItemTemplateOptOutGroupID { get; set; }
    }
}
