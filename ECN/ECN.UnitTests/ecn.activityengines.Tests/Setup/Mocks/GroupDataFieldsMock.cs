using ecn.activityengines.Tests.Setup.Interfaces;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using Moq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class GroupDataFieldsMock : Mock<IGroupDataFields>
    {
        public GroupDataFieldsMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            ShimGroupDataFields.GetByGroupID_NoAccessCheckInt32 = GetByGroupID_NoAccessCheck;
        }

        private List<GroupDataFields> GetByGroupID_NoAccessCheck(int groupId)
        {
            return Object.GetByGroupID_NoAccessCheck(groupId);
        }
    }
}
