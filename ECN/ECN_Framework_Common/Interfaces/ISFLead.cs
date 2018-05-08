using System;

namespace ECN_Framework_Common.Interfaces
{
    public interface ISFLead
    {
        double AnnualRevenue { get; set; }
        string City { get; set; }
        string Company { get; set; }
        string ConvertedAccountId { get; set; }
        string ConvertedContactId { get; set; }
        DateTime ConvertedDate { get; set; }
        string ConvertedOpportunityId { get; set; }
        string Country { get; set; }
        string CreatedById { get; set; }
        DateTime CreatedDate { get; set; }
        string Description { get; set; }
        bool DoNotCall { get; set; }
        string Email { get; set; }
        DateTime EmailBouncedDate { get; set; }
        string EmailBouncedReason { get; set; }
        string Fax { get; set; }
        string FirstName { get; set; }
        bool HasOptedOutOfEmail { get; set; }
        string Id { get; set; }
        string Industry { get; set; }
        bool IsConverted { get; set; }
        bool IsDeleted { get; set; }
        bool IsUnreadByOwner { get; set; }
        string JigsawContactId { get; set; }
        DateTime LastActivityDate { get; set; }
        string LastModifiedById { get; set; }
        DateTime LastModifiedDate { get; set; }
        string LastName { get; set; }
        DateTime LastReferencedDate { get; set; }
        DateTime LastViewedDate { get; set; }
        double Latitude { get; set; }
        string LeadSource { get; set; }
        double Longitude { get; set; }
        string MasterRecordId { get; set; }
        string MobilePhone { get; set; }
        string Name { get; set; }
        string OwnerId { get; set; }
        string Phone { get; set; }
        string PostalCode { get; set; }
        string Rating { get; set; }
        string Salutation { get; set; }
        string State { get; set; }
        string Status { get; set; }
        string Street { get; set; }
        string SystemModstamp { get; set; }
        string Title { get; set; }
        string Website { get; set; }
    }
}