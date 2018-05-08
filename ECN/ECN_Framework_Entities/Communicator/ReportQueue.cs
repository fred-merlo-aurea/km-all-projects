using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class ReportQueue
    {
        public ReportQueue()
        {
            ReportQueueID = -1;
            SendTime = null;
            ReportID = -1;
            Status = "";
            FailureReason = "";
            FinishTime = null;
        }

        #region properties
        [DataMember]
        public int ReportQueueID { get; set; }

        [DataMember]
        public DateTime? SendTime { get; set; }

        [DataMember]
        public int ReportID { get; set; }

        [DataMember]
        public int ReportScheduleID { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string FailureReason { get; set; }

        [DataMember]
        public DateTime? FinishTime { get; set; }

        

        #endregion
    }
}
