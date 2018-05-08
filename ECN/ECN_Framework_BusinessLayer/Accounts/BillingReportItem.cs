using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Accounts
{
    public class BillingReportItem
    {
        public static List<ECN_Framework_Entities.Accounts.BillingReportItem> GetItemsByBillingReportID(int BillingReportID)
        {
            List<ECN_Framework_Entities.Accounts.BillingReportItem> retList = new List<ECN_Framework_Entities.Accounts.BillingReportItem>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Accounts.BillingReportItem.GetItemsByBillingReportID(BillingReportID);

                scope.Complete();
            }
            return retList;
        }

        public static List<ECN_Framework_Entities.Accounts.BillingReportItem> GetEmailUsageByCustomer(string CustomerIDs, DateTime startDate, DateTime endDate, string fieldsToInclude, string columnSQL)
        {
            List<ECN_Framework_Entities.Accounts.BillingReportItem> retList = new List<ECN_Framework_Entities.Accounts.BillingReportItem>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Accounts.BillingReportItem.GetEmailUsageReport(CustomerIDs, startDate, endDate, fieldsToInclude, columnSQL);
                scope.Complete();
            }

            return retList;
        }

        public static ECN_Framework_Entities.Accounts.BillingReportItem GetByBillingReportItemID(int BillingReportItemID)
        {
            ECN_Framework_Entities.Accounts.BillingReportItem retItem = new ECN_Framework_Entities.Accounts.BillingReportItem();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retItem = ECN_Framework_DataLayer.Accounts.BillingReportItem.GetByBillingReportItemID(BillingReportItemID);
                scope.Complete();
            }

            return retItem;
        }

        public static int Save(ECN_Framework_Entities.Accounts.BillingReportItem bri)
        {
            int retID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Accounts.BillingReportItem.Save(bri);
                scope.Complete();
            }
            return retID;
        }
    }
}
