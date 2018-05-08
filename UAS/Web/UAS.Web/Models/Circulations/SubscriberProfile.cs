using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UAS.Web.Models.Circulations
{
    public class SubscriberProfile
    {
        [Display(Name = "First Name:")]
        public string FirstName
        {
            get;
            set;
        }
        [Display(Name = "Last Name:")]
        public string LastName
        {
            get;
            set;
        }
        [Display(Name = "Title:")]
        public string Title
        {
            get;
            set;
        }
        [Display(Name = "Company:")]
        public string Company
        {
            get;
            set;
        }
        [Display(Name = "AddressType:")]
        public int? AddressTypeID
        {
            get;
            set;
        }
        [Display(Name = "Address1:")]
        public string Address1
        {
            get;
            set;
        }
        [Display(Name = "Address2:")]
        public string Address2
        {
            get;
            set;
        }
        [Display(Name = "Address3:")]
        public string Address3
        {
            get;
            set;
        }
        [Display(Name = "City:")]
        public string City
        {
            get;
            set;
        }
        [Display(Name = "Country:")]
        public int? CountryID
        {
            get;
            set;
        }
        public string Country
        {
            get;
            set;
        }

        [Display(Name = "Region:")]
        public int? RegionID
        {
            get;
            set;
        }
        public string RegionCode
        {
            get;
            set;
        }
        [Display(Name = "Zip:")]
        public string FullZip
        {
            get;
            set;
        }
        [Display(Name = "County:")]
        public string County
        {
            get;
            set;
        }
        [Display(Name = "Phone:")]
        public string Phone
        {
            get;
            set;
        }
        public int PhoneCode
        {
            get;
            set;
        }
        public string PhoneExt
        {
            get;
            set;
        }
        [Display(Name = "Mobile:")]
        public string Mobile
        {
            get;
            set;
        }
        [Display(Name = "Fax:")]
        public string Fax
        {
            get;
            set;
        }
        [Display(Name = "Email:")]
        public string Email
        {
            get;
            set;
        }
        [Display(Name = "Website:")]
        public string Website
        {
            get;
            set;
        }
        [Display(Name = "Sequence #:")]
        public int SequenceID
        {
            get;
            set;
        }
        [Display(Name = "SubscriptionID:")]
        public int SubscriptionID
        {
            get;
            set;
        }
        public int PubSubscriptionID
        {
            get;
            set;
        }
        public int PubID
        {
            get;
            set;
        }
        public int PaidFreeTransactionCode { get; set; }

        public bool IsInActiveWaiveMailing
        {
            get;
            set;
        }
        public SubscriberProfile()
        {
            FirstName = "";
            LastName = "";
            Title = "";
            Company = "";
            AddressTypeID = null;
            Address1 = "";
            Address2 = "";
            Address3 = "";
            City = "";
            County = "";
            Country = "UNITED STATES";
            CountryID = 1;
            Mobile = "";
            Fax = "";
            Email = "";
            Phone = "";
            PhoneCode = 1;
            PhoneExt = "";
            SequenceID = 0;
            PubID = 0;
            PubSubscriptionID = 0;
            SubscriptionID = 0;
            IsInActiveWaiveMailing = false;

        }
    }
}