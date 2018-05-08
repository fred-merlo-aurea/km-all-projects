using KMPlatform;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface IUserManager
    {
        bool HasAccess(User user, Enums.Services serviceCode, Enums.ServiceFeatures servicefeatureCode, Enums.Access accessCode);
    }
}
