using System;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using KMPlatform.Entity;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class CampaignItemTestBlastManager : ICampaignItemTestBlastManager
    {
        public CommunicatorEntities.CampaignItemTestBlast GetByBlastId(int blastId, User user, bool getChildren)
        {
            return CampaignItemTestBlast.GetByBlastID(blastId, user, getChildren);
        }

        public void Delete(int campaignItemId, User user)
        {
            CampaignItemTestBlast.Delete(campaignItemId, user);
        }

        public void Delete(int campaignItemId, int campaignItemTestBlastId, User user)
        {
            CampaignItemTestBlast.Delete(campaignItemId, campaignItemTestBlastId, user);
        }
    }
}

