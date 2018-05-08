using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Collector
{
    [Serializable]
    [DataContract]
    public class ResponseOptions
    {
        public ResponseOptions()
        {
            OptionID = -1;
            QuestionID = -1;
            OptionValue = string.Empty;
            Score = -1;        
        }

        [DataMember]
        public int OptionID { get; set; }
        [DataMember]
        public int QuestionID { get; set; }
        [DataMember]
        public string OptionValue { get; set; }
        [DataMember]
        public int Score { get; set; }
    }
}
