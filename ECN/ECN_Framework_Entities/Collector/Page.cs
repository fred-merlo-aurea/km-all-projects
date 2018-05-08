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
    public class Page
    {
        public Page()
        {
            PageID = -1;
            SurveyID = -1;
            PageHeader = string.Empty;
            PageDesc = string.Empty;
            Number = -1;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = false;
            IsIntroPage = false;
            IsThankYouPage = false;
            IsSkipped = false;
        }

        [DataMember]
        public int PageID { get; set; }
        [DataMember]
        public int SurveyID { get; set; }
        [DataMember]
        public string PageHeader { get; set; }
        [DataMember]
        public string PageDesc { get; set; }      
        [DataMember]
        public int Number { get; set; }
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
        public List<Question> QuestionList { get; set; }
        public bool IsSkipped { get; set; }
        public bool IsThankYouPage { get; set; }
        public bool IsIntroPage { get; set; }
    }
}
