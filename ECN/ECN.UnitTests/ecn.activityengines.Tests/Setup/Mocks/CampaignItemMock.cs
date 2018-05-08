using System.Diagnostics.CodeAnalysis;
using ecn.activityengines.Tests.Setup.Interfaces;
using ECN_Framework_Entities.Communicator;
using Moq;
using Shim = ECN_Framework_BusinessLayer.Communicator.Fakes.ShimCampaignItem;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class CampaignItemMock : Mock<ICampaignItem>
    {
        public CampaignItemMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.GetByCampaignItemTestBlastID_NoAccessCheckInt32Boolean = GetByCampaignItemTestBlastIDNoAccessCheck;
            Shim.GetByBlastID_NoAccessCheckInt32Boolean = GetByBlastIDNoAccessCheck;
        }

        private CampaignItem GetByCampaignItemTestBlastIDNoAccessCheck(int campaignItemTestBlastId, bool getChildren)
        {
            return Object.GetByCampaignItemTestBlastIDNoAccessCheck(campaignItemTestBlastId, getChildren);
        }

        private CampaignItem GetByBlastIDNoAccessCheck(int blastId, bool getChildren)
        {
            return Object.GetByBlastIDNoAccessCheck(blastId, getChildren);
        }
    }
}
