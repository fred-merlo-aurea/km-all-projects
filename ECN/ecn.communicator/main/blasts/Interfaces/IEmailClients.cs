using System.Collections.Generic;
using ECN_Framework_Entities.Activity;

namespace ecn.communicator.main.blasts.Interfaces
{
    public interface IEmailClients
    {
        IList<EmailClients> Get();
    }
}
