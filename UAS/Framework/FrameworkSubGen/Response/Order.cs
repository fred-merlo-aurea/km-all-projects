using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Response
{
    [Serializable]
    [DataContract]
    public class Order
    {
        public Order()
        {
            orders = new List<Entity.Order>();
        }
        [DataMember]
        public List<Entity.Order> orders { get; set; }
    }
}
