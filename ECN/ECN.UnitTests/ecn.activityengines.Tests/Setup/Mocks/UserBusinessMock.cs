using ecn.activityengines.Tests.Setup.Interfaces;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class UserBusinessMock: Mock<IUserBusiness>
    {
        public UserBusinessMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            ShimUser.GetByAccessKeyStringBoolean = GetByAccessKey;
        }

        private User GetByAccessKey(string accessKey, bool getChildren)
        {
            return Object.GetByAccessKey(accessKey, getChildren);
        }
    }
}
