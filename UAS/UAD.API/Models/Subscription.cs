using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml;
using System.Runtime.Serialization;
using System.Collections;
using System.Xml.Serialization;

namespace UAD.API.Models
{
    [Serializable]
    [DataContract(Namespace = "")]
    
    public class Subscription
    {

        #region Exposed Properties
        /// <summary>
        /// External Key ID
        /// </summary>
        [DataMember(Name = "ExternalKeyID")]        
        public int Sequence { get; set; }
        /// <summary>
        /// First Name
        /// </summary>
        [DataMember(Name = "FirstName")]
        public string FName { get; set; }
        /// <summary>
        /// Last Name
        /// </summary>
        [DataMember(Name = "LastName")]
        public string LName { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// Company
        /// </summary>
        [DataMember]
        public string Company { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        [DataMember]
        public string Address { get; set; }
        /// <summary>
        /// Address 2
        /// </summary>
        [DataMember(Name = "Address2")]
        public string MailStop { get; set; }
        /// <summary>
        /// City
        /// </summary>
        [DataMember]
        public string City { get; set; }
        /// <summary>
        /// State
        /// </summary>
        [DataMember]
        public string State { get; set; }
        /// <summary>
        /// Zip
        /// </summary>
        [DataMember]
        public string Zip { get; set; }
        /// <summary>
        /// Plus 4
        /// </summary>
        [DataMember]
        public string Plus4 { get; set; }
        /// <summary>
        /// Foreign Zip
        /// </summary>
        [DataMember]
        public string ForZip { get; set; }
        /// <summary>
        /// County
        /// </summary>
        [DataMember]
        public string County { get; set; }
        /// <summary>
        /// Country
        /// </summary>
        [DataMember]
        public string Country { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        [DataMember]
        public string Phone { get; set; }
        /// <summary>
        /// Fax
        /// </summary>
        [DataMember]
        public string Fax { get; set; }
        /// <summary>
        /// Income
        /// </summary>
        [DataMember]
        public string Income { get; set; }
        /// <summary>
        /// Gender
        /// </summary>
        [DataMember]
        public string Gender { get; set; }
        /// <summary>
        /// Address 3
        /// </summary>
        [DataMember]
        public string Address3 { get; set; }
        /// <summary>
        /// Mobile
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }
        /// <summary>
        /// Score
        /// </summary>
        [DataMember]
        public int Score { get; set; }
        /// <summary>
        /// Latitude
        /// </summary>
        [DataMember]
        public decimal Latitude { get; set; }
        /// <summary>
        /// Longitude
        /// </summary>
        [DataMember]
        public decimal Longitude { get; set; }
        /// <summary>
        /// Demo 7
        /// </summary>
        [DataMember]
        public string Demo7 { get; set; }
        /// <summary>
        /// IGrp No
        /// </summary>
        [DataMember]
        public Guid IGrp_No { get; set; }
        /// <summary>
        /// Transaction Date
        /// </summary>
        [DataMember]
        public DateTime? TransactionDate { get; set; }
        /// <summary>
        /// Q Date
        /// </summary>
        [DataMember]
        public DateTime? QDate { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [DataMember]
        public string Email { get; set; }
        /// <summary>
        /// Subscription ID
        /// </summary>
        [DataMember]
        public int SubscriptionID { get; set; }
        /// <summary>
        /// Is Mailable
        /// </summary>
        [DataMember]
        public bool IsMailable { get; set; }
        /// <summary>
        /// Age
        /// </summary>
        [DataMember]
        public int Age { get; set; }
        /// <summary>
        /// Birth Date
        /// </summary>
        [DataMember]
        public DateTime? Birthdate { get; set; }
        /// <summary>
        /// Occupation
        /// </summary>
        [DataMember]
        public string Occupation { get; set; }
        /// <summary>
        /// Phone Extension
        /// </summary>
        [DataMember]
        public string PhoneExt { get; set; }
        /// <summary>
        /// Website
        /// </summary>
        [DataMember]
        public string Website { get; set; }
        /// <summary>
        /// Full Address
        /// </summary>
        [DataMember]
        public string FullAddress { get; set; }
        /// <summary>
        /// Account Number
        /// </summary>
        [DataMember]
        public string AccountNumber { get; set; }
        /// <summary>
        /// External Key Id
        /// </summary>
        [DataMember]
        public int ExternalKeyId { get; set; }
        /// <summary>
        /// Email ID
        /// </summary>
        [DataMember]
        public int EmailID { get; set; }
        /// <summary>
        /// Mail Permission
        /// </summary>
        [DataMember]
        public bool? MailPermission { get; set; }
        /// <summary>
        /// Fax Permission
        /// </summary> //Demo31
        [DataMember]
        public bool? FaxPermission { get; set; }
        /// <summary>
        /// Phone Permission
        /// </summary> //Demo32
        [DataMember]
        public bool? PhonePermission { get; set; }
        /// <summary>
        /// Other Products Permission
        /// </summary> //Demo33
        [DataMember]
        public bool? OtherProductsPermission { get; set; }
        /// <summary>
        /// Third Party Permission
        /// </summary> //Demo34
        [DataMember]
        public bool? ThirdPartyPermission { get; set; }
        /// <summary>
        /// Email Renew Permission
        /// </summary> //Demo35
        [DataMember]
        public bool? EmailRenewPermission { get; set; }
        /// <summary>
        /// Text Permission
        /// </summary> //Demo36
        [DataMember]
        public bool? TextPermission { get; set; }
        /// <summary>
        /// Subscriber Consensus Demographics
        /// </summary>
        [DataMember(Name = "SubscriberConsensusDemographics")]
        [Newtonsoft.Json.JsonProperty(PropertyName ="demo")]
        public List<Models.SubscriberConsensusDemographic> SubscriptionConsensusDemographics { get; set; }
        /// <summary>
        /// Product List
        /// </summary>
        [DataMember]        
        public List<Models.ProductSubscription> ProductList { get; set; }
        #endregion


        public bool IsValid()
        {
            return true;
        }

        public Subscription()
        {
            this.AccountNumber = string.Empty;            
            this.Address = string.Empty;
            this.Address3 = string.Empty;
            this.Age = 0;
            this.Birthdate = DateTime.Now;
            this.City = string.Empty;
            this.Company = string.Empty;
            this.Country = string.Empty;
            this.County = string.Empty;
            this.Demo7 = string.Empty;
            this.Email = string.Empty;
            this.EmailID = 0;
            this.ExternalKeyId = 0;
            this.Fax = string.Empty;
            this.FName = string.Empty;
            this.ForZip = string.Empty;
            this.FullAddress = string.Empty;
            this.Gender = string.Empty;
            this.IGrp_No = new Guid();
            this.Income = string.Empty;
            this.IsMailable = false;
            this.Latitude = 0;
            this.LName = string.Empty;
            this.Longitude = 0;
            this.MailStop = string.Empty;
            this.Mobile = string.Empty;
            this.Occupation = string.Empty;
            this.Phone = string.Empty;
            this.PhoneExt = string.Empty;
            this.Plus4 = string.Empty;
            this.QDate = DateTime.Now;
            this.Score = 0;
            this.Sequence = 0;
            this.State = string.Empty;
            this.SubscriptionID = 0;
            this.Title = string.Empty;
            this.TransactionDate = DateTime.Now;
            this.Website = string.Empty;
            this.Zip = string.Empty;
            this.MailPermission = false;
            this.FaxPermission = false;
            this.PhonePermission = false;
            this.OtherProductsPermission = false;
            this.EmailRenewPermission = false;
            this.ThirdPartyPermission = false;
            this.TextPermission = false;

            this.SubscriptionConsensusDemographics = new List<SubscriberConsensusDemographic>();
            this.ProductList = new List<ProductSubscription>();
        }

        public Subscription(FrameworkUAD.Object.Subscription s)
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
            this.IsMailable = s.IsMailable;
            this.Latitude = s.Latitude;
            this.LName = s.LName;
            this.Longitude = s.Longitude;
            this.MailStop = s.MailStop;
            this.Mobile = s.Mobile;
            this.Occupation = s.Occupation;
            this.Phone = s.Phone;
            this.PhoneExt = s.PhoneExt;
            this.Plus4 = s.Plus4;
            this.QDate = s.QDate;
            this.Score = s.Score;
            this.Sequence = s.Sequence;
            this.State = s.State;
            this.SubscriptionConsensusDemographics = new List<SubscriberConsensusDemographic>();
            foreach (FrameworkUAD.Object.SubscriberConsensusDemographic scd in s.SubscriptionConsensusDemographics)
            {
                Models.SubscriberConsensusDemographic demo = new SubscriberConsensusDemographic(scd.Name, scd.DisplayName, scd.Value);
                this.SubscriptionConsensusDemographics.Add(demo);
            }

            this.SubscriptionID = s.SubscriptionID;
            this.ProductList = new List<ProductSubscription>();
            foreach (FrameworkUAD.Object.ProductSubscription ps in s.ProductList)
            {
                Models.ProductSubscription productSubscription = new Models.ProductSubscription(ps);
                this.ProductList.Add(productSubscription);
            }
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
    }
}