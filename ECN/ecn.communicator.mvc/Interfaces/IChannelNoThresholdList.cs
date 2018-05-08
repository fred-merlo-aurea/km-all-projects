using System.Collections.Generic;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace Ecn.Communicator.Mvc.Interfaces
{
    public interface IChannelNoThresholdList
    {
        IList<ChannelNoThresholdList> GetByEmailAddress(int baseChannelID, string emailAddress, User user);
    }
}
