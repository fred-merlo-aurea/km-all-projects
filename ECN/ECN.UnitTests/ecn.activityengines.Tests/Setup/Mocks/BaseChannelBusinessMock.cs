using System.Diagnostics.CodeAnalysis;
using ecn.activityengines.Tests.Setup.Interfaces;
using ECN_Framework_Entities.Accounts;
using Moq;
using Shim = ECN_Framework_BusinessLayer.Accounts.Fakes.ShimBaseChannel;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class BaseChannelBusinessMock : Mock<IBaseChannelBusiness>
    {
        public BaseChannelBusinessMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.GetByBaseChannelIDInt32 = GetByBaseChannelID;
        }

        private BaseChannel GetByBaseChannelID(int baseChannelId)
        {
            return Object.GetByBaseChannelID(baseChannelId);
        }
    }
}
