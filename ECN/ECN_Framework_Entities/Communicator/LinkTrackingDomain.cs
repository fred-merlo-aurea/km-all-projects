using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class LinkTrackingDomain
    {
        public LinkTrackingDomain()
        {
            LTID = -1;
            LinkTrackingDomainID = -1;
            Domain = string.Empty;
            CustomerID = -1;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int LinkTrackingDomainID { get; set; }
        [DataMember]
        public int LTID { get; set; }
        [DataMember]
        public string Domain { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
    }
}
