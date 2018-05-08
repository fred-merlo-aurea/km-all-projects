using System.Collections.Generic;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class ConversionLinksManager : IConversionLinksManager
    {
        public IList<EntitiesCommunicator.ConversionLinks> GetByLayoutID(int layoutID, User user)
        {
            return ConversionLinks.GetByLayoutID(layoutID, user);
        }
    }
}
