using System;
using System.Runtime.Serialization;
using ECN_Framework_Common.Interfaces;

namespace ECN_Framework_Entities.Salesforce
{
    public class LeadBase : SalesForceBase, ISFLead
    {
        public LeadBase()
        {
            Id = string.Empty;
            IsDeleted = false;
            MasterRecordId = string.Empty;
            Title = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Salutation = string.Empty;
            Name = string.Empty;
            Company = string.Empty;
            Street = string.Empty;
            City = string.Empty;
            State = string.Empty;
            PostalCode = string.Empty;
            Country = string.Empty;
            Latitude = 0.0;
            Longitude = 0.0;
            Email = string.Empty;
            Fax = string.Empty;
            MobilePhone = string.Empty;
            Phone = string.Empty;
            Website = string.Empty;
            Description = string.Empty;
        }

        public string Id { get; set; }
        public bool IsDeleted { get; set; }
        public string MasterRecordId { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public string Website { get; set; }
        public string Status { get; set; }
        public string Industry { get; set; }
        public string Rating { get; set; }
        public double AnnualRevenue { get; set; }
        public bool IsConverted { get; set; }
        public DateTime ConvertedDate { get; set; }
        public string ConvertedAccountId { get; set; }
        public string ConvertedContactId { get; set; }
        public string ConvertedOpportunityId { get; set; }
        public bool IsUnreadByOwner { get; set; }
        public string SystemModstamp { get; set; }
    }
}
