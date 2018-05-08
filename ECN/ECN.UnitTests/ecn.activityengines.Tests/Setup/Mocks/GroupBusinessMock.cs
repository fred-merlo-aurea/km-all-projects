using ecn.activityengines.Tests.Setup.Interfaces;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class GroupBusinessMock:Mock<IGroupBusiness>
    {
        public GroupBusinessMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = GetByGroupID_NoAccessCheck;
        }

        private Group GetByGroupID_NoAccessCheck(int groupId)
        {
            return Object.GetByGroupID_NoAccessCheck(groupId);
        }
    }
}
