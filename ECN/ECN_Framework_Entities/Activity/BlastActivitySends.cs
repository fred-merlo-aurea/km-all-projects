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
    public class BlastActivitySends
    {
        public BlastActivitySends() 
        { 
        }
        
        [DataMember]
        public int SendID { get; set; }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public DateTime SendTime { get; set; }
        [DataMember]
        public bool IsOpened { get; set; }
        [DataMember]
        public bool IsClicked { get; set; }
        [DataMember]
        public string SMTPMessage { get; set; }
        [DataMember]
        public bool IsResend { get; set; }
        [DataMember]
        public int EAID { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
    }
}
