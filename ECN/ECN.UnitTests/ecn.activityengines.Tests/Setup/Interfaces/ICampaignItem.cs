using ECN_Framework_Entities.Communicator;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface ICampaignItem
    {
        CampaignItem GetByCampaignItemTestBlastIDNoAccessCheck(int campaignItemTestBlastId, bool getChildren);

        CampaignItem GetByBlastIDNoAccessCheck(int blastId, bool getChildren);
    }
}
