using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class IssueCompDetail : BaseUser
    {
        public IssueCompDetail() 
        {
            IssueCompDetailId = 0;
            IssueCompID = 0;
            PubSubscriptionID = 0;
            SubscriptionID = 0;
            PubID = 0;
            Demo7 = string.Empty;
            PubQSourceID = 0;
            PubCategoryID = 0;
            PubTransactionID = 0;
            Email = string.Empty;
            QualificationDate = DateTime.Now;
            EmailStatusID = 1;
            StatusUpdatedDate = DateTime.Now;
            StatusUpdatedReason = "Subscribed";
            Status = BusinessLogic.Enums.EmailStatus.Active.ToString();
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            SubscriptionStatusID = 1;
            Copies = 1;
            IsPaid = false;
            IsActive = true;

            RegionCode = string.Empty;
            IsAddressValidated = false;

            AccountNumber = string.Empty;
            ClientName = string.Empty;
            FullAddress = string.Empty;
            FullZip = string.Empty;
            IGrp_No = Guid.Empty;
            IMBSeq = string.Empty;
            MemberGroup = string.Empty;
            OnBehalfOf = string.Empty;
            OrigsSrc = string.Empty;
            Par3CID = 0;
            PublicationToolTip = string.Empty;
            SFRecordIdentifier = Guid.Empty;
            SubscriberSourceCode = string.Empty;
            SubSrcID = 0;
            Verified = string.Empty;
            UpdatedByUserID = 0;
            ReqFlag = 0;
        }

        [DataMember]
        public int IssueCompDetailId { get; set; }
        [DataMember]
        public int IssueCompID { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public int EmailStatusID { get; set; }
        [DataMember]
        public bool IsComp { get; set; }
        [DataMember]
        public int Copies { get; set; }
        [DataMember]
        public int GraceIssues { get; set; }
        [DataMember]
        public int SubSrcID { get; set; }
        [DataMember]
        public int Par3CID { get; set; }
        [DataMember]
        public bool IsPaid { get; set; }
        [DataMember]
        public bool IsSubscribed { get; set; }
        [DataMember]
        public string MemberGroup { get; set; }
        [DataMember]
        public string OnBehalfOf { get; set; }
        [DataMember]
        public string SubscriberSourceCode { get; set; }
        [DataMember]
        public string OrigsSrc { get; set; }
        [DataMember]
        public int SequenceID { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string Verified { get; set; }
        [DataMember]
        public int AddRemoveID { get; set; }
        [DataMember]
        public string IMBSeq { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime? LockDate { get; set; }
        [DataMember]
        public DateTime? LockDateRelease { get; set; }
        [DataMember]
        public DateTime? AddressLastUpdatedDate { get; set; }
        [DataMember]
        public Guid? IGrp_No { get; set; }
        [DataMember]
        public int ReqFlag { get; set; }
        [DataMember]
        public Guid? SFRecordIdentifier { get; set; }
        [DataMember]
        public string PublicationToolTip { get; set; }

        public string FullName
        {
            get
            { return FirstName + " " + LastName; }
        }
        [DataMember]
        public string FullZip { get; set; }
        [DataMember]
        public int PhoneCode {get; set;}
        [DataMember]
        public string FullAddress {get; set;}
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string RegionCode { get; set; }
    }
}
