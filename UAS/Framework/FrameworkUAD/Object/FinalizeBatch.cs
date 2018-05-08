using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class FinalizeBatch
    {
        public FinalizeBatch() { }
        #region Properties
        [DataMember]
        public int BatchID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string PublicationName { get; set; }
        [DataMember]    
        public int LastCount { get; set; }
        [DataMember]
        public DateTime? DateCreated { get; set; }
        [DataMember]
        public DateTime? DateFinalized { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string PublicationCode { get; set; }
        [DataMember]
        public int BatchNumber { get; set; }
        [DataMember]
        public int UserID { get; set; }
        #endregion
    }
}
