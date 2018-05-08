using System;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using KMPlatform.Entity;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class CampaignItemBlastManager : ICampaignItemBlastManager
    {
        public CommunicatorEntities.CampaignItemBlast GetByBlastId(int blastId, User user, bool getChildren)
        {
            return CampaignItemBlast.GetByBlastID(blastId, user, getChildren);
        }

        public void Delete(int campaignItemId, int campaignItemBlastId, User user)
        {
            CampaignItemBlast.Delete(campaignItemId, campaignItemBlastId, user);
        }
    }
}
