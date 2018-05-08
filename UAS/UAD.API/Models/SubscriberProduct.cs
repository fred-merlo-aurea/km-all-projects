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
    public class SubscriberProduct
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
        /// MailPermission
        /// </summary>
        [DataMember]
        public bool MailPermission { get; set; }//rename to MailPermission Demo31
        /// <summary>
        /// FaxPermission
        /// </summary>
        [DataMember]
        public bool FaxPermission { get; set; }//rename to FaxPermission Demo32
        /// <summary>
        /// PhonePermission
        /// </summary>
        [DataMember]
        public bool PhonePermission { get; set; }//rename to PhonePermission Demo33
        /// <summary>
        /// OtherProductsPermission
        /// </summary>
        [DataMember]
        public bool OtherProductsPermission { get; set; }//rename to OtherProductsPermission Demo34
        /// <summary>
        /// ThirdPartyPermission
        /// </summary>
        [DataMember]
        public bool ThirdPartyPermission { get; set; }//rename to ThirdPartyPermission Demo35
        /// <summary>
        /// EmailRenewPermission
        /// </summary>
        [DataMember]
        public bool EmailRenewPermission { get; set; }//rename to EmailRenewPermission Demo36
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
        /// Demo7
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
        /// QDate
        /// </summary>
        [DataMember]
        public DateTime? QDate { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [DataMember]
        public string Email { get; set; }
        /// <summary>
        /// SubscriptionID
        /// </summary>
        [DataMember]
        public int SubscriptionID { get; set; }
        /// <summary>
        /// IsActive
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }
        /// <summary>
        /// Demographics
        /// </summary>
        [DataMember]
        public List<Models.SubscriberProductDemographic> Demographics { get; set; }
        #endregion

        public SubscriberProduct()
        {
            this.Sequence = 0;
            this.FName = "";
            this.LName = "";
            this.Title = "";
            this.Company = "";
            this.Address = "";
            this.MailStop = "";
            this.City = "";
            this.State = "";
            this.Zip = "";
            this.Plus4 = "";
            this.ForZip = "";
            this.County = "";
            this.Country = "";
            this.Phone = "";
            this.Fax = "";
            this.MailPermission = false;
            this.FaxPermission = false;
            this.PhonePermission = false;
            this.OtherProductsPermission = false;
            this.ThirdPartyPermission = false;
            this.EmailRenewPermission = false;
            this.Gender = "";
            this.Address3 = "";
            this.Home_Work_Address = "";
            this.Mobile = "";
            this.Score = 0;
            this.Latitude = 0;
            this.Longitude = 0;
            this.Demo7 = "";
            this.IGrp_No = new Guid();
            this.Par3C = "";
            this.TransactionDate = DateTime.Now;
            this.QDate = DateTime.Now;
            this.Email = "";
            this.SubscriptionID = 0;
            this.IsActive = false;
            this.Demographics = new List<SubscriberProductDemographic>();
    }

        public SubscriberProduct(FrameworkUAD.Object.SubscriberProduct sp)
        {
            this.Sequence = sp.Sequence;
            this.FName = sp.FName;
            this.LName = sp.LName;
            this.Title = sp.Title;
            this.Company = sp.Company;
            this.Address = sp.Address;
            this.MailStop = sp.MailStop;
            this.City = sp.City;
            this.State = sp.State;
            this.Zip = sp.Zip;
            this.Plus4 = sp.Plus4;
            this.ForZip = sp.ForZip;
            this.County = sp.County;
            this.Country = sp.Country;
            this.Phone = sp.Phone;
            this.Fax = sp.Fax;
            this.MailPermission = sp.MailPermission;
            this.FaxPermission = sp.FaxPermission;
            this.PhonePermission = sp.PhonePermission;
            this.OtherProductsPermission = sp.OtherProductsPermission;
            this.ThirdPartyPermission = sp.ThirdPartyPermission;
            this.EmailRenewPermission = sp.EmailRenewPermission;
            this.Gender = sp.Gender;
            this.Address3 = sp.Address3;
            this.Home_Work_Address = sp.Home_Work_Address;
            this.Mobile = sp.Mobile;
            this.Score = sp.Score;
            this.Latitude = sp.Latitude;
            this.Longitude = sp.Longitude;
            this.Demo7 = sp.Demo7;
            this.IGrp_No = sp.IGrp_No;
            this.Par3C = sp.Par3C;
            this.TransactionDate = sp.TransactionDate;
            this.QDate = sp.QDate;
            this.Email = sp.Email;
            this.SubscriptionID = sp.SubscriptionID;
            this.IsActive = sp.IsActive;

            this.Demographics = new List<SubscriberProductDemographic>();
            foreach (FrameworkUAD.Object.SubscriberProductDemographic spd in sp.Demographics)
            {
                Models.SubscriberProductDemographic demo = new SubscriberProductDemographic(spd.Name, spd.Value, spd.DemoUpdateAction);
                this.Demographics.Add(demo);
            }
        }
    }
}