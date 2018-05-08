using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface ICampaignItemBlastRefBlastManager
    {
        void Delete(int campaignItemBlastId, User user, bool overrideUpdate = false);

        void Delete(int campaignItemBlastId, int campaignItemBlastRefBlastId, User user);
    }
}