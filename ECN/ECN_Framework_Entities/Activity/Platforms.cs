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
    public class Platforms
    {
        public Platforms() 
        { 
        }

        [DataMember]
        public int PlatformID { get; set; }
        [DataMember]
        public string PlatformName { get; set; }
    }
}
