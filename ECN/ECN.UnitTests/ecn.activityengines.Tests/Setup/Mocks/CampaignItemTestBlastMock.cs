using System.Diagnostics.CodeAnalysis;
using ecn.activityengines.Tests.Setup.Interfaces;
using ECN_Framework_Entities.Communicator;
using Moq;
using Shim = ECN_Framework_BusinessLayer.Communicator.Fakes.ShimCampaignItemTestBlast;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class CampaignItemTestBlastMock : Mock<ICampaignItemTestBlast>
    {
        public CampaignItemTestBlastMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.GetByBlastID_NoAccessCheckInt32Boolean = GetByBlastIDNoAccessCheck;
        }

        private CampaignItemTestBlast GetByBlastIDNoAccessCheck(int blastId, bool getChildren)
        {
            return Object.GetByBlastIDNoAccessCheck(blastId, getChildren);
        }
    }
}
