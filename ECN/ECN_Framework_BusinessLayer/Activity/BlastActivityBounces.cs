using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Activity
{
    [Serializable]
    public class BlastActivityBounces
    {
        public static ECN_Framework_Entities.Activity.BlastActivityBounces GetByBounceID(int bounceID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityBounces.GetByBounceID(bounceID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityBounces> GetByBlastID(int blastID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityBounces.GetByBlastID(blastID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityBounces> GetByEmailID(int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityBounces.GetByEmailID(emailID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityBounces> GetByBlastIDEmailID(int blastID, int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityBounces.GetByBlastIDEmailID(blastID, emailID);
        }

        public static DataTable BlastReportWithUDF(int blastID, string udfName, string udfData)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.BlastActivityBounces.BlastReportWithUDF(blastID, udfName, udfData);
                scope.Complete();
            }

            return dt;
        }

        public static DataTable BlastReport(int blastID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.BlastActivityBounces.BlastReport(blastID);
                scope.Complete();
            }

            return dt;
        }

        public static DataTable BlastReportByCampaignItemID(int campaignItemID)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.BlastActivityBounces.BlastReportByCampaignItemID(campaignItemID);
                scope.Complete();
            }

            return dt;
        }

        public static DataTable GetByDateRangeForCustomers(string startDate, string endDate, string customerIDs, string stringToFind)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityBounces.GetByDateRangeForCustomers(startDate, endDate, customerIDs, stringToFind);
        }
    }
}
