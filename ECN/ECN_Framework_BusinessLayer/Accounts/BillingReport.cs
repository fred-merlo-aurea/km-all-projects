using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Accounts
{
    public class BillingReport
    {
        public static ECN_Framework_Entities.Accounts.BillingReport GetByBillingReportID(int BillingReportID)
        {
            
            ECN_Framework_Entities.Accounts.BillingReport retItem = new ECN_Framework_Entities.Accounts.BillingReport();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retItem = ECN_Framework_DataLayer.Accounts.BillingReport.GetByBillingReportID(BillingReportID);
                scope.Complete();
            }
            return retItem;
        }

        public static List<ECN_Framework_Entities.Accounts.BillingReport> GetALL()
        {
            List<ECN_Framework_Entities.Accounts.BillingReport> retList = new List<ECN_Framework_Entities.Accounts.BillingReport>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Accounts.BillingReport.GetAll();
                scope.Complete();
            }
            return retList;
        }

        public static int SaveBillingReport(ECN_Framework_Entities.Accounts.BillingReport br)
        {
            int retID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Accounts.BillingReport.Save(br);
                scope.Complete();
            }
            return retID;
        }

        public static void Update(int BillingReportID,string customerIDs, string reportName, bool includeFulfillment, bool includeMasterFile, bool isRecurring, string recurrenceType, DateTime startDate, DateTime endDate, double EmailBillingRate, double masterFileRate, double fulfillmentRate, string FromEmail, string toEmail, string FromName, string subject, bool isDeleted)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Accounts.BillingReport.Update(BillingReportID,customerIDs, reportName, includeFulfillment, includeMasterFile,startDate, endDate, isRecurring, recurrenceType, EmailBillingRate, masterFileRate, fulfillmentRate, FromEmail, toEmail, FromName, subject, isDeleted);
                scope.Complete();
            }
        }
    }
}
