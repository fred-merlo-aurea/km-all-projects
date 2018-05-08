using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Response
{
    [Serializable]
    [DataContract]
    public class Subscription
    {
        public Subscription()
        {
            subscriptions = new List<Entity.Subscription>();
        }
        [DataMember]
        public List<Entity.Subscription> subscriptions { get; set; }
    }
}
