using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class HistorySubscription : BaseUser
    {
        public HistorySubscription() : base(false) { }
        #region Properties

        [DataMember]
        public int HistorySubscriptionID { get; set; }
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
        public string RegionCode { get; set; }
        [DataMember]
        public DateTime? LockDate { get; set; }
        [DataMember]
        public DateTime? LockDateRelease { get; set; }
        [DataMember]
        public DateTime? AddressLastUpdatedDate { get; set; }
        [DataMember]
        public Guid? IGrp_No { get; set; }
        [DataMember]
        public Guid? SFRecordIdentifier { get; set; }
        [DataMember]
        public int SubGenSubscriberID { get; set; }
        public bool? MailPermission { get; set; } //Demo31
        [DataMember]
        public bool? FaxPermission { get; set; } //Demo32
        [DataMember]
        public bool? PhonePermission { get; set; } //Demo33
        [DataMember]
        public bool? OtherProductsPermission { get; set; } //Demo34
        [DataMember]
        public bool? ThirdPartyPermission { get; set; } //Demo35
        [DataMember]
        public bool? EmailRenewPermission { get; set; } //Demo36
        [DataMember]
        public bool? TextPermission { get; set; }
        #endregion
    }
}
