using EntitiesCommunicator = ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface IMessageTypeManager
    {
        EntitiesCommunicator.MessageType GetByMessageTypeID(int messageTypeID, User user);
    }
}
