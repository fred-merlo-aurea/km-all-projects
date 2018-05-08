using System;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using KMPlatform;
using KMPlatform.Entity;
using Platform = KM.Platform;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class UserManager : IUserManager
    {
        public bool HasAccess(User user, Enums.Services serviceCode, Enums.ServiceFeatures servicefeatureCode, Enums.Access accessCode)
        {
            return Platform.User.HasAccess(user, serviceCode, servicefeatureCode, accessCode);
        }
    }
}
