using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class CampaignItemTemplateFilter
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CampaignItemTemplateFilter;

        public static List<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup> GetByCampaignItemTemplateID(int CampaignItemTemplateID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.CampaignItemTemplateGroup.GetByCampaignItemTemplateID(CampaignItemTemplateID);
                scope.Complete();
            }
            return retList;
        }

        public static void DeleteByCampaignItemTemplateID(int CampaignItemTemplateID, KMPlatform.Entity.User user)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemTemplateFilter.DeleteByCampaignItemTemplateID(CampaignItemTemplateID, user);
                scope.Complete();
            }
        } 
    }
}
