using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class EmailDirect
    {
        public EmailDirect()
        {
            EmailDirectID = -1;
            CustomerID = -1;
            Source = string.Empty;
            Process = string.Empty;
            Status = "Pending";
            SendTime = DateTime.Now;
            StartTime = null;
            FinishTime = null;
            EmailAddress = string.Empty;
            FromName = string.Empty;
            EmailSubject = string.Empty;
            ReplyEmailAddress = string.Empty;
            Content = string.Empty;
            CreatedDate = null;
            CreatedUserID = null;
            UpdatedDate = null;
            UpdatedUserID = null;
            OpenTime = null;
            Attachments = new List<System.Net.Mail.Attachment>();
            CCAddresses = new List<string>();
        }

        #region properties
        [DataMember]
        public int EmailDirectID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public string Process { get; set; }

        [DataMember]
        public string Status { get; set; }
 
        [DataMember]
        public DateTime? SendTime { get; set; }
        [DataMember]
        public DateTime? StartTime { get; set; }
        [DataMember]
        public DateTime? FinishTime { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string FromEmailAddress { get;set; }
        [DataMember]
        public string FromName { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public string ReplyEmailAddress { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? OpenTime { get; set; }

        [DataMember]
        public List<System.Net.Mail.Attachment> Attachments { get; set; }
        [DataMember]
        public List<string> CCAddresses { get; set; }
        #endregion
    }
}
