using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class CampaignItemBlast
    {
        public CampaignItemBlast()
        {
            CampaignItemBlastID = -1;
            CampaignItemID = null;
            EmailSubject = string.Empty;
            DynamicFromName = string.Empty;
            DynamicFromEmail = string.Empty;
            DynamicReplyTo = string.Empty;
            LayoutID = null;
            GroupID = null;
            SocialMediaID = null;
            BlastID = null;
            AddOptOuts_to_MS = null;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            CustomerID = null;
            Blast = null;
            RefBlastList = null;
            Filters = new List<CampaignItemBlastFilter>();
        }

        [DataMember]
        public int CampaignItemBlastID { get; set; }
        [DataMember]
        public int? CampaignItemID { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public string DynamicFromName { get; set; }
        [DataMember]
        public string DynamicFromEmail { get; set; }
        [DataMember]
        public string DynamicReplyTo { get; set; }
        [DataMember]
        public int? LayoutID { get; set; }
        [DataMember]
        public int? GroupID { get; set; }
        [DataMember]
        public int? SocialMediaID { get; set; }
        
        [DataMember]
        public bool? AddOptOuts_to_MS { get; set; }
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
        public string EmailFrom { get; set; }
        [DataMember]
        public string ReplyTo { get; set; }
        [DataMember]
        public string FromName { get; set; }

        public ECN_Framework_Entities.Communicator.BlastAbstract Blast { get; set; }
        public List<CampaignItemBlastRefBlast> RefBlastList { get; set; }

        public List<CampaignItemBlastFilter> Filters { get; set; }
    }
}
