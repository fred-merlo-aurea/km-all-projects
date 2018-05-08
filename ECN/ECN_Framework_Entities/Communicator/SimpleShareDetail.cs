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
    public partial class SimpleShareDetail
    {
        public SimpleShareDetail()
        {
            SimpleShareDetailID = -1;
            SocialMediaID = -1;
            SocialMediaAuthID = -1;
            Title = string.Empty;
            SubTitle = string.Empty;
            ImagePath = string.Empty;
            Content = string.Empty;
            CampaignItemID = -1;
            UseThumbnail = false;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedDate = null;
            UpdatedUserID = null;
            IsDeleted = false;
            PageAccessToken = string.Empty;
            PageID = string.Empty;
        }

        #region properties
        [DataMember]
        public int SimpleShareDetailID { get; set; }
        [DataMember]
        public int SocialMediaID { get; set; }
        [DataMember]
        public int SocialMediaAuthID { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string SubTitle { get;set;}
        [DataMember]
        public string ImagePath { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public int CampaignItemID { get; set; }
        [DataMember]
        public bool? UseThumbnail { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public string PageAccessToken { get; set; }
        [DataMember]
        public string PageID { get; set; }

        #endregion
    }
}
