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
    public partial class CampaignItemSocialMedia
    {
        public CampaignItemSocialMedia()
        { }

        #region properties  
        [DataMember]
        public int CampaignItemSocialMediaID { get; set; }
        [DataMember]
        public int CampaignItemID { get; set; }
        [DataMember]
        public int SocialMediaID { get; set; }
        [DataMember]
        public int? SimpleShareDetailID { get; set; }
        [DataMember]
        public int? SocialMediaAuthID { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public DateTime StatusDate { get; set; }
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string PageAccessToken { get; set; }
        [DataMember]
        public string PostID { get; set; }
        [DataMember]
        public int? LastErrorCode { get; set; }

        #endregion
    }
}
