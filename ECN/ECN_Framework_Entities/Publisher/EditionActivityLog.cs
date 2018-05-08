using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Publisher
{
    [Serializable]
    [DataContract]
    public class EditionActivityLog
    {
        public EditionActivityLog()
        {
            EAID = -1;
            EditionID = null;
            EmailID = null;
            BlastID = null;
            ActionDate = null;
            ActionTypeCode = null;
            ActionValue = string.Empty;
            IPAddress = string.Empty;
            IsAnonymous = null;
            LinkID = null;
            PageID = null;
            PageNo = null;
            SessionID = string.Empty;
            PageEnd = null;
            PageStart = null;
            CustomerID = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int EAID { get; set; }
        [DataMember]
        public int? EditionID { get; set; }
        [DataMember]
        public int? EmailID { get; set; }
        [DataMember]
        public int? BlastID { get; set; }
        [DataMember]
        public DateTime? ActionDate { get; private set; }
        [DataMember]
        public string ActionTypeCode { get; set; }
        [DataMember]
        public string ActionValue { get; set; }
        [DataMember]
        public string IPAddress { get; set; }
        [DataMember]
        public bool? IsAnonymous { get; set; }
        [DataMember]
        public int? LinkID { get; set; }
        [DataMember]
        public int? PageID { get; set; }
        [DataMember]
        public string PageNo { get; set; }
        [DataMember]
        public string SessionID { get; set; }
        [DataMember]
        public int? PageEnd { get; set; }
        [DataMember]
        public int? PageStart { get; set; }
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
