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
    public class BlastScheduleDays
    {
        public BlastScheduleDays()
        {
            BlastScheduleDaysID = null;
            BlastScheduleID = null;
            DayToSend = null;
            IsAmount = null;
            Total = null;
            Weeks = null;
            ErrorList = new List<ECNError>();
        }

        [DataMember]
        public int? BlastScheduleDaysID { get; set; }
        [DataMember]
        public int? BlastScheduleID { get; set; }
        [DataMember]
        public int? DayToSend { get; set; }
        [DataMember]
        public bool? IsAmount { get; set; }
        [DataMember]
        public int? Total { get; set; }
        [DataMember]
        public int? Weeks { get; set; }
        //validation
        public List<ECNError> ErrorList { get; set; }
    }
}
