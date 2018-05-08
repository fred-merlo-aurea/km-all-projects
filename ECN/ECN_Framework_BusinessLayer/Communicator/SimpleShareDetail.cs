using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public partial class SimpleShareDetail
    {
        public static List<ECN_Framework_Entities.Communicator.SimpleShareDetail> GetByCampaignItemID(int CampaignItemID)
        {
            List<ECN_Framework_Entities.Communicator.SimpleShareDetail> retList = new List<ECN_Framework_Entities.Communicator.SimpleShareDetail>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.SimpleShareDetail.GetByCampaignItemID(CampaignItemID);
                scope.Complete();
            }
            return retList;
        }

        public static void DeleteFromCampaignItem(int SocialMediaAuthID, int CampaignItemID)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.SimpleShareDetail.DeleteFromCampaignItem(SocialMediaAuthID, CampaignItemID);
                scope.Complete();
            }

        }

        public static ECN_Framework_Entities.Communicator.SimpleShareDetail GetBySimpleShareDetailID(int SimpleShareDetailID)
        {
            ECN_Framework_Entities.Communicator.SimpleShareDetail ssd = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ssd = ECN_Framework_DataLayer.Communicator.SimpleShareDetail.GetBySimpleShareDetailID(SimpleShareDetailID);
                scope.Complete();
            }
            return ssd;
        }

        public static int Save(ECN_Framework_Entities.Communicator.SimpleShareDetail ssd)
        {
            int retID = -1;
            using(TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Communicator.SimpleShareDetail.Save(ssd);
                scope.Complete();
            }
            return retID;

        }
    }
}
