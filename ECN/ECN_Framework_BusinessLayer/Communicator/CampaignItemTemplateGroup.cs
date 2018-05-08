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
    public class CampaignItemTemplateGroup
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CampaignItemTemplateGroup;

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup citg, KMPlatform.Entity.User user)
        {
            int retID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Communicator.CampaignItemTemplateGroup.Save(citg, user);
                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter f in citg.Filters)
                {
                    ECN_Framework_DataLayer.Communicator.CampaignItemTemplateFilter.Save(citg.CampaignItemTemplateID, citg.GroupID, f.FilterID.Value, false, user);
                }
                scope.Complete();
            }
            return retID;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup> GetByCampaignItemTemplateID(int CampaignItemTemplateID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.CampaignItemTemplateGroup.GetByCampaignItemTemplateID(CampaignItemTemplateID);
                foreach (ECN_Framework_Entities.Communicator.CampaignItemTemplateGroup citg in retList)
                {
                    citg.Filters = ECN_Framework_DataLayer.Communicator.CampaignItemTemplateFilter.GetByCampaignItemTemplateIDAndGroupID(CampaignItemTemplateID, citg.GroupID);
                }
                scope.Complete();
            }
            return retList;
        }

        public static void DeleteByCampaignItemTemplateID(int CampaignItemTemplateID, KMPlatform.Entity.User user)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemTemplateGroup.DeleteByCampaignItemTemplateID(CampaignItemTemplateID, user);
                scope.Complete();
            }
        } 
    }
}
