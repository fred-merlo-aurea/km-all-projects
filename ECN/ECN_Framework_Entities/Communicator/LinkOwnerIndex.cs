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
    public class LinkOwnerIndex
    {
        public LinkOwnerIndex()
        {
            LinkOwnerIndexID = -1;
            CustomerID = -1;
            LinkOwnerName = string.Empty;
            LinkOwnerCode = string.Empty;
            ContactFirstName = string.Empty;
            ContactLastName = string.Empty;
            ContactPhone = string.Empty;
            ContactEmail = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            State = string.Empty;
            IsActive = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int LinkOwnerIndexID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string LinkOwnerName { get; set; }
        [DataMember]
        public string LinkOwnerCode { get; set; }
        [DataMember]
        public string ContactFirstName { get; set; }
        [DataMember]
        public string ContactLastName { get; set; }
        [DataMember]
        public string ContactPhone { get; set; }
        [DataMember]
        public string ContactEmail { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
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
