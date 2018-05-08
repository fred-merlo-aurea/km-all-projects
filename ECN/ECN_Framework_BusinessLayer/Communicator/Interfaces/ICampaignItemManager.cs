using KMPlatform.Entity;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface ICampaignItemManager
    {
        bool Exists(int campaignId, int campaignItemId, int customerId);
        bool Exists(int campaignId, int customerId);
        bool Exists(string campaignItemName, int campaignId, int customerId);
        CommunicatorEntities.CampaignItem GetByCampaignItemID(int campaignItemId, User user, bool getChildren);
        CommunicatorEntities.CampaignItem GetByCampaignItemID_NoAccessCheck(int campaignItemId, bool getChildren);
    }
}