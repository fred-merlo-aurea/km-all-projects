using System.Collections.Generic;
using Ecn.Communicator.Mvc.Interfaces;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace Ecn.Communicator.Mvc.Helpers
{
    public class ChannelNoThresholdListAdapter : IChannelNoThresholdList
    {
        public IList<ChannelNoThresholdList> GetByEmailAddress(int baseChannelID, string emailAddress, User user)
        {
            return ECN_Framework_BusinessLayer.Communicator.ChannelNoThresholdList.GetByEmailAddress(baseChannelID, emailAddress, user);
        }
    }
}
