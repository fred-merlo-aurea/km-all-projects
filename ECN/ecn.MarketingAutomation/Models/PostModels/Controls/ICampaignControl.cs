using System.Collections.Generic;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public interface ICampaignControl : IVisualControl
    {
        int CampaignItemTemplateID { get; set; }

        string CampaignItemTemplateName { get; set; }

        bool UseCampaignItemTemplate { get; set; }

        string BlastField1 { get; set; }

        string BlastField2 { get; set; }

        string BlastField3 { get; set; }

        string BlastField4 { get; set; }

        string BlastField5 { get; set; }

        int MessageID { get; set; }

        string MessageName { get; set; }

        string FromEmail { get; set; }

        string ReplyTo { get; set; }

        string FromName { get; set; }

        string EmailSubject { get; set; }

        int? HeatMapStats { get; set; }

        string CampaignItemName { get; set; }

        int CustomerID { get; set; }

        int MarketingAutomationID { get; set; }

        bool IsConfigured { get; set; }

        string control_label { get; }

        string control_text { get; }
    }
}