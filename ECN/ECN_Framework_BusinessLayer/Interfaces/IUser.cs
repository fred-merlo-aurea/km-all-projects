using KMPlatform;
using KMPlatform.Entity;

namespace ECN.Framework.BusinessLayer.Interfaces
{
    public interface IUser
    {
        bool IsChannelAdministrator(User user);
        bool IsSystemAdministrator(User user);
        bool HasAccess(User user, Enums.Services serviceCode, Enums.ServiceFeatures servicefeatureCode, Enums.Access accessCode);
        User GetByUserID(int userID, bool getChildren);
    }
}
