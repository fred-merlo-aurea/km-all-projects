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
    public class BlastActivityConversion
    {
        public static DataTable GetRevenueData(int customerID, int blastID, string type)
        {
            DataTable dt = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.BlastActivityConversion.GetRevenueData(customerID, blastID, type);
                scope.Complete();
            }

            return dt;
        }

        public static int GetCount(int blastID, int customerID, string url, int length)
        {
            int count = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                count = ECN_Framework_DataLayer.Activity.BlastActivityConversion.GetCount(blastID, customerID, url, length, false);
                scope.Complete();
            }
            return count;
        }

        public static int GetDistinctCount(int blastID, int customerID, string url, int length)
        {
            int count = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                count = ECN_Framework_DataLayer.Activity.BlastActivityConversion.GetCount(blastID, customerID, url, length, true);
                scope.Complete();
            }
            return count;
        }
    }
}
