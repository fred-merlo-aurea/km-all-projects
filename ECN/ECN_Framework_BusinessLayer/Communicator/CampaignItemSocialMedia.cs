using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public partial class CampaignItemSocialMedia
    {
        public static List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia> GetByCampaignItemID(int CampaignItemID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.CampaignItemSocialMedia.GetByCampaignItemID(CampaignItemID);
                scope.Complete();
            }
            return retList;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItemSocialMedia GetByCampaignItemSocialMediaID(int CampaignItemSocialMediaID)
        {
            ECN_Framework_Entities.Communicator.CampaignItemSocialMedia retList = new ECN_Framework_Entities.Communicator.CampaignItemSocialMedia();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.CampaignItemSocialMedia.GetByCampaignItemSocialMediaID(CampaignItemSocialMediaID);
                scope.Complete();
            }
            return retList;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia> GetByStatus(ECN_Framework_Common.Objects.Communicator.Enums.SocialMediaStatusType status)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.CampaignItemSocialMedia.GetByStatus(status);
                scope.Complete();
            }
            return retList;
        }

        public static ECN_Framework_Entities.Communicator.CampaignItemSocialMedia GetFirstToSendByStatus(ECN_Framework_Common.Objects.Communicator.Enums.SocialMediaStatusType status)
        {
            try
            {
                return GetByStatus(status).Where(s => (s.SocialMediaID == 1 || s.SocialMediaID == 2 || s.SocialMediaID == 3) && s.SimpleShareDetailID != null).OrderBy(x => x.StatusDate).ToList().FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }          
        }

        public static void Delete(int CampaignItemID, string SimpleOrSubscriber)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemSocialMedia.Delete(CampaignItemID, SimpleOrSubscriber);
                scope.Complete();
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemSocialMedia cism)
        {
            int retID = -1;
            using(TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Communicator.CampaignItemSocialMedia.Save(cism);
                scope.Complete();
            }
            return retID;
        }

    }
}
