using System;
using ECN.Framework.BusinessLayer.Interfaces;
using KMPlatform;
using KMPlatform.Entity;

namespace ECN.Framework.BusinessLayer.Helpers
{
    public class UserAdapter : IUser
    {
        public User GetByUserID(int userID, bool getChildren)
        {
            return KMPlatform.BusinessLogic.User.GetByUserID(userID, getChildren);
        }

        public bool HasAccess(User user, Enums.Services serviceCode, Enums.ServiceFeatures servicefeatureCode, Enums.Access accessCode)
        {
            return KM.Platform.User.HasAccess(user, serviceCode, servicefeatureCode, accessCode);
        }

        public bool IsChannelAdministrator(User user)
        {
            return KM.Platform.User.IsChannelAdministrator(user);
        }

        public bool IsSystemAdministrator(User user)
        {
            return KM.Platform.User.IsSystemAdministrator(user);
        }
    }
}
