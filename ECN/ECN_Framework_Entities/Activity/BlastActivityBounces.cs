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
    public class BlastActivityBounces
    {
        public BlastActivityBounces() 
        { 
        }
        
        [DataMember]
        public int BounceID { get; set; }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public DateTime BounceTime { get; set; }
        [DataMember]
        public int BounceCodeID { get; set; }
        [DataMember]
        public string BounceMessage { get; set; }
        [DataMember]
        public string BounceCode { get; set; }
        [DataMember]
        public int EAID { get; set; }
    }
}
