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
    public class LandingPage
    {
        public LandingPage() 
        {
            LPID = -1;
            Name = string.Empty;
            Description = string.Empty;
            IsActive = null;
            BaseChannel = true;
            Customer = true;
        }

        [DataMember]
        public int LPID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public bool BaseChannel { get; set; }

        [DataMember]
        public bool Customer { get; set; }
    }
}
