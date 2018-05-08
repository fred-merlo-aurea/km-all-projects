using System;
using System.Linq;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [DataContract]
    [Serializable]
    public class SecurityGroupOptIn
    {
        public static string SaltValue = "gZKrtEASF[H;F0Z*=i:x8G>:huJCTE.w7X9BJK?C[cOCmH5SeXzxWdVq$}ajM9Gu";

        public SecurityGroupOptIn()
        {
            SecurityGroupOptInID = -1;
            UserID = -1;
            SecurityGroupID = -1;
            ClientID = -1;
            ClientGroupID = -1;
            SendTime = DateTime.Now;
            HasAccepted = false;
            DateAccepted = null;
            SetID = null;
            CreatedByUserID = -1;
            IsDeleted = false;
        }

        #region properties
        [DataMember]
        public int SecurityGroupOptInID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int SecurityGroupID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public int? ClientGroupID { get; set; }
        [DataMember]
        public DateTime SendTime { get; set; }
        [DataMember]
        public bool HasAccepted { get; set; }
        [DataMember]
        public DateTime? DateAccepted { get; set; }
        [DataMember]
        public Guid? SetID { get; set; }
        [DataMember]
        public int UserClientSecurityGroupMapID { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }

        #endregion
    }
}
