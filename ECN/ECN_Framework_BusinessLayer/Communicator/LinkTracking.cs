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
    public class LinkTracking
    {
         public static List<ECN_Framework_Entities.Communicator.LinkTracking> GetAll()
         {
             List<ECN_Framework_Entities.Communicator.LinkTracking> linkTrackingList = new List<ECN_Framework_Entities.Communicator.LinkTracking>();
             using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
             {
                 linkTrackingList = ECN_Framework_DataLayer.Communicator.LinkTracking.GetAll();
                 scope.Complete();
             }
             return linkTrackingList;
         }

         public static List<ECN_Framework_Entities.Communicator.LinkTracking> GetByCampaignItemID(int campaignItemID)
         {
             List<ECN_Framework_Entities.Communicator.LinkTracking> linkTrackingList = new List<ECN_Framework_Entities.Communicator.LinkTracking>();
             using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
             {
                 linkTrackingList = ECN_Framework_DataLayer.Communicator.LinkTracking.GetByCampaignItemID(campaignItemID);
                 scope.Complete();
             }
             return linkTrackingList;
         }

         public static bool CreateLinkTrackingParams(int customerID, string domain, int LTID)
         {
             bool returnValue = false;
             using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
             {
                 returnValue = ECN_Framework_DataLayer.Communicator.LinkTracking.CreateLinkTrackingParams(customerID, domain, LTID);
                 scope.Complete();
             }
             return returnValue;
         }
    }
}
