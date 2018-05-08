using System.Collections.Generic;
using ECN_Framework_Entities.Activity;
using ecn.communicator.main.blasts.Interfaces;

namespace ecn.communicator.main.blasts.Helpers
{
    public class EmailClientsAdapter : IEmailClients
    {
        public IList<EmailClients> Get()
        {
            return ECN_Framework_BusinessLayer.Activity.EmailClients.Get();
        }
    }
}
