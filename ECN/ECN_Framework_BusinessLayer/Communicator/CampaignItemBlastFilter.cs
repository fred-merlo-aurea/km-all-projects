using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    public class CampaignItemBlastFilter
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.CampaignItemBlastFilter;

        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> GetByCampaignItemBlastID(int CampaignItemBlastID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.CampaignItemBlastFilter.GetByCampaignItemBlastID(CampaignItemBlastID);
                scope.Complete();
            }
            return retList;

        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> GetByCampaignItemBlastID_UseAmbientTransaction(int CampaignItemBlastID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>();
            using (TransactionScope scope = new TransactionScope())
            {
                retList = ECN_Framework_DataLayer.Communicator.CampaignItemBlastFilter.GetByCampaignItemBlastID(CampaignItemBlastID);
                scope.Complete();
            }
            return retList;

        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> GetByCampaignItemSuppressionID(int CampaignItemSuppressionID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.CampaignItemBlastFilter.GetByCampaignItemSuppressionID(CampaignItemSuppressionID);
                scope.Complete();
            }
            return retList;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> GetByCampaignItemSuppressionID_UseAmbientTransaction(int CampaignItemSuppressionID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>();
            using (TransactionScope scope = new TransactionScope())
            {
                retList = ECN_Framework_DataLayer.Communicator.CampaignItemBlastFilter.GetByCampaignItemSuppressionID(CampaignItemSuppressionID);
                scope.Complete();
            }
            return retList;
        }

        public static  List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> GetByCampaignItemTestBlastID(int CampaignItemTestBlastID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> retList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.CampaignItemBlastFilter.GetByCampaignItemTestBlastID(CampaignItemTestBlastID);
                scope.Complete();
            }
            return retList;
        }

        
        public static bool SelectByFilterIDCanDelete(int filterID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.CampaignItemBlastFilter.SelectByFilterIDCanDelete(filterID);
                scope.Complete();
            }
            return exists;
        }

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf)
        {
            int retID = -1;
            using(TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Communicator.CampaignItemBlastFilter.Save(cibf);
                scope.Complete();
            }
            return retID;
        }

        public static void DeleteByCampaignItemBlastID(int CampaignItemBlastID)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemBlastFilter.DeleteByCampaignItemBlastID(CampaignItemBlastID);
                scope.Complete();
            }
        }

        public static void DeleteByCampaignItemSuppressionID(int CampaignItemSuppressionID)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemBlastFilter.DeleteByCampaignItemSuppressionID(CampaignItemSuppressionID);
                scope.Complete();
            }
        }

    }
}
