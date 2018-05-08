using EntitiesCommunicator = ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface ITemplateManager
    {
        EntitiesCommunicator.Template GetByTemplateID(int templateID, User user);
    }
}
