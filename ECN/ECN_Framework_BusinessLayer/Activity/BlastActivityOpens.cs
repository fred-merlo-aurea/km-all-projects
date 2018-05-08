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
    public class BlastActivityOpens
    {
        public static ECN_Framework_Entities.Activity.BlastActivityOpens GetByOpenID(int openID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityOpens.GetByOpenID(openID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityOpens> GetByBlastID(int blastID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityOpens.GetByBlastID(blastID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityOpens> GetByEmailID(int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityOpens.GetByEmailID(emailID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityOpens> GetByCampaignItemID(int campaignItemID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityOpens.GetByCampaignItemID(campaignItemID);
        }

        public static List<ECN_Framework_Entities.Activity.BlastActivityOpens> GetByBlastIDEmailID(int blastID, int emailID)
        {
            return ECN_Framework_DataLayer.Activity.BlastActivityOpens.GetByBlastIDEmailID(blastID, emailID);
        }
    }
}
