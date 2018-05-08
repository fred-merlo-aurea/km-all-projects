using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class LinkTracking
    {
        public LinkTracking()
        {
            LTID = -1;
            DisplayName = string.Empty;
            IsActive = null;
        }

        [DataMember]
        public int LTID { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
    }
}
