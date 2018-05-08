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
    public class BlastActivityClicks
    {
        public BlastActivityClicks() 
        { 
        }
        
        [DataMember]
        public int ClickID { get; set; }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public DateTime ClickTime { get; set; }
        [DataMember]
        public string URL { get; set; }
        [DataMember]
        public int BlastLinkID { get; set; }
        [DataMember]
        public int EAID { get; set; }
        [DataMember]
        public int? UniqueLinkID { get; set; }
    }
}
