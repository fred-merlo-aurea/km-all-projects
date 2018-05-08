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
    public class BlastActivityRefer
    {
        public BlastActivityRefer() 
        { 
        }
        
        [DataMember]
        public int ReferID { get; set; }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public DateTime ReferTime { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public int EAID { get; set; }
    }
}
