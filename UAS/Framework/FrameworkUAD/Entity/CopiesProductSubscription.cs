using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class CopiesProductSubscription
    {
        public CopiesProductSubscription()
        {
            PubSubscriptionID = 0;
            Copies = 0;
        }

        #region Properties

        [DataMember]
        public int PubSubscriptionID { get; set; }

        [DataMember]
        public int Copies { get; set; }

        #endregion
    }
}
