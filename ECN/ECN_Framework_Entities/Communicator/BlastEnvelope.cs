using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class BlastEnvelope
    {
        public BlastEnvelope()
        {
            BlastEnvelopeID = -1;
            CustomerID = null;
            FromName = string.Empty;
            FromEmail = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int BlastEnvelopeID { get; set; }
        [DataMember]
        public string FromName { get; set; }
        [DataMember]
        public string FromEmail { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
    }
}


