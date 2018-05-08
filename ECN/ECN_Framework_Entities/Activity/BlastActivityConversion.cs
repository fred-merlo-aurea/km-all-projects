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
    public class BlastActivityConversion
    {
        public BlastActivityConversion() 
        { 
        }
        
        [DataMember]
        public int ConversionID { get; set; }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public DateTime ConversionTime { get; set; }
        [DataMember]
        public string URL { get; set; }
        [DataMember]
        public int EAID { get; set; }
    }
}
