using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriptionList : IEntity
    {
        public SubscriptionList() { }
        #region Properties
        [DataMember]
        public int subscription_id { get; set; }
        [DataMember]
        public int copies { get; set; }
        [DataMember]
        public bool is_grace { get; set; }
        [DataMember]
        public bool is_paid { get; set; }
        [DataMember]
        public Enums.SubscriptionType type { get; set; }
        [DataMember]
        public int account_id { get; set; }
        #endregion
    }
}
