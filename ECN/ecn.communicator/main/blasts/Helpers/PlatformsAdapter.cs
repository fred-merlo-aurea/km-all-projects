using System.Collections.Generic;
using ECN_Framework_Entities.Activity;
using ecn.communicator.main.blasts.Interfaces;

namespace ecn.communicator.main.blasts.Helpers
{
    public class PlatformsAdapter : IPlatforms
    {
        public IList<Platforms> Get()
        {
            return ECN_Framework_BusinessLayer.Activity.Platforms.Get();
        }
    }
}
