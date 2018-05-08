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
    public class SurveyBranching
    {
        public SurveyBranching()
        {
            SurveyID = -1;
            QuestionID = -1;
            OptionID = -1;
            PageID = null;
            EndSurvey = false;
        }

        [DataMember]
        public int SurveyID { get; set; }
        [DataMember]
        public int QuestionID { get; set; }
        [DataMember]
        public int OptionID { get; set; }
        [DataMember]
        public int? PageID { get; set; }
        [DataMember]
        public bool EndSurvey { get; set; }
    }
}
