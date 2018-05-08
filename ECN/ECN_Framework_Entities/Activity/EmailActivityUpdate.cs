using System;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Activity
{
    [Serializable]
    [DataContract]
    public class EmailActivityUpdate
    {
        public EmailActivityUpdate()
        {
            ApplicationSourceID = 96;
        }
        [DataMember]
        public int UpdateID { get; set; }
        [DataMember]
        public int OldEmailID { get; set; }
        [DataMember]
        public string OldEmailAddress { get; set; }
        [DataMember]
        public int NewEmailID { get; set; }
        [DataMember]
        public string NewEmailAddress { get; set; }
        [DataMember]
        public DateTime UpdateTime { get; set; }
        [DataMember]
        public int ApplicationSourceID { get; set; }
        [DataMember]
        public int SourceID { get; set; }
        [DataMember]
        public string Comments { get; set; }
    }
}
