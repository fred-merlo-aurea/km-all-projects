using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class CampaignItemTemplate
    {
        public CampaignItemTemplate()
        {
            CampaignItemTemplateID = -1;
            TemplateName = string.Empty;
            FromName = string.Empty;
            FromEmail = string.Empty;
            ReplyTo = string.Empty;
            Subject = string.Empty;
            BlastField1 = string.Empty;
            BlastField2 = string.Empty;
            BlastField3 = string.Empty;
            BlastField4 = string.Empty;
            BlastField5 = string.Empty;
            Omniture1 = string.Empty;
            Omniture2 = string.Empty;
            Omniture3 = string.Empty;
            Omniture4 = string.Empty;
            Omniture5 = string.Empty;
            Omniture6 = string.Empty;
            Omniture7 = string.Empty;
            Omniture8 = string.Empty;
            Omniture9 = string.Empty;
            Omniture10 = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
            SuppressionGroupList = null;
            SelectedGroupList = null;
            Archived = false;
            LayoutID = null;
            OptOutMasterSuppression = false;
            OptOutSpecificGroup = false;
            OptoutGroupList = null;
            OmnitureCustomerSetup = false;
        }

        #region Properties
        [DataMember]
        public int CampaignItemTemplateID { get; set; }
        [DataMember]
        public string TemplateName { get; set; }
        [DataMember]
        public string FromName { get; set; }
        [DataMember]
        public string FromEmail { get; set; }
        [DataMember]
        public string ReplyTo { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string BlastField1 { get; set; }
        [DataMember]
        public string BlastField2 { get; set; }
        [DataMember]
        public string BlastField3 { get; set; }
        [DataMember]
        public string BlastField4 { get; set; }
        [DataMember]
        public string BlastField5 { get; set; }
        [DataMember]
        public string Omniture1 { get; set; }
        [DataMember]
        public string Omniture2 { get; set; }
        [DataMember]
        public string Omniture3 { get; set; }
        [DataMember]
        public string Omniture4 { get; set; }
        [DataMember]
        public string Omniture5 { get; set; }
        [DataMember]
        public string Omniture6 { get; set; }
        [DataMember]
        public string Omniture7 { get; set; }
        [DataMember]
        public string Omniture8 { get; set; }
        [DataMember]
        public string Omniture9 { get; set; }
        [DataMember]
        public string Omniture10 { get; set; }
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
        [DataMember]
        public int? CustomerID { get; set; }

        [DataMember]
        public bool? Archived { get; set; }

        [DataMember]
        public int? LayoutID { get; set; }
        [DataMember]
        public bool? OptOutMasterSuppression { get; set; }
        [DataMember]
        public bool? OptOutSpecificGroup { get; set; }
        [DataMember]
        public bool? OmnitureCustomerSetup { get; set; }

        [DataMember]
        public int? CampaignID { get; set; }

        public List<ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup> SuppressionGroupList { get; set; }

        public List<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup> SelectedGroupList { get; set; }

        public List<ECN_Framework_Entities.Communicator.CampaignItemTemplateOptoutGroup> OptoutGroupList { get; set; }
        #endregion

    }
}
