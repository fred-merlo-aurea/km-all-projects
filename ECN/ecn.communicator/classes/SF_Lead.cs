using System;
using System.Collections.Generic;
using System.Web;

namespace ecn.communicator.classes
{
    public class SF_Lead
    {
        public SF_Lead()
        {
            LeadId = string.Empty;
            AnnualRevenue = -1;
            City = string.Empty;
            Company = string.Empty;
            ConvertedAccountId = string.Empty;
            ConvertedContactId = string.Empty;
            ConvertedDate = DateTime.MinValue;
            ConvertedOpportunityId = string.Empty;
            Country = string.Empty;
            Description = string.Empty;
            Email = string.Empty;
            EmailBouncedDate = DateTime.MinValue;
            EmailBouncedReason = string.Empty;
            Fax = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            MobilePhone = string.Empty;
            NumberOfEmployees = -1;
            Phone = string.Empty;
            PostalCode = string.Empty;
            State = string.Empty;
            Street = string.Empty;
            Title = string.Empty;
            Website = string.Empty;
        }
        public string LeadId { get; set; }
        public int AnnualRevenue { get; set; }
        public string City { get; set; }
        public string Company { get; set; }
        public string ConvertedAccountId { get; set; }
        public string ConvertedContactId { get; set; }
        public DateTime ConvertedDate { get; set; }
        public string ConvertedOpportunityId { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public DateTime EmailBouncedDate { get; set; }
        public string EmailBouncedReason { get; set; }
        public string Fax { get; set; }
        public string FirstName { get; set; }
        public string HomePhone { get; set; }
        public string LastName { get; set; }
        public string MobilePhone { get; set; }
        public int NumberOfEmployees { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string Website { get; set; }
    }

   
}
