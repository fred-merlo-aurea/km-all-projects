using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    public class UASBridgeECN
    {
        public UASBridgeECN() { }

        #region Properties
        [DataMember]
        public int UASUserID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public int ECNUserID { get; set; }
        #endregion
    }
}
