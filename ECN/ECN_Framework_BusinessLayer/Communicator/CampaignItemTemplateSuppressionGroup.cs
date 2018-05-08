using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class CampaignItemTemplateSuppressionGroup
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CampaignItemTemplateSuppressionGroup;

        public static void Save(ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup CampaignItemTemplateSuppressionGroup, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemTemplateSuppressionGroup.Save(CampaignItemTemplateSuppressionGroup);
                foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter f in CampaignItemTemplateSuppressionGroup.Filters)
                {
                    ECN_Framework_DataLayer.Communicator.CampaignItemTemplateFilter.Save(CampaignItemTemplateSuppressionGroup.CampaignItemTemplateID.Value, CampaignItemTemplateSuppressionGroup.GroupID.Value, f.FilterID.Value, false, user);
                }
                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup> GetByCampaignItemTemplateID(int CampaignItemTemplateID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup> CampaignItemTemplateSuppressionGroupList = new List<ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                CampaignItemTemplateSuppressionGroupList = ECN_Framework_DataLayer.Communicator.CampaignItemTemplateSuppressionGroup.GetByCampaignItemTemplateID(CampaignItemTemplateID);
                foreach (ECN_Framework_Entities.Communicator.CampaignItemTemplateSuppressionGroup citg in CampaignItemTemplateSuppressionGroupList)
                {
                    citg.Filters = ECN_Framework_DataLayer.Communicator.CampaignItemTemplateFilter.GetByCampaignItemTemplateIDAndGroupID(CampaignItemTemplateID, citg.GroupID.Value);
                }
                scope.Complete();
            }
            return CampaignItemTemplateSuppressionGroupList;
        }

        public static void DeleteByCampaignItemTemplateID(int CampaignItemTemplateID, KMPlatform.Entity.User user)
        {
            List<ECNError> errorList = new List<ECNError>();
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemTemplateSuppressionGroup.DeleteByCampaignItemTemplateID(CampaignItemTemplateID, user.UserID);
                scope.Complete();
            }
        }

        public static void Delete(int CampaignItemTemplateSuppressionGroupID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemTemplateSuppressionGroup.Delete(CampaignItemTemplateSuppressionGroupID, user.UserID);
                scope.Complete();
            }
        }
    }
}
