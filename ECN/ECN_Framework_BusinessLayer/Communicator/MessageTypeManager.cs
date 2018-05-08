using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class MessageTypeManager : IMessageTypeManager
    {
        public EntitiesCommunicator.MessageType GetByMessageTypeID(int messageTypeID, User user)
        {
            return MessageType.GetByMessageTypeID(messageTypeID, user);
        }
    }
}
