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
    public class BlastActivityOpensReport
    {
        private static List<ECN_Framework_Entities.Activity.BlastActivityOpens> GetBlastOpensList(int blast, bool isGroup)
        {
            if (!isGroup)
            {
                return ECN_Framework_BusinessLayer.Activity.BlastActivityOpens.GetByBlastID(blast);
            }
            else
            {
                return ECN_Framework_BusinessLayer.Activity.BlastActivityOpens.GetByCampaignItemID(blast);
            }

        }

        public static List<ECN_Framework_Entities.Activity.Report.BlastActivityOpensReport> Get(int blast, bool isGroup)
        {
            List<ECN_Framework_Entities.Activity.EmailClients> eclist = ECN_Framework_BusinessLayer.Activity.EmailClients.Get();
            List<ECN_Framework_Entities.Activity.Platforms> plist = ECN_Framework_BusinessLayer.Activity.Platforms.Get();
            List<ECN_Framework_Entities.Activity.BlastActivityOpens> openslist = GetBlastOpensList(blast, isGroup);


            return ECN_Framework_DataLayer.Activity.Report.BlastActivityOpensReport.Get(openslist, eclist, plist);
        }
    }
}
