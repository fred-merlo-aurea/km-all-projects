using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class ContentManager : IContentManager
    {
        public EntitiesCommunicator.Content GetByContentID(int contentID, User user, bool getChildren)
        {
            return Content.GetByContentID(contentID, user, getChildren);
        }
    }
}
