using System;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class CampaignItemBlastRefBlastManager : ICampaignItemBlastRefBlastManager
    {
        public void Delete(int campaignItemBlastId, User user, bool overrideUpdate = false)
        {
            CampaignItemBlastRefBlast.Delete(campaignItemBlastId, user, overrideUpdate);
        }

        public void Delete(int campaignItemBlastId, int campaignItemBlastRefBlastId, User user)
        {
            CampaignItemBlastRefBlast.Delete(campaignItemBlastId, campaignItemBlastRefBlastId, user);
        }
    }
}
