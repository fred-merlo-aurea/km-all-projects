using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class SmartFormActivityLog
    {
        public SmartFormActivityLog()
        {
            SALID = -1;
            SFID = -1;
            CustomerID = -1;
            GroupID = -1;
            EmailID = -1;
            EmailType = string.Empty;
            EmailTo = string.Empty;
            EmailFrom = string.Empty;
            EmailSubject = string.Empty;
            SendTime = Convert.ToDateTime("1/1/1900");
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int SALID { get; set; }
        [DataMember]
        public int SFID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public int GroupID { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public string EmailType { get; set; }
        [DataMember]
        public string EmailTo { get; set; }
        [DataMember]
        public string EmailFrom { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public DateTime SendTime { get; set; }
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
