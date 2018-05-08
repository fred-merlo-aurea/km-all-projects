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
    public class CampaignItemMetaTag
    {
        public CampaignItemMetaTag()
        {
            CampaignItemMetaTagID = -1;
            CampaignItemID = -1;
            SocialMediaID = -1;
            Property = string.Empty;
            Content = string.Empty;
            IsDeleted = false;
            CreatedDate = null;
            CreatedUserID = null;
            UpdatedDate = null;
            UpdatedUserID = null;
        }

        #region Properties
        [DataMember]
        public int CampaignItemMetaTagID { get; set; }
        [DataMember]
        public int CampaignItemID { get; set; }
        [DataMember]
        public int SocialMediaID { get; set; }
        [DataMember]
        public string Property { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        #endregion
    }
}
