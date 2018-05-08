using System;
using System.Collections.Generic;
using System.Web;

namespace ecn.communicator.classes
{
    public class ECN_Contact
    {
        public ECN_Contact()
        {
            EmailAddress = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            State = string.Empty;
            PostalCode = string.Empty;
            Country = string.Empty;
            Company = string.Empty;
            Title = string.Empty;
            Voice = string.Empty;
            Mobile = string.Empty;
            Fax = string.Empty;
            GroupName = string.Empty;
        }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Voice { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string GroupName { get; set; }
    }
}
