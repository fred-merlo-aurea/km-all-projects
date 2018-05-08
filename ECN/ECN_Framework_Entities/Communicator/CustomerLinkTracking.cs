using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class CustomerLinkTracking
    {
        public CustomerLinkTracking()
        {
            CLTID = -1;
            CustomerID = null;
            LTPID = null;
            LTPOID = null;
            IsActive = null;
        }

        [DataMember]
        public int CLTID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public int? LTPID { get; set; }
        [DataMember]
        public int? LTPOID { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
    }
}
