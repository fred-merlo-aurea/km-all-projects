using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Response
{
    [Serializable]
    [DataContract]
    public class Address
    {
        public Address()
        {
            addresses = new List<Entity.Address>();
        }
        [DataMember]
        public List<Entity.Address> addresses { get; set; }
    }
}
