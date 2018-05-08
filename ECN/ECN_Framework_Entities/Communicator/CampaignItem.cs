using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class CampaignItem
    {
        public CampaignItem()
        {
            CampaignItemID = -1;
            CampaignID = null;
            CampaignItemName = string.Empty;
            CampaignItemType = string.Empty;
            FromName = string.Empty;
            FromEmail = string.Empty;
            ReplyTo = string.Empty;
            SendTime = null;
            SampleID = null;
            PageWatchID = null;
            BlastScheduleID = null;
            OverrideAmount = null;
            OverrideIsAmount = null;
            BlastField1 = string.Empty;
            BlastField2 = string.Empty;
            BlastField3 = string.Empty;
            BlastField4 = string.Empty;
            BlastField5 = string.Empty;
            CampaignItemFormatType = string.Empty;
            CompletedStep = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
            BlastList = null;
            TestBlastList = null;
            SuppressionList = null;
            OptOutGroupList = null;
            IsHidden = null;
            CampaignItemNameOriginal = string.Empty;
            NodeID = string.Empty; 
            CampaignItemIDOriginal = null;
            CampaignItemTemplateID = null;
            SFCampaignID = null;
            EnableCacheBuster = null;
            IgnoreSuppression = false;
        }

        #region Properties
        [DataMember]
        public int CampaignItemID { get; set; }
        [DataMember]
        public int? CampaignItemTemplateID { get; set; }
        [DataMember]
        public int? CampaignID { get; set; }
        [DataMember]
        public string CampaignItemName { get; set; }
        [DataMember]
        public string CampaignItemFormatType { get; set; }
        [DataMember]
        public string NodeID { get; set; }
        [DataMember]
        public string FromName { get; set; }
        [DataMember]
        public string FromEmail { get; set; }
        [DataMember]
        public string ReplyTo { get; set; }
        [DataMember]
        public DateTime? SendTime { get; set; }
        [DataMember]
        public int? SampleID { get; set; }
        [DataMember]
        public int? PageWatchID { get; set; }
        [DataMember]
        public int? BlastScheduleID { get; set; }
        [DataMember]
        public int? OverrideAmount { get; set; }
        [DataMember]
        public bool? OverrideIsAmount { get; set; }
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
        public int? CompletedStep { get; set; }
        [DataMember]
        public string CampaignItemType { get; set; }
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
        public int? CustomerID { get; set; }
        [DataMember]
        public string SFCampaignID { get; set; }
        [DataMember]
        public bool? IsHidden { get; set; }
        [DataMember]
        public string CampaignItemNameOriginal { get; set; }
        [DataMember]
        public int? CampaignItemIDOriginal { get; set; }
        [DataMember]
        public bool? EnableCacheBuster { get; set; }
        [DataMember]
        public bool? IgnoreSuppression { get; set; }
        //optional
        public List<CampaignItemBlast> BlastList { get; set; }
        public List<CampaignItemTestBlast> TestBlastList { get; set; }
        public List<CampaignItemSuppression> SuppressionList { get; set; }
        public List<CampaignItemOptOutGroup> OptOutGroupList { get; set; }
        #endregion

    }
}
