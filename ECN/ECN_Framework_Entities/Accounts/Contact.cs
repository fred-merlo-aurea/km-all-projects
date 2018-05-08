using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Accounts
{
    [Serializable]
    [DataContract]
    public class Contact
    {
        public Contact()
        {
            BillingContactID = -1;
            CustomerID = -1;
            Salutation = ECN_Framework_Common.Objects.Accounts.Enums.Salutation.Unknown;
            ContactName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            ContactTitle = string.Empty;
            Phone = string.Empty;
            Fax = string.Empty;
            Email = string.Empty;
            StreetAddress = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Country = string.Empty;
            Zip = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }

        public Contact(string salutation, string firstName, string lastName, string title, string phone, string fax, string email,
            string address, string city, string state, string country, string zip)
        {
            if (salutation.EndsWith("."))
                salutation = salutation.TrimEnd('.');
            Salutation = (ECN_Framework_Common.Objects.Accounts.Enums.Salutation)Enum.Parse(typeof(ECN_Framework_Common.Objects.Accounts.Enums.Salutation), salutation);
            FirstName = firstName;
            LastName = lastName;
            ContactName = string.Format("{0} {1}", firstName, lastName);
            ContactTitle = title;
            Phone = phone;
            Fax = fax;
            Email = email;
            StreetAddress = address;
            City = city;
            State = state;
            Country = country;
            Zip = zip;
        }

        [DataMember]
        public int BillingContactID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public ECN_Framework_Common.Objects.Accounts.Enums.Salutation Salutation { get; set; }
        [DataMember]
        public string ContactName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string ContactTitle { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string StreetAddress { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; private set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; private set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
    }
}
