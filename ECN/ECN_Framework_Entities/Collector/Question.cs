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
    public class Question
    {
        public Question()
        {
            QuestionID = -1;
            SurveyID = -1;
            Number  = -1;
            PageID = -1;
            Format = string.Empty;
            Grid_control_Type = string.Empty;
            QuestionText = string.Empty;
            Preface = string.Empty;
            MaxLength = -1;
            Required = false;
            GridValidation = -1;
            ShowTextControl = false;
            CreatedDate = null;
            CreatedUserID = -1;
            UpdatedUserID = -1;
            UpdatedDate = null;
            IsDeleted = false;
        }

        [DataMember]
        public int QuestionID { get; set; }
        [DataMember]
        public int SurveyID { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int PageID { get; set; }
        [DataMember]
        public string Format { get; set; }
        [DataMember]
        public string QuestionText { get; set; }
        [DataMember]
        public int MaxLength { get; set; }
        [DataMember]
        public bool ShowTextControl { get; set; }
        [DataMember]
        public string Grid_control_Type { get; set; }
        [DataMember]
        public string Preface { get; set; }
        [DataMember]
        public bool Required { get; set; }
        [DataMember]
        public int GridValidation { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool IsDeleted { get; set; }

        public List<Response> ResponseList = new List<Response>();
    }
}
