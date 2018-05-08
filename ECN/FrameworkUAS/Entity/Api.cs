using System;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class Api
    {
        public Api() { }

        #region Properties
        [DataMember]
        public int ApiId { get; set; }
        [DataMember]
        public int ServiceID { get; set; }
        [DataMember]
        public int ServiceFeatureID { get; set; }
        [DataMember]
        public string Entity { get; set; }
        [DataMember]
        public string Method { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public bool IsOnlyKM { get; set; }
        [DataMember]
        public bool IsClientSpecific { get; set; }
        #endregion
    }
}
