using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class BlastLink
    {
        public BlastLink()
        {
            BlastLinkID = -1;
            BlastID = -1;
            LinkURL = string.Empty;
        }

        [DataMember]
        public int BlastLinkID { get; set; }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public string LinkURL { get; set; }
    }
}
