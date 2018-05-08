using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class Wizard
    {
        public Wizard()
        {
            WizardID = -1;
            IsNewMessage = null;
            WizardName = string.Empty;
            EmailSubject = string.Empty;
            FromName = string.Empty;
            FromEmail = string.Empty;
            ReplyTo = string.Empty;
            UserID = -1;
            GroupID = null;
            ContentID = null;
            LayoutID = null;
            BlastID = null;
            FilterID = null;
            StatusCode = string.Empty;
            CompletedStep = null;
            CreatedDate = null;
            UpdatedDate = null;
            CardcvNumber = string.Empty;
            CardExpiration = string.Empty;
            CardHolderName = string.Empty;
            CardNumber = string.Empty;
            CardType = string.Empty;
            TransactionNo = string.Empty;
            MultiGroupIDs = string.Empty;
            SuppressionGroupIDs = string.Empty;
            PageWatchID = null;
            BlastType = string.Empty;
            EmailSubject2 = string.Empty;
            ContentID2 = null;
            LayoutID2 = null;
            SampleWizardID = null;
            RefBlastID = null;
            CampaignID = null;
            DynamicFromEmail = string.Empty;
            DynamicFromName = string.Empty;
            DynamicReplyToEmail = string.Empty;
            CreatedUserID = null;
            UpdatedUserID = null;
            IsDeleted = null;
        }

        [DataMember]
        public int WizardID { get; set; }
        [DataMember]
        public bool? IsNewMessage { get; set; }
        [DataMember]
        public string WizardName { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public string FromName { get; set; }
        [DataMember]
        public string FromEmail { get; set; }
        [DataMember]
        public string ReplyTo { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int? GroupID { get; set; }
        [DataMember]
        public int? ContentID { get; set; }
        [DataMember]
        public int? LayoutID { get; set; }
        [DataMember]
        public int? BlastID { get; set; }
        [DataMember]
        public int? FilterID { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public int? CompletedStep { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public string CardcvNumber { get; set; }
        [DataMember]
        public string CardExpiration { get; set; }
        [DataMember]
        public string CardHolderName { get; set; }
        [DataMember]
        public string CardNumber { get; set; }
        [DataMember]
        public string CardType { get; set; }
        [DataMember]
        public string TransactionNo { get; set; }
        [DataMember]
        public string MultiGroupIDs { get; set; }
        [DataMember]
        public string SuppressionGroupIDs { get; set; }
        [DataMember]
        public int? PageWatchID { get; set; }
        [DataMember]
        public string BlastType { get; set; }
        [DataMember]
        public string EmailSubject2 { get; set; }
        [DataMember]
        public int? ContentID2 { get; set; }
        [DataMember]
        public int? LayoutID2 { get; set; }
        [DataMember]
        public int? SampleWizardID { get; set; }
        [DataMember]
        public int? RefBlastID { get; set; }
        [DataMember]
        public int? CampaignID { get; set; }
        [DataMember]
        public string DynamicFromEmail { get; set; }
        [DataMember]
        public string DynamicFromName { get; set; }
        [DataMember]
        public string DynamicReplyToEmail { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
    }

}
