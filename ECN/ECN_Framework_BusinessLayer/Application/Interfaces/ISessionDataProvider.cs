using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Application.Interfaces
{
    public interface ISessionDataProvider
    {
        int GetCustomerId();

        User GetCurrentUser();
    }
}
