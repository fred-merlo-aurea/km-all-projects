using System.Collections.Generic;
using ECN_Framework_Entities.Communicator;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IGroupDataFields
    {
        List<GroupDataFields> GetByGroupID_NoAccessCheck(int groupId);
    }
}
