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
    public class BlastSchedule
    {
        public BlastSchedule()
        {
            BlastScheduleID = null;
            SchedTime = string.Empty;
            SchedStartDate = string.Empty;
            SchedEndDate = string.Empty;
            Period = string.Empty;
            CreatedBy = null;
            UpdatedBy = null;
            UpdatedDate = null;
            DaysList = null;
            SplitType = string.Empty;
            ErrorList = new List<ECNError>();
        }

        [DataMember]
        public int? BlastScheduleID { get; set; }
        [DataMember]
        public string SchedTime { get; set; }
        [DataMember]
        public string SchedStartDate { get; set; }
        [DataMember]
        public string SchedEndDate { get; set; }
        [DataMember]
        public string Period { get; set; }
        [DataMember]
        public int? CreatedBy { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public int? UpdatedBy { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public List<BlastScheduleDays> DaysList { get; set; }
        [DataMember]
        public string SplitType {get; set;}
        //validation
        public List<ECNError> ErrorList { get; set; }
    }
}
