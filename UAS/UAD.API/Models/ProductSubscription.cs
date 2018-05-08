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
    public class ProductSubscription
    {
        #region DataMember Properties
        /// <summary>
        /// Pub ID
        /// </summary>
        [DataMember]
        public int PubID { get; set; }
        /// <summary>
        /// Pub Subscription ID
        /// </summary>
        [DataMember]
        public int PubSubscriptionID { get; set; }
        /// <summary>
        /// Subscription Status ID
        /// </summary>
        [DataMember]
        public int SubscriptionStatusID { get; set; }
        /// <summary>
        /// Demo 7
        /// </summary>
        [DataMember]
        public string Demo7 { get; set; }
        /// <summary>
        /// Q Date
        /// </summary>
        [DataMember(Name = "QDate")]
        public DateTime? QualificationDate { get; set; }
        /// <summary>
        /// Q Source ID
        /// </summary>
        [DataMember(Name = "QSourceID")]
        public int PubQSourceID { get; set; }
        /// <summary>
        /// Category ID
        /// </summary>
        [DataMember(Name = "CategoryID")]
        public int PubCategoryID { get; set; }
        /// <summary>
        /// Transaction ID
        /// </summary>
        [DataMember(Name = "TransactionID")]
        public int PubTransactionID { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [DataMember]
        public string Email { get; set; }
        /// <summary>
        /// Status Updated Date
        /// </summary>
        [DataMember]
        public DateTime StatusUpdatedDate { get; set; }
        /// <summary>
        /// Status Updated Reason
        /// </summary>
        [DataMember]
        public string StatusUpdatedReason { get; set; }
        /// <summary>
        /// Date Created
        /// </summary>
        [DataMember]
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Date Updated
        /// </summary>
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        /// <summary>
        /// Email Status
        /// </summary>
        [DataMember(Name = "EmailStatus")]
        public string Status { get; set; }
        /// <summary>
        /// Copies
        /// </summary>
        [DataMember]
        public int Copies { get; set; }
        /// <summary>
        /// Grace Issues
        /// </summary>
        [DataMember]
        public int GraceIssues { get; set; }
        /// <summary>
        /// On Behalf Of
        /// </summary>
        [DataMember]
        public string OnBehalfOf { get; set; }
        /// <summary>
        /// Subscriber Source Code
        /// </summary>
        [DataMember]
        public string SubscriberSourceCode { get; set; }
        /// <summary>
        /// Origs Src
        /// </summary>
        [DataMember]
        public string OrigsSrc { get; set; }
        /// <summary>
        /// Sequence ID
        /// </summary>
        [DataMember]
        public int SequenceID { get; set; }
        /// <summary>
        /// Account Number
        /// </summary>
        [DataMember]
        public string AccountNumber { get; set; }
        /// <summary>
        /// Verify
        /// </summary>
        [DataMember]
        public string Verify { get; set; }
        /// <summary>
        /// External Key ID
        /// </summary>
        [DataMember]
        public int ExternalKeyID { get; set; }
        /// <summary>
        /// First Name
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }
        /// <summary>
        /// Last Name
        /// </summary>
        [DataMember]
        public string LastName { get; set; }
        /// <summary>
        /// Company
        /// </summary>
        [DataMember]
        public string Company { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// Occupation
        /// </summary>
        [DataMember]
        public string Occupation { get; set; }
        /// <summary>
        /// Address 1
        /// </summary>
        [DataMember]
        public string Address1 { get; set; }
        /// <summary>
        /// Address 2
        /// </summary>
        [DataMember]
        public string Address2 { get; set; }
        /// <summary>
        /// Address 3
        /// </summary>
        [DataMember]
        public string Address3 { get; set; }
        /// <summary>
        /// City
        /// </summary>
        [DataMember]
        public string City { get; set; }
        /// <summary>
        /// State
        /// </summary>
        [DataMember(Name = "State")]
        public string RegionCode { get; set; }
        /// <summary>
        /// Zip Code
        /// </summary>
        [DataMember]
        public string ZipCode { get; set; }
        /// <summary>
        /// Plus 4
        /// </summary>
        [DataMember]
        public string Plus4 { get; set; }
        /// <summary>
        /// Carrier Route
        /// </summary>
        [DataMember]
        public string CarrierRoute { get; set; }
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
        /// Mobile
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }
        /// <summary>
        /// Website
        /// </summary>
        [DataMember]
        public string Website { get; set; }
        /// <summary>
        /// Birth date
        /// </summary>
        [DataMember]
        public DateTime Birthdate { get; set; }
        /// <summary>
        /// Age
        /// </summary>
        [DataMember]
        public int Age { get; set; }
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
        /// Phone Extension
        /// </summary>
        [DataMember]
        public string PhoneExt { get; set; }
        /// <summary>
        /// IGrp No
        /// </summary>
        [DataMember]
        public Guid? IGrp_No { get; set; }
        /// <summary>
        /// Req Flag
        /// </summary>
        [DataMember]
        public int ReqFlag { get; set; }
        /// <summary>
        /// Product
        /// </summary>
        [DataMember]
        public string PubCode { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [DataMember(Name = "Name")]
        public string PubName { get; set; }
        /// <summary>
        /// Pub Type
        /// </summary>
        [DataMember(Name = "PubType")]
        public string PubTypeDisplayName { get; set; }
        /// <summary>
        /// Client Name
        /// </summary>
        [DataMember]
        public string ClientName { get; set; }
        /// <summary>
        /// Full Name
        /// </summary>
        [DataMember]
        public string FullName { get; set; }
        /// <summary>
        /// Full Address
        /// </summary>
        [DataMember]
        public string FullAddress { get; set; }
        /// <summary>
        /// Email ID
        /// </summary>
        [DataMember]
        public int EmailID { get; set; }
        /// <summary>
        /// Mail Permission
        /// </summary>
        [DataMember]
        public bool? MailPermission { get; set; } //Demo31
        /// <summary>
        /// Fax Permission
        /// </summary>
        [DataMember]
        public bool? FaxPermission { get; set; } //Demo32
        /// <summary>
        /// Phone Permission
        /// </summary>
        [DataMember]
        public bool? PhonePermission { get; set; } //Demo33
        /// <summary>
        /// Other Products Permission
        /// </summary>
        [DataMember]
        public bool? OtherProductsPermission { get; set; } //Demo34
        /// <summary>
        /// Third Party Permission
        /// </summary>
        [DataMember]
        public bool? ThirdPartyPermission { get; set; } //Demo35
        /// <summary>
        /// Email Renew Permission
        /// </summary>
        [DataMember]
        public bool? EmailRenewPermission { get; set; } //Demo36
        /// <summary>
        /// Text Permission
        /// </summary>
        [DataMember]
        public bool? TextPermission { get; set; }
        /// <summary>
        /// Subscriber Product Demographics
        /// </summary>
        [DataMember(Name = "SubscriberProductDemographics")]
        public List<Models.SubscriberProductDemographic> SubscriberProductDemographics { get; set; }
        #endregion

        public ProductSubscription()
        {
            this.AccountNumber = string.Empty;
            this.Copies = 0;
            this.DateCreated = DateTime.Now;
            this.DateUpdated = DateTime.Now;
            this.Demo7 = string.Empty;
            this.Email = string.Empty;
            this.GraceIssues = 0;
            this.OnBehalfOf = string.Empty;
            this.OrigsSrc = string.Empty;
            this.PubCategoryID = 0;
            this.PubID = 0;
            this.PubQSourceID = 0;
            this.PubSubscriptionID = 0;
            this.PubTransactionID = 0;
            this.QualificationDate = DateTime.Now;
            this.SequenceID = 0;
            this.Status = string.Empty;
            this.StatusUpdatedDate = DateTime.Now;
            this.StatusUpdatedReason = string.Empty;
            this.SubscriberSourceCode = string.Empty;
            this.SubscriptionStatusID = 0;
            this.Verify = string.Empty;
            this.Address1 = string.Empty;
            this.Address2 = string.Empty;
            this.Address3 = string.Empty;
            this.Age = 0;
            this.Birthdate = DateTime.Now;
            this.CarrierRoute = string.Empty;
            this.City = string.Empty;
            this.Company = string.Empty;
            this.Country = string.Empty;
            this.County = string.Empty;
            this.ExternalKeyID = 0;
            this.Fax = string.Empty;
            this.FirstName = string.Empty;
            this.Gender = string.Empty;
            this.Income = string.Empty;
            this.LastName = string.Empty;
            this.Latitude = 0;
            this.Longitude = 0;
            this.Mobile = string.Empty;
            this.Occupation = string.Empty;
            this.Phone = string.Empty;
            this.PhoneExt = string.Empty;
            this.Plus4 = string.Empty;
            this.RegionCode = string.Empty;
            this.Title = string.Empty;
            this.Website = string.Empty;
            this.ZipCode = string.Empty;
            this.IGrp_No = new Guid();
            this.ReqFlag = 0;
            this.EmailID = 0;
            this.MailPermission = false;
            this.FaxPermission = false;
            this.PhonePermission = false;
            this.OtherProductsPermission = false;
            this.EmailRenewPermission = false;
            this.ThirdPartyPermission = false;
            this.TextPermission = false;

            PubCode = string.Empty;
            PubName = string.Empty;
            PubTypeDisplayName = string.Empty;
            ClientName = string.Empty;
            FullName = string.Empty;
            FullAddress = string.Empty;

            this.SubscriberProductDemographics = new List<SubscriberProductDemographic>();            
        }

        public ProductSubscription(FrameworkUAD.Object.ProductSubscription p)
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
            this.SubscriberProductDemographics = new List<SubscriberProductDemographic>();
            foreach (FrameworkUAD.Object.SubscriberProductDemographic spd in p.SubscriberProductDemographics)
            {
                Models.SubscriberProductDemographic demo = new SubscriberProductDemographic(spd.Name, spd.Value, spd.DemoUpdateAction);
                this.SubscriberProductDemographics.Add(demo);
            }
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
            //SubscriberProductDemographics = p.SubscriberProductDemographics;            
        }
    }
}