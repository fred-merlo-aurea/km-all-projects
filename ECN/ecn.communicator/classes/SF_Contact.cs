using System;
using System.Collections.Generic;
using System.Web;
using ecn.communicator.SalesForcePartner;

namespace ecn.communicator.classes
{
    public class SF_Contact
    {
        public SF_Contact()
        {
            ContactId = string.Empty;
            AccountId = string.Empty;
            Email = string.Empty;
            Fax = string.Empty;
            FirstName = string.Empty;
            HomePhone = string.Empty;
            LastName = string.Empty;
            MailingCity = string.Empty;
            MailingState = string.Empty;
            MailingCountry = string.Empty;
            MailingPostalCode = string.Empty;
            MailingStreet = string.Empty;
            MobilePhone = string.Empty;
            Phone = string.Empty;
            Title = string.Empty;
        }
        public string ContactId { get; set; }
        public string AccountId { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string FirstName { get; set; }
        public string HomePhone { get; set; }
        public string LastName { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MailingCountry { get; set; }
        public string MailingPostalCode { get; set; }
        public string MailingStreet { get; set; }
        public string MobilePhone { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
    }
}
