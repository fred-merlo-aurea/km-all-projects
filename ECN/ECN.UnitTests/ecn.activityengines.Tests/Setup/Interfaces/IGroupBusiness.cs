using ECN_Framework_Entities.Communicator;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IGroupBusiness
    {
        Group GetByGroupID_NoAccessCheck(int groupId);
    }
}
