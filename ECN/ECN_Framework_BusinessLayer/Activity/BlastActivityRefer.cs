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
    public class BlastActivityRefer
    {
        public static ECN_Framework_Entities.Activity.BlastActivityRefer GetByReferID(int referID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityRefer.GetByReferID(referID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityRefer> GetByBlastID(int blastID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityRefer.GetByBlastID(blastID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityRefer> GetByEmailID(int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityRefer.GetByEmailID(emailID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityRefer> GetByBlastIDEmailID(int blastID, int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityRefer.GetByBlastIDEmailID(blastID, emailID);
        }
    }
}
