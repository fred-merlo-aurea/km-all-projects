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
    public class BlastActivityUnSubscribes
    {
        public BlastActivityUnSubscribes() 
        { 
        }
        
        [DataMember]
        public int UnsubscribeID { get; set; }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public DateTime UnsubscribeTime { get; set; }
        [DataMember]
        public int UnsubscribeCodeID { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public int EAID { get; set; }
    }
}
