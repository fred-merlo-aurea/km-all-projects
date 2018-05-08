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
    public class AutoResponders
    {
        public AutoResponders()
        {
            AutoResponderID = -1;
            BlastID = null;
            MailServer = string.Empty;
            AccountName = string.Empty;
            AccountPasswd = string.Empty;
            ForwardTo = string.Empty;
            CustomerID = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int AutoResponderID { get; set; }
        [DataMember]
        public int? BlastID { get; set; }
        [DataMember]
        public string MailServer { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public string AccountPasswd { get; set; }
        [DataMember]
        public string ForwardTo { get; set; }
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
