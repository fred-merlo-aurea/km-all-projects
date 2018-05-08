using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAS.Service
{
    [Serializable]
    [DataContract]
    public class Authentication
    {
        public Authentication() 
        {
            Client = null;
            User = null;
            Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Access_Validated;
            IsAuthenticated = false;
            IsKM = false;
            LogEntry = null;
        }
        #region Properties
        [DataMember]
        public KMPlatform.Entity.Client Client { get; set; }
        [DataMember]
        public KMPlatform.Entity.User User { get; set; }
        [DataMember]
        public FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes Status { get; set; }
        [DataMember]
        public bool IsAuthenticated { get; set; }
        [DataMember]
        public bool IsKM { get; set; }
        [DataMember]
        public KMPlatform.Entity.ApiLog LogEntry { get; set; }
        #endregion
    }
}
