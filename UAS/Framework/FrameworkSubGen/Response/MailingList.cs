using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkSubGen.Response
{
    [Serializable]
    [DataContract]
    public class MailingList
    {
        public MailingList()
        {
            mailinglists = new List<Entity.MailingList>();
        }
        [DataMember]
        public List<Entity.MailingList> mailinglists { get; set; }
    }
}
