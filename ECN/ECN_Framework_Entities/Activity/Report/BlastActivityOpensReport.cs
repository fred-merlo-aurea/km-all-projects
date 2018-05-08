using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Activity.Report
{
    [Serializable]
    [DataContract]
    public class BlastActivityOpensReport
    {
        public BlastActivityOpensReport() 
        { 
        }

        [DataMember]
        public string Usage { get; set; }
        [DataMember]
        public int Opens { get; set; }
        [DataMember]
        public string EmailClientName { get; set; }
        [DataMember]
        public string PlatformName { get; set; }
    }
}
