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
    public class BlastActivityResends
    {
        public static ECN_Framework_Entities.Activity.BlastActivityResends GetByResendID(int resendID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityResends.GetByResendID(resendID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityResends> GetByBlastID(int blastID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityResends.GetByBlastID(blastID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityResends> GetByEmailID(int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityResends.GetByEmailID(emailID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityResends> GetByBlastIDEmailID(int blastID, int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityResends.GetByBlastIDEmailID(blastID, emailID);
        }
    }
}
