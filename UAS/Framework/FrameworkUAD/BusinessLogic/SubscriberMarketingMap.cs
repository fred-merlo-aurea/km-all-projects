using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class SubscriberMarketingMap
    {
        public List<Object.SubscriberMarketingMap> Select(int subscriberID, KMPlatform.Object.ClientConnections client)
        {
            List<Object.SubscriberMarketingMap> x = null;
            x = DataAccess.SubscriberMarketingMap.Select(subscriberID,client);
            return x;
        }
       
    }
}
