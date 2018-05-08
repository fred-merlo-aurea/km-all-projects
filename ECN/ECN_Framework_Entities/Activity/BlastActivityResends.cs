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
    public class BlastActivityResends
    {
        public BlastActivityResends() 
        { 
        }
        
        [DataMember]
        public int ResendID { get; set; }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public DateTime ResendTime { get; set; }
        [DataMember]
        public int EAID { get; set; }
    }
}
