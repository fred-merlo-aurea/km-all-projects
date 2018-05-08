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
    public class BlastActivityClicks
    {
        public static ECN_Framework_Entities.Activity.BlastActivityClicks GetByClickID(int clickID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityClicks.GetByClickID(clickID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityClicks> GetByBlastID(int blastID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityClicks.GetByBlastID(blastID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityClicks> GetByEmailID(int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityClicks.GetByEmailID(emailID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityClicks> GetByBlastIDEmailID(int blastID, int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityClicks.GetByBlastIDEmailID(blastID, emailID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityClicks> GetByBlastLinkID(int blastLinkID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityClicks.GetByBlastLinkID(blastLinkID);
        }
        
        public static int GetUniqueByURL(string URL, int campaignItemID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityClicks.GetUniqueByURL(URL, campaignItemID);
        }
        public static DataTable GetByBlastID(int customerID, int blastID, KMPlatform.Entity.User user)
        {
            DataTable dt = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.BlastActivityClicks.GetByBlastID(customerID, blastID);
                scope.Complete();
            }

            return dt;
        }

        public static int CountByBlastID(int blastID)
        {
            int count = 0;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                count = ECN_Framework_DataLayer.Activity.BlastActivityClicks.CountByBlastID(blastID);
                scope.Complete();
            }

            return count;
        }
    }

}
