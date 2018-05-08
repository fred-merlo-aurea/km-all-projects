using KMPlatform.Entity;
using EntityCommunicator = ECN_Framework_Entities.Communicator;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public interface IValidateDirectOpenAndNoOpen
    {
        void Validate(
            Wait parentWait,
            bool hasCampaignItem,
            CampaignItem parentCampaignItem,
            Group parentGroup,
            EntityCommunicator.MarketingAutomation marketingAutomation,
            User currentUser);
    }
}