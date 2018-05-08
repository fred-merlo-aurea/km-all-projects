using ECN_Framework_Entities.Communicator;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface ICampaignItemTestBlast
    {
        CampaignItemTestBlast GetByBlastIDNoAccessCheck(int blastId, bool getChildren);
    }
}
