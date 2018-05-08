using EntitiesCommunicator = ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface IFolderManager
    {
        EntitiesCommunicator.Folder GetByFolderID(int folderID, User user);
    }
}
