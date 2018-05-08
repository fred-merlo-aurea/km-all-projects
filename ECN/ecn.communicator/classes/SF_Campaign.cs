using System;
using System.Collections.Generic;
using System.Web;

namespace ecn.communicator.classes
{
    public class SF_Campaign
    {
        public SF_Campaign()
        {
            CampaignId = string.Empty;
            ActualCost = 0;
            AmountAllOpportunities = 0;
            AmountWonOpportunities = 0;
            BudgetedCost = 0;
            CampaignMemberRecordTypeId = null;
            Description = string.Empty;
            EndDate = DateTime.MinValue;
            ExpectedResponse = 0;
            ExpectedRevenue = 0;
            Name = string.Empty;
            NumberOfContacts = 0;
            NumberOfConvertedLeads = 0;
            NumberOfLeads = 0;
            NumberOfOpportunities = 0;
            NumberOfResponses = 0;
            NumberOfWonOpportunities = 0;
            NumberSent = 0;
            StartDate = DateTime.MinValue;
        }
        public string CampaignId { get; set; }
        public double ActualCost { get; set; }
        public double AmountAllOpportunities { get; set; }
        public double AmountWonOpportunities { get; set; }
        public double BudgetedCost { get; set; }
        public string CampaignMemberRecordTypeId { get; set; }
        public string Description { get; set; }
        public DateTime  EndDate { get; set; }
        public double ExpectedResponse { get; set; }
        public double ExpectedRevenue { get; set; }
        public string  Name { get; set; }
        public int NumberOfContacts { get; set; }
        public int NumberOfConvertedLeads { get; set; }
        public int NumberOfLeads { get; set; }
        public int NumberOfOpportunities { get; set; }
        public int NumberOfResponses { get; set; }
        public int NumberOfWonOpportunities { get; set; }
        public double NumberSent { get; set; }
        public DateTime StartDate { get; set; }
    }
}
