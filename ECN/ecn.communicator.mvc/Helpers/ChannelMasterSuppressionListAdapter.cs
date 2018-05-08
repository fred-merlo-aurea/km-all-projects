using System;
using System.Collections.Generic;
using Ecn.Communicator.Mvc.Interfaces;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;

namespace Ecn.Communicator.Mvc.Helpers
{
    public class ChannelMasterSuppressionListAdapter : IChannelMasterSuppressionList
    {
        public IList<ChannelMasterSuppressionList> GetByEmailAddress(int baseChannelID, string emailAddress, User user)
        {
            return ECN_Framework_BusinessLayer.Communicator.ChannelMasterSuppressionList.GetByEmailAddress(baseChannelID, emailAddress, user);
        }
    }
}
