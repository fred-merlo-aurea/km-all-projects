using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class CampaignItemTestBlast
    {
        public CampaignItemTestBlast()
        {
            CampaignItemTestBlastID = -1;
            CampaignItemID = null;
            GroupID = null;
            FilterID = null;
            HasEmailPreview = false;
            BlastID = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
            Blast = null;
            RefBlastList = null;
            Filters = null;
            CampaignItemTestBlastType = "HTML";
            EmailSubject = string.Empty;
            FromName = string.Empty;
            FromEmail = string.Empty;
            ReplyTo = string.Empty;
            LayoutID = null;
        }

        [DataMember]
        public int CampaignItemTestBlastID { get; set; }
        [DataMember]
        public int? CampaignItemID { get; set; }
        [DataMember]
        public int? GroupID { get; set; }
        [DataMember]
        public int? FilterID { get; set; }
        [DataMember]
        public bool? HasEmailPreview { get; set; }
        [DataMember]
        public int? BlastID { get; set; }
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
        public string CampaignItemTestBlastType { get; set; }
        [DataMember]
        public int? LayoutID { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public string FromName { get; set; }
        [DataMember]
        public string FromEmail { get; set; }
        [DataMember]
        public string ReplyTo { get; set; }        

        public ECN_Framework_Entities.Communicator.BlastAbstract Blast { get; set; }
        public List<CampaignItemBlastRefBlast> RefBlastList { get; set; }

        public List<CampaignItemBlastFilter> Filters { get; set; }
    }
}

