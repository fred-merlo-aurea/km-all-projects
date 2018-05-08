using System;

namespace ECN_Framework_Common.Interfaces
{
    public interface ISFCampaign
    {
        double ActualCost { get; set; }
        double AmountAllOpportunities { get; set; }
        double AmountWonOpportunities { get; set; }
        double BudgetedCost { get; set; }
        string CampaignMemberRecordTypeId { get; set; }
        string CreatedById { get; set; }
        DateTime CreatedDate { get; set; }
        string Description { get; set; }
        DateTime EndDate { get; set; }
        decimal ExpectedResponse { get; set; }
        double ExpectedRevenue { get; set; }
        string Id { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
        DateTime LastActivityDate { get; set; }
        string LastModifiedById { get; set; }
        DateTime LastModifiedDate { get; set; }
        DateTime LastReferencedDate { get; set; }
        DateTime LastViewedDate { get; set; }
        string Name { get; set; }
        int NumberOfContacts { get; set; }
        int NumberOfConvertedLeads { get; set; }
        int NumberOfLeads { get; set; }
        int NumberOfOpportunities { get; set; }
        int NumberOfResponses { get; set; }
        int NumberOfWonOpportunities { get; set; }
        double NumberSent { get; set; }
        string OwnerId { get; set; }
        string ParentId { get; set; }
        DateTime StartDate { get; set; }
        string Status { get; set; }
        DateTime SystemModstamp { get; set; }
        string Type { get; set; }
    }
}