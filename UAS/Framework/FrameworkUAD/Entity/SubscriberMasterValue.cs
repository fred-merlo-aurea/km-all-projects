using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriberMasterValue
    {
        public SubscriberMasterValue() { }

        #region Properties
        [DataMember]
        public int MasterGroupID { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public string MastercodesheetValues { get; set; }
        #endregion
    }
}
