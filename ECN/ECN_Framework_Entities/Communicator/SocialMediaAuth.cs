using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [DataContract]
    [Serializable]
    public class SocialMediaAuth
    {
        public SocialMediaAuth()
        {
            SocialMediaAuthID = -1;
            SocialMediaID = -1;
            CustomerID = -1;
            Access_Token = string.Empty;
            UserID = string.Empty;
            CreatedDate = null;
            CreatedUserID = null;
            UpdatedDate = null;
            UpdatedUserID = null;
            IsDeleted = false;
            Access_Secret = string.Empty;
            ProfileName = string.Empty;
        }

        #region properties
        [DataMember]
        public int SocialMediaAuthID { get; set; }
        [DataMember]
        public int SocialMediaID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string Access_Token { get; set; }
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public string Access_Secret { get; set; }
        [DataMember]
        public string ProfileName { get; set; }

        #endregion
    }
}
