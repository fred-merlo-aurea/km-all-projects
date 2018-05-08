using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class SubscriberProductDemographic
    {
        public List<Object.SubscriberProductDemographic> Select(int subscriptionID, string pubCode, KMPlatform.Object.ClientConnections client)
        {
            List<Object.SubscriberProductDemographic> retList = null;
            retList = DataAccess.SubscriberProductDemographic.Select(subscriptionID, pubCode, client);
            return retList;
        }
      
    }
}
