using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Response
{
    [Serializable]
    [DataContract]
    public class Subscriber
    {
        public Subscriber()
        {
            subscribers = new List<Entity.Subscriber>();
        }
        [DataMember]
        public List<Entity.Subscriber> subscribers { get; set; }
    }
}
