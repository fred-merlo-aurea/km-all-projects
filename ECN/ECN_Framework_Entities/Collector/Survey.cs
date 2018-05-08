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
    public class Survey
    {
        public Survey()
        {
            SurveyID = -1;
            SurveyTitle = string.Empty;
            Description = string.Empty;
            CustomerID  = 0;
            GroupID  = 0;
            EnableDate = null;
            DisableDate = null;
            IntroHTML = string.Empty;
            ThankYouHTML = string.Empty;
            IsActive = false;
            CompletedStep = 0;
            CreatedUserID  = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = false;
            CompletedStep = 0;
            SurveyURL = string.Empty;
        }

        [DataMember]
        public int SurveyID { get; set; }
        [DataMember]
        public string SurveyTitle { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public int GroupID { get; set; }
        [DataMember]
        public DateTime? EnableDate { get; set; }
        [DataMember]
        public DateTime? DisableDate { get; set; }
        [DataMember]
        public string IntroHTML { get; set; }
        [DataMember]
        public string ThankYouHTML { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int CompletedStep { get; set; }
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
        [DataMember]
        public int ResponseCount { get; set; }
        [DataMember]
        public string SurveyURL { get; set; }

    }
}
