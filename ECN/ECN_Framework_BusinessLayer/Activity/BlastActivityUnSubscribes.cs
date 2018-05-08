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
    public class BlastActivityUnSubscribes
    {
        public static ECN_Framework_Entities.Activity.BlastActivityUnSubscribes GetByUnsubscribeID(int unsubscribeID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityUnSubscribes.GetByUnsubscribeID(unsubscribeID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes> GetByBlastID(int blastID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityUnSubscribes.GetByBlastID(blastID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes> GetByEmailID(int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityUnSubscribes.GetByEmailID(emailID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityUnSubscribes> GetByBlastIDEmailID(int blastID, int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityUnSubscribes.GetByBlastIDEmailID(blastID, emailID);
        }

        public static DataTable GetByDateRangeForCustomers(string startDate, string endDate, string customerIDs, string unsubscribeCode)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityUnSubscribes.GetByDateRangeForCustomers(startDate, endDate, customerIDs, unsubscribeCode);
        }
    }
}
