using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class BlastOpensByTime
    {
        public static List<ECN_Framework_Entities.Activity.Report.BlastOpensByTime> GetByBlastID(int blastID)
        {
            return ECN_Framework_DataLayer.Activity.Report.BlastOpensByTime.GetByBlastID(blastID);
        }

        public static List<ECN_Framework_Entities.Activity.Report.BlastOpensByTime> GetByCampaignItemID(int campaingItemID)
        {
            return ECN_Framework_DataLayer.Activity.Report.BlastOpensByTime.GetByCampaignItemID(campaingItemID);
        }
    }
}
