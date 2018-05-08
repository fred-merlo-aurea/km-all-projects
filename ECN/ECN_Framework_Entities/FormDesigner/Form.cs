using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.FormDesigner
{
    [Serializable]
    [DataContract]
    public class Form
    {
        public Form()
        {
            Form_Seq_ID = -1;
            TokenUID = new Guid();
            Name = string.Empty;
            Status = string.Empty; ;
            ActivationDateFrom = null;
            ActivationDateTo = null;
            LastUpdated = null;
            LastPublished = null;
            FormType = string.Empty; ;
            OptInType = -1;
            CssUri = string.Empty;
            HeaderHTML = string.Empty;
            FooterHTML = string.Empty;
            UpdatedBy = string.Empty;
            CustomerName = string.Empty;
            Active = -1;
            SubmitButtonText = null;
            ParentForm_ID = null;
            CssFile_Seq_ID = null;
            GroupID = -1;
            CustomerID = -1;
            UserID = -1;
            StylingType = -1;
            CustomerAccessKey = string.Empty;
            PublishAfter = null;
        }
        #region Properties
        [DataMember]
        public int Form_Seq_ID { get; set; }
        [DataMember]
        public Guid TokenUID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public String Status { get; set; }
        [DataMember]
        public DateTime? ActivationDateFrom { get; set; }
        [DataMember]
        public DateTime? ActivationDateTo { get; set; }
        [DataMember]
        public DateTime? LastUpdated { get; set; }
        [DataMember]
        public DateTime? LastPublished { get; set; }
        [DataMember]
        public String FormType { get; set; }
        [DataMember]
        public int OptInType { get; set; }
        [DataMember]
        public string CssUri { get; set; }
        [DataMember]
        public string HeaderHTML { get; set; }
        [DataMember]
        public string FooterHTML { get; set; }
        [DataMember]
        public string UpdatedBy { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public int Active { get; set; }
        [DataMember]
        public string SubmitButtonText { get; set; }
        [DataMember]
        public int? ParentForm_ID { get; set; }
        [DataMember]
        public int? CssFile_Seq_ID { get; set; }
        [DataMember]
        public int GroupID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int StylingType { get; set; }
        [DataMember]
        public string CustomerAccessKey { get; set; }
        [DataMember]
        public DateTime? PublishAfter { get; set; }
        #endregion
    }
}