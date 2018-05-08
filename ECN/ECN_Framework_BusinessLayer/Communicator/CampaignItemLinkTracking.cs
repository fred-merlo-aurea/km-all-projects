using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using System.Configuration;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class CampaignItemLinkTracking
    {
        public static DataTable GetParamInfo(int blastID, int LTID)
        {
            DataTable dtCampaignItemLinkTracking = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtCampaignItemLinkTracking = ECN_Framework_DataLayer.Communicator.CampaignItemLinkTracking.GetParamInfo(blastID, LTID);
                scope.Complete();
            }
            return dtCampaignItemLinkTracking;
        }

        public static int Save(ECN_Framework_Entities.Communicator.CampaignItemLinkTracking campaignItemLinkTracking, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                campaignItemLinkTracking.CILTID = ECN_Framework_DataLayer.Communicator.CampaignItemLinkTracking.Save(campaignItemLinkTracking);
                scope.Complete();
            }
            return campaignItemLinkTracking.CILTID;
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking> GetByCampaignItemID(int campaignItemID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking> campaignItemLinkTrackingList = new List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                campaignItemLinkTrackingList = ECN_Framework_DataLayer.Communicator.CampaignItemLinkTracking.GetByCampaignItemID(campaignItemID);
                scope.Complete();
            }
            return campaignItemLinkTrackingList;
        }

        public static void DeleteByCampaignItemID(int campaignItemID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemLinkTracking.DeleteByCampaignItemID(campaignItemID, user.CustomerID, user.UserID);
                scope.Complete();
            }
        }

        public static void DeleteByLTID(int campaignItemID, int LTID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.CampaignItemLinkTracking.DeleteByLTID(campaignItemID, LTID, user.UserID);
                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking> GetByLinkTrackingParamOptionID(int LTPOID)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking> campaignItemLinkTrackingList = new List<ECN_Framework_Entities.Communicator.CampaignItemLinkTracking>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                campaignItemLinkTrackingList = ECN_Framework_DataLayer.Communicator.CampaignItemLinkTracking.GetByLinkTrackingParamOptionID(LTPOID);
                scope.Complete();
            }
            return campaignItemLinkTrackingList;
        }
    }
}
