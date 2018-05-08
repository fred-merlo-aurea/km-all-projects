using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FrameworkUAD.Entity;
using KM.Common.Functions;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class ProductSubscription : CoreUserProperty
    {
        public ProductSubscription() : base(false)
        {
            PubSubscriptionID = 0;
            PubID = 0;
            Demo7 = string.Empty;
            PubQSourceID = 0;
            PubCategoryID = 0;
            PubTransactionID = 0;
            Email = string.Empty;
            QualificationDate = DateTime.Now;
            StatusUpdatedDate = DateTime.Now;
            StatusUpdatedReason = "Subscribed";
            Status = BusinessLogic.Enums.EmailStatus.Active.ToString();
            DateCreated = DateTime.Now;
            SubscriptionStatusID = 1;
            Copies = 1;
            SubscriberProductDemographics = new List<Object.SubscriberProductDemographic>();

            ExternalKeyID = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
            Company = string.Empty;
            Title = string.Empty;
            Occupation = string.Empty;
            Address1 = string.Empty;
            Address2 = string.Empty;
            Address3 = string.Empty;
            City = string.Empty;
            RegionCode = string.Empty;
            ZipCode = string.Empty;
            Plus4 = string.Empty;
            CarrierRoute = string.Empty;
            County = string.Empty;
            Country = string.Empty;
            Latitude = 0;
            Longitude = 0;
            Email = string.Empty;
            Phone = string.Empty;
            Fax = string.Empty;
            Mobile = string.Empty;
            Website = string.Empty;
            Birthdate = DateTimeFunctions.GetMinDate();
            Age = 0;
            Income = string.Empty;
            Gender = string.Empty;
            PhoneExt = string.Empty;
            AccountNumber = "0";
            IGrp_No = Guid.Empty;
            OnBehalfOf = string.Empty;
            OrigsSrc = string.Empty;
            SubscriberSourceCode = string.Empty;
            Verify = string.Empty;
            ReqFlag = 0;
            EmailID = 0;

            PubCode = string.Empty;
            PubName = string.Empty;
            PubTypeDisplayName = string.Empty;
            ClientName = string.Empty;
            FullName = string.Empty;
            FullAddress = string.Empty;
        }
        public ProductSubscription(Entity.ProductSubscription p)
        {
            this.AccountNumber = p.AccountNumber;
            this.Copies = p.Copies;
            this.DateCreated = p.DateCreated;
            this.DateUpdated = p.DateUpdated;
            this.Demo7 = p.Demo7;
            this.Email = p.Email;
            this.GraceIssues = p.GraceIssues;
            this.OnBehalfOf = p.OnBehalfOf;
            this.OrigsSrc = p.OrigsSrc;
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
            this.SubscriptionStatusID = p.SubscriptionStatusID;
            this.Verify = p.Verify;
            this.Address1 = p.Address1;
            this.Address2 = p.Address2;
            this.Address3 = p.Address3;
            this.Age = p.Age;
            this.Birthdate = p.Birthdate;
            this.CarrierRoute = p.CarrierRoute;
            this.City = p.City;
            this.Company = p.Company;
            this.Country = p.Country;
            this.County = p.County;
            this.DateCreated = p.DateCreated;
            this.DateUpdated = p.DateUpdated;
            this.Email = p.Email;
            this.ExternalKeyID = p.ExternalKeyID;
            this.Fax = p.Fax;
            this.FirstName = p.FirstName;
            this.Gender = p.Gender;
            this.Income = p.Income;
            this.LastName = p.LastName;
            this.Latitude = p.Latitude;
            this.Longitude = p.Longitude;
            this.Mobile = p.Mobile;
            this.Occupation = p.Occupation;
            this.Phone = p.Phone;
            this.PhoneExt = p.PhoneExt;
            this.Plus4 = p.Plus4;
            this.RegionCode = p.RegionCode;
            this.Title = p.Title;
            this.Website = p.Website;
            this.ZipCode = p.ZipCode;
            this.IGrp_No = p.IGrp_No;
            this.ReqFlag = p.ReqFlag;
            this.EmailID = p.EmailID;
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
            FullAddress = p.FullAddress;
            SubscriberProductDemographics = p.SubscriberProductDemographics;
        }
        public ProductSubscription(Entity.Subscription s)
        {
            int par3c = 0;
            int subsrc = 0;
            int.TryParse(s.Par3C, out par3c);
            int.TryParse(s.SubSrc, out subsrc);

            this.AccountNumber = s.AccountNumber ?? "0";
            this.DateCreated = DateTime.Now;
            this.StatusUpdatedDate = DateTime.Now;
            this.Demo7 = s.Demo7 ?? "";
            this.Email = s.Email ?? "";
            this.OrigsSrc = s.OrigsSrc ?? "";
            this.PubQSourceID = s.QSourceID;
            this.PubSubscriptionID = 0;
            this.QualificationDate = s.QDate;
            this.Verify = s.Verified;
            this.Address1 = s.Address ?? "";
            this.Address2 = s.MailStop ?? "";
            this.Address3 = s.Address3 ?? "";
            this.Age = s.Age;
            this.Birthdate = s.Birthdate ?? DateTime.MinValue;
            this.CarrierRoute = s.CarrierRoute ?? "";
            this.City = s.City ?? "";
            this.Company = s.Company ?? "";
            this.Country = s.Country ?? "";
            this.County = s.County ?? "";
            this.Email = s.Email ?? "";
            this.Fax = s.Fax ?? "";
            this.FirstName = s.FName ?? "";
            this.Gender = s.Gender ?? "";
            this.Income = s.Income ?? "";
            this.LastName = s.LName ?? "";
            this.Latitude = s.Latitude;
            this.Longitude = s.Longitude;
            this.Mobile = s.Mobile ?? "";
            this.Occupation = s.Occupation ?? "";
            this.Phone = s.Phone ?? "";
            this.PhoneExt = s.PhoneExt ?? "";
            this.Plus4 = s.Plus4 ?? "";
            this.RegionCode = s.State ?? "";
            this.Title = s.Title ?? "";
            this.Website = s.Website ?? "";
            this.ZipCode = s.Zip ?? "";
            this.IGrp_No = s.IGrp_No;
            this.Copies = 1;
            this.ReqFlag = 0;
            this.EmailID = s.EmailID;
            this.MailPermission = s.MailPermission;
            this.FaxPermission = s.FaxPermission;
            this.PhonePermission = s.PhonePermission;
            this.OtherProductsPermission = s.OtherProductsPermission;
            this.EmailRenewPermission = s.EmailRenewPermission;
            this.ThirdPartyPermission = s.ThirdPartyPermission;
            this.TextPermission = s.TextPermission;
        }
        public ProductSubscription(FrameworkUAD.Entity.IssueCompDetail comp)
        {
            this.AccountNumber = comp.AccountNumber;
            this.Copies = comp.Copies;
            this.DateCreated = comp.DateCreated;
            this.DateUpdated = comp.DateUpdated;
            this.Demo7 = comp.Demo7;
            this.Email = comp.Email;
            this.GraceIssues = comp.GraceIssues;
            this.OnBehalfOf = comp.OnBehalfOf;
            this.OrigsSrc = comp.OrigsSrc;
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
            this.SubscriptionStatusID = comp.SubscriptionStatusID;
            this.Verify = comp.Verified;
            this.Address1 = comp.Address1;
            this.Address2 = comp.Address2;
            this.Address3 = comp.Address3;
            this.Age = comp.Age;
            this.Birthdate = comp.Birthdate;
            this.CarrierRoute = comp.CarrierRoute;
            this.City = comp.City;
            this.Company = comp.Company;
            this.Country = comp.Country;
            this.County = comp.County;
            this.DateCreated = comp.DateCreated;
            this.DateUpdated = comp.DateUpdated;
            this.Email = comp.Email;
            this.ExternalKeyID = comp.ExternalKeyID;
            this.Fax = comp.Fax;
            this.FirstName = comp.FirstName;
            this.Gender = comp.Gender;
            this.Income = comp.Income;
            this.LastName = comp.LastName;
            this.Latitude = comp.Latitude;
            this.Longitude = comp.Longitude;
            this.Mobile = comp.Mobile;
            this.Occupation = comp.Occupation;
            this.Phone = comp.Phone;
            this.PhoneExt = comp.PhoneExt;
            this.Plus4 = comp.Plus4;
            this.RegionCode = comp.RegionCode;
            this.Title = comp.Title;
            this.Website = comp.Website;
            this.ZipCode = comp.ZipCode;
            this.ReqFlag = comp.ReqFlag;

            PubCode = comp.PubCode;
            PubName = comp.PubName;
            PubTypeDisplayName = comp.PubTypeDisplayName;
            ClientName = comp.ClientName;
            FullName = comp.FullName;
            FullAddress = comp.FullAddress;

            SubscriberProductDemographics = new List<Object.SubscriberProductDemographic>();
        }

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
    }
}
