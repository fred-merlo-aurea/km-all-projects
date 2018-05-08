using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class UniqueLink
    {
        public UniqueLink()
        {

        }
        [DataMember]
        public int UniqueLinkID { get; set; }

        [DataMember]
        public int BlastLinkID { get; set; }

        [DataMember]
        public int BlastID { get; set; }

        [DataMember]
        public string UniqueID { get; set; }

    }
}
