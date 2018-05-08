using System;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class UserClientSecurityGroupMap
    {
        public UserClientSecurityGroupMap() 
        {
            UserClientSecurityGroupMapID = 0;
            UserID = 0;
            ClientID = 0;
            SecurityGroupID = 0;
            IsActive = false;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            InactiveReason = string.Empty;
        }
        #region Properties
        [DataMember]
        public int UserClientSecurityGroupMapID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public int SecurityGroupID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public string InactiveReason { get; set; }
        #endregion
    }
}
