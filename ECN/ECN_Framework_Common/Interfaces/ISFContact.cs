using System;

namespace ECN_Framework_Common.Interfaces
{
    public interface ISFContact
    {
        string AccountId { get; set; }
        string AssistantName { get; set; }
        string AssistantPhone { get; set; }
        DateTime BirthDate { get; set; }
        string CreatedById { get; set; }
        DateTime CreatedDate { get; set; }
        string Department { get; set; }
        string Description { get; set; }
        bool DoNotCall { get; set; }
        string Email { get; set; }
        DateTime EmailBouncedDate { get; set; }
        string EmailBouncedReason { get; set; }
        string Fax { get; set; }
        string FirstName { get; set; }
        bool HasOptedOutOfEmail { get; set; }
        string HomePhone { get; set; }
        string Id { get; set; }
        bool IsDeleted { get; set; }
        string JigsawContactId { get; set; }
        DateTime LastActivityDate { get; set; }
        DateTime LastCURequestDate { get; set; }
        string LastModifiedById { get; set; }
        DateTime LastModifiedDate { get; set; }
        string LastName { get; set; }
        DateTime LastReferencedDate { get; set; }
        DateTime LastViewedDate { get; set; }
        string LeadSource { get; set; }
        string MailingCity { get; set; }
        string MailingCountry { get; set; }
        double MailingLatitude { get; set; }
        double MailingLongitude { get; set; }
        string MailingPostalCode { get; set; }
        string MailingState { get; set; }
        string MailingStreet { get; set; }
        bool Master_Suppressed__c { get; set; }
        string MasterRecordId { get; set; }
        string MobilePhone { get; set; }
        string Name { get; set; }
        string OtherCity { get; set; }
        string OtherCountry { get; set; }
        double OtherLatitude { get; set; }
        double OtherLongitude { get; set; }
        string OtherPhone { get; set; }
        string OtherPostalCode { get; set; }
        string OtherState { get; set; }
        string OtherStreet { get; set; }
        string OwnerId { get; set; }
        string Phone { get; set; }
        string Salutation { get; set; }
        DateTime SystemModstamp { get; set; }
        string Title { get; set; }
    }
}