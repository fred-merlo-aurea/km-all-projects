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
    public class MessageType
    {
        public MessageType()
        {
            MessageTypeID = -1;
            Name = string.Empty;
            Description = string.Empty;
            Threshold = false;
            Priority = false;
            PriorityNumber = null;
            BaseChannelID = -1;
            SortOrder = null;
            IsActive = false;
            CustomerID = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int MessageTypeID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool Threshold { get; set; }
        [DataMember]
        public bool Priority { get; set; }
        [DataMember]
        public int? PriorityNumber { get; set; }
        [DataMember]
        public int BaseChannelID { get; set; }
        [DataMember]
        public int? SortOrder { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
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
