using System;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class CampaignManager : ICampaignManager
    {
        public bool Exists(int campaignID, int customerID)
        {
            return Campaign.Exists(campaignID, customerID);
        }

        public bool Exists(int campaignID, string campaignName, int customerID)
        {
            return Campaign.Exists(campaignID, campaignName, customerID);
        }
    }
}
