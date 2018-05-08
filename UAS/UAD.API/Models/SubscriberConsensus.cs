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
    public class SubscriberConsensus
    {
        #region Properties
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
        /// Mail Permission
        /// </summary>
        [DataMember]
        public bool? MailPermission { get; set; }//rename to MailPermission Demo31
        /// <summary>
        /// Fax Permission
        /// </summary>
        [DataMember]
        public bool? FaxPermission { get; set; }//rename to FaxPermission Demo32
        /// <summary>
        /// Phone Permission
        /// </summary>
        [DataMember]
        public bool? PhonePermission { get; set; }//rename to PhonePermission Demo33
        /// <summary>
        /// Other Products Permission
        /// </summary>
        [DataMember]
        public bool? OtherProductsPermission { get; set; }//rename to OtherProductsPermission Demo34
        /// <summary>
        /// Third Party Permission
        /// </summary>
        [DataMember]
        public bool? ThirdPartyPermission { get; set; }//rename to ThirdPartyPermission Demo35
        /// <summary>
        /// Email Renew Permission
        /// </summary>
        [DataMember]
        public bool? EmailRenewPermission { get; set; }//rename to EmailRenewPermission Demo36
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
        /// Home Work Address
        /// </summary>
        [DataMember]
        public string Home_Work_Address { get; set; }
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
        /// Par3C
        /// </summary>
        [DataMember]
        public string Par3C { get; set; }
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
        /// Is Active
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }
        /// <summary>
        /// Demographics
        /// </summary>
        [DataMember]
        public List<SubscriberConsensusDemographic> Demographics { get; set; }
        #endregion

        public SubscriberConsensus()
        {
            this.Sequence = 0;
            this.FName = string.Empty;
            this.LName = string.Empty;
            this.Title = string.Empty;
            this.Company = string.Empty;
            this.Address = string.Empty;
            this.MailStop = string.Empty;
            this.City = string.Empty;
            this.State = string.Empty;
            this.Zip = string.Empty;
            this.Plus4 = string.Empty;
            this.ForZip = string.Empty;
            this.County = string.Empty;
            this.Country = string.Empty;
            this.Phone = string.Empty;
            this.Fax = string.Empty;
            this.MailPermission = false;
            this.FaxPermission = false;
            this.PhonePermission = false;
            this.OtherProductsPermission = false;
            this.ThirdPartyPermission = false;
            this.EmailRenewPermission = false;
            this.Gender = string.Empty;
            this.Address3 = string.Empty;
            this.Home_Work_Address = string.Empty;
            this.Mobile = string.Empty;
            this.Score = 0;
            this.Latitude = 0;
            this.Longitude = 0;
            this.Demo7 = string.Empty;
            this.IGrp_No = new Guid();
            this.Par3C = string.Empty;
            this.TransactionDate = DateTime.Now;
            this.QDate = DateTime.Now;
            this.Email = string.Empty;
            this.SubscriptionID = 0;
            this.IsActive = false;
            this.Demographics = new List<SubscriberConsensusDemographic>();
        }

        public SubscriberConsensus(FrameworkUAD.Object.SubscriberConsensus sc)
        {
            this.Sequence = sc.Sequence;
            this.FName = sc.FName;
            this.LName = sc.LName;
            this.Title = sc.Title;
            this.Company = sc.Company;
            this.Address = sc.Address;
            this.MailStop = sc.MailStop;
            this.City = sc.City;
            this.State = sc.State;
            this.Zip = sc.Zip;
            this.Plus4 = sc.Plus4;
            this.ForZip = sc.ForZip;
            this.County = sc.County;
            this.Country = sc.Country;
            this.Phone = sc.Phone;
            this.Fax = sc.Fax;
            this.MailPermission = sc.MailPermission;
            this.FaxPermission = sc.FaxPermission;
            this.PhonePermission = sc.PhonePermission;
            this.OtherProductsPermission = sc.OtherProductsPermission;
            this.ThirdPartyPermission = sc.ThirdPartyPermission;
            this.EmailRenewPermission = sc.EmailRenewPermission;
            this.Gender = sc.Gender;
            this.Address3 = sc.Address3;
            this.Home_Work_Address = sc.Home_Work_Address;
            this.Mobile = sc.Mobile;
            this.Score = sc.Score;
            this.Latitude = sc.Latitude;
            this.Longitude = sc.Longitude;
            this.Demo7 = sc.Demo7;
            this.IGrp_No = sc.IGrp_No;
            this.Par3C = sc.Par3C;
            this.TransactionDate = sc.TransactionDate;
            this.QDate = sc.QDate;
            this.Email = sc.Email;
            this.SubscriptionID = sc.SubscriptionID;
            this.IsActive = sc.IsActive;
            this.Demographics = new List<SubscriberConsensusDemographic>();
            foreach (FrameworkUAD.Object.SubscriberConsensusDemographic scd in sc.Demographics)
            {
                Models.SubscriberConsensusDemographic demo = new SubscriberConsensusDemographic(scd.Name, scd.DisplayName, scd.Value);
                this.Demographics.Add(demo);
            }
        }
    }
}