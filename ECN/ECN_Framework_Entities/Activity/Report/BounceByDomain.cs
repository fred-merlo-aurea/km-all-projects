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
    public class BounceByDomain
    {
        public BounceByDomain() 
        { 
        }

        [DataMember]
        public string Domain { get; set; }
        [DataMember]
        public int TotalBounces { get; set; }
        [DataMember]
        public int Hard { get; set; }
        [DataMember]
        public int Soft { get; set; }
        [DataMember]
        public int Other { get; set; }
        [DataMember]
        public int MessagesSent { get; set; }
        [DataMember]
        public float PercBounced { get; set; }
    }
}
