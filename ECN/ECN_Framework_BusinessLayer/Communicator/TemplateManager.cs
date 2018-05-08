using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class TemplateManager : ITemplateManager
    {
        public EntitiesCommunicator.Template GetByTemplateID(int templateID, User user)
        {
            return Template.GetByTemplateID(templateID, user);
        }
    }
}
