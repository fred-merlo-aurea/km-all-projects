using System.Collections.Generic;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface IConversionLinksManager
    {
        IList<EntitiesCommunicator.ConversionLinks> GetByLayoutID(int layoutID, User user);
    }
}
