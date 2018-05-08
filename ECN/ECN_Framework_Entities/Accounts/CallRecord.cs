using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class CallRecord
    {
        public CallRecord() 
        {
            CallRecordID = -1;
            StaffID = -1;
            //CallDate
            CallCount = -1;
            ErrorList = new List<ECNError>();
        }

        [DataMember]
        public int CallRecordID { get; set; }
        [DataMember]
        public int StaffID { get; set; }
        [DataMember]
        public DateTime CallDate { get; set; }
        [DataMember]
        public int CallCount { get; set; }
        //validation
        public List<ECNError> ErrorList { get; set; }
    }
}
