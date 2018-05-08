using System.Collections.Generic;
using ECN_Framework_Entities.Activity;

namespace ecn.communicator.main.blasts.Interfaces
{
    public interface IPlatforms
    {
        IList<Platforms> Get();
    }
}
