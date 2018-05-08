using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Activity
{
    [Serializable]
    [DataContract]
    public class EmailClients
    {
        public EmailClients() 
        { 
        }

        [DataMember]
        public int EmailClientID { get; set; }
        [DataMember]
        public string EmailClientName { get; set; }
    }
}
