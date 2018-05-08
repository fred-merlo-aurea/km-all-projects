using System;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class SubGenUserMap
    {
        public SubGenUserMap() { }
        #region Properties
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public int SubGenUserId { get; set; }
        [DataMember]
        public int SubGenAccountId { get; set; }
        [DataMember]
        public string SubGenAccountName { get; set; }
        #endregion
    }
}
