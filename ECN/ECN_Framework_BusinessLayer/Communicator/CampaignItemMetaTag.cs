using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class CampaignItemMetaTag
    {
        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemMetaTag cimt)
        {
            int retID = -1;
            using(TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Communicator.CampaignItemMetaTag.Save(cimt);
                scope.Complete();
            }
            return retID;
        }

        public static void Delete_CampaignItemID(int CampaignItemID, int UserID)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemMetaTag.Delete_CampaignItemID(CampaignItemID, UserID);
                scope.Complete();
            }
        }

        public static void Delete_SocialMediaID_CampaignItemID(int SocialMediaID, int CampaignItemID, int UserID)
        {
            using(TransactionScope scope = new TransactionScope())
            {

                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemMetaTag> GetByCampaignItemID(int CampaignItemID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemMetaTag> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemMetaTag>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.CampaignItemMetaTag.GetByCampaignItemID(CampaignItemID);
                scope.Complete();
            }
            return retList;
        }
    }
}
