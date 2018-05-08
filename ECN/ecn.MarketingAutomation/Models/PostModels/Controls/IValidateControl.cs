using KMPlatform.Entity;
using EntityCommunicator = ECN_Framework_Entities.Communicator;

namespace ecn.MarketingAutomation.Models.PostModels.Controls
{
    public interface IValidateControl
    {
        void Validate(
            Wait parentWait,
            CampaignItem parentCampaignItem,
            EntityCommunicator.MarketingAutomation automation,
            User currentUser);
    }
}