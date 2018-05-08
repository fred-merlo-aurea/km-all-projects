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
    public class EmailActivityLog
    {
        public EmailActivityLog()
        {
            EAID = 0;
            EmailID = 0;
            BlastID = 0;
            ActionTypeCode = string.Empty;
            ActionDate = null;
            ActionValue = string.Empty;
            ActionNotes = string.Empty;
            Processed = string.Empty;
            CustomerID = null;
            ErrorList = new List<ECNError>();
        }

        [DataMember]
        public int EAID { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public int BlastID { get; set; }
        [DataMember]
        public string ActionTypeCode { get; set; }
        [DataMember]
        public DateTime? ActionDate { get; set; }
        [DataMember]
        public string ActionValue { get; set; }
        [DataMember]
        public string ActionNotes { get; set; }
        [DataMember]
        public string Processed { get; set; }
        //optional
        [DataMember]
        public int? CustomerID { get; set; }    
        //validation
        public List<ECNError> ErrorList { get; set; }
    }
}
