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
    public class BlastActivitySuppressed
    {
        public static ECN_Framework_Entities.Activity.BlastActivitySuppressed GetBySuppressID(int suppressID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivitySuppressed.GetBySuppressID(suppressID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivitySuppressed> GetByBlastID(int blastID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivitySuppressed.GetByBlastID(blastID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivitySuppressed> GetByEmailID(int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivitySuppressed.GetByEmailID(emailID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivitySuppressed> GetByBlastIDEmailID(int blastID, int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivitySuppressed.GetByBlastIDEmailID(blastID, emailID);
        }
    }
}
