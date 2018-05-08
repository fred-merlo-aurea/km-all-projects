using CmmunicatorEntities = ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface ICampaignItemTestBlastManager
    {
        CmmunicatorEntities.CampaignItemTestBlast GetByBlastId(int blastId, User user, bool getChildren);

        void Delete(int campaignItemId, User user);

        void Delete(int campaignItemId, int campaignItemTestBlastId, User user);
    }
}