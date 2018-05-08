using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class SubscriberConsensusDemographic
    {
        public List<Object.SubscriberConsensusDemographic> Select(int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            List<Object.SubscriberConsensusDemographic> retList = null;
            retList = DataAccess.SubscriberConsensusDemographic.Select(subscriptionID, client);
            return retList;
        }
      
    }
}
