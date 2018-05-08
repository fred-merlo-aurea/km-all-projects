using System;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class ClientUADUsersMap
    {
        public ClientUADUsersMap() { }
        #region Properties
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        #endregion
    }
}
