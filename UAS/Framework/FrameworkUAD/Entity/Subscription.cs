using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract(Name = "Subscriber")]
    public class Subscription
    {
        public Subscription()
        {
            IsClientService = false;
            TransactionDate = DateTime.Now;
            QDate = DateTime.Now;
            SuppressedDate = DateTimeFunctions.GetMinDate();
            SubscriptionConsensusDemographics = new List<Object.SubscriberConsensusDemographic>();
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            AddressTypeCodeId = 0;
            AddressLastUpdatedDate = null;
            AddressUpdatedSourceTypeCodeId = 0;
            IsActive = true;
            TransactionID = -1;
            CategoryID = -1;
            ProductList = new List<ProductSubscription>();
            SubscriptionSearchResults = new List<Object.SubscriptionSearchResult>();
            MarketingMapList = new List<MarketingMap>();

            Sequence = 0;
            FName = string.Empty;
            LName = string.Empty;
            Title = string.Empty;
            Company = string.Empty;
            Address = string.Empty;
            MailStop = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            Plus4 = string.Empty;
            ForZip = string.Empty;
            County = string.Empty;
            Country = string.Empty;
            Phone = string.Empty;
            Fax = string.Empty;
            Income = string.Empty;
            Gender = string.Empty;
            Address3 = string.Empty;
            Mobile = string.Empty;
            Score = 0;
            Latitude = 0;
            Longitude = 0;
            Demo7 = string.Empty;
            IGrp_No = Guid.Empty;
            TransactionDate = null;
            QDate = null;
            Email = string.Empty;
            SubscriptionID = 0;
            IsMailable = false;
            Age = 0;
            Birthdate = null;
            Occupation = string.Empty;
            PhoneExt = string.Empty;
            Website = string.Empty;
            FullAddress = string.Empty;
            AccountNumber = string.Empty;
            ExternalKeyId = 0;
            EmailID = 0;
            MailPermission = null;
            FaxPermission = null;
            PhonePermission = null;
            OtherProductsPermission = null;
            ThirdPartyPermission = null;
            EmailRenewPermission = null;
            TextPermission = null;
            SubscriptionConsensusDemographics = new List<Object.SubscriberConsensusDemographic>();
            ProductList = new List<FrameworkUAD.Entity.ProductSubscription>();
            IsClientService = false;

            SubscriptionSearchResults = new List<FrameworkUAD.Object.SubscriptionSearchResult>();
            MarketingMapList = new List<FrameworkUAD.Entity.MarketingMap>();
            SuppressedDate = null;
            SuppressedEmail = string.Empty;
            InSuppression = false;
            Home_Work_Address = string.Empty;
            Par3C = string.Empty;
            RegionID = 0;
            WaveMailingID = 0;
            IsInActiveWaveMailing = false;
            PublicationToolTip = string.Empty;
            FullZip = string.Empty;
            PhoneCode = 0;
            CarrierRoute = string.Empty;
            IsAddressValidated = false;
            IsLocked = false;
            IsNewSubscription = false;
            LockDate = null;
            LockDateRelease = null;
            LockedByUserID = 0;
            AddressValidationDate = null;
            AddressValidationMessage = string.Empty;
            AddressValidationSource = string.Empty;
            tmpSubscriptionID = 0;
            ProductID = 0;
            ProductCode = string.Empty;
            ClientName = string.Empty;
            IsActive = false;
            AddressTypeCodeId = 0;
            AddressLastUpdatedDate = null;
            AddressUpdatedSourceTypeCodeId = 0;
            IsComp = false;
            CountryID = 0;
            PhoneExists = false;
            FaxExists = false;
            EmailExists = false;
            CategoryID = 0;
            TransactionID = 0;
            QSourceID = 0;
            RegCode = string.Empty;
            Verified = string.Empty;
            SubSrc = string.Empty;
            OrigsSrc = string.Empty;
            Source = string.Empty;
            Priority = string.Empty;
            IGrp_Cnt = 0;
            CGrp_No = Guid.Empty;
            CGrp_Cnt = 0;
            StatList = false;
            Sic = string.Empty;
            SicCode = string.Empty;
            IGrp_Rank = string.Empty;
            CGrp_Rank = string.Empty;
            PubIDs = string.Empty;
            IsExcluded = false;
            IsLatLonValid = false;
            LatLonMsg = string.Empty;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 0;
            UpdatedByUserID = null;
        }
        public Subscription(bool isClientService)
        {
            IsClientService = isClientService;
            TransactionDate = DateTime.Now;
            QDate = DateTime.Now;
            SuppressedDate = DateTimeFunctions.GetMinDate();
            SubscriptionConsensusDemographics = new List<Object.SubscriberConsensusDemographic>();
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            AddressTypeCodeId = 0;
            AddressLastUpdatedDate = null;
            AddressUpdatedSourceTypeCodeId = 0;
            IsActive = true;
            TransactionID = -1;
            CategoryID = -1;
            ProductList = new List<ProductSubscription>();
            SubscriptionSearchResults = new List<Object.SubscriptionSearchResult>();
            MarketingMapList = new List<MarketingMap>();
        }
        public Subscription(Entity.Subscription s)
        {
            IsClientService = false;
            this.AccountNumber = s.AccountNumber;
            this.Address = s.Address;
            this.Address3 = s.Address3;
            this.AddressLastUpdatedDate = s.AddressLastUpdatedDate;
            this.AddressTypeCodeId = s.AddressTypeCodeId;
            this.AddressUpdatedSourceTypeCodeId = s.AddressUpdatedSourceTypeCodeId;
            this.AddressValidationDate = s.AddressValidationDate;
            this.AddressValidationMessage = s.AddressValidationMessage;
            this.AddressValidationSource = s.AddressValidationSource;
            this.Age = s.Age;
            this.Birthdate = s.Birthdate;
            this.CarrierRoute = s.CarrierRoute;
            this.CategoryID = s.CategoryID;
            this.CGrp_Cnt = s.CGrp_Cnt;
            this.CGrp_No = s.CGrp_No;
            this.CGrp_Rank = s.CGrp_Rank;
            this.City = s.City;
            this.Company = s.Company;
            this.Country = s.Country;
            this.CountryID = s.CountryID;
            this.County = s.County;
            this.CreatedByUserID = s.CreatedByUserID;
            this.DateCreated = s.DateCreated;
            this.DateUpdated = s.DateUpdated;
            this.Demo7 = s.Demo7;
            this.Email = s.Email;
            this.EmailExists = s.EmailExists;
            this.EmailID = s.EmailID;
            this.ExternalKeyId = s.ExternalKeyId;
            this.Fax = s.Fax;
            this.FaxExists = s.FaxExists;
            this.FName = s.FName;
            this.ForZip = s.ForZip;
            this.FullAddress = s.FullAddress;
            this.FullName = s.FullName;
            this.FullZip = s.FullZip;
            this.Gender = s.Gender;
            this.Home_Work_Address = s.Home_Work_Address;
            this.IGrp_Cnt = s.IGrp_Cnt;
            this.IGrp_No = s.IGrp_No;
            this.IGrp_Rank = s.IGrp_Rank;
            this.Income = s.Income;
            this.InSuppression = s.InSuppression;
            this.IsAddressValidated = s.IsAddressValidated;
            this.IsExcluded = s.IsExcluded;
            this.IsInActiveWaveMailing = s.IsInActiveWaveMailing;
            this.IsLatLonValid = s.IsLatLonValid;
            this.IsLocked = s.IsLocked;
            this.IsNewSubscription = s.IsNewSubscription;
            this.Latitude = s.Latitude;
            this.LatLonMsg = s.LatLonMsg;
            this.LName = s.LName;
            this.LockDate = s.LockDate;
            this.LockDateRelease = s.LockDateRelease;
            this.LockedByUserID = s.LockedByUserID;
            this.Longitude = s.Longitude;
            this.MailStop = s.MailStop;
            this.MarketingMapList = s.MarketingMapList;
            this.Mobile = s.Mobile;
            this.OrigsSrc = s.OrigsSrc;
            this.Par3C = s.Par3C;
            this.Phone = s.Phone;
            this.PhoneCode = s.PhoneCode;
            this.PhoneExists = s.PhoneExists;
            this.PhoneExt = s.PhoneExt;
            this.Plus4 = s.Plus4;
            this.Priority = s.Priority;
            this.IsActive = s.IsActive;
            this.PubIDs = s.PubIDs;
            this.PublicationToolTip = s.PublicationToolTip;
            this.QDate = s.QDate;
            this.QSourceID = s.QSourceID;
            this.RegCode = s.RegCode;
            this.RegionID = s.RegionID;
            this.Score = s.Score;
            this.Sequence = s.Sequence;
            this.Source = s.Source;
            this.State = s.State;
            this.StatList = s.StatList;
            this.SubscriptionConsensusDemographics = s.SubscriptionConsensusDemographics;
            this.SubscriptionID = s.SubscriptionID;
            this.ProductList = s.ProductList;
            this.SubscriptionSearchResults = s.SubscriptionSearchResults;
            this.SubSrc = s.SubSrc;
            this.SuppressedDate = s.SuppressedDate;
            this.SuppressedEmail = s.SuppressedEmail;
            this.Title = s.Title;
            this.TransactionDate = s.TransactionDate;
            this.TransactionID = s.TransactionID;
            this.UpdatedByUserID = s.UpdatedByUserID;
            this.WaveMailingID = s.WaveMailingID;
            this.Website = s.Website;
            this.Zip = s.Zip;
            this.Sic = s.Sic;
            this.SicCode = s.SicCode;
            this.MailPermission = s.MailPermission;
            this.FaxPermission = s.FaxPermission;
            this.PhonePermission = s.PhonePermission;
            this.OtherProductsPermission = s.OtherProductsPermission;
            this.EmailRenewPermission = s.EmailRenewPermission;
            this.ThirdPartyPermission = s.ThirdPartyPermission;
            this.TextPermission = s.TextPermission;
        }

        public Subscription(Entity.IssueCompDetail s)
        {
            IsClientService = false;
            this.AccountNumber = s.AccountNumber;
            this.Address = s.Address1;
            this.Address3 = s.Address3;
            this.AddressLastUpdatedDate = s.AddressLastUpdatedDate;
            this.AddressTypeCodeId = s.AddressTypeCodeId;
            this.AddressUpdatedSourceTypeCodeId = s.AddressUpdatedSourceTypeCodeId;
            this.AddressValidationDate = s.AddressValidationDate;
            this.AddressValidationMessage = s.AddressValidationMessage;
            this.AddressValidationSource = s.AddressValidationSource;
            this.Age = s.Age;
            this.Birthdate = s.Birthdate;
            this.CarrierRoute = s.CarrierRoute;
            this.CategoryID = s.PubCategoryID;
            this.City = s.City;
            this.Company = s.Company;
            this.Country = s.Country;
            this.CountryID = s.CountryID;
            this.County = s.County;
            this.CreatedByUserID = s.CreatedByUserID;
            this.DateCreated = s.DateCreated;
            this.DateUpdated = s.DateUpdated;
            this.Demo7 = s.Demo7;
            this.Email = s.Email;
            this.Fax = s.Fax;
            this.FName = s.FirstName;
            this.Gender = s.Gender;
            this.IGrp_No = s.IGrp_No ?? new Guid();
            this.Income = s.Income;
            this.IsAddressValidated = s.IsAddressValidated;
            this.IsInActiveWaveMailing = s.IsInActiveWaveMailing;
            this.IsLocked = s.IsLocked;
            this.IsNewSubscription = s.IsNewSubscription;
            this.Latitude = s.Latitude;
            this.LName = s.LastName;
            this.LockDate = s.LockDate;
            this.LockDateRelease = s.LockDateRelease;
            this.LockedByUserID = s.LockedByUserID;
            this.Longitude = s.Longitude;
            this.MailStop = s.Address2;
            this.Mobile = s.Mobile;
            this.OrigsSrc = s.OrigsSrc;
            this.Phone = s.Phone;
            this.PhoneExt = s.PhoneExt;
            this.Plus4 = s.Plus4;
            this.QDate = s.QualificationDate;
            this.QSourceID = s.PubQSourceID;
            this.RegionID = s.RegionID;
            this.Sequence = s.SequenceID;
            this.State = s.RegionCode;
            this.SubscriptionID = s.SubscriptionID;
            this.Title = s.Title;
            this.TransactionID = s.PubTransactionID;
            this.UpdatedByUserID = s.UpdatedByUserID;
            this.WaveMailingID = s.WaveMailingID;
            this.Website = s.Website;
            this.Zip = s.ZipCode;
        }
        private string fullName = string.Empty;

        #region Exposed Properties
        [DataMember(Name = "ExternalKeyID")]
        public int Sequence { get; set; }
        [DataMember(Name = "FirstName")]
        public string FName { get; set; }
        [DataMember(Name = "LastName")]
        public string LName { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember(Name = "Address2")]
        public string MailStop { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string Plus4 { get; set; }
        [DataMember]
        public string ForZip { get; set; }
        [DataMember]
        public string County { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Income { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string Address3 { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public int Score { get; set; }
        [DataMember]
        public decimal Latitude { get; set; }
        [DataMember]
        public decimal Longitude { get; set; }
        [DataMember]
        public string Demo7 { get; set; }
        [DataMember]
        public Guid IGrp_No { get; set; }
        [DataMember]
        public DateTime? TransactionDate { get; set; }
        [DataMember]
        public DateTime? QDate { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public bool IsMailable { get; set; }
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public DateTime? Birthdate { get; set; }
        [DataMember]
        public string Occupation { get; set; }
        [DataMember]
        public string PhoneExt { get; set; }
        [DataMember]
        public string Website { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public int ExternalKeyId { get; set; }
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
        //public List<Prospect> ProspectList { get; set; } - commented out on 8/28/2015 Justin.Wagner
        //[DataMember] - commented out on 8/28/2015 Justin.Wagner - did this because same list is nested in the ProductList object
        //public List<FrameworkUAD.Entity.ProductSubscriptionDetail> ProductSubscriptionDetailList { get; set; }
        [DataMember(Name = "SubscriberConsensusDemographics")]
        public List<Object.SubscriberConsensusDemographic> SubscriptionConsensusDemographics { get; set; }
        [DataMember]
        public List<FrameworkUAD.Entity.ProductSubscription> ProductList { get; set; }
        #endregion

        public bool IsClientService { get; set; }
        public bool ShouldSerializeSubscriptionSearchResults()
        {
            if (IsClientService == true)
                return false;
            else
                return true;
        }
        public bool ShouldSerializeMarketingMapList()
        {
            if (IsClientService == true)
                return false;
            else
                return true;
        }

        #region Ignore Properties for ClientServices
        [DataMember]
        //[ConditionalDataMember(IsClientServiceVisible = false)]
        public List<FrameworkUAD.Object.SubscriptionSearchResult> SubscriptionSearchResults { get; set; }
        [DataMember]
        //[ConditionalDataMember(IsClientServiceVisible = false)]
        public List<FrameworkUAD.Entity.MarketingMap> MarketingMapList { get; set; }
        [DataMember]
        public DateTime? SuppressedDate { get; set; }
        [DataMember]
        public string SuppressedEmail { get; set; }
        [DataMember]
        public bool InSuppression { get; set; }
        [DataMember]
        public string Home_Work_Address { get; set; }
        [DataMember]
        public string Par3C { get; set; }
        [DataMember]
        public int RegionID { get; set; }
        [DataMember]
        public int WaveMailingID { get; set; }
        [DataMember]
        public bool IsInActiveWaveMailing { get; set; }
        [DataMember]
        public string PublicationToolTip { get; set; }
        [DataMember]
        public string FullName
        {
            get
            { return FName + " " + LName; }
            set { this.fullName = value; }
        }
        [DataMember]
        public string FullZip { get; set; }
        [DataMember]
        public int PhoneCode { get; set; }
        [DataMember]
        public string CarrierRoute { get; set; }
        [DataMember]
        public bool IsAddressValidated { get; set; }
        [DataMember]
        public bool IsLocked { get; set; }
        [DataMember]
        public bool IsNewSubscription { get; set; }
        [DataMember]
        public DateTime? LockDate { get; set; }
        [DataMember]
        public DateTime? LockDateRelease { get; set; }
        [DataMember]
        public int LockedByUserID { get; set; }
        [DataMember]
        public DateTime? AddressValidationDate { get; set; }
        [DataMember]
        public string AddressValidationMessage { get; set; }
        [DataMember]
        public string AddressValidationSource { get; set; }
        [DataMember]
        public int tmpSubscriptionID { get; set; }
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int AddressTypeCodeId { get; set; }
        [DataMember]
        public DateTime? AddressLastUpdatedDate { get; set; }
        [DataMember]
        public int AddressUpdatedSourceTypeCodeId { get; set; }
        [DataMember]
        public bool IsComp { get; set; }
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public bool PhoneExists { get; set; }
        [DataMember]
        public bool FaxExists { get; set; }
        [DataMember]
        public bool EmailExists { get; set; }
        [DataMember]
        public int CategoryID { get; set; }
        [DataMember]
        public int TransactionID { get; set; }
        [DataMember]
        public int QSourceID { get; set; }
        [DataMember]
        public string RegCode { get; set; }
        [DataMember]
        public string Verified { get; set; }
        [DataMember]
        public string SubSrc { get; set; }
        [DataMember]
        public string OrigsSrc { get; set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public string Priority { get; set; }
        [DataMember]
        public int IGrp_Cnt { get; set; }
        [DataMember]
        public Guid CGrp_No { get; set; }
        [DataMember]
        public int CGrp_Cnt { get; set; }
        [DataMember]
        public bool StatList { get; set; }
        [DataMember]
        public string Sic { get; set; }
        [DataMember]
        public string SicCode { get; set; }
        [DataMember]
        public string IGrp_Rank { get; set; }
        [DataMember]
        public string CGrp_Rank { get; set; }
        [DataMember]
        public string PubIDs { get; set; }
        [DataMember]
        public bool IsExcluded { get; set; }
        [DataMember]
        public bool IsLatLonValid { get; set; }
        [DataMember]
        public string LatLonMsg { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
    }
}
