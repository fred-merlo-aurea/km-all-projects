using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface ICampaignItemBlastManager
    {
        ECN_Framework_Entities.Communicator.CampaignItemBlast GetByBlastId(int blastId, User user, bool getChildren);
        void Delete(int campaignItemId, int campaignItemBlastId, User user);
    }
}