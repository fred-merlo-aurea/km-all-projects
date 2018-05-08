using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Activity
{
    [Serializable]
    public class BlastActivitySends
    {
        public static ECN_Framework_Entities.Activity.BlastActivitySends GetBySendID(int sendID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivitySends.GetBySendID(sendID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivitySends> GetByBlastID(int blastID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivitySends.GetByBlastID(blastID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivitySends> GetByEmailID(int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivitySends.GetByEmailID(emailID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivitySends> GetByBlastIDEmailID(int blastID, int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivitySends.GetByBlastIDEmailID(blastID, emailID);
        }

        public static void update_SMTPMessage(string xmldata)
        {
            ECN_Framework_DataLayer.Activity.BlastActivitySends.update_SMTPMessage(xmldata);
        }

        public static void doBulkUpdate_SMTPMessage()
        {
            ECN_Framework_DataLayer.Activity.BlastActivitySends.doBulkUpdate_SMTPMessage();
        }

        public static void update_BounceData(string xmldata)
        {
            ECN_Framework_DataLayer.Activity.BlastActivitySends.update_BounceData(xmldata);
        }

        public static bool ActivityByBlastIDsEmailID(string blastIDs, int emailID)
        {
            bool exists = false;
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Activity.BlastActivitySends.ActivityByBlastIDsEmailID(blastIDs, emailID);
                scope.Complete();
            }
            return exists;
        }
    }
}
