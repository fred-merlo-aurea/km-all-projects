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
    public class DomainSuppression
    {
        public DomainSuppression()
        {
            DomainSuppressionID = -1;
            BaseChannelID = null;
            CustomerID = null;
            Domain = string.Empty;
            IsActive = false;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int DomainSuppressionID { get; set; }
        [DataMember]
        public int? BaseChannelID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public string Domain { get; set; }
        [DataMember]
        public bool IsActive { get; set; }        
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
