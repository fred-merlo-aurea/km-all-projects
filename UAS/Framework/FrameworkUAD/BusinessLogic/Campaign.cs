using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class Campaign
    {
        public List<Entity.Campaign> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Campaign> x = null;
            x = DataAccess.Campaign.Select(client).ToList();
            return x;
        }
        public int CampaignExists(KMPlatform.Object.ClientConnections client,string cmpName)
        {
            
            return DataAccess.Campaign.CampaignExists(client, cmpName);
        }
        public int GetCountByCampaignID(KMPlatform.Object.ClientConnections client, int CampaignID)
        {

            return DataAccess.Campaign.GetCountByCampaignID(client, CampaignID);
        }

        public int Save(Entity.Campaign x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.CampaignID = DataAccess.Campaign.Save(x, client);
                scope.Complete();
            }

            return x.CampaignID;
        }
    }
}
