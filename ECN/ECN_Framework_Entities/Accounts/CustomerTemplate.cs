using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class CustomerTemplate
    {
        public CustomerTemplate()
        {
            CTID = -1;
            CustomerID = null;
            TemplateTypeCode = string.Empty;
            HeaderSource = string.Empty;
            FooterSource = string.Empty;
            CCNotifyEmail = string.Empty;
            IsActive = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        [DataMember]
        public int CTID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public string TemplateTypeCode { get; set; }
        [DataMember]
        public string HeaderSource { get; set; }
        [DataMember]
        public string FooterSource { get; set; }
        [DataMember]
        public string CCNotifyEmail { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
    }
}
