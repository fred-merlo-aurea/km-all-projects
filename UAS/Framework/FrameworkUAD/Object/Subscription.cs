using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract(Name = "ClientSubscriber")]
    public class Subscription
    {
        public Subscription()
        {
            TransactionDate = DateTime.Now;
            QDate = DateTime.Now;
            SubscriptionConsensusDemographics = new List<Object.SubscriberConsensusDemographic>();
            
        }
        public Subscription(bool isClientService)
        {
            TransactionDate = DateTime.Now;
            QDate = DateTime.Now;
            SubscriptionConsensusDemographics = new List<Object.SubscriberConsensusDemographic>();
            
        }
        public Subscription(Object.Subscription s)
        {
            this.AccountNumber = s.AccountNumber;
            this.Address = s.Address;
            this.Address3 = s.Address3;
            this.Age = s.Age;
            this.Birthdate = s.Birthdate;
            this.City = s.City;
            this.Company = s.Company;
            this.Country = s.Country;
            this.County = s.County;
            this.Demo7 = s.Demo7;
            this.Email = s.Email;
            this.EmailID = s.EmailID;
            this.ExternalKeyId = s.ExternalKeyId;
            this.Fax = s.Fax;
            this.FName = s.FName;
            this.ForZip = s.ForZip;
            this.FullAddress = s.FullAddress;
            this.Gender = s.Gender;
            this.IGrp_No = s.IGrp_No;
            this.Income = s.Income;
            this.Latitude = s.Latitude;
            this.LName = s.LName;
            this.Longitude = s.Longitude;
            this.MailStop = s.MailStop;
            this.Mobile = s.Mobile;
            this.Phone = s.Phone;
            this.PhoneExt = s.PhoneExt;
            this.Plus4 = s.Plus4;
            this.QDate = s.QDate;
            this.Score = s.Score;
            this.Sequence = s.Sequence;
            this.State = s.State;
            this.SubscriptionConsensusDemographics = s.SubscriptionConsensusDemographics;
            this.SubscriptionID = s.SubscriptionID;
            this.ProductList = s.ProductList;
            this.Title = s.Title;
            this.TransactionDate = s.TransactionDate;
            this.Website = s.Website;
            this.Zip = s.Zip;
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
            this.AccountNumber = s.AccountNumber;
            this.Address = s.Address1;
            this.Address3 = s.Address3;
            this.Age = s.Age;
            this.Birthdate = s.Birthdate;
            this.City = s.City;
            this.Company = s.Company;
            this.Country = s.Country;
            this.County = s.County;
            this.Demo7 = s.Demo7;
            this.Email = s.Email;
            this.Fax = s.Fax;
            this.FName = s.FirstName;
            this.Gender = s.Gender;
            this.IGrp_No = s.IGrp_No ?? new Guid();
            this.Income = s.Income;
            this.Latitude = s.Latitude;
            this.LName = s.LastName;
            this.Longitude = s.Longitude;
            this.MailStop = s.Address2;
            this.Mobile = s.Mobile;
            this.Phone = s.Phone;
            this.PhoneExt = s.PhoneExt;
            this.Plus4 = s.Plus4;
            this.QDate = s.QualificationDate;
            this.Sequence = s.SequenceID;
            this.State = s.RegionCode;
            this.SubscriptionID = s.SubscriptionID;
            this.Title = s.Title;
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
        [DataMember(Name = "SubscriberConsensusDemographics")]
        public List<Object.SubscriberConsensusDemographic> SubscriptionConsensusDemographics { get; set; }
        [DataMember]
        public List<FrameworkUAD.Object.ProductSubscription> ProductList { get; set; }
        #endregion
    }
}
