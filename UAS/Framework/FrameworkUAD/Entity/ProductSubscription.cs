using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class ProductSubscription: BaseUser
    {
        public ProductSubscription()
        {
            PubSubscriptionID = 0;
            SubscriptionID = 0;
            PubID = 0;
            Demo7 = string.Empty;
            PubQSourceID = 0;
            PubCategoryID = 0;
            PubTransactionID = 0;
            Email = string.Empty;
            QualificationDate = DateTime.Now;
            EmailStatusID = 0;
            StatusUpdatedDate = DateTime.Now;
            StatusUpdatedReason = "Subscribed";
            Status = BusinessLogic.Enums.EmailStatus.Active.ToString();
            
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            SubscriptionStatusID = 1;
            Copies = 1;
            IsPaid = false;
            IsActive = true;

            IsAddressValidated = false;
            MarketingMapList = new List<FrameworkUAD.Entity.MarketingMap>();
            ProspectList = new List<FrameworkUAD.Entity.Prospect>();
            ProductMapList = new List<ProductSubscriptionDetail>();
            SubscriberProductDemographics = new List<Object.SubscriberProductDemographic>();

            RegionCode = string.Empty;

            AccountNumber = "0";
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
            Verify = string.Empty;
            ReqFlag = 0;
            UpdatedByUserID = 0;
            tmpSubscriptionID = 0;
            EmailID = 0;
            SubGenSubscriberID = 0;
            SubGenPublicationID = 0;
            SubGenSubscriptionID = 0;
            SubGenIsLead = false;
            SubGenRenewalCode = string.Empty;
            SubGenSubscriptionRenewDate = null;
            SubGenSubscriptionExpireDate = null;
            SubGenSubscriptionLastQualifiedDate = null;

            PubCode = string.Empty;
            PubName = string.Empty;
            PubTypeDisplayName = string.Empty;
            ClientName = string.Empty;
            FullName = string.Empty;
            FullZip = string.Empty;
            PhoneCode = 0;
            FullAddress = string.Empty;
            AdHocFields = new List<Object.PubSubscriptionAdHoc>();
        }
        public ProductSubscription(ProductSubscription p)
        {
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
            this.PubID = p.PubID;
            this.PubQSourceID = p.PubQSourceID;
            this.PubSubscriptionID = p.PubSubscriptionID;
            this.PubTransactionID = p.PubTransactionID;
            this.QualificationDate = p.QualificationDate;
            this.SequenceID = p.SequenceID;
            this.Status = p.Status;
            this.StatusUpdatedDate = p.StatusUpdatedDate;
            this.StatusUpdatedReason = p.StatusUpdatedReason;
            this.SubscriberProductDemographics = p.SubscriberProductDemographics;
            this.SubscriberSourceCode = p.SubscriberSourceCode;
            this.SubscriptionID = p.SubscriptionID;
            this.SubscriptionStatusID = p.SubscriptionStatusID;
            this.SubSrcID = p.SubSrcID;
            this.UpdatedByUserID = p.UpdatedByUserID;
            this.Verify = p.Verify;
            this.Address1 = p.Address1;
            this.Address2 = p.Address2;
            this.Address3 = p.Address3;
            this.AddressTypeID = p.AddressTypeID;
            this.AddressTypeCodeId = p.AddressTypeCodeId;
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
            this.IsInActiveWaveMailing = p.IsInActiveWaveMailing;
            this.WaveMailingID = p.WaveMailingID;
            this.IsLocked = p.IsLocked;
            this.LockedByUserID = p.LockedByUserID;
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
            this.IsComp = false;
            this.IGrp_No = p.IGrp_No;
            this.ReqFlag = p.ReqFlag;
            this.EmailID = p.EmailID;
            this.SubGenSubscriberID = p.SubGenSubscriberID;
            SubGenPublicationID = 0;
            SubGenSubscriptionID = 0;
            SubGenIsLead = false;
            SubGenRenewalCode = string.Empty;
            SubGenSubscriptionRenewDate = null;
            SubGenSubscriptionExpireDate = null;
            SubGenSubscriptionLastQualifiedDate = null;
            this.MailPermission = p.MailPermission;
            this.FaxPermission = p.FaxPermission;
            this.PhonePermission = p.PhonePermission;
            this.OtherProductsPermission = p.OtherProductsPermission;
            this.EmailRenewPermission = p.EmailRenewPermission;
            this.ThirdPartyPermission = p.ThirdPartyPermission;
            this.TextPermission = p.TextPermission;

            PubCode = p.PubCode;
            PubName = p.PubName;
            PubTypeDisplayName = p.PubTypeDisplayName;
            ClientName = p.ClientName;
            FullName = p.FullName;
            FullZip = p.FullZip;
            PhoneCode = p.PhoneCode;
            FullAddress = p.FullAddress;

            MarketingMapList = p.MarketingMapList;
            ProspectList = p.ProspectList;
            ProductMapList = p.ProductMapList;
            SubscriberProductDemographics = p.SubscriberProductDemographics;
            this.AdHocFields = p.AdHocFields;
        }
        public ProductSubscription(Subscription s)
        {
            int par3c = 0;
            int subsrc = 0;
            int.TryParse(s.Par3C, out par3c);
            int.TryParse(s.SubSrc, out subsrc);

            this.AccountNumber = s.AccountNumber ?? "0";
            this.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            this.DateCreated = DateTime.Now;
            this.StatusUpdatedDate = DateTime.Now;
            this.Demo7 = s.Demo7 ?? "";
            this.Email = s.Email ?? "";
            this.IsActive = s.IsActive;
            this.IsComp = s.IsComp;
            this.OrigsSrc = s.OrigsSrc ?? "";
            this.Par3CID = par3c;
            this.PubQSourceID = s.QSourceID;
            this.PubSubscriptionID = 0;
            this.QualificationDate = s.QDate;
            //this.SequenceID = s.Sequence;
            this.SubscriptionID = s.SubscriptionID;
            this.SubSrcID = subsrc;
            this.Verify = s.Verified;
            this.Address1 = s.Address ?? "";
            this.Address2 = s.MailStop ?? "";
            this.Address3 = s.Address3 ?? "";
            this.AddressTypeCodeId = s.AddressTypeCodeId;
            this.AddressValidationDate = s.AddressValidationDate;
            this.AddressValidationMessage = s.AddressValidationMessage ?? "";
            this.AddressValidationSource = s.AddressValidationSource ?? "";
            this.Age = s.Age;
            this.Birthdate = s.Birthdate ?? DateTime.MinValue;
            this.CarrierRoute = s.CarrierRoute ?? "";
            this.City = s.City ?? "";
            this.Company = s.Company ?? "";
            this.Country = s.Country ?? "";
            this.CountryID = s.CountryID;
            this.County = s.County ?? "";
            this.Email = s.Email ?? "";
            this.Fax = s.Fax ?? "";
            this.FirstName = s.FName ?? "";
            this.Gender = s.Gender ?? "";
            this.Income = s.Income ?? "";
            this.IsAddressValidated = s.IsAddressValidated;
            this.IsInActiveWaveMailing = false;
            this.IsLocked = false;
            this.LastName = s.LName ?? "";
            this.Latitude = s.Latitude;
            this.Longitude = s.Longitude;
            this.Mobile = s.Mobile ?? "";
            this.Occupation = s.Occupation ?? "";
            this.Phone = s.Phone ?? "";
            this.PhoneExt = s.PhoneExt ?? "";
            this.Plus4 = s.Plus4 ?? "";
            this.RegionCode = s.State ?? "";
            this.RegionID = s.RegionID;
            this.Title = s.Title ?? "";
            this.Website = s.Website ?? "";
            this.ZipCode = s.Zip ?? "";
            this.IsComp = false;
            this.IGrp_No = s.IGrp_No;
            this.IsNewSubscription = true;
            this.Copies = 1;
            this.ReqFlag = 0;
            this.EmailID = s.EmailID;
            this.SubGenSubscriberID = 0;
            SubGenPublicationID = 0;
            SubGenSubscriptionID = 0;
            SubGenIsLead = false;
            SubGenRenewalCode = string.Empty;
            SubGenSubscriptionRenewDate = null;
            SubGenSubscriptionExpireDate = null;
            SubGenSubscriptionLastQualifiedDate = null;
            this.MailPermission = s.MailPermission;
            this.FaxPermission = s.FaxPermission;
            this.PhonePermission = s.PhonePermission;
            this.OtherProductsPermission = s.OtherProductsPermission;
            this.EmailRenewPermission = s.EmailRenewPermission;
            this.ThirdPartyPermission = s.ThirdPartyPermission;
            this.TextPermission = s.TextPermission;
            AdHocFields = new List<Object.PubSubscriptionAdHoc>();
        }
        public ProductSubscription(FrameworkUAD.Entity.IssueCompDetail comp)
        {
            this.AccountNumber = comp.AccountNumber;
            this.AddRemoveID = comp.AddRemoveID;
            this.Copies = comp.Copies;
            this.CreatedByUserID = comp.CreatedByUserID;
            this.DateCreated = comp.DateCreated;
            this.DateUpdated = comp.DateUpdated;
            this.Demo7 = comp.Demo7;
            this.Email = comp.Email;
            this.EmailStatusID = comp.EmailStatusID;
            this.GraceIssues = comp.GraceIssues;
            this.IMBSeq = comp.IMBSeq;
            this.IsActive = comp.IsActive;
            this.IsComp = comp.IsComp;
            this.IsPaid = comp.IsPaid;
            this.IsSubscribed = comp.IsSubscribed;
            this.MemberGroup = comp.MemberGroup;
            this.OnBehalfOf = comp.OnBehalfOf;
            this.OrigsSrc = comp.OrigsSrc;
            this.Par3CID = comp.Par3CID;
            this.PubCategoryID = comp.PubCategoryID;
            this.PubID = comp.PubID;
            this.PubQSourceID = comp.PubQSourceID;
            this.PubSubscriptionID = comp.PubSubscriptionID;
            this.PubTransactionID = comp.PubTransactionID;
            this.QualificationDate = comp.QualificationDate;
            this.SequenceID = comp.SequenceID;
            this.Status = comp.Status;
            this.StatusUpdatedDate = comp.StatusUpdatedDate;
            this.StatusUpdatedReason = comp.StatusUpdatedReason;
            this.SubscriberSourceCode = comp.SubscriberSourceCode;
            this.SubscriptionID = comp.SubscriptionID;
            this.SubscriptionStatusID = comp.SubscriptionStatusID;
            this.SubSrcID = comp.SubSrcID;
            this.UpdatedByUserID = comp.UpdatedByUserID;
            this.Verify = comp.Verified;
            this.Address1 = comp.Address1;
            this.Address2 = comp.Address2;
            this.Address3 = comp.Address3;
            this.AddressTypeID = comp.AddressTypeID;
            this.AddressValidationDate = comp.AddressValidationDate;
            this.AddressValidationMessage = comp.AddressValidationMessage;
            this.AddressValidationSource = comp.AddressValidationSource;
            this.Age = comp.Age;
            this.Birthdate = comp.Birthdate;
            this.CarrierRoute = comp.CarrierRoute;
            this.City = comp.City;
            this.Company = comp.Company;
            this.Country = comp.Country;
            this.CountryID = comp.CountryID;
            this.County = comp.County;
            this.CreatedByUserID = comp.CreatedByUserID;
            this.DateCreated = comp.DateCreated;
            this.DateUpdated = comp.DateUpdated;
            this.Email = comp.Email;
            this.ExternalKeyID = comp.ExternalKeyID;
            this.Fax = comp.Fax;
            this.FirstName = comp.FirstName;
            this.Gender = comp.Gender;
            this.Income = comp.Income;
            this.IsAddressValidated = comp.IsAddressValidated;
            this.IsInActiveWaveMailing = false;
            this.IsLocked = false;
            this.LastName = comp.LastName;
            this.Latitude = comp.Latitude;
            this.Longitude = comp.Longitude;
            this.Mobile = comp.Mobile;
            this.Occupation = comp.Occupation;
            this.Phone = comp.Phone;
            this.PhoneExt = comp.PhoneExt;
            this.Plus4 = comp.Plus4;
            this.RegionCode = comp.RegionCode;
            this.RegionID = comp.RegionID;
            this.Title = comp.Title;
            this.Website = comp.Website;
            this.ZipCode = comp.ZipCode;
            this.IsComp = true;
            this.ReqFlag = comp.ReqFlag;
            this.SubGenSubscriberID = 0;
            SubGenPublicationID = 0;
            SubGenSubscriptionID = 0;
            SubGenIsLead = false;
            SubGenRenewalCode = string.Empty;
            SubGenSubscriptionRenewDate = null;
            SubGenSubscriptionExpireDate = null;
            SubGenSubscriptionLastQualifiedDate = null;

            PubCode = comp.PubCode;
            PubName = comp.PubName;
            PubTypeDisplayName = comp.PubTypeDisplayName;
            ClientName = comp.ClientName;
            FullName = comp.FullName;
            FullZip = comp.FullZip;
            PhoneCode = comp.PhoneCode;
            FullAddress = comp.FullAddress;

            MarketingMapList = new List<MarketingMap>();
            ProspectList = new List<Prospect>();
            ProductMapList = new List<ProductSubscriptionDetail>();
            SubscriberProductDemographics = new List<Object.SubscriberProductDemographic>();
            AdHocFields = new List<Object.PubSubscriptionAdHoc>();
        }
        #region DataMember Properties
        [DataMember]
        public int Copies { get; set; }
        [DataMember]
        public int GraceIssues { get; set; }
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
        public string Verify { get; set; }
        [DataMember(Name = "State")]
        public string RegionCode { get; set; }
        [DataMember]
        public Guid? IGrp_No { get; set; }
        [DataMember]
        public int ReqFlag { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public int EmailID { get; set; }
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

        [DataMember(Name = "SubscriberProductDemographics")]
        public List<Object.SubscriberProductDemographic> SubscriberProductDemographics { get; set; }
        #endregion

        #region Ignore Properties for ClientServices
        [DataMember]
        public DateTime? AddressLastUpdatedDate { get; set; }
        [DataMember]
        public int AddRemoveID { get; set; }
        [DataMember]
        public int EmailStatusID { get; set; }
        [DataMember]
        public string FullZip { get; set; }
        [DataMember]
        public string IMBSeq { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsComp { get; set; }
        [DataMember]
        public bool IsPaid { get; set; }
        [DataMember]
        public bool IsSubscribed { get; set; }
        [DataMember]
        public DateTime? LockDate { get; set; }
        [DataMember]
        public DateTime? LockDateRelease { get; set; }
        [DataMember]
        public List<FrameworkUAD.Entity.MarketingMap> MarketingMapList { get; set; }
        [DataMember]
        public string MemberGroup { get; set; }
        [DataMember]
        public int Par3CID { get; set; }
        [DataMember]
        public int PhoneCode { get; set; }
        [DataMember]
        public List<FrameworkUAD.Entity.ProductSubscriptionDetail> ProductMapList { get; set; }
        [DataMember]
        public List<Prospect> ProspectList { get; set; }
        [DataMember]
        public Guid? SFRecordIdentifier { get; set; }
        [DataMember]
        public int SubSrcID { get; set; }
        [DataMember]
        public int tmpSubscriptionID { get; set; }
        [DataMember]
        public int SubGenSubscriberID { get; set; }
        [DataMember]
        public int SubGenSubscriptionID { get; set; }
        [DataMember]
        public int SubGenPublicationID { get; set; }
        [DataMember]
        public int SubGenMailingAddressId { get; set; }
        [DataMember]
        public int SubGenBillingAddressId { get; set; }
        [DataMember]
        public int IssuesLeft { get; set; }
        [DataMember]
        public decimal UnearnedReveue { get; set; }
        [DataMember]
        public bool SubGenIsLead { get; set; } 
        [DataMember]
        public string SubGenRenewalCode { get; set; } 
        [DataMember]
        public DateTime? SubGenSubscriptionRenewDate { get; set; } 
        [DataMember]
        public DateTime? SubGenSubscriptionExpireDate { get; set; } 
        [DataMember]
        public DateTime? SubGenSubscriptionLastQualifiedDate { get; set; }
        [DataMember]
        public List<FrameworkUAD.Object.PubSubscriptionAdHoc> AdHocFields { get; set; }
        [DataMember]
        public string PublicationToolTip { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        #endregion
    }
}
