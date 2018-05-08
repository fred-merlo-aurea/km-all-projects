using EntitiesCommunicator = ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface IContentManager
    {
        EntitiesCommunicator.Content GetByContentID(int contentID, User user, bool getChildren);
    }
}
