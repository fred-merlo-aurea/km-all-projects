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
    public class CampaignItemTemplateOptoutGroup
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CampaignItemTemplateOptoutGroup;

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemTemplateOptoutGroup citg, KMPlatform.Entity.User user)
        {
            int retID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Communicator.CampaignItemTemplateOptoutGroup.Save(citg, user);
                scope.Complete();
            }
            return retID;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemTemplateOptoutGroup> GetByCampaignItemTemplateID(int CampaignItemTemplateID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTemplateOptoutGroup> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemTemplateOptoutGroup>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.CampaignItemTemplateOptoutGroup.GetByCampaignItemTemplateID(CampaignItemTemplateID);
                scope.Complete();
            }
            return retList;
        }

        public static void DeleteByCampaignItemTemplateID(int CampaignItemTemplateID, KMPlatform.Entity.User user)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemTemplateOptoutGroup.DeleteByCampaignItemTemplateID(CampaignItemTemplateID, user);
                scope.Complete();
            }
        } 
    }
}
