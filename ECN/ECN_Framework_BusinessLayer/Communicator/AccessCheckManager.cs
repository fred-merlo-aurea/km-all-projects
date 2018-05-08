using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class AccessCheckManager : IAccessCheckManager
    {
        public bool CanAccessByCustomer(IList<EntitiesCommunicator.Layout> toCheck, User user)
        {
            return AccessCheck.CanAccessByCustomer(toCheck, user);
        }

        public bool CanAccessByCustomer(EntitiesCommunicator.Layout toCheck, User user)
        {
            return AccessCheck.CanAccessByCustomer(toCheck, user);
        }
    }
}
