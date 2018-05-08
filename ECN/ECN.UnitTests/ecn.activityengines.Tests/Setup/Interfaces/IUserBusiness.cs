using KMPlatform.Entity;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IUserBusiness
    {
        User GetByAccessKey(string accessKey, bool getChildren);
    }
}
