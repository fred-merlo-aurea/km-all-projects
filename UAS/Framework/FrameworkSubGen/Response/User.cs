using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Response
{
    [Serializable]
    [DataContract]
    public class User
    {
        public User()
        {
            users = new List<Entity.User>();
        }
        [DataMember]
        public List<Entity.User> users { get; set; }
    }
}
