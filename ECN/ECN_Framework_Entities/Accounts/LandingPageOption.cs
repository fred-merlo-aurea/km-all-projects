using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class LandingPageOption
    {
        public LandingPageOption()
        {
            LPOID = -1;
            LPID = null;
            Name = string.Empty;
            Description = string.Empty;
            IsActive = null;
        }

        [DataMember]
        public int LPOID { get; set; }
        [DataMember]
        public int? LPID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
    }
}
