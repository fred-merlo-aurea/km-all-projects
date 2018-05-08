using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
namespace ECN_Framework_Entities.Communicator
{

    [Serializable]
    [DataContract]
    public class SocialShareDetail
    {
        public SocialShareDetail()
        {
            SocialShareDetailID = -1;
            ContentID = -1;
            Title = null;
            Description = null;
            Image = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        #region Properties
        [DataMember]
        public int SocialShareDetailID { get; set; }
        [DataMember]
        public int ContentID { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Image { get; set; }
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
        #endregion
    }

}
