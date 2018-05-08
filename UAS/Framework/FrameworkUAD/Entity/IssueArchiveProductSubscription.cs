using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class IssueArchiveProductSubscription : BaseUser
    {
        public IssueArchiveProductSubscription() : base(false) 
        {
            IssueArchiveSubscriptionId = 0;
            IsComp = false;
            CompId = 0;
            SubscriptionID = 0;

            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 0;
            UpdatedByUserID = 0;
        }
        public IssueArchiveProductSubscription(ProductSubscription p)
        {
            this.IssueArchiveSubscriptionId = 0;
            this.IssueID = 0;
            this.SplitID = 0;
            this.IsComp = p.IsComp;
            this.CompId = 0;
            this.AccountNumber = p.AccountNumber;
            this.AddRemoveID = p.AddRemoveID;
            this.Copies = p.Copies;
            this.CreatedByUserID = p.CreatedByUserID;
            this.DateCreated = p.DateCreated;
            this.DateUpdated = p.DateUpdated;
            this.Demo7 = p.Demo7;
            this.Email = p.Email;
            this.EmailStatusID = p.EmailStatusID;
            this.GraceIssues = p.GraceIssues;
            this.IMBSeq = p.IMBSeq;
            this.IsActive = p.IsActive;
            this.IsComp = p.IsComp;
            this.IsPaid = p.IsPaid;
            this.IsSubscribed = p.IsSubscribed;
            this.MemberGroup = p.MemberGroup;
            this.OnBehalfOf = p.OnBehalfOf;
            this.OrigsSrc = p.OrigsSrc;
            this.Par3CID = p.Par3CID;
            this.PubCategoryID = p.PubCategoryID;
            this.PubCode = p.PubCode;
            this.PubID = p.PubID;
            this.PubName = p.PubName;
            this.PubQSourceID = p.PubQSourceID;
            this.PubSubscriptionID = p.PubSubscriptionID;
            this.PubTransactionID = p.PubTransactionID;
            this.PubTypeDisplayName = p.PubTypeDisplayName;
            this.QualificationDate = p.QualificationDate;
            this.SequenceID = p.SequenceID;
            this.Status = p.Status;
            this.StatusUpdatedDate = p.StatusUpdatedDate;
            this.StatusUpdatedReason = p.StatusUpdatedReason;
            this.SubscriberSourceCode = p.SubscriberSourceCode;
            this.SubscriptionID = p.SubscriptionID;
            this.SubscriptionStatusID = p.SubscriptionStatusID;
            this.SubSrcID = p.SubSrcID;
            this.UpdatedByUserID = p.UpdatedByUserID;
            this.Verified = p.Verify;
            this.Address1 = p.Address1;
            this.Address2 = p.Address2;
            this.Address3 = p.Address3;
            this.AddressTypeID = p.AddressTypeID;
            this.AddressValidationDate = p.AddressValidationDate;
            this.AddressValidationMessage = p.AddressValidationMessage;
            this.AddressValidationSource = p.AddressValidationSource;
            this.Age = p.Age;
            this.Birthdate = p.Birthdate;
            this.CarrierRoute = p.CarrierRoute;
            this.City = p.City;
            this.Company = p.Company;
            this.Country = p.Country;
            this.CountryID = p.CountryID;
            this.County = p.County;
            this.CreatedByUserID = p.CreatedByUserID;
            this.DateCreated = p.DateCreated;
            this.DateUpdated = p.DateUpdated;
            this.Email = p.Email;
            this.ExternalKeyID = p.ExternalKeyID;
            this.Fax = p.Fax;
            this.FirstName = p.FirstName;
            this.Gender = p.Gender;
            this.Income = p.Income;
            this.IsAddressValidated = p.IsAddressValidated;
            this.IsInActiveWaveMailing = false;
            this.IsLocked = false;
            this.LastName = p.LastName;
            this.Latitude = p.Latitude;
            this.Longitude = p.Longitude;
            this.Mobile = p.Mobile;
            this.Occupation = p.Occupation;
            this.Phone = p.Phone;
            this.PhoneExt = p.PhoneExt;
            this.Plus4 = p.Plus4;
            this.RegionCode = p.RegionCode;
            this.RegionID = p.RegionID;
            this.Title = p.Title;
            this.Website = p.Website;
            this.ZipCode = p.ZipCode;
            this.IsComp = true;
            this.SubGenSubscriberID = p.SubGenSubscriberID;
        }

        [DataMember]
        public int IssueArchiveSubscriptionId { get; set; }
        [DataMember]
        public bool IsComp { get; set; }
        [DataMember]
        public int CompId { get; set; }
        [DataMember]
        public int IssueID { get; set; }
        [DataMember]
        public int SplitID { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public int EmailStatusID { get; set; }
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
        public int ReqFlag { get; set; }
        [DataMember]
        public Guid? SFRecordIdentifier { get; set; }
        [DataMember]
        public int SubGenSubscriberID { get; set; }
        [DataMember]
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
    }
}
