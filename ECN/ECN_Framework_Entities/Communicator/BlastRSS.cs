using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class BlastRSS
    {
        [DataMember]
        public int BlastRSSID { get; set; }

        [DataMember]
        public int FeedID { get; set; }

        [DataMember]
        public int BlastID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FeedHTML{ get; set; }

        [DataMember]
        public string FeedTEXT { get; set; }
    }
}
