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
    public class Template
    {
        public Template()
        {
            TemplateID = -1;
            BaseChannelID = null;
            TemplateStyleCode = string.Empty;
            TemplateName = string.Empty;
            TemplateImage = string.Empty;
            TemplateDescription = string.Empty;
            SortOrder = null;
            SlotsTotal = null;
            IsActive = null;
            TemplateSource = string.Empty;
            TemplateText = string.Empty;
            TemplateSubject = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            Category = null;
        }

        [DataMember]
        public int TemplateID { get; set; }
        [DataMember]
        public int? BaseChannelID { get; set; }
        [DataMember]
        public string TemplateStyleCode { get; set; }
        [DataMember]
        public string TemplateName { get; set; }
        [DataMember]
        public string TemplateImage { get; set; }
        [DataMember]
        public string TemplateDescription { get; set; }
        [DataMember]
        public int? SortOrder { get; set; }
        [DataMember]
        public int? SlotsTotal { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public string TemplateSource { get; set; }
        [DataMember]
        public string TemplateText { get; set; }
        [DataMember]
        public string TemplateSubject { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public string Category { get; set; }
    }
}
