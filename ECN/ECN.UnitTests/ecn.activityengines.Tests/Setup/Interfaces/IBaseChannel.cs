using ECN_Framework_Entities.Accounts;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IBaseChannelBusiness
    {
        BaseChannel GetByBaseChannelID(int baseChannelId);
    }
}
