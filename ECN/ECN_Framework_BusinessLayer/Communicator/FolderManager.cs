using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class FolderManager : IFolderManager
    {
        public EntitiesCommunicator.Folder GetByFolderID(int folderID, User user)
        {
            return Folder.GetByFolderID(folderID, user);
        }
    }
}
