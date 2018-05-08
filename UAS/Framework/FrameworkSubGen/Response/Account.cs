using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Response
{
    [Serializable]
    [DataContract]
    public class Account
    {
        public Account()
        {
            accounts = new List<Entity.Account>();
        }
        [DataMember]
        public List<Entity.Account> accounts { get; set; }
    }
}
