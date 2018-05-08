using System;
using System.Collections.Generic;
using System.Web;

namespace ecn.communicator.classes
{
    public class SF_Account
    {
        public SF_Account()
        {
            AccountId = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            AccountNumber = string.Empty;// Max 40 characters
            BillingStreet = string.Empty;
            BillingCity = string.Empty;
            BillingState = string.Empty;
            BillingPostalCode = string.Empty;
            BillingCountry = string.Empty;
            Phone = string.Empty;
            Fax = string.Empty;
        }

        public string AccountId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AccountNumber { get; set; }
        public string BillingStreet { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingPostalCode { get; set; }
        public string BillingCountry { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
}
