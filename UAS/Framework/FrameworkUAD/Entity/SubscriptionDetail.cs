using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriptionDetail
    {
        public SubscriptionDetail() { }
       
        [DataMember]
        public int sdID { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public int MasterID { get; set; }
    }
}
