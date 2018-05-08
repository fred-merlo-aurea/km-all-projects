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
    public class APILogging
    {
        public APILogging()
        {
            APILogID = -1;
            AccessKey = string.Empty;
            APIMethod = string.Empty;
            Input = string.Empty;
            StartTime = null;
            LogID = null;
            EndTime = null;
        }

        [DataMember]
        public int APILogID { get; set; }
        [DataMember]
        public string AccessKey { get; set; }
        [DataMember]
        public string APIMethod { get; set; }
        [DataMember]
        public string Input { get; set; }
        [DataMember]
        public DateTime? StartTime { get; set; }
        [DataMember]
        public int? LogID { get; set; }
        [DataMember]
        public DateTime? EndTime { get; set; }
    }
}