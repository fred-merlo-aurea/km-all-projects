using KMPlatform.Entity;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public interface IValidateFormAndDirectClick
    {
        void Validate(
            Wait parentWait,
            bool hasCampaignItem,
            CampaignItem parentCampaignItem,
            Group parentGroup,
            ControlBase parent,
            User currentUser);
    }
}