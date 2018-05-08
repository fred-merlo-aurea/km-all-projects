using System;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using KMPlatform.Entity;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class CampaignItemManager : ICampaignItemManager
    {
        public bool Exists(int campaignId, int campaignItemId, int customerId)
        {
            return CampaignItem.Exists(campaignId, campaignItemId, customerId);
        }

        public bool Exists(int campaignId, int customerId)
        {
            return CampaignItem.Exists(campaignId, customerId);
        }

        public bool Exists(string campaignItemName, int campaignId, int customerId)
        {
            return CampaignItem.Exists(campaignItemName, campaignId, customerId);
        }

        public CommunicatorEntities.CampaignItem GetByCampaignItemID(int campaignItemId, User user, bool getChildren)
        {
            return CampaignItem.GetByCampaignItemID(campaignItemId, user, getChildren);
        }

        public CommunicatorEntities.CampaignItem GetByCampaignItemID_NoAccessCheck(int campaignItemId, bool getChildren)
        {
            return CampaignItem.GetByCampaignItemID_NoAccessCheck(campaignItemId, getChildren);
        }
    }
}

