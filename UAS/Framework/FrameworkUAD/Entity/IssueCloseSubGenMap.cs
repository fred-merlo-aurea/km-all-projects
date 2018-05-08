using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class IssueCloseSubGenMap
    {
        public IssueCloseSubGenMap()
        {
            IssueId = 0;
            PubSubscriptionID = 0;
            SubGenSubscriberID = 0;
        }

        [DataMember]
        public int IssueId { get; set; }
        [DataMember]
        public int PubSubscriptionID { get; set; }
        [DataMember]
        public int SubGenSubscriberID { get; set; }
    }
}
