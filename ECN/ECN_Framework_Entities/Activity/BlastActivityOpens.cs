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
    public class BlastActivityOpens
    {
        public BlastActivityOpens() 
        { 
        }
        
        [DataMember]
        public int OpenID { get; set; }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public DateTime OpenTime { get; set; }
        [DataMember]
        public string BrowserInfo { get; set; }
        [DataMember]
        public int EAID { get; set; }
        [DataMember]
        public int EmailClientID { get; set; }
        [DataMember]
        public int PlatformID { get; set; }
    }
}
