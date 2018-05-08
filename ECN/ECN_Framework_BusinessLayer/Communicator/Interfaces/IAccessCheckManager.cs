using System.Collections.Generic;
using KMPlatform.Entity;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface IAccessCheckManager
    {
        bool CanAccessByCustomer(IList<EntitiesCommunicator.Layout> toCheck, User user);
        bool CanAccessByCustomer(EntitiesCommunicator.Layout layout, User user);
    }
}
